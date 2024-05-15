using TMPro;
using UnityEngine;

public class ResourceCountView : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _resourceCounter.Changed += OnResourceCountChanged;
    }

    private void OnDisable()
    {
        _resourceCounter.Changed -= OnResourceCountChanged;
    }

    private void OnResourceCountChanged(int score)
    {
        _text.text = score.ToString();
    }
}
