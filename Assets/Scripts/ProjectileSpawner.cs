using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public SegmentManager segmentManager;

    public float spawnInterval = 2f;
    public float speed = 2f;
    public float difficulty = 0f;
    
    private float timer;
    private float gameTime = 0f;
    
    private enum SpawnPattern { Single, Stream, Pincer, Random }
    private SpawnPattern currentPattern = SpawnPattern.Single;
    
    private const float STREAM_UNLOCK_TIME = 30f;
    private const float PINCER_UNLOCK_TIME = 60f;
    private const float CHAOS_UNLOCK_TIME = 90f;

    void Update()
    {
        gameTime += Time.deltaTime;
        timer += Time.deltaTime;
        
        UpdateDifficulty();
        UpdateSpawnProbability();
        
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnProjectilePattern();
        }
    }
    
    private void UpdateDifficulty()
    {
        spawnInterval = Mathf.Max(0.4f, 2f - (gameTime * 0.005f));
        speed = 2f + (gameTime * 0.08f);
        
        if (gameTime < STREAM_UNLOCK_TIME)
            currentPattern = SpawnPattern.Single;
        else if (gameTime < PINCER_UNLOCK_TIME)
            currentPattern = SpawnPattern.Stream;
        else if (gameTime < CHAOS_UNLOCK_TIME)
            currentPattern = SpawnPattern.Pincer;
        else
            currentPattern = SpawnPattern.Random;
    }
    
    private void UpdateSpawnProbability()
    {
        if (gameTime < 30f)
            difficulty = 0.3f;
        else if (gameTime < 90f)
            difficulty = 0.5f;
        else
            difficulty = 0.6f;
    }
    
    private void SpawnProjectilePattern()
    {
        switch (currentPattern)
        {
            case SpawnPattern.Single:
                SpawnSingle();
                break;
            case SpawnPattern.Stream:
                SpawnStream();
                break;
            case SpawnPattern.Pincer:
                SpawnPincer();
                break;
            case SpawnPattern.Random:
                SpawnChaos();
                break;
        }
    }
    
    private void SpawnSingle()
    {
        int randomSide = Random.Range(0, 8);
        SpawnProjectileAtSide(randomSide);
    }
    
    private void SpawnStream()
    {
        int streamSide = Random.Range(0, 8);
        int streamCount = Random.Range(2, 4);
        
        for (int i = 0; i < streamCount; i++)
        {
            SpawnProjectileAtSide(streamSide);
        }
    }
    
    private void SpawnPincer()
    {
        int side1 = Random.Range(0, 8);
        int side2 = (side1 + 4) % 8;
        
        SpawnProjectileAtSide(side1);
        SpawnProjectileAtSide(side2);
        
        if (Random.value > 0.6f)
        {
            SpawnProjectileAtSide(Random.Range(0, 8));
        }
    }
    
    private void SpawnChaos()
    {
        float chaosRoll = Random.value;
        
        if (chaosRoll < 0.3f)
            SpawnSingle();
        else if (chaosRoll < 0.6f)
            SpawnStream();
        else
            SpawnPincer();
    }

    private void SpawnProjectileAtSide(int side)
    {
        Vector2 spawnPos = segmentManager.GetWorldPositionForSide(side);
        Vector2 direction = (Vector2.zero - spawnPos).normalized;

        var p = Instantiate(projectilePrefab, spawnPos, Quaternion.identity).GetComponent<Projectile>();

        p.type = Random.value < (1f - difficulty) ? ProjectileType.Apple : ProjectileType.Bomb;
        p.speed = speed;
        p.direction = direction;
        
        SpriteRenderer sr = p.GetComponent<SpriteRenderer>();
        
        if (p.type == ProjectileType.Bomb)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.green;
        }
    }
    
}
