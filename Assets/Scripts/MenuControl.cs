using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public string SceneName;
    public string MenuName;
    public Toggle MusicControlToggle;

    AudioSource Music;

    private void Awake()
    {
        GameObject temp = GameObject.FindWithTag("MUSIC");
        if (temp != null)
        {
            Music = temp.GetComponent<AudioSource>();
            if (Music.isPlaying)
            {
                MusicControlToggle.isOn = true;
            }
            else
            {
                MusicControlToggle.isOn = false;
            }
        }
    }
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
    public void PlayMusic(bool play)
    {
        if (play) Music.Play();
        else Music.Stop();
    }
}
