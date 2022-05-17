using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Alliances
{
  //비트 연산
  //시프트 연산자
  //2진수로 표현된 비트를 해당하는 방향으로 이동시키는 연산자
  None = 0,
  Netural= 1 << 0,
  Hero = 1 << 1,
  Enemy = 1 << 2,
}
