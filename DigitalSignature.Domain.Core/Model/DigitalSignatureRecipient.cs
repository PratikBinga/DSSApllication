using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignature.Domain.Core.Model
{
  public  class DigitalSignatureRecipient
    {
        public Guid Id { get; set; }
        public string ClientAuthToken { get; set; }

        public string EnvelopeId { get; set; }

        public string RecipientEmail { get; set; }
        // need to change it further to Int.
        public string Status { get; set; }

        public DateTime SentOn { get; set; }

        public DateTime UpdateOn { get; set; }

        public string ReturnUrl { get; set; }

        
    }
}
