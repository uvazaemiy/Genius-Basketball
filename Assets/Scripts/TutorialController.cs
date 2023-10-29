using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;
    
    [SerializeField] private SpriteRenderer Arrow;
    [SerializeField] private Text FirstText;
    [SerializeField] private SpriteRenderer ShieldTutor;
    [SerializeField] private Text SecondText;

    private void Start()
    {
        instance = this;
    }

    public IEnumerator DisableObjects()
    {
        if (Arrow != null)
        {
            FirstText.DOFade(0, 0.5f);
            yield return Arrow.DOFade(0, 0.5f).WaitForCompletion();
            Arrow.gameObject.SetActive(false);
            FirstText.gameObject.SetActive(false);
        }

        if (ShieldTutor != null)
        {
            SecondText.DOFade(0, 0.5f);
            yield return ShieldTutor.DOFade(0, 0.5f).WaitForCompletion();
            ShieldTutor.gameObject.SetActive(false);
            SecondText.gameObject.SetActive(false);
        }
    }

    public void EnableObjects()
    {
        if (Arrow != null)
        {
            Arrow.gameObject.SetActive(true);
            FirstText.gameObject.SetActive(true);
            FirstText.DOFade(1, 0.5f);
            Arrow.DOFade(1, 0.5f);
        }

        if (ShieldTutor != null)
        {
            ShieldTutor.gameObject.SetActive(true);
            SecondText.gameObject.SetActive(true);
            SecondText.DOFade(1, 0.5f);
            ShieldTutor.DOFade(1, 0.5f);
        }
    }
}
