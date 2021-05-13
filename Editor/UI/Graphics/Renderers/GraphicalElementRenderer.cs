﻿namespace VPG.CreatorEditor.UI.Graphics.Renderers
{
    /// <summary>
    /// Base class for all graphical element renderers.
    /// </summary>
    internal abstract class GraphicalElementRenderer<TOwner> : GraphicalElementRenderer where TOwner : GraphicalElement
    {
        /// <summary>
        /// Graphical element to which this renderer belongs.
        /// </summary>
        protected TOwner Owner
        {
            get;
            private set;
        }

        protected GraphicalElementRenderer(TOwner owner)
        {
            Owner = owner;
        }
    }

    public abstract class GraphicalElementRenderer
    {
        /// <summary>
        /// Called once in the end of the frame. Use it to draw elements in the editor window.
        /// </summary>
        public abstract void Draw();
    }
}
