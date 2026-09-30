using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Parámetros del Juego")]
    public float tiempoLimite = 60f;
    public int paquetesRequeridos = 10;

    [Header("Paneles UI")]
    public GameObject mainMenuCanvas;
    public GameObject hudCanvas;
    public GameObject gameOverCanvas;

    [Header("Elementos HUD")]
    public TextMeshProUGUI textoTiempoHud;
    public TextMeshProUGUI textoPaquetesHud;

    [Header("Elementos Game Over")]
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoPaquetesFinal;
    public TextMeshProUGUI textoTiempoFinal;

    private float tiempoRestante;
    private int paquetesRecolectados;
    private bool juegoActivo = false; // Controla si el contador está corriendo

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        MostrarMenuPrincipal();
    }

    private void Update()
    {
        // Solo descuenta el tiempo si el juego se activó al spawnear el carro
        if (!juegoActivo) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarHUD();

            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                FinalizarJuego(false); // Perdió por tiempo
            }
        }
    }

    /// <summary>
    /// Se llama desde el botón "Jugar" del menú o "Reiniciar".
    /// Prepara las variables pero NO arranca el contador todavía.
    /// </summary>
    public void PrepararJuego()
    {
        juegoActivo = false; // Mantiene el temporizador pausado
        tiempoRestante = tiempoLimite;
        paquetesRecolectados = 0;

        mainMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        hudCanvas.SetActive(true);

        ActualizarHUD();
        FindObjectOfType<CarManager>()?.ResetearCarro();
    }

    /// <summary>
    /// ¡LLAMA A ESTA FUNCIÓN JUSTO CUANDO SPAWNEE EL CARRO!
    /// </summary>
    public void ComenzarConteo()
    {
        juegoActivo = true; // Activa el Update para que el temporizador comience
    }

    public void RecolectarPaquete()
    {
        if (!juegoActivo) return;

        paquetesRecolectados++;
        ActualizarHUD();

        if (paquetesRecolectados >= paquetesRequeridos)
        {
            FinalizarJuego(true); // Ganó
        }
    }

    private void ActualizarHUD()
    {
        textoTiempoHud.text = $"Tiempo: {Mathf.CeilToInt(tiempoRestante)}s";
        textoPaquetesHud.text = $"Paquetes: {paquetesRecolectados} / {paquetesRequeridos}";
    }

    private void FinalizarJuego(bool gano)
    {
        juegoActivo = false;

        hudCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);

        float tiempoEmpleado = tiempoLimite - tiempoRestante;

        textoResultado.text = gano ? "¡VICTORIA!" : "¡DERROTA!";
        textoPaquetesFinal.text = $"Paquetes: {paquetesRecolectados} / {paquetesRequeridos}";
        textoTiempoFinal.text = $"Tiempo: {Mathf.RoundToInt(tiempoEmpleado)}s";
    }

    public void ReiniciarJuego()
    {
        PrepararJuego();
    }

    public void MostrarMenuPrincipal()
    {
        juegoActivo = false;

        mainMenuCanvas.SetActive(true);
        hudCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }
    public bool IsJuegoActivo()
{
    return juegoActivo;
}
}