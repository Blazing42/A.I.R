using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Slider healthslider;

    public void SetMaxHealth(int maxHealth)
    {
        healthslider.maxValue = maxHealth;
        healthslider.value = maxHealth;
    }

    public void SetHealthValue(int health)
    {
        healthslider.value = health;
    }
}
