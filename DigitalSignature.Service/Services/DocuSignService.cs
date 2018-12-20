using DigitalSignature.Domain.Core.Model;
using System;
using System.Collections.Generic;
using AutoMapper;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using DocuSign.eSign.Api;
using DigitalSignature.Domain.Core;
using Microsoft.Extensions.Options;
using DigitalSignature.Domain.Core.Data;
using Newtonsoft.Json;

namespace DigitalSignature.Service.Services
{
    public class DocuSignService : IDocuSignService
    {


        public string _accountId = "";

         private readonly DigitalSignatureContext _context;

        private readonly DSAConfiguration _dSAConfiguration;

        private readonly IMapper _mapper;
        public DocuSignService(IMapper mapper, DigitalSignatureContext context, IOptions<DSAConfiguration> dSAConfiguration)
        {
            _mapper = mapper;
            _context = context;
            _dSAConfiguration = dSAConfiguration.Value;
        }
        public DocuSignPostResponse docusign(byte[] fileData, List<Recipient> recipients)
        {
            DocuSignPostResponse docuSignPostResponse = new DocuSignPostResponse();

            ApiClient apiClient = new ApiClient(_dSAConfiguration.DocuSignRestAPI);
            Configuration.Default.ApiClient = apiClient;

            string accountId = loginApi(_dSAConfiguration.DSADocuSignUserName, _dSAConfiguration.DSADocuSignPassword);


            List<EnvelopeEvent> _lstEnvelopeEvents = new List<EnvelopeEvent>();
            _lstEnvelopeEvents.Add(new EnvelopeEvent() { EnvelopeEventStatusCode = "sent" });
            _lstEnvelopeEvents.Add(new EnvelopeEvent() { EnvelopeEventStatusCode = "delivered" });
            _lstEnvelopeEvents.Add(new EnvelopeEvent() { EnvelopeEventStatusCode = "completed" });
            _lstEnvelopeEvents.Add(new EnvelopeEvent() { EnvelopeEventStatusCode = "declined" });
            _lstEnvelopeEvents.Add(new EnvelopeEvent() { EnvelopeEventStatusCode = "voided" });


            List<RecipientEvent> _lstRecipientEvents = new List<RecipientEvent>();
            _lstRecipientEvents.Add(new RecipientEvent() { RecipientEventStatusCode = "Sent" });
            _lstRecipientEvents.Add(new RecipientEvent() { RecipientEventStatusCode = "Delivered" });
            _lstRecipientEvents.Add(new RecipientEvent() { RecipientEventStatusCode = "Completed" });
            _lstRecipientEvents.Add(new RecipientEvent() { RecipientEventStatusCode = "Declined" });
            _lstRecipientEvents.Add(new RecipientEvent() { RecipientEventStatusCode = "AuthenticationFailed" });
            _lstRecipientEvents.Add(new RecipientEvent() { RecipientEventStatusCode = "AutoResponded" });


            EventNotification _eventNotification = new EventNotification();
            EnvelopeDefinition envDef = new EnvelopeDefinition();

            _eventNotification.Url = _dSAConfiguration.DSAWebHookCallBackURL;
            _eventNotification.LoggingEnabled = "true";
            _eventNotification.RequireAcknowledgment = "true";
            _eventNotification.UseSoapInterface = "true";
            _eventNotification.IncludeCertificateWithSoap = "false";
            _eventNotification.SignMessageWithX509Cert = "false";
            _eventNotification.IncludeDocuments = "true";
            _eventNotification.IncludeEnvelopeVoidReason = "true";
            _eventNotification.IncludeTimeZone = "true";
            _eventNotification.IncludeSenderAccountAsCustomField = "true";
            _eventNotification.IncludeDocumentFields = "true";
            _eventNotification.IncludeCertificateOfCompletion = "true";
            _eventNotification.EnvelopeEvents = _lstEnvelopeEvents;
            _eventNotification.RecipientEvents = _lstRecipientEvents;


            envDef.EmailSubject = _dSAConfiguration.DSADocumentEmailSubject;
            // Add a document to the envelope  
            Document doc = new Document();
            doc.DocumentBase64 = System.Convert.ToBase64String(fileData);

            // assign document id with Guid
            doc.DocumentId = new Guid().ToString();
            envDef.Documents = new List<Document>();
            envDef.Documents.Add(doc);
            // Add a recipient to sign the documeent  
            Signer signer = new Signer();

            envDef.Recipients = new Recipients();
            envDef.Recipients.Signers = new List<Signer>();

            foreach (var item in recipients)
            {
                signer.Email = item.Email;
                signer.Name = item.Name;
                //need to change in future if reqd.
                signer.RecipientId = "1";
                envDef.Recipients.Signers.Add(signer);
            }

            envDef.EventNotification = _eventNotification;
            //set envelope status to "sent" to immediately send the signature request  
            envDef.Status = "sent";

            // |EnvelopesApi| contains methods related to creating and sending Envelopes (aka signature requests)  
            EnvelopesApi envelopesApi = new EnvelopesApi();
            EnvelopeSummary envelopeSummary = envelopesApi.CreateEnvelope(accountId, envDef, null);
            
           
            if (envelopeSummary != null || envelopeSummary.EnvelopeId == null)
            {
               // print the JSON response
                var result = JsonConvert.SerializeObject(envelopeSummary);

                // storing the envelope detail and recipent in DB
                DigitalSignatureRecipient dsRecipient = new DigitalSignatureRecipient();
                dsRecipient.Status = envelopeSummary.Status;
                dsRecipient.ClientAuthToken = "";
                dsRecipient.EnvelopeId = envelopeSummary.EnvelopeId;
                dsRecipient.RecipientEmail = "";
                dsRecipient.ReturnUrl = "";
                dsRecipient.SentOn = System.Convert.ToDateTime(envelopeSummary.StatusDateTime);
                //DigitalSignatureContext _context = new DigitalSignatureContext();
                _context.DigitalSignatureRecipients.Add(dsRecipient);
                _context.SaveChanges();


                //recipient.Description = "envDef.EmailSubject";
                //recipient.Email = recipientEmail;
                //recipient.Name = recipientName;
                //recipient.Status = envelopeSummary.Status;
                //recipient.Documents = fileBytes;
                //recipient.SentOn = System.Convert.ToDateTime(envelopeSummary.StatusDateTime);
                //recipient.EnvelopeID = envelopeSummary.EnvelopeId;
                //DocusignDBEntities _context = new DocusignDBEntities();
                //_context.Recipients.Add(recipient);
                //_context.SaveChanges();

            }

            return docuSignPostResponse;
        }

        public string loginApi(string userName, string passWord)
        {

            // we set the api client in global config when we configured the client  
            ApiClient apiClient = Configuration.Default.ApiClient;
            string authHeader = "{\"Username\":\"" + userName + "\", \"Password\":\"" + passWord + "\", \"IntegratorKey\":\"" + _dSAConfiguration.DSADocuSignIntegratorKey + "\"}";
            Configuration.Default.AddDefaultHeader("X-DocuSign-Authentication", authHeader);
            // we will retrieve this from the login() results  
            string accountId = null;
            // the authentication api uses the apiClient (and X-DocuSign-Authentication header) that are set in Configuration object  
            AuthenticationApi authApi = new AuthenticationApi();
            LoginInformation loginInfo = authApi.Login();
            // find the default account for this user  
            foreach (LoginAccount loginAcct in loginInfo.LoginAccounts)
            {
                if (loginAcct.IsDefault == "true")
                {
                    accountId = loginAcct.AccountId;
                    _accountId = accountId;
                    break;
                }
            }
            if (accountId == null)
            { // if no default found set to first account  
                accountId = loginInfo.LoginAccounts[0].AccountId;
            }
            return accountId;
        }



    }
}
