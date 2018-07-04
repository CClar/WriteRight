// This C# function can be called by an Animation Event
using UnityEngine;
using System.Collections;


public class DestroyExplosion : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}