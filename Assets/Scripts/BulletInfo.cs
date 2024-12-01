using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    Rigidbody2D rb2d;

    public int bulletColour;

    Colour_Information colourInfo;

    //Speed of bullet, can be changed outside script
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1f;
        colourInfo = GameObject.FindGameObjectWithTag("GameController").GetComponent<Colour_Information>();

        //On spawn, set bullet colour to ship colour
        colourInfo.BulletFromPlayer(gameObject.GetComponent<BulletInfo>());
        rb2d = GetComponent<Rigidbody2D>();
    }

    // TODO: change this once sprite is updated
    void Update()
    {
        //Applies force to move the bullet
        rb2d.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    public void setColour(int colour)
    {
        bulletColour = colour;
    }

    public int getColour()
    {
        return bulletColour;
    }
}
