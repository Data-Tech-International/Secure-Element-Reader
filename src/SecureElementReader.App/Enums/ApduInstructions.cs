namespace SecureElementReader.App.Enums
{
    public enum ApduInstructions : byte
    {
        ExportCert = 0x04,
        PinVerify = 0x11,
        ExportInternalData = 0x12,
        AmountStatus = 0x14,
        SECommand = 0x40,
        StopAudit = 0x20,
        ExportPKICert = 0xCB,
    }
}
