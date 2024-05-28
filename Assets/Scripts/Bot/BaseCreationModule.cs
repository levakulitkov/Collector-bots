using System;
using System.Collections;
using UnityEngine;

public class BaseCreationModule : MonoBehaviour
{
    [SerializeField] private BotsBase _template;
    [SerializeField] private float _creationDuration = 3f;

    public event Action<BotsBase> Created;

    private Coroutine _coroutine;

    public bool TryStartCreation(BaseFlag baseFlag, Resource resource)
    {
        if (resource.Amount >= Constants.BasePrice && _coroutine == null)
        {
            _coroutine = StartCoroutine(BaseCreatingRoutine(baseFlag));
            return true;
        }

        return false;
    }

    private IEnumerator BaseCreatingRoutine(BaseFlag baseFlag)
    {
        yield return new WaitForSeconds(_creationDuration);

        Vector3 position = new(baseFlag.transform.position.x,
            _template.transform.position.y, baseFlag.transform.position.z);
        BotsBase botsBase = Instantiate(_template, baseFlag.transform.parent);

        Destroy(baseFlag.gameObject);

        botsBase.transform.position = position;

        Created?.Invoke(botsBase);
    }
}