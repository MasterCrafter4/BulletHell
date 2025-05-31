using UnityEngine;
using System.Collections;

public class jonathan : MonoBehaviour
{
    [Header("Movimiento ZigZag")]
    [SerializeField] private float velocidad = 1f;    // Velocidad de movimiento diagonal
    [SerializeField] private float distanciaX = 1f;    // Alcance del zigzag en el eje X

    [Header("Disparo de Patrones")]
    [SerializeField] public GameObject bulletPrefab;  // Prefab de la bala 
    [SerializeField] public float fireRate = 1f;      // Tiempo en segundos entre cada “volley” de disparo en el patrón

    private void Start()
    {
        // Iniciamos simultáneamente la corutina de movimiento y la de disparo
        StartCoroutine(MoveZigZag());
        StartCoroutine(ShootPatterns());
    }

    // -------------------------------------------------------------
    // Se mantiene exactamente tu MoveZigZag original (sólo reduje velocidad/distancia).
    // -------------------------------------------------------------
    private IEnumerator MoveZigZag()
    {
        Vector3 startPos = transform.position;
        
        while (true)
        {
            // Movimiento hacia la derecha y arriba
            while (transform.position.x < startPos.x + distanciaX)
            {
                transform.position += new Vector3(velocidad * Time.deltaTime, velocidad * Time.deltaTime, 0);
                yield return null;
            }
            
            // Movimiento hacia la derecha y abajo
            while (transform.position.x < startPos.x + (distanciaX * 2f))
            {
                transform.position += new Vector3(velocidad * Time.deltaTime, -velocidad * Time.deltaTime, 0);
                yield return null;
            }
            
            // Movimiento hacia la izquierda y arriba
            while (transform.position.x > startPos.x + distanciaX)
            {
                transform.position += new Vector3(-velocidad * Time.deltaTime, velocidad * Time.deltaTime, 0);
                yield return null;
            }
            
            // Movimiento hacia la izquierda y abajo
            while (transform.position.x > startPos.x)
            {
                transform.position += new Vector3(-velocidad * Time.deltaTime, -velocidad * Time.deltaTime, 0);
                yield return null;
            }
        }
    }

    // -------------------------------------------------------------
    // Corutina principal de disparo: alterna círculo, cuadrado y triángulo
    // -------------------------------------------------------------
    private IEnumerator ShootPatterns()
    {
        // Bucle infinito: repetimos los tres patrones en orden
        while (true)
        {
            yield return StartCoroutine(CirclePattern(10f));    // 10 seg de patrón "círculo"
            yield return StartCoroutine(SquarePattern(10f));    // 10 seg de patrón "cuadrado"
            yield return StartCoroutine(TrianglePattern(10f));  // 10 seg de patrón "triángulo"
        }
    }

    // -------------------------------------------------------------
    // Patrón CÍRCULO: durante 'duration' segundos dispara anillos de 'nBullets' balas
    // -------------------------------------------------------------
    private IEnumerator CirclePattern(float duration)
    {
        float endTime = Time.time + duration;
        int nBullets = 20;       // ¿Cuántas balas por anillo?
        
        while (Time.time < endTime)
        {
            // Disparo de un anillo completo: nBullets balas equiespaciadas
            for (int i = 0; i < nBullets; i++)
            {
                float angleDeg = i * (360f / nBullets);
                float angleRad = angleDeg * Mathf.Deg2Rad;
                
                // Rotación para que la bala “mire” en esa dirección
                Quaternion rot = Quaternion.Euler(0f, 0f, angleDeg);
                
                // Instanciamos la bala en la posición del boss
                Instantiate(bulletPrefab, transform.position, rot);
            }
            
            // Esperamos un intervalo antes de disparar el siguiente anillo
            yield return new WaitForSeconds(fireRate);
        }
    }

    // -------------------------------------------------------------
    // Patrón CUADRADO: durante 'duration' segundos dispara en 4 direcciones diagonales
    // (ángulos: 45°, 135°, 225° y 315°)
    // -------------------------------------------------------------
    private IEnumerator SquarePattern(float duration)
    {
        float endTime = Time.time + duration;
        // Definimos los 4 ángulos en grados para un “patrón cuadrado” (diagonales)
        float[] angles = new float[] { 45f, 135f, 225f, 315f };
        
        while (Time.time < endTime)
        {
            foreach (float angleDeg in angles)
            {
                Quaternion rot = Quaternion.Euler(0f, 0f, angleDeg);
                Instantiate(bulletPrefab, transform.position, rot);
            }
            
            yield return new WaitForSeconds(fireRate);
        }
    }

    // -------------------------------------------------------------
    // Patrón TRIÁNGULO: durante 'duration' segundos dispara en 3 ángulos (0°, 120°, 240°)
    // -------------------------------------------------------------
    private IEnumerator TrianglePattern(float duration)
    {
        float endTime = Time.time + duration;
        float[] angles = new float[] { 0f, 120f, 240f };
        
        while (Time.time < endTime)
        {
            foreach (float angleDeg in angles)
            {
                Quaternion rot = Quaternion.Euler(0f, 0f, angleDeg);
                Instantiate(bulletPrefab, transform.position, rot);
            }
            
            yield return new WaitForSeconds(fireRate);
        }
    }
}