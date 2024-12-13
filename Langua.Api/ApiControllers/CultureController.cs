using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.ApiControllers.ApiControllers
{
    [Route("Culture/[action]")]
    public partial class CultureController : Controller
    {
        public IActionResult SetCulture(string culture, string redirectUri)
        {
            if (culture != null)
            {
                try
                {
                    Response.Cookies.Delete("LanguaLangue");
                    Response.Cookies.Append(
                        "LanguaLangue",
                        culture,
                        new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.MaxValue, IsEssential = true, MaxAge = TimeSpan.MaxValue }
                        );
                }
                catch (Exception ex)
                {

                }
            }

            return LocalRedirect(redirectUri);
        }
    }
}
