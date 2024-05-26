using TMPro;
using UnityEngine;

public class ResourcesCountView : MonoBehaviour
{
    [SerializeField] private ResourcesStorage _resourcesStorage;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _resourcesStorage.CountChanged += OnResourcesCountChanged;
    }

    private void OnDisable()
    {
        _resourcesStorage.CountChanged -= OnResourcesCountChanged;
    }

    private void OnResourcesCountChanged(int score)
    {
        _text.text = score.ToString();
    }
}