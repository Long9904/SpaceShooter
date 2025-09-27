using UnityEngine;
using UnityEngine.SceneManagement;
public class UICanvasMenu : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManagement.instance.PlayStartMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickButtonEvent(string sceneName)
    {
        AudioManagement.instance.PlayButtonClick();
        SceneManager.LoadScene(sceneName);
    }
}
