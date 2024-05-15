using UnityEngine;

public class ResourceCarrier : MonoBehaviour
{
    private Resource _resource;

    public void Put(Resource resource)
    {
        if (_resource != null)
            return;

        resource.transform.position = transform.position;
        resource.transform.SetParent(transform);
        _resource = resource;
    }

    public Resource Take()
    {
        Resource resource = _resource;
        _resource = null;

        return resource;
    }
}