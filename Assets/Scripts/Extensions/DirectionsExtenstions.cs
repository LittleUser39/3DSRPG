using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class DirectionsExtensions
{
    // 타겟과의 방향에 따라 Directions의 Enum 값이 리턴된다.
    public static Directions GetDirection(this Tile t1, Tile t2)
    {
        if (t1.pos.y < t2.pos.y)
            return Directions.North;
        if (t1.pos.x < t2.pos.x)
            return Directions.East;
        if (t1.pos.y > t2.pos.y)
            return Directions.South;
        return Directions.West;
    }

    //GetDirections 오버로딩
    //공격자가 바라보는 방향 반환
    public static Directions GetDirections(this Point p)
    {
        if (p.y > 0)
            return Directions.North;
        if (p.x > 0)
            return Directions.East;
        if (p.y < 0)
            return Directions.South;
        return Directions.West;
    }
    //바라보고 있는 방향 반환
    public static Point GetNormal(this Directions dir)
    {
        switch(dir)
        {
            case Directions.North://북
                return new Point(0, 1);
            case Directions.East://동
                return new Point(1, 0);
            case Directions.South://남
                return new Point(0, -1);
            default://서
                return new Point(-1, 0);
        }
    }
   // 방향을 오일러 각도로 반환한다.
   public static Vector3 ToEuler(this Directions d)
   {
       return new Vector3(0, (int)d * 90, 0);
   }
}
