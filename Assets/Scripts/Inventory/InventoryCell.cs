using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] private InventoryItem prefabTemplate;
    [SerializeField] private int itemCount;
    [SerializeField] private Text itemCountText;
    [Space] 
    [SerializeField] private Image[] images;


    private Camera camera;
    private Vector3 screenPoint;

    private void Start()
    {
        camera = Camera.main;
        screenPoint = camera.WorldToScreenPoint(gameObject.transform.position);
        
        itemCountText.text = itemCount.ToString();
        
        VisibilityItem((itemCount != 0));
    }

    public void OnMouseDown()
    {
        if (itemCount > 0 && GameManager.instance.allowDrag && !GameManager.instance.finish)
        {
            Vector3 offset = transform.position - camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            Vector3 cursorScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 spawnPosition = camera.ScreenToWorldPoint(cursorScreenPoint) + offset;
            GameManager.instance.SpawnObstacle(prefabTemplate, spawnPosition, this);
            ChangeItemCount(-1);
        }
    }

    public void ChangeItemCount(int value)
    {
        itemCount += value;
        itemCountText.text = itemCount.ToString();
        
        VisibilityItem((itemCount != 0));
    }

    private void VisibilityItem(bool state)
    {
        foreach (Image image in images)
            image.enabled = state;
        itemCountText.enabled = state;
    }
}
