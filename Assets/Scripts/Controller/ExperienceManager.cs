using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//List<GameObject>와 동일한 Party 라는 네임스페이스를 만듬
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;


//총 경험치를 캐릭터의 레벨에 따라 다르게 배분
//경험치가 구해지고 ranks[i].EXP+=subAmount;로 경험치를 더할때 델리게이트에 
//등록된 함수들이 호출되어 경험치를 결정
 public static class ExperienceManger
 {
        const float minLevelBonus = 1.5f;
        const float maxLevelBonus = 0.5f;
        public static void AwardExperience(int amount,Party party)
        {
            //party 숫자만큼 ranks에 추가
            // ranks가 이제 
            List<Rank> ranks = new List<Rank>(party.Count);
            for(int i=0; i<party.Count;++i)
            {
                Rank r = party[i].GetComponent<Rank>();
                if (r != null) ranks.Add(r);
            }

            //step 1:인원들의 레벨 범위 체크
            int min = int.MaxValue;
            int max = int.MinValue;
            for(int i=ranks.Count-1;i>=0;--i)
            {
                min = Mathf.Min(ranks[i].LVL, min);
                max = Mathf.Max(ranks[i].LVL, max);
            }

            //step 2:레벨에 따른 경험치 보정
            float[] weights = new float[party.Count];
            float summedWeights = 0;

            for(int i=ranks.Count-1;i>=0;--i)
            {
                //ranks[i]의 레벨이
                //인원들의 레벨 범위에서 위치하는 범위
                float percent = (float)(ranks[i].LVL - min) / (float)(max - min);

                //percent가 1이면 0.5배
                //percent가 0이면 1.5배
                //레벨이 높을수록 weights 값을 낮게 받음
                weights[i] = Mathf.Lerp(minLevelBonus, maxLevelBonus, percent);

                //weights 총합
                summedWeights += weights[i];
            }

            //step 3: weights에 따라 경험치 증가시키기
            for(int i=ranks.Count-1;i>=0;--i)
            {
                int subAmount = Mathf.FloorToInt((weights[i] / summedWeights) * amount);
                ranks[i].EXP += subAmount;
            }
        }
 }
