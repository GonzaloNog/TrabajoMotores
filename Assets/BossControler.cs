using System.Collections;
using UnityEngine;

public class BossControler : MonoBehaviour
{
    public GameObject[] points;
    private int pointID = 0;
    public float speed = 5;
    public float timePoint;
    public float timeStartBoss;
    private bool startCombat = true;
    private bool waitNewPoint = true;
    public GameObject buff;
    void Start()
    {
        buff.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (startCombat)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[pointID].transform.position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, points[pointID].transform.position) < 0.1f && waitNewPoint)
            {
                Debug.Log("DRAGON: Nuevo Punto Encontrado");
                if(pointID >= 1)
                {
                    LevelManager.Instance.dificultad += 0.5f;
                    buff.SetActive(true);
                }
                StartCoroutine(newPoint());
            }
        }
    }
    IEnumerator newPoint()
    {
        waitNewPoint = false;
        yield return new WaitForSeconds(timePoint);
        buff.SetActive(false);
        pointID++;
        if(pointID == points.Length)
        {
            pointID = points.Length - 1;
        }
        else
        {
            waitNewPoint = true;
        }
    }
}
