using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ResourceCarrier : MonoBehaviour
{
    [SerializeField] private Material _invisibleMaterial;
    [SerializeField] private Material _resourceMaterial;
    [SerializeField] private Material _baseResourceMaterial;

    private Renderer _renderer;
    private Resource? _resource;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public bool TryPut(Resource resource, bool forBase = false)
    {
        if (_resource != null)
            return false;

        if (forBase)
            _renderer.material = _baseResourceMaterial;
        else
            _renderer.material = _resourceMaterial;

        _resource = resource;
        return true;
    }

    public bool TryTake(out Resource? resource)
    {
        if (_resource == null)
        {
            resource = null;
            return false;
        }

        _renderer.material = _invisibleMaterial;
        resource = _resource;
        _resource = null;

        return resource != null;
    }
}