using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbilityEffect : BaseAbilityEffect
{
   //이게 데미지를 계산하는 함수
    public override int Predict(Tile target)
    {
        //공격자 방어자 유닛 참조
        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.content.GetComponent<Unit>();

        //공격자의 기본 공격력 참조
        //공격 종류에 따라 참조되는 능력치가 다름
        int attack = GetStat(attacker, defender, GetAttackNotification, 0);

        //방어자의 기본 방어력 참조
        //공격 종류에 따라 참조되는 능력치가 다름
        int defense = GetStat(attacker, defender, GetDefenseNotification, 0);

        //피해량 계산
        int damage = attack - (defense / 2);
        //피해량이 마이너스가 되지 않게
        damage = Mathf.Max(damage, 1);

        //레벨 보정값
        int power = GetStat(attacker, defender, GetPowerNotification, 0);
        damage = power * damage / 100;
        damage = Mathf.Max(damage, 1);

        // 델리게이트에서 계산된 damage 값이 그대로 보존됨
        damage = GetStat(attacker, defender, TweakDamageNotification, damage);
        //피해량이 최소,최대 범위를 넘지 않도록 한다
        damage = Mathf.Clamp(damage, minDamage, maxDamage);
        return -damage;
    }

    //데미지를 적용하는 함수
    public override int OnApply(Tile target)
    {
        //방어자 유닛 참조
        Unit defender = target.content.GetComponent<Unit>();
        //최종 피해량이 참조
        int value = Predict(target);
        //0.9~1.1배 랜덤값 적용
        value = Mathf.FloorToInt(value * UnityEngine.Random.Range(0.9f, 1.1f));
        //value가 최소,최대 값 넘지 않도록 조정
        value = Mathf.Clamp(value, minDamage, maxDamage);
        //방어자의 체력값 변경 (데미지적용)
        Stats stats = defender.GetComponent<Stats>();
        stats[StateTypes.HP] += value;
        
        return value;
    }
   
}
