using System;

[Serializable]
public class SerialDateTime
{
    public int Day;
    public int Month;
    public int Year;
    public int Hour;
    public int Min;
    public int Sec;

    public SerialDateTime(int year, int month, int day, int hour, int min, int sec)
    {
        Day = day;
        Year = year;
        Month = month;
        Hour = hour;
        Min = min;
        Sec = sec;
    }


    public DateTime ToDateTime() => new DateTime(Year,Month,Day,Hour,Min,Sec);
}
