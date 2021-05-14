using UnityEngine;

using Deblue.DialogSystem;

namespace Deblue.LD48
{
    public class ChoiceChecker : IChoiceReciver
    {
        public bool CheckChoiceAvalible(Choice choice)
        {
            if (!string.IsNullOrEmpty(choice.ItemID))
            {
                var boy = Object.FindObjectOfType<Boy>();
                return boy.IsContainObject(choice.ItemID);
            }
            return true;
        }
    }
}