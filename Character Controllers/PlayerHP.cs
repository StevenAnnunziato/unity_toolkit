using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{

    [Header("HP Settings")]
    public int maxHP = 5;
    public float killPlaneDepth = -20f;

    // private member variables
    private int currentHP;
    private bool dead;

    // getters/setters
    public bool CheckDead() { return dead; }
    public int GetCurrentHP() { return currentHP; }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead && currentHP <= 0)
        {
            Die();
        }

        if (transform.position.y < killPlaneDepth)
        {
            Die();
        }
    }

    public void TakeDamage(int dam)
    {
        currentHP -= dam;
    }

    public void Heal(int hp)
    {
        currentHP += hp;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    private void Die()
    {
        dead = true;

        // disable components
        //GetComponent<PlayerMovement>().enabled = false;

        // enable a death/loss screen
        //deathScreen.SetActive(true);

        // unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReloadScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
