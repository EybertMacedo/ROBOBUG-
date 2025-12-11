using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyCodeControllerlvl3 : EnemyCodeReader
{
    [Header("Control de Enemigos de Bandada")]
    private List<EnemyMovement3> enemies; 

    new void Start()
    {
        // 1. Encontrar todos los enemigos de bandada en la escena
        EnemyMovement3[] enemyArray = FindObjectsOfType<EnemyMovement3>();
        enemies = new List<EnemyMovement3>(enemyArray);

        if (enemies.Count == 0)
        {
            Debug.LogError("No se encontraron objetos con el script EnemyMovement3.");
            return;
        }

        // 2. Suscribirse a los cambios del InputField (usando la referencia codeInput heredada)
        if (codeInput != null)
        {
            ApplyCode(codeInput.text);
            codeInput.onValueChanged.AddListener(ApplyCode);
        }
    }

    new void ApplyCode(string codeText)
    {
        if (enemies == null || enemies.Count == 0 || string.IsNullOrEmpty(codeText))
            return;

        string[] commands = codeText.Split(new char[] { ';', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        
        float floatValue = -1f;
        bool boolValue = false;
        
        foreach (string command in commands)
        {
            string cleanCommand = command.Trim();
            if (string.IsNullOrEmpty(cleanCommand)) continue;

            string propertyName = "";
            
            // --- Lógica de Extracción Local ---
            
            // Reutilizando las funciones de extracción de float y bool, definidas localmente.
            
            if ((floatValue = GetFloatFromCode(cleanCommand, "setSeparationWeight")) != -1)
            {
                propertyName = "separationWeight";
            }
            else if ((floatValue = GetFloatFromCode(cleanCommand, "setCohesionWeight")) != -1)
            {
                propertyName = "cohesionWeight";
            }
            else if ((floatValue = GetFloatFromCode(cleanCommand, "setAlignmentWeight")) != -1)
            {
                propertyName = "alignmentWeight";
            }
            else if ((floatValue = GetFloatFromCode(cleanCommand, "setTargetWeight")) != -1)
            {
                propertyName = "targetWeight";
            }
            else if ((floatValue = GetFloatFromCode(cleanCommand, "setSeparationDistance")) != -1)
            {
                propertyName = "separationDistance";
            }
            else if ((floatValue = GetFloatFromCode(cleanCommand, "setMaxSpeed")) != -1)
            {
                propertyName = "maxSpeed";
            }
            else if ((floatValue = GetFloatFromCode(cleanCommand, "setSpeed")) != -1)
            {
                propertyName = "maxSpeed"; 
            }
            else if (cleanCommand.ToLower().Contains("setismoving"))
            {
                boolValue = GetBoolFromCode(cleanCommand, "setIsMoving");
                propertyName = "isMoving";
            }
            
            // -----------------------------------------------------------------
            // APLICACIÓN DE LOS CAMBIOS A TODOS LOS ENEMIGOS DE LA BANDADA
            // -----------------------------------------------------------------
            
            if (propertyName == "") continue;

            foreach (EnemyMovement3 enemy in enemies)
            {
                if (enemy == null) continue;

                switch (propertyName)
                {
                    case "separationWeight":
                        if (floatValue >= 0) enemy.SetSeparationWeight(floatValue);
                        break;
                    case "cohesionWeight":
                        if (floatValue >= 0) enemy.SetCohesionWeight(floatValue);
                        break;
                    case "alignmentWeight":
                        if (floatValue >= 0) enemy.SetAlignmentWeight(floatValue);
                        break;
                    case "targetWeight":
                        if (floatValue >= 0) enemy.SetTargetWeight(floatValue);
                        break;
                        
                    case "separationDistance":
                        if (floatValue > 0) enemy.SetSeparationDistance(floatValue);
                        break;
                        
                    case "maxSpeed":
                        if (floatValue >= 0) enemy.SetMaxSpeed(floatValue);
                        break;
                        
                    case "isMoving":
                        enemy.SetMovementEnabled(boolValue); 
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }

    // -----------------------------------------------------------------
    // RECREACIÓN DE FUNCIONES DE EXTRACCIÓN
    // -----------------------------------------------------------------

    float GetFloatFromCode(string text, string key)
    {
        string pattern = key + " = ";
        int index = text.IndexOf(pattern);
        if (index == -1) return -1;

        int start = index + pattern.Length;
        int end = text.IndexOfAny(new char[] {';', '\n'}, start);
        if (end == -1) end = text.Length;

        string valueStr = text.Substring(start, end - start).Trim();
        
        if (float.TryParse(valueStr, out float result))
            return result;
        return -1;
    }

    bool GetBoolFromCode(string text, string key)
    {
        string pattern = key + " = ";
        int index = text.IndexOf(pattern);
        if (index == -1) return true;

        int start = index + pattern.Length;
        int end = text.IndexOfAny(new char[] {';', '\n'}, start);
        if (end == -1) end = text.Length;

        string valueStr = text.Substring(start, end - start).Trim().ToLower();
        return valueStr == "true";
    }
}