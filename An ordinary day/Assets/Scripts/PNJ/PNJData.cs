using UnityEngine;

/// <summary>
/// Scriptable containing the PNJ informations
/// </summary>
[CreateAssetMenu(menuName ="Scriptables/PNJData")]
public class PNJData : ScriptableObject
{
    [Header("Standard Information")]
    public string FirstName;
    public string LastName;
    public int Age;
    public Gender Gender;

    [Header("Sprites")]
    public CardinalSpriteData CardinalSprite;
    public CardinalAnimationData WalkingAnimation;

    [Header("Schedule")]
    public ScheduleData DefaultSchedule; // readyonly, just used to store the basic schedule data
    public RuntimeSchedule InGameSchedule; // reference to the dynamic schedule


    public override string ToString() 
        => string.Format("{0} {1}, {2}, {3}", FirstName, LastName, Age, Gender);


    public void InitRuntimeSchedule()
    {
        Debug.Log("[PNJSchedulesManager] Init schedule of " + FirstName);
        InGameSchedule.Init(DefaultSchedule);
    }
}
