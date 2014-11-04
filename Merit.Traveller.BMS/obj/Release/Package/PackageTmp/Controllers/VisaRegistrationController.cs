using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Merit.Traveller.BMS.BMS_Web_Permit_Proc;
using Merit.Traveller.BMS.BMS_Web_Permit_Data;
using Merit.Traveller.BMS.BMS_Web_Admin;
using System.ServiceModel;
using System.Diagnostics;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Merit.Traveller.BMS.Controllers
{


    public class VisaRegistrationController : Controller
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

        //
        // GET: /VisaRegistration/
        [Authorize]
        public ActionResult Index()
        {
            //check if the cookie has expired before continuing
            if (Request.Cookies["user_id"] != null)
            {
                //add 60 minutes to cookie expiry dates
                refreshCookies();

                String user_id = Request.Cookies["user_id"].Value;
                String password = Request.Cookies["password"].Value;

                ViewBag.webAppAlias = System.Configuration.ConfigurationManager.AppSettings["WebAppAlias"].ToString();

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
             
                return View();
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }
        public JsonResult getProjectsList(String visaType, String visaClass)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            var WebAdminWebServices = getWebAdminServices();
            common_codes_list projects = WebAdminWebServices.ws_get_projects(user_id, password, visaType, visaClass);
            return Json(new { Result = "OK", Records = projects }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getCountriesList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            Debug.WriteLine("come here");
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            var countries = getWebAdminServices().ws_get_nationality(user_id, password);

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            int iterations = 0;

            for (int i = jtStartIndex; i < countries.nationality_dets.Count(); i++)
            {

                if (countries.nationality_dets.ElementAt(i).favourite==true)
                {
                    var keyValues = new Dictionary<string, string>
                   {
                       { "name", countries.nationality_dets.ElementAt(i).nationality_desc},
                       { "code", countries.nationality_dets.ElementAt(i).nationality_code}
                   };
                    data.Add(keyValues);
                }
                iterations++;

                //use this if paging is enabled (default is 10):
                /*if (iterations == 10)
                    break;*/
            }

            try
            {
                return Json(new { Result = "OK", Records = data, TotalRecordCount = countries.nationality_dets.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult getSponsorsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string userInput="")
        {

            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var WebAdminWebServices = getWebAdminServices();
            sponsor_list sponsors = WebAdminWebServices.ws_get_all_sponsors(user_id, password, userInput);

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            int iterations = 0;

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
                     
                   };
                data.Add(keyValues);

                iterations++;
            }
            //add blank rows if less than 13
            /*if (sponsors.sponsor_dets.Count() < 13)
            {
                for (int i = jtStartIndex; i < (13-sponsors.sponsor_dets.Count()); i++)
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

        public String getFilteredClasses(String visaId)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var WebAdminWebServices = getWebAdminServices();
            common_codes_list permitClasses = WebAdminWebServices.ws_get_ptypes(user_id, password);

            ArrayList list = new ArrayList();

            for (int i = 0; i < permitClasses.common_codes_dets.Count; i++)
            {
                if(visaId.CompareTo(permitClasses.common_codes_dets[i].code_type)==0)
                    list.Add(permitClasses.common_codes_dets[i].code_desc + "|" + permitClasses.common_codes_dets[i].code_guid);
            }

            var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jsonSerializer.Serialize(list);

            return json;

        }

        public string changeVisaDetails(String guid)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var WebAdminWebServices = getWebAdminServices();
            permit_class_list pcList = WebAdminWebServices.ws_get_pclass_det(user_id, password, guid);

            ArrayList list = new ArrayList();

            //conditions are in first element only
            list.Add(pcList.permit_class_dets.ElementAt(0).reg_conditions + pcList.permit_class_dets.ElementAt(0).fee_amount + "|" + pcList.permit_class_dets.ElementAt(0).requires_sponsor);
            //pcList.permit_class_dets.ElementAt(0).
            String sponsorRequired = pcList.permit_class_dets.ElementAt(0).requires_sponsor;

            var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jsonSerializer.Serialize(list);

            return json;
        }

        public int checkReceiptNo(String receiptNo)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var WebAdminWebServices = getWebAdminServices();
            receipt_no_details receiptDetails = WebAdminWebServices.ws_chk_receipt_no(user_id, password, receiptNo,"");

            return receiptDetails.entry_count;

        }
        public string checkDate(String date, String type)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            String returnDate = getWebPermitDataServices().ws_check_date(user_id, password, type, date).return_message ;
            Debug.WriteLine(returnDate);
            return returnDate;
        }

        public JsonResult getBioDataDetails(String docType, String nationality, String docNum)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var webpermitdataservices = getWebPermitDataServices();
            biodata_details bdd = webpermitdataservices.ws_get_biodata_details(user_id, password, docType, nationality, docNum);

            //if we didnt find anything
            if (bdd.st_given == "")            
                return Json(new { Result = "notfound" }, JsonRequestBehavior.AllowGet);


            //if we found a document, check now if there is an image
            if (bdd.st_remarks != "")
            {

                byte[] decodedImage = Convert.FromBase64String(bdd.st_remarks);

                String filepath = "C:\\inetpub\\wwwroot\\Merit.BMS.Traveller" + "\\pdf\\" + bdd.st_picture_guid + ".jpg";

                using (Image image = Image.FromStream(new MemoryStream(decodedImage)))
                {
                    image.Save(filepath, ImageFormat.Jpeg);
                }
                //Response.Cookies["documentPictureID"].Value = bdd.st_image_guid;
            }
            
            return Json(new
            {
                Result = "found",
                docnum = bdd.st_doc_number,
                imgguid = bdd.st_picture_guid,
                givenName = bdd.st_given,
                surname = bdd.st_surname,
                dob = bdd.st_dob.ToString("dd-MMM-yyyy").Split(' ')[0],
                expirydate = bdd.st_expiration.ToString("dd-MMM-yyyy").Split(' ')[0],
                sex = bdd.st_sex,
                docNum = bdd.st_doc_number
            }, JsonRequestBehavior.AllowGet);
 
        }

        public char checkBioDataString(String biodata)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var webpermitdataservices = getWebPermitDataServices();
            
            //action_return is ambiguous. need to specify package
            Merit.Traveller.BMS.BMS_Web_Permit_Data.action_return bds = webpermitdataservices.ws_chk_biodata_string(user_id, password, biodata);
            

            return bds.return_message.ElementAt(0);
        }

        public String getSponsors(String userInput)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var WebAdminWebServices = getWebAdminServices();
            sponsor_list sponsors = WebAdminWebServices.ws_get_all_sponsors(user_id, password, userInput);

            List<String> sponsorsList = new List<string>();

            for (int i = 0; i < sponsors.sponsor_dets.Count; i++)
            {
                String sponsorDetail =
                sponsors.sponsor_dets.ElementAt(i).company_name + "|" +
                sponsors.sponsor_dets.ElementAt(i).address_1 + "|" +
                sponsors.sponsor_dets.ElementAt(i).sponsor_guid;

                sponsorsList.Add(sponsorDetail);

            }

            var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jsonSerializer.Serialize(sponsorsList);


            return json;

        }

        public JsonResult getPrimeEmployerDetails(String projectID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var WebAdminWebServices = getWebAdminServices();
            common_codes_list ccl = WebAdminWebServices.ws_get_pemps(user_id, password, projectID);


            Debug.WriteLine("this is a message"+ccl.ws_message);

            return Json(new { Result = "OK", Records = ccl }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getHeadEmployerDetails(String projectID, String primeID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            common_codes_list ccl = getWebAdminServices().ws_get_hemps(user_id, password, projectID, primeID);

            return Json(new { Result = "OK", Records = ccl }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getProjectUndertakings(String ptype, String pclass, String projectID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            checklist_list undertakings = getWebAdminServices().ws_get_undertakings(user_id, password, projectID, ptype, pclass);
            
            return Json(new { Result = "OK", Records = undertakings, Number=undertakings.checklist_dets.Count() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSubEmployerDetails(String projectID, String primeID, String headID)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;

            var WebAdminWebServices = getWebAdminServices();
            common_codes_list ccl = WebAdminWebServices.ws_get_semps(user_id, password, projectID, primeID, headID);

            return Json(new { Result = "OK", Records = ccl }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult registerApplication(String pTypeID, String pClassID, String receiptNo,
                                          String docType, String nationID, String docNo,
                                          String expiryDate, String surname, String givenName,
                                          String genderCode, String dob, String sponsorGuid,
                                          String projectID, String comments, String primeID,
                                          String headID, String subID, String[] checksIDs,
                                          String curPerGuid, String guarantee, String withReceipt,
                                          String sponsorName, String imgguid)
        {
            String user_id = Request.Cookies["user_id"].Value;
            String password = Request.Cookies["password"].Value;
            String officeID = Request.Cookies["officeID"].Value;
            String centralID = Request.Cookies["centralID"].Value;
            String officerID = Request.Cookies["officerID"].Value;
            DateTime dateLodged = DateTime.Now;
            
            var webpermitprocServices = getWebPermitProcServices();

            new_visa_inp newVisaInput = new new_visa_inp();
            newVisaInput.ws_user_id = user_id;
            newVisaInput.ws_password = password;
            newVisaInput.s_office_guid = officeID;
            newVisaInput.s_officer_guid = officerID;
            newVisaInput.s_date_lodged = dateLodged;
            newVisaInput.s_ptype_guid = pTypeID;
            newVisaInput.s_pclass_guid = pClassID;
            newVisaInput.s_receipt_no = receiptNo;
            newVisaInput.s_doc_type = docType;
            newVisaInput.s_doc_no = docNo.ToUpper();
            newVisaInput.s_nation_guid = nationID;
            newVisaInput.s_expiry_date = Convert.ToDateTime(expiryDate);
            newVisaInput.s_surname = surname;
            newVisaInput.s_given_name = givenName;
            newVisaInput.s_gender_code = genderCode;
            newVisaInput.s_dob = Convert.ToDateTime(dob);
            newVisaInput.s_sponsor_guid = sponsorGuid;
            newVisaInput.s_project_guid = projectID;
            newVisaInput.s_comments = comments;
            newVisaInput.s_prime_guid = primeID;
            newVisaInput.s_head_guid = headID;
            newVisaInput.s_sub_guid = subID;
            newVisaInput.s_central_guid = centralID;
            newVisaInput.s_image_guid = imgguid;
            

            Merit.Traveller.BMS.BMS_Web_Permit_Proc.ArrayOfString chkguid = new Merit.Traveller.BMS.BMS_Web_Permit_Proc.ArrayOfString();

            if (checksIDs != null)
            {
                for (int i = 0; i < checksIDs.Count(); i++)
                {
                    chkguid.Add(checksIDs[i]);
                }
            }

            newVisaInput.s_checks_guid = chkguid;
            newVisaInput.s_current_permit_guid = curPerGuid;
            newVisaInput.s_guarantee = guarantee;

            new_visa_ret returnedVisa = webpermitprocServices.ws_new_visa_reg(newVisaInput);

            String fileName = "none";

            if (returnedVisa.ws_status == 0)
            {
                if (withReceipt.Equals("y"))
                {
                    //create pdf receipt
                    String installationDirectory = System.Configuration.ConfigurationManager.AppSettings["InstallationDirectory"].ToString();
                    PdfDocument custodyReceipt = PdfReader.Open(installationDirectory+"/doc/pngcustreceipt.pdf");
                    XGraphics gfx = XGraphics.FromPdfPage(custodyReceipt.Pages[0]);
                    XFont boldfont = new XFont("Verdana", 10, XFontStyle.Bold);
                    XFont font = new XFont("Verdana", 10, XFontStyle.Regular);

                    //draw applicant name
                    gfx.DrawString(givenName + " " + surname, boldfont, XBrushes.Black, 185, 208, XStringFormats.Default);

                    //draw document number (passport number)
                    gfx.DrawString(docNo, boldfont, XBrushes.Black, 185, 220, XStringFormats.Default);

                    nationality_list nations = getWebAdminServices().ws_get_nationality(user_id, password);
                    String nationName = "";
                    for (int i = 0; i < nations.nationality_dets.Count(); i++)
                    {
                        if (nations.nationality_dets.ElementAt(i).nationality_code == nationID)
                            nationName = nations.nationality_dets.ElementAt(i).nationality_desc;
                    }

                    //draw nationality
                    gfx.DrawString(nationName, boldfont, XBrushes.Black, 382, 220, XStringFormats.Default);

                    //draw "lodged at" variable
                    gfx.DrawString(Request.Cookies["centralName"].Value, boldfont, XBrushes.Black, 185, 232, XStringFormats.Default);

                    //draw "date lodged " variable
                    gfx.DrawString(dateLodged.ToString("dd-MMM-yyyy"), boldfont, XBrushes.Black, 382, 232, XStringFormats.Default);

                    //draw receipt no
                    gfx.DrawString(receiptNo, boldfont, XBrushes.Black, 185, 244, XStringFormats.Default);

                    //draw fee
                    permit_class_list pcList = getWebAdminServices().ws_get_pclass_det(user_id, password, pClassID);
                    gfx.DrawString(pcList.permit_class_dets.ElementAt(0).fee_amount+"PGK", boldfont, XBrushes.Black, 382, 244, XStringFormats.Default);

                    //draw sponsor
                    if (String.IsNullOrEmpty(sponsorName))
                        sponsorName = "-";

                    gfx.DrawString(sponsorName, boldfont, XBrushes.Black, 185, 256, XStringFormats.Default);

                    common_codes_list pTypesList = getWebAdminServices().ws_get_pclasses(user_id, password);
                    common_codes_list permitClasses = getWebAdminServices().ws_get_ptypes(user_id, password);

                    String visaTypeName = "";
                    String visaClassName = "";
                    for (int i = 0; i < pTypesList.common_codes_dets.Count(); i++)
                    {
                        if (pTypesList.common_codes_dets.ElementAt(i).code_guid.Equals(pTypeID))
                        {
                            visaTypeName = pTypesList.common_codes_dets.ElementAt(i).code_desc;
                        }
                    }
                    for (int i = 0; i < permitClasses.common_codes_dets.Count(); i++)
                    {
                        if (permitClasses.common_codes_dets.ElementAt(i).code_guid.Equals(pClassID))
                        {
                            visaClassName = permitClasses.common_codes_dets.ElementAt(i).code_desc;
                        }
                    }

                    //draw visa Type
                    gfx.DrawString(visaTypeName + " / " + visaClassName, boldfont, XBrushes.Black, 185, 268, XStringFormats.Default);

                    //draw varn
                    gfx.DrawString(returnedVisa.st_varn_ref.ToString(), boldfont, XBrushes.Black, 260, 310, XStringFormats.Default);

                    //draw finalised date

                    //web service doesnt return finalised date yet
                    //if finalised date is null or empty, finalised date = datelodged+ 28 days

                    gfx.DrawString(dateLodged.AddDays(28).ToString("dd-MMM-yyyy"), boldfont, XBrushes.Black, 210, 358, XStringFormats.Default);

                    fileName = installationDirectory + "/doc/" + returnedVisa.st_varn_ref.ToString() + "permit.pdf";
                    custodyReceipt.Save(fileName);

                    return Json(new { varn = returnedVisa.st_varn_ref.ToString(), fileName = "doc/" + returnedVisa.st_varn_ref.ToString() + "permit.pdf" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { varn = returnedVisa.st_varn_ref.ToString(), fileName = "none" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { varn = "Error", fileName = fileName }, JsonRequestBehavior.AllowGet);

        }
    }
}
