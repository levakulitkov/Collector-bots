using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private SpawnedResource _template;
    [SerializeField] private float _interval;

    private float _minGenerationPosX;
    private float _maxGenerationPosX;
    private float _minGenerationPosZ;
    private float _maxGenerationPosZ;
    private Coroutine _coroutine;

    private void Awake()
    {
        float halfExtentX = transform.localScale.x / 2;
        float halfExtentZ = transform.localScale.z / 2;

        _minGenerationPosX = transform.position.x - halfExtentX;
        _maxGenerationPosX = transform.position.x + halfExtentX;
        _minGenerationPosZ = transform.position.z - halfExtentZ;
        _maxGenerationPosZ = transform.position.z + halfExtentZ;
    }

    private void OnEnable()
    {
        StartGeneration();
    }

    private void StartGeneration()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(GeneratingRoutine());
    }

    private IEnumerator GeneratingRoutine()
    {
        var wait = new WaitForSeconds(_interval);

        while (enabled)
        {
            Generate();

            yield return wait;
        }
    }

    private void Generate()
    {
        float posX = Random.Range(_minGenerationPosX, _maxGenerationPosX);
        float posZ = Random.Range(_minGenerationPosZ, _maxGenerationPosZ);
        float resourceRadius = _template.transform.localScale.y / 2;

        var position = new Vector3(posX, transform.position.y + resourceRadius, posZ);
        Instantiate(_template, position, Quaternion.identity);
    }
}