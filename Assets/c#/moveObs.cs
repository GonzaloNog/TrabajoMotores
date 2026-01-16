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
        playerAtaque,
        escenario,
        powerUp
    }

    public obsType type;
    public float speed;
    public int prioridad;
    public float ataqueEnfriamiento;
    public bool resetAtaque = false;
    public float direccion;
    public GameObject buff;
    public Animator animatorEnemyDragon;

    private void Start()
    {
        AddObserver(PlayerControler.Instance);
        AddObserver(UIManager.Instance);
        if(type != obsType.playerAtaque && type != obsType.escenario)
        {
            buff.SetActive(false);
        }
    }
    void Update()
    {
        if (LevelManager.Instance.dificultad > 1 && buff != null)
        {
            buff.SetActive(true);
        }
        if (this.gameObject.activeInHierarchy)
        {
            this.transform.Translate(new Vector3(0,0,direccion) * speed * LevelManager.Instance.dificultad * Time.deltaTime);
        }
        if (type == obsType.enemigo && resetAtaque)
        {
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
                    Notify(GameEvent.enemyDestroy);
                    PlayerControler.Instance.enemigosDerrotados++;
                    other.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                }
                if (other.GetComponent<moveObs>().type == obsType.enemigoAtaque)
                {
                    other.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                }
            }
            if(other.tag == "Boss")
            {
                Notify(GameEvent.win);
            }
        }
        else if(other.tag == "spawner")
        {
            if (other.GetComponent<moveObs>().type != obsType.enemigoAtaque && type != obsType.enemigoAtaque)
            {
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

        if (animatorEnemyDragon != null) { 
            animatorEnemyDragon.SetTrigger("Attack"); 
        } 
        GameObject obj = Spawner.instance.getFuego(); 

        if (obj != null) 
        { 
            obj.SetActive(true);
            obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z + 6); 
        } 
        else 
            StartCoroutine(nuevoAtaque()); 
    }

    public IEnumerator apagadoManual(float time)
    {
       
        yield return new WaitForSeconds(time);
       
        this.gameObject.SetActive(false);
    }

}
