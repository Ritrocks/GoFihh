using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private KeyCode restartKey = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(restartKey))
        {
            RestartCurrentScene();
        }
    }

    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;

        if (FSM.Instance != null)
        {
            Destroy(FSM.Instance.gameObject);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
