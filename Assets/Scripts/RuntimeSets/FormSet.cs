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

    public void DestroyAll()
    {
        for (int i = set.Count - 1; i >= 0; i--)
        {
            Destroy(set[i].gameObject);
            set.RemoveAt(i);
        }
    }
}