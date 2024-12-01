using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Controller : MonoBehaviour
{

    private string powerup;
    public Ship_Controller ship;

    public AudioSource audioSource;

    public ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        powerup = this.tag;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Gets the collision object's tag
        string tag = collision.gameObject.tag;

        if (tag == "Bullet")
        {
            Destroy(collision.gameObject);

            switch (powerup)
            {
                case "Multi":
                    StartCoroutine("MultiPowerUp");
                    break;
                case "Fast":
                    StartCoroutine("FastPowerUp");
                    break;
                case "Spread":
                    StartCoroutine("SpreadPowerUp");
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator MultiPowerUp()
    {

        //Both are destroyed
        //Vector3 position = this.transform.position;

        //Plays explosion audio
        audioSource.Play();
        Destroy(this.gameObject);

        //Instantiate(particles, position, Quaternion.identity);

        yield return new WaitForSeconds(10);
    }

    IEnumerator FastPowerUp()
    {

        //Both are destroyed
        //Vector3 position = this.transform.position;

        //Plays explosion audio
        audioSource.Play();
        Destroy(this.gameObject);

        //Instantiate(particles, position, Quaternion.identity);

        yield return new WaitForSeconds(10);
    }

    IEnumerator SpreadPowerUp()
    {
        yield return 0;
    }
}
