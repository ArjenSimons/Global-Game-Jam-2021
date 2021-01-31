using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommentaryLibrary", menuName = "ScriptableObjects/CommentaryLibrary")]
public class CommentaryLibrary : ScriptableObject
{
    [SerializeField]
    List<Commentary> commentaryList = new List<Commentary>();

    public Commentary GetCommentary()
    {
        if (commentaryList.Count == 0)
        {
            Debug.LogWarning("Commentary library couldn't find a commentary!");
            return null;
        }

        Commentary commentary = commentaryList[Random.Range(0, commentaryList.Count)];
        return commentary;
    }
}
