using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Graphics.Renderers
{
    /// <summary>
    /// Base class which handles rendering of a <see cref="Grid"/>.
    /// </summary>
    public abstract class GridRenderer<TOwner> : GraphicalElementRenderer<TOwner> where TOwner : Grid
    {
        /// <summary>
        /// Bounding box of the grid.
        /// </summary>
        public virtual Rect BoundingBox { get; set; }

        /// <summary>
        /// Size of a cell within the grid. Cells are always square.
        /// </summary>
        public virtual float CellSize { get; set; } = 10f;

        protected GridRenderer(TOwner owner) : base(owner)
        {
        }

        /// <summary>
        /// Color used for lines of grid.
        /// </summary>
        public virtual Color MainColor { get; set; } = Color.gray * 0.3f;

        /// <summary>
        /// Color used for every 10th line of the grid.
        /// </summary>
        public virtual Color SecondaryColor { get; set; } = Color.black;

        /// <inheritdoc />
        public override void Draw()
        {
            Color drawColor;

            int lineCount = 0;
            float yPosTop = 0;
            float yPosBot = 0;

            // Draw horizontal lines starting from (0, 0).
            while (yPosTop > BoundingBox.y || yPosBot <= BoundingBox.height)
            {
                drawColor = lineCount % 10 == 0 ? SecondaryColor : MainColor;
                lineCount++;

                if (yPosTop > BoundingBox.y)
                {
                    EditorDrawingHelper.DrawHorizontalLine(new Vector3(BoundingBox.x, yPosTop), BoundingBox.width, drawColor);
                    yPosTop -= CellSize;
                }

                if (yPosBot <= BoundingBox.height)
                {
                    EditorDrawingHelper.DrawHorizontalLine(new Vector3(BoundingBox.x, yPosBot), BoundingBox.width, drawColor);
                    yPosBot += CellSize;
                }
            }

            lineCount = 0;
            float xPosLeft = 0;
            float xPosRight = 0;

            // Draw vertical lines starting from (0, 0).
            while (xPosLeft > BoundingBox.x || xPosRight <= BoundingBox.width)
            {
                drawColor = lineCount % 10 == 0 ? SecondaryColor : MainColor;
                lineCount++;

                if (xPosLeft > BoundingBox.x)
                {
                    EditorDrawingHelper.DrawVerticalLine(new Vector3(xPosLeft, BoundingBox.y), BoundingBox.height, drawColor);
                    xPosLeft -= CellSize;
                }

                if (xPosRight <= BoundingBox.width)
                {
                    EditorDrawingHelper.DrawVerticalLine(new Vector3(xPosRight, BoundingBox.y), BoundingBox.height, drawColor);
                    xPosRight += CellSize;
                }
            }
        }
    }
}
