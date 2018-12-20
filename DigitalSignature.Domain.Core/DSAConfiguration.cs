using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignature.Domain.Core
{
    public class DSAConfiguration
    {
        public string DocuSignRestAPI { get; set; }

        public string DSAConnectionString { get; set; }

        public string DSADocuSignUserName { get; set; }

        public string DSADocuSignPassword { get; set; }

        public string DSADocuSignIntegratorKey { get; set; }

        public string DSAWebHookCallBackURL { get; set; }

        public string DSADocumentEmailSubject { get; set; }
    }
}
