using System.Collections;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraMoving : MonoBehaviour
{
    public static CameraMoving instance;
    
    [SerializeField] private Transform movePosition;
    public bool allowDrag;
    public bool isDragging;
    [Range(0, 0.5f)]
    [SerializeField] private float timeOfLerp;
    [Space]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float leftLimit = -10f;
    [SerializeField] private float rightLimit = 10f;
    
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;


    private void Start()
    {
        instance = this;
        
        StartCoroutine(StartMoving());
    }

    private IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(2);

        yield return transform.DOMove(movePosition.position, 1).WaitForCompletion();
        allowDrag = true;
    }

    private void Update()
    {
        if (allowDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                fingerDownPosition = Input.mousePosition;
            }

            if (isDragging)
            {
                Vector2 offset = (Vector2)Input.mousePosition - fingerDownPosition;

                transform.position = CheckLimits(offset.x);

                fingerDownPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
                isDragging = false;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isGame)
        {
            Vector3 ballPos = new Vector3(GameManager.instance.balls[0].transform.position.x, 0, -10);
            
            transform.position = Vector3.Lerp(transform.position, ballPos, timeOfLerp);
            
            transform.position = CheckLimits();
        }

    }

    private Vector3 CheckLimits(float xOffset = 0)
    {
        Vector3 pos = transform.position;
        pos.x += xOffset * speed * Time.deltaTime;

        if (pos.x < leftLimit)
        {
            pos.x = leftLimit;
        }
        else if (pos.x > rightLimit)
        {
            pos.x = rightLimit;
        }

        return pos;
    }
}
