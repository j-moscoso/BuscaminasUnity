using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementación de una Matriz Dispersa mediante nodos enlazados.
/// Estructura de datos principal del juego Buscaminas.
/// Cada nodo conoce sus 4 vecinos cardinales (arriba, abajo, izquierda, derecha).
/// </summary>
public class SparseMatrix
{
    // ========== PROPIEDADES DE LA MATRIZ ==========
    /// <summary>Ancho de la matriz (número de columnas)</summary>
    public int Width { get; private set; }
    
    /// <summary>Alto de la matriz (número de filas)</summary>
    public int Height { get; private set; }
    
    /// <summary>Número total de minas en el tablero</summary>
    public int TotalMines { get; private set; }

    /// <summary>
    /// Nodo superior izquierdo de la matriz.
    /// Punto de entrada para recorrer toda la estructura.
    /// </summary>
    private SparseMatrixNode head;

    /// <summary>
    /// Diccionario para acceso rápido a cualquier nodo por coordenadas.
    /// Clave: "x,y", Valor: nodo correspondiente
    /// </summary>
    private Dictionary<string, SparseMatrixNode> nodeMap;

    /// <summary>
    /// Constructor de la matriz dispersa.
    /// Crea e interconecta todos los nodos del tablero.
    /// </summary>
    /// <param name="width">Ancho del tablero</param>
    /// <param name="height">Alto del tablero</param>
    /// <param name="totalMines">Cantidad de minas a distribuir</param>
    public SparseMatrix(int width, int height, int totalMines)
    {
        Width = width;
        Height = height;
        TotalMines = totalMines;
        nodeMap = new Dictionary<string, SparseMatrixNode>();
        
        // Crear la estructura de la matriz dispersa
        BuildMatrix();
        
        // Colocar las minas aleatoriamente
        PlaceMines();
        
        // Calcular los números de minas adyacentes
        CalculateAdjacentMines();
    }

    /// <summary>
    /// Construye la estructura completa de la matriz dispersa.
    /// Crea todos los nodos y los enlaza en las 4 direcciones cardinales.
    /// </summary>
    private void BuildMatrix()
    {
        SparseMatrixNode previousRowHead = null;
        
        // Recorrer cada fila
        for (int y = 0; y < Height; y++)
        {
            SparseMatrixNode rowHead = null;
            SparseMatrixNode previousNode = null;
            
            // Recorrer cada columna
            for (int x = 0; x < Width; x++)
            {
                // Crear nuevo nodo
                SparseMatrixNode newNode = new SparseMatrixNode(x, y);
                
                // Guardar en el diccionario para acceso rápido
                nodeMap[GetKey(x, y)] = newNode;
                
                // Si es el primer nodo de la matriz, guardarlo como head
                if (x == 0 && y == 0)
                {
                    head = newNode;
                }
                
                // Si es el primer nodo de la fila, guardarlo
                if (x == 0)
                {
                    rowHead = newNode;
                }
                
                // Enlazar horizontalmente (izquierda-derecha)
                if (previousNode != null)
                {
                    previousNode.Right = newNode;
                    newNode.Left = previousNode;
                }
                
                // Enlazar verticalmente (arriba-abajo)
                if (previousRowHead != null)
                {
                    SparseMatrixNode nodeAbove = GetNode(x, y - 1);
                    if (nodeAbove != null)
                    {
                        nodeAbove.Down = newNode;
                        newNode.Up = nodeAbove;
                    }
                }
                
                previousNode = newNode;
            }
            
            previousRowHead = rowHead;
        }
    }

    /// <summary>
    /// Distribuye las minas aleatoriamente en el tablero.
    /// Garantiza que no se coloquen dos minas en la misma celda.
    /// </summary>
    private void PlaceMines()
    {
        int minesPlaced = 0;
        System.Random random = new System.Random();
        
        while (minesPlaced < TotalMines)
        {
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            
            SparseMatrixNode node = GetNode(x, y);
            
            // Solo colocar mina si la celda no tiene una ya
            if (node != null && !node.HasMine)
            {
                node.HasMine = true;
                minesPlaced++;
            }
        }
    }

    /// <summary>
    /// Calcula el número de minas adyacentes para cada celda del tablero.
    /// Solo calcula para celdas que no contienen minas.
    /// </summary>
    private void CalculateAdjacentMines()
    {
        // Recorrer toda la matriz
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SparseMatrixNode node = GetNode(x, y);
                
                // Si la celda no tiene mina, contar minas adyacentes
                if (node != null && !node.HasMine)
                {
                    node.AdjacentMines = CountAdjacentMines(x, y);
                }
            }
        }
    }

    /// <summary>
    /// Cuenta cuántas minas hay en las 8 celdas adyacentes a una posición.
    /// </summary>
    /// <param name="x">Coordenada X de la celda</param>
    /// <param name="y">Coordenada Y de la celda</param>
    /// <returns>Número de minas adyacentes (0-8)</returns>
    private int CountAdjacentMines(int x, int y)
    {
        int count = 0;
        
        // Direcciones de las 8 celdas adyacentes
        int[] dx = { -1, -1, -1,  0,  0,  1,  1,  1 };
        int[] dy = { -1,  0,  1, -1,  1, -1,  0,  1 };
        
        for (int i = 0; i < 8; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];
            
            // Verificar límites y contar si hay mina
            if (IsValidPosition(newX, newY))
            {
                SparseMatrixNode neighbor = GetNode(newX, newY);
                if (neighbor != null && neighbor.HasMine)
                {
                    count++;
                }
            }
        }
        
        return count;
    }

    /// <summary>
    /// Obtiene un nodo de la matriz por sus coordenadas.
    /// </summary>
    /// <param name="x">Coordenada X</param>
    /// <param name="y">Coordenada Y</param>
    /// <returns>Nodo en la posición especificada, o null si no existe</returns>
    public SparseMatrixNode GetNode(int x, int y)
    {
        string key = GetKey(x, y);
        return nodeMap.ContainsKey(key) ? nodeMap[key] : null;
    }

    /// <summary>
    /// Genera la clave única para el diccionario basada en coordenadas.
    /// </summary>
    /// <param name="x">Coordenada X</param>
    /// <param name="y">Coordenada Y</param>
    /// <returns>Clave en formato "x,y"</returns>
    private string GetKey(int x, int y)
    {
        return $"{x},{y}";
    }

    /// <summary>
    /// Verifica si una posición está dentro de los límites del tablero.
    /// </summary>
    /// <param name="x">Coordenada X a verificar</param>
    /// <param name="y">Coordenada Y a verificar</param>
    /// <returns>true si la posición es válida, false en caso contrario</returns>
    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    /// <summary>
    /// Obtiene todos los nodos vecinos (8 direcciones) de una posición.
    /// </summary>
    /// <param name="x">Coordenada X central</param>
    /// <param name="y">Coordenada Y central</param>
    /// <returns>Lista de nodos vecinos válidos</returns>
    public List<SparseMatrixNode> GetNeighbors(int x, int y)
    {
        List<SparseMatrixNode> neighbors = new List<SparseMatrixNode>();
        
        // Direcciones de las 8 celdas adyacentes
        int[] dx = { -1, -1, -1,  0,  0,  1,  1,  1 };
        int[] dy = { -1,  0,  1, -1,  1, -1,  0,  1 };
        
        for (int i = 0; i < 8; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];
            
            if (IsValidPosition(newX, newY))
            {
                SparseMatrixNode neighbor = GetNode(newX, newY);
                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
                }
            }
        }
        
        return neighbors;
    }

    /// <summary>
    /// Revela una celda y propaga si es necesario.
    /// Si la celda es vacía (sin minas adyacentes), revela automáticamente sus vecinos.
    /// </summary>
    /// <param name="x">Coordenada X de la celda a revelar</param>
    /// <param name="y">Coordenada Y de la celda a revelar</param>
    /// <returns>true si se reveló una mina (juego perdido), false en caso contrario</returns>
    public bool RevealCell(int x, int y)
    {
        SparseMatrixNode node = GetNode(x, y);
        
        // Validaciones
        if (node == null || node.IsRevealed || node.IsFlagged)
        {
            return false;
        }
        
        // Revelar la celda
        node.Reveal();
        
        // Si tiene mina, el juego termina
        if (node.HasMine)
        {
            return true;
        }
        
        // Si es una celda vacía (sin minas adyacentes), propagar revelado
        if (node.AdjacentMines == 0)
        {
            PropagateReveal(x, y);
        }
        
        return false;
    }

    /// <summary>
    /// Propaga el revelado a celdas adyacentes vacías.
    /// Utiliza un algoritmo iterativo para evitar desbordamiento de pila.
    /// </summary>
    /// <param name="startX">Coordenada X inicial</param>
    /// <param name="startY">Coordenada Y inicial</param>
    private void PropagateReveal(int startX, int startY)
    {
        Queue<SparseMatrixNode> queue = new Queue<SparseMatrixNode>();
        HashSet<SparseMatrixNode> visited = new HashSet<SparseMatrixNode>();
        
        SparseMatrixNode startNode = GetNode(startX, startY);
        queue.Enqueue(startNode);
        visited.Add(startNode);
        
        while (queue.Count > 0)
        {
            SparseMatrixNode current = queue.Dequeue();
            
            // Obtener vecinos
            List<SparseMatrixNode> neighbors = GetNeighbors(current.X, current.Y);
            
            foreach (SparseMatrixNode neighbor in neighbors)
            {
                // Si ya fue visitado o está marcado, saltar
                if (visited.Contains(neighbor) || neighbor.IsFlagged)
                {
                    continue;
                }
                
                // Revelar el vecino
                neighbor.Reveal();
                visited.Add(neighbor);
                
                // Si el vecino también es vacío, agregarlo a la cola
                if (neighbor.AdjacentMines == 0)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

    /// <summary>
    /// Alterna el estado de bandera en una celda.
    /// </summary>
    /// <param name="x">Coordenada X</param>
    /// <param name="y">Coordenada Y</param>
    public void ToggleFlag(int x, int y)
    {
        SparseMatrixNode node = GetNode(x, y);
        if (node != null)
        {
            node.ToggleFlag();
        }
    }

    /// <summary>
    /// Verifica si el jugador ha ganado la partida.
    /// Gana cuando todas las celdas sin mina han sido reveladas.
    /// </summary>
    /// <returns>true si el jugador ha ganado, false en caso contrario</returns>
    public bool CheckWin()
    {
        int totalCells = Width * Height;
        int cellsToReveal = totalCells - TotalMines;
        int revealedCells = 0;
        
        // Contar celdas reveladas sin mina
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SparseMatrixNode node = GetNode(x, y);
                if (node != null && node.IsRevealed && !node.HasMine)
                {
                    revealedCells++;
                }
            }
        }
        
        return revealedCells == cellsToReveal;
    }

    /// <summary>
    /// Revela todas las minas del tablero.
    /// Se utiliza cuando el jugador pierde o gana.
    /// </summary>
    public void RevealAllMines()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SparseMatrixNode node = GetNode(x, y);
                if (node != null && node.HasMine)
                {
                    node.Reveal();
                }
            }
        }
    }
}