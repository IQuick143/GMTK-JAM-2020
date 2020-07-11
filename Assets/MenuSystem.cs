using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private Button start_button;
    [SerializeField] private string start_scene_name;
    [SerializeField] private Button quit_button;

    private void Start()
    {
        start_button.onClick.AddListener(StartGame);
        quit_button.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(start_scene_name);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
