using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehaviour : MonoBehaviour
{
    public Slider Slider;
    public Vector3 Offset;
    public GameObject Target;
    void FixedUpdate()
    {
        if(Target != null)
        {
            Slider.transform.position = Camera.main.WorldToScreenPoint(Target.transform.position + Offset);
        }
    }

    public void SetHealth (float currHealth, float maxHealth)
    {
        if(Slider.maxValue != maxHealth)
        {
            Slider.maxValue = maxHealth;
        }

        if(Slider.value != currHealth)
        {
            Slider.value = currHealth;
        }

        Slider.gameObject.SetActive(currHealth < maxHealth);
    }
}
