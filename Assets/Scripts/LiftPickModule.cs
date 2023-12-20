using UnityEngine;

public class LiftPickModule : MonoBehaviour
{
    private Resource _takenResource;

    public void PickUp(Resource resource)
    {
        Vector3 position = transform.position;
        position.y = position.y + transform.localScale.y + resource.transform.localScale.y / 2;

        resource.transform.position = position;
        resource.transform.SetParent(transform);
        _takenResource = resource;
    }

    public void GiveResource(BotsBase botsBase)
    {
        if (_takenResource is null)
            return;

        botsBase.AddResource(_takenResource);
        _takenResource = null;
    }
}