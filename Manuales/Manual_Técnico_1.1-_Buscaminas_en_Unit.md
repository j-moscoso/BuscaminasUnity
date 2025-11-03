##### **Manual Técnico - Buscaminas en Unity**



###### 📋 Índice



Arquitectura del Sistema

2\. Estructura de Datos: Matriz Dispersa

3\. Clases y Componentes

4\. Flujo Lógico del Juego

5\. Algoritmos Principales

6\. Sistema de Sprites

7\. Configuración en Unity



###### **🏗️ Arquitectura del Sistema**



El proyecto está estructurado siguiendo el patrón MVC (Modelo-Vista-Controlador) adaptado para Unity:



Modelo: SparseMatrix y SparseMatrixNode (estructura de datos del tablero)

Vista: Cell y UIManager (representación visual e interfaz)

Controlador: GameManager (lógica del juego y coordinación)



###### **🔗 Estructura de Datos: Matriz Dispersa**

###### 

Concepto:

Una matriz dispersa es una estructura de datos donde cada elemento (nodo) conoce sus vecinos mediante referencias directas, en lugar de usar índices en un array tradicional.



**Ventajas de esta Implementación**



Navegación eficiente: Acceso directo a vecinos sin cálculos de índices

Flexibilidad: Fácil de expandir o modificar dinámicamente

Memoria optimizada: Solo almacena nodos existentes

Algoritmos naturales: Propagación de celdas vacías es intuitiva



Construcción de la Matriz

La matriz se construye en dos fases:



Creación de nodos: Se generan todos los nodos y se guardan en un diccionario para acceso rápido

Enlazamiento: Cada nodo se conecta con sus 4 vecinos cardinales



Acceso a Nodos

Existen dos formas de acceso:



Por coordenadas: GetNode(x, y) usando un diccionario (O(1))

Por navegación: Siguiendo los enlaces Up, Down, Left, Right



###### **📦 Clases y Componentes**



**1. SparseMatrixNode**



Propósito: Representa una celda individual del tablero.

Responsabilidades:



Almacenar estado de la celda (mina, revelada, bandera)

Mantener referencias a vecinos cardinales

Proporcionar métodos básicos (Reveal(), ToggleFlag())



Complejidad espacial: O(1) por nodo



**2. SparseMatrix**



Propósito: Gestionar toda la estructura del tablero y la lógica de juego.

Métodos principales:



|         Método            |     Complejidad    |              Descripción                 |

|BuildMatrix()              |  O(W × H)          |   Construye y enlaza todos los nodos     |

|PlaceMines()               |  O(M)              |   Distribuye M minas aleatoriamente      |

|CalculateAdjacentMines()   |  O(W × H)          |  Calcula números para cada celda         |

|RevealCell(x, y)           |  O(N)              |  Revela celda y propaga si es vacía      |

|CheckWin()                 |  O(W × H)          |   Verifica condición de victoria         |



**Donde:**



W = ancho del tablero

H = alto del tablero

M = número de minas

N = número de celdas reveladas por propagación



**3. DifficultySettings**



Propósito: Encapsular configuraciones de dificultad.

Niveles predefinidos:



Principiante: 8×8, 10 minas (15.6% del tablero)

Intermedio: 16×16, 40 minas (15.6% del tablero)

Experto: 16×30, 99 minas (20.6% del tablero)

Personalizado: Valores definidos por el usuario



**Validación:**



Dimensiones: 4×4 mínimo, 50×50 máximo

Minas: 1 ≤ M < (W × H)

Recomendación: M ≤ 70% del total de celdas



**4. Cell (MonoBehaviour)**



Propósito: Representación visual de una celda en Unity usando sprites.

Componentes necesarios:



Button: Para detectar clics

Image: Para el sprite de fondo (tile\_hidden/tile\_revealed)

Image (NumberImage): Para mostrar sprites de números

Image (FlagIcon): Sprite de bandera

Image (MineIcon): Sprite de mina



**Interacción (usando IPointerClickHandler):**



Clic izquierdo: Revelar celda (OnPointerClick())

Clic derecho: Marcar/desmarcar bandera (OnPointerClick())



Estados visuales:



Sin revelar: Sprite tile\_hidden

Revelada vacía: Sprite tile\_revealed sin iconos

Revelada con número: Sprite tile\_revealed + sprite número correspondiente

Marcada: Sprite tile\_hidden + icono de bandera

Mina explotada: Sprite tile\_revealed + icono de mina



**5. UIManager (MonoBehaviour)**



Propósito: Gestionar toda la interfaz de usuario.

Paneles gestionados:



Menú principal (selección de dificultad)

Panel de juego (tablero, contadores, botones)

Configuración personalizada

Pantalla de victoria

Pantalla de derrota



Elementos dinámicos:



Contador de minas: Minas: XXX

Temporizador: Tiempo: MM:SS

Mensajes de resultado



Actualización de Referencias:



Usa FindFirstObjectByType<T>() en lugar de FindObjectOfType<T>() (API actualizada de Unity)





**6. GameManager (MonoBehaviour)**



Propósito: Controlador central del juego.

Responsabilidades principales:



Inicialización del juego:



csharp   StartNewGame(settings):

       - Crear SparseMatrix

       - Generar tablero visual

       - Iniciar temporizador

       - Actualizar UI



Gestión de eventos:



OnCellLeftClick(x, y): Procesar revelado

OnCellRightClick(x, y): Procesar banderas



Control de estado:



Detectar victoria/derrota

Gestionar temporizador

Actualizar contadores



Generación visual:



Instanciar celdas desde prefab

Configurar GridLayoutGroup dinámicamente

Calcular tamaño de celdas según dimensiones



Actualización de Referencias:



Usa FindFirstObjectByType<UIManager>() para compatibilidad con Unity moderno



🔄 Flujo Lógico del Juego



Diagrama de Flujo Principal



INICIO

  ↓

Mostrar Menú Principal

  ↓

Usuario selecciona dificultad

  ↓

\[GameManager] StartNewGame()

  ├─→ Crear SparseMatrix

  ├─→ Colocar minas aleatoriamente

  ├─→ Calcular números adyacentes

  ├─→ Generar celdas visuales con sprites

  └─→ Iniciar temporizador

  ↓

JUGANDO ←───┐

  ↓         │

Usuario hace clic en celda

  ↓         │

¿Clic izquierdo o derecho?

  ├─→ Izquierdo: RevealCell()

  │   ├─→ ¿Tiene mina? → SÍ → DERROTA

  │   └─→ ¿Es vacía? → SÍ → Propagar revelado

  │       ↓

  │   ¿Victoria? (todas las celdas sin mina reveladas)

  │       ├─→ SÍ → VICTORIA

  │       └─→ NO ──────────────┘

  │

  └─→ Derecho: ToggleFlag()

      └─→ Actualizar contador ─┘





Secuencia de Revelado de Celda



1\. Usuario hace clic izquierdo

2\. Cell.OnPointerClick() detecta PointerEventData.InputButton.Left

3\. Llamar a GameManager.OnCellLeftClick(x, y)

4\. GameManager llama a SparseMatrix.RevealCell(x, y)

5\. SparseMatrix revela el nodo

6\. Si es celda vacía → PropagateReveal()

7\. GameManager actualiza visuales: UpdateBoardVisuals()

8\. Cada Cell.UpdateVisual() actualiza su sprite

9\. GameManager verifica victoria/derrota

10\. Si termina → mostrar panel resultado



###### **🧮 Algoritmos Principales**



**1. Colocación de Minas**



PlaceMines():

    minesPlaced = 0

    MIENTRAS minesPlaced < TotalMines:

        x = Random(0, Width)

        y = Random(0, Height)

        node = GetNode(x, y)

        SI node NO tiene mina:

            node.HasMine = true

            minesPlaced++

Complejidad: O(M) en promedio, donde M = número de minas



**2. Cálculo de Minas Adyacentes**



CalculateAdjacentMines():

    PARA cada celda (x, y) en el tablero:

        SI celda NO tiene mina:

            count = 0

            PARA cada vecino en las 8 direcciones:

                SI vecino tiene mina:

                    count++

            celda.AdjacentMines = count

Complejidad: O(W × H × 8) = O(W × H)



**3. Propagación de Revelado (BFS)**



PropagateReveal(startX, startY):

    queue = Queue()

    visited = Set()

 

    queue.add(startNode)

    visited.add(startNode)

 

    MIENTRAS queue no esté vacía:

        current = queue.dequeue()

 

        PARA cada vecino de current:

            SI vecino NO visitado Y NO marcado:

                vecino.Reveal()

                visited.add(vecino)

 

                SI vecino es vacío (AdjacentMines == 0):

                    queue.add(vecino)

Complejidad: O(N), donde N es el número de celdas reveladas

Estructura: Breadth-First Search (BFS) iterativo



**4. Verificación de Victoria**



CheckWin():

    totalCells = Width × Height

    cellsToReveal = totalCells - TotalMines

    revealedCells = 0

 

    PARA cada celda en el tablero:

        SI celda revelada Y NO tiene mina:

            revealedCells++

 

    RETORNAR revealedCells == cellsToReveal

Complejidad: O(W × H)

Optimización posible: Mantener contador incremental



###### **🎨 Sistema de Sprites**



Sprites Necesarios



El juego utiliza los siguientes sprites:

Tiles (Celdas):



tile\_hidden.png - Celda sin revelar

tile\_revealed.png - Celda revelada



Números (1-8):



number\_1.png hasta number\_8.png

Cada sprite representa la cantidad de minas adyacentes



**Iconos:**



bomb.png - Mina

flag.png - Bandera



Configuración de Sprites



Texture Type: Sprite (2D and UI)

Pixels Per Unit: 100

Filter Mode: Bilinear

Compression: None (mejor calidad)



Componentes del GameObject Cell:



Button - Para detectar clics

Image - Sprite de fondo (tile\_hidden/tile\_revealed)

Cell.cs - Script con lógica



###### **🎮 Configuración en Unity**



Referencias a Asignar en Inspector

GameManager:



uiManager: UIManager del scene

boardContainer: Transform del GridLayoutGroup

cellPrefab: Prefab de Cell

gridLayout: GridLayoutGroup component

baseCellSize: 40-60 (recomendado)

cellSpacing: 2-4 (recomendado)



UIManager:



Todos los paneles (MainMenu, Game, Custom, Win, Lose)

Todos los botones

TextMeshProUGUI de contadores

InputFields de configuración personalizada



Cell Prefab:



numberSprites: Array de 8 sprites (number\_1 a number\_8)

revealedSprite: tile\_revealed

hiddenSprite: tile\_hidden

numberImage: GameObject NumberImage

flagIcon: GameObject FlagIcon

mineIcon: GameObject MineIcon



###### **📝 Notas de Implementación**



Sprites: Se usan en lugar de texto para mejor rendimiento y calidad visual

IPointerClickHandler: Reemplaza el método Update() para detección de clics más confiable

Corrutinas: El temporizador usa corrutinas para no bloquear el thread principal

Eventos: Se usan onClick listeners en lugar de polling

Separación de concerns: Lógica de juego separada de la visualización

Raycast optimization: Solo el botón principal tiene Raycast Target activado





###### **🚀 Optimizaciones Implementadas**



Sprites reutilizables: Los mismos sprites se usan para todas las celdas

Content Size Fitter: Ajuste automático del tamaño del tablero

Grid Layout Group: Organización automática de celdas

Dictionary lookup: Acceso O(1) a cualquier nodo por coordenadas

BFS iterativo: Evita desbordamiento de pila en propagación





###### **🎯 Especificaciones Finales del Sistema**



**Requisitos:**



Unity 2021.3 LTS o superior

TextMeshPro (incluido en Unity)

Sprites personalizados (14 sprites totales)



**Rendimiento:**



Tablero 8×8: ~64 GameObjects

Tablero 16×16: ~256 GameObjects

Tablero 16×30: ~480 GameObjects

FPS objetivo: 60+ en todos los tamaños



**Memoria:**



Matriz dispersa: ~100 bytes por nodo

Sprites: ~50KB total (con compresión)

UI: ~2MB en memoria



**Redactado por:**



Juan Manuel Moscoso

Slleider Rojas Aleman



Versión: 1.0

Fecha: 2025/11/01

