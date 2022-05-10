//3D 타일의 좌표를 가져옴
//포인트 구조체

[System.Serializable]
public struct Point
{
    public int x;
    public int y;
    public Point(int x, int y) //생성자
    {
        this.x = x;
        this.y = y;
    }

    //연산자 재정의
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }          
    public static Point operator -(Point a,Point b)
    {
        return new Point(a.x - b.x, a.y - b.y);
    }
    public static bool operator == (Point a,Point b)
    {
        return a.x == b.x && a.y == b.y;
    }
    public static bool operator !=(Point a,Point b)
    {
        return !(a == b);
    }

    //비교하여 같은 것인지 다른것인지 확인하는 함수
    //Equals와 해스코드를 재정의 한 이유는 Garbage가 생기는 것을 방지
    //C#은 
    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return x == p.x && y == p.y;
        }
        return false;
    }
    public bool Equals(Point p)
    {
        return x == p.x && y == p.y;
    }
    //개체를 식별하는 정수의 값, 객체의 메모리 번지를 이용해서 해시코드를 만들어 리턴 객체마다 다른값
    public override int GetHashCode()
    {
        return x^y;
    }
    //{0}에 x좌표를 {1}에 y좌표를 넣어 문자열을 만들어 반환
    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }

}

