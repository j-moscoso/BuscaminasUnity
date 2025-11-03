using UnityEngine;

/// <summary>
/// Nodo individual de la matriz dispersa.
/// Representa una celda del tablero del Buscaminas.
/// Cada nodo conoce sus vecinos en las 4 direcciones cardinales.
/// </summary>
public class SparseMatrixNode
{
    // ========== PROPIEDADES DE POSICIÓN ==========
    /// <summary>Coordenada X del nodo en la matriz</summary>
    public int X { get; set; }
    
    /// <summary>Coordenada Y del nodo en la matriz</summary>
    public int Y { get; set; }

    // ========== PROPIEDADES DEL JUEGO ==========
    /// <summary>Indica si esta celda contiene una mina</summary>
    public bool HasMine { get; set; }
    
    /// <summary>Número de minas adyacentes (0-8)</summary>
    public int AdjacentMines { get; set; }
    
    /// <summary>Indica si la celda ha sido revelada por el jugador</summary>
    public bool IsRevealed { get; set; }
    
    /// <summary>Indica si el jugador ha marcado esta celda con una bandera</summary>
    public bool IsFlagged { get; set; }

    // ========== ENLACES DE LA MATRIZ DISPERSA ==========
    /// <summary>Referencia al nodo de arriba</summary>
    public SparseMatrixNode Up { get; set; }
    
    /// <summary>Referencia al nodo de abajo</summary>
    public SparseMatrixNode Down { get; set; }
    
    /// <summary>Referencia al nodo de la izquierda</summary>
    public SparseMatrixNode Left { get; set; }
    
    /// <summary>Referencia al nodo de la derecha</summary>
    public SparseMatrixNode Right { get; set; }

    /// <summary>
    /// Constructor del nodo de la matriz dispersa.
    /// Inicializa una celda vacía sin mina en la posición especificada.
    /// </summary>
    /// <param name="x">Coordenada X en la matriz</param>
    /// <param name="y">Coordenada Y en la matriz</param>
    public SparseMatrixNode(int x, int y)
    {
        X = x;
        Y = y;
        HasMine = false;
        AdjacentMines = 0;
        IsRevealed = false;
        IsFlagged = false;
        
        // Inicialmente sin enlaces
        Up = null;
        Down = null;
        Left = null;
        Right = null;
    }

    /// <summary>
    /// Revela esta celda (la descubre).
    /// Una vez revelada, no puede volver a ocultarse.
    /// </summary>
    public void Reveal()
    {
        IsRevealed = true;
    }

    /// <summary>
    /// Alterna el estado de la bandera en esta celda.
    /// Solo puede marcarse si la celda no ha sido revelada.
    /// </summary>
    public void ToggleFlag()
    {
        if (!IsRevealed)
        {
            IsFlagged = !IsFlagged;
        }
    }
}