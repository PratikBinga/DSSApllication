using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignature.Domain.Core.Model;

namespace DigitalSignature.Service.Services
{
  public  interface IDocuSignService
    {
        DocuSignPostResponse docusign( byte [] fileData , List <Recipient> recipients);
    }
}
