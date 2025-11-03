## **🎮 Manual de Usuario - Buscaminas**

### 

### **Bienvenido al Buscaminas Clásico**



Este es un juego de lógica y estrategia donde debes encontrar todas las casillas sin minas, evitando detonarlas. ¡Pon a prueba tu razonamiento deductivo!.



**📋 Tabla de Contenidos**



1. Objetivo del Juego

2\. Cómo Jugar

3\. Controles

4\. Niveles de Dificultad

5\. Interfaz del Juego

6\. Estrategias y Consejos

7\. Preguntas Frecuentes



###### **🎯 Objetivo del Juego:**



El objetivo es revelar todas las casillas del tablero que NO contienen minas, sin detonar ninguna mina.



###### **¿Cómo se gana?**

Ganas cuando hayas revelado todas las casillas seguras. Las minas pueden quedar sin revelar.



###### **¿Cómo se pierde?**

Pierdes si haces clic en una casilla que contiene una mina.



###### **🕹️ Cómo Jugar:**

###### 

###### **Reglas Básicas**



1. **El tablero está compuesto de casillas cuadradas que pueden contener:**



* Una mina 💣
* Un número (1-8) que indica cuántas minas hay en las 8 casillas adyacentes
* Un espacio vacío (0 minas adyacentes)



**2. Al inicio, todas las casillas están ocultas mostrando la textura sin revelar.**



**3. Haz clic izquierdo en una casilla para revelarla:**



Si tiene una mina → Pierdes 💥

Si tiene un número → Se muestra el sprite del número

Si está vacía → Se revelan automáticamente todas las casillas vacías conectadas





**4. Usa el clic derecho para marcar casillas sospechosas con una bandera 🚩**



**5. Ganas cuando todas las casillas sin mina están reveladas.**





###### **🖱️ Controles**



**|Acción                   |     Control               |    Descripción                                            |**

|Revelar casilla          |     Clic izquierdo        |    Descubre qué hay en la casilla                         |

|Marcar bandera           |     Clic derecho          |    Marca/desmarca una casilla como mina sospechosa        |

|Reiniciar partida        |     Botón "Reiniciar"     |    Comienza una nueva partida con la misma dificultad     |

|Volver al menú           |     Botón "Menú"          |    Regresa a la selección de dificultad                   |



**Nota: Los clics funcionan de manera precisa gracias al sistema de eventos de Unity. Si una casilla no responde, asegúrate de hacer clic directamente sobre ella.**



###### **📊 Niveles de Dificultad**



**🟢 Principiante**



Tablero: 8 × 8 (64 casillas)

Minas: 10

Ideal para: Jugadores nuevos que están aprendiendo

Tiempo promedio: 2-5 minutos



**🟡 Intermedio**



Tablero: 16 × 16 (256 casillas)

Minas: 40

Ideal para: Jugadores con experiencia básica

Tiempo promedio: 5-10 minutos



**🔴 Experto**



Tablero: 16 × 30 (480 casillas)

Minas: 99

Ideal para: Jugadores expertos que buscan un desafío

Tiempo promedio: 10-20 minutos



**⚙️ Personalizado**



Tablero: Tú decides el tamaño

Minas: Tú decides cuántas

Restricciones:



Tamaño mínimo: 4 × 4

Tamaño máximo: 50 × 50

Las minas deben ser menos que el total de casillas

Recomendado: No más del 70% de minas



###### **Elementos en Pantalla**



**Contador de Minas (esquina superior izquierda)**



Muestra: Minas: XXX

Indica cuántas minas faltan por encontrar

Se reduce cuando marcas banderas

Puede ser negativo si marcas más banderas que minas





**Temporizador (esquina superior derecha)**



Formato: Tiempo: MM:SS

Comienza al iniciar el juego

Se detiene al ganar o perder





**Botón Reiniciar**



Inicia una nueva partida con la misma dificultad

No regresa al menú principal

Genera un nuevo tablero aleatorio





**Botón Menú**



Vuelve a la pantalla de selección de dificultad

Abandona la partida actual





**Tablero de Juego**



Cuadrícula de casillas con sprites profesionales



Haz clic para jugar





###### **Cómo se ven las Casillas**



**Casilla sin revelar:**



Muestra el sprite de tile oculta

Puede tener una bandera encima si la marcaste



**Casilla revelada vacía:**



Muestra el sprite de tile revelada

Sin ningún icono adicional



**Casilla revelada con número:**



Muestra el sprite de tile revelada

Con el sprite del número correspondiente encima



**Casilla con bandera:**



Muestra el sprite de tile oculta

Con el sprite de la bandera encima



**Casilla con mina (al perder):**



Muestra el sprite de tile revelada

Con el sprite de la mina encima



###### **💡 Estrategias y Consejos** 



**Comienza por las esquinas**



Las esquinas tienen menos casillas adyacentes (3)

Es más fácil deducir dónde hay minas





**Busca patrones obvios**



Si un "1" tiene solo una casilla sin revelar cerca, ahí hay una mina

Si un "1" ya tiene una bandera cerca, las demás casillas son seguras





**Usa las banderas**



Marca las minas que identifiques con certeza

Te ayuda a visualizar el tablero

No tengas miedo de quitar banderas si te equivocaste





**No adivines al principio**



Primero revela las zonas seguras

Adivina solo cuando no haya otra opción





**Observa los sprites de números**



Cada número tiene su propio diseño

Con la práctica los reconocerás instantáneamente



###### **❓ Preguntas Frecuentes** 



**¿Puedo cambiar de dificultad durante una partida?**

No directamente, pero puedes hacer clic en "Menú" para volver a seleccionar otra dificultad. La partida actual se perderá.



**¿Las minas se generan aleatoriamente?**

Sí, cada vez que inicias una partida nueva, las minas se colocan en posiciones completamente aleatorias.



**¿Qué significa el contador de minas negativo?**

El contador muestra: Total de minas - Banderas colocadas. Si colocas más banderas que minas reales, el número será negativo. Esto significa que has marcado casillas de más.



**¿Puedo perder en el primer clic?**

Sí, es posible hacer clic en una mina en el primer intento. Esta es una característica del Buscaminas clásico.



**¿Hay un límite de tiempo?**

No hay límite de tiempo. El temporizador solo registra cuánto tardas, pero puedes tomarte el tiempo que necesites.



**¿El juego guarda mi progreso?**

No, si cierras el juego o vuelves al menú, la partida se pierde. Cada sesión es independiente.



**¿Cómo sé si estoy cerca de ganar?**

Cuando hayas revelado casi todas las casillas y solo queden las minas sin revelar. El juego detecta automáticamente la victoria cuando revelas la última casilla segura.



**¿Qué pasa si marco una casilla incorrecta?**

Nada malo. Las banderas son solo marcadores visuales para ti. No afectan el juego directamente, solo te ayudan a organizar tu estrategia. Puedes quitar banderas en cualquier momento con clic derecho.



**¿Puedo quitar una bandera?**

Sí, haz clic derecho nuevamente en la casilla con bandera para quitarla.



**¿Por qué algunas casillas se revelan solas?**

Cuando haces clic en una casilla vacía (sin minas adyacentes), el juego automáticamente revela todas las casillas vacías conectadas y sus números vecinos. Esto se llama "propagación" y te ahorra tiempo.



**¿Por qué no puedo hacer clic en algunas casillas?**

Si una casilla ya está revelada, no podrás hacer clic en ella nuevamente. Las casillas reveladas se distinguen por el sprite de tile revelada.



**¿Puedo usar banderas en casillas reveladas?**

No, solo puedes marcar banderas en casillas sin revelar. Una vez que una casilla está revelada, no se puede marcar.



**¿Los sprites cambian el gameplay?**

No, los sprites son puramente visuales. El juego funciona exactamente igual que el Buscaminas clásico, solo se ve mejor y es más fácil distinguir los números.



**¿Qué pasa al hacer clic derecho sin querer?**

Si haces clic derecho en una casilla sin bandera, se marca con bandera. Si ya tiene bandera, la quita. No hay penalización por marcar y desmarcar.





###### **📞 Soporte**

###### **Si encuentras algún problema técnico:**

**(Nosotros lloramos)**



* Verifica que todos los sprites estén cargados correctamente
* Reinicia el juego si los clics no responden
* Asegúrate de hacer clic directamente sobre las casillas



Para sugerencias de mejora, ¡tus comentarios son bienvenidos!


**psdt: Es nuestro primer juego no nos funen**



**¡Disfruta el juego y buena suerte encontrando todas las minas! 💣🎮**



**Manual de Usuario v1.0 - Buscaminas Unity con Sistema de Sprites**

Fecha de actualización: 2025/1/11



**Redactado y escrito por:**

Slleider Rojas Aleman

Juan Manuel Moscoso

