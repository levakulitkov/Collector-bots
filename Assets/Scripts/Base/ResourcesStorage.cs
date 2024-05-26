using System;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    public event Action<int> CountChanged;

    private int _resourcesCount;
    private bool _accumulateResourcesForNewBase = false;

    public event Action<Resource> ResourcesForBaseAccumulated;

    public event Action<Resource> ResourcesForBotAccumulated;

    public void AddResources(Resource resource)
    {
        IncreaseResources—ount(resource.Amount);

        if (_accumulateResourcesForNewBase && _resourcesCount >= Constants.BasePrice
            && TryGetResources(Constants.BasePrice, out Resource? resourceForBase))
        {
            _accumulateResourcesForNewBase = false;
            ResourcesForBaseAccumulated?.Invoke(resourceForBase.Value);
        }
        else if (!_accumulateResourcesForNewBase && _resourcesCount >= Constants.BotPrice
            && TryGetResources(Constants.BotPrice, out Resource? resourceForBot))
        {
            ResourcesForBotAccumulated?.Invoke(resourceForBot.Value);
        }
    }

    public void StartAccumulatingResourcesForNewBase()
        => _accumulateResourcesForNewBase = true;

    public bool TryGetResources(int amount, out Resource? resources)
    {
        var resource = new Resource(amount);

        if (_resourcesCount < resource.Amount)
        {
            resources = null;
            return false;
        }

        DecreaseResources—ount(resource.Amount);
        resources = resource;
        return true;
    }

    private void IncreaseResources—ount(int value)
    {
        _resourcesCount += value;
        CountChanged?.Invoke(_resourcesCount);
    }

    private void DecreaseResources—ount(int value)
        => IncreaseResources—ount(-value);
}