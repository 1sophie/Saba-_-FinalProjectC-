using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Added to access the UI Slider component

public class Moving : MonoBehaviour
{
    public float speed = 5f;
    public float speed2 = 7f;
    public Animator anim;
    private bool facingRight = true;
    public int maxHealth = 20;
    public int minHealth = 13;
    public int currentHealth;
    public HealthBar healthBar;

    [Header("Stamina")]
    public float stamina = 100f;
    public float maxStamina = 100f;
    private bool canRun = true;
    public Slider staminaSlider; // Added to reference the UI Slider in the Inspector

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Added initialization for the stamina slider
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = stamina;
        }
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        if (stamina <= 0) canRun = false;
        else if (stamina > 20f) canRun = true;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && h != 0 && canRun;

        transform.Translate(speed * Time.deltaTime * h, 0, 0);

        if (isRunning)
        {
            transform.Translate(speed2 * Time.deltaTime * h, 0, 0);
            stamina -= 20f * Time.deltaTime;
            if (stamina < 0) stamina = 0;
        }
        else
        {
            stamina += 10f * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = stamina;
        }

        if (h > 0)
        {
            facingRight = true;
            if (isRunning) anim.Play("RUN09");
            else anim.Play("WALKING09");
        }
        else if (h < 0)
        {
            facingRight = false;
            if (isRunning) anim.Play("RUNL09");
            else anim.Play("WALKINGL09");
        }
        else
        {
            if (facingRight) anim.Play("IDLE09");
            else anim.Play("IDLEL09");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(2);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= minHealth)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over! Health reached minimum limit.");
        Destroy(gameObject);
    }
}