using UnityEngine;

[RequireComponent(typeof(Mover))]
public class CollectorBot : MonoBehaviour
{
    private Transform _basePosition;
    private Transform _target;
    private Mover _mover;
    private ResourceCarrier _resourceCarrier;

    public bool HasTarget => _target != null;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _resourceCarrier = GetComponentInChildren<ResourceCarrier>();
    }

    private void FixedUpdate()
    {
        if (HasTarget)
            _mover.Move();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource freeResource)
            && freeResource.transform.Equals(_target))
        {
            _resourceCarrier.Put(freeResource);
            _target = _basePosition;
        }
        else if (_target == _basePosition && other.TryGetComponent(out BotsBase botsBase))
        {
            _target = null;

            botsBase.AddResource(this, _resourceCarrier.Take());
        }
    }

    private void Update()
    {
        if (HasTarget)
        {
            Vector3 targetPosition = _target.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }
    }

    public void AssignBase(Transform basePosition)
    {
        _basePosition = basePosition;
    }

    public void SetTarget(Transform resource)
    {
        _target = resource;
    }
}