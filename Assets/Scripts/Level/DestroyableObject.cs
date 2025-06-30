using UnityEngine;

public class DestroableObject : MonoBehaviour, IDestroyable
{
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
