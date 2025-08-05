using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image frontImage;
    public Image backImage;
    public Button button;
    public int cardId;

    [Header("References")]
    public AudioSource audioSource;
    public AudioClip flipSound;
    

    private GameManager gameManager;

    [Header("Animation Settings")]
    public float zoomOutDuration = 0.5f;
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 10f;
    public float flipDuration = 0.3f;

    private bool isFlipping = false;

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
        if (!isFlipping)
            StartCoroutine(SmoothFlip(true));
    }

    public void FlipClose()
    {
        if (!isFlipping)
            StartCoroutine(SmoothFlip(false));
    }

    private IEnumerator SmoothFlip(bool opening)
    {
        isFlipping = true;
        float time = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion midRotation = Quaternion.Euler(0, 90, 0);
        Quaternion endRotation = Quaternion.Euler(0, 0, 0);

        // Step 1: Rotate to 90 & hide 
        while (time < flipDuration / 2)
        {
            transform.rotation = Quaternion.Lerp(startRotation, midRotation, time / (flipDuration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = midRotation;

        // Step 2: Switch card face
        if (opening)
        {
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);
        }
        else
        {
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);
        }

        // flip sound
        if (audioSource && flipSound)
            audioSource.PlayOneShot(flipSound);

        // Step 3: Rotate back to 0 original face
        time = 0f;
        while (time < flipDuration / 2)
        {
            transform.rotation = Quaternion.Lerp(midRotation, endRotation, time / (flipDuration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        isFlipping = false;
    }

    // Match animation
    public void PlayMatchAnimation()
    {

        StartCoroutine(ZoomOutAndDestroy());
    }

    // Mismatch animation
    public void PlayMismatchAnimation()
    {
        StartCoroutine(ShakeCard());
    }

    private IEnumerator ZoomOutAndDestroy()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float time = 0f;

        while (time < zoomOutDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, time / zoomOutDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        Destroy(gameObject);
    }

    private IEnumerator ShakeCard()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.localPosition = originalPos + new Vector3(x, 0f, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
