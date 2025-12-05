using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    //Prifad enemigos y objetos
    public GameObject[] Enemigos;
    public GameObject[] Obstaculos;
    public GameObject[] Premios;
    public GameObject[] fuegos;
    //Largos de los pools
    public int maxPoolEnemigos;
    public int maxPoolObstaculos;
    public int maxPoolPremios;
    public int maxFuegosPool;
    //Listas para almacenar objetos 
    private List<GameObject> ObstaculosPool = new List<GameObject>();
    private List<GameObject> ObstaculosEnemigos = new List<GameObject>();
    private List<GameObject> PremiosPool = new List<GameObject>();
    private List<GameObject> FuegosPool = new List<GameObject>();
    //puntos de spawn
    public GameObject[] points;
    //Spawn Type Time
    public float enemigosTime;
    public float obstaculoTime;
    public float premiosTime;
    public float rangeTimeRandom;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //Instanciamos los objetos y los guardamos en las listas luego de apagarlos
        for(int a = 0; a < maxPoolObstaculos; a++)
        {
            GameObject tempGO = Instantiate(Obstaculos[Random.Range(0, Obstaculos.Length)]);
            tempGO.SetActive(false);
            ObstaculosPool.Add(tempGO);
        }

        for(int a = 0; a < maxPoolEnemigos; a++)
        {
            GameObject tempGO = Instantiate(Enemigos[Random.Range(0, Enemigos.Length)]);
            tempGO.SetActive(false);
            ObstaculosEnemigos.Add(tempGO);
        }

        for (int a = 0; a < maxPoolPremios; a++)
        {
            GameObject tempGO = Instantiate(Premios[Random.Range(0, Premios.Length)]);
            tempGO.SetActive(false);
            PremiosPool.Add(tempGO);
        }
        for (int a = 0; a < maxFuegosPool; a++)
        {
            GameObject tempGO = Instantiate(fuegos[Random.Range(0, Premios.Length)]);
            tempGO.SetActive(false);
            FuegosPool.Add(tempGO);
        }
        //Debug.Log("Lista enemigos: " + ObstaculosEnemigos.Count);
        //Debug.Log("Lista premios: " + PremiosPool.Count);
        StartCoroutine(spawnObstacle());
        StartCoroutine(spawnPremios());
        StartCoroutine(spawnEnemigos());
    }
    IEnumerator spawnEnemigos()
    {
        yield return new WaitForSeconds(enemigosTime);
        GameObject obj = null;
        for(int a = 0; a < ObstaculosEnemigos.Count; a++)
        {
            if (!ObstaculosEnemigos[a].gameObject.activeSelf)
            {
                obj = ObstaculosEnemigos[a];
                break;
            }
        }
        if (obj != null)
        {
            obj.SetActive(true);
            obj.GetComponent<moveObs>().resetAtaque = true;
            obj.transform.position = points[Random.RandomRange(0,points.Length)].transform.position;
        }
        else
            Debug.Log("Lista obstaculos completa");
        StartCoroutine(spawnEnemigos());
    }
    IEnumerator spawnObstacle()
    {
        yield return new WaitForSeconds(obstaculoTime);
        GameObject obj = null;
        for (int a = 0; a < ObstaculosPool.Count; a++)
        {
            if (!ObstaculosPool[a].gameObject.activeSelf)
            {
                obj = ObstaculosPool[a];
                break;
            }
        }
        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = points[Random.RandomRange(0, points.Length)].transform.position;
        }
        else
            Debug.Log("Lista obstaculos completa");
        StartCoroutine(spawnObstacle());
    }
    IEnumerator spawnPremios()
    {
        //Debug.Log("Coins: nueva coin");
        yield return new WaitForSeconds(premiosTime);
        GameObject obj = null;
        for (int a = 0; a < PremiosPool.Count; a++)
        {
            if (!PremiosPool[a].gameObject.activeSelf)
            {
                obj = PremiosPool[a];
                break;
            }
        }
        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = points[Random.RandomRange(0, points.Length)].transform.position;
        }
        else
            Debug.Log("Lista obstaculos completa");
        StartCoroutine(spawnPremios());
    }
    public GameObject getFuego()
    {
        for (int a = 0; a < FuegosPool.Count; a++)
        {
            if (!FuegosPool[a].gameObject.activeSelf)
            {
                return FuegosPool[a];
            }
        }
        Debug.Log("Lista de fuego completa");
        return null;
    }
}
