using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss_Controller : MonoBehaviour
{
    //Boss health
    public float health;
    public bool isMoving = false;

    //Colour of Boss
    public int colour;
    public Material material;

    //Boss position fields
    private float orbitAngle;
    private float orbitRadiusX;
    private float orbitRadiusY;
    private float elipseAngle;
    private float orbitSpeed;
    private float elipseSpeed;

    public GameObject blackHole;

    //Sound
    public AudioClip clip;

    //Explosion effect
    public ParticleSystem Hit;

    //Access to spawning enemies
    protected Enemy_Spawner enemySpawner;

    //Setting proper colour for boss
    protected Colour_Information ColourInfo;

    // Start is called before the first frame update
    void Start()
    {
        ColourInfo = GameObject.FindGameObjectWithTag("GameController").GetComponent<Colour_Information>();

        health = 7;
        colour = -1;

        orbitAngle = 90f;
        orbitRadiusX = 5.75f;
        orbitRadiusY = 3.75f;
        elipseAngle = 0f;
        
        enemySpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Enemy_Spawner>();
    }

    public IEnumerator BossManager()
    {
        yield return new WaitForEndOfFrame();
        RandomizeBossColour();

        GameObject bh = Instantiate(blackHole, new Vector3(orbitRadiusX * Mathf.Cos(Mathf.Deg2Rad * orbitAngle), orbitRadiusY * Mathf.Sin(Mathf.Deg2Rad * orbitAngle), 0), Quaternion.identity);
        yield return new WaitForSeconds(2);
        Destroy(bh);

        while (0 < health)
        {
            //move the boss to the updated position along the orbit of the circle with the given radius and the given angle. Angle increments are CCW.
            
            //unrotated "raw" coordinates
            float bossX = 0f + orbitRadiusX * Mathf.Cos(Mathf.Deg2Rad * orbitAngle);
            float bossY = 0f + orbitRadiusY * Mathf.Sin(Mathf.Deg2Rad * orbitAngle);
            //rotated coordinates
            float rotatedBossX = bossX * Mathf.Cos(Mathf.Deg2Rad * elipseAngle) - bossY * Mathf.Sin(Mathf.Deg2Rad * elipseAngle);
            float rotatedBossY = bossX * Mathf.Sin(Mathf.Deg2Rad * elipseAngle) + bossY * Mathf.Cos(Mathf.Deg2Rad * elipseAngle);


            transform.position = new Vector3(rotatedBossX, rotatedBossY, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator BossPanic()
    {
        isMoving = true;

        orbitSpeed = Random.Range(2.5f, 3.5f);
        elipseSpeed = orbitSpeed/3f;

        int direction = -1 + 2 * Convert.ToInt32(1f < Random.Range(0f, 2f));
        int duration = Mathf.FloorToInt(Random.Range(35f, 45f));

        for (int i = 0; i < duration; i++)
        {
            orbitAngle += orbitSpeed * direction;
            elipseAngle += elipseSpeed * direction;
            yield return new WaitForEndOfFrame();
        }

        RandomizeBossColour();

        isMoving = false;
    }

    public IEnumerator BossWaveTypeA()
    {
        //Get angle from (0, 0) to boss
        //Spit out 3 enemies of the colours that the boss is NOT, with a fork pattern from behind the boss.

        float initSpeed = 0;
        float destSpeed = 0.5f;
        float accel = 1000;
        float launchInitSpeed = 5;

        int tMax = 1 + Mathf.FloorToInt((7 - health) / 2);

        for (int t = 0; t < tMax; t++)
        {
            //int randomEnemy = Mathf.FloorToInt(Random.Range(0f, 4f));
            RandomizeBossColour();

            yield return new WaitForEndOfFrame();

            int randomEnemy = Mathf.FloorToInt(Random.Range(1f, 4.99f));
            for (int i = -1; i < 2; i++)
            {
                float initAngle = Mathf.Rad2Deg * Mathf.Atan2(transform.position.y, transform.position.x);
                float launchRadius = Random.Range(5.5f, 6.5f);
                float launchAngle = initAngle + 17.5f * i;
                float launchX = launchRadius * Mathf.Cos(Mathf.Deg2Rad * launchAngle);
                float launchY = launchRadius * Mathf.Sin(Mathf.Deg2Rad * launchAngle);
                float launchAccel = Random.Range(0.05f, 0.1f);

                enemySpawner.SpawnBossEnemy(transform, randomEnemy, initSpeed, destSpeed, accel, launchX, launchY, launchInitSpeed, launchAccel);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Gets the collision object's tag
        string tag = collision.gameObject.tag;

        //Get's the colour of the collision object
        int objColour = collision.gameObject.GetComponent<BulletInfo>().getColour();

        //If the collision object is a Bullet, and is the same colour of the enemy
        // OR the player has the Multi shot

        if (tag == "Bullet" && (objColour == colour || colour == -1))
        {
            //Plays damage audio
            Instantiate(Hit, this.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(clip, transform.position);

            if (1 < health)
            {
                if (!isMoving)
                {
                    StartCoroutine("BossPanic");
                }

                StartCoroutine("BossWaveTypeA");
                health--;
            }
            else
            {
                GameWon();
                health--;
                Destroy(this.gameObject);
            }
        }
        
        Destroy(collision.gameObject);

    }

    private void GameWon()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Wave_Controller>().GameWin();
    }

    public void RandomizeBossColour()
    {
        int randIndex = Mathf.FloorToInt(Random.Range(1f, 4.99f));
        int initColour = colour;
        while (initColour == randIndex)
        {
            randIndex = Mathf.FloorToInt(Random.Range(1f, 4.99f));
        }
        colour = randIndex;

        material.color = ColourInfo.GetColor(colour);
    }
}
