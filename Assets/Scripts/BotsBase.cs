using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ScannerModule))]
public class BotsBase : MonoBehaviour
{
    [SerializeField] private CollectorBot[] _startBots;

    private ScannerModule _scanner;
    private List<CollectorBot> _bots;
    private List<Resource> _freeResources;
    private List<Resource> _busyResources;
    private Coroutine _findingCoroutine;

    public event Action<int> ResourceAdded;

    private void Awake()
    {
        _scanner = GetComponent<ScannerModule>();
        _bots = new List<CollectorBot>(_startBots);
        _busyResources = new List<Resource>();

        foreach (CollectorBot bot in _bots)
            bot.AssignBase(transform);
    }

    private void OnEnable()
    {
        _scanner.ScanningCompleted += OnGetScanningResults;
    }

    private void OnDisable()
    {
        _scanner.ScanningCompleted -= OnGetScanningResults;
    }

    private void Start()
    {
        StartFindingResources();
    }

    public void AddResource(CollectorBot bot, Resource resource)
    {
        ResourceAdded?.Invoke(1);

        _busyResources.Remove(resource);
        
        Destroy(resource.gameObject);
        
        TryAssignTargetResourceForBot(bot);
    }

    private void StartFindingResources()
    {
        _findingCoroutine ??= StartCoroutine(_scanner.FindFreeResources(_busyResources.ToArray()));
    }

    private void OnGetScanningResults(Resource[] detectedResources)
    {
        _freeResources = detectedResources.Except(_busyResources).ToList();

        foreach (CollectorBot bot in _bots.Where(bot => !bot.HasTarget))
            if (!TryAssignTargetResourceForBot(bot))
                break;

        _findingCoroutine = null;
    }

    private bool TryAssignTargetResourceForBot(CollectorBot bot)
    {
        if (_freeResources.Count == 0)
        {
            StartFindingResources();
            return false;
        }

        Resource resource = _freeResources.Last();
        _freeResources.Remove(resource);
        _busyResources.Add(resource);

        bot.SetTarget(resource.transform);

        return true;
    }
}