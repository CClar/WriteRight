using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth = 2;
    public GameObject explosion;
    public Text healthText;

    private void Start()
    {
        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void TakeDamage(int attack)
    {
        currentHealth -= attack;
        healthText.text = currentHealth + "/" + maxHealth;
        if (currentHealth < 1)
            PlayerDeath();
    }
    private void PlayerDeath()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
        // TODO: add gameover/restart
    }
}
