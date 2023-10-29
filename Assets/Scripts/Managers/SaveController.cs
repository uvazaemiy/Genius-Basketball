using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    public static SaveController instance;
    
    private void Start()
    {
        instance = this;
    }

    public void SaveScene(int sceneID)
    {
        PlayerPrefs.SetInt("savedLevel", sceneID);
        PlayerPrefs.SetInt("levelCounts", PlayerPrefs.GetInt("levelCounts") + 1);
    }
}
