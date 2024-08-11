using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public string SceneName;
    public string MenuName;
    public void OnGameClick(int idx)
    {

        SceneManager.LoadScene(SceneName);
    }
    public void OnGoToMenuClick()
    {

        SceneManager.LoadScene(MenuName);
    }
    public void OnExitClicked()
    {
        //TODO: test on build
        DebugControl.Log("quit called", 1);
        Application.Quit();
    }
}
