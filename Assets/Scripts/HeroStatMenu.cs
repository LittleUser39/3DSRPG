using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStatMenu : MonoBehaviour
{
    //필요한 스텟 (레벨,체력,전체 체력,방어력,마법공격,마법방어,스피드,회피)
    public GameObject heroContainer;
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    public Text text5;
    public Text text6;
    public Text text7;
    public Text text8;
    
   
    private void Start()
    {
        heroContainer = GameObject.FindGameObjectWithTag("Container");
        SetStat();
    }
    void SetStat()
    {
        
        if (heroContainer == null)
            return;
       // for(int i=0;i<heroContainer.transform.childCount;++i)
       // {
            GameObject hero = heroContainer.transform.GetChild(1).gameObject;
            Stats stats = hero.GetComponent<Stats>();
            text1.text = stats[StateTypes.LVL].ToString();
            text2.text = stats[StateTypes.HP].ToString();
            text3.text = stats[StateTypes.MHP].ToString();
            text4.text = stats[StateTypes.ATK].ToString();
            text5.text = stats[StateTypes.DEF].ToString();
            text6.text = stats[StateTypes.MAT].ToString();
            text7.text = stats[StateTypes.MDF].ToString();
            text8.text = stats[StateTypes.SPD].ToString();
        //}
    }  
}
