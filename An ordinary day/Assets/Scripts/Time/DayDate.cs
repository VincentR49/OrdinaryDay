using System;

[Serializable]
public class DayDate
{
    public int Year;
    public int Month;
    public int Day;

    public DayDate(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }
}
