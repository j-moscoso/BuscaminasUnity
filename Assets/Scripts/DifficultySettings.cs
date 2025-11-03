using UnityEngine;

/// <summary>
/// Configuración de niveles de dificultad para el Buscaminas.
/// Define los parámetros de tamaño de tablero y cantidad de minas.
/// </summary>
[System.Serializable]
public class DifficultySettings
{
    // ========== ENUMERACIÓN DE DIFICULTADES ==========
    /// <summary>
    /// Niveles de dificultad disponibles en el juego.
    /// </summary>
    public enum Difficulty
    {
        Beginner,    // Principiante: 8x8 con 10 minas
        Intermediate, // Intermedio: 16x16 con 40 minas
        Expert,      // Experto: 16x30 con 99 minas
        Custom       // Personalizado: valores definidos por el usuario
    }

    // ========== PROPIEDADES DEL TABLERO ==========
    /// <summary>Ancho del tablero (número de columnas)</summary>
    public int Width { get; set; }
    
    /// <summary>Alto del tablero (número de filas)</summary>
    public int Height { get; set; }
    
    /// <summary>Cantidad total de minas en el tablero</summary>
    public int Mines { get; set; }

    /// <summary>
    /// Constructor por defecto. Crea configuración para nivel Principiante.
    /// </summary>
    public DifficultySettings()
    {
        SetDifficulty(Difficulty.Beginner);
    }

    /// <summary>
    /// Constructor con nivel de dificultad específico.
    /// </summary>
    /// <param name="difficulty">Nivel de dificultad deseado</param>
    public DifficultySettings(Difficulty difficulty)
    {
        SetDifficulty(difficulty);
    }

    /// <summary>
    /// Constructor para configuración personalizada.
    /// </summary>
    /// <param name="width">Ancho del tablero</param>
    /// <param name="height">Alto del tablero</param>
    /// <param name="mines">Cantidad de minas</param>
    public DifficultySettings(int width, int height, int mines)
    {
        Width = width;
        Height = height;
        Mines = mines;
    }

    /// <summary>
    /// Establece los parámetros según el nivel de dificultad.
    /// </summary>
    /// <param name="difficulty">Nivel de dificultad a configurar</param>
    public void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Beginner:
                // Principiante: tablero pequeño, pocas minas
                Width = 8;
                Height = 8;
                Mines = 10;
                break;

            case Difficulty.Intermediate:
                // Intermedio: tablero mediano, minas moderadas
                Width = 16;
                Height = 16;
                Mines = 40;
                break;

            case Difficulty.Expert:
                // Experto: tablero grande, muchas minas
                Width = 16;
                Height = 30;
                Mines = 99;
                break;

            case Difficulty.Custom:
                // Personalizado: mantener valores actuales
                // No modificar Width, Height, ni Mines
                break;
        }
    }

    /// <summary>
    /// Valida que la configuración sea correcta.
    /// El número de minas no puede exceder el número total de celdas.
    /// </summary>
    /// <returns>true si la configuración es válida, false en caso contrario</returns>
    public bool IsValid()
    {
        // Validar dimensiones mínimas
        if (Width < 4 || Height < 4)
        {
            Debug.LogWarning("El tablero debe ser al menos de 4x4");
            return false;
        }

        // Validar dimensiones máximas (para evitar problemas de rendimiento)
        if (Width > 50 || Height > 50)
        {
            Debug.LogWarning("El tablero no puede exceder 50x50");
            return false;
        }

        // Validar número de minas
        int totalCells = Width * Height;
        if (Mines < 1)
        {
            Debug.LogWarning("Debe haber al menos 1 mina");
            return false;
        }

        if (Mines >= totalCells)
        {
            Debug.LogWarning("El número de minas debe ser menor que el número total de celdas");
            return false;
        }

        // Recomendación: no más del 70% de celdas con minas
        if (Mines > totalCells * 0.7f)
        {
            Debug.LogWarning("Se recomienda que las minas no excedan el 70% del tablero");
            // Aún así es válido, solo advertencia
        }

        return true;
    }

    /// <summary>
    /// Obtiene el nombre descriptivo de un nivel de dificultad.
    /// </summary>
    /// <param name="difficulty">Nivel de dificultad</param>
    /// <returns>Nombre en español del nivel</returns>
    public static string GetDifficultyName(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Beginner:
                return "Principiante";
            case Difficulty.Intermediate:
                return "Intermedio";
            case Difficulty.Expert:
                return "Experto";
            case Difficulty.Custom:
                return "Personalizado";
            default:
                return "Desconocido";
        }
    }

    /// <summary>
    /// Obtiene la descripción detallada de la configuración actual.
    /// </summary>
    /// <returns>String con las especificaciones del tablero</returns>
    public string GetDescription()
    {
        return $"Tablero: {Width}x{Height} | Minas: {Mines}";
    }
}