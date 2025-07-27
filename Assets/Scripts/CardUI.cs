using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;
    public Button button;
    public int cardId;  

    private GameManager gameManager;

    void Start()
    {
        button.onClick.AddListener(OnCardClicked);
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnCardClicked()
    {
        gameManager.OnCardSelected(this);
    }

    public void FlipOpen()
    {
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
    }

    public void FlipClose()
    {
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
    }
}
