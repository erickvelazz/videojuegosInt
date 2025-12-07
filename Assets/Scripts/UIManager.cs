using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; 

    [Header("--- VIDAS (Corazones) ---")]
    public Image[] hearts; 

    [Header("--- FLECHAS ---")]
    public TextMeshProUGUI arrowCountText;
    public Image arrowIcon; 
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    [Header("--- MONEDAS ---")]
    public TextMeshProUGUI coinCountText;
    private int totalCoins = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
            {
                hearts[i].enabled = true; 
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void UpdateArrows(int amount)
    {
        arrowCountText.text = amount.ToString();

        if (amount > 0)
        {
            arrowIcon.color = activeColor;
        }
        else
        {
            arrowIcon.color = inactiveColor;
        }
    }

    public void AddCoin()
    {
        totalCoins++;
        coinCountText.text = totalCoins.ToString();
    }
}