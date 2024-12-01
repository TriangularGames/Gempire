using System.Collections;
using UnityEngine;

public class Wave_Controller : MonoBehaviour
{
    //Spawners for the Waves
    public Enemy_Spawner enemySpawner;
    public Powerup_Spawner powerupSpawner;
    public Boss_Controller bossController;

    //Wave UI
    public WaveUI ui;

    public bool gameOver = false;
    public int liveEnemyCount = 0;

    public void GameOver()
    {
        gameOver = true;
        ui.GameOverDisplay();
        Time.timeScale = 0;
    }

    public void GameWin()
    {
        ui.WinDisplay();
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    //Wave Parameters

    //WaveTapeA parms {int enemycount, float mintime, float maxtime, float deltamax, float initspeed, float destspeed, float accel} Square
    //WaveTapeB parms {int enemycount, float mintime, float maxtime, float initspeed, float destspeed, float accel} Burst
    //WaveTapeC parms {int enemycount, float radius, float initspeed, float destspeed, float accel} Circle
    public IEnumerator WaveManager()
    {
        // Wave 1
        ui.StartCoroutine("WaveDisplay", new object[] { 1 });
        yield return new WaitForSeconds(3);
        enemySpawner.StartCoroutine("WaveTypeA", new object[] { 8, 3f, 5f, 0f, 5f, 2f, 1000f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 3f, 1f, 1000f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 3f, 0.5f, 1000f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeC", new object[] { 6, 10f, 0.5f, 1f, 0f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 3f, 0.5f, 1000f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 3f, 1f, 1000f });

        yield return new WaitUntil(() => liveEnemyCount == 0 && !GetComponent<Enemy_Spawner>().waveTypeARunning && !GetComponent<Enemy_Spawner>().waveTypeBRunning);
        ui.StartCoroutine("WaveClear");
        yield return new WaitForSeconds(3);

        //Wave 2
        ui.StartCoroutine("WaveDisplay", new object[] { 2 });
        yield return new WaitForSeconds(3);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 6, 0.35f, 0.35f, 4f, 0.5f, 1000f });
        yield return new WaitForSeconds(3);
        enemySpawner.StartCoroutine("WaveTypeA", new object[] { 10, 2f, 3f, 0f, 5f, 1.5f, 1000f });
        yield return new WaitForSeconds(7);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 6, 0.35f, 0.35f, 4f, 1f, 1000f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeC", new object[] { 6, 10f, 0.5f, 1f, 0f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 3f, 1f, 1000f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 3f, 1f, 1000f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 3f, 1f, 1000f });

        yield return new WaitUntil(() => liveEnemyCount == 0 && !GetComponent<Enemy_Spawner>().waveTypeARunning && !GetComponent<Enemy_Spawner>().waveTypeBRunning && !GetComponent<Enemy_Spawner>().waveTypeCRunning);
        ui.StartCoroutine("WaveClear");
        yield return new WaitForSeconds(3);

        //Wave 3
        ui.StartCoroutine("WaveDisplay", new object[] { 3 });
        yield return new WaitForSeconds(3);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 2f, 1f, 1000f });
        yield return new WaitForSeconds(1.5f);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 4, 0.35f, 0.35f, 2.5f, 1.25f, 1000f });
        yield return new WaitForSeconds(5);
        enemySpawner.StartCoroutine("WaveTypeC", new object[] { 6, 10f, 0.5f, 1f, 0f });
        yield return new WaitForSeconds(1.5f);
        enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0.35f, 0.35f, 3f, 1.5f, 1000f });
        yield return new WaitForSeconds(3);
        enemySpawner.StartCoroutine("WaveTypeA", new object[] { 10, 1.5f, 1.5f, 0f, 5f, 2.5f, 1000f });
        yield return new WaitForSeconds(6);
        for (int i = 0; i < 5; i++)
        {
            enemySpawner.StartCoroutine("WaveTypeB", new object[] { 5, 0f, 0f, 1f, 0.2f, 1000f });
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitUntil(() => liveEnemyCount == 0 && !GetComponent<Enemy_Spawner>().waveTypeARunning && !GetComponent<Enemy_Spawner>().waveTypeBRunning && !GetComponent<Enemy_Spawner>().waveTypeCRunning);
        ui.StartCoroutine("WaveClear");
        yield return new WaitForSeconds(3);

        //Wave 4
        ui.StartCoroutine("WaveDisplay", new object[] { 4 });
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 5; i++)
        {
            enemySpawner.StartCoroutine("WaveTypeC", new object[] { 2 + (int)Mathf.Floor(i / 3), 10f + i / 2f, 4f + i / 6f, 0.5f + i / 6f, 1000f });
            yield return new WaitForSeconds(3f - i / 9f);
        }
        yield return new WaitForSeconds(2);
        enemySpawner.StartCoroutine("WaveTypeC", new object[] { 12, 12f, 3f, 0.2f, 1000f });
        yield return new WaitForSeconds(10);
        enemySpawner.StartCoroutine("WaveTypeA", new object[] { 8, 1.5f, 1.5f, 0f, 5f, 2.5f, 1000f });
        for (int i = 0; i < 3; i++)
        {
            enemySpawner.StartCoroutine("WaveTypeB", new object[] { 1, 0f, 0f, 4f, 1.5f, 1000f });
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitUntil(() => liveEnemyCount == 0 && !GetComponent<Enemy_Spawner>().waveTypeARunning && !GetComponent<Enemy_Spawner>().waveTypeBRunning && !GetComponent<Enemy_Spawner>().waveTypeCRunning);
        ui.StartCoroutine("WaveClear");
        yield return new WaitForSeconds(3);

        //Wave 5
        ui.StartCoroutine("WaveFinal", new object[] { 5 });
        yield return new WaitForSeconds(3);
        bossController.StartCoroutine("BossManager");
        enemySpawner.StartCoroutine("BossWaveTypeB");
        enemySpawner.setBossAlive(true);
    }
}
