// This C# function can be called by an Animation Event
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}