using UnityEngine;

public class DeleteAllPrefs : MonoBehaviour
{
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All prefs have been deleted!");
    }
}
