using System.Collections;
using UnityEngine;

public class Ship_Controller : MonoBehaviour
{
    Rigidbody2D rb2d;

    //Stuff for shoot audio
    public AudioSource audioSource;

    //Colour Change
    public int shipColour;

    //Variables for movement
    public float maxSpeed;
    public float speed;
    public float acceleration;
    public Transform target;
    private Vector3 zAxis = new Vector3(0, 0, 1);

    private float radius;
    private Transform pivot;
    public Transform firePoint;

    //Variables for shooting
    public GameObject bullet;
    public float destroyTime;
    bool canFire = true;
    public float fireRate;
    public int recentKey = 0;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //Sets ship to default colour
        shipColour = 0;

        radius = -0.27f;

        pivot = target.transform;
        transform.parent = pivot;
        transform.position += Vector3.up * radius;
    }

    private void FixedUpdate()
    {
        bool gameOver = GameObject.FindGameObjectWithTag("GameController").GetComponent<Wave_Controller>().gameOver;

        //Moves Player counterclockwise (left)
        //if (Input.GetKey(KeyCode.A) && !gameOver)
        //{
        //    if (speed < maxSpeed)
        //    {
        //        speed += maxSpeed / (acceleration * Time.deltaTime);
        //        speed = Mathf.Min(speed, maxSpeed);
        //    }

        //    recentKey = 0;
        //    rb2d.transform.RotateAround(target.position, zAxis, speed * Time.deltaTime);
        //}
        ////Moves Player clockwise (right)
        //else if (Input.GetKey(KeyCode.D) && !gameOver)
        //{
        //    if (speed < maxSpeed)
        //    {
        //        speed += maxSpeed / (acceleration * Time.deltaTime);
        //        speed = Mathf.Min(speed, maxSpeed);
        //    }

        //    recentKey = 1;
        //    rb2d.transform.RotateAround(target.position, -zAxis, speed * Time.deltaTime);
        //}
        //else if (speed > 0)
        //{
        //    speed -= maxSpeed / (acceleration  * 0.5f * Time.deltaTime);
        //    speed = Mathf.Max(0, speed);
        //    rb2d.transform.RotateAround(target.position, zAxis - 2 * zAxis * recentKey, speed * Time.deltaTime);
        //}

        Vector3 planetVector = Camera.main.WorldToScreenPoint(target.position);
        planetVector = Input.mousePosition - planetVector;
        float angle = Mathf.Atan2(planetVector.y, planetVector.x) * Mathf.Rad2Deg;

        pivot.position = target.position;
        pivot.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        //Fires bullet when space is pressed
        if ((Input.GetMouseButton(0) && canFire) && !gameOver)
        {
            StartCoroutine("ShootBullet");
        }
    }

    IEnumerator ShootBullet()
    {
        //Disallows firing on fire
        canFire = false;

        //Spawning the bullet to be shot
        GameObject clone = Instantiate(bullet, firePoint.position, transform.rotation);

        //Plays shoot audio
        audioSource.Play();

        //Waits for next fire chance based on fire rate
        yield return new WaitForSeconds(fireRate);

        //Allows fire
        canFire = true;

        //Bullet is destroyed after set time, this variable is changed outside scripte
        Destroy(clone, destroyTime);
    }

    //Colour get and set functions
    public void setColour(int colour)
    {
        shipColour = colour;
    }

     public int getColour()
    {
        return shipColour;
    }
}
