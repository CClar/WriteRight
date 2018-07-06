using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed;
    public int attack = 1;
    public Text textWord;
    public GameObject explosion;

    private string currentWord;

    private void Update()
    {
        // Find and move towards Player gameObject.
        if (GameObject.Find("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
                new Vector2(player.transform.position.x, player.transform.position.y),
                speed * Time.deltaTime);
        }
    }
    public string getWord() {
        return currentWord;
    }
    public void SetWord(string w)
    {
        if (w != null)
            currentWord = w;
            textWord.text = w;
    }

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
        DestroyEnemy();
    }
    public void DestroyEnemy() {
        // Spawn explosion and remove current enemy
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
