using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float spawnIntervalMin = 5f;
    [SerializeField] private float spawnIntervalMax = 10f;
    private float spawnTimer;

    private GameObject spawnedObject;

    private void Start()
    {
        spawnTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    private void Update()
    {
        if (spawnTimer > 0 && spawnedObject == null)
        {
            spawnTimer -= Time.deltaTime;
        }

        if (spawnTimer <= 0)
        {
            SpawnObject();
            spawnTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
        }
    }


    private void SpawnObject()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject prefab = prefabs[randomIndex];
        spawnedObject = Instantiate(prefab, transform.position, transform.rotation);
    }
}
