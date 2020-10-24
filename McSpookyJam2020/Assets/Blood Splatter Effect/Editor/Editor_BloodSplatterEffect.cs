using UnityEngine;
using System.Collections;
using UnityEditor;

public class Editor_BloodSplatterEffect : Editor
{
    [MenuItem("Window/Blood Splatter Effect/Create")]
    public static void CreateBloodSplatter()
    {
        GameObject template = (GameObject)Resources.Load("EffectTemplate");
        if (template != null)
        {
            GameObject clone = Instantiate(template);
            clone.name = "BloodSplatterEffect";
            Selection.activeGameObject = clone;
            Undo.RegisterCreatedObjectUndo(clone, "Created BloodSplatterEffect");
        }
        else
        {
            Debug.LogWarning("The blood splatter effect template could not be found! Consider re-importing the asset");
        }
    }
}
