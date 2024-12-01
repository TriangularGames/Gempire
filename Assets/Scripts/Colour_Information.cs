using System.Collections;
using TMPro;
using UnityEngine;

public class Colour_Information : MonoBehaviour
{
    public Material ship;
    Ship_Controller shipControl;

    public Material Vignette;

    //RGBY
    private Color[] colours;

    TextMeshProUGUI countTimer;
    public int colourCountdown = 0;

    private void Start()
    {
        colours = new Color[5];
        colours[0] = Color.white;
        colours[1] = Color.red;
        colours[2] = Color.green;
        colours[3] = Color.blue;
        colours[4] = Color.yellow;

        shipControl = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship_Controller>();
        countTimer = GameObject.FindGameObjectWithTag("ColourTimer").GetComponent<TextMeshProUGUI>();

        //Sets material default colours
        ship.color = Color.white;
        Vignette.color = Color.black;

        StartCoroutine("CountdownManager");
    }

    public IEnumerator CountdownManager()
    {
        while (true)
        {
            colourCountdown = Mathf.Max(colourCountdown - 1, 0);
            countTimer.text = colourCountdown.ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    //Changes bullet colour using player colour on fire
    public void BulletFromPlayer(BulletInfo bullet)
    {
        bullet.setColour(shipControl.getColour());

    }

    //Changes player colour based on enemy clicked
    public void EnemyToPlayer(int colour)
    {
        shipControl.setColour(colour);
        //colourText.text = colour;

        //Coroutine pulses the Vignette colour
        StartCoroutine("Pulse", colour);
    }

    public Color GetColor(int colour)
    {
        return colours[colour];
    }

    IEnumerator Pulse(int colour)
    {
        //Changes ship and Vignette to current colour (enemy colour clicked)
        ship.color = colours[colour];
        Vignette.color = colours[colour];

        yield return new WaitUntil(() => colourCountdown == 0);

        //Changes Vignette back to default black
        Vignette.color = Color.black;
        ship.color = Color.white;
        GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship_Controller>().setColour(0);

        StopCoroutine("Pulse");
    }
}
