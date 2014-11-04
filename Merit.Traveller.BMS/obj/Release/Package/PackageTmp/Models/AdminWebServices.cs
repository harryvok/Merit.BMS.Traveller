using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Diagnostics;
using Merit.Traveller.BMS.BMS_Web_Admin;

namespace Merit.Traveller.BMS.Models
{
    public class AdminWebServices
    {
        bms_web_adminSoapClient client;

        public AdminWebServices(){
            this.client = new bms_web_adminSoapClient();
        }


        //ws_change_password
        public String changePassword(String userID, String oldPassword, String newPassword)
        {
            return client.ws_change_password(userID, oldPassword, newPassword);
        }
        //ws_chk_receipt_no
        public receipt_no_details checkReceiptNo(String userID, String password, String receiptNo, String defaultRecs)
        {
            return client.ws_chk_receipt_no(userID, password, receiptNo, defaultRecs);
        }
        //ws_get_all_sponsors
        public sponsor_list getAllSponsors(String userID, String password, String sponsor)
        {
            return client.ws_get_all_sponsors(userID, password, sponsor);
        }
        //ws_get_an_officer
        public officers_list getAnOfficer(String userID, String password, String officerID)
        {
            return client.ws_get_an_officer(userID, password, officerID);
        }
        //ws_get_checklist
        public checklist_list getCheckList(String userID, String password)
        {
            return client.ws_get_checklist(userID, password);
        }
        //ws_get_common_codes
        public common_codes_list getCommonCodes(String userID, String password)
        {
            return client.ws_get_common_codes(userID, password);
        }
        //ws_get_countries
        public countries_list getCountries(String userID, String password)
        {
            return client.ws_get_countries(userID, password);
        }
        //ws_get_doc_types
        public doc_types_list getDocTypes(String userID, String password)
        {
            return client.ws_get_doc_types(userID, password);
        }
        //ws_get_filter_intray
        public filter_list getFilterIntray(String userID, String password)
        {
            return client.ws_get_filter_intray(userID, password);
        }
        //ws_get_hemps
        public common_codes_list getHemps(String userID, String password, String projectGuid, String primeGuid)
        {
            return client.ws_get_hemps(userID, password, projectGuid, primeGuid);
        }
        //ws_get_merit_ini
        public merit_ini_list getMeritIni(String userID, String password)
        {
            return client.ws_get_merit_ini(userID, password);
        }
        //ws_get_merit_roles
        public officer_roles getMeritRoles(String userID, String password)
        {
            return client.ws_get_merit_roles(userID, password);
        }
        //ws_get_merit_settings
        public merit_settings getMeritSettings(String userID, String password)
        {
            return client.ws_get_merit_settings(userID, password);
        }
        //ws_get_nationality
        public nationality_list getNationality(String userID, String password)
        {
            return client.ws_get_nationality(userID, password);
        }
        //ws_get_officers
        public officers_list getOfficers(String userID, String password )
        {
            return client.ws_get_officers(userID, password);
        }
        //ws_get_offices
        public office_list getOffices(String userID, String password)
        {
            return client.ws_get_offices(userID, password);
        }
        //ws_get_outcomes
        public common_codes_list getOutcomes(String userID, String password)
        {
            return client.ws_get_outcomes(userID, password);
        }
        //ws_get_pclass_det
        public permit_class_list getPClassDet(String userID, String password, String permitTypeGuid)
        {
            return client.ws_get_pclass_det(userID, password, permitTypeGuid);
        }
        //ws_get_pclasses
        public common_codes_list getPClasses(String userID, String password)
        {
            return client.ws_get_pclasses(userID, password);
        }
        //ws_get_pemps
        public common_codes_list getPemps(String userID, String password, String projectGuid)
        {
            return client.ws_get_pemps(userID, password, projectGuid);
        }
        //ws_get_permit_class
        public permit_class_list getPermitClass(String userID, String password)
        {
            return client.ws_get_permit_class(userID, password);
        }
        //ws_get_permit_fees
        public permit_fees_list getPermitFees(String userID, String password)
        {
            return client.ws_get_permit_fees(userID, password);
        }
       
        //ws_get_proj_employers
        public proj_employers_list getProjEmployers(String userID, String password)
        {
            return client.ws_get_proj_employers(userID, password);
        }
        //ws_get_proj_pt_checks
        public proj_pt_checks_list proj_pt_checks(String userID, String password)
        {
            return client.ws_get_proj_pt_checks(userID, password);
        }
        //ws_get_proj_ptypes
        public proj_ptypes_list getProjectPTypes(String userID, String password)
        {
            return client.ws_get_proj_ptypes(userID, password); 
        }
        //ws_get_proj_sites
        public proj_sites_list getProjectSites(String userID, String password)
        {
            return client.ws_get_proj_sites(userID, password);
        }
        //ws_get_proj_structure
        public proj_structure_list getProjectStructure(String userID, String password)
        {
            return client.ws_get_proj_structure(userID, password);
        }
        //ws_get_project
        public project_list getProject(String userID, String password)
        {
            return client.ws_get_project( userID, password);
        }
        //ws_get_ptypes
        public common_codes_list getPtypes(String userID, String password)
        {
            return client.ws_get_ptypes(userID, password);
        }
        //ws_get_semps
        public common_codes_list getSemps(String userID, String password, String projectGuid, String primeGuid, String headGuid )
        {
            return client.ws_get_semps(userID, password, projectGuid, primeGuid, headGuid);
        }
        //ws_get_specific_codes
        public common_codes_list getSpecificCodes(String userID, String password, String codeType)
        {
            return client.ws_get_specific_codes(userID, password, codeType);
        }
        //ws_get_sponsor
        public sponsor_list getSponsor(String userID, String password)
        {
            return client.ws_get_sponsor(userID, password);
        }
        //ws_get_undertakings
        public checklist_list getUndertakings(String userID, String password, String projectGuid, String ptypeGuid, String pclassGuid )
        {
            return client.ws_get_undertakings(userID, password, projectGuid, ptypeGuid, pclassGuid);
        }

        //ws_get_user_options
        public user_options getUserOptions(String userID, String password, String responsibleCode)
        {
            return client.ws_get_user_options(userID, password, responsibleCode);
        }
        //ws_merit_login
        public login_result meritLogin(String userID, String password)
        {
            return client.ws_merit_login(userID, password);
        }


    }
}