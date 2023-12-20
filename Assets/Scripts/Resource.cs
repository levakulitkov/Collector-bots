using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsBusy { get; private set; }

    public void DoBusy()
    {
        IsBusy = true;
    }
}
