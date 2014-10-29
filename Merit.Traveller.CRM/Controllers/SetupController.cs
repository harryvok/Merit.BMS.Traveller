using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Merit.Traveller.CRM.Controllers
{
    public class SetupController : Controller
    {
        //
        // GET: /Setup/

        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Setup/
        [HttpPost]
        public ActionResult Index(CRMSettings settings)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

            CRMSettings section = (CRMSettings)config.GetSection("CRMSettings");

            section.Setup = true;
            section.AuthUser = settings.AuthUser;
            section.AuthPass = settings.AuthPass;
            section.CouncilName = settings.CouncilName;
            section.Attachments = settings.Attachments;
            section.LookupEnabled =  settings.LookupEnabled;
            config.Save();

            return RedirectToAction("Index", "Home");
        }

    }
}
