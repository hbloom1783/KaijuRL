using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KaijuRL.Map;

//[CustomEditor(typeof(MapMobile))]
/*public class MapMobileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapMobile myTarget = (MapMobile)target;

        myTarget.occupiesSpace = EditorGUILayout.Toggle("Occupies Space", myTarget.occupiesSpace);

        myTarget.facingSprites[Facing.ne] = (Sprite)EditorGUILayout.ObjectField(
            myTarget.facingSprites[Facing.ne],
            typeof(Sprite), 
            false);

        myTarget.facingSprites[Facing.e] = (Sprite)EditorGUILayout.ObjectField(
            "E Sprite",
            myTarget.facingSprites[Facing.e],
            typeof(Sprite), 
            false);

        myTarget.facingSprites[Facing.se] = (Sprite)EditorGUILayout.ObjectField(
            "SE Sprite",
            myTarget.facingSprites[Facing.se], 
            typeof(Sprite), 
            false);

        myTarget.facingSprites[Facing.sw] = (Sprite)EditorGUILayout.ObjectField(
            "SW Sprite",
            myTarget.facingSprites[Facing.sw], 
            typeof(Sprite), 
            false);

        myTarget.facingSprites[Facing.w] = (Sprite)EditorGUILayout.ObjectField(
            "W Sprite", 
            myTarget.facingSprites[Facing.w], 
            typeof(Sprite), 
            false);

        myTarget.facingSprites[Facing.nw] = (Sprite)EditorGUILayout.ObjectField(
            "NW Sprite", 
            myTarget.facingSprites[Facing.nw], 
            typeof(Sprite),
            false);
    }
}*/
