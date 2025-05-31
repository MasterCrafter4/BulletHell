using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;   // Velocidad de la bala
    
    void Update()
    {
        // Mueve la bala en la dirección de su "up" local (hacia adelante)
        transform.position += transform.up * speed * Time.deltaTime;
    }

    // Este callback se invoca cuando el renderer deja de ser visible por cualquier cámara
    // (p.ej. cuando la bala sale de la vista de la cámara principal).
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}