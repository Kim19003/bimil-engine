using System;

namespace Bimil.Engine.Models
{
    /// <summary>
    /// A GUI element events, that can automatically be invoked by the navigator.
    /// </summary>
    [Flags]
    public enum NavigatorInvokes
    {
        None = 0,
        OnFocused = 1,
        OnSelected = 2,
        All = OnFocused | OnSelected,
    }
}