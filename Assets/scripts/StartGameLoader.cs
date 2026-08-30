using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartGameLoader : MonoBehaviour
{
    [SerializeField] private String sceneToLoad;
    /* [SerializeField] private bool loadOnStart = true;

    private void Start()
    {
        if (loadOnStart)
        {
            LoadSelectedScene();
        }
    } */

    public void LoadSelectedScene()
    {
        if (sceneToLoad == null)
        {
            Debug.LogWarning("StartGameLoader: No scene assigned. Select a scene in the Inspector.");
            return;
        }

        if (SceneManager.GetActiveScene().name == sceneToLoad)
        {
            
            return;
        }
        Time.timeScale = 1f;

        if (FSM.Instance != null)
        {
            Destroy(FSM.Instance.gameObject);
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
