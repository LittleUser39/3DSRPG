using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationPanel : MonoBehaviour
{
    //대화 내용
    public Text message;

    //말하는 사람의 아바타
    public Image speaker;

    //다음 내용이 남아있으면 표시할 UI
    public GameObject arrow;

    //대화 상자
    public Panel panel;
    private void Start()
    {
        //시작시 arrow를 살짝 위로 올림
        Vector3 pos = arrow.transform.localPosition;
        arrow.transform.localPosition = new Vector3(pos.x, pos.y + 5, pos.z);

        //화살표가 위아래로 움직이는 연출
        Tweener t = arrow.transform.MoveToLocal(new Vector3(pos.x, pos.y - 5, pos.z), 0.5f, EasingEquations.EaseInQuad);
        t.loopType = EasingControl.LoopType.PingPong;
        t.loopCount = -1;
    }

    public IEnumerator Display(SpeakerData sd)
    {
        //말하는 사람의 이미지를 변경
        speaker.sprite = sd.speaker;

        //해당 이미지의 크기를 원래 크기로 변경
        speaker.SetNativeSize();

        for(int i=0;i<sd.messages.Count;++i)
        {
            //대화 내용을 저장
            message.text = sd.messages[i];

            //마지막 대사이면 화살표를 비활성화
            arrow.SetActive(i + 1 < sd.messages.Count);

            //한프레임 쉼
            yield return null;
        }
    }
}
