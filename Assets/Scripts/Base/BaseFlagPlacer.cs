using UnityEngine;

public class BaseFlagPlacer : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private BaseFlag _template;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxRayDistance = 100f;
    [SerializeField] private LayerMask _surfaceLayer;

    private BaseFlag _flag;

    private void OnEnable()
    {
        _flag = Instantiate(_template, _container);
    }

    private void OnDisable()
    {
        if (_flag != null)
            Destroy(_flag.gameObject);
    }

    private void Update()
    {
        UpdateFlagPosition();
    }

    public bool TryPlaceFlag(BotsBase hitBotsBase)
    {
        if (!_flag.CanPlace)
            return false;

        hitBotsBase.AddNewBaseFlag(_flag);
        _flag.RemoveColorBox();

        _flag = null;
        return true;
    }

    private void UpdateFlagPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxRayDistance, _surfaceLayer))
            _flag.transform.position = hitInfo.point;
    }
}