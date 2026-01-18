using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameEvent
{
    GameOver,
    dataChange,
    playerDamage,
    win,
    enemyDestroy,
    powerUpCollected,
    coinCollected,
    boostStart,
    boostEnd,
}

public class PlayerControler : Subject<GameEvent>, IObserver<GameEvent>
{

    public static PlayerControler Instance;
    public GameObject pointSpawn;
    public float speed;
    public GameObject[] points;
    private int pointID = 1;
    private Vector2 moveInput;
    public int life = 3;
    public int puntos = 0;
    public GameObject magicPrifad;
    private GameObject magicObj;
    public float timeMagic;
    public float durationMagic;
    private bool magiaDisponible = true;
    public bool powerUpActive = false;
    public int powerUpCount = 0;
    public PlayerAnimation playerAnim; //Referencia al componente PlayerAnimation
    private Animator playerAnimator;
    public int enemigosDerrotados;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        magicObj = Instantiate(magicPrifad);
        magicObj.SetActive(false);
        AddObserver(UIManager.Instance);
        AddObserver(AudioManager.Instance);
        UIManager.Instance.UpdateUIPLayerData(puntos,life,powerUpCount);
        playerAnimator = GetComponent<Animator>();
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
    public void attack(InputAction.CallbackContext context)
    {
        if (context.performed && magiaDisponible)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        magiaDisponible = false;

        // Lanzar animación
        playerAnimator.SetTrigger("Attack");

        // Espera hasta el frame donde sale el hechizo
        yield return new WaitForSeconds(0.75f);   

        // Disparo real
        magicObj.SetActive(true);
        magicObj.transform.position = pointSpawn.transform.position;
        StartCoroutine(magicObj.GetComponent<moveObs>().apagadoManual(durationMagic));

        // Cooldown
        yield return new WaitForSeconds(timeMagic);
        magiaDisponible = true;
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
                    Notify(GameEvent.dataChange, new int[] { puntos, life, enemigosDerrotados, powerUpCount });
                    Notify(GameEvent.coinCollected);
                    other.gameObject.SetActive(false);
                    break;
                case moveObs.obsType.obstaculo:
                    life--;
                    Notify(GameEvent.dataChange, new int[] { puntos, life, enemigosDerrotados, powerUpCount });
                    Notify(GameEvent.playerDamage);
                    if(life <= 0)
                    {
                        Notify(GameEvent.GameOver);
                    }
                    other.gameObject.SetActive(false);
                    break;
                case moveObs.obsType.enemigo:
                    life--;
                    Notify(GameEvent.dataChange, new int[] { puntos, life, enemigosDerrotados, powerUpCount });
                    Notify(GameEvent.playerDamage);
                    if (life <= 0)
                    {
                        Notify(GameEvent.GameOver);
                    }
                    other.gameObject.SetActive(false);
                    break;
                case moveObs.obsType.enemigoAtaque:
                    life--;
                    Notify(GameEvent.dataChange, new int[] { puntos, life, enemigosDerrotados, powerUpCount });
                    Notify(GameEvent.playerDamage);
                    if (life <= 0)
                    {
                        Notify(GameEvent.GameOver);
                    }
                    other.gameObject.SetActive(false);
                    break;
                case moveObs.obsType.powerUp:
                    powerUpCount++;
                    Notify(GameEvent.dataChange, new int[] { puntos, life, enemigosDerrotados, powerUpCount });
                    Notify(GameEvent.powerUpCollected, null);
                    other.gameObject.SetActive(false);
                    break;
            }
        }
    }
    public void OnNotify(GameEvent gameEvent, object data)
    {
        switch (gameEvent)
        {
            case GameEvent.GameOver:
                break;
            case GameEvent.dataChange:
                break;
            case GameEvent.playerDamage:
                break;
            case GameEvent.win:
                break;
            case GameEvent.enemyDestroy:
                puntos += 5;
                Notify(GameEvent.dataChange, new int[] { puntos, life, enemigosDerrotados, powerUpCount });
                break;
        }
    }

 
    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed && powerUpCount > 0 && !powerUpActive)
        {
            Debug.Log("Boost Activado");
            StartCoroutine(BoostCoroutine());
        }
    }

    IEnumerator BoostCoroutine()
    {
        powerUpActive = true;
        powerUpCount--;

        Notify(GameEvent.dataChange, new int[] { puntos, life, enemigosDerrotados, powerUpCount });
        Notify(GameEvent.boostStart);
        speed *= 2f;
        playerAnim.isBoosting = true;

        yield return new WaitForSeconds(5f);

        Notify(GameEvent.boostEnd);
        speed /= 2f;
        playerAnim.isBoosting = false;
        powerUpActive = false;
    }

}
