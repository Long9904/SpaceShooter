using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    [SerializeField] Animator animator;
    [SerializeField] GameObject transitionPrefab;
    private bool isLoading = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (animator == null)
            {
                GameObject transObj = Instantiate(transitionPrefab);
                DontDestroyOnLoad(transObj);
                animator = transObj.GetComponent<Animator>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void NextLevel()
    {
        if (!isLoading)
            StartCoroutine(LoadLevel());
    }
    IEnumerator LoadLevel()
    {
        AudioManagement.instance.StopMusic();
        isLoading = true;
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetTrigger("Start");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
 
}
