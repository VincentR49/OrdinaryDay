using System.Collections;
using UnityEngine;

/// <summary>
/// Attach this to a game object to make him able to speak (start dialogue) with the Player.
/// Will launch the dialogue system run by PlayerDialogueRunner.
/// </summary>
// todo Only for player related dialogue ?
// todo not satisifed with name. not enough generic maybe?
public class DialogueWithPlayerAgent : DialogueAgent<PlayerDialogueRunner>, I_InteractionResponse
{
    [Header("Optional")]
    [SerializeField]
    private bool _facePlayerWhileSpeaking;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;
    [SerializeField]
    private WalkManager _walkManager;


    private void Start()
    {
        _dialogueRunner = FindObjectOfType<PlayerDialogueRunner>(); // todo change this later
    }


    protected override IEnumerator SpeaksToRoutine(GameObject other, string node, TextAsset yarnFile)
    {
        if (_walkManager != null) // stop the game object if he is walking (animation bug otherwise)
        {
            _walkManager.Stop();
            yield return new WaitForEndOfFrame();
        }
        if (_facePlayerWhileSpeaking) // more polite
        {
            _spriteDirectioner.FaceTowards(other.transform);
            yield return new WaitForEndOfFrame();
        }
        yield return base.SpeaksToRoutine(other, node, yarnFile);
    }


    public void OnInteraction(GameObject interactor)
    {
        SpeaksTo(interactor);
    }
}
