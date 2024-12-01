using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    public void setMax(float max)
    {
        slider.maxValue = max;
        slider.value = max;
    }

    public void setHealth(float health)
    {
        slider.value = health;
    }
}
