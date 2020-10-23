using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CursorSwapper : EventTrigger
{
    public Texture2D cursor;
    
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
    
    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(CursorSwapper))]
public class MenuButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CursorSwapper targetCursorSwapper = (CursorSwapper)target;
            
        targetCursorSwapper.cursor = (Texture2D)EditorGUILayout.ObjectField("Cursor", targetCursorSwapper.cursor, typeof(Texture2D), true);
        //component.onSprite = (Sprite)EditorGUILayout.ObjectField("On Sprite", component.onSprite, typeof(Sprite), true);
 
        // Show default inspector property editor
        DrawDefaultInspector();
    }
}
#endif
