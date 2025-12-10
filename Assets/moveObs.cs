using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class moveObs : Subject<GameEvent>
{
    public enum obsType
    {
        obstaculo,
        premio,
        enemigo,
        enemigoAtaque,
        playerAtaque
    }

    public obsType type;
    public float speed;
    public int prioridad;
    public int ataqueEnfriamiento;
    public bool resetAtaque = false;
    public float direccion;
    public float aumento = 1;

    private void Start()
    {
        AddObserver(PlayerControler.Instance);
    }
    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.transform.Translate(new Vector3(0,0,direccion) * speed * aumento * Time.deltaTime);
        }
        if (type == obsType.enemigo && resetAtaque)
        {
            Debug.Log("fuego prendido");
            resetAtaque = false;
            StartCoroutine(nuevoAtaque());
        }
    }

    public void startPoint(Vector3 pont)
    {
        this.transform.position = pont;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(type == obsType.playerAtaque)
        {
            if(other.tag == "spawner")
            {
                if(other.GetComponent<moveObs>().type == obsType.enemigo)
                {
                    Debug.Log("ENP: Enemigo Golpeado pora taque player");
                    Notify(GameEvent.enemyDestroy);
                    other.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                }
                if(other.GetComponent<moveObs>().type == obsType.enemigoAtaque)
                {
                    other.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                }
                if (other.GetComponent<moveObs>().type == obsType.enemigoAtaque)
                {
                    other.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                }
            }   
        }
        else if(other.tag == "spawner")
        {
            if (other.GetComponent<moveObs>().type != obsType.enemigoAtaque && type != obsType.enemigoAtaque)
            {
                Debug.Log("interpolacion de spawners detectada");
                if (prioridad > other.GetComponent<moveObs>().prioridad)
                {
                    other.gameObject.SetActive(false);
                }
                else
                    this.gameObject.SetActive(false);
            }
        }
        if(other.tag == "offSpawner")
        {
            this.gameObject.SetActive(false);
        }
    }
    IEnumerator nuevoAtaque()
    {
        yield return new WaitForSeconds(ataqueEnfriamiento);
        GameObject obj = Spawner.instance.getFuego();
        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = this.transform.position;
        }
        else
            Debug.Log("Error al intentar atacar fuego");
        StartCoroutine(nuevoAtaque());
    }
    public IEnumerator apagadoManual(float time)
    {
        Debug.Log("Apagado Manual:" + time);
        yield return new WaitForSeconds(time);
        Debug.Log("SE APAGO");
        this.gameObject.SetActive(false);
    }
}
