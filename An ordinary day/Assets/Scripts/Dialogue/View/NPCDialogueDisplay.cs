using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueDisplay : CharacterDialogueDisplay
{
    public override void Reset()
    {
        base.Reset();
        _picture.sprite = null;
        _name.text = "";
    }
}
