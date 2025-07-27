using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;  

public class MainMenu : MonoBehaviour  
{
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    public void OnPlayButtonClicked()
    {
        if (audioSource && buttonClickSound)
        {
            audioSource.PlayOneShot(buttonClickSound);
            StartCoroutine(DelayedStartGame());  
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator DelayedStartGame()
    {
        yield return new WaitForSeconds(buttonClickSound.length);
        SceneManager.LoadScene(1);
    }
}
