using ObiletApp.Core.Interfaces;
using QRCoder;
using System;

namespace ObiletApp.Infrastructure.Services
{
    public class QrCodeService : IQrCodeService
    {
        public byte[] GenerateQrCode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Array.Empty<byte>();

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        public string GenerateQrCodeBase64(string text)
        {
            var bytes = GenerateQrCode(text);
            return Convert.ToBase64String(bytes);
        }
    }
}
