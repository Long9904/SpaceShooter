using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadNextScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = true;
        //SceneController.Instance.NextLevel(); // Use SceneController to load next level

    }
}
