using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using System.Net.WebSockets;

namespace Langua.WebUI.Pages.Components
{
        public class GridColumn<TItem> 
        {
            public string Header { get; set; }
            public Func<TItem, object> Template { get; set; }
            public string FilterValue { get; set; } = "";
            public string Property { get; set; }
    }

    public partial class LanguaGridComponent : BasePage
    {
        public InputText Filter;
        public string FilterValue { get; set; }



        public static string GetPropertyName<TItem>(Func<TItem, object> template)
        {
            var r = nameof(template);
            var t = typeof(TItem).GetType().GetProperties();
            var tt = template.GetType().GetProperties();


            return "";
        }
    }
}