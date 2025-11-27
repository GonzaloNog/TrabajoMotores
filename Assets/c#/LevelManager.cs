using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int points;
    private void Awake()
    {
        Instance = this;
    }
}
