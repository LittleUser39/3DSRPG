using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Editor 타겟 대상
//컴포넌트에 대한 사용자 정의 에디터를 만들때 사용하는 코드
[CustomEditor(typeof(TileCreator))]
public class TileCreatorInspector : Editor
{
public TileCreator current
    {
        get
        {
            //타겟은 tilecreator
            return (TileCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {
        //inspector 에 버튼을 추가 하고 싶을때 쓰는 함수
        DrawDefaultInspector();

        if (GUILayout.Button("Clear"))
            current.Clear();

        if (GUILayout.Button("Grow"))
            current.Grow();
        
        if (GUILayout.Button("Shrink"))
            current.Shrink();
        
        if (GUILayout.Button("GrowArea"))
            current.GrowArea();
        
        if (GUILayout.Button("ShrinkArea"))
            current.ShrinkArea();

        if (GUILayout.Button("Save"))
            current.Save();

        if (GUILayout.Button("Load"))
            current.Load();

        if (GUI.changed)
            current.UpdateMarker();
    }
}
