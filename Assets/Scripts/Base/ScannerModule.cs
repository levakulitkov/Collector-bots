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
    [SerializeField] private LayerMask _layerMask;

    public event Action<SpawnedResource[]> ScanningCompleted;

    public IEnumerator FindFreeResources(SpawnedResource[] busyResources)
    {
        var wait = new WaitForSeconds(_interval);

        while (true)
        {
            SpawnedResource[] detectedResources = ScanWithOverlapSphere();
            SpawnedResource[] freeResources = detectedResources
                .Except(busyResources)
                .OrderBy(r => (r.transform.position - transform.position).sqrMagnitude)
                .ToArray();

            if (freeResources.Length > 0)
            {
                ScanningCompleted?.Invoke(freeResources);
                yield break;
            }

            yield return wait;
        }
    }

    private SpawnedResource[] ScanWithOverlapSphere()
    {
        Collider[] hitColliders = new Collider[_maxColliders];
        int colliders = Physics.OverlapSphereNonAlloc(transform.position, _radius,
            hitColliders, _layerMask);

        var resources = new List<SpawnedResource>();
        for (int i = 0; i < colliders; i++)
        {
            if (hitColliders[i].TryGetComponent(out SpawnedResource resource)
                && resource.enabled)
                resources.Add(resource);
        }

        return resources.ToArray();
    }
}