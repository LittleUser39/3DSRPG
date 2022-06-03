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
    public Text text9;
    public Text text10;
    public Text text11;
    public Text text12;
    public Text text13;
    public Text text14;
    public Text text15;
    public Text text16;
    public Text text17;
    public Text text18;
    public Text text19;
    public Text text20;
    public Text text21;
    public Text text22;
    public Text text23;
    public Text text24;
   
    private void Start()
    {
        heroContainer = GameObject.FindGameObjectWithTag("Container");
        SetStat();
    }
    public void SetStat()
    {
        if (heroContainer == null)
            return;
   
        GameObject hero = heroContainer.transform.GetChild(0).gameObject;
        Stats stats = hero.GetComponent<Stats>();
        text1.text = string.Format("LVL : {0}" , stats[StateTypes.LVL].ToString());
        text2.text = string.Format("HP : {0}" , stats[StateTypes.HP].ToString());
        text3.text = string.Format("MHP : {0}" ,stats[StateTypes.MHP].ToString());
        text4.text = string.Format("ATK : {0}" ,stats[StateTypes.ATK].ToString());
        text5.text = string.Format("DEF : {0}" ,stats[StateTypes.DEF].ToString());
        text6.text = string.Format("MAT : {0}" ,stats[StateTypes.MAT].ToString());
        text7.text = string.Format("MDF : {0}" ,stats[StateTypes.MDF].ToString());
        text8.text = string.Format("SPD : {0}", stats[StateTypes.SPD].ToString());

        hero = heroContainer.transform.GetChild(1).gameObject;
        stats = hero.GetComponent<Stats>();

        text9.text = string.Format("LVL : {0}", stats[StateTypes.LVL].ToString());
        text10.text = string.Format("HP : {0}", stats[StateTypes.HP].ToString());
        text11.text = string.Format("MHP : {0}", stats[StateTypes.MHP].ToString());
        text12.text = string.Format("ATK : {0}", stats[StateTypes.ATK].ToString());
        text13.text = string.Format("DEF : {0}", stats[StateTypes.DEF].ToString());
        text14.text = string.Format("MAT : {0}", stats[StateTypes.MAT].ToString());
        text15.text = string.Format("MDF : {0}", stats[StateTypes.MDF].ToString());
        text16.text = string.Format("SPD : {0}", stats[StateTypes.SPD].ToString());
       
        hero = heroContainer.transform.GetChild(2).gameObject;
        stats = hero.GetComponent<Stats>();

        text17.text = string.Format("LVL : {0}", stats[StateTypes.LVL].ToString());
        text18.text = string.Format("HP : {0}", stats[StateTypes.HP].ToString());
        text19.text = string.Format("MHP : {0}", stats[StateTypes.MHP].ToString());
        text20.text = string.Format("ATK : {0}", stats[StateTypes.ATK].ToString());
        text21.text = string.Format("DEF : {0}", stats[StateTypes.DEF].ToString());
        text22.text = string.Format("MAT : {0}", stats[StateTypes.MAT].ToString());
        text23.text = string.Format("MDF : {0}", stats[StateTypes.MDF].ToString());
        text24.text = string.Format("SPD : {0}", stats[StateTypes.SPD].ToString());

    }

   
}
