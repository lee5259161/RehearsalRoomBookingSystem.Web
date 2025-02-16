namespace RehearsalRoomBookingSystem.Common.Interface
{
    /// <summary>
    /// interface ReturnResult
    /// </summary>
    public interface IEncryptHelper
    {
        /// <summary>Encrypt value use SHA256</summary>
        /// <param name="value">The value.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        string SHAEncrypt(string value, string salt);
    }
}
