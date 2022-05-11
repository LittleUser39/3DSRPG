using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    //스태틱으로 되어 다른 클래스에서 쉽게 접근할수 있음
    public static void CreateAsset<T>() where T:ScriptableObject
    {
        //scriptableobject 인스턴틑 만듬
        T asset = ScriptableObject.CreateInstance<T>();

       //저장할 파일의 경로
       //selection 는 현재 선택된 경로
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
            path = "Assets";

        //지정된 경로 문자열에서 확장명을 반환
        else if(Path.GetExtension(path)!="")
        {
            //확장명이 있으면 잘못된 경로
            //해당 파일이 있는 폴더 경로를 불러옴
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        //해당 경로가 중복되었으면 파일 뒤에 숫자를 붙여
        //유이크한 경로를 만들어냄
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New" + typeof(T).ToString() + ".asset");

        //에셋을 생성
        AssetDatabase.CreateAsset(asset, assetPathAndName);

        //에섯을 저장
        AssetDatabase.SaveAssets();

        //변경사항을 임포트
        AssetDatabase.Refresh();

        //저장된 경로에 포커스를 맞춤
        //project 뷰가 해당 파일이 생긴 위치로 이동
        EditorUtility.FocusProjectWindow();

        //해당 폴더를 선택상태로 만듬 (클릭한것 처럼)
        Selection.activeObject = asset;
    }
}
