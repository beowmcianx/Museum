using QRCoder;
using System.Drawing;
using System.IO;

public static class QrHelper
{
    public static byte[] Generate(string text)
    {
        var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(data);

        using (var bitmap = qrCode.GetGraphic(20))
        using (var ms = new MemoryStream())
        {
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
