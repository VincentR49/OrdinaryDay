using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "RuntimeVariable/PNJControllerList")]
public class PNJControllerList : RuntimeDataList<PNJController>
{
    public PNJController Get(PNJData pnj)
    {
        if (Items == null)
            return null;
        return Items.FirstOrDefault(x => x.GetPNJData() == pnj);
    }
}
