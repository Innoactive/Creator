using Innoactive.CreatorEditor.UI;
using UnityEditor;
using UnityEngine;

internal class CreatorPageProvider : SettingsProvider
{
    const string Path = "Project/Creator";

    private CreatorProjectSettings data;

    public CreatorPageProvider() : base(Path, SettingsScope.Project) {}

    public static bool IsSettingsAvailable()
    {
        return true;
    }

    public override void OnGUI(string searchContext)
    {
        GUILayout.Label("Have a look at How-To's and an in-depth Webinar for further information.", CreatorEditorStyles.Paragraph);
        GUILayout.Label("How-To's", CreatorEditorStyles.Header);

        CreatorGUILayout.DrawLink("How to build your VR Training application", "https://developers.innoactive.de/documentation/creator/latest/articles/getting-started/designer.html", CreatorEditorStyles.IndentLarge);
        CreatorGUILayout.DrawLink("How to extend the Creator using a training template", "https://developers.innoactive.de/documentation/creator/latest/articles/developer/01-introduction.html", CreatorEditorStyles.IndentLarge);

        GUILayout.Label("Need Help?", CreatorEditorStyles.Header);

        CreatorGUILayout.DrawLink("In-depth webinar on how the Creator works", "https://vimeo.com/417328541/93a752e72c", CreatorEditorStyles.IndentLarge);
        CreatorGUILayout.DrawLink("Visit our developer community", "https://innoactive.io/creator/community", CreatorEditorStyles.IndentLarge);
        CreatorGUILayout.DrawLink("Contact Us for Support", "https://www.innoactive.io/support", CreatorEditorStyles.IndentLarge);

        GUILayout.Space(CreatorEditorStyles.Indent);

        GUILayout.Label("Also, if you are facing any issues, don't hesitate to reach out to us for support", CreatorEditorStyles.Label);

        GUILayout.Space(24);
    }

    [SettingsProvider]
    public static SettingsProvider GetCreatorSettingsProvider()
    {
        if (IsSettingsAvailable())
        {
            SettingsProvider provider = new CreatorPageProvider();

            // Automatically extract all keywords from the Styles.
            //provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
            return provider;
        }

        // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
        return null;
    }
}
