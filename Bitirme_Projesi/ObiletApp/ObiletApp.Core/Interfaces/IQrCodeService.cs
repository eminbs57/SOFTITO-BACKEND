namespace ObiletApp.Core.Interfaces
{
    public interface IQrCodeService
    {
        byte[] GenerateQrCode(string text);
        string GenerateQrCodeBase64(string text);
    }
}
