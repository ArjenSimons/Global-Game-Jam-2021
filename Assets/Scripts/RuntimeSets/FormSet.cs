using UnityEngine;

[CreateAssetMenu(menuName = "RuntimeSets/Forms")]
public class FormSet : RuntimeSet<Form>
{
    public Form GetMatchWithLostItem(LostItem lostItem)
    {
        foreach (Form form in set)
        {
            if (lostItem.Equals(form.ItemDisplaying))
            {
                return form;
            }
        }

        return null;
    }
}