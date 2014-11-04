using Merit.Traveller.BMS.BMS_Web_Admin;
using Merit.Traveller.BMS.BMS_Web_Permit_Data;
using Merit.Traveller.BMS.BMS_Web_Permit_Proc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace Merit.Traveller.BMS.Controllers
{
    public class MessagesController : Controller
    {
        public static bms_web_permit_procSoapClient getWebPermitProcServices()
        {
            var basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.MaxBufferPoolSize = 10000000;
            basicHttpBinding.MaxBufferSize = 10000000;
            basicHttpBinding.MaxReceivedMessageSize = 10000000;

            String webServicesEndpointAddress = System.Configuration.ConfigurationManager.AppSettings["WebService_PermitProc"].ToString();
            var endpointAddress = new EndpointAddress(webServicesEndpointAddress);
            new ChannelFactory<bms_web_permit_procSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new bms_web_permit_procSoapClient(basicHttpBinding, endpointAddress);
            return webServices;
        }

        public static bms_web_adminSoapClient getWebAdminServices()
        {
            var basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.MaxBufferPoolSize = 10000000;
            basicHttpBinding.MaxBufferSize = 10000000;
            basicHttpBinding.MaxReceivedMessageSize = 10000000;
            String webServicesEndpointAddress = System.Configuration.ConfigurationManager.AppSettings["WebService_Admin"].ToString();
            var endpointAddress = new EndpointAddress(webServicesEndpointAddress);
            new ChannelFactory<bms_web_adminSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new bms_web_adminSoapClient(basicHttpBinding, endpointAddress);
            return webServices;
        }

        public static bms_web_permit_dataSoapClient getWebPermitDataServices()
        {
            var basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.MaxBufferPoolSize = 10000000;
            basicHttpBinding.MaxBufferSize = 10000000;
            basicHttpBinding.MaxReceivedMessageSize = 10000000;
            String webServicesEndpointAddress = System.Configuration.ConfigurationManager.AppSettings["WebService_PermitData"].ToString();
            var endpointAddress = new EndpointAddress(webServicesEndpointAddress);
            new ChannelFactory<bms_web_permit_dataSoap>(basicHttpBinding, endpointAddress).CreateChannel();
            var webServices = new bms_web_permit_dataSoapClient(basicHttpBinding, endpointAddress);
            return webServices;
        }

        public String getPassword()
        {
            return Request.Cookies["password"].Value;
            //return "Æßæ";
        }

        //
        // GET: /Messages/

        public void refreshCookies()
        {
            /*Request.Cookies["user_id"].Expires.AddMinutes(60);
            Request.Cookies["password"].Expires.AddMinutes(60);
            Request.Cookies["mission"].Expires.AddMinutes(60);
            Request.Cookies["officeID"].Expires.AddMinutes(60);
            Request.Cookies["officerID"].Expires.AddMinutes(60);
            Request.Cookies["centralID"].Expires.AddMinutes(60);
            Request.Cookies["centralName"].Expires.AddMinutes(60);*/
        }

        public ActionResult Index()
        {
            //check if the cookie has expired before continuing
            if (Request.Cookies["user_id"] != null)
            {
                //add 60 minutes to cookie expiry dates
                refreshCookies();


                ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();

                String user_id = Request.Cookies["user_id"].Value;
                String password = getPassword();

                ViewBag.officerList = getWebAdminServices().ws_get_officers(user_id, getPassword());
                return View();
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public JsonResult getMessagesList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string filterNum = "1")
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;


            var messageList = getWebPermitDataServices().ws_get_received_msgs(user_id, password, "N");

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            int iterations = 0;

            for (int i = jtStartIndex; i < messageList.message_dets.Count(); i++)
            {

                var keyValues = new Dictionary<string, string>
                   {
                       { "read",  messageList.message_dets.ElementAt(i).read_ind},
                       { "messageID", messageList.message_dets.ElementAt(i).message_guid},
                       { "from", messageList.message_dets.ElementAt(i).sent_by_name},
                       { "subject", messageList.message_dets.ElementAt(i).subject_txt},
                       { "received", messageList.message_dets.ElementAt(i).sent_date.ToString()},
                       { "contents", messageList.message_dets.ElementAt(i).message_txt}
                  
                
                   };
                data.Add(keyValues);
                iterations++;
            }
            //if there is <10 records on the last page of the table, we need to add some blanks to keep page heights consistent
            if (data.Count % 10 != 0)
            {
                for (int i = 0; i <  data.Count % 10; i++)
                {
                    var keyValues = new Dictionary<string, string>
                   {
                       { "read",  "-"},
                       { "messageID", "-"},
                       { "from", "-"},
                       { "subject", "-"},
                       { "received", "-"},
                       { "contents", "-"}
                   };
                    data.Add(keyValues);
                }
            }

            try
            {
                return Json(new { Result = "OK", Records = data, TotalRecordCount = messageList.message_dets.Count() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public String deleteMessage(String messageID)
        {
            return "success";
        }

        [HttpPost]
        public JsonResult messageDetails(String messageID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var messages = getWebPermitDataServices().ws_get_received_msgs(user_id, password, "N");


            for (int i = 0; i < messages.message_dets.Count; i++)
            {
                if(messages.message_dets.ElementAt(i).message_guid.Equals(messageID)){
                    return Json(new { Message = messageID,
                                      subject = messages.message_dets.ElementAt(i).subject_txt,
                                      datereceived = messages.message_dets.ElementAt(i).sent_date.ToString(),
                                      from = messages.message_dets.ElementAt(i).sent_by_name, 
                                      contents = messages.message_dets.ElementAt(i).message_txt
                    });
                }

            }
            return Json(new { Error = "not found"});

            
        }

        public JsonResult getNumberUnreadMessages()
        {

            int unreads = 0;

            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var messages = getWebPermitDataServices().ws_get_received_msgs(user_id, password, "N");

            for (int i = 0; i < messages.message_dets.Count; i++)
            {
                if (messages.message_dets.ElementAt(i).read_ind.Equals("N"))
                {
                    unreads++;
                }

            }
            return Json(new { number = unreads.ToString() });


        }

        public String sendMessage(String message_txt,String subject_txt, String recipients)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            BMS_Web_Permit_Data.ArrayOfString sent_to = new BMS_Web_Permit_Data.ArrayOfString();
            sent_to.Add(recipients);

            var result = getWebPermitDataServices().ws_send_message(user_id, password, sent_to, message_txt, subject_txt);

            if (result.ws_status != 0)
            {
                return result.ws_message;
            }
            else
            {
                return "success";
            }

        }

    }
}
