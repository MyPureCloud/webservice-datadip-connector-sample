using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using inin.Bridge.WebServices.Datadip.Lib;

namespace inin.Bridge.WebServices.Datadip.Impl
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SampleWebServicesImplementation : IWebServicesServer
    {
        private List<Contact> contacts = new List<Contact>();
        private List<Case> cases = new List<Case>();
        private List<Account> accounts = new List<Account>();
        private List<ContactAccountRelationship> contactAccountRelationships = new List<ContactAccountRelationship>();
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        private static readonly Regex NonDigitsExpression = new Regex(@"\D", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public SampleWebServicesImplementation(string filePath) 
        {
            reload(filePath);
        }

        private void reload(string filePath)
        {
            contacts = new List<Contact>();
            cases = new List<Case>();
            accounts = new List<Account>();
            importAccounts(filePath + "/accounts");
            importCases(filePath + "/cases");
            importContacts(filePath + "/contacts");
            importRelationships(filePath + "/contactAccountRelationship.json");
        }

        private string readFromFile(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            string returnVal = sr.ReadToEnd();
            return returnVal;
        }

        private void importAccounts(string filePath)
        {
            Console.WriteLine("Importing Accounts.");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            String[] filePaths = Directory.GetFiles(filePath);
            foreach (String importFile in filePaths)
            {
                Account account = jss.Deserialize<Account>(readFromFile(importFile));
                Console.WriteLine("Found account with ID: " + account.Id);
                accounts.Add(account);
            }
        }

        private void importContacts(string filePath)
        {
            Console.WriteLine("Importing Contacts.");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            String[] filePaths = Directory.GetFiles(filePath);
            foreach (String importFile in filePaths)
            {
                Contact contact = jss.Deserialize<Contact>(readFromFile(importFile));
                Console.WriteLine("Found contact with ID: " + contact.Id);
                contacts.Add(contact);
            }
        }

        private void importCases(string filePath)
        {
            Console.WriteLine("Importing cases.");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            String[] filePaths = Directory.GetFiles(filePath);
            foreach (String importFile in filePaths)
            {
                Case newCase = jss.Deserialize<Case>(readFromFile(importFile));
                cases.Add(newCase);
                Console.WriteLine("Found case with subject: " + newCase.Subject);
            }
        }

        private void importRelationships(string filePath)
        {
            ContactAccountRelationshipRecord carr = jss.Deserialize<ContactAccountRelationshipRecord>(readFromFile(filePath));
            contactAccountRelationships = carr.ContactAccountRelationship;
            Console.WriteLine("Relationship Import Complete.");
            foreach (ContactAccountRelationship car in contactAccountRelationships) {
                Console.WriteLine("Loaded relationship between contact " + car.ContactId + " and account " + car.AccountNumber);
            }
        }

        public ResponseContact GetContactByPhoneNumber(PhoneNumberRequest req)
        {
            string phoneNumber = req.PhoneNumber;
            ResponseContact rc = new ResponseContact();


            var ct = from contact in contacts where HasPhoneNumber(contact.PhoneNumbers.PhoneNumber, phoneNumber) select contact;
            if (ct.Count() < 1) 
            {
                throw new WebFaultException(HttpStatusCode.NoContent);
            }
            else if (ct.Count() > 1) 
            {
                throw new WebFaultException(HttpStatusCode.Conflict);
            }
            Contact retContact = new Contact(ct.FirstOrDefault());

            if ((retContact != null) && (req.CustomAttribute != null))
            {
                if (req.CustomAttribute.Equals("overwrite"))
                {
                    retContact.CustomAttribute = "overwritten custom attribute";
                }
                else if (req.CustomAttribute.Equals("error_never_return"))
                {
                    Thread.Sleep(System.Threading.Timeout.Infinite);
                }
                else if (req.CustomAttribute.Equals("error_internal_server"))
                {
                    throw new WebFaultException<string>("Thrown because error_internal_server was the custom attribute", HttpStatusCode.InternalServerError);
                }
            }

            rc.Contact = retContact;

            return rc;
        }

        public ResponseAccount GetAccountByPhoneNumber(PhoneNumberRequest req)
        {
            ResponseAccount retVal = new ResponseAccount();
            String phoneNumber = req.PhoneNumber;

            var acc = from account in accounts where HasPhoneNumber(account.PhoneNumbers.PhoneNumber,phoneNumber) select account;
            if (acc.Count() < 1)
            {
                throw new WebFaultException(HttpStatusCode.NoContent);
            }
            else if (acc.Count() > 1)
            {
                throw new WebFaultException(HttpStatusCode.Conflict);
            }
            Account retAccount = new Account(acc.FirstOrDefault());

            if (retAccount != null && req.CustomAttribute != null && req.CustomAttribute.Equals("overwrite"))
            {
                retAccount.CustomAttribute = "overwritten custom attribute";
            }

            retVal.Account = retAccount;
            return retVal;
        }

        private bool HasPhoneNumber(List<PhoneNumber> phoneList, string phoneNumber) {
            foreach (PhoneNumber pn in phoneList)
            {
                if (digitsOnly(pn.Number).Equals(digitsOnly(phoneNumber)))
                {
                    return true;
                }
            }
            return false;
        }

        public ResponseAccount GetAccountByAccountNumber(AccountNumberRequest req)
        {
            var accountNumber = req.AccountNumber;
            var acc = from account in accounts where account.Number == accountNumber select account;
            if (acc.Count() < 1)
            {
                throw new WebFaultException(HttpStatusCode.NoContent);
            }
            else if (acc.Count() > 1)
            {
                throw new WebFaultException(HttpStatusCode.Conflict);
            }
            ResponseAccount retVal = new ResponseAccount();
            Account retAccount = new Account(acc.FirstOrDefault());

            if (retAccount != null && req.CustomAttribute != null && req.CustomAttribute.Equals("overwrite"))
            {
                retAccount.CustomAttribute = "overwritten custom attribute";
            }

            retVal.Account = retAccount;
            return retVal;
        }

        public ResponseAccount GetAccountByContactId(ContactIdRequest cidr)
        {
            var contactId = cidr.ContactId;
            var car = from ca in contactAccountRelationships where ca.ContactId.Equals(contactId) select ca;
            if (car.Count() < 1)
            {
                throw new WebFaultException(HttpStatusCode.NoContent);
            }
            else if (car.Count() > 1)
            {
                throw new WebFaultException(HttpStatusCode.Conflict);
            }
            ContactAccountRelationship rel = car.FirstOrDefault();
            AccountNumberRequest anr = new AccountNumberRequest {
                AccountNumber = rel.AccountNumber,
                CustomAttribute = cidr.CustomAttribute
            };
            return GetAccountByAccountNumber(anr);
        }

        public ResponseCase GetMostRecentOpenCaseByContactId(ContactIdRequest cidr)
        {
            ResponseCase rc = new ResponseCase();
            var caseList = from myCase in cases where myCase.ContactId == cidr.ContactId select myCase;
            if (caseList.Count() < 1)
            {
                throw new WebFaultException(HttpStatusCode.NoContent);
            }
            else if (caseList.Count() > 1)
            {
                throw new WebFaultException(HttpStatusCode.Conflict);
            }
            Case retCase = new Case(caseList.FirstOrDefault());
            rc.Case = retCase;
            if (retCase != null && cidr.CustomAttribute != null && cidr.CustomAttribute.Equals("overwrite"))
            {
                retCase.CustomAttribute = "overwritten custom attribute";
            }

            return rc;
        }

        private static string digitsOnly(String phoneNumber)
        {
            return NonDigitsExpression.Replace(phoneNumber, String.Empty);
        }
    }
}
