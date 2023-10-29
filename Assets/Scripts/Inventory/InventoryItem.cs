using System.Collections;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public bool isShield;
    [Header("Cannon settings")]
    public bool isCannon;
    [SerializeField] private Transform CannonBallPosition;
    [SerializeField] private Transform CannonDirectionPosition;
    [SerializeField] private Animation CannonBombAnimation;
    private PolygonCollider2D mainCollider;
    private BoxCollider2D addCollider;
    [Space]
    [Header("Main settings")]
    [HideInInspector] public InventoryCell cell;
    public SpriteRenderer[] images;
    [SerializeField] private Sprite[] mainSprites;
    [SerializeField] private Sprite[] redSprites;
    [SerializeField] private RotateChecker checker;
    
    private Camera camera;
    private Vector3 screenPoint;
    private Vector3 offset;
    public Vector3 prevPosition;
    
    private bool allowMoving = true;
    private bool crossInventory;
    private bool cross;

    private void Start()
    {
        SoundController.instance.PlayClick1();

        camera = Camera.main;
        screenPoint = camera.WorldToScreenPoint(gameObject.transform.position);

        if (isCannon)
        {
            mainCollider = GetComponent<PolygonCollider2D>();
            addCollider = GetComponent<BoxCollider2D>();
            GameManager.instance.CannonBallPosition = CannonBallPosition;
            GameManager.instance.CannonDirectionPosition = CannonDirectionPosition;
            GameManager.instance.CannonBombAnimation = CannonBombAnimation;
        }
    }

    private void Update()
    {
        if (allowMoving)
        {
            if (CameraMoving.instance != null)
                CameraMoving.instance.allowDrag = false;
            
            Vector3 cursorScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = camera.ScreenToWorldPoint(cursorScreenPoint) + offset;
            transform.position = cursorPosition;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (!cross)
                allowMoving = false;
            else
            {
                if (prevPosition != Vector3.zero && !crossInventory)
                {
                    transform.position = prevPosition;
                    cross = false;
                    ChangeColor(1, 1, 1, 1, mainSprites);
                    allowMoving = false;
                }
                else
                {
                    cell.ChangeItemCount(1);
                    GameManager.instance.ObjectCount--;
                    Destroy(gameObject);
                }
            }
            
            if (isCannon)
            {
                mainCollider.enabled = true;
                addCollider.enabled = false;
            }

            if (GameManager.instance.allowDrag && CameraMoving.instance != null)
            {
                CameraMoving.instance.allowDrag = true;
                CameraMoving.instance.isDragging = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.allowDrag && !GameManager.instance.finish)
        {
            SoundController.instance.PlayClick1();

            offset = transform.position - camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            prevPosition = transform.position;
            
            allowMoving = true;
            
            if (isCannon)
            {
                mainCollider.enabled = false;
                addCollider.enabled = true;
            }
        }
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.allowDrag)
        {
            SoundController.instance.PlayClick2();

            if (prevPosition != null)
                if (Vector3.Distance(prevPosition, transform.position) < 0.1f)
                {
                    if (!checker.state)
                    {
                        if (isCannon)
                            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y,
                                transform.localScale.z);
                        else
                            transform.Rotate(new Vector3(0, 0, -90));
                    }
                    else
                        StartCoroutine(ChangeColorRoutine());
                }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (allowMoving)
        {
            cross = true;
            
            if (col.gameObject.tag == "Inventory")
            {
                crossInventory = true;
                ChangeColor(1, 1, 1, 0.7f, mainSprites);
            }
            else
                ChangeColor(1, 1, 1, 1, redSprites);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (allowMoving)
        {
            cross = false;
            crossInventory = false;
            
            ChangeColor(1, 1, 1, 1, mainSprites);
        }
    }

    private void ChangeColor(float r, float g, float b, float a, Sprite[] sprites)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(r, g, b, a);
            images[i].sprite = sprites[i];
        }
    }

    private IEnumerator ChangeColorRoutine()
    {
        ChangeColor(1, 1, 1, 1, redSprites);
        yield return new WaitForSeconds(1);
        ChangeColor(1, 1, 1, 1, mainSprites);
    }
}
