using UnityEngine;

/// <summary>
/// Interface for class reacting to an interaction
/// </summary>
public interface I_InteractionResponse
{
    void OnInteraction(GameObject interactor);
}
