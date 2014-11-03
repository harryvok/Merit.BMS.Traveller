using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Diagnostics;
using Merit.Traveller.BMS.BMS_Web_Permit_Proc;

namespace Merit.Traveller.BMS.Models
{
    public class PermitProcWebServices
    {
        bms_web_permit_procSoapClient client;

        public PermitProcWebServices() {
            this.client = new bms_web_permit_procSoapClient();
        }

        
        //ws_complete_action
        public action_return completeAction(String userID, String password, String intrayGuid, String rejectInd, DateTime status_Date, 
                                           DateTime statusTime, String comments, String officerGuid, String centralGuid)
        {
            return client.ws_complete_action(userID, password, intrayGuid, rejectInd, status_Date, statusTime, comments, officerGuid, centralGuid);
        }
        //ws_new_intray_dets
        public action_return newIntrayDets(String userID, String password, String intrayGuid, String officerGuid, DateTime intrayDate, DateTime intrayTime,
                                            int intrayType,String intrayDesc, String statusCode, DateTime statusDate, DateTime statusTime, String readInd,
                                            String linkGuid, String otherOfficer, String noteGuid, String permitGuid, String finalisedInd, String comments,
                                            String officeGuid, String decideAtCentral, String decideAtPost, DateTime changeDate )
        {
            intray_details intdet = new intray_details();
            intdet.ws_user_id = userID;
            intdet.ws_password = password;
            intdet.s_intray_guid = intrayGuid;
            intdet.s_change_date = changeDate;
            intdet.s_comments = comments;
            intdet.s_decide_at_central = decideAtCentral;
            intdet.s_decide_at_post = decideAtPost;
            intdet.s_finalised_ind = finalisedInd;
            intdet.s_intray_date = intrayDate;
            intdet.s_intray_desc = intrayDesc;
            intdet.s_intray_time = intrayTime;
            intdet.s_intray_type = intrayType;
            intdet.s_link_guid = linkGuid;
            intdet.s_note_guid = noteGuid;
            intdet.s_office_guid = officeGuid;
            intdet.s_officer_guid = officerGuid;
            intdet.s_other_officer = otherOfficer;
            intdet.s_permit_guid = permitGuid;
            intdet.s_read_ind = readInd;
            intdet.s_status_code = statusCode;
            intdet.s_status_date = statusDate;
            intdet.s_status_time = statusTime;

            return client.ws_new_intray_dets(intdet);
        }
        //ws_new_receipt_cntrl
        public new_return newReceiptCntrl( String userID, String password, String rec_cntrl_guid,
                                           String receiptNo, DateTime batch_dt, String batch_bal_ind,
                                           decimal batch_tot_cnt, decimal batch_tot_amt, String multi_sponsor_ind,
                                           String sponsor_guid)
        {
            receipt_cntrl_details rcd = new receipt_cntrl_details();
            rcd.ws_user_id = userID;
            rcd.ws_password = password;
            rcd.s_batch_bal_ind = batch_bal_ind;
            rcd.s_batch_dt = batch_dt;
            rcd.s_batch_tot_amt = batch_tot_amt;
            rcd.s_batch_tot_cnt = batch_tot_cnt;
            rcd.s_multi_sponsor_ind = multi_sponsor_ind;
            rcd.s_rec_cntrl_guid = rec_cntrl_guid;
            rcd.s_receipt_no = receiptNo;
            rcd.s_sponsor_guid = sponsor_guid;

            return client.ws_new_receipt_cntrl(rcd);
        }
        //ws_new_receipt_tot
        public new_return newReceptTot( String userID, String password, String rec_tot_guid, String rec_cntrl_guid,
                                        String permitTypeGuid, String permitClassGuid, String tot_bal_ind, decimal given_tot_cnt,
                                        decimal given_tot_amt, decimal pt_tot_cnt, decimal pt_tot_amt)
        {
            receipt_tot_details rtd = new receipt_tot_details();
            rtd.ws_user_id = userID;
            rtd.ws_password = password;
            rtd.s_given_tot_amt = given_tot_amt;
            rtd.s_given_tot_cnt = given_tot_cnt;
            rtd.s_permit_class_guid = permitClassGuid;
            rtd.s_permit_type_guid = permitTypeGuid;
            rtd.s_pt_tot_amt = pt_tot_amt;
            rtd.s_pt_tot_cnt = pt_tot_cnt;
            rtd.s_rec_cntrl_guid = rec_cntrl_guid;
            rtd.s_rec_tot_guid = rec_tot_guid;
            rtd.s_tot_bal_ind = tot_bal_ind;

            return client.ws_new_receipt_tot(rtd);
        }
        //ws_new_visa_reg
        public new_visa_ret newVisaReg(String userID, String password, String officeGuid, String officerGuid,
                                       String OfficerGuid, DateTime dateLodged, String pTypeGuid, String pClassGuid,
                                       String receiptNo, String docType, String nationGuid, String docNo, 
                                       DateTime expiryDate, String surname, String givenName, String genderCode,
                                       DateTime dob, String sponsorGuid, String projectGuid, String comments,
                                       String primeGuid, String headGuid, String subGuid, String checksGuid1,
                                       String checksGuid2, String centralGuid, String currentPermitGuid, String guarantee )
        {
            new_visa_inp nvi = new new_visa_inp();
         
            ArrayOfString chkguid = new ArrayOfString();
            chkguid.Add(checksGuid1);
            chkguid.Add(checksGuid2);

            nvi.ws_user_id = userID;
            nvi.ws_password = password;
            nvi.s_central_guid = centralGuid;
            nvi.s_checks_guid = chkguid;
            nvi.s_comments = comments;
            nvi.s_current_permit_guid = currentPermitGuid;
            nvi.s_date_lodged = dateLodged;
            nvi.s_dob = dob;
            nvi.s_doc_no = docNo;
            nvi.s_doc_type = docType;
            nvi.s_expiry_date = expiryDate;
            nvi.s_gender_code = genderCode;
            nvi.s_given_name = givenName;
            nvi.s_guarantee = guarantee;
            nvi.s_head_guid = headGuid;
            nvi.s_nation_guid = nationGuid;
            nvi.s_office_guid = officeGuid;
            nvi.s_officer_guid = officerGuid;
            nvi.s_pclass_guid = pClassGuid;
            nvi.s_prime_guid = primeGuid;
            nvi.s_project_guid = projectGuid;
            nvi.s_ptype_guid = pTypeGuid;
            nvi.s_receipt_no = receiptNo;
            nvi.s_sponsor_guid = sponsorGuid;
            nvi.s_sub_guid = subGuid;
            nvi.s_surname = surname;
            
            return client.ws_new_visa_reg(nvi);
        }
        //ws_upd_applicant_dets
        public applicant_dets updApplicantDets(String userID, String password, int status, String message,
                                               String titleGuid, String surname, String givenName, String nationalityCode,
                                               DateTime dob, String genderCode, String countryOfBirth, 
                                               String placeOfBirth, String maritalStatus, String occupation,
                                               String passportNumber, String imageGuid, String address1, 
                                               String address2, String address3, String phoneNo, String emailAddr,
                                               int fileNo, String fileType, decimal workPermitNo, String positionNo,
                                               DateTime wpExpiry, DateTime ppExpiry, byte[] picture, 
                                               String completeInd, String picTxt, String posNoReqdInd, String workPermReqdInd, String permitGuid,
                                               String applicantGuid, String addrInCountry, String officerGuid)
        {
            applicant_dets ad = new applicant_dets();
            ad.s_addr_in_country = addrInCountry;
            ad.s_address_1 = address1;
            ad.s_address_2 = address2;
            ad.s_address_3 = address3;
            ad.s_applicant_guid = applicantGuid;
            ad.s_complete_ind = completeInd;
            ad.s_country_of_birth = countryOfBirth;
            ad.s_date_of_birth = dob;
            ad.s_email_addr = emailAddr;
            ad.s_file_no = fileNo;
            ad.s_file_type = fileType;
            ad.s_gender_code = genderCode;
            ad.s_given_name = givenName;
            ad.s_image_guid = imageGuid;
            ad.s_marital_status = maritalStatus;
            ad.s_nationality_code = nationalityCode;
            ad.s_occupation = occupation;
            ad.s_officer_guid = officerGuid;
            ad.s_passport_number = passportNumber;
            ad.s_permit_guid = permitGuid;
            ad.s_phone_no = phoneNo;
            ad.s_pic_txt = picTxt;
            ad.s_picture = picture;
            ad.s_place_of_birth = placeOfBirth;
            ad.s_pos_no_reqd_ind = posNoReqdInd;
            ad.s_position_no = positionNo;
            ad.s_pp_expiry = ppExpiry;
            ad.s_surname = surname;
            ad.s_title_guid = titleGuid;
            ad.s_work_perm_reqd_ind = workPermReqdInd;
            ad.s_work_permit_no = workPermitNo;
            ad.s_wp_expiry = wpExpiry;

            return client.ws_upd_applicant_dets(userID, password, ad);
        }
        //ws_upd_proc_check_dets
        public applicant_dets updProcCheckDets(String userID, String password, String wsStatus,
                                               int wsMessage, String pcheckGuid1, String pcheckGuid2,
                                               String checkGuid1, String checkGuid2, String officerGuid1,
                                               String officerGuid2, DateTime dateTime1, DateTime dateTime2,
                                               String receiptNumber1, String receiptNumber2, String comments1,
                                               String comments2, String receiptGuid1, String receiptGuid2,
                                               String finalisedInd1, String finalisedInd2, String checkDesc1,
                                               String checkDesc2, String mandatoryInd1, String mandatoryInd2,
                                               String postInd1, String postInd2, String comments, String officerGuid,
                                               String officeGuid, String centralGuid, String rejectInd,
                                               String forOfficeGuid, String permitGuid, String intrayGiud)
        {
            applicant_dets ad = new applicant_dets();

            ArrayOfString pckGuid = new ArrayOfString();
            pckGuid.Add(pcheckGuid1);
            pckGuid.Add(pcheckGuid2);
            ArrayOfString chkGuid = new ArrayOfString();
            chkGuid.Add(checkGuid1);
            chkGuid.Add(checkGuid2);
            ArrayOfString offcrGuid = new ArrayOfString();
            offcrGuid.Add(officerGuid1);
            offcrGuid.Add(officerGuid2);
            ArrayOfDateTime chkDate = new ArrayOfDateTime();
            chkDate.Add(dateTime1);
            chkDate.Add(dateTime2);
            ArrayOfString rcptNum = new ArrayOfString();
            rcptNum.Add(receiptNumber1);
            rcptNum.Add(receiptNumber2);
            ArrayOfString comnts = new ArrayOfString();
            comnts.Add(comments1);
            comnts.Add(comments2);
            ArrayOfString rcptGuid = new ArrayOfString();
            rcptGuid.Add(receiptGuid1);
            rcptGuid.Add(receiptGuid2);
            ArrayOfString finalisedInd = new ArrayOfString();
            finalisedInd.Add(finalisedInd1);
            finalisedInd.Add(finalisedInd2);
            ArrayOfString checkDesc = new ArrayOfString();
            checkDesc.Add(checkDesc1);
            checkDesc.Add(checkDesc2);
            ArrayOfString mndtryInd = new ArrayOfString();
            mndtryInd.Add(mandatoryInd1);
            mndtryInd.Add(mandatoryInd2);

            return client.ws_upd_applicant_dets(userID, password, ad);
        }
        //ws_upd_recommend_dets
        public action_return updRecommendDets(String userID, String password, int wsStatus, String wsMessage,
                                              String comments, String officer_guid, 
                                              String office_guid, String centralGuid, String rejectInd,
                                              String permitGuid, String intrayGuid, 
                                              DateTime validFrom, DateTime validTo)
        {
            proc_recommend_dets prd = new proc_recommend_dets();
            prd.s_central_guid = centralGuid;
            prd.s_comments = comments;
            prd.s_officer_guid = officer_guid;
            prd.s_permit_guid = permitGuid;
            prd.s_reject_ind = rejectInd;
            prd.s_valid_from = validFrom;
            prd.s_valid_to = validTo;

            return client.ws_upd_recommend_dets(userID, password, prd);
        }

        //ws_upd_sponsor_dets
        public sponsor_dets updSponsorDets(String userID, String password, int wsStatus,
                                           String wsMessage, String sponsorGuid, String companyName,
                                           String address1, String address2, String address3, String phoneNo,
                                           String faxNo, String emailAddr, String postalAdd1,
                                           String postalAdd2, String postalAdd3, String principalPosition,
                                           String titleGuid, String Surname, String givenName, String passportNumber,
                                           String statusGuid, String activeInd, String verifiedInd,
                                           String projEnabled, String commentGuid, String applicantGuid,
                                           String permitGuid, String officerGuid, String completeInd)
        {
            sponsor_dets sd = new sponsor_dets();
            sd.s_active_ind = activeInd;
            sd.s_address_1 = address1;
            sd.s_address_2 = address2;
            sd.s_address_3 = address3;
            sd.s_applicant_guid = applicantGuid;
            sd.s_comment_guid = commentGuid;
            sd.s_company_name = companyName;
            sd.s_complete_ind = completeInd;
            sd.s_email_addr = emailAddr;
            sd.s_fax_no = faxNo;
            sd.s_given_name = givenName;
            sd.s_officer_guid = officerGuid;
            sd.s_passport_number = passportNumber;
            sd.s_permit_guid = permitGuid;
            sd.s_phone_no = phoneNo;
            sd.s_postal_addr_1 = postalAdd1;
            sd.s_postal_addr_2 = postalAdd2;
            sd.s_postal_addr_3 = postalAdd3;
            sd.s_principal_position = principalPosition;
            sd.s_proj_enabled = projEnabled;
            sd.s_sponsor_guid = sponsorGuid;
            sd.s_status_guid = statusGuid;
            sd.s_surname = Surname;
            sd.s_title_guid = titleGuid;
            sd.s_verified_ind = verifiedInd;
            sd.ws_message = wsMessage;
            sd.ws_status = wsStatus;

            return client.ws_upd_sponsor_dets(userID, password, sd);
        }
    }
}