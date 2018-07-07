using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject enemy;
    public InputField input;

    private int currentLevel = 1;
    private int maxLevel;
    private List<Enemy> enemies = new List<Enemy>();
    private Text pHolder;
    private List<string> words;

    private void Awake()
    {
        // Get pHolder text object
        pHolder = input.placeholder.GetComponent<Text>();
        // Gets list of words to use for the enemies
        string path = System.IO.Directory.GetCurrentDirectory() + "/words.txt";
        if (!System.IO.File.Exists(path))
            return;
        string[] fileWords = System.IO.File.ReadAllLines(path);
        words = new List<string>(fileWords);
        // Spawn enemies for level
        SpawnWaves();
    }
    private void ClearPlaceholder()
    {
        pHolder.text = "";
    }
    private void Update()
    {
        // Set inputbox active on every frame.
        input.ActivateInputField();
    }
    private void SpawnWaves()
    {
        // Reset variables to refault
        enemies.Clear();
        pHolder.text = "Start Typing!";
        Invoke("ClearPlaceholder", 3);

        // Instantiates enemies based on level
        SpawnEnemy();
    }
    private void SpawnEnemy()
    {
        // Instantiates enemies based on level
        int spawns = currentLevel * 3;
        GameObject tempEnemy;
        Enemy tempScript;
        System.Random rnd = new System.Random();

        for (int i = 0; i < spawns; i++)
        {
            // Spawns enemy outside camera
            tempEnemy = Instantiate(enemy, PositionOutsideCamera(), Quaternion.identity);
            tempScript = tempEnemy.GetComponent<Enemy>();
            // Sets word for enemy
            ChooseWord(tempScript, rnd, spawns);
            // Adds reference to enemy script to a list.
            enemies.Add(tempScript);
        }
    }
    private void ChooseWord(Enemy tempScript, System.Random rnd, int spawns)
    {
        // Get a random index, and remove the word after setting the current enemy to that word
        int rndIndex = rnd.Next(0, words.Count - 1);
        tempScript.SetWord(words[rndIndex]);

        // Remove word only if there are more or same amount of enemies as there are words in the list
        if (words.Count <= spawns)
            words.RemoveAt(rndIndex);
    }
    public void ClearInputField(string value)
    {
        input.text = "";
    }
    public void FindWord(string value)
    {
        foreach (Enemy e in enemies)
        {
            // check if the word and enemy object exists
            if (string.Equals(e.getWord(), value.Trim(), System.StringComparison.OrdinalIgnoreCase) && e)
            {
                e.DestroyEnemy();
                input.text = "";
                break;
            }
        }
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
}
