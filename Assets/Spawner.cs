using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Prifad enemigos y objetos
    public GameObject[] Enemigos;
    public GameObject[] Obstaculos;
    //Largos de los pools
    public int maxPoolEnemigos;
    public int maxPoolObstaculos;
    //Listas para almacenar objetos 
    private List<GameObject> ObstaculosPool = new List<GameObject>();
    private List<GameObject> ObstaculosEnemigos = new List<GameObject>();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
