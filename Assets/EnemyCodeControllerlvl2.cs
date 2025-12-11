using UnityEngine;
using TMPro;

public class EnemyCodeControllerlvl2 : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TMP_InputField codeInput;  // Campo donde el jugador escribe el código
    
    [Header("Referencia a los Enemigos")]
    // Referencia al script que controla el movimiento. 
    // Si hay múltiples enemigos, este script debe estar en cada uno, o un controlador central debe iterar sobre ellos.
    public EnemyMovement1 enemy; 

    void Start()
    {
        if (codeInput != null)
        {
            // Aplicar el código inicial al iniciar el nivel
            ApplyCode(codeInput.text);

            // Suscribirse al evento de cambio de texto para aplicar la IA en tiempo real
            codeInput.onValueChanged.AddListener(ApplyCode);
        }
    }

    void ApplyCode(string codeText)
    {
        if (enemy == null || string.IsNullOrEmpty(codeText))
            return;

        // Dividir el código para procesar cada comando
        string[] commands = codeText.Split(new char[] { ';', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string command in commands)
        {
            ProcessCommand(command.Trim());
        }
    }

    void ProcessCommand(string commandLine)
    {
        // Buscar el patrón: propiedad = valor
        string[] parts = commandLine.Split('=');

        if (parts.Length != 2) return; 

        string propertyName = parts[0].Trim().ToLower();
        string valueStr = parts[1].Trim();
        
        // Limpiar el prefijo 'enemy.' si existe
        if (propertyName.StartsWith("enemy."))
        {
            propertyName = propertyName.Substring(6).Trim();
        }
        
        // Usaremos TryParse para extraer valores de forma segura
        
        switch (propertyName)
        {
            case "setmovementenabled":
                if (bool.TryParse(valueStr, out bool enabled))
                    enemy.SetMovementEnabled(enabled);
                else if (valueStr.ToLower() == "true")
                    enemy.SetMovementEnabled(true);
                else if (valueStr.ToLower() == "false")
                    enemy.SetMovementEnabled(false);
                break;
                
            case "setwanderdistancemax":
                if (float.TryParse(valueStr, out float distance) && distance >= 0)
                    enemy.wanderDistanceMax = distance;
                break;
                
            case "setdetectionrange":
                if (float.TryParse(valueStr, out float detectionRange) && detectionRange > 0)
                    enemy.SetDetectionRange(detectionRange);
                break;
                
            case "setchasespeed":
                if (float.TryParse(valueStr, out float chaseSpeed) && chaseSpeed >= 0)
                    enemy.SetChaseSpeed(chaseSpeed);
                break;

            case "wanderspeed":
                if (float.TryParse(valueStr, out float wanderSpeed) && wanderSpeed >= 0)
                    enemy.wanderSpeed = wanderSpeed;
                break;
                
            case "setturnsmoothness":
                if (float.TryParse(valueStr, out float smoothness) && smoothness > 0)
                    enemy.turnSmoothness = smoothness;
                break;
            
            default:
                // Ignorar comandos no reconocidos
                break;
        }
    }
}