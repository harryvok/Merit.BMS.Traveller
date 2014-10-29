using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Merit.Traveller.BMS.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/NotFound/

        public ActionResult NotFound()
        {
            return View();
        }

        //
        // GET: /Error/ServerError/

        public ActionResult ServerError()
        {
            return View();
        }

        //
        // GET: /Error/BadRequest/

        public ActionResult BadRequest()
        {
            return View();
        }

        //
        // GET: /Error/Unauthorized/

        public ActionResult Unauthorized()
        {
            return View();
        }

        //
        // GET: /Error/Forbidden/

        public ActionResult Forbidden()
        {
            return View();
        }

        //
        // GET: /Error/MethodNotAllowed/

        public ActionResult MethodNotAllowed()
        {
            return View();
        }

    }
}
