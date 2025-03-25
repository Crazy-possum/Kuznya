using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    
    public void GoToGameplayScene()
    {
        SceneManager.LoadScene(1);
    }
}
