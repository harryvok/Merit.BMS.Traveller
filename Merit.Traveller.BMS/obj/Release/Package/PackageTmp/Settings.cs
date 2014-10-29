using System;
using System.Configuration;

namespace Merit.Traveller.BMS
{
    public class BMSSettings : ConfigurationSection
    {
        private static BMSSettings settings
          = ConfigurationManager.GetSection("BMSSettings") as BMSSettings;

        public static BMSSettings Settings
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

        [ConfigurationProperty("SiteName")]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;’\"|\\")]
        public string SiteName
        {
            get { return (string)this["SiteName"]; }
            set { this["SiteName"] = value; }
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

        [ConfigurationProperty("WebServices")]
        public string WebServices
        {
            get { return (string)this["WebServices"]; }
            set { this["WebServices"] = value; }
        }
    }
}