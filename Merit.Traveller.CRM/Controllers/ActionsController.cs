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
    public class ActionsController : Controller
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
        // GET: /Actions/

        public ActionResult Index()
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String dataGroup = Request.Cookies["dataGroup"].Value;

            DateTime nullDate = new DateTime(); 

            //action_intray_list is ambiguous (exists in two packages) need to specify ws_merit_action
            Merit.Traveller.CRM.ws_merit_action.action_intray_list actionList = GetMeritActionServices().ws_get_action_intray(user_id, password, dataGroup, 598438, nullDate, nullDate);

            var filterList = GetMeritActionServices().ws_get_officer_filters(user_id,password,"action");
            ViewBag.filterList = filterList;
           
            return View();
        }

        [Authorize]
        public ActionResult ActionIntray(int filterNum)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String dataGroup = Request.Cookies["dataGroup"].Value;
            String respCode = Request.Cookies["officerID"].Value;

            DateTime nullDate = new DateTime();
           
            var actionIntray = GetMeritActionServices().ws_get_action_intray(user_id, password, dataGroup, filterNum, nullDate, nullDate);
                 
            if(actionIntray != null)
                return PartialView(actionIntray);
            else
                return null;

        }

        public ActionResult ActionSearch(String search)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var searchResult = GetTravellerServices().ws_search(user_id, password, search, 0);

            

            for (int i = 0; i < searchResult.search_dets.Count; i++)
            {

                Debug.WriteLine(searchResult.search_dets.ElementAt(i).description +"-"+ searchResult.search_dets.ElementAt(i).result_type);
                    Debug.WriteLine("-----------");
                
            }

            return null;
        }

    }
}
