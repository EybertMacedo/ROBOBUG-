using UnityEngine;
using TMPro;

public class EnemyCodeReader : MonoBehaviour
{
    [Header("Referencia UI")]
    public TMP_InputField codeInput;  // InputField con el código
    public EnemyWaypointPatrol enemy; // El enemigo a controlar

    void Start()
    {
        if (codeInput != null)
        {
            // Leer el código al iniciar
            ApplyCode(codeInput.text);

            // Suscribirse a cambios en tiempo real
            codeInput.onValueChanged.AddListener(ApplyCode);
        }
    }

    void ApplyCode(string codeText)
    {
        if (enemy == null || string.IsNullOrEmpty(codeText))
            return;

        // Cambiar velocidad si existe
        float speed = GetFloatFromCode(codeText, "setSpeed");
        if (speed > 0)
            enemy.moveSpeed = speed;

        // Cambiar movimiento
        bool moving = GetBoolFromCode(codeText, "setIsMoving");
        enemy.SetMoving(moving);
    }

    // Extrae un float del código tipo "enemy.setSpeed = 5;"
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

    // Extrae un bool del código tipo "enemy.setIsMoving = True;"
    bool GetBoolFromCode(string text, string key)
    {
        string pattern = key + " = ";
        int index = text.IndexOf(pattern);
        if (index == -1) return true; // por defecto true

        int start = index + pattern.Length;
        int end = text.IndexOfAny(new char[] {';', '\n'}, start);
        if (end == -1) end = text.Length;

        string valueStr = text.Substring(start, end - start).Trim().ToLower();
        return valueStr == "true";
    }
}
