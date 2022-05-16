using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{ 
    public static T AddChildComponent<T>(this GameObject obj)where T:MonoBehaviour
    {
        //T로 전달받은 녀석의 타입이름으로 child 오브젝의 이름을 만듬
        GameObject child = new GameObject(typeof(T).Name);
        //child의 부모 오브젝트를 매개변수로 전달받은 obj로 만듬
        child.transform.SetParent(obj.transform);
        //child에게 T로 전달받은 컴포넌트를 부착해서 T를 반환
        return child.AddComponent<T>();
    }
}
