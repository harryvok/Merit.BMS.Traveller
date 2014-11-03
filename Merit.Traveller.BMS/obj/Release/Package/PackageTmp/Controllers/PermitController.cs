using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Merit.Traveller.BMS.BMS_Web_Permit_Data;
using Merit.Traveller.BMS.BMS_Web_Admin;
using System.ServiceModel;
using Merit.Traveller.BMS.BMS_Web_Permit_Proc;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Merit.Traveller.BMS.Controllers
{
    public class PermitController : Controller
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


        public void getProcesses(string id)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, id);

            

            ViewBag.guid = id;
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;

            var permitProcDets = webpermitservices.ws_get_proc_permit_dets(user_id, password, id, Request.Cookies["officeID"].Value);
            ViewBag.processDescs = permitProcDets;
            ViewBag.dependentID = permitProcDets.dependant_guid;
            ViewBag.sponsorID = permitProcDets.sponsor_guid;

            //ViewBag.intrayID = permitProcDets.line_guid;

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
            ViewBag.processes = new List<String>();
            ViewBag.processes = processes;

            ViewBag.processTypes = new List<String>();
            ViewBag.processTypes = processTypes;
        }

        public String getNextUncompletedProcess(String permitID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var webpermitservices = getWebPermitDataServices();
            var permitProcDets = webpermitservices.ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);

            for (int i = 0; i < permitProcDets.line_txt.Count; i++)
            {
                Debug.WriteLine(permitProcDets.line_type.ElementAt(i) + permitProcDets.line_icon.ElementAt(i));
                if (permitProcDets.line_icon.ElementAt(i).Equals("N"))
                {
                    return "p" + permitProcDets.line_type.ElementAt(i);
                }
            }
            return "completed";
        }

        [Authorize]
        public ActionResult Index(String id)
        {
            Debug.WriteLine(id);


            //check if the cookie has expired before continuing
            if (Request.Cookies["user_id"] != null)
            {

                this.getProcesses(id);
                ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();

                return View();
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult Summary(String id)
        {
            this.getProcesses(id);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, id);

            Debug.WriteLine(permitDets.s_permit_no);

            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.guid = id;
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;

            var officeList = getWebAdminServices().ws_get_offices(user_id, password);
            ViewBag.officeList = officeList;

            return View();
        }

        public ActionResult p455(String permitID, String dependentID, String intrayID, String modify)
        {
            this.getProcesses(permitID);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;
            Debug.WriteLine("cen" + Request.Cookies["centralID"].Value);

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);
            
            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.guid = permitID;
            ViewBag.intrayID = intrayID;
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.officeList = getWebAdminServices().ws_get_offices(user_id, password);
            ViewBag.fromDate = DateTime.Now.ToString("dd/MM/yy");
            ViewBag.isModifiable = modify;

            //get decision details
            var permitDataservices = getWebPermitDataServices();
            var decisionDets = permitDataservices.ws_get_proc_decision_dets(user_id, password, intrayID, centralID);

            ViewBag.applicant = decisionDets.s_applicant_name;
            ViewBag.fromText = decisionDets.s_from_officer_name;
            ViewBag.recommendText = decisionDets.s_recommend_txt;
            ViewBag.controlText = decisionDets.s_control_txt.Split('|');
            ViewBag.validFrom = decisionDets.s_date_valid_from.ToString().Split(' ').ElementAt(0);
            Debug.WriteLine(decisionDets.s_date_valid_from);
            Debug.WriteLine(decisionDets.s_date_valid_to);
            ViewBag.validTo = decisionDets.s_date_valid_to.ToString().Split(' ').ElementAt(0);
            ViewBag.comments = decisionDets.s_comment_txt;

            var permitProcDets = getWebPermitDataServices().ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);

            //check whether the process is finished or not. If it is, 
            //set ViewBag.editable to false so no fields can be edited
            for (int i = 0; i < permitProcDets.line_txt.Count; i++)
            {
                Debug.WriteLine(permitProcDets.line_type.ElementAt(i) + " " + permitProcDets.line_icon.ElementAt(i) + " " + permitProcDets.line_txt.ElementAt(i));

                //compare to 455 means check if it is check requirements
                if (permitProcDets.line_type.ElementAt(i) == "455")
                {
                    //editable is set to either "Y" for 'Yes step is complete therefore UNEDITABLE'
                    //or is set to "N" for 'No step is incomplete therefore EDITABLE'
                    ViewBag.editable = permitProcDets.line_icon.ElementAt(i);
                }
            }
            var checkItems = getWebPermitDataServices().ws_get_proc_check_items(user_id, password, permitID, intrayID, centralID, officeID, officerID);

            return View();
        }

        public String approvePermit()
        {
            return "";
        }

        public ActionResult p456(String permitID, String intrayID, String modify)
        {
            this.getProcesses(permitID);

            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);

            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.howUsed = getWebAdminServices().ws_get_how_used(user_id, password);
            ViewBag.isModifiable = modify;
            ViewBag.intrayID = intrayID;
            ViewBag.documentURL = createVisaPDF(permitID);
            return View();
        }

        public string createVisaPDF(String permit_guid){

            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            //create pdf
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.MFEH = PdfFontEmbedding.Automatic;
            XFont font = new XFont("OCR-B-10 BT", 10, XFontStyle.Regular);

            var strings = getWebPermitProcServices().ws_print_visa(user_id, password, permit_guid);
            var lines = strings.visa_print_dets.ElementAt(0);

            gfx.DrawString(lines.line_1, font, XBrushes.Black, 130, 100, XStringFormats.Default);
            gfx.DrawString(lines.line_2, font, XBrushes.Black, 130, 110, XStringFormats.Default);
            gfx.DrawString(lines.line_3, font, XBrushes.Black, 130, 120, XStringFormats.Default);
            gfx.DrawString(lines.line_4, font, XBrushes.Black, 130, 130, XStringFormats.Default);
            gfx.DrawString(lines.line_5, font, XBrushes.Black, 130, 140, XStringFormats.Default);
            gfx.DrawString(lines.line_6, font, XBrushes.Black, 130, 150, XStringFormats.Default);
            gfx.DrawString(lines.line_7, font, XBrushes.Black, 130, 160, XStringFormats.Default);
            gfx.DrawString(lines.line_8, font, XBrushes.Black, 130, 170, XStringFormats.Default);
            gfx.DrawString(lines.line_9, font, XBrushes.Black, 130, 180, XStringFormats.Default);
            gfx.DrawString(lines.line_10, font, XBrushes.Black, 130, 190, XStringFormats.Default);
            gfx.DrawString(lines.line_11, font, XBrushes.Black, 130, 200, XStringFormats.Default);
            gfx.DrawString(lines.line_12, font, XBrushes.Black, 130, 210, XStringFormats.Default);
            gfx.DrawString(lines.line_13, font, XBrushes.Black, 130, 220, XStringFormats.Default);
            gfx.DrawString(lines.line_14, font, XBrushes.Black, 130, 230, XStringFormats.Default);
            gfx.DrawString(lines.line_15, font, XBrushes.Black, 130, 240, XStringFormats.Default);
            gfx.DrawString(lines.line_16, font, XBrushes.Black, 130, 250, XStringFormats.Default);
            gfx.DrawString(lines.line_17, font, XBrushes.Black, 130, 260, XStringFormats.Default);
            gfx.DrawString(lines.line_18, font, XBrushes.Black, 130, 270, XStringFormats.Default);
            gfx.DrawString(lines.line_19, font, XBrushes.Black, 130, 280, XStringFormats.Default);
            gfx.DrawString(lines.line_20, font, XBrushes.Black, 130, 290, XStringFormats.Default);
            gfx.DrawString(lines.line_21, font, XBrushes.Black, 130, 300, XStringFormats.Default);
            gfx.DrawString(lines.line_22, font, XBrushes.Black, 130, 310, XStringFormats.Default);
            gfx.DrawString(lines.line_23, font, XBrushes.Black, 130, 320, XStringFormats.Default);
            gfx.DrawString(lines.line_24, font, XBrushes.Black, 130, 330, XStringFormats.Default);
            gfx.DrawString(lines.line_25, font, XBrushes.Black, 130, 340, XStringFormats.Default);
            gfx.DrawString(lines.line_26, font, XBrushes.Black, 130, 350, XStringFormats.Default);
            gfx.DrawString(lines.line_27, font, XBrushes.Black, 130, 360, XStringFormats.Default);
            gfx.DrawString(lines.line_28, font, XBrushes.Black, 130, 370, XStringFormats.Default);
            gfx.DrawString(lines.line_29, font, XBrushes.Black, 130, 380, XStringFormats.Default);
            gfx.DrawString(lines.line_30, font, XBrushes.Black, 130, 390, XStringFormats.Default);
            gfx.DrawString(lines.line_31, font, XBrushes.Black, 130, 400, XStringFormats.Default);
            gfx.DrawString(lines.line_32, font, XBrushes.Black, 130, 410, XStringFormats.Default);
            gfx.DrawString(lines.line_33, font, XBrushes.Black, 130, 420, XStringFormats.Default);
            gfx.DrawString(lines.line_34, font, XBrushes.Black, 130, 430, XStringFormats.Default);
            gfx.DrawString(lines.line_35, font, XBrushes.Black, 130, 440, XStringFormats.Default);
            gfx.DrawString(lines.line_36, font, XBrushes.Black, 130, 450, XStringFormats.Default);
            gfx.DrawString(lines.line_37, font, XBrushes.Black, 130, 460, XStringFormats.Default);
            gfx.DrawString(lines.line_38, font, XBrushes.Black, 130, 470, XStringFormats.Default);
            gfx.DrawString(lines.line_39, font, XBrushes.Black, 130, 480, XStringFormats.Default);

            document.Info.Title = "Visa Permit";

            

            String filename = "C:\\inetpub\\wwwroot\\Merit.BMS.Traveller" + "\\pdf\\permit" + permit_guid + ".pdf";
            document.Save(filename);

            return "permit" + permit_guid + ".pdf|" + strings.need_vlid;
        }
        /*public String createLetter(String varn)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont boldfont = new XFont("Verdana", 10, XFontStyle.Bold);
            XFont font = new XFont("Verdana", 10, XFontStyle.Regular);

            document.Info.Title = "eVisa";
            gfx.DrawString("TEST BMS letter [PROTOTYPE] ", boldfont, XBrushes.Black, 130, 100, XStringFormats.Default);

            String filename = "C:\\inetpub\\wwwroot\\Merit.BMS.Traveller" + "\\pdf\\" + varn + "letter.pdf";
            document.Save(filename);

            return "";
        }*/
        public String processPrint(String permit_guid, String intrayGUID, String vlid_series, String vlid_id, String how_used, String comments,String process_type, String bypass_comment){

            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String officeID = Request.Cookies["officeID"].Value;
            String centralID = Request.Cookies["centralID"].Value;

            var result = getWebPermitProcServices().ws_upd_perm_vlid(user_id, password, permit_guid, vlid_series, Convert.ToInt32(vlid_id), officeID, how_used, comments, process_type, bypass_comment, intrayGUID, centralID); 
            if (result.ws_status != 0)
                return result.ws_message;

            var result2 = getWebPermitProcServices().ws_upd_print_visa(user_id, password, permit_guid, intrayGUID, vlid_id, officeID, process_type, comments, centralID);
            if (result2.ws_status != 0)
                return result2.ws_message;

            return "success";
        }

        public ActionResult p454(string id)
        {
            this.getProcesses(id);
            return View();
        }

        public ActionResult p457(string id)
        {
            this.getProcesses(id);
            return View();
        }

        public ActionResult p462(String permitID, String dependentID, String intrayID, String modify)
        {
            this.getProcesses(permitID);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);

            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.guid = permitID;
            ViewBag.intrayID = intrayID;
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.officeList = getWebAdminServices().ws_get_offices(user_id, password);
            //recommendation info
            var recommendationDetails = webpermitservices.ws_get_recommend_dets(user_id, password, intrayID);

            ViewBag.isModifiable = modify;
     
            ViewBag.from = recommendationDetails.from_txt;
            ViewBag.action = recommendationDetails.action_txt;
            ViewBag.given = recommendationDetails.given_txt;
            ViewBag.forText = recommendationDetails.for_txt;
            ViewBag.Due = recommendationDetails.due_txt;
            ViewBag.recs = recommendationDetails.recommend_txt;

            if (recommendationDetails.validity_date_from == "")
            {
                ViewBag.fromDate = DateTime.Now.ToString("dd/MM/yy");
            }
            else
            {
                ViewBag.fromDate = recommendationDetails.validity_date_from;
            }

            if (recommendationDetails.validity_date_to == "")
            {
                ViewBag.toDate = "";
            }
            else
            {
                ViewBag.toDate = recommendationDetails.validity_date_to;
            }
            

            var permitProcDets = getWebPermitDataServices().ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);
            //check whether the process is finished or not. If it is, 
            //set ViewBag.editable to false so no fields can be edited
            for (int i = 0; i < permitProcDets.line_txt.Count; i++)
            {
                Debug.WriteLine(permitProcDets.line_type.ElementAt(i) + " " + permitProcDets.line_icon.ElementAt(i) + " " + permitProcDets.line_txt.ElementAt(i));

                //compare to 461 means check if it is check requirements
                if (permitProcDets.line_type.ElementAt(i) == "462")
                {
                    //editable is set to either "Y" for 'Yes step is complete therefore UNEDITABLE'
                    //or is set to "N" for 'No step is incomplete therefore EDITABLE'
                    ViewBag.editable = permitProcDets.line_icon.ElementAt(i);
                }
            }

            return View();
        }

        public String recommendProcess(String comments, String reject, String validfrom, String validTo, String permitID, String intrayID)
        {
            this.getProcesses(permitID);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;

          

            DateTime vFrom = DateTime.Parse(validfrom);

            DateTime vTo = DateTime.Parse(validTo);

            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();

            proc_recommend_dets rd= new proc_recommend_dets();
            rd.s_comments = comments;
            rd.s_reject_ind = reject;
            rd.s_office_guid = officeID;
            rd.s_officer_guid = officerID;
            rd.s_central_guid = centralID;
            rd.s_permit_guid = permitID;
            rd.s_intray_guid = intrayID;
            rd.s_valid_from = vFrom;
            rd.s_valid_to = vTo;


            var permitProcServices = getWebPermitProcServices();
            var updatedDets = permitProcServices.ws_upd_recommend_dets(user_id, password, rd);

            if (updatedDets.ws_status == 0)
                return "0";
            else
                return "1";

          
        }

        public JsonResult getMetRequirements(String permitID, String dependentID, String intrayID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;

            var checkItems = getWebPermitDataServices().ws_get_proc_check_items(user_id, password, permitID, intrayID, centralID, officeID, officerID);

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            if (checkItems.s_m_pcheck_guid.Count > 0)
            {
                for (int i = 0; i < checkItems.s_m_pcheck_guid.Count; i++)
                {
                    var keyValues = new Dictionary<string, string>
                   {
                       { "description", checkItems.s_m_check_desc[i]},
                       { "officerName", checkItems.s_m_officer_name[i]},
                       { "atPost", checkItems.s_m_at_post_ind[i]},
                       { "checkDate", checkItems.s_m_check_date[i].GetDateTimeFormats('D')[1]},
                       { "comments", checkItems.s_m_comments[i]}
                   };
                    data.Add(keyValues);
                }
            }
            else
            {
                var keyValues = new Dictionary<string, string>
                   {
                       { "description", "-"},
                       { "officerName", "-"},
                       { "atPost", "-"},
                       { "checkDate", "-"},
                       { "comments", "-"}
                   };
                data.Add(keyValues);
            }
            

            return Json(new { Result = "OK", Records = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getNotMetRequirements(String permitID, String dependentID, String intrayID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;

            var checkItems = getWebPermitDataServices().ws_get_proc_check_items(user_id, password, permitID, intrayID, centralID, officeID, officerID);

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            
            if (checkItems.s_n_pcheck_guid.Count > 0)
            {
                for (int i = 0; i < checkItems.s_n_pcheck_guid.Count; i++)
                {
                    var keyValues = new Dictionary<string, string>
                   {
                       { "description", checkItems.s_n_check_desc[i]},
                       { "officerName", "[webservice todo]"},
                       { "atPost", checkItems.s_n_at_post_ind[i]},
                       { "checkDate", checkItems.s_n_check_date[i].GetDateTimeFormats('D')[1]},
                       { "comments", checkItems.s_n_comments[i]}
                   };
                    data.Add(keyValues);
                }
            }
            else
            {
                var keyValues = new Dictionary<string, string>
                   {
                       { "description", "-"},
                       { "officerName", "-"},
                       { "atPost", "-"},
                       { "checkDate", "-"},
                       { "comments", "-"}
                   };
                data.Add(keyValues);
            }
            

            return Json(new { Result = "OK", Records = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult p461(String permitID, String dependentID, String intrayID, String modify)
        {
            this.getProcesses(permitID);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;

            var permitDets = getWebPermitDataServices().ws_get_permit_base_dets(user_id, password, permitID);

            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.guid = permitID;
            ViewBag.intrayID = intrayID;
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.dependentID = dependentID;
            ViewBag.officeList = getWebAdminServices().ws_get_offices(user_id, password);
            ViewBag.isModifiable = modify;

            var permitProcDets = getWebPermitDataServices().ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);

            //check whether the process is finished or not. If it is, 
            //set ViewBag.editable to false so no fields can be edited
            for (int i = 0; i < permitProcDets.line_txt.Count; i++)
            {
                Debug.WriteLine(permitProcDets.line_type.ElementAt(i) + " " + permitProcDets.line_icon.ElementAt(i) + " " + permitProcDets.line_txt.ElementAt(i));

                //compare to 461 means check if it is check requirements
                if (permitProcDets.line_type.ElementAt(i) == "461")
                {
                    //editable is set to either "Y" for 'Yes step is complete therefore UNEDITABLE'
                    //or is set to "N" for 'No step is incomplete therefore EDITABLE'
                    ViewBag.editable = permitProcDets.line_icon.ElementAt(i);
                }
            }
            var checkItems = getWebPermitDataServices().ws_get_proc_check_items(user_id, password, permitID, intrayID, centralID, officeID, officerID);

            ViewBag.checkItems = checkItems;

            return View();
        }

        public string processCheckRequirements(String permitID, String intrayID, String rejectInd, String comments)
        {
            this.getProcesses(permitID);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;



            var webpermitservices = getWebPermitDataServices();
            var checkItems = webpermitservices.ws_get_proc_check_items(user_id, password, permitID, intrayID, centralID, officeID, officerID);

            Debug.WriteLine(user_id);
            Debug.WriteLine(password);
            Debug.WriteLine(permitID);
            Debug.WriteLine(intrayID);
            Debug.WriteLine(centralID);
            Debug.WriteLine(officeID);
            Debug.WriteLine(officerID);
            Debug.WriteLine("comments are" + comments);
            Debug.WriteLine("comments are: " + checkItems.s_comments);
            Debug.WriteLine("central id is: " + centralID);
            Debug.WriteLine(checkItems.s_n_pcheck_guid.Count);


            //new object to store details and pass to web service
            proc_check_dets pcd = new proc_check_dets();

            //because there is a conflict between traveller.bms.permit.proc.arrayofString and traveller.bms.permit.data.arrayofString
            //each element for every array needs to be added individually as there is no apparent way to convert one to the other

            /*pcd.s_n_pcheck_guid.AddRange(checkItems.s_n_pcheck_guid);
            pcd.s_n_check_guid.AddRange(checkItems.s_n_check_guid);
            pcd.s_n_officer_guid.AddRange(checkItems.s_n_officer_guid);
            pcd.s_n_check_date.AddRange(checkItems.s_n_check_date);
            pcd.s_n_receipt_number.AddRange(checkItems.s_n_receipt_number);
            pcd.s_n_comments.AddRange(checkItems.s_n_comments);
            pcd.s_n_receipt_guid.AddRange(checkItems.s_m_receipt_guid);
            pcd.s_n_finalised_ind.AddRange(checkItems.s_n_finalised_ind);
            pcd.s_n_check_desc.AddRange(checkItems.s_n_check_desc);
            pcd.s_n_mandatory_ind.AddRange(checkItems.s_n_mandatory_ind);
            pcd.s_n_at_post_ind.AddRange(checkItems.s_n_at_post_ind);*/
            pcd.s_comments = comments;

            pcd.s_officer_guid = officerID;
            pcd.s_office_guid = officeID;
            pcd.s_central_guid = centralID;
            pcd.s_reject_ind = rejectInd;
            pcd.s_for_office_guid = "";
            pcd.s_permit_guid = permitID;
            pcd.s_intray_guid = intrayID;

            //finally, call web service
            var webpermitProcServices = getWebPermitProcServices();
            var updatedCheckItems = webpermitProcServices.ws_upd_proc_check_dets(user_id, password, pcd);



            return updatedCheckItems.ws_status.ToString();
        }

        public ActionResult p453(string permitID, string intrayID, string title, String modify)
        {
            this.getProcesses(permitID);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String centralID = Request.Cookies["centralID"].Value;

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);

            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.title = title.Split('-').ElementAt(0);
            ViewBag.processingOfficer = title.Split('-').ElementAt(1);
            ViewBag.officeList = getWebAdminServices().ws_get_offices(user_id, password);
            ViewBag.intrayID = intrayID;
            ViewBag.isModifiable = modify;
            return View();
        }

        public ActionResult p451(string id)
        {
            this.getProcesses(id);
            return View();
        }

        public ActionResult ApplicantDetails(String permitID, String dependentID, String modify)
        {
            this.getProcesses(permitID);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);

            ViewBag.guid = permitID;
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.applicantGuid = dependentID;
            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.modify = modify;

            var permitProcDets = webpermitservices.ws_get_proc_permit_dets(user_id, password, permitID, Request.Cookies["officeID"].Value);

            //check whether the process is finished or not. If it is, 
            //set ViewBag.editable to false so no fields can be edited
            for (int i = 0; i < permitProcDets.line_txt.Count; i++)
            {
                //compare to 456 means check if it is an applicant process
                //and need to break so other empty lines arent picked up
                if (permitProcDets.line_type.ElementAt(i) == "456")
                {
                    Debug.WriteLine(permitProcDets.line_type.ElementAt(i) + " " + permitProcDets.line_icon.ElementAt(i));

                    //editable is set to either "Y" for 'Yes step is complete therefore UNEDITABLE'
                    //or is set to "N" for 'No step is incomplete therefore EDITABLE'
                    ViewBag.editable = permitProcDets.line_icon.ElementAt(i);
                    break;
                }
                ViewBag.editable = "N";
            }

            //populate drop down boxes
            var webAdminServices = getWebAdminServices();

            var titles = webAdminServices.ws_get_specific_codes(user_id, password, "01"); //titles
            ViewBag.titles = new List<ArrayOfCommon_codes_details>();
            ViewBag.titles = titles.common_codes_dets;

            var ms = webAdminServices.ws_get_specific_codes(user_id, password, "08"); //marital status
            ViewBag.maritalstats = new List<ArrayOfCommon_codes_details>();
            ViewBag.maritalstats = ms.common_codes_dets;

            var nationalities = webAdminServices.ws_get_nationality(user_id, password);
            ViewBag.nationalities = nationalities.nationality_dets;

            var countriesOfBirth = webAdminServices.ws_get_countries(user_id, password);
            ViewBag.countriesOfBirth = countriesOfBirth.countries_dets;


            //code that fills in already present data into the form
            var applicantDetails = webpermitservices.ws_get_applicant_dets(user_id, password, dependentID, permitID);


            //data to be stored in hidden input fields that can be used in javascript
            ViewBag.permitID = permitID;
            //ViewBag.applicantGuid = applicantDetails.s_applicant_guid;
            ViewBag.officerID = applicantDetails.s_officer_guid;

            ViewBag.applicantTitle = applicantDetails.s_title_guid;
            ViewBag.applicantGivenName = applicantDetails.s_given_name;
            ViewBag.applicantSurName = applicantDetails.s_surname;
            ViewBag.applicantGender = applicantDetails.s_gender_code;
            ViewBag.applicantDob = applicantDetails.s_date_of_birth.ToString("dd-MMM-yyyy").Split(' ').ElementAt(0);
            ViewBag.applicantNationality = applicantDetails.s_nationality_code;
            ViewBag.applicantCOB = applicantDetails.s_country_of_birth;
            ViewBag.applicantPOB = applicantDetails.s_place_of_birth;
            ViewBag.applicantMaritalStatus = applicantDetails.s_marital_status;
            ViewBag.applicantPassportNumber = applicantDetails.s_passport_number;
            ViewBag.applicantExpiryDate = applicantDetails.s_pp_expiry.ToString("dd-MMM-yyyy").Split(' ').ElementAt(0);
            ViewBag.applicantWorkPermitNo = applicantDetails.s_work_permit_no;
            ViewBag.applicantPositionNumber = applicantDetails.s_position_no;
            ViewBag.applicantWorkPermExpiry = applicantDetails.s_wp_expiry.ToString().Split(' ').ElementAt(0);
            ViewBag.applicantWorkPermExpiry = applicantDetails.s_wp_expiry.ToString().Split(' ').ElementAt(0);
            ViewBag.applicantAddressLine1 = applicantDetails.s_address_1;
            ViewBag.applicantAddressLine2 = applicantDetails.s_address_2;
            ViewBag.applicantAddressLine3 = applicantDetails.s_address_3;
            ViewBag.applicantEmail = applicantDetails.s_email_addr;
            ViewBag.applicantPhoneNumber = applicantDetails.s_phone_no;
            ViewBag.applicantAddressInCountry = applicantDetails.s_addr_in_country;
            ViewBag.applicantOccupation = applicantDetails.s_occupation;
           

            //need to obtain associated guid of applicant's title
            for (int i = 0; i < titles.common_codes_dets.Count; i++)
            {
                if (titles.common_codes_dets.ElementAt(i).code_guid == applicantDetails.s_title_guid)
                    ViewBag.titleName = titles.common_codes_dets.ElementAt(i).code_desc;
            }

            //need to obtain associated marital status name from the guid thats returned from webservice
            for (int i = 0; i < ms.common_codes_dets.Count; i++)
            {
                if (ms.common_codes_dets.ElementAt(i).code_guid == applicantDetails.s_marital_status)
                {
                    ViewBag.applicantMaritalStatusName = ms.common_codes_dets.ElementAt(i).code_desc;
                    break;
                }
            }

            //need to obtain associated nationality name from the country guid thats returned from the webservice
            for (int i = 0; i < nationalities.nationality_dets.Count; i++)
            {
                if (nationalities.nationality_dets.ElementAt(i).nationality_code == applicantDetails.s_nationality_code)
                {
                    ViewBag.applicantNationalityName = nationalities.nationality_dets.ElementAt(i).nationality_desc;
                    break;
                }

            }

            //need to obtain associated country name form the country guid thats returned from the webservice
            for (int i = 0; i < countriesOfBirth.countries_dets.Count; i++)
            {
                if (countriesOfBirth.countries_dets.ElementAt(i).country_code == applicantDetails.s_country_of_birth)
                {
                    ViewBag.cobName = countriesOfBirth.countries_dets.ElementAt(i).country_name;
                    break;
                }
            }
            //permitDets.

            //Code to convert and display image
            if (applicantDetails.s_pic_txt.Count() > 0)
            {
                ViewBag.imageID = applicantDetails.s_image_guid;
                String filepath = "C:\\inetpub\\wwwroot\\Merit.BMS.Traveller" + "\\pdf\\" + applicantDetails.s_image_guid+ ".jpg";

                using (Image image = Image.FromStream(new MemoryStream(Convert.FromBase64String(applicantDetails.s_pic_txt))))
                {
                    image.Save(filepath, ImageFormat.Jpeg);
                }
            }
            ViewBag.s_image_guid = applicantDetails.s_image_guid;
            

            return View();
        }

        public ActionResult p999(String permitID, String sponsorID, String modify)
        {
            this.getProcesses(permitID);
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);

            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.guid = permitID;
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.modify = modify;


            //populate drop down boxes
            var webAdminServices = getWebAdminServices();
            var titles = webAdminServices.ws_get_specific_codes(user_id, password, "01"); //titles
            ViewBag.titles = new List<ArrayOfCommon_codes_details>();
            ViewBag.titles = titles.common_codes_dets;

            var statuses = webAdminServices.ws_get_specific_codes(user_id, password, "10");
            ViewBag.statuses = new List<ArrayOfCommon_codes_details>();
            ViewBag.statuses = statuses.common_codes_dets;

            //get sponsor details to auto populate fields
            var sponsorDets = webpermitservices.ws_get_single_sponsor(user_id, password, sponsorID);
            ViewBag.company = sponsorDets.s_company_name;
            ViewBag.titleID = sponsorDets.s_title_guid;
            ViewBag.surname = sponsorDets.s_surname;
            ViewBag.givenName = sponsorDets.s_given_name;
            ViewBag.position = sponsorDets.s_principal_position;
            ViewBag.passportNum = sponsorDets.s_passport_number;
            ViewBag.email = sponsorDets.s_email_addr;
            ViewBag.lAdd1 = sponsorDets.s_address_1;
            ViewBag.lAdd2 = sponsorDets.s_address_2;
            ViewBag.lAdd3 = sponsorDets.s_address_3;
            ViewBag.pNo = sponsorDets.s_phone_no;
            ViewBag.fax = sponsorDets.s_fax_no;
            ViewBag.pAdd1 = sponsorDets.s_postal_addr_1;
            ViewBag.pAdd2 = sponsorDets.s_postal_addr_2;
            ViewBag.pAdd3 = sponsorDets.s_postal_addr_3;
            ViewBag.status = sponsorDets.s_status_guid;
            ViewBag.active = sponsorDets.s_active_ind;
            ViewBag.verified = sponsorDets.s_verified_ind;
            ViewBag.ProjectEnabled = sponsorDets.s_proj_enabled;
            ViewBag.sponsorID = sponsorDets.s_sponsor_guid;
            ViewBag.applicantID = sponsorDets.s_applicant_guid;

            //need to obtain associated guid of sponsor's title
            for (int i = 0; i < titles.common_codes_dets.Count; i++)
            {
                if (titles.common_codes_dets.ElementAt(i).code_guid == sponsorDets.s_title_guid)
                    ViewBag.titleName = titles.common_codes_dets.ElementAt(i).code_desc;
            }

            //obtains status name for status sponsor's status id
            for (int i = 0; i < statuses.common_codes_dets.Count; i++)
            {
                if (statuses.common_codes_dets.ElementAt(i).code_guid == sponsorDets.s_status_guid)
                    ViewBag.statusName = statuses.common_codes_dets.ElementAt(i).code_desc;
            }



            return View();
        }
        public JsonResult getSponsorDetails(String sponsorID)
        {

            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
 
            var sponsorDets = getWebPermitDataServices().ws_get_single_sponsor(user_id, password, sponsorID);

            Dictionary<string, string> sponsorDetails = new Dictionary<string, string>
                   {
                       { "company", sponsorDets.s_company_name },
                       { "title", sponsorDets.s_title_guid },
                       { "surname", sponsorDets.s_surname },
                       { "name", sponsorDets.s_given_name },
                       { "position", sponsorDets.s_principal_position },
                       { "passportno", sponsorDets.s_passport_number },
                       { "emailAddress", sponsorDets.s_email_addr },
                       { "add1", sponsorDets.s_address_1 },
                       { "add2", sponsorDets.s_address_2 },
                       { "add3", sponsorDets.s_address_3 },
                       { "phone", sponsorDets.s_phone_no },
                       { "fax", sponsorDets.s_fax_no },
                       { "padd1", sponsorDets.s_postal_addr_1 },
                       { "padd2", sponsorDets.s_postal_addr_2 },
                       { "padd3", sponsorDets.s_postal_addr_3 },
                       { "status", sponsorDets.s_status_guid },
                       { "projenabled", sponsorDets.s_proj_enabled },
                       { "sponsorguid", sponsorDets.s_sponsor_guid },
                       { "active", sponsorDets.s_active_ind},
                       { "verified", sponsorDets.s_verified_ind},
                       { "applicantguid", sponsorDets.s_applicant_guid },
                       {"sponsorID",sponsorDets.s_sponsor_guid}
                    
                   };

            return Json(new { Result = "OK", records = sponsorDetails }, JsonRequestBehavior.AllowGet);
        }
        

        public String updateSponsorDetails(String sponsorGuid, String companyName, String lAdd1,
                                           String lAdd2, String lAdd3, String phoneNo, String faxNo,
                                           String email, String pAdd1, String pAdd2, String pAdd3,
                                           String position, String titleID, String surname, String givenName,
                                           String passportNo, String statusID, String activeInd, String verifiedInd,
                                           String projEnabled, String permitID, String applicantID, String officerID,
                                           String completeInd)
        {

            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            officerID = Request.Cookies["officerID"].Value;

            //ambiguous reference for sponsor dets. need to specify correct package
            BMS.BMS_Web_Permit_Proc.sponsor_dets updatedDets = new BMS.BMS_Web_Permit_Proc.sponsor_dets();

            updatedDets.s_sponsor_guid = sponsorGuid;
            updatedDets.s_company_name = companyName;
            updatedDets.s_address_1 = lAdd1;
            updatedDets.s_address_2 = lAdd2;
            updatedDets.s_address_3 = lAdd3;
            updatedDets.s_phone_no = phoneNo;
            updatedDets.s_principal_position = position;
            updatedDets.s_fax_no = faxNo;
            updatedDets.s_email_addr = email;
            updatedDets.s_postal_addr_1 = pAdd1;
            updatedDets.s_postal_addr_2 = pAdd2;
            updatedDets.s_postal_addr_3 = pAdd3;
            updatedDets.s_title_guid = titleID;
            updatedDets.s_surname = surname;
            updatedDets.s_given_name = givenName;
            updatedDets.s_passport_number = passportNo;
            updatedDets.s_status_guid = statusID;
            updatedDets.s_active_ind = activeInd;
            updatedDets.s_verified_ind = verifiedInd;
            updatedDets.s_proj_enabled = projEnabled;
            updatedDets.s_permit_guid = permitID;
            updatedDets.s_applicant_guid = applicantID;
            updatedDets.s_officer_guid = officerID;
            updatedDets.s_complete_ind = completeInd;

            var procServices = getWebPermitProcServices();
            var details = procServices.ws_upd_sponsor_dets(user_id, password, updatedDets);

            if (details.ws_status == 0)
                return "0";
            else
                return "1";

           
        }

        [HttpPost]
        public JsonResult updateApplicantDetails(String permitID, String fileNo, String permitType,
                                                 String dateLodged, String whereLodged, String permitPeriod,
                                                 String entriesAllowed, String dependents, String express,
                                                 String feeAmount, String paidAt, String paidOn,
                                                 String receiptNo, String poffshore, String sReturn,
                                                 String offShore, String title, String surname,
                                                 String givenName, String nationality, String cob,
                                                 String pob, String occupation, String gender,
                                                 String dob, String maritalStatus, String passportNo,
                                                 String add1, String add2, String add3,
                                                 String email, String phone_no, String docExpiry,
                                                 String positionNo, String appWorkPermExpiry, String addrInCountry,
                                                 String workPermNo, String applicantID, String officerID,
                                                 String s_complete_ind, String imageguid)
        {
            //MemoryStream target1 = new MemoryStream();


            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            officerID = Request.Cookies["officerID"].Value;

            //ambiguous reference for sponsor dets. need to specify correct package
            BMS.BMS_Web_Permit_Proc.applicant_dets updatedDets = new BMS.BMS_Web_Permit_Proc.applicant_dets();


      

            //applicant details
            updatedDets.s_title_guid = title;
            updatedDets.s_surname = surname;
            updatedDets.s_given_name = givenName;
            updatedDets.s_gender_code = gender;
            updatedDets.s_date_of_birth = Convert.ToDateTime(dob); 
            updatedDets.s_occupation = occupation;
            updatedDets.s_nationality_code = nationality;
            updatedDets.s_country_of_birth = cob;
            updatedDets.s_place_of_birth = pob;
            updatedDets.s_marital_status = maritalStatus;
            updatedDets.s_passport_number = passportNo;
            updatedDets.s_pp_expiry = Convert.ToDateTime(docExpiry); 
            updatedDets.s_position_no = positionNo;
            updatedDets.s_image_guid = imageguid;

            //work permit details
            updatedDets.s_work_permit_no = Convert.ToInt32(workPermNo);
            updatedDets.s_position_no = positionNo;
            updatedDets.s_wp_expiry = Convert.ToDateTime(appWorkPermExpiry); 

            //address Details and Photo
            updatedDets.s_address_1 = add1;
            updatedDets.s_address_2 = add2;
            updatedDets.s_address_3 = add3;
            updatedDets.s_phone_no = phone_no;
            updatedDets.s_email_addr = email;
            updatedDets.s_permit_guid = permitID;
            updatedDets.s_applicant_guid = applicantID;
            updatedDets.s_addr_in_country = addrInCountry;
            updatedDets.s_officer_guid = officerID;
            updatedDets.s_complete_ind = s_complete_ind;

            String filename = "uploadedFile.jpg";
            String filepath = "";

            if (!String.IsNullOrEmpty(Response.Cookies["documentPictureID"].Value))
            {
                updatedDets.s_image_guid = Response.Cookies["documentPictureID"].Value;
            }

            if (Request.Files.Count > 0)
            {

                filepath = "C:\\inetpub\\wwwroot\\Merit.BMS.Traveller" + "\\pdf\\" + filename;
                Request.Files[0].SaveAs(filepath);

                MemoryStream target1 = new MemoryStream();
                byte[] contents;

                //convert image to byte array and attach to updatedDets object
                Request.Files[0].InputStream.CopyTo(target1);
                contents = target1.ToArray();
                updatedDets.s_picture = contents;
               
            }

            //Debug.WriteLine("uploaded file in bytes:" + updatedDets.s_picture.Count());

            var procServices = getWebPermitProcServices();
            var details = procServices.ws_upd_applicant_dets(user_id, password, updatedDets);

            if(details.ws_status == 0)
                 return Json(new { Result = "0", imagepath = filename });
            else
                return Json(new { Result = "1", imagepath = filename });
        }

        public String makeDecision(String intray_guid, String decision_ind, String from_date, String to_date, String comment_txt )
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = getPassword();
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String central_guid = Request.Cookies["centralID"].Value;

            var result = getWebPermitProcServices().ws_upd_proc_decision_dets(user_id, password, intray_guid, central_guid, decision_ind, from_date, to_date, comment_txt);
            return result.ws_status.ToString();
        }
        public JsonResult getSponsorsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string userInput = "")
        {

            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var WebAdminWebServices = getWebAdminServices();
            sponsor_list sponsors = WebAdminWebServices.ws_get_all_sponsors(user_id, password, userInput);

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            int iterations = 0;

            if (sponsors.sponsor_dets.Count() > 0)
            {
                for (int i = jtStartIndex; i < sponsors.sponsor_dets.Count(); i++)
                {

                    var keyValues = new Dictionary<string, string>
                   {
                       { "guID", sponsors.sponsor_dets.ElementAt(i).sponsor_guid },
                       { "companyName", sponsors.sponsor_dets.ElementAt(i).company_name},
                       { "address", sponsors.sponsor_dets.ElementAt(i).address_1 },
                       { "address2", sponsors.sponsor_dets.ElementAt(i).address_2 },
                       { "address3", sponsors.sponsor_dets.ElementAt(i).address_3 },
                       { "email_addr", sponsors.sponsor_dets.ElementAt(i).email_addr},
                       { "fax_no", sponsors.sponsor_dets.ElementAt(i).fax_no },
                       { "given_name", sponsors.sponsor_dets.ElementAt(i).given_name},
                       { "phone_no", sponsors.sponsor_dets.ElementAt(i).phone_no },
                       { "postal_addr_1", sponsors.sponsor_dets.ElementAt(i).postal_addr_1},
                       { "postal_addr_2", sponsors.sponsor_dets.ElementAt(i).postal_addr_2},
                       { "postal_addr_3", sponsors.sponsor_dets.ElementAt(i).postal_addr_3},
                       { "principal_position", sponsors.sponsor_dets.ElementAt(i).principal_position},
                       { "surname", sponsors.sponsor_dets.ElementAt(i).surname},
                       { "title_description", sponsors.sponsor_dets.ElementAt(i).title_description},
                       { "passport_number", sponsors.sponsor_dets.ElementAt(i).passport_number},
                       { "status_name", sponsors.sponsor_dets.ElementAt(i).status_description},
                       { "title_guid",sponsors.sponsor_dets.ElementAt(i).title_guid},
                   };
                    data.Add(keyValues);

                    iterations++;
                }
            }
            else
            {
                var keyValues = new Dictionary<string, string>
                   {
                       { "guID", "-" },
                       { "companyName", "-"},
                       { "address", "-" },
                       { "address2", "-" },
                       { "address3", "-" },
                       { "email_addr", "-"},
                       { "fax_no", "-" },
                       { "given_name", "-"},
                       { "phone_no", "-" },
                       { "postal_addr_1", "-"},
                       { "postal_addr_2", "-"},
                       { "postal_addr_3", "-"},
                       { "principal_position", "-"},
                       { "surname", "-"},
                       { "title_description", "-"},
                       { "passport_number", "-"},
                       { "status_name", "-"},
                       { "title_guid","-"},
                   };
                data.Add(keyValues);
            }

            
            //add blank rows if less than 13
            /*if (sponsors.sponsor_dets.Count() < 13)
            {
                for (int i = jtStartIndex; i < (13 - sponsors.sponsor_dets.Count()); i++)
                {
                    var keyValues = new Dictionary<string, string>
                   {
                       { "guID", " " },
                       { "companyName", "-"},
                       { "address", " "},
                       { "address2", " "},
                       { "address3", " "},
                       { "email_addr", " "},
                       { "fax_no", " " },
                       { "given_name"," "},
                       { "phone_no", " " },
                       { "postal_addr_1", " "},
                       { "postal_addr_2", " "},
                       { "postal_addr_3", " "},
                       { "principal_position", " "},
                       { "surname", " "},
                       { "title_description", " "},
                       { "passport_number", " "},
                       { "status_name", " "},
                     
                   };
                    data.Add(keyValues);
                }
            }*/

            try
            {
                return Json(new { Result = "OK", Records = data, TotalRecordCount = sponsors.sponsor_dets.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public String createLetter(String permit_guid, String intray_guid, String comment_txt)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String officeID = Request.Cookies["officeID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            String central_guid = Request.Cookies["centralID"].Value;

            //create pdf
            String installationDirectory = System.Configuration.ConfigurationManager.AppSettings["InstallationDirectory"].ToString();
            PdfDocument custodyReceipt = PdfReader.Open(installationDirectory + "/doc/Rejection Letter1 - Visa Branch.pdf");
            XGraphics gfx = XGraphics.FromPdfPage(custodyReceipt.Pages[0]);
            XFont boldfont = new XFont("Verdana", 10, XFontStyle.Bold);
            XFont font = new XFont("Times New Roman", 14, XFontStyle.Regular);


            var letterDets = getWebPermitProcServices().ws_print_reject_letter(user_id, password, permit_guid, officeID, intray_guid, comment_txt, central_guid);

            custodyReceipt.Info.Title = "Letter";
            //width height
            gfx.DrawString(letterDets.when_lodged.ToString(), font, XBrushes.Black, 389, 180, XStringFormats.Default);
            gfx.DrawString(letterDets.varn_ref, font, XBrushes.Black, 389, 194, XStringFormats.Default);
            gfx.DrawString(letterDets.sponsor_name_full, font, XBrushes.Black, 56, 201, XStringFormats.Default);
            gfx.DrawString(letterDets.sponsor_name, font, XBrushes.Black, 56, 214, XStringFormats.Default);
            gfx.DrawString(letterDets.sponsor_principal, font, XBrushes.Black, 56, 227, XStringFormats.Default);
            gfx.DrawString(letterDets.sponsor_post1, font, XBrushes.Black, 56, 240, XStringFormats.Default);
            gfx.DrawString(letterDets.sponsor_post2, font, XBrushes.Black, 56, 253, XStringFormats.Default);
            gfx.DrawString(letterDets.sponsor_post3, font, XBrushes.Black, 56, 266, XStringFormats.Default);
            gfx.DrawString(letterDets.applicant_title + " " + letterDets.applicant_given + " " + letterDets.applicant_surname, font, XBrushes.Black, 99, 306, XStringFormats.Default);
            gfx.DrawString(letterDets.permit_class + " " + letterDets.permit_type, font, XBrushes.Black, 219, 337, XStringFormats.Default);
            gfx.DrawString(letterDets.applicant_given + " " + letterDets.applicant_surname, font, XBrushes.Black, 120, 370, XStringFormats.Default);
            gfx.DrawString(letterDets.when_lodged.ToString(), font, XBrushes.Black, 81, 418, XStringFormats.Default);
            gfx.DrawString(letterDets.reject_reason, font, XBrushes.Black, 56, 512, XStringFormats.Default);
            gfx.DrawString(letterDets.mission_name + " " + letterDets.mission_city + " " + letterDets.mission_country, font, XBrushes.Black, 75, 727, XStringFormats.Default);

            custodyReceipt.Info.Title = "Letter";

            String filename = "C:\\inetpub\\wwwroot\\Merit.BMS.Traveller" + "\\pdf\\letter" + permit_guid + ".pdf";
            custodyReceipt.Save(filename);
            return "";
        }
        public ActionResult pDocument(String permitID, String intrayID, String modify)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String officeID = Request.Cookies["officeID"].Value;

            var webpermitservices = getWebPermitDataServices();
            var permitDets = webpermitservices.ws_get_permit_base_dets(user_id, password, permitID);

            ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();
            ViewBag.guid = permitID;
            ViewBag.varnRef = permitDets.s_varn_ref;
            ViewBag.dateLodged = permitDets.s_lodged_date;
            ViewBag.lodgedDate = permitDets.s_lodged_at;
            ViewBag.permitType = permitDets.s_permit_type;
            ViewBag.permitClass = permitDets.s_permit_class;
            ViewBag.permitNo = permitDets.s_permit_no;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.applicant = permitDets.s_applicant;
            ViewBag.dob = permitDets.s_app_dob;
            ViewBag.nationality = permitDets.s_app_nation;
            ViewBag.approvalDate = permitDets.s_decide_date;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.outcome = permitDets.s_outcome;
            ViewBag.decidedDate = permitDets.s_decide_date;
            ViewBag.permit_guid = permitDets.s_permit_guid;
            ViewBag.finalised = permitDets.s_finalised;
            ViewBag.isModifiable = modify;
            ViewBag.permitID = permitID;

            var vdoc = webpermitservices.ws_get_proc_vdoc_dets(user_id, password, permitID, officeID);
            ViewBag.vdoctype = vdoc.document_type;
            ViewBag.vdocno = vdoc.document_no;
            ViewBag.vdocNationality = vdoc.nationality;
            ViewBag.vdocexpiry = vdoc.expiry_date;
            ViewBag.vdocdescription = vdoc.description;
            ViewBag.vdocsurname = vdoc.surname;
            ViewBag.vdocgender = vdoc.gender;
            ViewBag.vdocgivenname = vdoc.given_name;
            ViewBag.dob = vdoc.date_of_birth;

    //vdoc.do

            ViewBag.permitID = permitID;
            return View();
        }
        public String processReturneDocument(String permitID, String returnedDate, String comments, String documentNo)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var result = getWebPermitProcServices().ws_upd_vdocs(user_id, password, permitID, documentNo,comments,returnedDate);

            if (result.ws_status == 0)
                return "success";
            else
                return result.ws_message;
        }

    }
}
