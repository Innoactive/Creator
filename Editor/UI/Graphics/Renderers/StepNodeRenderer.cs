using System.Collections.Generic;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.CourseValidation;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics.Renderers
{
    internal class StepNodeRenderer : MulticoloredGraphicalElementRenderer<StepNode>
    {
        private const float labelBorderOffsetInwards = 10f;

        public override Color NormalColor
        {
            get
            {
                if (Owner.IsLastSelectedStep)
                {
                    return SelectedColor;
                }

                return ColorPalette.ElementBackground;
            }
        }

        protected override Color PressedColor
        {
            get
            {
                return SelectedColor;
            }
        }

        protected override Color HoveredColor
        {
            get
            {
                return ColorPalette.Secondary;
            }
        }

        protected override Color TextColor
        {
            get
            {
                return ColorPalette.Text;
            }
        }

        protected virtual Color SelectedColor
        {
            get
            {
                return ColorPalette.Primary;
            }
        }

        public StepNodeRenderer(StepNode owner, WorkflowEditorColorPalette colorPalette) : base(owner, colorPalette)
        {
            owner.SelectedChanged += isSelected =>
            {
                CurrentColor = isSelected ? SelectedColor : NormalColor;
            };
        }

        public override void Draw()
        {
            EditorDrawingHelper.DrawRoundedRect(Owner.BoundingBox, CurrentColor, 10f);

            IValidationHandler validation = EditorConfigurator.Instance.Validation;
            if (validation.IsAllowedToValidate())
            {
                IContextResolver resolver = validation.ContextResolver;

                List<EditorReportEntry> errors = validation.LastReport.GetEntriesFor(resolver.FindContext(Owner.Step.Data, GlobalEditorHandler.GetCurrentCourse()));
                if (errors.Count > 0)
                {
                    string tooltip = ValidationTooltipGenerator.CreateStepTooltip(errors,
                        resolver.FindContext(Owner.ActiveChapter.Data, GlobalEditorHandler.GetCurrentCourse()));
                    GUIContent content = new GUIContent("", null, tooltip);
                    Rect rect = new Rect(Owner.BoundingBox.x + Owner.BoundingBox.width * 0.70f, Owner.BoundingBox.y - 8, 16, 16);
                    // Label icons are too small so we draw a label for the tool tip and icon separated.
                    GUI.Label(rect, content);
                    GUI.DrawTexture(rect, EditorGUIUtility.IconContent("Warning").image);
                }
            }

            float labelX = Owner.BoundingBox.x + labelBorderOffsetInwards;
            float labelY = Owner.BoundingBox.y + labelBorderOffsetInwards;
            float labelWidth = Owner.BoundingBox.width - labelBorderOffsetInwards * 2f;
            float labelHeight = Owner.BoundingBox.height - labelBorderOffsetInwards * 2f;

            Rect labelPosition = new Rect(labelX, labelY, labelWidth, labelHeight);

            GUIStyle labelStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = TextColor },
                wordWrap = false,
            };

            string name = EditorDrawingHelper.TruncateText(Owner.Step.Data.Name, labelStyle, labelPosition.width);

            GUIContent labelContent = new GUIContent(name);

            GUI.Label(labelPosition, labelContent, labelStyle);
        }
    }
}
