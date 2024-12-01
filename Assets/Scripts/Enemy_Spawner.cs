using System.Collections;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    //spawnerList Behaviors
    //spawnerList[0] is modified by WaveTypeA
    //spawnerList[1] is modified by WaveTypeB
    //spawnerList[2] is modified by WaveTypeC

    public bool waveTypeARunning = false;
    public bool waveTypeBRunning = false;
    public bool waveTypeCRunning = false;

    public GameObject BlackHole;

    //public Transform spawner;
    public GameObject[] spawnerList;
    public GameObject[] enemies;

    public bool BossAlive;

    public void setBossAlive(bool set)
    {
        BossAlive = set;
    }

    //WaveTypeA consists of random enemy spawning from the edges of the screen with a range of random time intervals.
    public IEnumerator WaveTypeA(object[] parms)
    {
        waveTypeARunning = true;

        int enemyCount = (int)parms[0];
        float minTime = (float)parms[1];
        float maxTime = (float)parms[2];
        float deltaMax = (float)parms[3];
        float initSpeed = (float)parms[4];
        float destSpeed = (float)parms[5];
        float accel = (float)parms[6];

        for (int num = 0; num < enemyCount; num++)
        {
            randomSpawnerBorderPosition(spawnerList[0]);
            SpawnEnemy(0, spawnerList[0], Random.Range(0, enemies.Length), initSpeed, destSpeed, accel);

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            if (minTime < maxTime)
            {
                maxTime -= deltaMax;
            }
        }

        waveTypeARunning = false;
    }

    //WaveTypeB consists of one spot of several enemies spawning of a singular colour.
    public IEnumerator WaveTypeB(object[] parms)
    {
        waveTypeBRunning = true;

        int enemyCount = (int)parms[0];
        float minTime = (float)parms[1];
        float maxTime = (float)parms[2];
        float initSpeed = (float)parms[3];
        float destSpeed = (float)parms[4];
        float accel = (float)parms[5];

        randomSpawnerScreenPosition(spawnerList[1]);

        int randomEnemy = Random.Range(0, enemies.Length);

        for (int num = 0; num < enemyCount; num++)
        {
            SpawnEnemy(1, spawnerList[1], randomEnemy, initSpeed, destSpeed, accel);

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }

        waveTypeBRunning = false;
    }

    //WaveTypeC consists of a circular formation of a number of enemies encroaching on the planet
    public IEnumerator WaveTypeC(object[] parms)
    {
        waveTypeCRunning = true;

        int enemyCount = (int)parms[0];
        float radius = (float)parms[1];
        float initSpeed = (float)parms[2];
        float destSpeed = (float)parms[3];
        float accel = (float)parms[4];

        int randomEnemy = Random.Range(0, enemies.Length);
        float initAngle = Random.Range(0f, 360f);

        for (int num = 0; num < enemyCount; num++)
        {
            plottedSpawnerCirclePosition(spawnerList[2], num, enemyCount, initAngle, radius);
            SpawnEnemy(0, spawnerList[2], randomEnemy, initSpeed, destSpeed, accel);
            yield return new WaitForSeconds(0.1f);
        }

        waveTypeCRunning = false;
    }

    //BossWaveTypeB spawns enemies of the same colour as the boss perodically
    public IEnumerator BossWaveTypeB()
    {
        yield return new WaitForSeconds(1);

        Boss_Controller bossController = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Controller>();

        while (0 < bossController.health)
        {
            randomSpawnerBorderPosition(spawnerList[0]);
            int randomEnemy = bossController.colour;
            SpawnEnemy(0, spawnerList[0], randomEnemy, 1f, 1f, 0f);
            yield return new WaitForSeconds(5);
        }
    }

    //Places enemies for WaveType A and B using the Black Holes
    public IEnumerator PlaceEnemy1(object[] parms)
    {
        GameObject spawner = (GameObject) parms[0];
        int randomEnemy = (int) parms[1];
        float initSpeed = (float)parms[2];
        float destSpeed = (float)parms[3];
        float accel = (float)parms[4];

        Vector3 location = new Vector3(spawner.transform.position.x + Random.Range(-0.75f, 0.75f), spawner.transform.position.y + Random.Range(-0.75f, 0.75f), spawner.transform.position.z);
        GameObject blackhole = Instantiate(BlackHole, location, Quaternion.identity);
        yield return new WaitForSeconds(1);
        GameObject enemy = Instantiate(enemies[randomEnemy], location, Quaternion.identity);
        Destroy(blackhole);

        enemy.GetComponent<Enemy_Controller>().initSpeed = initSpeed;
        enemy.GetComponent<Enemy_Controller>().destSpeed = destSpeed;
        enemy.GetComponent<Enemy_Controller>().accel = accel;
        enemy.GetComponent<Enemy_Controller>().setBossAlive(BossAlive);
        enemy.GetComponent<Enemy_Controller>().StartCoroutine("WaveTypeMovement");

        yield return 0;
    }

    //Places enemies for WaveType C without the use of Black Holes
    public IEnumerator PlaceEnemy2(object[] parms)
    {
        GameObject spawner = (GameObject)parms[0];
        int randomEnemy = (int)parms[1];
        float initSpeed = (float)parms[2];
        float destSpeed = (float)parms[3];
        float accel = (float)parms[4];

        GameObject enemy = Instantiate(enemies[randomEnemy], spawner.transform.position, Quaternion.identity);

        enemy.GetComponent<Enemy_Controller>().initSpeed = initSpeed;
        enemy.GetComponent<Enemy_Controller>().destSpeed = destSpeed;
        enemy.GetComponent<Enemy_Controller>().accel = accel;
        enemy.GetComponent<Enemy_Controller>().setBossAlive(BossAlive);
        enemy.GetComponent<Enemy_Controller>().StartCoroutine("WaveTypeMovement");

        yield return 0;
    }

    public void SpawnEnemy(int waveType, GameObject spawner, int randomEnemy, float initSpeed, float destSpeed, float accel)
    {
        GetComponent<Wave_Controller>().liveEnemyCount++;

        switch (waveType)
        {
            case 1:
                StartCoroutine("PlaceEnemy1", new object[] { spawner, randomEnemy, initSpeed, destSpeed, accel });
                break;

            default:
                StartCoroutine("PlaceEnemy2", new object[] { spawner, randomEnemy, initSpeed, destSpeed, accel });
                break;
        }
    }

    public void SpawnBossEnemy(Transform t, int randomEnemy, float initSpeed, float destSpeed, float accel, float launchX, float launchY, float launchInitSpeed, float launchAccel)
    {
        GetComponent<Wave_Controller>().liveEnemyCount++;
        GameObject enemy;

        enemy = Instantiate(enemies[randomEnemy], t.position, Quaternion.identity);
        enemy.GetComponent<Enemy_Controller>().initSpeed = initSpeed;
        enemy.GetComponent<Enemy_Controller>().destSpeed = destSpeed;
        enemy.GetComponent<Enemy_Controller>().accel = accel;
        enemy.GetComponent<Enemy_Controller>().StartCoroutine("BossTypeMovement", new object[] { launchX, launchY, launchInitSpeed, launchAccel });
    }

    public void randomSpawnerBorderPosition(GameObject spawner)
    {
        float min = 0;
        float max = 0;
        float floatTemp;

        float vectorX = 0;
        float vectorY = 0;

        //Coords for the spawner boundaries
        Vector2[] coordList = new Vector2[4];
        coordList[0] = new Vector2(-9, 9);
        coordList[1] = new Vector2(9, 9);
        coordList[2] = new Vector2(9, -9);
        coordList[3] = new Vector2(-9, -9);

        //TL -> TR 1, TR -> BR 2, BR -> BL 3, BL -> TL 4
        int quadrant = (int)Random.Range(0, 5f);

        //VectorX
        min = coordList[quadrant % 4].x;
        max = coordList[(quadrant + 1) % 4].x;

        if (min > max)
        {
            floatTemp = min;
            min = max;
            max = floatTemp;
        }

        vectorX = Random.Range(min, max);

        //VectorY
        min = coordList[quadrant % 4].y;
        max = coordList[(quadrant + 1) % 4].y;

        if (min > max)
        {
            floatTemp = min;
            min = max;
            max = floatTemp;
        }

        vectorY = Random.Range(min, max);

        //Moves spawner to the random position
        spawner.transform.position = new Vector2(vectorX, vectorY);
    }

    public void randomSpawnerScreenPosition(GameObject spawner)
    {
        Vector2[] coordOuterList = new Vector2[2];
        coordOuterList[0] = new Vector2(-7, -7);
        coordOuterList[1] = new Vector2(7, 7);

        Vector2[] coordInnerList = new Vector2[2];
        coordInnerList[0] = new Vector2(-5, -5);
        coordInnerList[1] = new Vector2(5, 5);

        Vector2 coords = new Vector2(0f, 0f);

        while(IsInsideSquare(coords, coordInnerList))
        {
            coords.x = Random.Range(coordOuterList[0].x, coordOuterList[1].x);
            coords.y = Random.Range(coordOuterList[0].y, coordOuterList[1].y);
        }

        spawner.transform.position = coords;
    }

    public void plottedSpawnerCirclePosition(GameObject spawner, int iteration, int total, float initAngle, float radius)
    {
        float vectorX = 0f + radius * Mathf.Cos(Mathf.Deg2Rad * (initAngle + iteration * 360f/total));
        float vectorY = 0f + radius * Mathf.Sin(Mathf.Deg2Rad * (initAngle + iteration * 360f/total));

        spawner.transform.position = new Vector2(vectorX, vectorY);
    }

    public bool IsInsideSquare(Vector2 coord, Vector2[] boundary)
    {
        return boundary[0].x < coord.x && coord.x < boundary[1].x && boundary[0].y < coord.y && coord.y < boundary[1].y;
    }
}
