using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject enemy;
    public InputField input;
    public Text transitionText;
    public GameObject levelTransition;


    private int currentLevel = 1;
    private int maxLevel = 3;
    private List<Enemy> enemies = new List<Enemy>();
    private Text pHolder;
    private List<string> words = new List<string>();
    private int currentNumEnemies;
    private bool levelStarted = false;

    private void Awake()
    {
        // Initialize Components
        pHolder = input.placeholder.GetComponent<Text>();

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
            //TODO: Victory
            if (currentLevel > maxLevel) return;
            // Goto next level
            else
                LevelTransition();
        }
    }
    private void StartLevel()
    {
        levelStarted = true;
        levelTransition.SetActive(false);
        words = new List<string>(GetWordList());
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
        levelTransition.SetActive(true);
        Invoke("StartLevel", 3);
    }
    private void SpawnWaves()
    {
        // Reset variables to default
        enemies.Clear();
        pHolder.text = "Start Typing!";
        Invoke("ClearPlaceholder", 3);

        // Instantiates enemies based on level
        SpawnEnemy();
    }
    private void SpawnEnemy()
    {
        // TOOO: Add error message
        if (words.Count < 1)
        {
            Debug.LogError("No Valid Words in List");
            return;
        }
        // Instantiates enemies based on level
        int spawns = currentLevel * 3;
        currentNumEnemies = 0;
        GameObject tempEnemy;
        Enemy tempScript;
        System.Random rnd = new System.Random();
        bool removeWord = words.Count >= spawns;  // Only remove words if there are enough words per enemy

        for (int i = 0; i < spawns; i++)
        {
            currentNumEnemies++;
            // Spawns enemy outside camera
            tempEnemy = Instantiate(enemy, PositionOutsideCamera(), Quaternion.identity);
            tempScript = tempEnemy.GetComponent<Enemy>();
            // Sets word for enemy
            ChooseWord(tempScript, rnd, removeWord);
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
                input.text = "";
                break;
            }
    }
    private void ClearPlaceholder()
    {
        pHolder.text = "";
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
}
