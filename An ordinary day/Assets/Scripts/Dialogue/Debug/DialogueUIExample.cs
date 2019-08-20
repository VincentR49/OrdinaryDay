using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn;

public class DialogueUIExample : Yarn.Unity.DialogueUIBehaviour
{
    [SerializeField]
    private Text _text;

    public override IEnumerator RunCommand(Command command)
    {
        // todo
        throw new System.NotImplementedException();
    }

    public override IEnumerator RunLine(Line line)
    {
        // todo do more stuff here
        _text.text = line.text;
        yield return null;
    }

    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        // todo
        throw new System.NotImplementedException();
    }
}
