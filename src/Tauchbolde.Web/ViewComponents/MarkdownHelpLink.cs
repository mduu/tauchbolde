using System;
using Microsoft.AspNetCore.Mvc;

namespace Tauchbolde.Web.ViewComponents
{
    public class MarkdownHelpLink : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
