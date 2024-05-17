using UnityEngine;

public class ResourcesDistributionModule : MonoBehaviour
{
    public int BotPrice => _newBotPrice;
    public int BasePrice => _newBasePrice;

    [SerializeField] private ResourcesStorage _resourcesStorage;
    [SerializeField] private int _newBotPrice = 3;
    [SerializeField] private int _newBasePrice = 5;
    [SerializeField] private BotConstructionModule _botConstructionModule;
    [SerializeField] private BaseConstructionModule _baseConstructionModule;

    private bool _saveResourcesForNewBase = false;

    private void OnEnable()
    {
        _resourcesStorage.CountChanged += OnResourceCountChanged;
    }

    private void OnDisable()
    {
        _resourcesStorage.CountChanged -= OnResourceCountChanged;
    }

    private void OnResourceCountChanged(int value)
    {
        if (_saveResourcesForNewBase && value >= _newBasePrice && _resourcesStorage.TryGiveResourcesForBaseConstruction(this))
        {
            //ConstructBase();
        }
        else if (!_saveResourcesForNewBase && value >= _newBotPrice 
            && !_botConstructionModule.InProgress && _resourcesStorage.TryGiveResourcesForBotConstruction(this)) 
        {
            _botConstructionModule.TryCreateBot();
        }
    }
}
