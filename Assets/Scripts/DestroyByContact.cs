using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it's player and not other enemies
        if (collision.gameObject.name != "Player")
            return;

        // if touched player, then play death anmation and damage player
        Player player = collision.gameObject.GetComponent<Player>();
        Enemy currentEnemy = GetComponent<Enemy>();

        player.TakeDamage(currentEnemy.attack);


        // Spawn explosion and remove current enemy
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
