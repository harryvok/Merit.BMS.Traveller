using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.ServiceModel;
using Merit.Traveller.CRM.ws_merit_action;
using Merit.Traveller.CRM.ws_merit_admin;
using Merit.Traveller.CRM.ws_merit_request;
using Merit.Traveller.CRM.ws_traveller;

namespace Merit.Traveller.CRM.Controllers
{
    public class RequestsController : Controller
    {
        public static ws_merit_actionSoapClient GetMeritActionServices()
        {
            var basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.MaxBufferPoolSize = 10000000;
            basicHttpBinding.MaxBufferSize = 10000000;
            basicHttpBinding.MaxReceivedMessageSize = 10000000;
            var endpointAddress = new EndpointAddress("http://merit-laptop-3/merit_traveller/ws_merit_action.asmx");
            new ChannelFactory<ws_merit_actionSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new ws_merit_actionSoapClient(basicHttpBinding, endpointAddress);
            return webServices;
        }

        public static ws_merit_adminSoapClient GetMeritAdminServices()
        {
            var basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.MaxBufferPoolSize = 10000000;
            basicHttpBinding.MaxBufferSize = 10000000;
            basicHttpBinding.MaxReceivedMessageSize = 10000000;
            var endpointAddress = new EndpointAddress("http://merit-laptop-3/merit_traveller/ws_merit_admin.asmx");
            new ChannelFactory<ws_merit_adminSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new ws_merit_adminSoapClient(basicHttpBinding, endpointAddress);
            return webServices;
        }

        public static ws_merit_requestSoapClient GetMeritRequestServices()
        {
            var basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.MaxBufferPoolSize = 10000000;
            basicHttpBinding.MaxBufferSize = 10000000;
            basicHttpBinding.MaxReceivedMessageSize = 10000000;
            var endpointAddress = new EndpointAddress("http://merit-laptop-3/merit_traveller/ws_merit_request.asmx");
            new ChannelFactory<ws_merit_requestSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new ws_merit_requestSoapClient(basicHttpBinding, endpointAddress);
            return webServices;
        }

        public static ws_travellerSoapClient GetTravellerServices()
        {
            var basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.MaxBufferPoolSize = 10000000;
            basicHttpBinding.MaxBufferSize = 10000000;
            basicHttpBinding.MaxReceivedMessageSize = 10000000;
            var endpointAddress = new EndpointAddress("http://merit-laptop-3/merit_traveller/ws_traveller.asmx");
            new ChannelFactory<ws_travellerSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new ws_travellerSoapClient(basicHttpBinding, endpointAddress);
            return webServices;
        }

        //
        // GET: /Requests/

        [Authorize]
        public ActionResult Index()
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String dataGroup = Request.Cookies["dataGroup"].Value;

            var filterList = GetMeritActionServices().ws_get_officer_filters(user_id, password, "complaint");
            ViewBag.filterList = filterList;

            String defaultFilter = filterList.filter_det.ElementAt(0).filter_no.ToString();


            var requestIntray = GetMeritRequestServices().ws_get_request_intray(user_id, password, dataGroup, defaultFilter);

            Debug.WriteLine("cum here2");
            return View();
        }

        [Authorize]
        public ActionResult RequestIntray(String filterNum)
        {

            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String dataGroup = Request.Cookies["dataGroup"].Value;

            var requestIntray = GetMeritRequestServices().ws_get_request_intray(user_id, password, dataGroup, filterNum);

            Debug.WriteLine("cum here");

            if (requestIntray != null)
                return PartialView(requestIntray);
            else
                return null;
        }

    }
}
