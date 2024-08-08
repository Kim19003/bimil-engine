using Bimil.Engine.GUI.Elements.Bases;

namespace Bimil.Engine.GUI.Elements
{
    /// <summary>
    /// A text GUI element.
    /// </summary>
    public class Text : Element
    {
        /// <summary>
        /// Value of the text.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="string.Empty"/>.
        /// </remarks>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Font of the text.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="string.Empty"/>.
        /// </remarks>
        public string Font { get; set; } = string.Empty;

        /// <summary>
        /// Size of the font.
        /// </summary>
        /// <remarks>
        /// The default value is <c>12f</c>.
        /// </remarks>
        public float FontSize { get; set; } = 12f;
    }
}