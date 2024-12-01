using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy_Controller : MonoBehaviour
{
    private bool alive;

    public AudioSource audioSource;

    //Colour info variables
    public int colour;
    Colour_Information colourInfo;

    //Explosion particle effect
    public GameObject explosion;

    //Enemy damage to the planet
    public float damage = 1f;

    //Speed of the enemy
    public float initSpeed;
    public float destSpeed;
    public float accel;

    //Boss variables
    private Boss_Controller boss;
    public bool BossAlive;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Controller>();

        if (BossAlive)
        {
            StartCoroutine("KillAll");
        }

        alive = true;
        colourInfo = GameObject.FindGameObjectWithTag("GameController").GetComponent<Colour_Information>();
    }

    public IEnumerator KillAll()
    {
        while (0 < boss.health)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }

    public void setBossAlive(bool set)
    {
        BossAlive = set;
    }

    public IEnumerator WaveTypeMovement()
    {
        alive = true;
        while (alive)
        {
            if (initSpeed > destSpeed)
            {
                initSpeed -= destSpeed / (accel * Time.deltaTime);
                initSpeed = Mathf.Max(destSpeed, initSpeed);
            }
            //Moves the enemy towards the planet wherever it is spawned
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Planet").transform.position, initSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator BossTypeMovement(object[] parms)
    {
        alive = true;

        float launchDestX = (float)parms[0];
        float launchDestY = (float)parms[1];
        float launchInitSpeed = (float)parms[2];
        float launchAccel = (float)parms[3];
        //launchDestSpeed is always 0

        while (alive && 0 < launchInitSpeed)
        {
            launchInitSpeed -= launchAccel;
            launchInitSpeed = Mathf.Max(0f, launchInitSpeed);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(launchDestX, launchDestY), launchInitSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);

        while (alive)
        {
            if (initSpeed < destSpeed)
            {
                initSpeed += destSpeed / (accel * Time.deltaTime);
                initSpeed = Mathf.Min(initSpeed, destSpeed);
            }
            //Moves the enemy towards the planet wherever it is spawned
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Planet").transform.position, initSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    //Colour selection
    public void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            //Detects mouse click
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            //Gets the hit position
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            //If enemy was clicked
            if (hit.collider.tag == "Enemy")
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<Colour_Information>().colourCountdown = 3;
                //Set playerColour to the enemyColour
                colourInfo.EnemyToPlayer(colour);
            }
        }
    }

    //For when the enemy collides with the "Planet"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If colliding with the "planet", do KABAM
        if (collision.gameObject.tag == "Planet")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Wave_Controller>().liveEnemyCount--;

            GameObject.FindGameObjectWithTag("Planet").GetComponent<PlanetInfo>().decreaseHealth(damage);

            alive = false;
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }
    }

    //Detection for Bullets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Gets the collision object's tag
        string tag = collision.gameObject.tag;

        //Get's the colour of the collision object
        int objColour = collision.gameObject.GetComponent<BulletInfo>().getColour();

        //If the collision object is a Bullet, and is the same colour of the enemy
        if (tag == "Bullet" && objColour == colour)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Wave_Controller>().liveEnemyCount--;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveUI>().UpdateScore(1, colour);

            //Both are destroyed
            Vector3 position = this.transform.position;

            //Plays explosion audio
            audioSource.Play();
            alive = false;
            Destroy(this.gameObject);

            Instantiate(explosion, position, Quaternion.identity);

            Destroy(collision.gameObject);
        }
        //Otherwise, just the bullet is destroyed
        else
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveUI>().UpdateScore(-1, -1);

            Destroy(collision.gameObject);
        }
    }
}
