using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ScannerModule))]
public class BotsBase : MonoBehaviour
{
    [SerializeField] private CollectorBot[] _startBots;
    [SerializeField] private BotCreationModule _botCreationModule;
    [SerializeField] private ResourcesStorage _resourcesStorage;

    private ScannerModule _scanner;
    private List<CollectorBot> _bots;
    private List<SpawnedResource> _detectedResources;
    private List<SpawnedResource> _busyResources;
    private Coroutine _findingCoroutine;
    private BaseFlag _newBaseFlag;
    private bool _needToAssignBotForNewBaseCreation;
    private Resource? _resourceForNewBaseCreation;

    private void Awake()
    {
        _scanner = GetComponent<ScannerModule>();
    }

    private void OnEnable()
    {
        _busyResources = new List<SpawnedResource>();
        _bots = new List<CollectorBot>();
        _detectedResources = new List<SpawnedResource>();

        _scanner.ScanningCompleted += OnScanningCompleted;
        _resourcesStorage.ResourcesForBotAccumulated += OnAccumulatedResourcesForBot;
        _resourcesStorage.ResourcesForBaseAccumulated += OnAccumulatedResourcesForBase;
    }

    private void OnDisable()
    {
        _scanner.ScanningCompleted -= OnScanningCompleted;
        _resourcesStorage.ResourcesForBotAccumulated -= OnAccumulatedResourcesForBot;
        _resourcesStorage.ResourcesForBaseAccumulated -= OnAccumulatedResourcesForBase;
    }

    private void Start()
    {
        foreach (CollectorBot bot in _startBots)
            AddBot(bot);
    }

    public void AddBot(CollectorBot bot)
    {
        _bots.Add(bot);

        bot.AssignBase(this);

        TryAssignTargetForBot(bot);
    }

    public void RemoveBot(CollectorBot bot)
    {
        _bots.Remove(bot);
    }

    public void AddResource(Resource resource)
    {
        _resourcesStorage.AddResources(resource);
    }

    public void AddNewBaseFlag(BaseFlag newBaseFlag)
    {
        if (_newBaseFlag == null)
        {
            _newBaseFlag = newBaseFlag;

            _resourcesStorage.StartAccumulatingResourcesForNewBase();

            return;
        }

        BaseFlag oldFlagForDestroy = _newBaseFlag;
        _newBaseFlag = newBaseFlag;

        CollectorBot botForNewBase = _bots.Find(bot =>
            bot.Target == oldFlagForDestroy.transform);

        if (botForNewBase != null)
            botForNewBase.SetTarget(newBaseFlag.transform);

        Destroy(oldFlagForDestroy.gameObject);
    }

    public (Resource, Transform) GetResourcesAndPositionForNewBase()
    {
        Resource resource = _resourceForNewBaseCreation.Value;

        _resourceForNewBaseCreation = null;

        return (resource, _newBaseFlag.transform);
    }

    private void StartFindingResources()
    {
        _findingCoroutine ??= StartCoroutine(_scanner.FindFreeResources(_busyResources.ToArray()));
    }

    public bool TryAssignTargetForBot(CollectorBot bot)
    {
        if (!_needToAssignBotForNewBaseCreation)
            return TryAssignTargetResourceForBot(bot);

        _needToAssignBotForNewBaseCreation = false;

        bot.SetTarget(transform);
        return true;
    }

    private void OnScanningCompleted(SpawnedResource[] detectedResources)
    {
        _busyResources = _busyResources.Where(resource => resource != null).ToList();
        _detectedResources = detectedResources.Except(_busyResources).ToList();

        _findingCoroutine = null;
    }

    private bool TryAssignTargetResourceForBot(CollectorBot bot)
    {
        List<SpawnedResource> freeResources = _detectedResources
            .Where(resource => resource != null).ToList();

        if (freeResources.Count == 0)
        {
            StartFindingResources();
            return false;
        }

        SpawnedResource resource = freeResources.First();
        _detectedResources.Remove(resource);
        _busyResources.Add(resource);

        bot.SetTarget(resource.transform);

        return true;
    }

    private void OnAccumulatedResourcesForBot(Resource resource)
    {
        if (!_botCreationModule.InProgress)
            _botCreationModule.TryStartCreation(resource);
    }

    private void OnAccumulatedResourcesForBase(Resource resource)
    {
        _resourceForNewBaseCreation = resource;
        _needToAssignBotForNewBaseCreation = true;
    }
}