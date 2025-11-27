using UnityEngine;

public class AudioManager : MonoBehaviour, IObserver<GameEvent>
{
   
    public static AudioManager Instance;
    //controlador de audio
    private AudioSource m_AudioSource;
    //clips de audio 
    public AudioClip damage;
    public AudioClip points;
    public AudioClip win;
    public AudioClip lose;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //referencia del AudioSource
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void OnNotify(GameEvent gameEvent, object data)
    {
        Debug.Log("Nuevo Audio SFX");
        m_AudioSource.Stop();
        switch (gameEvent)
        {
            case GameEvent.GameOver:
                m_AudioSource.clip = lose;
                break;
            case GameEvent.dataChange:
                m_AudioSource.clip = points;
                break;
            case GameEvent.playerDamage:
                m_AudioSource.clip = damage;
                break;
            case GameEvent.win:
                m_AudioSource.clip = win;
                break;
        }
        m_AudioSource.Play();

    }
}
