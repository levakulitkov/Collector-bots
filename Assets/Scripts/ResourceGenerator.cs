using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _template;
    [SerializeField] private float _interval;
    
    private float _minGenerationPosX;
    private float _maxGenerationPosX;
    private float _minGenerationPosZ;
    private float _maxGenerationPosZ;
    private Coroutine _coroutine;
    
    private void Awake()
    {
        _minGenerationPosX = transform.position.x - transform.localScale.x / 2;
        _maxGenerationPosX = transform.position.x + transform.localScale.x / 2;
        _minGenerationPosZ = transform.position.z - transform.localScale.z / 2;
        _maxGenerationPosZ = transform.position.z + transform.localScale.z / 2;
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
        var position = new Vector3(posX, transform.position.y, posZ);
        Resource resource = Instantiate(_template, position, Quaternion.identity);
    }
}
