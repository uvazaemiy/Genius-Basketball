using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Animation bombAnim;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!GameManager.instance.allowDrag && other.gameObject.layer == 6)
        {
            bombAnim.Play();
            SoundController.instance.PlayBombSound();
            GameManager.instance.RestartGame();
        }
    }
}
