using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Data holder of Dialogue Information for Yarn Plugin
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Dialogue/Dialogue Agent")]
public class DialogueAgentData : ScriptableObject
{
    public string Tag; // tag of the agent in the yarn dialogue
    public string DialogueDisplayName; // name that should be displayed on the dialogue UI
    public TextAsset YarnDialogue; // yarn dialogue file containing the node related data
    public Sprite DialoguePicture; // picture that should be displayed during the dialogue
    public string DefaultStoryNode; // node to start by default when starting a dialogue with the agent

    public List<NodeWithTag> OtherNodes;

    [Serializable]
    public class NodeWithTag
    {
        public string Tag;
        public string Node;
    }


    /// <summary>
    /// Return the yarn node name from the node tag described in the dialogue agent data.
    /// If this node cannot be find, return the default node.
    /// </summary>
    /// <param name="nodeTag"></param>
    /// <returns></returns>
    public string GetYarnNode(string nodeTag)
    {
        NodeWithTag node = null;
        if (OtherNodes != null)
        {
            node = OtherNodes.FirstOrDefault(n => n.Tag.Equals(nodeTag));
        }
        if (node == null)
        {
            Debug.LogError("Couldnt find any node with the tag: " + nodeTag + ". Will use default node instead.");
            return DefaultStoryNode;
        }
        return node.Node;
    }

    /// <summary>
    /// Return the node tag corresponding the given yarnNode.
    /// If this one cannot be find, return the yarnNode name.
    /// </summary>
    /// <param name="yarnNode"></param>
    /// <returns></returns>
    public string GetNodeTag(string yarnNode)
    {
        if (OtherNodes != null)
        {
            var nodeTag = OtherNodes.FirstOrDefault(n => n.Node.Equals(yarnNode));
            if (nodeTag != null)
            {
                return nodeTag.Tag;
            }
        }
        return yarnNode;
    }
}
