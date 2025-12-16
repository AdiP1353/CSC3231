using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ProjectileSpawner projectileSpawner;
    [SerializeField] SegmentManager segmentManager;
    
    [Header("UI")]
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public GameObject gameOverScreen;
    [SerializeField] public GameObject victoryScreen;
    [SerializeField] public TextMeshProUGUI difficultyText;
    [SerializeField] public float difficultTextFlashTimer = 2f;
    public int score = 0;

    
    public float difficultyInterval = 20f;
    private float _difficultyTimer = 0f;
    
    public bool isDead = false;
    public int currentLevel = 1;

    private void Awake()
    {
        victoryScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    private void Update()
    {
        if (isDead)
        {
            OnPlayerDeath();
        }

        if (score >= 50)
        {
            OnPlayerVictory();
        }
        
        _difficultyTimer += Time.deltaTime;
        if (_difficultyTimer >= difficultyInterval)
        {
            _difficultyTimer = 0f;
            IncreaseDifficulty();
        }
        UpdateScore();
    }


    private void UpdateScore()
    {
        if (score < 0)
        {
            isDead = true;
        }
        scoreText.text = $"Score: {score}";
    }
    
    
    
    private void OnPlayerDeath()
    {
        score = 0;
        projectileSpawner.enabled = false;
        segmentManager.enabled = false;
        gameOverScreen.SetActive(true);
    }
    
    
    private void OnPlayerVictory()
    {
        score = 0;
        projectileSpawner.enabled = false;
        segmentManager.enabled = false;
        victoryScreen.SetActive(true);
    }


    private void IncreaseDifficulty()
    {
        projectileSpawner.speed += 1;
        projectileSpawner.spawnInterval = Mathf.Max(0.2f, projectileSpawner.spawnInterval - 0.05f);
        projectileSpawner.difficulty = Mathf.Min(0.9f, projectileSpawner.difficulty + 0.1f);
        FlashMessage("Difficulty Increased!");
    }
    
    

    
    public void FlashMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessage(message));
    }

    private IEnumerator ShowMessage(string message)
    {
        difficultyText.text = message;
        difficultyText.gameObject.SetActive(true);

        yield return new WaitForSeconds(difficultTextFlashTimer);

        difficultyText.gameObject.SetActive(false);
    }
}
