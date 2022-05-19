using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//패널에 능력치를 표시하는 클래스
public class StatPanel : MonoBehaviour
{
    //유닛 정보 UI 관련 
    public Panel panel;
    public Sprite allyBackground;
    public Sprite enemyBackground;
    public Image background;
    public Image avatar;
    public Text nameLable;
    public Text hpLable;
    public Text mpLable;
    public Text lvLable;

    public void Display(GameObject obj)
    {
        //테스트 용도
        //50% 확률로 적 백그라운드, 아군 백그라운드 사용
        //background.sprite = UnityEngine.Random.value > 0.5f ? enemyBackground : allyBackground;

        //오브젝트들의 동맹상태를 참조
        Alliance alliance = obj.GetComponent<Alliance>();
        //동맹이 적군이면 enemybackgroun를 아니면 allybackground 사용
        background.sprite = alliance.type == Alliances.Enemy ? enemyBackground : allyBackground;

        //avatar 이미지를 유닛 개별로 사용할 경우 아래 코드를 활성화
        //avatar.sprite = null; Need a component which provides this data

        //유닛의 이름
        nameLable.text = obj.name;

        //능력치 표시
        Stats stats = obj.GetComponent<Stats>();

        if(stats)
        {
            hpLable.text = string.Format("HP{0}/{1}", stats[StateTypes.HP], stats[StateTypes.MHP]);
            mpLable.text = string.Format("MP{0}/{1}", stats[StateTypes.MP], stats[StateTypes.MMP]);
            lvLable.text = string.Format("LV.{0}", stats[StateTypes.LVL]);
        }
    }

}
