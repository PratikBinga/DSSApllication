using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignature.Domain.Core.Model
{
  public  class DocuSignPostResponse
    {
        public int Status { get; set; }
        public string EnvelopeId { get; set; }

        public  string Message { get; set; }
    }
}
