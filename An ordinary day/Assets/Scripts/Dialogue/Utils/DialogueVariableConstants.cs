/// <summary>
/// Static class containing all the variable names
/// TODO: change later
/// </summary>
public static class DialogueVariableConstants
{ 
    public const string PlayerName = "$playerName";
    public const string KnowJisooHouseKeyLocation = "$knowJisooHouseKeyLocation";


    public static string GetHasItemVariable(string itemTag, string ownerTag)
        => string.Format("${0}_has_{1}", ownerTag, itemTag);
}
