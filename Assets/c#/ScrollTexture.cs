using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeed = 2f; // Velocidad de desplazamiento de la textura
    private Renderer rend; // Componente Renderer del objeto
    private Vector2 offset; // Offset de la textura


    void Start()
    {
        rend = GetComponent<Renderer>(); // Obtener el componente Renderer del objeto
    }

    // Update is called once per frame
    void Update()
    {
        // Calcular el nuevo offset de la textura
        offset.y -= Time.deltaTime * scrollSpeed;
        rend.material.mainTextureOffset = offset; // Aplicar el offset a la textura principal del material
    }
}
