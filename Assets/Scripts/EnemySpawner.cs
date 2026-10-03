using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float TimeCoefficient;
    public float minimumTime;
    public GameObject enemy;
    private Camera mainCamera;

    float timeBetweenSpawns = 0;
    float elapsedTime;

    Vector2 positionOffScreen
    {
        get
        {
            bool isOffscreenX = Random.value > 0.5;
            bool isOffscreenY = Random.value > 0.5;

            if (!isOffscreenX && !isOffscreenY)
            {
                isOffscreenX = true;
            }

            int signX = Random.value > 0.5 ? -1 : 1;
            int signY = Random.value > 0.5 ? -1 : 1;

            float x = (isOffscreenX ? 20 : 0 + Random.value * 6) * signX;
            float y = (isOffscreenY ? 20 : 0 + Random.value * 6) * signY;

            return new(x, y);
        }
    }
    private void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime < timeBetweenSpawns) { return; }
        SpawnEnemy();
        timeBetweenSpawns = minimumTime + Random.value * TimeCoefficient;
        elapsedTime = 0;
    }
    void SpawnEnemy()
    {
        Instantiate(enemy, (Vector2)mainCamera.transform.position + positionOffScreen, Quaternion.identity);
        Debug.Log("Enemy spawned!");
    }

}
