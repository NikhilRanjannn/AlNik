using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
       
        SceneManager.LoadScene("Menu");  
    }
}
