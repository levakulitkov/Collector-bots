using System;

[Serializable]
public readonly struct Resource
{
    public int Amount { get; }

    public Resource(int amount)
    {
        Amount = amount;
    }
}