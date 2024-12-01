using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCircles : MonoBehaviour
{
    public GameObject circles;

    public void Showcircles()
    {
        circles.SetActive(true);
    }

    public void Hidecircles()
    {
        circles.SetActive(false);
    }
}
