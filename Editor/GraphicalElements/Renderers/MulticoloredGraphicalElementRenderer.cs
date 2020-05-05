﻿using UnityEngine;

namespace Innoactive.Hub.Training.Editors.GraphicalElements.Renderers
{
    /// <summary>
    /// Renderer for graphical elements that are supposed to change their color while being pressed or hovered over.
    /// </summary>
    public abstract class MulticoloredGraphicalElementRenderer<TOwner> : ColoredGraphicalElementRenderer<TOwner> where TOwner : GraphicalElement
    {
        /// <summary>
        /// Color of the element while that element is pressed down.
        /// </summary>
        protected abstract Color PressedColor { get; }

        /// <summary>
        /// Color of the element that is hovered over.
        /// </summary>
        protected abstract Color HoveredColor { get; }

        /// <summary>
        /// Color of the text at the element.
        /// </summary>
        protected abstract Color TextColor { get; }

        protected MulticoloredGraphicalElementRenderer(TOwner owner, WorkflowEditorColorPalette colorPalette) : base(owner, colorPalette)
        {
            if (owner.IsReceivingEvents)
            {
                owner.GraphicalEventHandler.PointerDown += (sender, args) => CurrentColor = PressedColor;
                owner.GraphicalEventHandler.PointerUp += (sender, args) => CurrentColor = NormalColor;
                owner.GraphicalEventHandler.PointerHoverStart += (sender, args) => CurrentColor = HoveredColor;
                owner.GraphicalEventHandler.PointerHoverStop += (sender, args) => CurrentColor = NormalColor;

                owner.GraphicalEventHandler.ContextPointerDown += (sender, args) => CurrentColor = PressedColor;
                owner.GraphicalEventHandler.ContextPointerUp += (sender, args) => CurrentColor = NormalColor;
            }
        }
    }
}
