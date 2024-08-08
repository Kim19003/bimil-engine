using Microsoft.Xna.Framework;

namespace Bimil.Engine.GUI.Elements.Bases
{
    /// <summary>
    /// Base class for all GUI elements.
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Name of the element.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="string.Empty"/>.
        /// </remarks>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Position of the element.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="Vector2.Zero"/>.
        /// </remarks>
        public Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>
        /// Scale of the element.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="Vector2.One"/>.
        /// </remarks>
        public Vector2 Scale { get; set; } = Vector2.One;

        /// <summary>
        /// Color of the element.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="Color.White"/>.
        /// </remarks>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Index of the element.
        /// </summary>
        /// <remarks>
        /// The index is used to navigate through elements. The default value is <c>0</c>.
        /// </remarks>
        public int Index
        {
            get => _index;
            set => _index = value < 0 ? 0 : value;
        }
        private int _index = 0;

        /// <summary>
        /// Additional data.
        /// </summary>
        public object AdditionalData { get; set; }

#region Events
        /// <summary>
        /// Event, that is triggered when the element is hovered.
        /// </summary>
        public OnHoveredHandler OnHovered { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is pressed.
        /// </summary>
        public OnPressedHandler OnPressed { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is disabled.
        /// </summary>
        public OnDisabledHandler OnDisabled { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is visible.
        /// </summary>
        public OnVisibleHandler OnVisible { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is clickable.
        /// </summary>
        public OnClickableHandler OnClickable { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is draggable.
        /// </summary>
        public OnDraggableHandler OnDraggable { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is focused.
        /// </summary>
        public OnFocusedHandler OnFocused { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is selected.
        /// </summary>
        public OnSelectedHandler OnSelected { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is toggled.
        /// </summary>
        public OnToggledHandler OnToggled { get; set; }

        /// <summary>
        /// Event, that is triggered when the element is hidden.
        /// </summary>
        public OnHiddenHandler OnHidden { get; set; }
        
        /// <summary>
        /// Event, that is triggered when the element is enabled.
        /// </summary>
        public OnEnabledHandler OnEnabled { get; set; }

        public delegate void OnHoveredHandler(Element sender);
        public delegate void OnPressedHandler(Element sender);
        public delegate void OnDisabledHandler(Element sender);
        public delegate void OnVisibleHandler(Element sender);
        public delegate void OnClickableHandler(Element sender);
        public delegate void OnDraggableHandler(Element sender);
        public delegate void OnFocusedHandler(Element sender);
        public delegate void OnSelectedHandler(Element sender);
        public delegate void OnToggledHandler(Element sender);
        public delegate void OnHiddenHandler(Element sender);
        public delegate void OnEnabledHandler(Element sender);
#endregion
    }
}