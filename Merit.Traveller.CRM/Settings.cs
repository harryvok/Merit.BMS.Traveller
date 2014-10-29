using System;
using System.Configuration;

namespace Merit.Traveller.CRM
{
    public class CRMSettings : ConfigurationSection
    {
        private static CRMSettings settings
          = ConfigurationManager.GetSection("CRMSettings") as CRMSettings;

        public static CRMSettings Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("Setup"
          , IsRequired = true)]
        public bool Setup
        {
            get { return (bool)this["Setup"]; }
            set { this["Setup"] = value; }
        }

        [ConfigurationProperty("CouncilName")]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;’\"|\\")]
        public string CouncilName
        {
            get { return (string)this["CouncilName"]; }
            set { this["CouncilName"] = value; }
        }

        [ConfigurationProperty("Attachments"
          , DefaultValue = "/Attachments/")]
        [StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}’" )]
        public string Attachments
        {
            get { return (string)this["Attachments"]; }
            set { this["Attachments"] = value; }
        }

         [ConfigurationProperty("LookupEnabled"
        , IsRequired = true)]
        public bool LookupEnabled
        {
            get { return (bool)this["LookupEnabled"]; }
            set { this["LookupEnabled"] = value; }
        }

        [ConfigurationProperty("AuthUser"
          , DefaultValue = "administrator")]
        [StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}’" )]
        public string AuthUser
        {
            get { return (string)this["AuthUser"]; }
            set { this["AuthUser"] = value; }
        }

        [ConfigurationProperty("AuthPass")]
        public string AuthPass
        {
            get { return (string)this["AuthPass"]; }
            set { this["AuthPass"] = value; }
        }
    }
}