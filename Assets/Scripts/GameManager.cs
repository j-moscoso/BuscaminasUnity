using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador principal del juego Buscaminas.
/// Gestiona la lógica del juego, la matriz dispersa y la interacción con la UI.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ========== REFERENCIAS UI ==========
    /// <summary>Referencia al UIManager</summary>
    [Header("References")]
    [SerializeField] private UIManager uiManager;
    
    /// <summary>Contenedor donde se generarán las celdas del tablero</summary>
    [SerializeField] private Transform boardContainer;
    
    /// <summary>Prefab de la celda para instanciar</summary>
    [SerializeField] private GameObject cellPrefab;
    
    /// <summary>GridLayoutGroup para organizar las celdas</summary>
    [SerializeField] private GridLayoutGroup gridLayout;

    // ========== CONFIGURACIÓN DE TAMAÑO DE CELDAS ==========
    /// <summary>Tamaño base de cada celda en píxeles</summary>
    [Header("Cell Settings")]
    [SerializeField] private float baseCellSize = 40f;
    
    /// <summary>Espaciado entre celdas</summary>
    [SerializeField] private float cellSpacing = 2f;

    // ========== DATOS DEL JUEGO ==========
    /// <summary>Matriz dispersa que representa el tablero</summary>
    private SparseMatrix gameBoard;
    
    /// <summary>Array 2D de celdas visuales</summary>
    private Cell[,] cells;
    
    /// <summary>Configuración de dificultad actual</summary>
    private DifficultySettings currentSettings;
    
    /// <summary>Indica si el juego ha terminado</summary>
    public bool IsGameOver { get; private set; }
    
    /// <summary>Número de banderas colocadas</summary>
    private int flagCount;
    
    /// <summary>Tiempo transcurrido en segundos</summary>
    private int elapsedTime;
    
    /// <summary>Indica si el temporizador está activo</summary>
    private bool isTimerRunning;

    /// <summary>
    /// Inicialización del GameManager.
    /// </summary>
    private void Start()
    {
        IsGameOver = false;
        
        // Si no hay referencia al UIManager, buscarla
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }
    }

    /// <summary>
    /// Inicia una nueva partida con la configuración especificada.
    /// </summary>
    /// <param name="settings">Configuración de dificultad</param>
    public void StartNewGame(DifficultySettings settings)
    {
        Debug.Log("=== INICIANDO NUEVO JUEGO ===");
        Debug.Log($"Configuración: {settings.Width}x{settings.Height}, {settings.Mines} minas");
        Debug.Log($"Cell Prefab: {(cellPrefab != null ? "OK" : "NULL")}");
        Debug.Log($"Board Container: {(boardContainer != null ? "OK" : "NULL")}");
        Debug.Log($"UI Manager: {(uiManager != null ? "OK" : "NULL")}");
            
        
        
        // Guardar configuración
        currentSettings = settings;
        
        // Limpiar tablero anterior si existe
        ClearBoard();
        
        // Crear nueva matriz dispersa
        gameBoard = new SparseMatrix(settings.Width, settings.Height, settings.Mines);
        
        // Generar tablero visual
        GenerateBoard();
        
        // Reiniciar estado del juego
        IsGameOver = false;
        flagCount = 0;
        elapsedTime = 0;
        isTimerRunning = true;
        
        // Actualizar UI
        if (uiManager != null)
        {
            uiManager.UpdateMineCounter(settings.Mines);
            uiManager.UpdateTimer(0);
        }
        
        // Iniciar temporizador
        StartCoroutine(TimerCoroutine());
        
        Debug.Log($"Nueva partida iniciada: {settings.GetDescription()}");
    }

    /// <summary>
    /// Genera el tablero visual basado en la matriz dispersa.
    /// Instancia las celdas y configura el GridLayoutGroup.
    /// </summary>
    private void GenerateBoard()
    {
        if (cellPrefab == null || boardContainer == null)
        {
            Debug.LogError("Falta asignar cellPrefab o boardContainer en el inspector");
            return;
        }

        // Configurar GridLayoutGroup
        if (gridLayout != null)
        {
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = currentSettings.Width;
            
            // Calcular tamaño de celda dinámicamente
            float cellSize = CalculateCellSize();
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.spacing = new Vector2(cellSpacing, cellSpacing);
        }

        // Crear array de celdas
        cells = new Cell[currentSettings.Width, currentSettings.Height];

        // Generar celdas
        for (int y = 0; y < currentSettings.Height; y++)
        {
            for (int x = 0; x < currentSettings.Width; x++)
            {
                // Instanciar celda
                GameObject cellObj = Instantiate(cellPrefab, boardContainer);
                Cell cell = cellObj.GetComponent<Cell>();
                
                if (cell != null)
                {
                    // Obtener nodo de la matriz dispersa
                    SparseMatrixNode node = gameBoard.GetNode(x, y);
                    
                    // Configurar celda
                    cell.Setup(x, y, node, this);
                    cells[x, y] = cell;
                }
            }
        }
    }

    /// <summary>
    /// Calcula el tamaño óptimo de celda según el tamaño del tablero.
    /// Tableros más grandes tendrán celdas más pequeñas.
    /// </summary>
    /// <returns>Tamaño de celda en píxeles</returns>
    private float CalculateCellSize()
    {
        // Tableros grandes necesitan celdas más pequeñas
        int maxDimension = Mathf.Max(currentSettings.Width, currentSettings.Height);
        
        if (maxDimension <= 10)
            return baseCellSize;
        else if (maxDimension <= 16)
            return baseCellSize * 0.75f;
        else if (maxDimension <= 20)
            return baseCellSize * 0.6f;
        else
            return baseCellSize * 0.5f;
    }

    /// <summary>
    /// Limpia el tablero actual destruyendo todas las celdas.
    /// </summary>
    private void ClearBoard()
    {
        if (boardContainer != null)
        {
            // Destruir todas las celdas hijas
            foreach (Transform child in boardContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
        cells = null;
        gameBoard = null;
    }

    /// <summary>
    /// Maneja el clic izquierdo en una celda (revelar).
    /// </summary>
    /// <param name="x">Coordenada X de la celda</param>
    /// <param name="y">Coordenada Y de la celda</param>
    public void OnCellLeftClick(int x, int y)
    {
        if (IsGameOver) return;

        // Revelar celda en la matriz dispersa
        bool hitMine = gameBoard.RevealCell(x, y);

        // Actualizar visualización de todas las celdas afectadas
        UpdateBoardVisuals();

        // Si tocó una mina, fin del juego
        if (hitMine)
        {
            GameOver(false);
        }
        // Verificar victoria
        else if (gameBoard.CheckWin())
        {
            GameOver(true);
        }
    }

    /// <summary>
    /// Maneja el clic derecho en una celda (marcar/desmarcar bandera).
    /// </summary>
    /// <param name="x">Coordenada X de la celda</param>
    /// <param name="y">Coordenada Y de la celda</param>
    public void OnCellRightClick(int x, int y)
    {
        if (IsGameOver) return;

        SparseMatrixNode node = gameBoard.GetNode(x, y);
        
        if (node != null && !node.IsRevealed)
        {
            // Cambiar estado de bandera
            bool wasFlaged = node.IsFlagged;
            gameBoard.ToggleFlag(x, y);
            
            // Actualizar contador de banderas
            if (node.IsFlagged && !wasFlaged)
            {
                flagCount++;
            }
            else if (!node.IsFlagged && wasFlaged)
            {
                flagCount--;
            }
            
            // Actualizar contador de minas en UI
            if (uiManager != null)
            {
                int remainingMines = currentSettings.Mines - flagCount;
                uiManager.UpdateMineCounter(remainingMines);
            }
        }
    }

    /// <summary>
    /// Actualiza la visualización de todas las celdas del tablero.
    /// </summary>
    private void UpdateBoardVisuals()
    {
        if (cells == null) return;

        for (int y = 0; y < currentSettings.Height; y++)
        {
            for (int x = 0; x < currentSettings.Width; x++)
            {
                if (cells[x, y] != null)
                {
                    cells[x, y].UpdateVisual();
                }
            }
        }
    }

    /// <summary>
    /// Finaliza el juego (victoria o derrota).
    /// </summary>
    /// <param name="won">true si el jugador ganó, false si perdió</param>
    private void GameOver(bool won)
    {
        IsGameOver = true;
        isTimerRunning = false;

        // Revelar todas las minas
        gameBoard.RevealAllMines();
        UpdateBoardVisuals();

        // Mostrar panel correspondiente
        if (uiManager != null)
        {
            if (won)
            {
                uiManager.ShowWinPanel(elapsedTime);
                Debug.Log($"¡Victoria! Tiempo: {elapsedTime} segundos");
            }
            else
            {
                uiManager.ShowLosePanel();
                Debug.Log("Derrota - Mina explotada");
            }
        }
    }

    /// <summary>
    /// Reinicia la partida con la misma configuración.
    /// </summary>
    public void RestartGame()
    {
        if (currentSettings != null)
        {
            StartNewGame(currentSettings);
        }
    }

    /// <summary>
    /// Corrutina que actualiza el temporizador cada segundo.
    /// </summary>
    /// <returns>IEnumerator para la corrutina</returns>
    private IEnumerator TimerCoroutine()
    {
        while (isTimerRunning)
        {
            yield return new WaitForSeconds(1f);
            
            if (isTimerRunning)
            {
                elapsedTime++;
                
                if (uiManager != null)
                {
                    uiManager.UpdateTimer(elapsedTime);
                }
            }
        }
    }
}