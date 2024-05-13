using UnityEngine;

[RequireComponent(typeof(Mover))]
public class CollectorBot : MonoBehaviour
{
    private Transform _basePosition;
    private Transform _target;
    private Mover _mover;
    private Bag _bag;

    public bool HasTarget => _target != null;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _bag = GetComponentInChildren<Bag>();
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
            _bag.Add(freeResource);
            _target = _basePosition;
        }
        else if (_target == _basePosition)
        {
            var botsBase = other.GetComponentInParent<BotsBase>();
            
            if (botsBase != null)
            {
                _target = null;
                
                botsBase.AddResource(this, _bag.GetResource());
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