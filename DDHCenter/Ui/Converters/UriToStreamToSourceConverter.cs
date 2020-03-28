using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DDHCenter.Ui.Converters
{
    public class UriToStreamToSourceConverter : BaseValueConverter<UriToStreamToSourceConverter>
    {
        public UriToStreamToSourceConverter()
        {

        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string fileUri = (string)value;
            BitmapSource imgSource = null;
            if (fileUri == null || fileUri.Length == 0)
            {

                if (parameter != null)
                    return (new BitmapImage(new Uri((string)parameter, UriKind.Relative)));
                //
                return (new BitmapImage(new Uri("/DDHCenter;component/Ui/Images/socio.jpg", UriKind.Relative)));
            }

            using (var fileStream = new FileStream(fileUri, FileMode.Open, FileAccess.Read))
            {
                imgSource = bitmapSource(fileStream);

                if (imgSource.Width != imgSource.PixelWidth && imgSource.Height != imgSource.PixelHeight)
                {
                    imgSource = ConvertBitmapTo96DPI(imgSource);
                }

                if (imgSource.Width > imgSource.Height)
                {
                    imgSource = cropAlign(imgSource, imgSource.Height, imgSource.Height, "center", "middle");
                }

                if (imgSource.Height > imgSource.Width)
                {
                    imgSource = cropAlign(imgSource, imgSource.Width, imgSource.Width, "center", "top");
                }
                return imgSource;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public BitmapSource bitmapSource(FileStream fileStream)
        {
            if (fileStream == null) return null;
            BitmapImage bmps = null;
            BitmapImage bmpsFinal = null;
            bmps = new BitmapImage();
            bmps.BeginInit();
            bmps.CacheOption = BitmapCacheOption.OnLoad;
            bmps.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bmps.StreamSource = fileStream;
            bmps.Rotation = Rotation.Rotate0;
            bmps.EndInit();
            if (bmps.PixelWidth > 200)
            {
                //
                fileStream.Position = 0;
                //
                bmpsFinal = new BitmapImage();
                bmpsFinal.BeginInit();
                bmpsFinal.CacheOption = BitmapCacheOption.OnLoad;
                bmpsFinal.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bmpsFinal.DecodePixelWidth = 300;
                if (bmps.PixelHeight <= 300)
                    bmpsFinal.DecodePixelHeight = bmps.PixelHeight;
                else
                    bmpsFinal.DecodePixelHeight = bmps.PixelHeight * (300 / bmps.PixelWidth);
                bmpsFinal.StreamSource = fileStream;
                bmpsFinal.Rotation = Rotation.Rotate0;
                bmpsFinal.EndInit();
                bmps = null;
                fileStream = null;
                return bmpsFinal;
            }

            fileStream = null;
            return bmps;
        }

        public BitmapSource bitmapSource(byte[] imageBuffer)
        {
            if (imageBuffer == null) return null;
            BitmapImage bmps = null;
            MemoryStream mem;
            // Only load thumbnails
            mem = new MemoryStream(imageBuffer);
            bmps = new BitmapImage();
            bmps.BeginInit();
            bmps.CacheOption = BitmapCacheOption.None;
            bmps.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bmps.DecodePixelWidth = 200;
            bmps.DecodePixelHeight = 200;
            bmps.StreamSource = mem;
            bmps.Rotation = Rotation.Rotate0;
            bmps.EndInit();
            mem = null;
            imageBuffer = null;
            return bmps;
        }

        public static byte[] ImageToByte(BitmapSource imageSource)
        {
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public static BitmapSource ConvertBitmapTo96DPI(BitmapSource bitmapImage)
        {
            double dpi = 96;
            int width = bitmapImage.PixelWidth;
            int height = bitmapImage.PixelHeight;

            int stride = width * bitmapImage.Format.BitsPerPixel;
            byte[] pixelData = new byte[stride * height];
            bitmapImage.CopyPixels(pixelData, stride, 0);

            return BitmapSource.Create(width, height, dpi, dpi, bitmapImage.Format, null, pixelData, stride);
        }

        private BitmapSource cropAlign(BitmapSource image, double cropWidth, double cropHeight, string horizontalAlign = "center", string verticalAlign = "middle")
        {
            double width = image.Width;
            double height = image.Height;
            double[] horizontalAlignPixels = calculatePixelsForAlign(width, cropWidth, horizontalAlign);
            double[] verticalAlignPixels = calculatePixelsForAlign(height, cropHeight, verticalAlign);
            Int32Rect rcFrom = new Int32Rect();
            rcFrom.X = (int)horizontalAlignPixels[0];
            rcFrom.Y = (int)verticalAlignPixels[0];
            rcFrom.Width = (int)horizontalAlignPixels[1];
            rcFrom.Height = (int)verticalAlignPixels[1];
            BitmapSource bs = new CroppedBitmap(image, rcFrom);
            return bs;
        }

        private double[] calculatePixelsForAlign(double imageSize, double cropSize, string align)
        {
            double[] pixels = new double[2];
            switch (align)
            {
                case "left":
                case "top":
                    pixels[0] = 0;
                    pixels[1] = Math.Min(cropSize, imageSize);
                    return pixels;
                case "right":
                case "bottom":
                    pixels[0] = Math.Max(0, imageSize - cropSize);
                    pixels[1] = Math.Min(cropSize, imageSize);
                    return pixels;
                case "center":
                case "middle":
                    pixels[0] = Math.Max(0, Math.Floor((imageSize / 2) - (cropSize / 2)));
                    pixels[1] = Math.Min(cropSize, imageSize);
                    return pixels;
                default:
                    pixels[0] = 0;
                    pixels[1] = imageSize;
                    return pixels;
            }
        }
    }
}
