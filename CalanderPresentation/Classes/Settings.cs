﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CalanderPresentation
{
    public class Settings
    {
        public String SETTINGSFileVersion;

        public String SQLConnectionString;
        public bool SQLInitialized;
        public bool SQLWindowsAuthorization;
        public String SQLLogin;
        public String SQLPassword;
        public bool SQLAllowWriteToSFIDatabases;


        public bool OPCVariablesInitialized;
        public String OPCConnectionString;
        public String OPCCounterName;
        public String OPCSpeedName;

        public bool GENERALShowHistoryBrowser;


    }
}
