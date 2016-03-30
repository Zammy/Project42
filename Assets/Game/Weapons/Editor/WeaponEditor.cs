using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Activate"))
        {
            ((Weapon)target).IsActive = true;
        }

        if (GUILayout.Button("Deactivate"))
        {
            ((Weapon)target).IsActive = false;
        }
    }
}
