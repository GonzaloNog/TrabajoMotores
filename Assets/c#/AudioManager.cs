using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour, IObserver<GameEvent>
{
   
    public static AudioManager Instance;
    //controlador de audio
    private AudioSource m_AudioSource;
    //Controlador de musica
    public AudioSource musicSource;
    //clips de audio 
    public AudioClip damage;
    public AudioClip points;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip powerUp;
    public AudioClip speedUp;
    public AudioClip speedDown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        //referencia del AudioSource
        m_AudioSource = GetComponent<AudioSource>();
        if (!musicSource.isPlaying)
            musicSource.Play();

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinicia la música de fondo
        musicSource.Stop();
        musicSource.time = 0f;
        musicSource.Play();
    }
    public void OnNotify(GameEvent gameEvent, object data)
    {
        switch (gameEvent)
        {
            case GameEvent.GameOver:
                musicSource.Stop();
                m_AudioSource.PlayOneShot(lose);
                break;

            case GameEvent.powerUpCollected:
                m_AudioSource.PlayOneShot(powerUp);
                break;

            case GameEvent.playerDamage:
                m_AudioSource.PlayOneShot(damage);
                break;

            case GameEvent.win:
                Debug.Log("Win sound played");
                musicSource.Stop();
                m_AudioSource.PlayOneShot(win);
                break;

            case GameEvent.coinCollected:
                m_AudioSource.PlayOneShot(points);
                break;

            case GameEvent.boostStart:
                m_AudioSource.PlayOneShot(speedUp);
                break;

            case GameEvent.boostEnd:
                m_AudioSource.PlayOneShot(speedDown);
                break;
        }
    }
}
