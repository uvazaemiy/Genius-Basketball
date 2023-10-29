using UnityEngine;
using UnityEngine.Serialization;

public class RotateChecker : MonoBehaviour
{
    public bool state;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance.allowDrag)
            state = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        state = false;
    }
}
