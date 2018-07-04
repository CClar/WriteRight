using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 2;
    public GameObject explosion;

    public void TakeDamage(int attack)
    {
        health -= attack;
        if (health < 1)
        {
            playerDeath();
        }
    }
    private void playerDeath()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
        // TODO: add gameover/restart
    }
}
