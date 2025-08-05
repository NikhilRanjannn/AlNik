using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip errorSound;
    public AudioClip winSound;

    private CardUI firstCard = null;
    private CardUI secondCard = null;
    private bool canClick = false;

    private List<CardUI> allCards = new List<CardUI>();

    [Header("UI Elements")]
    public Text scoreText;
    public Text bestScoreText;
    public Text attemptsText;

    private int score = 0;
    private int attempts = 0;
    private int bestScore = 0;

    private int totalPairs = 0;
    private int matchedPairs = 0;

    void Start()
    {
        allCards.AddRange(FindObjectsOfType<CardUI>());
        totalPairs = allCards.Count / 2;

        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateUI();
        StartCoroutine(PreviewCardsAtStart());
    }

    IEnumerator PreviewCardsAtStart()
    {
        foreach (var card in allCards)
        {
            card.FlipOpen();
        }

        yield return new WaitForSeconds(2.5f);

        foreach (var card in allCards)
        {
            card.FlipClose();
        }

        canClick = true;
    }

    public void OnCardSelected(CardUI selectedCard)
    {
        if (!canClick || selectedCard == firstCard || selectedCard.frontImage.gameObject.activeSelf)
            return;

        selectedCard.FlipOpen();

        if (firstCard == null)
        {
            firstCard = selectedCard;
        }
        else
        {
            secondCard = selectedCard;
            canClick = false;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.6f);

        if (firstCard.cardId == secondCard.cardId)
        {
            // Play win sound
            if (audioSource && winSound)
                audioSource.PlayOneShot(winSound);

            // Play match animation
            firstCard.PlayMatchAnimation();
            secondCard.PlayMatchAnimation();

            score++;
            matchedPairs++;

            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt("BestScore", bestScore);
            }

            // Wait for animation before continue
            yield return new WaitForSeconds(0.5f);

            if (matchedPairs >= totalPairs)
            {
                Debug.Log("All cards matched. Loading next scene...");
                yield return new WaitForSeconds(1f);
                LoadNextScene();
            }
        }
        else
        {
            // Play error sound
            if (audioSource && errorSound)
                audioSource.PlayOneShot(errorSound);

            // Play shake animation
            firstCard.PlayMismatchAnimation();
            secondCard.PlayMismatchAnimation();

            yield return new WaitForSeconds(0.5f); 

            firstCard.FlipClose();
            secondCard.FlipClose();
            attempts++;
        }

        firstCard = null;
        secondCard = null;
        canClick = true;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score: " + score;
        if (bestScoreText) bestScoreText.text = "Best: " + bestScore;
        if (attemptsText) attemptsText.text = "Fails: " + attempts;
    }

    void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.Log("No more scenes in build settings.");
    }
}
