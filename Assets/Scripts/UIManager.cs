using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gestor de la interfaz de usuario del Buscaminas.
/// Controla menús, contadores, temporizador y mensajes de victoria/derrota.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ========== REFERENCIAS A PANELES ==========
    /// <summary>Panel del menú principal (selección de dificultad)</summary>
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    
    /// <summary>Panel del juego activo</summary>
    [SerializeField] private GameObject gamePanel;
    
    /// <summary>Panel de configuración personalizada</summary>
    [SerializeField] private GameObject customPanel;
    
    /// <summary>Panel de victoria</summary>
    [SerializeField] private GameObject winPanel;

    /// <summary>Panel de derrota</summary>
    [SerializeField] private GameObject losePanel;
    
    /// <summary>Panel de instrucciones "Cómo Jugar"</summary>
    [SerializeField] private GameObject howToPlayPanel;

    // ========== ELEMENTOS DE JUEGO ==========
    /// <summary>Texto que muestra el contador de minas restantes</summary>
    [Header("Game Elements")]
    [SerializeField] private TextMeshProUGUI mineCounterText;
    
    /// <summary>Texto que muestra el tiempo transcurrido</summary>
    [SerializeField] private TextMeshProUGUI timerText;
    
    /// <summary>Botón de reinicio de partida</summary>
    [SerializeField] private Button restartButton;
    
    /// <summary>Botón para volver al menú principal</summary>
    [SerializeField] private Button mainMenuButton;

    // ========== BOTONES DE DIFICULTAD ==========
    /// <summary>Botón para nivel Principiante</summary>
    [Header("Difficulty Buttons")]
    [SerializeField] private Button beginnerButton;
    
    /// <summary>Botón para nivel Intermedio</summary>
    [SerializeField] private Button intermediateButton;
    
    /// <summary>Botón para nivel Experto</summary>
    [SerializeField] private Button expertButton;
    
    /// <summary>Botón para abrir configuración personalizada</summary>
    [SerializeField] private Button customButton;

    // ========== CAMPOS PERSONALIZADOS ==========
    /// <summary>Campo de entrada para el ancho del tablero</summary>
    [Header("Custom Settings")]
    [SerializeField] private TMP_InputField widthInput;
    
    /// <summary>Campo de entrada para el alto del tablero</summary>
    [SerializeField] private TMP_InputField heightInput;
    
    /// <summary>Campo de entrada para el número de minas</summary>
    [SerializeField] private TMP_InputField minesInput;
    
    /// <summary>Botón para iniciar partida personalizada</summary>
    [SerializeField] private Button startCustomButton;
    
    /// <summary>Botón para cancelar configuración personalizada</summary>
    [SerializeField] private Button cancelCustomButton;

    // ========== BOTONES DE RESULTADO ==========
    /// <summary>Botón de reinicio en pantalla de victoria</summary>
    [Header("Result Buttons")]
    [SerializeField] private Button winRestartButton;
    
    /// <summary>Botón para volver al menú desde victoria</summary>
    [SerializeField] private Button winMenuButton;
    
    /// <summary>Botón de reinicio en pantalla de derrota</summary>
    [SerializeField] private Button loseRestartButton;
    
    /// <summary>Botón para volver al menú desde derrota</summary>
    [SerializeField] private Button loseMenuButton;

    /// <summary>Texto de tiempo en pantalla de victoria</summary>
    [SerializeField] private TextMeshProUGUI winTimeText;

    /// <summary>Botón para abrir el panel de instrucciones</summary>
    [SerializeField] private Button howToPlayButton;

    /// <summary>Botón para cerrar el panel de instrucciones</summary>
    [SerializeField] private Button closeHowToPlayButton;

    // ========== REFERENCIAS ==========
    /// <summary>Referencia al GameManager</summary>
    private GameManager gameManager;

    /// <summary>
    /// Inicializa el UIManager y configura los listeners de botones.
    /// </summary>
    private void Start()
    {
        // Obtener referencia al GameManager
        gameManager = FindFirstObjectByType<GameManager>();

        // Configurar listeners de botones de dificultad
        if (beginnerButton != null)
            beginnerButton.onClick.AddListener(() => StartGame(DifficultySettings.Difficulty.Beginner));
        
        if (intermediateButton != null)
            intermediateButton.onClick.AddListener(() => StartGame(DifficultySettings.Difficulty.Intermediate));
        
        if (expertButton != null)
            expertButton.onClick.AddListener(() => StartGame(DifficultySettings.Difficulty.Expert));

        if (customButton != null)
            customButton.onClick.AddListener(ShowCustomPanel);
            
        // Botón "¿Cómo Jugar?"
        if (howToPlayButton != null)
            howToPlayButton.onClick.AddListener(ShowHowToPlayPanel);

        // Botón "Cerrar" dentro del panel
        if (closeHowToPlayButton != null)
            closeHowToPlayButton.onClick.AddListener(HideHowToPlayPanel);

        // Configurar botones de configuración personalizada
        if (startCustomButton != null)
            startCustomButton.onClick.AddListener(StartCustomGame);
        
        if (cancelCustomButton != null)
            cancelCustomButton.onClick.AddListener(HideCustomPanel);

        // Configurar botones de control de juego
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        // Configurar botones de pantallas de resultado
        if (winRestartButton != null)
            winRestartButton.onClick.AddListener(RestartGame);
        
        if (winMenuButton != null)
            winMenuButton.onClick.AddListener(ReturnToMainMenu);
        
        if (loseRestartButton != null)
            loseRestartButton.onClick.AddListener(RestartGame);
        
        if (loseMenuButton != null)
            loseMenuButton.onClick.AddListener(ReturnToMainMenu);

        // Mostrar menú principal al inicio
        ShowMainMenu();
    }

    /// <summary>
    /// Inicia una partida con la dificultad seleccionada.
    /// </summary>
    /// <param name="difficulty">Nivel de dificultad elegido</param>
    private void StartGame(DifficultySettings.Difficulty difficulty)
    {
        if (gameManager != null)
        {
            DifficultySettings settings = new DifficultySettings(difficulty);
            gameManager.StartNewGame(settings);
            ShowGamePanel();
        }
    }

    /// <summary>
    /// Muestra el panel de configuración personalizada.
    /// </summary>
    private void ShowCustomPanel()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (customPanel != null) customPanel.SetActive(true);
        
        // Valores por defecto para campos personalizados
        if (widthInput != null) widthInput.text = "10";
        if (heightInput != null) heightInput.text = "10";
        if (minesInput != null) minesInput.text = "15";
    }

    /// <summary>
    /// Oculta el panel de configuración personalizada.
    /// </summary>
    private void HideCustomPanel()
    {
        if (customPanel != null) customPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Muestra el panel de instrucciones "Cómo Jugar".
    /// </summary>
    private void ShowHowToPlayPanel()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(true);
    }

    /// <summary>
    /// Oculta el panel de instrucciones y vuelve al menú principal.
    /// </summary>
    private void HideHowToPlayPanel()
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Inicia una partida con configuración personalizada.
    /// Valida los valores ingresados por el usuario.
    /// </summary>
    private void StartCustomGame()
    {
        // Obtener y validar valores
        if (int.TryParse(widthInput.text, out int width) &&
            int.TryParse(heightInput.text, out int height) &&
            int.TryParse(minesInput.text, out int mines))
        {
            DifficultySettings settings = new DifficultySettings(width, height, mines);

            // Validar configuración
            if (settings.IsValid())
            {
                if (gameManager != null)
                {
                    gameManager.StartNewGame(settings);
                    ShowGamePanel();
                    if (customPanel != null) customPanel.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Configuración personalizada inválida");
                // Aquí podrías mostrar un mensaje de error al usuario
            }
        }
        else
        {
            Debug.LogWarning("Por favor ingresa valores numéricos válidos");
        }
    }

    /// <summary>
    /// Muestra el menú principal y oculta otros paneles.
    /// </summary>
    public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gamePanel != null) gamePanel.SetActive(false);
        if (customPanel != null) customPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    /// <summary>
    /// Muestra el panel de juego y oculta otros paneles.
    /// </summary>
    public void ShowGamePanel()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(true);
        if (customPanel != null) customPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    /// <summary>
    /// Actualiza el contador de minas en la interfaz.
    /// </summary>
    /// <param name="remainingMines">Número de minas restantes</param>
    public void UpdateMineCounter(int remainingMines)
    {
        if (mineCounterText != null)
        {
            mineCounterText.text = $"Minas: {remainingMines:D3}";
        }
    }

    /// <summary>
    /// Actualiza el temporizador en la interfaz.
    /// </summary>
    /// <param name="seconds">Tiempo transcurrido en segundos</param>
    public void UpdateTimer(int seconds)
    {
        if (timerText != null)
        {
            int minutes = seconds / 60;
            int secs = seconds % 60;
            timerText.text = $"Tiempo: {minutes:D2}:{secs:D2}";
        }
    }

    /// <summary>
    /// Muestra el panel de victoria con el tiempo final.
    /// </summary>
    /// <param name="finalTime">Tiempo total de la partida en segundos</param>
    public void ShowWinPanel(int finalTime)
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            
            if (winTimeText != null)
            {
                int minutes = finalTime / 60;
                int seconds = finalTime % 60;
                winTimeText.text = $"¡Completado en {minutes:D2}:{seconds:D2}!";
            }
        }
    }

    /// <summary>
    /// Muestra el panel de derrota.
    /// </summary>
    public void ShowLosePanel()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }

    /// <summary>
    /// Reinicia la partida actual con la misma configuración.
    /// </summary>
    private void RestartGame()
    {
        if (gameManager != null)
        {
            gameManager.RestartGame();
            
            // Ocultar paneles de resultado
            if (winPanel != null) winPanel.SetActive(false);
            if (losePanel != null) losePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Vuelve al menú principal.
    /// </summary>
    private void ReturnToMainMenu()
    {
        ShowMainMenu();
    }
}