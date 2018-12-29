using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace OPUPMS.Infrastructure.Common
{
    public class ImageFileManager
    {
        public static bool IsInSize(string path, int width, int height)
        {
            Image img = Image.FromFile(path);
            bool isIn = !(img.Width > width || img.Height > height);
            img.Dispose();
            return isIn;
        }

        public static bool SaveTo(string original, string savePath)
        {
            Image img = Image.FromFile(original);
            return SaveTo(img, Path.GetDirectoryName(savePath), Path.GetFileName(savePath));
        }

        public static bool SaveTo(Image image, string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fullPath = Path.Combine(path, fileName);
            string ext = Path.GetExtension(fullPath).ToLower();

            if (File.Exists(fullPath)) File.Delete(fullPath);

            Hashtable htmimes = new Hashtable();
            htmimes[".jpeg"] = "image/jpeg";
            htmimes[".jpg"] = "image/jpeg";
            htmimes[".png"] = "image/png";
            htmimes[".tif"] = "image/tiff";
            htmimes[".tiff"] = "image/tiff";
            htmimes[".bmp"] = "image/bmp";
            htmimes[".gif"] = "image/gif";

            ImageCodecInfo currentCodec = null;

            ImageCodecInfo[] codecInfos = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo info in codecInfos)
            {
                if (info.MimeType == htmimes[ext].ToString())
                {
                    currentCodec = info;
                    break;
                }
            }

            try
            {
                EncoderParameters param = new EncoderParameters(1);
                param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)90));
                image.Save(fullPath, currentCodec, param);
                param.Dispose();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="originalFile"></param>
        /// <param name="savePath"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="isCutOverRange">从内部适配长/宽，多的切除，取中段</param>
        /// <returns></returns>
        public string SaveImage(Image originalFile, string savePath, int maxWidth, int maxHeight, Watermark watermark, bool isCutOverRange = false)
        {
            if (originalFile == null || string.IsNullOrEmpty(savePath)) return string.Empty;

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var formalImage = SaveImage(originalFile, maxWidth, maxHeight, watermark, isCutOverRange);
            var fileName = DateTimeExtensions.GetRandomString(DateTime.Now) + ".jpeg";
            var file = SaveTo(formalImage, savePath, fileName);
            return Path.Combine(savePath, fileName);
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="originalFile"></param>
        /// <param name="savePath"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="isCutOverRange">从内部适配长/宽，多的切除，取中段</param>
        /// <returns></returns>
        public string SaveImage(string originalFile, string savePath, int maxWidth, int maxHeight, Watermark watermark, bool isCutOverRange = false)
        {
            if (string.IsNullOrEmpty(originalFile) || string.IsNullOrEmpty(savePath)) return string.Empty;

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var formalImage = SaveImage(Image.FromFile(originalFile), maxWidth, maxHeight, watermark, isCutOverRange);
            var fileName = DateTimeExtensions.GetRandomString(DateTime.Now) + ".jpeg";
            var file = SaveTo(formalImage, savePath, fileName);
            return Path.Combine(savePath, fileName);
        }

        /// <summary>
        /// 按比例格式化图片
        /// </summary>
        /// <param name="original"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="isCutOverRange">从内部适配长/宽，多的切除，取中段</param>
        /// <returns></returns>
        private Image SaveImage(Image original, int maxWidth, int maxHeight, Watermark waterMark, bool isCutOverRange = false)
        {
            if (original == null) return original;

            Image response = null;
            var x = 0d;
            var y = 0d;
            var width = 0d;
            var height = 0d;

            //get minimal image
            if (original.Width > maxWidth)
            {
                x = (original.Width - maxWidth) / 2d;
                width = maxWidth;
            }
            else
            {
                width = original.Width;
            }
            if (original.Height > maxWidth)
            {
                y = (original.Height - maxHeight) / 2.0d;
                height = maxHeight;
            }
            else
            {
                height = original.Height;
            }

            if (isCutOverRange)
            {
                //var crossWidth = width;
                //var crossHeight = height;
                var widthRatio = width / maxWidth * 1d;
                var heightRatio = height / maxHeight * 1d;
                if (widthRatio >= heightRatio)
                {
                    width = maxWidth * heightRatio;
                }
                else
                {
                    height = maxHeight * widthRatio;
                }
                x = (original.Width - width) / 2d;
                y = (original.Height - height) / 2d;
            }

            //用指定的大小和格式初始化 Bitmap 类的新实例
            //Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
            Bitmap bitmap = new Bitmap((int)width, (int)height);
            bitmap.SetResolution(original.VerticalResolution, original.HorizontalResolution);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.DrawImage(original, 0, 0, new Rectangle((int)x, (int)y, (int)width, (int)height), GraphicsUnit.Pixel);
            graphics.Save();
            graphics.Dispose();
            response = bitmap;
            if (waterMark != null) response = new ImageWatermarkMaker().MakeWatermark(bitmap, waterMark);
            return response;
        }
    }
}
