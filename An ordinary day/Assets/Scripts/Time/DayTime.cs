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


    public long ToSeconds() => Hour * 3600 + Min * 60 + Sec;

    public override string ToString() => string.Format("{0}:{1}:{2}", Hour, Min, Sec);
}
