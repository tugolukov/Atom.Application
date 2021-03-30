using Microsoft.AspNetCore.Mvc;

namespace Atom.Application.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/help");
        }
    }
}