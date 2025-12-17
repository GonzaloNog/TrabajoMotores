using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public float dificultad = 1;
    private void Awake()
    {
        Instance = this;
    }
}
