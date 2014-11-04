using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Merit.Traveller.BMS.BMS_Web_Permit_Data;
using Merit.Traveller.BMS.BMS_Web_Admin;
using Merit.Traveller.BMS.BMS_Web_Permit_Proc;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Globalization;

namespace Merit.Traveller.BMS.Controllers
{
    public class PermitIntrayController : Controller
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
        }

        //called each time a controller is called
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


        public ActionResult getEnquiryList(String filterOption, String fromDate, String toDate)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String office_guid = Request.Cookies["officeID"].Value;

            var enquiryListmodel = getWebPermitDataServices().ws_get_perm_list(user_id, password, filterOption, office_guid, fromDate, toDate);
            return PartialView("EnquiryList", enquiryListmodel);
        }
        public ActionResult getPermitProcessSummary(String permitID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);
            var permitProcDets = webpermitservices.ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);

            List<String> processes = new List<String>();
            List<String> processTypes = new List<String>();

            for (int i = 0; i < permitProcDets.line_txt.Count; i++)
            {
                if (permitProcDets.line_txt.ElementAt(i) != "")
                {
                    processes.Add(permitProcDets.line_guid.ElementAt(i));
                    if (permitProcDets.line_type.ElementAt(i) == "")
                        processTypes.Add("ApplicantDetails");
                    else
                        processTypes.Add("p" + permitProcDets.line_type.ElementAt(i));
                }
            }

            permitmodel pm = new permitmodel(
                permitID,
                permitDets.s_varn_ref.ToString(),
                permitDets.s_lodged_date,
                permitDets.s_lodged_at,
                permitDets.s_permit_class,
                permitDets.s_permit_type,
                permitDets.s_permit_no.ToString(),
                permitDets.s_outcome,
                permitDets.s_applicant,
                permitDets.s_app_dob,
                permitDets.s_app_nation,
                permitDets.s_decide_date,
                permitDets.s_finalised,
                permitProcDets,
                permitProcDets.dependant_guid,
                permitProcDets.sponsor_guid,
                processes,
                processTypes);

            return PartialView("permitDetails", pm);
        }

        [Authorize]
        public ActionResult Index(String permitID, String visaReg)
        {
            //check if the cookie has expired before continuing
            if (Request.Cookies["user_id"] != null)
            {

                //add 60 minutes to cookie expiry dates
                refreshCookies();

                ViewBag.isVisaRegScreen = visaReg;

                String user_id = Request.Cookies["user_id"].Value;
                String password = getPassword();

                ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();

                var webAdminDataServices = getWebAdminServices();
                String office_guid = Request.Cookies["officeID"].Value;
                merit_ini_list PermitFilters = webAdminDataServices.ws_get_permit_list_filters(user_id, password, office_guid);

                Debug.WriteLine("user_id: " + user_id);
                Debug.WriteLine("password: "+ password);

                var permitProcDets = getWebPermitDataServices().ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);
                ViewBag.dependentID = permitProcDets.dependant_guid;
            
                //fill select element with permit filters
                ViewBag.permitFilters = new List<merit_ini_details>();
                ViewBag.permitFilters = PermitFilters;

                ViewBag.permitID = permitID;
                Debug.WriteLine("permitID"+permitID);

                DateTime today = DateTime.Today;
                ViewBag.todaysDate = today.ToString();

                common_codes_list pClasses = webAdminDataServices.ws_get_pclasses(user_id, password);
                ViewBag.permitClasses = new List<ArrayOfCommon_codes_details>();
                ViewBag.permitClasses = pClasses.common_codes_dets;

                common_codes_list pTypes = webAdminDataServices.ws_get_ptypes(user_id, password);
                ViewBag.permitTypes = new List<ArrayOfCommon_codes_details>();
                ViewBag.permitTypes = pTypes.common_codes_dets;

                office_list missions = webAdminDataServices.ws_get_offices(user_id, password);
                ViewBag.missions = new List<office_details>();
                ViewBag.missions = missions.office_dets;

                common_codes_list outcomes = webAdminDataServices.ws_get_outcomes(user_id, password);
                ViewBag.outcomes = new List<common_codes_list>();
                ViewBag.outcomes = outcomes.common_codes_dets;

 
                var WebAdminWebServices = getWebAdminServices();
                common_codes_list pTypesList = WebAdminWebServices.ws_get_pclasses(user_id, password);
                ViewBag.pTypesList = new List<common_codes_list>();
                ViewBag.pTypesList = pTypesList.common_codes_dets;

                office_list of = WebAdminWebServices.ws_get_offices(user_id, password);
                ViewBag.offices = of;

                ViewBag.lodgedFor = Request.Cookies["officeID"].Value;

                doc_types_list documentTypes = WebAdminWebServices.ws_get_doc_types(user_id, password);
                ViewBag.docTypes = new List<doc_types_details>();
                ViewBag.docTypes = documentTypes.doc_types_dets;

                nationality_list nations = WebAdminWebServices.ws_get_nationality(user_id, password);
                ViewBag.nationalities = new List<nationality_details>();
                ViewBag.nationalities = nations.nationality_dets;

                /*common_codes_list projects = WebAdminWebServices.ws_get_projects(user_id, password);
                Debug.WriteLine(projects.ws_message);
                ViewBag.projects = new List<common_codes_details>();
                ViewBag.projects = projects.common_codes_dets;*/

                countries_list countries = getWebAdminServices().ws_get_countries(user_id, password);
                ViewBag.countries = countries;

                var receipts = getWebAdminServices().ws_get_receipt_list(user_id, password);
                ViewBag.receipts = receipts.common_codes_dets;


                ViewBag.officerList = getWebAdminServices().ws_get_officers(user_id, password);

                var officeList = getWebAdminServices().ws_get_offices(user_id, password);
                ViewBag.officeList = officeList;


                ViewBag.filterID = Request.Cookies["filterID"].Value;

                return View();
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        [Authorize]
        public ActionResult Proc(String permitID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);
            var permitProcDets = webpermitservices.ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);

            List<String> processes = new List<String>();
            List<String> processTypes = new List<String>();

            for (int i = 0; i < permitProcDets.line_txt.Count; i++)
            {
                if (permitProcDets.line_txt.ElementAt(i) != "")
                {
                    processes.Add(permitProcDets.line_guid.ElementAt(i));
                    if (permitProcDets.line_type.ElementAt(i) == "")
                        processTypes.Add("ApplicantDetails");
                    else
                        processTypes.Add("p" + permitProcDets.line_type.ElementAt(i));
                }
            }

            permitmodel pm = new permitmodel(
                permitID,
                permitDets.s_varn_ref.ToString(),
                permitDets.s_lodged_date,
                permitDets.s_lodged_at,
                permitDets.s_permit_class,
                permitDets.s_permit_type,
                permitDets.s_permit_no.ToString(),
                permitDets.s_outcome,
                permitDets.s_applicant,
                permitDets.s_app_dob,
                permitDets.s_app_nation,
                permitDets.s_decide_date,
                permitDets.s_finalised,
                permitProcDets,
                permitProcDets.dependant_guid,
                permitProcDets.sponsor_guid,
                processes,
                processTypes);
            return View(pm);
        }



        [Authorize]
        public ActionResult PermitIntray(String filterNum)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var permitDataServices = getWebPermitDataServices();
            var permitIntray = permitDataServices.ws_get_perm_det_list(user_id, password, filterNum);


            return PartialView(permitIntray);
        }

        public JsonResult getPermitProcesses(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string permitID = "")
        {

            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);
            var permitProcDets = webpermitservices.ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

          

            for (int i = jtStartIndex; i < permitProcDets.line_txt.Count; i++)
            {

                if (permitProcDets.line_txt.ElementAt(i) != "")
                {
                    //if there is no line_txt it is either applicant details or "no sponsor required". It is "no sponsor required"
                    //if line_txt says so. otherwise it is applicant details
                    if (permitProcDets.line_guid.ElementAt(i) == "" && permitProcDets.line_txt[i] == "Sponsor - No sponsor required")
                    {
                        var keyValues = new Dictionary<string, string>
                            {
                                { "processGUID", permitProcDets.line_guid.ElementAt(i)},
                                { "processNumber", "NoSponsor"},
                                { "Status", permitProcDets.line_icon[i]},
                                { "Process", permitProcDets.line_txt[i]},
                                { "IsModifiable", permitProcDets.line_can_modify[i]},
                                { "MustReassign", permitProcDets.line_reassign[i]},
                                { "canModify", permitProcDets.line_can_modify[i]}
                            };
                        data.Add(keyValues);
                    }
                    else if (permitProcDets.line_guid.ElementAt(i) == "" && permitProcDets.line_txt[i] == "Sponsor - Needs a sponsor")
                    {
                        var keyValues = new Dictionary<string, string>
                            {
                                { "processGUID", permitProcDets.line_guid.ElementAt(i)},
                                { "processNumber", "p999"},
                                { "Status", permitProcDets.line_icon[i]},
                                { "Process", permitProcDets.line_txt[i]},
                                { "IsModifiable", permitProcDets.line_can_modify[i]},
                                { "MustReassign", permitProcDets.line_reassign[i]},
                                { "canModify", permitProcDets.line_can_modify[i]},

                            };
                        data.Add(keyValues);
                    }
                    else if (permitProcDets.line_guid.ElementAt(i) == "")
                    {
                        var keyValues = new Dictionary<string, string>
                            {
                                { "processGUID", permitProcDets.line_guid.ElementAt(i)},
                                { "processNumber", "ApplicantDetails"},
                                { "Status", permitProcDets.line_icon[i]},
                                { "Process", permitProcDets.line_txt[i]},
                                { "IsModifiable", permitProcDets.line_can_modify[i]},
                                { "MustReassign", permitProcDets.line_reassign[i]},
                                { "canModify", permitProcDets.line_can_modify[i]}
                            };
                        data.Add(keyValues);
                    }
                    else{
                        var keyValues = new Dictionary<string, string>
                            {
                                { "processGUID", permitProcDets.line_guid.ElementAt(i)},
                                { "processNumber", "p"+permitProcDets.line_type.ElementAt(i)},
                                { "Status", permitProcDets.line_icon[i]},
                                { "Process", permitProcDets.line_txt[i]},
                                { "IsModifiable", permitProcDets.line_can_modify[i]},
                                { "MustReassign", permitProcDets.line_reassign[i]},
                                { "canModify", permitProcDets.line_can_modify[i]}
                            };
                        data.Add(keyValues);
                    }
                }
            }

            //get the last process number so we can determine on the client whether
            //we are up to the printing stage
            String lastProc = "";
            String lastProcCompleted = "";
            for (int i = 0; i < permitProcDets.line_type.Count(); i++)
            {
                lastProc = permitProcDets.line_type.ElementAt(i);
                lastProcCompleted = permitProcDets.line_icon.ElementAt(i);

                if ((i + 1) > permitProcDets.line_type.Count() - 1)
                    break;

                if ( permitProcDets.line_type.ElementAt(i+1) == "")
                    break;
            }

            String permNo="";
            if(permitDets.s_permit_no==0)
                permNo = "-";
            else
                permNo = permitDets.s_permit_no.ToString();

            try
            {
                return Json(new
                {
                    Result = "OK",
                    Records = data,
                    TotalRecordCount = permitProcDets.line_txt.Count,
                    dependentGUID = permitProcDets.dependant_guid,
                    sponsorGUID = permitProcDets.sponsor_guid,
                    varnRef = permitDets.s_varn_ref,
                    permitClass = permitDets.s_permit_class,
                    permitType = permitDets.s_permit_type,
                    applicant = permitDets.s_applicant,
                    dob = permitDets.s_app_dob,
                    nationality = permitDets.s_app_nation,
                    datelodged = permitDets.s_lodged_date,
                    lodgedAt = permitDets.s_lodged_at,
                    permitNo = permNo,
                    decideDate = permitDets.s_decide_date,
                    permID = permitID,
                    lastProc = lastProc,
                    lastProcCompleted = lastProcCompleted,
                    outcome = permitDets.s_outcome

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string filterNum = "", string filterString = "", string fromDate="", string toDate="")
        {
            String sortingKey = jtSorting.Split(' ')[0];
            String order = jtSorting.Split(' ')[1];
            String user_id = Request.Cookies["user_id"].Value;
            String office_guid = Request.Cookies["officeID"].Value;
            String password = getPassword();
            var permitDataServices = getWebPermitDataServices();
            BMS_Web_Permit_Data.permit_list_list permitIntray=null;

            //set to new filter
            Response.Cookies["filterID"].Value = filterNum;

            if (filterNum == "4" && fromDate != "" && toDate != "")
            {
                Response.Cookies["permitListDateFrom"].Value = fromDate;
                Response.Cookies["permitListDateTo"].Value = toDate;
                permitIntray = permitDataServices.ws_get_perm_list(user_id, password, filterNum, office_guid, fromDate, toDate);
            }
            else if (filterNum == "4" && (fromDate == "" || toDate == ""))
            {
                permitIntray = permitDataServices.ws_get_perm_list(user_id, password, filterNum, office_guid, Request.Cookies["permitListDateFrom"].Value, Request.Cookies["permitListDateTo"].Value);
            }
            else
            {
                Response.Cookies["permitListDateFrom"].Value = fromDate;
                Response.Cookies["permitListDateTo"].Value = toDate;
                permitIntray = permitDataServices.ws_get_perm_list(user_id, password, filterNum, office_guid, "", "");
            }
            
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            if (permitIntray.permit_list_dets.Count() == 0)
            {
                
                    var keyValues = new Dictionary<string, string>
                   {
                       { "permID", " "},
                       { "varnRef", " "},
                       { "PermitNo", " "},
                       { "surname", " "},
                       { "givenName", " "},
                       { "PermitClass", " "},
                       { "NextAction", " "},
                       { "Status", " "},
                       { "Finalised", " "},
                       { "DocumentNumber", " "},
                       { "DateLodged", " "},
                       { "Site", " "}
                   };
                    data.Add(keyValues);
                
                
                return Json(new { Result = "OK", Records = data, TotalRecordCount = permitIntray.permit_list_dets.Count() }, JsonRequestBehavior.AllowGet);
            }


            int iterator = 0;
            for (int i = jtStartIndex; i < permitIntray.permit_list_dets.Count(); i++)
            {
                

                //if we are loading the whole table
                if (filterString == "")
                {
                    Response.Cookies["permitListFilterString"].Value = "";
                        if (String.IsNullOrEmpty(permitIntray.permit_list_dets.ElementAt(i).document_no))
                            permitIntray.permit_list_dets.ElementAt(i).document_no = "none";

                        String permitNo = permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString();
                        if (permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString().Equals("0"))
                            permitNo = "";

                        var keyValues = new Dictionary<string, string>
                        {
                            { "permID", permitIntray.permit_list_dets.ElementAt(i).permit_guid.ToString()},
                            { "varnRef", permitIntray.permit_list_dets.ElementAt(i).varn_ref.ToString()},
                            { "PermitNo", permitNo},
                            { "surname", permitIntray.permit_list_dets.ElementAt(i).applic_surname_given.Split(' ')[0]},
                            { "givenName", permitIntray.permit_list_dets.ElementAt(i).given_name.ToString()},
                            { "PermitClass", permitIntray.permit_list_dets.ElementAt(i).permit_type_class},
                            { "NextAction", permitIntray.permit_list_dets.ElementAt(i).next_action},
                            { "Status", permitIntray.permit_list_dets.ElementAt(i).status_outcome},
                            { "Finalised", permitIntray.permit_list_dets.ElementAt(i).finalised_ind},
                            { "DocumentNumber", permitIntray.permit_list_dets.ElementAt(i).document_no},
                            { "DateLodged", permitIntray.permit_list_dets.ElementAt(i).date_lodged.ToString("dd-MMM-yyyy").Split(' ')[0]},
                            { "Site", permitIntray.permit_list_dets.ElementAt(i).lodged_at_for}
                        };
                        data.Add(keyValues);

                        iterator++;
                        if (iterator == jtPageSize)
                            break;
                }
                else //if we have a filter 
                {
                    Response.Cookies["permitListFilterString"].Value = filterString;
                    
                        if (String.IsNullOrEmpty(permitIntray.permit_list_dets.ElementAt(i).document_no))
                            permitIntray.permit_list_dets.ElementAt(i).document_no = "none";

                        String permitNo = permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString();
                        if (permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString().Equals("0"))
                            permitNo = "";

                        //this object used for ignoring case
                        CompareInfo ci = CultureInfo.CurrentCulture.CompareInfo;
               
             
                        //if we find a match in any of these fields:
                        if (
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).varn_ref.ToString(), filterString, CompareOptions.IgnoreCase) != -1 ||
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString(), filterString, CompareOptions.IgnoreCase) != -1 ||
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).given_name, filterString, CompareOptions.IgnoreCase) != -1 ||
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).surname, filterString, CompareOptions.IgnoreCase) != -1 ||
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).permit_type_class, filterString, CompareOptions.IgnoreCase) != -1 ||
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).status_outcome, filterString, CompareOptions.IgnoreCase) != -1 ||
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).document_no, filterString, CompareOptions.IgnoreCase) != -1 ||
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).date_lodged.ToString().Split(' ')[0], filterString, CompareOptions.IgnoreCase) != -1 ||
                            ci.IndexOf(permitIntray.permit_list_dets.ElementAt(i).lodged_at_for, filterString, CompareOptions.IgnoreCase) != -1)
                        {
                            var keyValues = new Dictionary<string, string>
                        {
                            { "permID", permitIntray.permit_list_dets.ElementAt(i).permit_guid.ToString()},
                            { "varnRef", permitIntray.permit_list_dets.ElementAt(i).varn_ref.ToString()},
                            { "PermitNo", permitNo},
                            { "surname", permitIntray.permit_list_dets.ElementAt(i).surname},
                            { "givenName", permitIntray.permit_list_dets.ElementAt(i).given_name},
                            { "PermitClass", permitIntray.permit_list_dets.ElementAt(i).permit_type_class},
                            { "NextAction", permitIntray.permit_list_dets.ElementAt(i).next_action},
                            { "Status", permitIntray.permit_list_dets.ElementAt(i).status_outcome},
                            { "Finalised", permitIntray.permit_list_dets.ElementAt(i).finalised_ind},
                            { "DocumentNumber", permitIntray.permit_list_dets.ElementAt(i).document_no},
                            { "DateLodged", permitIntray.permit_list_dets.ElementAt(i).date_lodged.ToString("dd-MMM-yyyy").Split(' ')[0]},
                            { "Site", permitIntray.permit_list_dets.ElementAt(i).lodged_at_for}
                
                        };
                            data.Add(keyValues);
                        }

                        iterator++;
                        if (iterator == jtPageSize)
                            break;
                    }
                
                
            }

            //sort records with LINQ
            var sortedRecords = from record in data select record;

            if (!sortingKey.Equals("DateLodged"))
            {
                
                if (order.Equals("DESC"))
                {
                    sortedRecords =
                        from record in data
                        orderby record[sortingKey] descending
                        select record;
                }
                else
                {
                    sortedRecords =
                        from record in data
                        orderby record[sortingKey] ascending
                        select record;
                }
            }
            else
            {
                if (order.Equals("DESC"))
                {
                    sortedRecords =
                        from record in data
                        orderby Convert.ToDateTime(record[sortingKey]) descending
                        select record;
                }
                else
                {
                    sortedRecords =
                        from record in data
                        orderby Convert.ToDateTime(record[sortingKey]) ascending
                        select record;
                }

            }
            List<Dictionary<string, string>> listifiedIEnumerable =  sortedRecords.ToList();

  
            try
            {
                return Json(new { Result = "OK", Records = listifiedIEnumerable, TotalRecordCount = permitIntray.permit_list_dets.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public void getCalendarFirst()
        {
            String user_id = Request.Cookies["user_id"].Value;
            String office_guid = Request.Cookies["officeID"].Value;
            String password = getPassword();
            var permitDataServices = getWebPermitDataServices();
            permit_list_list permitIntray = null;
            JavaScriptSerializer objSerializer = new JavaScriptSerializer();

            Response.Cookies["permitListDateFrom"].Value = "";
            Response.Cookies["permitListDateTo"].Value = "";
            permitIntray = permitDataServices.ws_get_perm_list(user_id, password, "1", office_guid, "", "");

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            if (permitIntray.permit_list_dets.Count() == 0)
            {

                var keyValues = new Dictionary<string, string>
                   {
                       { "permID", " "},
                       { "varnRef", " "},
                       { "PermitNo", " "},
                       { "surname", " "},
                       { "givenName", " "},
                       { "PermitClass", " "},
                       { "NextAction", " "},
                       { "Status", " "},
                       { "Finalised", " "},
                       { "DocumentNumber", " "},
                       { "DateLodged", " "},
                       { "Site", " "}
                   };
                data.Add(keyValues);

                Response.Write(objSerializer.Serialize(Json(new { Result = "OK", Records = data, TotalRecordCount = permitIntray.permit_list_dets.Count() })));
                //return Json(new { Result = "OK", Records = data, TotalRecordCount = permitIntray.permit_list_dets.Count() }, JsonRequestBehavior.AllowGet);
            }
            int iterator = 0;
            for (int i = 0; i < permitIntray.permit_list_dets.Count(); i++)
            {
                Response.Cookies["permitListFilterString"].Value = "";
                if (String.IsNullOrEmpty(permitIntray.permit_list_dets.ElementAt(i).document_no))
                    permitIntray.permit_list_dets.ElementAt(i).document_no = "none";

                String permitNo = permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString();
                if (permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString().Equals("0"))
                    permitNo = "";

                var keyValues = new Dictionary<string, string>
                        {
                            { "permID", permitIntray.permit_list_dets.ElementAt(i).permit_guid.ToString()},
                            { "varnRef", permitIntray.permit_list_dets.ElementAt(i).varn_ref.ToString()},
                            { "PermitNo", permitNo},
                            { "surname", permitIntray.permit_list_dets.ElementAt(i).applic_surname_given.Split(' ')[0]},
                            { "givenName", permitIntray.permit_list_dets.ElementAt(i).given_name.ToString()},
                            { "PermitClass", permitIntray.permit_list_dets.ElementAt(i).permit_type_class},
                            { "NextAction", permitIntray.permit_list_dets.ElementAt(i).next_action},
                            { "Status", permitIntray.permit_list_dets.ElementAt(i).status_outcome},
                            { "Finalised", permitIntray.permit_list_dets.ElementAt(i).finalised_ind},
                            { "DocumentNumber", permitIntray.permit_list_dets.ElementAt(i).document_no},
                            { "DateLodged", permitIntray.permit_list_dets.ElementAt(i).date_lodged.ToString("dd-MMM-yyyy").Split(' ')[0]},
                            { "Site", permitIntray.permit_list_dets.ElementAt(i).lodged_at_for}
                        };
                data.Add(keyValues);
                iterator++;
            }

            try
            {
                Response.Write(objSerializer.Serialize(Json(new { Result = "OK", Records = data, TotalRecordCount = permitIntray.permit_list_dets.Count() })));
            }
            catch (Exception ex)
            {
                Response.Write(objSerializer.Serialize("Error"));
            }

        }

        public void getCalendarSecond()
        {
            String user_id = Request.Cookies["user_id"].Value;
            String office_guid = Request.Cookies["officeID"].Value;
            String password = getPassword();
            var permitDataServices = getWebPermitDataServices();
            permit_list_list permitIntray = null;
            JavaScriptSerializer objSerializer = new JavaScriptSerializer();

            Response.Cookies["permitListDateFrom"].Value = "";
            Response.Cookies["permitListDateTo"].Value = "";
            permitIntray = permitDataServices.ws_get_perm_list(user_id, password, "2", office_guid, "", "");

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            if (permitIntray.permit_list_dets.Count() == 0)
            {

                var keyValues = new Dictionary<string, string>
                   {
                       { "permID", " "},
                       { "varnRef", " "},
                       { "PermitNo", " "},
                       { "surname", " "},
                       { "givenName", " "},
                       { "PermitClass", " "},
                       { "NextAction", " "},
                       { "Status", " "},
                       { "Finalised", " "},
                       { "DocumentNumber", " "},
                       { "DateLodged", " "},
                       { "Site", " "}
                   };
                data.Add(keyValues);

                Response.Write(objSerializer.Serialize(Json(new { Result = "OK", Records = data, TotalRecordCount = permitIntray.permit_list_dets.Count() })));
                //return Json(new { Result = "OK", Records = data, TotalRecordCount = permitIntray.permit_list_dets.Count() }, JsonRequestBehavior.AllowGet);
            }
            int iterator = 0;
            for (int i = 0; i < permitIntray.permit_list_dets.Count(); i++)
            {
                Response.Cookies["permitListFilterString"].Value = "";
                if (String.IsNullOrEmpty(permitIntray.permit_list_dets.ElementAt(i).document_no))
                    permitIntray.permit_list_dets.ElementAt(i).document_no = "none";

                String permitNo = permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString();
                if (permitIntray.permit_list_dets.ElementAt(i).permit_no.ToString().Equals("0"))
                    permitNo = "";

                var keyValues = new Dictionary<string, string>
                        {
                            { "permID", permitIntray.permit_list_dets.ElementAt(i).permit_guid.ToString()},
                            { "varnRef", permitIntray.permit_list_dets.ElementAt(i).varn_ref.ToString()},
                            { "PermitNo", permitNo},
                            { "surname", permitIntray.permit_list_dets.ElementAt(i).applic_surname_given.Split(' ')[0]},
                            { "givenName", permitIntray.permit_list_dets.ElementAt(i).given_name.ToString()},
                            { "PermitClass", permitIntray.permit_list_dets.ElementAt(i).permit_type_class},
                            { "NextAction", permitIntray.permit_list_dets.ElementAt(i).next_action},
                            { "Status", permitIntray.permit_list_dets.ElementAt(i).status_outcome},
                            { "Finalised", permitIntray.permit_list_dets.ElementAt(i).finalised_ind},
                            { "DocumentNumber", permitIntray.permit_list_dets.ElementAt(i).document_no},
                            { "DateLodged", permitIntray.permit_list_dets.ElementAt(i).date_lodged.ToString("dd-MMM-yyyy").Split(' ')[0]},
                            { "Site", permitIntray.permit_list_dets.ElementAt(i).lodged_at_for}
                        };
                data.Add(keyValues);
                iterator++;
            }

            try
            {
                Response.Write(objSerializer.Serialize(Json(new { Result = "OK", Records = data, TotalRecordCount = permitIntray.permit_list_dets.Count() })));
            }
            catch (Exception ex)
            {
                Response.Write(objSerializer.Serialize("Error"));
            }

        }





        public JsonResult generateSearchQueryTable(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string SearchQuery= "")
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            var permitDataServices = getWebPermitDataServices();
            var permitIntray = permitDataServices.ws_get_perm_det_list(user_id, password, "1");

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            try
            {
                return Json(new { Result = "OK", Records = data, TotalRecordCount = permitIntray.permit_list_dets.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult permitdetails(string id)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

     

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, id);
            var permitProcDets = webpermitservices.ws_get_proc_permit_dets(user_id, password, id, Request.Cookies["officeID"].Value);

    

            List<String> processes = new List<String>();
            List<String> processTypes = new List<String>();

            for (int i = 0; i < permitProcDets.line_txt.Count; i++)
            {
                if (permitProcDets.line_txt.ElementAt(i) != "")
                {
                    processes.Add(permitProcDets.line_guid.ElementAt(i));
                    if (permitProcDets.line_type.ElementAt(i) == "")
                        processTypes.Add("ApplicantDetails");
                    else
                        processTypes.Add("p" + permitProcDets.line_type.ElementAt(i));
                }
            }
       
            permitmodel pm = new permitmodel(
                id,
                permitDets.s_varn_ref.ToString(),
                permitDets.s_lodged_date,
                permitDets.s_lodged_at,
                permitDets.s_permit_class,
                permitDets.s_permit_type,
                permitDets.s_permit_no.ToString(),
                permitDets.s_outcome,
                permitDets.s_applicant,
                permitDets.s_app_dob,
                permitDets.s_app_nation,
                permitDets.s_decide_date,
                permitDets.s_finalised,
                permitProcDets,
                permitProcDets.dependant_guid,
                permitProcDets.sponsor_guid,
                processes,
                processTypes);

            return PartialView(pm);
        }


        [Authorize]
        public JsonResult advancedPermitIntray(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null,
                                                 String dateLodgedFrom = "", String dateLodgedTo = "", String dateApprovedFrom = "",
                                                 String dateApprovedTo = "", String varn = "", String permitNo = "", String permitType = "",
                                                 String permitClass = "", String mission = "", String outcome = "", String finalised = "",
                                                 String docNo = "", String givenName = "", String surname = "")
        {
            

            //if null, this means an invalid date was set. set default date. 
            DateTime dtDateLodgedFrom = DateTime.Parse(dateLodgedFrom);
            DateTime dtDateLodgedTo = DateTime.Parse(dateLodgedTo);
            DateTime dtApprovedFrom = DateTime.Parse(dateApprovedFrom);
            DateTime dtApprovedTo = DateTime.Parse(dateApprovedTo);

            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            permit_search searchCriteria = new permit_search();
            searchCriteria.date_approved_from = dtApprovedFrom.ToString();
            searchCriteria.date_approved_to = dtApprovedTo.ToString();
            searchCriteria.date_lodged_from = dtApprovedFrom.ToString();
            searchCriteria.date_lodged_to = dtDateLodgedTo.ToString();
            searchCriteria.doc_number = docNo;
            searchCriteria.finalised_ind = finalised;
            searchCriteria.given_name = givenName;
            searchCriteria.outcome = outcome;
            searchCriteria.permit_class = permitClass;
            searchCriteria.permit_no = Convert.ToInt32(permitNo);
            searchCriteria.permit_type = permitType;
            searchCriteria.site = mission;
            searchCriteria.surname = surname;
            searchCriteria.varn_ref = Convert.ToInt32(varn);
            var permitIntray = getWebPermitDataServices().ws_search_perm_list(user_id, password, searchCriteria);
            
            //return PartialView(permitIntray);
            return null;
            

        }
        public String changeSite(String site)
        {
            Response.Cookies["officeID"].Value= site.Split('|').ElementAt(0);
            Response.Cookies["centralName"].Value = site.Split('|').ElementAt(1);
            return "success";
        }

        public String reassignAction( String comments, String intray_guid, String permit_guid)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String officeID = Request.Cookies["officeID"].Value;
            String password = Request.Cookies["password"].Value;

            var result = getWebPermitProcServices().ws_reassign_action(user_id, password, intray_guid, permit_guid, comments, officeID);

            if (result.ws_status != 0)
                return result.ws_message;
            else
                return "success";
        }

    }
}
