using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100;
    public float health;
    private float easeSpeed = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != health)
            healthSlider.value = health;

        if (healthSlider.value != easeHealthSlider.value)
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, easeSpeed);
    }
}
