using UnityEngine;

[RequireComponent(typeof(Mover), typeof(LiftPickModule))]
public class CollectorBot : MonoBehaviour
{
    private Transform _basePosition;
    private Transform _target;
    private Mover _mover;
    private LiftPickModule _liftPickModule;

    public bool HasTarget => _target is not null;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _liftPickModule = GetComponent<LiftPickModule>();
    }

    private void FixedUpdate()
    {
        if (HasTarget)
            _mover.Move();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource)
            && resource.transform.Equals(_target))
        {
            _liftPickModule.PickUp(resource);
            _target = _basePosition;
        }
        else if (_target == _basePosition)
        {
            var botsBase = other.GetComponentInParent<BotsBase>();
            if (botsBase is not null)
            {
                _liftPickModule.GiveResource(botsBase);
                _target = null;
            }
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

    public void GetTarget(Transform resource)
    {
        _target = resource;
    }
}