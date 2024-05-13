using UnityEngine;

public class Bag : MonoBehaviour
{
    private Resource _takenResource;

    public void Add(Resource resource)
    {
        if (_takenResource != null)
            return;

        resource.transform.position = transform.position;
        resource.transform.SetParent(transform);
        _takenResource = resource;
    }

    public Resource GetResource()
    {
        Resource resource = _takenResource;
        _takenResource = null;

        return resource;
    }
}