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
    public class ActionSummaryController : Controller
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
        // GET: /ActionSummary/

        public ActionResult Index(int aid)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;



            var actionDetails = GetMeritActionServices().ws_get_action_details(user_id, password, aid);
            var requestDetails = GetMeritRequestServices().ws_get_request_details(user_id, password, actionDetails.request_id);

            ViewBag.actionID = aid;
            ViewBag.name = actionDetails.reason_assigned_name;
            ViewBag.dateInput = actionDetails.assign_datetime;
            ViewBag.dueDate = actionDetails.due_datetime;
            ViewBag.completeDate = actionDetails.outcome_datetime;
            ViewBag.actionOfficer = actionDetails.action_officer_name;
            ViewBag.inputOfficer = requestDetails.input_by_name;
            ViewBag.completedOutcome = actionDetails.outcome_name;
            ViewBag.Priority = actionDetails.priority;
            ViewBag.ReferenceNo = requestDetails.refer_no;
            ViewBag.howReceived = requestDetails.how_received_name;
            ViewBag.locationAddress = requestDetails.address_det;
            ViewBag.locationAddressDesc = requestDetails.address_desc;

            String temp="";
            for (int i = 0; i < requestDetails.customer_name_det.Count; i++)
            {
                temp += requestDetails.customer_name_det.ElementAt(i).given_names + ", ";
                Debug.WriteLine(temp);
            }
            ViewBag.customerName = temp;

            for (int i = 0; i < requestDetails.customer_name_det.Count; i++)
            {
                temp += requestDetails.customer_name_det.ElementAt(i).company_name + ", ";
            }
            ViewBag.companyName = temp;
            ViewBag.requestDesc = requestDetails.request_description;
            ViewBag.requestInstructions = requestDetails.request_instruction;


            return View();
        }

    }
}
