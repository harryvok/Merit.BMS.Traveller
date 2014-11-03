using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Diagnostics;
using Merit.Traveller.BMS.BMS_Web_Permit_Data;

namespace Merit.Traveller.BMS.Models
{
    public class PermitDataWebServices
    {

        bms_web_permit_dataSoapClient client;

        public PermitDataWebServices() {
            this.client = new bms_web_permit_dataSoapClient();
        }


        //ws_calc_fee_amt
        public rec_cntrl_return calculateFeeAmount(String userID, String password, String cntrlGuid, String ptypeGuid,
                                                   Boolean allow_ext, String officeGuid, String pclassGuid)
        {
            rec_cntrl_inp rci = new rec_cntrl_inp();
            rci.ws_user_id = userID;
            rci.ws_password = password;
            rci.s_rec_cntrl_guid = cntrlGuid;
            rci.s_ptype_guid = ptypeGuid;
            rci.s_allow_ext = allow_ext;
            rci.s_office_guid = officeGuid;
            rci.s_pclass_guid = pclassGuid;

            return  client.ws_calc_fee_amt(rci);

        }
        //ws_check_outstand_items
        public action_return checkOutstandItems(String userID, String password, String permitGuid)
        {
            return client.ws_check_outstand_items(userID, password, permitGuid);
        }
        //ws_check_sponsor_complete
        public action_return checkSponsorCompete(String userID, String password, String sponsorGuid, String permitGuid)
        {
            return  client.ws_check_sponsor_complete(userID, password, sponsorGuid, permitGuid);
        }
        //ws_chk_approve_allowed
        public action_return checkApproveAllowed(String userID, String password, String permitGuid)
        {
            return client.ws_chk_approve_allowed(userID, password, permitGuid);
        }
        //ws_chk_biodata_string
        public action_return checkBioData(String userID, String password, String aString)
        {
            return client.ws_chk_biodata_string(userID, password, aString);
        }
        //ws_get_action_intray
        public action_intray_list getActionIntray(String userName, String password, String dataGroup, 
                                                  String otherDataGroup, String fromDate, String toDate)
        {
            return client.ws_get_action_intray(userName, password, dataGroup, otherDataGroup, fromDate, toDate);
        }
        //ws_get_applicant_dets
        public applicant_dets getApplicationDets(String userID, String password, String dependantGuid, String guid)
        {
            return client.ws_get_applicant_dets(userID, password, dependantGuid, guid);
        }
        //ws_get_biodata_details
        public biodata_details getBiodataDetails(String userID, String password, String docType, String nationality, String docNumber)
        {
            return client.ws_get_biodata_details(userID, password, docType, nationality, docNumber);
        }
        //ws_get_custody_recpt_details
        public cust_recpt_details getCustodyReceipt(String userID, String password, String permitGuid)
        {
            return  client.ws_get_custody_recpt_details(userID, password, permitGuid);
        }
        //ws_get_image
        public image_details getImage(String userID, String password, String nationality, String docNumber, String permitGuid, String imageType)
        {
            return client.ws_get_image(userID, password, nationality, docNumber, permitGuid, imageType);
        }
        //ws_get_message_count
        public int getMessageCount(String userID, String password, String officerGuid)
        {
            return client.ws_get_message_count(userID, password, officerGuid);
        }
        //ws_get_officer_filters
        public filter_list getOfficerFilters(String userId, String password, String filterType)
        {
            return client.ws_get_officer_filters(userId, password, filterType);
        }
        //ws_get_perm_det_list
        public permit_list_list getPermitListDets(String userID, String password, String permitGuid)
        {
            return client.ws_get_perm_det_list(userID, password, permitGuid);
        }
        //ws_get_permit_base_dets
        public permit_base_dets getPermitBaseDets(String userID, String password, String guid)
        {
            return client.ws_get_permit_base_dets(userID, password, guid);
        }
        //ws_get_permit_head_dets
        public permit_head_dets getPermitHeadDets(String userID, String password, String permitGuid)
        {
            return client.ws_get_permit_head_dets(userID, password, permitGuid);
        }
        //ws_get_permit_list
        /*public permit_list getPermitList(String User_id,
                                            String Password,
                                            DateTime Lodged_f,
                                            DateTime Lodged_t,
                                            DateTime Spec_com_f,
                                            DateTime Spec_com_t,
                                            DateTime Spec_dept_f,
                                            DateTime Spec_dept_t,
                                            DateTime Act_arr_f,
                                            DateTime Act_arr_t,
                                            DateTime Act_dep_f,
                                            DateTime Act_dep_t,
                                            DateTime Act_approve_f,
                                            DateTime Act_approve_t,
                                            decimal Permit_no,
                                            decimal varnNo,
                                            int File_no,
                                            int Days_f,
                                            int Days_t,
                                            int Aperiod_f,
                                            int Aperiod_t,
                                            String Permit_type,
                                            String Office,
                                            String permitClass,
                                            String Finalised,
                                            String Express,
                                            String In_country,
                                            String Company,
                                            String Surname,
                                            String Given,
                                            String Outcome,
                                            String Project,
                                            String Prime_emp,
                                            String Head_emp,
                                            String Sub_emp,
                                            String Emp)
        {
            return client.ws_get_permit_list(Password,Lodged_f,Lodged_t,Spec_com_f,Spec_com_t,
                                             Spec_dept_f,Spec_dept_t,Act_arr_f,Act_arr_t,Act_dep_f,
                                             Act_dep_t, Act_approve_f, Act_approve_t, Permit_no, varnNo, File_no,
                                             Days_f, Days_t, Aperiod_f,Aperiod_t,Permit_type,Office,
                                             permitClass, Finalised, Express, In_country, Company, Surname,
                                             Given,Outcome,Project,Prime_emp,Head_emp,Sub_emp,Emp);
        }*/

       
        //ws_get_permit_log_dets
        public permit_log_dets getPermitLogDets(String userID, String password, String permitGuid)
        {
            return client.ws_get_permit_log_dets(userID, password, permitGuid);
        }
        //ws_get_permit_print
        public permit_print_list getPermitPrint(String userID, String password, String permit_guid)
        {
           return  client.ws_get_permit_print(userID, password, permit_guid);
        }
        //ws_get_permit_proc_dets
        public permit_proc_dets getPermitProcDets(String userID, String password, String permitGuid)
        {
            return client.ws_get_permit_proc_dets(userID, password, permitGuid);
        }
        //ws_get_proc_approve_dets
        public proc_approve_details getProcApproveDetails(String userID, String password, String intrayGuid)
        {
            return client.ws_get_proc_approve_dets(userID, password, intrayGuid);
        }
        //ws_get_proc_check_items
        public proc_check_details getProcCheckItems(String userID, String password, String permitGuid,String intrayGuid, String centralGuid, String officeGuid, String officerGuid )
        {
            return client.ws_get_proc_check_items(userID, password, permitGuid, intrayGuid, centralGuid, officeGuid, officerGuid);
        }
        //ws_get_proc_decision_dets
        public proc_decision_details getProcDecisionDets(String userID, String password, String intrayGuid, String centralGuid)
        {
            return client.ws_get_proc_decision_dets(userID, password, intrayGuid, centralGuid);
        }

        //ws_get_rec_pc_cnts
        public rec_cntrl_return getRecPcCnts(String userID, String password, String s_rec_cntrl_giud, String s_ptype_guid, bool s_allow_ext, String s_office_guid, String s_pclass_guid)
        {
            rec_cntrl_inp rci = new rec_cntrl_inp();
            rci.s_allow_ext = s_allow_ext;
            rci.s_office_guid = s_office_guid;
            rci.s_pclass_guid = s_pclass_guid;
            rci.s_ptype_guid = s_ptype_guid;
            rci.s_rec_cntrl_guid = s_rec_cntrl_giud;
            rci.ws_password = password;
            rci.ws_user_id = userID;

            return client.ws_get_rec_pc_cnts(rci);


        }
        //ws_get_rec_pclasses
        public rec_cntrl_return getRecPclasses(String userID, String password, String s_rec_cntrl_guid, String s_ptype_guid, bool s_allow_ext, String s_office_guid, String s_pclass_guid)
        {
            rec_cntrl_inp rci = new rec_cntrl_inp();
            rci.ws_user_id = userID;
            rci.ws_password = password;
            rci.s_rec_cntrl_guid = s_rec_cntrl_guid;
            rci.s_ptype_guid = s_ptype_guid;
            rci.s_allow_ext = s_allow_ext;
            rci.s_office_guid = s_office_guid;
            rci.s_pclass_guid = s_pclass_guid;

            return client.ws_get_rec_pclasses(rci);
        }
        //ws_get_rec_ptypes
        public rec_cntrl_return getRecPtypes(String userID, String password, String s_rec_cntrl_guid, String s_ptype_guid, bool s_allow_ext, String s_office_guid, String s_pclass_guid)
        {
            rec_cntrl_inp rci = new rec_cntrl_inp();
            rci.s_allow_ext = s_allow_ext;
            rci.s_office_guid = s_office_guid;
            rci.s_pclass_guid = s_pclass_guid;
            rci.s_ptype_guid = s_ptype_guid;
            rci.s_rec_cntrl_guid = s_rec_cntrl_guid;
            rci.ws_password = password;
            rci.ws_user_id = userID;

            return client.ws_get_rec_ptypes(rci);
        }
        //ws_get_rec_unbal_list
        public rec_unbal_list getRecUnbalList(String userID, String password)
        {
            return client.ws_get_rec_unbal_list(userID, password);
        }

        //ws_get_recommend_dets
        public recommend_dets getRecommendDets(String userID, String password, String intray_Guid)
        {
            return client.ws_get_recommend_dets(userID, password, intray_Guid);
        }
        //ws_get_single_message
        public message_dets getSingleMessage(String userID, String password, String messageGuid)
        {
            return client.ws_get_single_message(userID, password, messageGuid);
        }
        //ws_get_single_sponsor
        public sponsor_dets getSingleSponsor(String userID, String password, String sponsor_Guid)
        {
            return client.ws_get_single_sponsor(userID, password, sponsor_Guid);
        }
        
       
         
    }
}