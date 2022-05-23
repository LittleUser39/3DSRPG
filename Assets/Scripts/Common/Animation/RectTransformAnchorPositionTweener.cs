using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformAnchorPositionTweener : Vector3Tweener
{
    RectTransform rt;
    void Awake()
    {
        rt = transform as RectTransform;
    }

    // 매프레임마다 rt의 위치값을 변경시킨다.
    protected override void OnUpdate()
    {
        base.OnUpdate();
        rt.anchoredPosition = currentTweenValue;
    }
}
