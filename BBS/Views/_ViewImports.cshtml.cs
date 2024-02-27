using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BBS.Views
{
    public class _ViewImports : PageModel
    {
        private readonly ILogger<_ViewImports> _logger;

        public _ViewImports(ILogger<_ViewImports> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}