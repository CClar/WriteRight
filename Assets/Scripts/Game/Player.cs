using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public GameObject explosion;
    public Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
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
        GameObject.Find("GameController").GetComponent<GameController>().Defeat();
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
