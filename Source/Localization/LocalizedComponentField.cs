using System;
using System.Reflection;
using UnityEngine;

namespace Innoactive.Hub
{
    public class LocalizedComponentField : MonoBehaviour
    {
        private static readonly Common.Logging.ILog logger = Logging.LogManager.GetLogger<LocalizedComponentField>();

        [SerializeField]
        private LocalizedString localizedString = null;

        [SerializeField]
        private Component componentToChange = null;

        [SerializeField]
        private string fieldToChange = null;

        private void OnEnable()
        {
            UpdateComponentValue();
            Localization.OnLocaleChanged += HandleLocalizationChange;
        }

        private void OnDisable()
        {
            Localization.OnLocaleChanged -= HandleLocalizationChange;
        }

        private void HandleLocalizationChange(string newLocale, string oldLocale)
        {
            UpdateComponentValue();
        }

        private bool UpdateComponentValue()
        {
            if (localizedString == null)
            {
                logger.Error("ApplyLocalizedStringToComponent failed - no localized string assigned.");
                return false;
            }
            else if (componentToChange == null)
            {
                logger.Error("ApplyLocalizedStringToComponent failed - no target component assigned.");
                return false;
            }
            else if (string.IsNullOrEmpty(fieldToChange))
            {
                logger.Error("ApplyLocalizedStringToComponent failed - no target field assigned.");
                return false;
            }

            Type componentType = componentToChange.GetType();
            FieldInfo field = componentType.GetField(fieldToChange, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            if (field != null)
            {

                if (!field.FieldType.IsAssignableFrom(typeof(string)))
                {
                    logger.ErrorFormat("ApplyLocalizedStringToComponent failed - field \"{0}\" of type \"{1}\" is not assignable from a string", field.Name, field.FieldType.Name);
                }
                else
                {
                    field.SetValue(componentToChange, localizedString.Value);
                    return true;
                }
            }

            PropertyInfo prop = componentType.GetProperty(fieldToChange, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            if (prop != null)
            {
                if (!prop.PropertyType.IsAssignableFrom(typeof(string)))
                {
                    logger.ErrorFormat("ApplyLocalizedStringToComponent failed - property \"{0}\" of type \"{1}\" is not assignable from a string", prop.Name, prop.PropertyType.Name);
                }
                else
                {
                    prop.SetValue(componentToChange, localizedString.Value, null);
                    return true;
                }
            }

            logger.ErrorFormat("ApplyLocalizedStringToComponent failed - component of type \"{0}\" has no field or property \"{1}\"", componentType.Name, fieldToChange);
            return false;
        }
    }
}
