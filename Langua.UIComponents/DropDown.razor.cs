using Microsoft.AspNetCore.Components;
using Radzen;

namespace Langua.UIComponents
{
    public partial class DropDownComponent<T> : DropDownBase<T>
    {
        [Parameter] public string FilterPlaceholder { get; set; } = string.Empty;
        [Parameter] public string SelectedItemsText { get; set; } = "items selected";
        [Parameter] public string SelectAllText { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string TextProperty { get; set; }
        [Parameter] public EventCallback<object> Change { get; set; }

    }
}