using System.Collections;
using UnityEngine;

public class BossControler : MonoBehaviour
{
    public GameObject[] points;
    private int pointID = 0;

    public float speed = 5f;
    public float timePoint = 2f;

    private bool startCombat = true;
    private bool isWaitingAtPoint = false;
    private float screamDuration = 2.27f;
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

    float speedAnim = distance > 0.1f ? 1f : 0f;
    dragonAnim.SetFloat("Speed", speedAnim);

    if (distance > 0.1f)
    {
        isWaitingAtPoint = false;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
    else
    {
        if (pointID == points.Length - 1 && !bossCollider.enabled) 
        { 
               bossCollider.enabled = true; 
        }

        if (!isWaitingAtPoint)
        {
               
                StartCoroutine(WaitAtPoint());
        }
    }
}
    IEnumerator WaitAtPoint()
    {
        isWaitingAtPoint = true;

        // Gritar UNA vez
        dragonAnim.SetBool("Scream", true);
        Debug.Log("RUGIENDO");
        rugido.Play();

        yield return new WaitForSeconds(screamDuration);
        dragonAnim.SetBool("Scream", false);


        if (pointID >= 1)
        {
            LevelManager.Instance.dificultad += 0.2f;
            buff.SetActive(true);
        }

        yield return new WaitForSeconds(timePoint);

        buff.SetActive(false);

        // Pasar al siguiente punto
        pointID++;
        if (pointID >= points.Length)
            pointID = points.Length - 1;
    }
}
