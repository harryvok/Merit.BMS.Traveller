using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Merit.Traveller.BMS.BMS_Web_Permit_Data;

namespace Merit.Traveller.BMS
{
    public class permitmodel
    {
        public String guid { get; set; }
        public String varnRef { get; set; }
        public String dateLodged { get; set; }
        public String lodgedAt { get; set; }
        public String permitClass { get; set; }
        public String permitType { get; set; }
        public String permitNo { get; set; }
        public String outcome { get; set; }
        public String applicant { get; set; }
        public String dob { get; set; }
        public String nationality { get; set; }
        public String approvalDate { get; set; }
        public String finalised { get; set; }

        public proc_permit_dets processDescs { get; set; }
        public String dependentID { get; set; }
        public String sponsorID { get; set; }
        public List<String> processes { get; set; }
        public List<String> processTypes { get; set; }


        public permitmodel(String guid, String varnRef, String dateLodged, String lodgedAt, String permitClass, String permitType, String permitNo, String outcome, String applicant, String dob, String nationality,
                   String approvalDate, String finalised, proc_permit_dets processDescs, String dependentID, String sponsorID, List<String> processes, List<String> processTypes)
        {
            this.guid = guid;
            this.varnRef = varnRef;
            this.dateLodged = dateLodged;
            this.lodgedAt = lodgedAt;
            this.permitClass = permitClass;
            this.permitType = permitType;
            this.permitNo = permitNo;
            this.outcome = outcome;
            this.applicant = applicant;
            this.dob = dob;
            this.nationality = nationality;
            this.approvalDate = approvalDate;
            this.finalised = finalised;
            this.processDescs = processDescs;
            this.dependentID = dependentID;
            this.sponsorID = sponsorID;
            this.processes = processes;
            this.processTypes = processTypes;

        }

    }
}