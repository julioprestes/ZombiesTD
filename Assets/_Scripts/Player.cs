using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player main;

    [SerializeField] private int health = 500;
    public int money = 0;

    [SerializeField] private TextMeshProUGUI HPGui;
    [SerializeField] private TextMeshProUGUI MoneyGUI;
    [SerializeField] private TextMeshProUGUI HelpGUI; 

    [SerializeField] private GameObject gameOverGUI;

    private bool isHelpVisible = false;

    void Awake()
    {
        main = this;
    }

    void Update()
    {
        HPGui.text = "HP: " + health.ToString();
        MoneyGUI.text = "Dinheiro: " + money.ToString();

        if (Input.GetKeyDown(KeyCode.H))
        {
            isHelpVisible = !isHelpVisible; 
            HelpGUI.gameObject.SetActive(isHelpVisible);
        }
    }

    public void damage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameOverGUI.SetActive(true);
        }
    }

    public void restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}