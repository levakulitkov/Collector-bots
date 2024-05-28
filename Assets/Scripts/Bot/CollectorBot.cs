using UnityEngine;

[RequireComponent(typeof(Mover), typeof(BaseCreationModule))]
public class CollectorBot : MonoBehaviour
{
    [SerializeField] private ResourceCarrier _resourceCarrier;

    private Mover _mover;
    private BaseCreationModule _baseCreationModule;
    private BotsBase _base;
    private Transform _target;
    private bool _baseCreationState;

    public Transform Target => _target;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _baseCreationModule = GetComponent<BaseCreationModule>();
    }

    private void OnEnable()
    {
        _baseCreationModule.Created += OnCreated;
    }

    private void OnDisable()
    {
        _baseCreationModule.Created -= OnCreated;
    }

    private void FixedUpdate()
    {
        if (!_baseCreationState)
        {
            if (_target != null)
                _mover.Move();
            else
                _base.TryAssignTargetForBot(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollider(other);
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 targetPosition = _target.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }
    }

    public void AssignBase(BotsBase botsBase)
    {
        _base = botsBase;
    }

    public void SetTarget(Transform target)
    {
        _target = target;

        Collider[] hitColliders = new Collider[1];
        if (Physics.OverlapCapsuleNonAlloc(transform.position, transform.position,
            transform.localScale.z, hitColliders, 1 << _target.gameObject.layer) > 0)
            CheckCollider(hitColliders[0]);
    }

    private void CheckCollider(Collider collider)
    {
        if (collider.TryGetComponent(out SpawnedResource spawnedResource)
            && _target == spawnedResource.transform)
        {
            InteractWithResource(spawnedResource);
        }
        else if (collider.TryGetComponent(out BotsBase botsBase)
           && _base == botsBase && _target == _base.transform)
        {
            InteractWithBase(botsBase);
        }
        else if (collider.TryGetComponent(out BaseFlag baseFlag)
            && _target == baseFlag.transform)
        {
            InteractWithBaseFlag(baseFlag);
        }
    }

    private void InteractWithResource(SpawnedResource spawnedResource)
    {
        Resource resource = spawnedResource.Extract();

        if (_resourceCarrier.TryPut(resource))
            SetTarget(_base.transform);
    }

    private void InteractWithBase(BotsBase botsBase)
    {
        if (_resourceCarrier.TryTake(out Resource? takenResource))
        {
            _target = null;
            _mover.Stop();

            botsBase.AddResource(takenResource.Value);
        }
        else
        {
            (Resource resourceForBase, Transform baseFlag) =
                botsBase.GetResourcesAndPositionForNewBase();

            if (_resourceCarrier.TryPutForBase(resourceForBase))
                SetTarget(baseFlag);
        }
    }

    private void InteractWithBaseFlag(BaseFlag baseFlag)
    {
        _mover.Stop();

        if (_resourceCarrier.TryTake(out Resource? resource)
            && _baseCreationModule.TryStartCreation(
                baseFlag, resource.Value))
        {
            _baseCreationState = true;
        }
    }

    private void OnCreated(BotsBase botsBase)
    {
        _base.RemoveBot(this);

        _target = null;
        _baseCreationState = false;

        botsBase.AddBot(this);
    }
}