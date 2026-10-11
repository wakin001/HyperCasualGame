using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        if (!RopeCtrl.instance.isRunning)
        {
            return;
        }
        RopeCtrl.instance.fingerImage.gameObject.SetActive(true);
        RopeCtrl.instance.UpdateFingerPos(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RopeCtrl.instance.fingerImage.gameObject.SetActive(false);
    }
}
