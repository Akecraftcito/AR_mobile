using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/**
 * Spawns a <see cref="CarBehaviour"/> when a plane is tapped.
 */
public class CarManager : MonoBehaviour
{
    public GameObject CarPrefab;
    public ReticleBehaviour Reticle;
    public DrivingSurfaceManager DrivingSurfaceManager;

    public CarBehaviour Car;

    private void Update()
    {
        if (Car == null && WasTapped() && Reticle.CurrentPlane != null)
        {
            // Spawn our car at the reticle location.
            var obj = GameObject.Instantiate(CarPrefab);
            Car = obj.GetComponent<CarBehaviour>();
            Car.Reticle = Reticle;
            Car.transform.position = Reticle.transform.position;
            DrivingSurfaceManager.LockPlane(Reticle.CurrentPlane);

            // -------------------------------------------------------------
            // LÍNEA AÑADIDA: Inicia el contador en el GameManager
            // -------------------------------------------------------------
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ComenzarConteo();
            }
        }
    }

    /// <summary>
    /// Llama a esta función desde el GameManager para destruir el carro al reiniciar o ir al menú.
    /// </summary>
    public void ResetearCarro()
    {
        if (Car != null)
        {
            Destroy(Car.gameObject);
            Car = null;
        }
    }

    private bool WasTapped()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        if (Input.touchCount == 0)
        {
            return false;
        }

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
        {
            return false;
        }

        return true;
    }
}