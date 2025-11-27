using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IObserver<GameEvent>
{
    public static UIManager Instance;
    public TextMeshProUGUI life;
    public TextMeshProUGUI points;
    public GameObject damage;

    public void Awake()
    {
        Instance = this;
    }
    public void OnNotify(GameEvent gameEvent, object data)
    {
        Debug.Log("Evento detectado en la UI");
        switch (gameEvent)
        {
            case GameEvent.GameOver:
                break;
            case GameEvent.dataChange:
                if (data is int[] arr)
                {
                    int points = arr[0];
                    int lifes = arr[1];
                    UpdateUIPLayerData(points, lifes);
                }
                break;
            case GameEvent.playerDamage:
                StartCoroutine(newDamage());//ejecutamos una corrutina concreta
                break;
            case GameEvent.win:
                break;
        }
    }
    public void UpdateUIPLayerData(int _points, int _lifes)
    {
        life.text = _lifes.ToString();
        points.text = _points.ToString();   
    }
    //corrutina, se ejecuta en paralelo, nos permite esperar tiempo concreto entre lineas de codigo
    public IEnumerator newDamage()
    {
        damage.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        damage.SetActive(false);
    }
}
