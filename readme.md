# 👾 BOTS: Battleground of Tactical Scripts (BOTS)

## 🎯 Controla la IA, Escribe el Código

**BOTS** es un sandbox educativo y técnico que transforma la programación en tiempo real en una experiencia de juego. El objetivo es simple: el jugador utiliza una consola de código integrada para reescribir las reglas de Inteligencia Artificial (IA) de los enemigos (Bugbots) y controlar su comportamiento, desde el movimiento individual hasta la gestión de enjambres masivos y la balística.

Este proyecto destaca por su capacidad de **modificación de IA en caliente**, permitiendo la experimentación directa con conceptos avanzados de física y lógica de comportamiento.

## ✨ Funcionalidades Destacadas

| Característica | Descripción |
| :--- | :--- |
| **Consola Integrada** | Panel de código accesible con **`F1`** para reescribir las propiedades de los enemigos en tiempo real. |
| **IA Condicional** | Enemigos que alternan su comportamiento entre **Vagabundeo** (Wander) y **Persecución** (Chase). |
| **Movimiento Suavizado (Lerp)** | Movimientos interpolados para simular aceleración y giros orgánicos. |
| **Física Balística Simulada** | Torretas lanzan proyectiles con trayectoria parabólica y simulación de gravedad manual. |
| **Bandada Boids** | Sistema de inteligencia artificial de grupo con control total sobre las reglas de **Cohesión**, **Separación** y **Alineación**.  |

## 🛠️ Requisitos del Sistema

Para clonar y ejecutar este proyecto, necesitarás:

* **Unity Editor:** Versión 6 (LTS recomendado).
* **Plataforma de Desarrollo:** Linux.
* **Dependencias:** TextMeshPro (TMP) y el sistema de `Physics 2D` activado.

## 📥 Instalación y Ejecución

Sigue estos pasos para poner en marcha el proyecto:

1.  **Clonar el Repositorio:**
    ```bash
    git clone [dirección del repositorio]
    cd BOTS
    ```

2.  **Abrir en Unity:**
    * Abre Unity Hub.
    * Haz clic en "Add" y selecciona la carpeta raíz `BOTS`.
    * Abre el proyecto.

3.  **Configurar Escenas:**
    * Asegúrate de que las escenas principales estén añadidas a **File > Build Settings...**.

4.  **Ejecutar:**
    * Abre la escena de inicio (o `Nivel1`).
    * Presiona **Play**.

## 💻 Estructura de Scripts y Archivos Clave

El núcleo del juego reside en la carpeta `Assets/Scripts`.

| Archivo | Propósito Principal |
| :--- | :--- |
| **`EnemyMovement1.cs`** | Lógica base para Vagabundeo, Persecución y límites condicionales. |
| **`EnemyMovement3.cs`** | Implementación de las **reglas de la Bandada (Boids)** y los pesos de fuerza. |
| **`EnemyMovement2.cs`** | Simulación de la **Física Balística** para los proyectiles. |
| **`EnemyCodeControllerlvl3.cs`** | Lector de código que modifica las propiedades de los enemigos de la bandada. |
| **`SceneRestarter.cs`** | Utilidad para reiniciar el nivel con la tecla **`R`**. |
| **`BugbotCodePanel.cs`** | Controlador de la interfaz de usuario (abre/cierra con F1). |

## ⌨️ Guía de Comandos y Gameplay

Durante la ejecución del juego, utiliza estos comandos para interactuar y depurar:

| Tecla | Acción |
| :--- | :--- |
| **`F1`** | Abre/Cierra la **consola de código** y pausa/reanuda el movimiento del jugador. |
| **`R`** | **Reinicia** la escena o el nivel actual. |

## 🤝 Contribución y Licencia

Este proyecto fue desarrollado como un sandbox educativo para demostrar la interacción de la lógica de programación en tiempo real con la simulación de física y la inteligencia artificial de enjambre.

Si deseas contribuir, mejorar las optimizaciones de Boids (ej., implementando Quadtrees para la búsqueda de vecinos) o añadir nuevos sistemas de IA, ¡todas las contribuciones son bienvenidas!

### Contacto

Para consultas técnicas o reportes de fallos, contactar a través del sistema de Issues de este repositorio.

---

Licencia: El proyecto BOTS está licenciado bajo la **Licencia MIT**.
