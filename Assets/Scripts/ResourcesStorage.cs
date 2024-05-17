using System;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    public event Action<int> CountChanged;

    [SerializeField] private BotsBase _botsBase;

    private int _resourcesCount;

    private void OnEnable()
    {
        _botsBase.ResourceAdded += IncreaseResources—ount;
    }

    private void OnDisable()
    {
        _botsBase.ResourceAdded -= IncreaseResources—ount;
    }

    public bool TryGiveResourcesForBotConstruction(ResourcesDistributionModule constructionModule)
    {
        if (_resourcesCount < constructionModule.BotPrice)
            return false;

        DecreaseResources—ount(constructionModule.BotPrice);
        return true;
    }

    public bool TryGiveResourcesForBaseConstruction(ResourcesDistributionModule constructionModule)
    {
        if (_resourcesCount < constructionModule.BasePrice)
            return false;

        DecreaseResources—ount(constructionModule.BasePrice);
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
