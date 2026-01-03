using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnim;
    [SerializeField] private float dampTime = 0.2f;

    public bool isBoosting;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        // Empieza corriendo normal
        playerAnim.SetFloat("Speed", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRunBlend();
    }

    void UpdateRunBlend()
    {
        // 0 = Run, 1 = FastRun
        float targetSpeed = isBoosting ? 1f : 0f;

        // Transición suave en el Blend Tree
        playerAnim.SetFloat("Speed", targetSpeed, dampTime, Time.deltaTime);
    }

}
