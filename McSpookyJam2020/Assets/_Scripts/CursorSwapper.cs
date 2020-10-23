using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorSwapper : EventTrigger
{
    
    [SerializeField] 
    private Texture2D cursor;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Cursor.SetCursor (cursor, Vector2.zero, CursorMode.Auto);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}
