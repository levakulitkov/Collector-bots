using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ScannerModule : MonoBehaviour
{
    [SerializeField] private float _interval;

    private Coroutine _coroutine;
    
    public event Action<Resource[]> ScanningCompleted;

    private void OnEnable()
    {
        StartScan();
    }
    
    private void StartScan()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        yield return new WaitForEndOfFrame();
        
        var wait = new WaitForSeconds(_interval);
        
        while (enabled)
        {
            SimulateScanning();
            
            yield return wait;
        }
    }

    private void SimulateScanning()
    {
        Resource[] resources = FindObjectsOfType<Resource>();
        
        ScanningCompleted?.Invoke(resources.ToArray());
    }
}