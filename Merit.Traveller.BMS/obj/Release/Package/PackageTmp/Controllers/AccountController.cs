using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Merit.Traveller.BMS.BMS_Web_Admin;
using System.ServiceModel;
using System.Diagnostics;

namespace Merit.Traveller.BMS.Controllers
{
    public class AccountController : Controller
    {
        public static bms_web_adminSoapClient GetWebServices(string asmx)
        {

            String webServicesEndpointAddress = System.Configuration.ConfigurationManager.AppSettings["WebService_Admin"].ToString();
            var basicHttpBinding = new BasicHttpBinding();
            var endpointAddress = new EndpointAddress(webServicesEndpointAddress); 
            new ChannelFactory<bms_web_adminSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new bms_web_adminSoapClient(basicHttpBinding, endpointAddress);
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
            var webServices = GetWebServices("bms_web_admin.asmx");
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

                HttpCookie loginCookie3 = new HttpCookie("mission");
                loginCookie3.Name = login.central_guid;
                loginCookie3.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie4 = new HttpCookie("officeID");
                loginCookie4.Value = login.office_guid;
                loginCookie4.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie5 = new HttpCookie("officerID");
                loginCookie5.Value = login.responsible_code;
                loginCookie5.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie6 = new HttpCookie("centralID");
                loginCookie6.Value = login.central_guid;
                loginCookie6.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie7 = new HttpCookie("centralName");
                loginCookie7.Value = login.office_name;
                loginCookie7.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie12 = new HttpCookie("userGivenName");
                loginCookie12.Value = login.given_name;
                loginCookie12.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie16 = new HttpCookie("userSurname");
                loginCookie16.Value = login.surname;
                loginCookie16.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie13 = new HttpCookie("role");
                loginCookie13.Value = login.security_group;
                loginCookie13.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie14 = new HttpCookie("vlid_bypass_officer");
                loginCookie14.Value = login.vlid_bypass_officer;
                loginCookie14.Expires = DateTime.Now.AddMinutes(500);

                //The following are for the permit list filtering
                HttpCookie loginCookie8 = new HttpCookie("filterID");
                loginCookie8.Value = "1";
                loginCookie8.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie9 = new HttpCookie("permitListDateFrom");
                loginCookie9.Value = "";
                loginCookie9.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie10 = new HttpCookie("permitListDateTo");
                loginCookie10.Value = "";
                loginCookie10.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie11 = new HttpCookie("permitListFilterString");
                loginCookie11.Value = "";
                loginCookie11.Expires = DateTime.Now.AddMinutes(500);

                HttpCookie loginCookie15 = new HttpCookie("documentPictureID");
                loginCookie15.Value = "";
                loginCookie15.Expires = DateTime.Now.AddMinutes(500);


                //end
                
                Response.Cookies.Add(loginCookie);
                Response.Cookies.Add(loginCookie2);
                Response.Cookies.Add(loginCookie3);
                Response.Cookies.Add(loginCookie4);
                Response.Cookies.Add(loginCookie5);
                Response.Cookies.Add(loginCookie6);
                Response.Cookies.Add(loginCookie7);
                Response.Cookies.Add(loginCookie8);
                Response.Cookies.Add(loginCookie9);
                Response.Cookies.Add(loginCookie10);
                Response.Cookies.Add(loginCookie11);
                Response.Cookies.Add(loginCookie12);
                Response.Cookies.Add(loginCookie13);
                Response.Cookies.Add(loginCookie14);
                Response.Cookies.Add(loginCookie15);
                Response.Cookies.Add(loginCookie16);
                

                return RedirectToAction("Index", "PermitIntray");

            }
            else if (login.ws_status == -1)
            {
                ModelState.AddModelError("", login.ws_message);
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "PermitIntray");
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(string password, string new_password, string confirm_password)
        {
            String current_password = Request.Cookies["password"].Value;
            if (current_password == password)
            {
                if (new_password == confirm_password)
                {
                    try
                    {
                        String user_id = Request.Cookies["user_id"].Value;
                        var webServices = GetWebServices("bms_web_admin.asmx");
                        var login = webServices.ws_change_password(user_id, password, new_password);
                        HttpCookie loginCookie2 = new HttpCookie("password");
                        loginCookie2.Value = password;
                        loginCookie2.Expires = DateTime.Now.AddMinutes(50);

                        Response.Cookies.Add(loginCookie2);

                        return RedirectToAction("Index", "PermitIntray");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e.ToString());
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Current password is incorrect.");
            }

            return View();
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // GET: /Account/ChangeSite

        [Authorize]
        public ActionResult ChangeSite()
        {
            return View();
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
