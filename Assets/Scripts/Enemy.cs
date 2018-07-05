using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed;
    public int attack = 1;

    void Update()
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
}
