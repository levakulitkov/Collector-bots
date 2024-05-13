using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ScannerModule : MonoBehaviour
{
    [SerializeField] private float _interval;
    
    public event Action<Resource[]> ScanningCompleted;
    
    public IEnumerator FindFreeResources(Resource[] busyResources)
    {
        var wait = new WaitForSeconds(_interval);
        
        while (true)
        {
            Resource[] detectedResources = SimulateScanning();
            Resource[] freeResources = detectedResources
                .Except(busyResources)
                .Where(resource => resource.enabled)
                .ToArray();
            
            if (freeResources.Length > 0)
            {
                ScanningCompleted?.Invoke(freeResources);
                yield break;
            }
            
            yield return wait;
        }
    }

    private Resource[] SimulateScanning()
    {
        Resource[] resources = FindObjectsOfType<Resource>();

        return resources;
    }
}