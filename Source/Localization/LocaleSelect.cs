using System.Globalization;
using System.Threading;
using UnityEngine;


namespace Innoactive.Hub
{
    public class LocaleSelect : MonoBehaviour
    {
        private enum Locale
        {
            EN,
            DE,
        }

        [SerializeField]
        private Locale locale = Locale.EN;

        private void Start()
        {
            switch (locale)
            {
                case Locale.EN:
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-EN");
                    break;
                case Locale.DE:
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
                    break;
                default:
                    break;
            }

        }
    }
}