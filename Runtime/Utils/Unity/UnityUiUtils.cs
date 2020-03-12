using UnityEngine;
using UnityEngine.UI;

namespace Innoactive.Hub.Training.Unity.Utils
{
    public static class UnityUiUtils
    {
        public static void Refresh(this Dropdown dropdown)
        {
            Transform oldDropdown = dropdown.transform.Find("Dropdown List");
            bool isDropdownExpanded = oldDropdown != null && (oldDropdown.Equals(null) == false);
            if (isDropdownExpanded)
            {
                dropdown.Hide();
                dropdown.StopAllCoroutines();
                Object.DestroyImmediate(oldDropdown.gameObject);
                dropdown.Show();
                dropdown.StopAllCoroutines();
                Transform newDropdown = dropdown.transform.Find("Dropdown List");
                newDropdown.GetComponent<CanvasGroup>().alpha = 1f;
            }
        }
    }
}
