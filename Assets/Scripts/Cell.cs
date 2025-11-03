using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; 
/// <summary>
/// Representa visualmente una celda del tablero de Buscaminas.
/// Maneja la interacción del jugador (clic izquierdo/derecho) y la visualización.
/// </summary>
[RequireComponent(typeof(Button))]
public class Cell : MonoBehaviour, IPointerClickHandler
{
    // ========== COMPONENTES UI ==========
    /// <summary>Componente Button de Unity para detectar clics</summary>
    private Button button;
    
    /// <summary>Imagen de fondo de la celda</summary>
    private Image image;
    
    /// <summary>Texto que muestra el número de minas adyacentes</summary>
    [SerializeField] private Image numberImage;
    
    /// <summary>Icono de la bandera (cuando el jugador marca la celda)</summary>
    [SerializeField] private GameObject flagIcon;

    /// <summary>Icono de la mina (cuando se revela una mina)</summary>
    [SerializeField] private GameObject mineIcon;
    
    // ========== SPRITES DE NÚMEROS ==========
    /// <summary>Sprites para los números del 1 al 8</summary>
    [Header("Number Sprites")]
    [SerializeField] private Sprite[] numberSprites = new Sprite[8];

    /// <summary>Sprite de celda revelada (fondo)</summary>
    [SerializeField] private Sprite revealedSprite;

    /// <summary>Sprite de celda sin revelar (fondo)</summary>
    [SerializeField] private Sprite hiddenSprite;

    // ========== DATOS DE LA CELDA ==========
    /// <summary>Coordenada X de la celda en el tablero</summary>
    public int X { get; private set; }
    
    /// <summary>Coordenada Y de la celda en el tablero</summary>
    public int Y { get; private set; }
    
    /// <summary>Referencia al nodo de la matriz dispersa</summary>
    private SparseMatrixNode node;
    
    /// <summary>Referencia al GameManager</summary>
    private GameManager gameManager;

    /// <summary>
    /// Inicializa los componentes de la celda.
    /// Se ejecuta al crear el objeto.
    /// </summary>
    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        
        // Configurar el clic izquierdo (revelar celda)
        button.onClick.AddListener(OnLeftClick);
        
        // Ocultar iconos inicialmente
        if (flagIcon != null) flagIcon.SetActive(false);
        if (mineIcon != null) mineIcon.SetActive(false);
        if (numberImage != null) numberImage.gameObject.SetActive(false);
        
        // Sprite inicial sin revelar
        if (hiddenSprite != null)
        {
            image.sprite = hiddenSprite;
        }
    }

    /// <summary>
    /// Detecta el clic derecho del mouse para marcar banderas.
    /// Unity no tiene un evento directo para clic derecho en botones,
    /// por lo que se detecta en Update.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameManager == null || gameManager.IsGameOver)
            return;
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameManager.OnCellLeftClick(X, Y);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    /// <summary>
    /// Configura la celda con sus datos y referencia al GameManager.
    /// </summary>
    /// <param name="x">Coordenada X</param>
    /// <param name="y">Coordenada Y</param>
    /// <param name="node">Nodo de la matriz dispersa asociado</param>
    /// <param name="manager">Referencia al GameManager</param>
    public void Setup(int x, int y, SparseMatrixNode node, GameManager manager)
    {
        X = x;
        Y = y;
        this.node = node;
        gameManager = manager;
    }

    /// <summary>
    /// Maneja el clic izquierdo (revelar celda).
    /// Notifica al GameManager para procesar la jugada.
    /// </summary>
    private void OnLeftClick()
    {
        gameManager.OnCellLeftClick(X, Y);
    }

    /// <summary>
    /// Maneja el clic derecho (marcar/desmarcar bandera).
    /// Solo funciona si la celda no ha sido revelada.
    /// </summary>
    private void OnRightClick()
    {
        if (gameManager != null && !gameManager.IsGameOver && !node.IsRevealed)
        {
            gameManager.OnCellRightClick(X, Y);
            UpdateVisual();
        }
    }

    /// <summary>
    /// Actualiza la visualización de la celda según su estado.
    /// Muestra números, banderas o minas según corresponda.
    /// </summary>
    public void UpdateVisual()
    {
        // Si está marcada con bandera
        if (node.IsFlagged && !node.IsRevealed)
        {
            if (flagIcon != null) flagIcon.SetActive(true);
            if (mineIcon != null) mineIcon.SetActive(false);
            if (numberImage != null) numberImage.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (flagIcon != null) flagIcon.SetActive(false);
        }

        // Si está revelada
        if (node.IsRevealed)
        {
            // Cambiar sprite a revelada
            if (revealedSprite != null)
            {
                image.sprite = revealedSprite;
            }
            
            // Si tiene mina
            if (node.HasMine)
            {
                if (mineIcon != null) mineIcon.SetActive(true);
                if (numberImage != null) numberImage.gameObject.SetActive(false);
            }
            // Si tiene número de minas adyacentes
            else if (node.AdjacentMines > 0)
            {
                if (mineIcon != null) mineIcon.SetActive(false);
                if (numberImage != null)
                {
                    numberImage.gameObject.SetActive(true);
                    
                    // Asignar sprite del número correspondiente (1-8)
                    int index = node.AdjacentMines - 1; // Arrays empiezan en 0
                    if (index >= 0 && index < numberSprites.Length && numberSprites[index] != null)
                    {
                        numberImage.sprite = numberSprites[index];
                    }
                }
            }
            // Si es celda vacía (sin minas adyacentes)
            else
            {
                if (mineIcon != null) mineIcon.SetActive(false);
                if (numberImage != null) numberImage.gameObject.SetActive(false);
            }
            
            // Deshabilitar el botón cuando se revela
            button.interactable = false;
        }
    }

    /// <summary>
    /// Revela visualmente la celda.
    /// Llamado por el GameManager cuando el jugador hace clic.
    /// </summary>
    public void Reveal()
    {
        UpdateVisual();
    }

    /// <summary>
    /// Limpia la celda para ser reutilizada (útil al reiniciar).
    /// </summary>
    public void Clear()
    {
        // Restaurar sprite oculta
        if (hiddenSprite != null)
        {
            image.sprite = hiddenSprite;
        }
        
        button.interactable = true;
        
        if (flagIcon != null) flagIcon.SetActive(false);
        if (mineIcon != null) mineIcon.SetActive(false);
        if (numberImage != null) numberImage.gameObject.SetActive(false);
    }
}