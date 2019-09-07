using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleApp.Pages
{
    public class IndexModel : PageModel
    {
        // see https://code-maze.com/global-error-handling-aspnetcore/#custommiddleware
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-2.2

        public void OnGet()
        {

        }       
    }
}
