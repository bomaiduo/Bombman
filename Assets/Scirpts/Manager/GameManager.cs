using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private PlayerController player;

    public bool gameOver;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        player = FindObjectOfType<PlayerController>();
    }

    public void Update()
    {
        gameOver = player.isDead;
        UIManager.instance.GameOverUI(gameOver);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
