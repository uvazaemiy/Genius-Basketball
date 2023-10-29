using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform currentPlatform;
    [SerializeField] private float Time;
    [Space]
    [SerializeField] Transform firstPosition;
    [SerializeField] Transform secondPosition;

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        yield return new WaitForEndOfFrame();/*

        if (GameManager.instance.isGame)
        {*/
            yield return currentPlatform.DOMove(secondPosition.position, Time).WaitForCompletion();
            yield return currentPlatform.DOMove(firstPosition.position, Time).WaitForCompletion();
        //}
        
        StartCoroutine(MoveRoutine());
    }
}
