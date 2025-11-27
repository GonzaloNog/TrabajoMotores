using UnityEngine;

public class moveObs : MonoBehaviour
{
    public enum obsType
    {
        obstaculo,
        premio,
        enemigo
    }

    public obsType type;
    public float speed;


    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.transform.Translate(new Vector3(0,0,-1) * speed * Time.deltaTime);
        }
    }

    public void startPoint(Vector3 pont)
    {
        this.transform.position = pont;
    }
}
