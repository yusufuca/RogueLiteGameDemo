using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] int listMaxCount = 10;
    [SerializeField] private float enemySpawnInterval = 5;
    [SerializeField] private float lastSpawnEnemyTime;
    private Vector3 scale;
    private List<GameObject> enemyPool = new List<GameObject>();

    private void Awake()
    {
        scale = new Vector3(enemyPrefab.transform.localScale.x / transform.lossyScale.x,
               enemyPrefab.transform.localScale.y / transform.lossyScale.y,
               enemyPrefab.transform.localScale.z / transform.lossyScale.z);
        for (int i = 0; i < listMaxCount; i++)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab,transform);
            spawnedEnemy.transform.localPosition = Vector3.zero;
            spawnedEnemy.transform.localRotation = Quaternion.identity;
            spawnedEnemy.transform.localScale = scale;

            spawnedEnemy.SetActive(false);
            enemyPool.Add(spawnedEnemy);
        }
    }
    void Update()
    {
        SpawnEnemy();
    }
    public void SpawnEnemy()
    {
      
        if (Time.time - lastSpawnEnemyTime > enemySpawnInterval) 
        {
            for (int i = 0; i < enemyPool.Count; i++)
            {
                if (!enemyPool[i].activeSelf)
                {
                    enemyPool[i].SetActive(true);
                    enemyPool[i].transform.localPosition = Vector3.zero;
                    enemyPool[i].transform.localRotation = Quaternion.identity;
                    enemyPool[i].transform.localScale = scale;
                    lastSpawnEnemyTime = Time.time;
                    break;
                }
            }
        }

    }
}
