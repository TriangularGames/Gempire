using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveUI : MonoBehaviour
{
    public GameObject WaveNumDisplay;
    public GameObject WaveStartComplete;
    public GameObject WaveStartComplete1;
    public GameObject Countdown;
    public GameObject Countdown1;

    public GameObject EndScreen;
    public GameObject EndText;

    public GameObject TutorialScreen;
    public GameObject StartButton;

    public GameObject Score;
    public int scoreNum = 0;
    public int scoreColourCombo = 0;
    public int scoreColour;

    public void Start()
    {
        Time.timeScale = 1;
        TutorialScreen.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine("Startgame");
    }

    IEnumerator Startgame()
    {
        yield return new WaitForEndOfFrame();
        TutorialScreen.SetActive(false);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Wave_Controller>().StartCoroutine("WaveManager");
    }

    public void UpdateScore(int addScore, int enemyColour)
    {
        if (enemyColour != -1 && scoreColour.Equals(enemyColour))
        {
            scoreColourCombo++;
        }
        else 
        {
            scoreColour = enemyColour;
            scoreColourCombo = 0;
        }

        scoreNum += addScore + Mathf.Max(0, scoreColourCombo - 1);
        scoreNum = Mathf.Max(0, scoreNum);
        Score.GetComponent<TextMeshProUGUI>().text = "Score: " + scoreNum;
    }

    //Game Over Display
    public void GameOverDisplay()
    {
        EndScreen.SetActive(true);
        GameObject.FindGameObjectWithTag("StartOver").GetComponent<TextMeshProUGUI>().text = "Start Over";
        EndText.GetComponent<TextMeshProUGUI>().text = "Game Over!";
    }

    //Displays the fact you won! Sick dude
    public void WinDisplay()
    {
        EndScreen.SetActive(true);
        GameObject.FindGameObjectWithTag("StartOver").GetComponent<TextMeshProUGUI>().text = "Play Again";
        EndText.GetComponent<TextMeshProUGUI>().text = "You Win!";
    }

    //Used for the Quit button
    public void MainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Music"));
        SceneManager.LoadScene(0);
    }

    //Used for the Start Over button
    public void StartOver()
    {
        SceneManager.LoadScene(1);
    }

    //When a wave is cleared, display
    public IEnumerator WaveClear()
    {
        WaveStartComplete.SetActive(true);

        WaveStartComplete.GetComponent<TextMeshProUGUI>().text = "Wave Complete!";
        WaveStartComplete1.GetComponent<TextMeshProUGUI>().text = "Wave Complete!";
        yield return new WaitForSeconds(2f);

        WaveStartComplete.SetActive(false);
    }

    //Start of wave display
    public IEnumerator WaveDisplay(object[] parms)
    {
        int waveNum = (int)parms[0];

        WaveStartComplete.SetActive(true);
        Countdown.SetActive(true);

        WaveStartComplete.GetComponent<TextMeshProUGUI>().text = "Wave " + waveNum.ToString() + " starts in:";
        WaveStartComplete1.GetComponent<TextMeshProUGUI>().text = "Wave " + waveNum.ToString() + " starts in:";
        Countdown.GetComponent<TextMeshProUGUI>().text = "3";
        Countdown1.GetComponent<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1f);
        Countdown.GetComponent<TextMeshProUGUI>().text = "2";
        Countdown1.GetComponent<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1f);
        Countdown.GetComponent<TextMeshProUGUI>().text = "1";
        Countdown1.GetComponent<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1f);

        Countdown.SetActive(false);
        WaveStartComplete.SetActive(false);

        WaveNumDisplay.GetComponent<TextMeshProUGUI>().text = "Wave: " + waveNum.ToString();
    }

    public IEnumerator WaveFinal(object[] parms)
    {
        int waveNum = (int)parms[0];

        WaveStartComplete.SetActive(true);
        Countdown.SetActive(true);

        WaveStartComplete.GetComponent<TextMeshProUGUI>().text = "Final Wave starts in: ";
        WaveStartComplete1.GetComponent<TextMeshProUGUI>().text = "Final Wave starts in: ";
        Countdown.GetComponent<TextMeshProUGUI>().text = "3";
        Countdown1.GetComponent<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1f);
        Countdown.GetComponent<TextMeshProUGUI>().text = "2";
        Countdown1.GetComponent<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1f);
        Countdown.GetComponent<TextMeshProUGUI>().text = "1";
        Countdown1.GetComponent<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1f);

        Countdown.SetActive(false);
        WaveStartComplete.SetActive(false);

        WaveNumDisplay.GetComponent<TextMeshProUGUI>().text = "Wave: " + waveNum.ToString();
    }
}
