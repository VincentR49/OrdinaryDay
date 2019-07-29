using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName ="GameData/PNJControllerList")]
public class PNJControllerList : DataList<PNJController>
{

    public PNJController Get(PNJData pnj)
    {
        if (_items == null)
            return null;
        return _items.FirstOrDefault(x => x.GetPNJData() == pnj);
    }
}
