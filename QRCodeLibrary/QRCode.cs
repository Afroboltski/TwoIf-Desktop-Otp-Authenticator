using System.Drawing;
using ZXing;
using ZXing.Windows.Compatibility;

namespace QRCodeLibrary
{
    public class QRCode
    {
        private Bitmap _qrCodeBitmap;
        private Result _qrCodeDecodeResult;
        private string _qrCodeText;

        private QRCode(Bitmap bitmap, Result result, string text)
        {
            _qrCodeBitmap = bitmap;
            _qrCodeDecodeResult = result;
            _qrCodeText = text;
        }

        public Bitmap Bitmap { get { return _qrCodeBitmap; } }
        public Result RawResult { get { return _qrCodeDecodeResult; } }
        public string Data { get { return _qrCodeText; } }

        public static QRCode LoadFromImage(string qrCodePath)
        {
            if (!File.Exists(qrCodePath))
            {
                throw new IOException("Could not find/access the specified file \"" + qrCodePath + "\".");
            }
            Bitmap qrCodeBitmap = new Bitmap(qrCodePath);
            return LoadFromBitmap(qrCodeBitmap);
        }

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

            QRCode qrCode = new QRCode(qrCodeBitmap, result, qrText);
            return qrCode;
        }
    }
}
