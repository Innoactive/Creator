﻿using UnityEngine;

namespace Innoactive.Hub.Training.Editors.GraphicalElements.Renderers
{
    /// <summary>
    /// Base class for graphical element renderers that use only one color.
    /// </summary>
    public abstract class ColoredGraphicalElementRenderer<TOwner> : GraphicalElementRenderer<TOwner> where TOwner : GraphicalElement
    {
        /// <summary>
        /// Color palette which is used in current Workflow Editor window. Use colors from it to keep your elements in the same style.
        /// </summary>
        protected WorkflowEditorColorPalette ColorPalette
        {
            get;
            private set;
        }

        /// <summary>
        /// Default color of the element.
        /// </summary>
        public abstract Color NormalColor
        {
            get;
        }

        /// <summary>
        /// Current color of the element.
        /// </summary>
        public Color CurrentColor
        {
            get;
            set;
        }

        protected ColoredGraphicalElementRenderer(TOwner owner, WorkflowEditorColorPalette colorPalette) : base(owner)
        {
            ColorPalette = colorPalette;
            CurrentColor = NormalColor;
        }
    }
}
