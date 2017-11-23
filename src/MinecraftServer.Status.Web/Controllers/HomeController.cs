using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinecraftServer.Status.Web.Controllers
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        // GET: Default
        [Route]
        public ActionResult Index()
        {
            return View();
        }
    }
}