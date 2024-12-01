using UnityEngine;
using UnityEngine.UI;

public class PlanetInfo : MonoBehaviour
{

    public Texture2D sprite;

    private float health;
    private const float maxHealth = 15f;

    // Transform of the GameObject you want to shake
    private Transform cameraPos;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.03f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 0.5f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    public ParticleSystem Hit;
    private AudioSource audioSource;
    public HealthBar healthBar;

    private void Start()
    {

        Cursor.SetCursor(sprite, new Vector2(sprite.width/2, sprite.height/2), CursorMode.Auto);

        health = maxHealth;
        healthBar.setMax(maxHealth);


        cameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform;
        audioSource = this.GetComponent<AudioSource>();
        initialPosition = cameraPos.position;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            cameraPos.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            cameraPos.position = initialPosition;
        }
    }

    public void decreaseHealth(float amount)
    {
        if (health != 0)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<WaveUI>().UpdateScore(-10, -1);

            health = health - amount;
            healthBar.setHealth(health);

            TriggerShake();
            Instantiate(Hit, this.transform.position, Quaternion.identity);
            audioSource.Play();
        }
        else if (health == 0)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Wave_Controller>().GameOver();
        }

    }

    public void TriggerShake()
    {
        shakeDuration = 0.2f;
    }
}
