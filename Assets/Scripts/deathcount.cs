using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    public static int Deaths = 0;

    [SerializeField] private TextMeshProUGUI deathsText;

    void Start()
    {
        UpdateText();
    }

    public static void AddDeath()
    {
        Deaths++;
    }

    public void UpdateText()
    {
        if (deathsText != null)
            deathsText.text = "Muertes: " + Deaths;
    }
}