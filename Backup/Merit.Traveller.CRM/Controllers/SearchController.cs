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
    public class SearchController : Controller
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
        // GET: /Search/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchResults(String searchText)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;


            search_list searchResults = GetTravellerServices().ws_search(user_id, password, searchText, 0);

            return PartialView(searchResults);
        }

    }
}
