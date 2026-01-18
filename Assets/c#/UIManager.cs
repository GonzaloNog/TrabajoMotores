using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IObserver<GameEvent>
{
    public static UIManager Instance;
    public TextMeshProUGUI life;
    public TextMeshProUGUI points;
    public TextMeshProUGUI powerUps;
    public GameObject damage;
    public GameObject panelWin;
    public GameObject panelLose;
    public GameObject panelLogros;
    public TextMeshProUGUI panelLogrosTexto;
    public Image logroPuntosImage;
    public Image logroMatadragonesImage;
    private bool enemigosB;
    private bool pointB;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        enemigosB = false;
        pointB = false;
        panelLogros.SetActive(false);
        panelWin.SetActive(false);   
        panelLose.SetActive(false); 
        logroPuntosImage.gameObject.SetActive(false);
        logroMatadragonesImage.gameObject.SetActive(false);
    }
    public void OnNotify(GameEvent gameEvent, object data)
    {
        
        switch (gameEvent)
        {
            case GameEvent.GameOver:
                Time.timeScale = 0;
                panelLose.SetActive(true);
                break;
            case GameEvent.dataChange:
                if (data is int[] arr)
                {
                    int points = arr[0];
                    int lifes = arr[1];
                    int enemigos = arr[2];
                    int powerUps = arr[3];
                    nuevoLogro(points, enemigos);
                    UpdateUIPLayerData(points, lifes, powerUps);
                }
                break;
            case GameEvent.playerDamage:
                StartCoroutine(newDamage());//ejecutamos una corrutina concreta
                break;
            case GameEvent.win:
                Debug.Log("GANO");
                Time.timeScale = 0;
                panelWin.SetActive(true);
                break;
        }
    }
    public void nuevoLogro(int puntos, int enemigos)
    {
        if(puntos > 100 && !pointB)
        {
            pointB = true;
            panelLogros.SetActive(true);
            logroPuntosImage.gameObject.SetActive(true);
            panelLogrosTexto.text = "Nuevo Logro: +100 monedas";
            StartCoroutine(nuevoLogroUI());
        }
        if (enemigos > 10 && !enemigosB)
        {
            enemigosB = true;
            panelLogros.SetActive(true);
            logroMatadragonesImage.gameObject.SetActive(true);
            panelLogrosTexto.text = "Nuevo Logro: Cazador de dragones";
            StartCoroutine(nuevoLogroUI());
        }
    }
    public IEnumerator nuevoLogroUI()
    {
        yield return new WaitForSeconds(3);
        panelLogros.SetActive(false);
        logroMatadragonesImage.gameObject.SetActive(false);
        logroPuntosImage.gameObject.SetActive(false);

    }
    public void UpdateUIPLayerData(int _points, int _lifes, int _powerUps)
    {
        if (life.text != _lifes.ToString())
            life.text = _lifes.ToString();

        if (points.text != _points.ToString())
            points.text = _points.ToString();

        if (powerUps.text != _powerUps.ToString())
            powerUps.text = _powerUps.ToString();
    }
    //corrutina, se ejecuta en paralelo, nos permite esperar tiempo concreto entre lineas de codigo
    public IEnumerator newDamage()
    {
        damage.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        damage.SetActive(false);
    }
    public void newSceneLoad(int a)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(a);
    }
}
