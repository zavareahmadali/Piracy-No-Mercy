using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    void Start()
    {
        
    }

    public void StartMainLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
