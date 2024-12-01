using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu_Controller : MonoBehaviour
{
    public Texture2D sprite;

    public Button Play;
    public Button Quit;

    public TMP_Dropdown resolution;
    public Resolution[] resolutions;

    public AudioMixer audioMixer;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(sprite, Vector2.zero, CursorMode.Auto);

        Play.onClick.AddListener(PlayGame);
        Quit.onClick.AddListener(QuitGame);

        //For loading the main menu later
        PlayerPrefs.SetString("MainMenu", SceneManager.GetActiveScene().name);

        resolutions = new Resolution[3];
        int width = 600;
        int height = 600;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutions[i].width = width;
            resolutions[i].height = height;
            width += 200;
            height += 200;
        }

        List<string> options = new List<string>();
        int curResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                curResolutionIndex = i;
            }
        }

        resolution.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, false);
    }

    private void PlayGame()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Music"));
        SceneManager.LoadScene(1);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void setMasterVolume(float masterVol)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(masterVol) * 20);
    }

    public void setMusicVolume(float musicVol)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(musicVol) * 20);
    }

    public void setSEVolume(float SEVol)
    {
        audioMixer.SetFloat("SE", Mathf.Log10(SEVol) * 20);
    }

    public void playSE()
    {
        audioSource.Play();
    }
}
