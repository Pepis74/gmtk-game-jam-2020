using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public void Load()
    {
        SceneManager.LoadScene(1);
    }
}
