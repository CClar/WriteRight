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
    private List<Enemy> enemies;

    private void Awake()
    {
        SpawnWaves();
    }
    private void ClearPlaceholder()
    {
        input.placeholder.GetComponent<Text>().text = "";
    }
    private void Update()
    {
        // Set inputbox active on every frame.
        input.ActivateInputField();
    }
    private void SpawnWaves()
    {
        Invoke("ClearPlaceholder", 3);
        // Instantiates enemies based on level
        int spawns = currentLevel * 3;
        GameObject tempEnemy;
        enemies = new List<Enemy>();
        Enemy tempScript;

        for (int i = 0; i < spawns; i++)
        {
            tempEnemy = Instantiate(enemy, PositionOutsideCamera(), Quaternion.identity);
            tempScript = tempEnemy.GetComponent<Enemy>();
            tempScript.SetWord("Example" + i);
            enemies.Add(tempScript);
        }
    }
    public void ClearInputField(string value)
    {
        input.text = " ";
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
