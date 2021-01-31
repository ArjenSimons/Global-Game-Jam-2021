using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommentaryLibrary", menuName = "ScriptableObjects/CommentaryLibrary")]
public class CommentaryLibrary : ScriptableObject
{
    [SerializeField]
    List<CommentaryGroup> commentaryGroups = new List<CommentaryGroup>();

    public Commentary GetCommentary(CommentaryType commentaryType)
    {
        // Find correct commentaryGroup
        CommentaryGroup commentaryGroup = null;
        foreach (CommentaryGroup group in commentaryGroups)
        {
            if (group.IsOfType(commentaryType))
            {
                commentaryGroup = group;
                break;
            }
        }

        if (commentaryGroup == null)
        {
            Debug.LogWarning($"Couldn't find commentary group of type {commentaryType}");
            return null;
        }

        Commentary commentary = commentaryGroup.GetRandomCommentary();
        return commentary;
    }
}
