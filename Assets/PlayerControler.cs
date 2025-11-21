using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public float speed;
    public GameObject[] points;
    private int pointID = 1;
    private Vector2 moveInput;
    //public KeyCode izquierda;
    //public KeyCode derecha;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[1].transform.position, speed * Time.deltaTime);//Mover el player a un punto concreto usando una velocidad 
        //movePlayerInput();
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    public void movePlayerInput()
    {
        /*
        //Input
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(pointID < points.Length)
                pointID++;  
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (pointID > 0)
                pointID--;
        }*/

        //Animation 
    }
}
