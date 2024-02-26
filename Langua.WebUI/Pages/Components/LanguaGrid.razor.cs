using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Langua.WebUI.Pages.Components
{
        public class GridColumn<TItem> 
        {
            public string Header { get; set; }
            public Func<TItem, object> Template { get; set; }
        }

    public partial class LanguaGridComponent : BasePage
    {
        public InputText Filter;
        public string FilterValue { get; set; }
        
        public void Search(ChangeEventArgs args)
        {
            // This is the same as (args) => Search(args.Value.ToString())
            // The value of the input will be passed directly to the Search method
            // without the need for additional casting
            // Ensure that this method is declared with the correct parameter type
            Console.WriteLine($"Search called with: {args.Value}");
        }
    }
}