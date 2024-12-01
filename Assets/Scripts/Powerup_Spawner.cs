using UnityEngine;

public class Powerup_Spawner : MonoBehaviour
{
    public GameObject[] powerUps;
    int randomPowerup;

    Vector3 spawnPos;

    public void SpawnPowerup()
    {
        Vector3 playerPos;
        float dist = 0;
        float radius = 2.5f;

        //Chooses a random powerup
        randomPowerup = Random.Range(0, powerUps.Length);

        //Gets random position on screen
        while (dist < radius)
        {
            float randomX = Random.value;
            float randomY = Random.value;

            spawnPos = new Vector3(randomX, randomY, 0);
            spawnPos = Camera.main.ViewportToWorldPoint(spawnPos);
            spawnPos.z = 0;

            playerPos = GameObject.FindGameObjectWithTag("Ship").transform.position;
            dist = Mathf.Sqrt((spawnPos.x - playerPos.x) * (spawnPos.x - playerPos.x) + (spawnPos.y - playerPos.y) * (spawnPos.y - playerPos.y));
        }

        //Spawns powerup
        Instantiate(powerUps[randomPowerup], spawnPos, Quaternion.identity);
    }
}
