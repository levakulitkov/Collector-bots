using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScannerModule : MonoBehaviour
{
    [SerializeField] private float _interval = 1f;
    [SerializeField] private int _maxColliders = 16;
    [SerializeField] private int _radius = 100;
    
    public event Action<Resource[]> ScanningCompleted;
    
    public IEnumerator FindFreeResources(Resource[] busyResources)
    {
        var wait = new WaitForSeconds(_interval);
        
        while (true)
        {
            Resource[] detectedResources = ScanWithOverlapSphere();
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

    private Resource[] ScanWithOverlapSphere()
    {
        Collider[] hitColliders = new Collider[_maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _radius, hitColliders, LayerMask.GetMask("Resource"));

        var resources = new List<Resource>();
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent(out Resource resource))
                resources.Add(resource);
        }

        return resources.ToArray();
    }
}