using System.Collections.Generic;
using System.Linq;
using VPG.Creator.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace VPG.CreatorEditor.UI
{
    /// <summary>
    /// Base Settings provider which allows to inject additional sections which implement <see cref="IProjectSettingsSection"/>.
    /// </summary>
    public abstract class BaseSettingsProvider : SettingsProvider
    {
        protected List<IProjectSettingsSection> sections = new List<IProjectSettingsSection>();

        protected BaseSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            ReflectionUtils.GetConcreteImplementationsOf<IProjectSettingsSection>().ToList().ForEach(type =>
            {
                IProjectSettingsSection section = (IProjectSettingsSection) ReflectionUtils.CreateInstanceOfType(type);
                if (section.TargetPageProvider == GetType())
                {
                    sections.Add(section);
                }
            });

            sections.Sort((ext1, ext2) => ext1.Priority.CompareTo(ext2.Priority));
        }

        /// <summary>
        /// Draw call.
        /// </summary>
        public override void OnGUI(string searchContext)
        {
            GUILayout.BeginHorizontal();
                GUILayout.Space(8);
                GUILayout.BeginVertical();

                    InternalDraw(searchContext);

                    sections.ForEach(section =>
                    {
                        GUILayout.Label(section.Title, CreatorEditorStyles.Header);
                        section.OnGUI(searchContext);
                    });
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Your draw implementation, will be drawn above the injected sections.
        /// </summary>
        protected abstract void InternalDraw(string searchContext);
    }
}
