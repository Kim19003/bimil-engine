using Bimil.Engine.Objects;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Bimil.Engine.Models
{
    public class GridSettings
    {
        /// <summary>
        /// Is the grid enabled?
        /// </summary>
        public bool Enabled { get; set; } = false;
        /// <summary>
        /// The camera used to draw the grid.
        /// </summary>
        public Camera2D Camera { get; set; } = null;
        /// <summary>
        /// The cell size of the grid.
        /// </summary>
        public Vector2 CellSize { get; set; } = new(4, 4);
        /// <summary>
        /// The line thickness of the grid.
        /// </summary>
        public float LineThickness { get; set; } = 0.5f;
        /// <summary>
        /// The line color of the grid.
        /// </summary>
        public Color LineColor { get; set; } = new(0, 0, 0, 40);
        /// <summary>
        /// The sorting layer of the grid.
        /// </summary>
        public float SortingLayer { get; set; } = 0f;
    }
}