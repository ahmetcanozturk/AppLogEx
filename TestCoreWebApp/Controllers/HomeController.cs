using TestCoreWebApp.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TestCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        LogEx.LogManager _logger;
        public HomeController(LogEx.LogManager logger)
        {
            this._logger = logger;
        }
        public IActionResult Index()
        {
            //dummy log
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("id", 1) };

            var resultText = _logger.CreateLog("LOG0005", parameters, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, HttpContext.User.Identity.Name, HttpContext.Connection.RemoteIpAddress.ToString(), HttpContext.Request.Headers["User-Agent"]).Text;

            ViewData["Message"] = resultText;

            //throw a dummy null reference exception in business code
            new BusinessLogic().DoTest();

            return View();
        }
    }
}
