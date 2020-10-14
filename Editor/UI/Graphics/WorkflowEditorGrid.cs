﻿using System.Collections;
using System.Collections.Generic;
using Innoactive.CreatorEditor.UI;
using Innoactive.CreatorEditor.UI.Graphics;
using Innoactive.CreatorEditor.UI.Graphics.Renderers;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics
{
    /// <summary>
    /// Represents the grid in the background of the chapter within the Workflow window.
    /// </summary>
    internal class WorkflowEditorGrid : Grid
    {
        private readonly WorkflowEditorGridRenderer renderer;

        /// <inheritdoc />
        public override GraphicalElementRenderer Renderer
        {
            get
            {
                return renderer;
            }
        }

        /// <inheritdoc />
        public override int Layer
        {
            get { return 1000; }
        }

        /// <inheritdoc />
        /// <remarks>The grid should always be drawn.</remarks>
        public override bool IsVisibleInRect(Rect rect)
        {
            return true;
        }

        public WorkflowEditorGrid(EditorGraphics editorGraphics) : base(editorGraphics, false)
        {
            renderer = new WorkflowEditorGridRenderer(this);
        }

        public WorkflowEditorGrid(EditorGraphics editorGraphics, float cellSize) : base(editorGraphics, false)
        {
            renderer = new WorkflowEditorGridRenderer(this);
            renderer.CellSize = cellSize;
        }

        /// <summary>
        /// Set the size of the whole grid.
        /// </summary>
        /// <param name="controlRect">Rect which defines size.</param>
        public void SetSize(Rect controlRect)
        {
            renderer.BoundingBox = controlRect;
        }
    }
}
