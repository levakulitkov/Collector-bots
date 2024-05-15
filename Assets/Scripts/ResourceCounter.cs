using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    public event Action<int> Changed;
    public int Count => _count;

    [SerializeField] private BotsBase _botsBase;

    private int _count;

    private void OnEnable()
    {
        _botsBase.ResourceAdded += Increase;
    }

    private void OnDisable()
    {
        _botsBase.ResourceAdded -= Increase;
    }

    private void Increase(int value)
    {
        _count += value;
        Changed?.Invoke(_count);
    }
}
