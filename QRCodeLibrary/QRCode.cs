using System.Drawing;
using ZXing;
using ZXing.Windows.Compatibility;

namespace QRCodeLibrary
{
    public class QRCode
    {
        private Result _qrCodeDecodeResult;
        private string _qrCodeText;

        private QRCode(Result result, string text)
        {
            _qrCodeDecodeResult = result;
            _qrCodeText = text;
        }

        public Result RawResult { get { return _qrCodeDecodeResult; } }
        public string Data { get { return _qrCodeText; } }

        public static QRCode LoadFromImage(string qrCodePath)
        {
            if (!File.Exists(qrCodePath))
            {
                throw new IOException("Could not find/access the specified file \"" + qrCodePath + "\".");
            }
            Bitmap qrCodeBitmap = new Bitmap(qrCodePath);
            try
            {
                return LoadFromBitmap(qrCodeBitmap);
            }
            finally
            {
                qrCodeBitmap.Dispose();
            }
        }

        // Note: Caller responsible for disposing bitmap
        public static QRCode LoadFromBitmap(Bitmap qrCodeBitmap)
        {
            if(qrCodeBitmap== null)
            {
                throw new ArgumentNullException(nameof(qrCodeBitmap));
            }
            BarcodeReader reader = new BarcodeReader();
            reader.AutoRotate = true;
            reader.Options.TryHarder = true;

            Result result = reader.Decode(qrCodeBitmap);
            // do something with the result
            if (result == null)
            {
                throw new ApplicationException("Failed to decode QR code image.");
            }

            string qrText = result.Text;

            QRCode qrCode = new QRCode(result, qrText);
            return qrCode;
        }
    }
}
