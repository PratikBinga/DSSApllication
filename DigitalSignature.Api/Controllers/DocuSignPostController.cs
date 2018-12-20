using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DigitalSignature.Service.Services;
using DigitalSignature.Domain;
using DigitalSignature.Domain.Core.Model;


namespace DigitalSignature.Api.Controllers
{
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("api/[controller]")]
    [ApiController]
    public class DocuSignPostController : ControllerBase
    {
        private readonly IDocuSignService _docuSignService;
       
        public DocuSignPostController(IDocuSignService docuSignService)
        {
            _docuSignService = docuSignService;
           
        }

        public ActionResult<DigitalSignature.Domain.Core.Model.DocuSignPostResponse> SendDocumentforSign(List<Recipient> recipients, byte[] filedata)
        {

            if (recipients == null || recipients.Count == 0)
            {
                return BadRequest();
            }

            else
            {
                DocuSignPostResponse docuSignPostResponse = new DocuSignPostResponse();

                docuSignPostResponse = _docuSignService.docusign(filedata, recipients);
                if (docuSignPostResponse == null)
                {
                    return NotFound(docuSignPostResponse);
                }
                return Ok(docuSignPostResponse);

            }
            // fetch the url from caller api and store in DB for further operation.
            // HttpContext.Request.Body.

        }
    }
}