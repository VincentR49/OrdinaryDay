using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName ="GameData/PNJControllerList")]
public class PNJControllerList : DataList<PNJController>
{
    public PNJController Get(PNJData pnj)
    {
        if (Items == null)
            return null;
        return Items.FirstOrDefault(x => x.GetPNJData() == pnj);
    }
}
