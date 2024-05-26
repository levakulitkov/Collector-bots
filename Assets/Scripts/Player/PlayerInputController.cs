using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private BaseFlagPlacer _baseFlagPlacer;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxRayDistance = 100f;
    [SerializeField] private LayerMask _baseLayer;

    private PlayerInput _playerInput;
    private BotsBase _hitBotsBase;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.Building.performed += OnBuilding;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnBuilding(InputAction.CallbackContext callbackContext)
    {
        if (_baseFlagPlacer.enabled && _baseFlagPlacer.TryPlaceFlag(_hitBotsBase))
            _baseFlagPlacer.enabled = false;
        else if (HitBase())
            _baseFlagPlacer.enabled = true;
    }

    private bool HitBase()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxRayDistance, _baseLayer)
            && hitInfo.collider.TryGetComponent(out BotsBase hitBotsBase))
        {
            _hitBotsBase = hitBotsBase;
            return true;
        }

        return false;
    }
}