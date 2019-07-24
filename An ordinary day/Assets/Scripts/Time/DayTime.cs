using System;

[Serializable]
public class DayTime
{
    public int Hour;
    public int Min;
    public int Sec;

    public DayTime(int hour, int min, int sec)
    {
        Hour = hour;
        Min = min;
        Sec = sec;
    }
}
