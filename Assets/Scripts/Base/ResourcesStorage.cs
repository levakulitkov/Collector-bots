using System;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    private int _resourcesCount;
    private bool _accumulateResourcesForNewBase;

    public event Action<int> CountChanged;
    public event Action<Resource> ResourcesForBaseAccumulated;
    public event Action<Resource> ResourcesForBotAccumulated;

    public void AddResources(Resource resource)
    {
        IncreaseResourcesÑount(resource.Amount);

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

        DecreaseResourcesÑount(resource.Amount);
        resources = resource;
        return true;
    }

    private void IncreaseResourcesÑount(int value)
    {
        _resourcesCount += value;
        CountChanged?.Invoke(_resourcesCount);
    }

    private void DecreaseResourcesÑount(int value)
    {
        _resourcesCount -= value;
        CountChanged?.Invoke(_resourcesCount);
    }
}