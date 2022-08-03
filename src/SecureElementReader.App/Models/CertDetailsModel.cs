using TaxCore.Libraries.Certificates;
using SecureElementReader.App.Enums;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace SecureElementReader.App.Models
{
    public class CertDetailsModel
    {
        public CertDetailsModel()
        {
            ErrorCodes = new List<string>();
        }

        public string Subject { get; set; }
        public string CommonName { get; internal set; }
        public string Organization { get; internal set; }
        public DateTime ExpiryDate { get; internal set; }
        public string OrganizationUnit { get; internal set; }
        public string StreetAddress { get; internal set; }
        public string RequestedBy { get; internal set; }
        public CertificateTypes CertificateType { get; internal set; }
        public string GivenName { get; internal set; }
        public string SurName { get; internal set; }
        public string State { get; internal set; }
        public bool PkiVerifyed { get; internal set; }
        public bool ReadPkiSuccess { get; internal set; }
        public bool SEVerify { get; internal set; }
        public bool SEReadSuccess { get; internal set; }
        public List<string> ErrorCodes { get; set; }
        public string ApiUrl { get; internal set; }
        public string Tin { get; internal set; }
        public string UniqueIdentifier { get; internal set; }
        public int? CardId { get; internal set; }
        public int CertificateId { get; internal set; }
        public string FriendlyName { get; internal set; }
        public X500DistinguishedName IssuerName { get; internal set; }
        public string SEVerificationInfo { get; set; }
        public string PKIVerificationInfo { get; set; }
    }
}
