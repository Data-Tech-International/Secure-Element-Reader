namespace SecureElementReader.Enums
{
    public enum ApduP1
    {
        /// <summary>
        /// Default P1 0x04
        /// </summary>
        Default = 0x04,

        /// <summary>
        /// P1 used for Get Response if there is more then 256 bytes for T0 protocol
        /// </summary>
        GetResponse = 0x00,

        PKIExport = 0xA0
    }
}
