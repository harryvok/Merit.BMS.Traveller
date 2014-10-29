using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Merit.Traveller.CRM.ws_merit_admin;
using System.ServiceModel;
using System.Diagnostics;

namespace Merit.Traveller.CRM.Controllers
{


    public class AccountController : Controller
    {
        
       

        public static ws_merit_adminSoapClient GetWebServices(string asmx)
        {
            var basicHttpBinding = new BasicHttpBinding();
            var endpointAddress = new EndpointAddress("http://merit-laptop-3/merit_traveller/" + asmx);
            new ChannelFactory<ws_merit_adminSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new ws_merit_adminSoapClient(basicHttpBinding, endpointAddress);
            return webServices;
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(string user_id, string password)
        {
            var webServices = GetWebServices("ws_merit_admin.asmx");
            var login = webServices.ws_merit_login(user_id, password);


            if (login.ws_status == 0)
            {
                FormsAuthentication.SetAuthCookie(user_id, false);
                HttpCookie loginCookie = new HttpCookie("user_id");
                loginCookie.Value = user_id;
                loginCookie.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie2 = new HttpCookie("password");
                loginCookie2.Value = password;
                loginCookie2.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie3 = new HttpCookie("officerID");
                loginCookie3.Value = login.responsible_code;
                loginCookie3.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie4 = new HttpCookie("dataGroup");
                loginCookie4.Value = login.data_group;
                loginCookie4.Expires = DateTime.Now.AddMinutes(500);

                Response.Cookies.Add(loginCookie);
                Response.Cookies.Add(loginCookie2);
                Response.Cookies.Add(loginCookie3);
                Response.Cookies.Add(loginCookie4);


                return RedirectToAction("Index", "Home");

            }
            else if (login.ws_status == -1)
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }


            // If we got this far, something failed, redisplay form
            return View();
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
