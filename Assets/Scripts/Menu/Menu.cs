using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {
    public Texture2D handCursor;

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame() {
        Application.Quit();
    }
}
