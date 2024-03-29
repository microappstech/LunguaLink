using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Langua.UIComponents
{
    public class LanguaBaseComponent : ComponentBase
    {
        [Parameter] public bool Visible { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            
        }

    }
}
