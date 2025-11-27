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

    private void Start()
    {
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
    public void movePlayerInput()
    {
        /*
        //Input
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            
        }*/

        //Animation 
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "spawner")
        {
            moveObs mov = other.GetComponent<moveObs>();
            switch (mov.type)
            {
                case moveObs.obsType.premio:
                    Notify(GameEvent.dataChange, new int[] { puntos, life });
                    LevelManager.Instance.points++;
                    Destroy(other.gameObject);
                    break;
                case moveObs.obsType.obstaculo:
                    Notify(GameEvent.dataChange, new int[] { puntos, life });
                    Notify(GameEvent.playerDamage);
                    life--;
                    if(life <= 0)
                    {
                        Notify(GameEvent.GameOver);
                    }
                    Destroy(other.gameObject);
                    break;
                case moveObs.obsType.enemigo:

                    break;
            }
        }
    }

}
