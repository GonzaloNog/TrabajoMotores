using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameEvent
{
    GameOver,
    dataChange,
    playerDamage,
    win
}

public class PlayerControler : Subject<GameEvent>
{


    public float speed; 
    public GameObject[] points;
    private int pointID = 1;
    private Vector2 moveInput;
    public int life = 3;
    public int puntos = 0;
    public GameObject magicPrifad;
    private GameObject magicObj;
    public float timeMagic;
    private bool magiaDisponible = true;

    private void Start()
    {
        magicObj = Instantiate(magicPrifad);
        magicObj.SetActive(false);
        AddObserver(UIManager.Instance);
        AddObserver(AudioManager.Instance);
        UIManager.Instance.UpdateUIPLayerData(puntos,life);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[pointID].transform.position, speed * Time.deltaTime);//Mover el player a un punto concreto usando una velocidad 
        //movePlayerInput();
    }
    //Evento que invocamos de Player Input para control movimiento
    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        //Notify(GameEvent.dataChange, new int[] { puntos, life });
        Vector2 inputVector = context.ReadValue<Vector2>();

        float horizontalInput = inputVector.x;

        if (context.performed)
        {
            //Debug.Log(horizontalInput);
            if(horizontalInput == 1)
            {
                if (pointID > 0)
                {
                    //Debug.Log("move-");
                    pointID--;
                }
            }
            else if(horizontalInput == -1)
            {
                if (pointID < points.Length - 1)
                {
                    //Debug.Log("move+");
                    pointID++;
                }
            }
        }
    }
    public void ataque(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Ataque");
            if (magiaDisponible)
            {
                magicObj.SetActive(true);
                magicObj.transform.position = this.transform.position;
                StartCoroutine(magicObj.GetComponent<moveObs>().apagadoManual(1));
                StartCoroutine(esperaMagia());
            }
        }

    }
    IEnumerator esperaMagia()
    {
        magiaDisponible = false;
        yield return new WaitForSeconds(timeMagic);
        magiaDisponible = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "spawner")
        {
            moveObs mov = other.GetComponent<moveObs>();
            switch (mov.type)
            {
                case moveObs.obsType.premio:
                    puntos++;
                    Notify(GameEvent.dataChange, new int[] { puntos, life });
                    other.gameObject.SetActive(false);
                    break;
                case moveObs.obsType.obstaculo:
                    Notify(GameEvent.dataChange, new int[] { puntos, life });
                    Notify(GameEvent.playerDamage);
                    life--;
                    if(life <= 0)
                    {
                        Notify(GameEvent.GameOver);
                    }
                    other.gameObject.SetActive(false);
                    break;
                case moveObs.obsType.enemigo:
                    Notify(GameEvent.dataChange, new int[] { puntos, life });
                    Notify(GameEvent.playerDamage);
                    life--;
                    if (life <= 0)
                    {
                        Notify(GameEvent.GameOver);
                    }
                    other.gameObject.SetActive(false);
                    break;
                case moveObs.obsType.enemigoAtaque:
                    Notify(GameEvent.dataChange, new int[] { puntos, life });
                    Notify(GameEvent.playerDamage);
                    life--;
                    if (life <= 0)
                    {
                        Notify(GameEvent.GameOver);
                    }
                    other.gameObject.SetActive(false);
                    break;
            }
        }
    }

}
