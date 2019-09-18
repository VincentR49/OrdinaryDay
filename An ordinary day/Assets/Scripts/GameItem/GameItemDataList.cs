using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Game Item List")]
public class GameItemDataList : DataList<GameItemData>
{
    public GameItemData GetItem(string itemTag) => Items.FirstOrDefault(x => x.Tag.Equals(itemTag));
}
