using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommentaryGroup", menuName = "ScriptableObjects/CommentaryGroup")]
public class CommentaryGroup : ScriptableObject
{
    [SerializeField]
    private CommentaryType commentaryType;

    [SerializeField]
    private List<Commentary> commentaryList = new List<Commentary>();

    private Commentary lastUsedCommentary;

    public bool IsOfType(CommentaryType commentaryType)
    {
        return this.commentaryType == commentaryType;
    }

    public Commentary GetRandomCommentary()
    {
        List<Commentary> listMinusLastUsed = new List<Commentary>(commentaryList);
        if (lastUsedCommentary != null)
        {
            listMinusLastUsed.Remove(lastUsedCommentary);
        }

        if (listMinusLastUsed.Count == 0)
        {
            Debug.LogWarning("Commentary group has no non-last-used commentaries");
            return null;
        }

        Commentary commentary = listMinusLastUsed[(Random.Range(0, listMinusLastUsed.Count))];
        lastUsedCommentary = commentary;
        return commentary;
    }
}
