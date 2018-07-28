using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
// 继承：按下，抬起和离开的三个接口

public class ButtomEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,IPointerEnterHandler

{
    public bool isDown = false;
    public void OnPointerDown(PointerEventData eventData)

    {
        isDown = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }
    public void OnPointerExit(PointerEventData eventData)

    {
        print(1);
        isDown = false;
    }
}
