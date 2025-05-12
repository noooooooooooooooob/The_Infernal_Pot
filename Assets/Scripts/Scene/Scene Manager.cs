using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        // 현재 씬 이름을 가져옵니다.
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // 현재 씬을 리로드합니다.
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
    }
}