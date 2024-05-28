using UnityEngine;

public class BaseFlag : MonoBehaviour
{
    [SerializeField] private Color _goodPlaceColor;
    [SerializeField] private Color _badPlaceColor;
    [SerializeField] private Renderer _colorBox;
    [SerializeField] private LayerMask _layerMask;

    private readonly Collider[] _results = new Collider[1];

    public bool CanPlace { get; private set; }

    private void Start()
    {
        SetBadPlaceColor();
    }

    private void Update()
    {
        UpdatePlacementColor();
    }

    public void RemoveColorIndication()
    {
        if (_colorBox.enabled)
            _colorBox.enabled = false;
    }

    private void UpdatePlacementColor()
    {
        int collisions = Physics.OverlapBoxNonAlloc(_colorBox.transform.position,
            _colorBox.transform.localScale * 0.5f, _results, Quaternion.identity, _layerMask);
        if (collisions == 0 && !CanPlace)
        {
            CanPlace = true;
            SetGoodPlaceColor();
        }
        else if (collisions > 0 && CanPlace)
        {
            CanPlace = false;
            SetBadPlaceColor();
        }
    }

    private void SetBadPlaceColor()
    {
        _colorBox.material.color = _badPlaceColor;
    }

    private void SetGoodPlaceColor()
    {
        _colorBox.material.color = _goodPlaceColor;
    }
}