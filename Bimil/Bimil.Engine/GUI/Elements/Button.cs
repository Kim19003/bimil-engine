using Bimil.Engine.GUI.Elements.Bases;
using Bimil.Engine.Models.DrawShapes;
using Microsoft.Xna.Framework.Graphics;

namespace Bimil.Engine.GUI.Elements
{
    /// <summary>
    /// A button GUI element.
    /// </summary>
    public class Button : Element
    {
        /// <summary>
        /// Text of the button.
        /// </summary>
        /// <remarks>
        /// The default value is <see langword="null"/>.
        /// </remarks>
        public Text Text { get; set; } = null;

        /// <summary>
        /// Rectangle of the button.
        /// </summary>
        /// <remarks>
        /// The default value is <see langword="null"/>.
        /// </remarks>
        public RectangleDrawShape Rectangle { get; set; } = null;

        /// <summary>
        /// Texture of the button.
        /// </summary>
        /// <remarks>
        /// The default value is <see langword="null"/>.
        /// </remarks>
        public Texture2D Texture { get; set; } = null;
    }
}