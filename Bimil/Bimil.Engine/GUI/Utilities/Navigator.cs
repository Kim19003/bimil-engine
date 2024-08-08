using System;
using System.Collections.Generic;
using System.Linq;
using Bimil.Engine.GUI.Elements.Bases;
using Bimil.Engine.Models;

namespace Bimil.Engine.GUI.Utilities
{
    /// <summary>
    /// A navigator to navigate through GUI elements.
    /// </summary>
    public sealed class Navigator
    {
        /// <summary>
        /// The current element.
        /// </summary>
        public Element CurrentElement => _elements.Count > 0
            ? _elements.FirstOrDefault(x => x.Index == CurrentIndex)
            : null;

        /// <summary>
        /// The current index.
        /// </summary>
        /// <remarks>
        /// The default value is <c>-1</c>.
        /// </remarks>
        public int CurrentIndex { get; private set; } = -1;
        
        /// <summary>
        /// The elements to navigate.
        /// </summary>
        public IReadOnlyCollection<Element> Elements => _elements;
        private readonly HashSet<Element> _elements;

        /// <summary>
        /// Element events, that can automatically be invoked by the navigator.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="NavigatorInvokes.All"/>.
        /// </remarks>
        public NavigatorInvokes Invokes { get; set; } = NavigatorInvokes.All;

        private readonly int[] _indexes;

        public Navigator(HashSet<Element> elements)
        {
            if (elements.Select(x => x.Index).Distinct().Count() != elements.Count)
            {
                throw new ArgumentException("The elements must have unique indexes!");
            }

            _elements = elements;
            _indexes = elements.Select(x => x.Index).ToArray();
        }

        /// <summary>
        /// Navigates to the next element.
        /// </summary>
        public void NextElement()
        {
            int lastIndex = CurrentIndex;

            if (CurrentIndex == -1)
            {
                CurrentIndex = _indexes.Min();
            }
            else
            {
                CurrentIndex = _indexes.FirstOrDefault(x => x > CurrentIndex);
            }

            bool indexChanged = lastIndex != CurrentIndex;

            if (indexChanged)
            {
                HandleInvokes(CurrentElement, Invokes);
            }
        }

        /// <summary>
        /// Navigates to the previous element.
        /// </summary>
        public void PreviousElement()
        {
            int lastIndex = CurrentIndex;

            if (CurrentIndex == -1)
            {
                CurrentIndex = _indexes.Max();
            }
            else
            {
                CurrentIndex = _indexes.LastOrDefault(x => x < CurrentIndex);
            }

            bool indexChanged = lastIndex != CurrentIndex;

            if (indexChanged)
            {
                HandleInvokes(CurrentElement, Invokes);
            }
        }

        /// <summary>
        /// Navigates to a specific element.
        /// </summary>
        public void SpecificElement(int index)
        {
            if (_indexes.Contains(index))
            {
                int lastIndex = CurrentIndex;
                CurrentIndex = index;

                bool indexChanged = lastIndex != CurrentIndex;

                if (indexChanged)
                {
                    HandleInvokes(CurrentElement, Invokes);
                }
            }
        }

        /// <summary>
        /// Forgets the current location.
        /// </summary>
        public void ForgetLocation()
        {
            CurrentIndex = -1;
        }

        /// <summary>
        /// Handles the element event invokes.
        /// </summary>
        private static void HandleInvokes(Element element, NavigatorInvokes invokes)
        {
            if (element != null)
            {
                if (invokes.HasFlag(NavigatorInvokes.OnSelected))
                {
                    element.OnSelected?.Invoke(element);
                }
                if (invokes.HasFlag(NavigatorInvokes.OnFocused))
                {
                    element.OnFocused?.Invoke(element);
                }
            }
        }
    }
}