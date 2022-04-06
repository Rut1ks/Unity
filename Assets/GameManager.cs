using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Slider HealthBar;
    public Image DeathScreen;

    private int TotalScore;

    public int Health = 100;

    public static GameManager ManagerInstance;

    public GameObject Player;

    private void Awake()
    {
        ManagerInstance = this;
    }

    public void DamagePlayer(int Count)
    {
        if (Health > 0)
        {
            Health -= Count;
            HealthBar.value = Health;
            Debug.Log("Вам нанесли урон в размере" + Count);
        }
        else if (Health <= 0 || Player.transform.position.y == -20) GameOver();
    }




    private void GameOver()
    {
        DeathScreen.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void AddScore(int Count)
    {
        TotalScore += Count;         
    }

    public void AddHealth (int Count)
    {
        Health += Count;
    }

}
