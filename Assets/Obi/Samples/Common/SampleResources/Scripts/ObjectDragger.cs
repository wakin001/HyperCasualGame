using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ObjectDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
 
	private Vector3 screenPoint;
	private Vector3 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        transform.position = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
 }