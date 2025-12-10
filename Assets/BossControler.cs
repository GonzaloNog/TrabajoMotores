using System.Collections;
using UnityEngine;

public class BossControler : MonoBehaviour
{
    public GameObject[] points;
    public int pointID = 0;
    public float speed = 5;
    public float timePoint;
    private bool startCombat = true;
    private bool waitNewPoint = true;
    void Start()
    {
        
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
                StartCoroutine(newPoint());
            }
        }
    }
    IEnumerator newPoint()
    {
        waitNewPoint = false;
        yield return new WaitForSeconds(timePoint);
        pointID++;
        if(pointID == points.Length - 1)
        {
            pointID = points.Length;
        }
        else
        {
            waitNewPoint = true;
        }
    }
}
