using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ScannerModule))]
public class BotsBase : MonoBehaviour
{
    [SerializeField] private CollectorBot[] _startBots;

    private ScannerModule _scanner;
    private List<CollectorBot> _bots;
    private HashSet<Resource> _detectedResources;
    private int _resourceCount;

    private void Awake()
    {
        _scanner = GetComponent<ScannerModule>();
        _bots = new List<CollectorBot>(_startBots);
        _detectedResources = new HashSet<Resource>();

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

    private void Update()
    {
        AssignTargetResourcesForBots();
    }

    public void AddResource(Resource resource)
    {
        _resourceCount++;
        Destroy(resource.gameObject);
        Debug.Log(_resourceCount);
    }

    private void OnGetScanningResults(Resource[] detectedResources)
    {
        foreach (Resource resource in detectedResources)
            _detectedResources.Add(resource);
    }

    private void AssignTargetResourcesForBots()
    {
        Resource freeResource = _detectedResources.FirstOrDefault(resource => !resource.IsBusy);
        if (freeResource is not null)
        {
            CollectorBot freeBot = _bots.FirstOrDefault(bot => !bot.HasTarget);
            if (freeBot is not null)
            {
                freeResource.DoBusy();
                freeBot.GetTarget(freeResource.transform);
            }
        }
    }
}