using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.ModelView.InputModels
{
    public class ConfirmEmail
    {
        public string Email { get; set; }
        
        public string CodeVerification { get; set; }
        
    }
}
