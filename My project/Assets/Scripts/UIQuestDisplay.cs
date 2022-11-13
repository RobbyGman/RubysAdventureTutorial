using UnityEngine;
using UnityEngine.UI;

public class UIQuestDisplay : MonoBehaviour
{
    public static UIQuestDisplay Instance { get; private set; }
    
    public Text questText;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void SetText(string text)
    {
        questText.text = text;
    }
}
