using System.Collections;
using UnityEngine;

public class BossControler : MonoBehaviour
{
    public GameObject[] points;
    private int pointID = 0;

    public float speed = 5f;
    public float timePoint = 2f;

    private bool startCombat = true;
    private bool waiting = false;
    private Collider bossCollider;

    public GameObject buff;
    public Animator dragonAnim;
    private AudioSource rugido;

    void Start()
    {
        buff.SetActive(false);
        rugido = GetComponent<AudioSource>();
        bossCollider = GetComponent<Collider>();
        bossCollider.enabled = false;
    }

    void Update()
    {
        if (!startCombat) return;

        Vector3 target = points[pointID].transform.position;
        float distance = Vector3.Distance(transform.position, target);

        bool isMoving = distance > 0.1f;
        dragonAnim.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            if (pointID == points.Length - 1 && !bossCollider.enabled)
            {
                bossCollider.enabled = true;
            }

            if (!waiting)
            {
                StartCoroutine(WaitAndMove());
            }
        }
    }

    IEnumerator WaitAndMove()
    {
        waiting = true;

        // Rugido
        dragonAnim.SetTrigger("Scream");
        rugido.Play();

        // Solo a partir del segundo punto
        if (pointID >= 1)
        {
            LevelManager.Instance.dificultad += 0.2f;
            buff.SetActive(true);
        }

        yield return new WaitForSeconds(timePoint);

        buff.SetActive(false);

        pointID++;
        if (pointID >= points.Length)
            pointID = points.Length - 1;
         


        waiting = false;
    }
}
