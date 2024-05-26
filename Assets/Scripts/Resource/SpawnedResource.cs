using UnityEngine;

public class SpawnedResource : MonoBehaviour
{
    [SerializeField] private int _amount = 1;

    private Resource _resource;

    private void Start()
    {
        _resource = new Resource(_amount);
    }

    public Resource Extract()
    {
        Destroy(gameObject);
        return _resource;
    }
}