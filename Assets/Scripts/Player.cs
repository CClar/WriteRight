using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 2;
    public GameObject explosion;

    private void Start()
    {
        
    }

    public void TakeDamage(int attack)
    {
        health -= attack;
        if (health < 1)
        {
            PlayerDeath();
        }
    }
    private void PlayerDeath()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
        // TODO: add gameover/restart
    }
}
