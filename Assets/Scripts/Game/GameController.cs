﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject enemy;
    public InputField input;
    public Text transitionText;
    public Text levelText;
    public GameObject levelTransition;
    public GameObject retunToMenu;
    public GameObject levelMenu;

    private int currentLevel = 1;
    private int maxLevel = 5;
    private float timeBetweenSpawns;
    private Text pHolder;
    private List<Enemy> enemies = new List<Enemy>();
    private List<string> words = new List<string>();
    private int currentNumEnemies;
    private bool levelStarted = false;
    public GameObject playerInput;
    private GameObject player;
    private GameObject menuButton;

    private void Awake()
    {
        // Initialize Components
        pHolder = input.placeholder.GetComponent<Text>();
        player = GameObject.Find("Player");
        menuButton = GameObject.Find("MenuButton");

        // Start Game
        LevelTransition();
    }
    private void Update()
    {
        // Set inputbox active on every frame.
        input.ActivateInputField();

        // Check if level end, start next level if true
        if (currentNumEnemies < 1 && levelStarted)
        {
            levelStarted = false;
            ++currentLevel;
            // Victory
            if (currentLevel > maxLevel)
                Invoke("Victory", 1);
            // Goto next level
            else
                Invoke("LevelTransition", 1);
        }
    }
    private void StartLevel()
    {
        levelStarted = true;
        playerInput.SetActive(true);
        levelTransition.SetActive(false);
        menuButton.SetActive(true);
        words = new List<string>(GetWordList());
        // Set spawn rate
        timeBetweenSpawns = 0.75f;
        // Spawn enemies for level
        SpawnWaves();
    }
    private string[] GetWordList()
    {
        // Gets list of words to use for the enemies
        string path = System.IO.Directory.GetCurrentDirectory() + "/words.txt";
        if (!System.IO.File.Exists(path))
            return new string[0];
        return System.IO.File.ReadAllLines(path);
    }
    private void LevelTransition()
    {
        // Starts the next level
        transitionText.text = "Level " + currentLevel;
        levelText.text = "Level " + currentLevel;
        levelTransition.SetActive(true);
        playerInput.SetActive(false);
        menuButton.SetActive(false);
        Invoke("StartLevel", 3);
    }
    private void Victory()
    {
        levelStarted = false;
        transitionText.text = "Victory!";
        levelTransition.SetActive(true);
        retunToMenu.SetActive(true);
    }
    public void Defeat()
    {
        levelStarted = false;
        transitionText.text = "You have been Defeated.";
        levelTransition.SetActive(true);
        retunToMenu.SetActive(true);
    }
    private void SpawnWaves()
    {
        // Reset variables to default
        enemies.Clear();
        pHolder.text = "Start Typing!";
        Invoke("ClearPlaceholder", 3);

        // Instantiates enemies based on level
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        // TODO: Add error message
        if (words.Count < 1)
        {
            Debug.LogError("No Valid Words in List");
            yield return null;
        }
        // Instantiates enemies based on level
        int spawns = currentLevel * 2;
        currentNumEnemies = spawns;
        GameObject tempEnemy;
        Enemy tempScript;
        Vector2 tempPosition;
        System.Random rnd = new System.Random();
        bool removeWord = words.Count >= spawns;  // Only remove words if there are enough words per enemy

        for (int i = 0; i < spawns; i++)
        {
            // Don't spawn while player is inactive
            while (!player.activeSelf)
                yield return new WaitForSeconds(1);
            // Wait between spawns, skip first spawn
            if (i != 0)
                yield return new WaitForSeconds(timeBetweenSpawns);
            // Spawns enemy outside camera
            tempPosition = PositionOutsideCamera();
            tempEnemy = Instantiate(enemy, tempPosition, Quaternion.identity);
            tempScript = tempEnemy.GetComponent<Enemy>();
            // Sets word for enemy
            ChooseWord(tempScript, rnd, removeWord);
            // Flips sprite if on right side of player
            // 0 since player spawns in middle
            if (tempPosition.x > 0)
                tempEnemy.GetComponent<SpriteRenderer>().flipX = true;
            // Adds reference to enemy script to a list.
            enemies.Add(tempScript);
        }
    }
    private void ChooseWord(Enemy tempScript, System.Random rnd, bool removeWord)
    {
        // Get a random index, and remove the word after setting the current enemy to that word
        int rndIndex = rnd.Next(0, words.Count);
        tempScript.SetWord(words[rndIndex]);

        // Remove word only if there are more or same amount of enemies as there are words in the list
        if (removeWord)
            words.RemoveAt(rndIndex);
    }
    public void FindWord(string value)
    {
        // check if the word and enemy object exists
        foreach (Enemy e in enemies)
            if (string.Equals(e.getWord(), value.Trim(), System.StringComparison.OrdinalIgnoreCase) && e)
            {
                e.DestroyEnemy();
                ClearInputField();
                break;
            }
    }

    private void ClearPlaceholder()
    {
        pHolder.text = "";
    }
    public void ClearInputField()
    {
        input.text = "";
    }
    private Vector2 PositionOutsideCamera()
    {
        // Gets a Vector2 coordinate slightly outside the camera.
        Vector2 dir = Random.insideUnitCircle;
        Vector2 position = Vector2.zero;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {//make it appear on the side
            position = new Vector2(Mathf.Sign(dir.x) * Camera.main.orthographicSize * Camera.main.aspect + (Mathf.Sign(dir.x) * 1),
                                    dir.y * Camera.main.orthographicSize);
        }
        else
        {//make it appear on the top/bottom
            position = new Vector2(dir.x * Camera.main.orthographicSize * Camera.main.aspect,
                                    Mathf.Sign(dir.y) * Camera.main.orthographicSize + (Mathf.Sign(dir.y) * 1));
        }
        return position;
    }
    public void DecreaseEnemyCount()
    {
        currentNumEnemies--;
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void LevelMenu()
    {
        // Pause game and bring up menu
        player.SetActive(false);
        levelMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        // Unpause game and close menu
        player.SetActive(true);
        levelMenu.SetActive(false);
    }
}
