namespace SecureElementReader.App.Enpoints
{
    public class EndpointUrls
    {
        public const string NotifyCommandProcessed = "api/sdc/UploadCommandStatus";
        public const string GetPendingCommands = "api/sdc/GetOutstandingCommands";
        public const string SubmitInternalData = "api/SecureElements/Audit";
    }
}
