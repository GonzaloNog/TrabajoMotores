using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject[] wallPrefabs;   // trozos de muralla
    public int poolSize = 20;

    public Transform leftPoint;
    public Transform rightPoint;

    public float spawnTime = 1.5f;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        // Instanciamos los objetos y los guardamos en la lista luego de apagarlos
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]);
            obj.SetActive(false);
            pool.Add(obj);
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(spawnTime);

        // Spawnear izquierda
        SpawnWall(leftPoint);

        // Spawnear derecha
        SpawnWall(rightPoint);

        StartCoroutine(SpawnLoop());
    }

    void SpawnWall(Transform spawnPoint)
    {
        GameObject obj = GetFreeObject();
        if (obj == null) return;

        obj.transform.position = spawnPoint.position;
        obj.SetActive(true);
    }

    GameObject GetFreeObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeSelf)
                return obj;
        }
        return null;
    }
}
