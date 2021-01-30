using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RuntimeSets/Forms")]
public class FormSet : RuntimeSet<Form>
{
    public Form GetMatchWithLostItem(LostItem lostItem)
    {
        foreach (Form form in set)
        {
            Debug.Log(form);
            if (lostItem.Equals(form.ItemDisplaying))
            {
                return form;
            }
        }

        return null;
    }
}