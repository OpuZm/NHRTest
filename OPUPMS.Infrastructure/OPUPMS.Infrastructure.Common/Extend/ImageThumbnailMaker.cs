using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace OPUPMS.Infrastructure.Common
{
    public class ImageThumbnailMaker
    {
        public void MakeThumbnail(string path, int s_width, int s_height, string saveFilePath)
        {
            if (string.IsNullOrEmpty(saveFilePath) || string.IsNullOrEmpty(path)) return;
            Image img = Image.FromFile(path);
            Image thumb = MakeThumbnail(img, s_width, s_height);
            img.Dispose();
            var savePath = Path.GetDirectoryName(saveFilePath);
            var saveFile = Path.GetFileName(saveFilePath);
            ImageFileManager.SaveTo(thumb, savePath, saveFile);
            thumb.Dispose();
        }

        /// <summary>
        /// 返回 保存的缩略文件名 原名!<s_width>x<s_height>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="s_width"></param>
        /// <param name="s_height"></param>
        /// <returns></returns>
        public string MakeThumbnail(string path, int s_width, int s_height)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            Image img = Image.FromFile(path);
            Image thumb = MakeThumbnail(img, s_width, s_height);
            int width = thumb.Width;
            int height = thumb.Height;
            var savePath = Path.GetDirectoryName(path);
            var saveFile = Path.GetFileNameWithoutExtension(path) + "tmp" + Path.GetExtension(path);
            ImageFileManager.SaveTo(thumb, savePath, saveFile);
            img.Dispose();
            thumb.Dispose();
            var filePath = path + "!" + width.ToString() + "x" + height.ToString(); //DateTime.Now.ToString("yyyyMMddHHmmssffffff") + new Random(DateTime.Now.Millisecond).ToString() + "!" + width.ToString() + "x" + height.ToString();
            File.Move(Path.Combine(savePath, saveFile), filePath);
            return filePath;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="original"></param>
        /// <param name="s_width"></param>
        /// <param name="s_height"></param>
        /// <returns></returns>
        internal Image MakeThumbnail(Image original, int s_width, int s_height)
        {
            int width = 0;
            int height = 0;
            float widthRatio = s_width * 1.0f / original.Width;
            float heightRatio = s_height * 1.0f / original.Height;
            if (widthRatio >= 1 && heightRatio >= 1) return original;
            if (widthRatio > heightRatio)
            {
                height = s_height;
                width = (int)Math.Round(heightRatio * original.Width);
            }
            else
            {
                height = (int)Math.Round(widthRatio * original.Height);
                width = s_width;
            }

            //用指定的大小和格式初始化 Bitmap 类的新实例
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            bitmap = new Bitmap(original, new Size(width, height));
            return bitmap;

            ////从指定的 Image 对象创建新 Graphics 对象
            //Graphics graphics = Graphics.FromImage(bitmap);

            ////清除整个绘图面并以透明背景色填充
            //graphics.Clear(Color.Transparent);

            //graphics.SmoothingMode = SmoothingMode.HighQuality;
            //graphics.InterpolationMode = InterpolationMode.High;

            ////在指定位置并且按指定大小绘制 原图片 对象
            //graphics.DrawImage(original, new Rectangle(0, 0, width, height));
            //graphics.Save();
            //graphics.Dispose();
            //return bitmap;
        }

        public string UploadImage(HttpPostedFileBase stream, int s_width, int s_height, string saveFilePath, bool isBuildThunb)
        {
            string paths = string.Empty;
            if (!string.IsNullOrEmpty(saveFilePath))
            {
                string fileNewName = string.Empty;
                string fileNameDate = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + new Random(3).Next().ToString();
                string fileExt = string.Empty;
                Image imgBase = null;
                Image img = Image.FromStream(stream.InputStream);
                if (img.Width > 1920)
                {
                    int imgBaseHeight = Convert.ToInt32(((1920 * 1.0) / img.Width) * img.Height);    //原图 宽度1000
                    imgBase = MakeThumbnail(img, img.Width, imgBaseHeight);
                }
                else
                {
                    imgBase = img;
                }
                var savePath = HttpContext.Current.Server.MapPath(saveFilePath);
                fileExt = Path.GetExtension(stream.FileName).ToLower();
                fileNewName = fileNameDate + fileExt;
                ImageFileManager.SaveTo(imgBase, savePath, fileNewName);
                paths = saveFilePath + fileNewName;
                if (isBuildThunb)
                {
                    int imgThunbHeight = Convert.ToInt32(((s_width * 1.0) / img.Width) * img.Height);    //缩略图
                    Image thumb = MakeThumbnail(imgBase, s_width, imgThunbHeight);
                    fileNewName = fileNameDate + fileExt + s_width.ToString() + "x" + fileExt;
                    ImageFileManager.SaveTo(thumb, savePath, fileNewName);
                    thumb.Dispose();
                }
                imgBase.Dispose();
                img.Dispose();
            }
            return paths;
        }

        public List<string> UploadImage(List<HttpPostedFileBase> stream, int s_width, int s_height, string saveFilePath, bool isBuildThunb)
        {
            var savePath = HttpContext.Current.Server.MapPath(saveFilePath); //saveFilePath;
            List<string> paths = new List<string>();
            if (!string.IsNullOrEmpty(saveFilePath))
            {
                string fileNewName = string.Empty;
                string fileExt = string.Empty;
                string fileNameDate = string.Empty;
                foreach (var item in stream)
                {
                    fileNameDate = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                    Image imgBase = null;
                    Image img = Image.FromStream(item.InputStream);
                    if (img.Width > 1920)
                    {
                        int imgBaseHeight = Convert.ToInt32(((1920 * 1.0) / img.Width) * img.Height);    //原图 宽度1000
                        imgBase = MakeThumbnail(img, img.Width, imgBaseHeight);
                    }
                    else
                    {
                        imgBase = img;
                    }
                    fileExt = Path.GetExtension(item.FileName).ToLower();
                    fileNewName = fileNameDate + fileExt;
                    ImageFileManager.SaveTo(imgBase, savePath, fileNewName);
                    paths.Add(saveFilePath + fileNewName);
                    if (isBuildThunb)
                    {
                        int imgThunbHeight = Convert.ToInt32(((s_width * 1.0) / img.Width) * img.Height);    //缩略图
                        Image thumb = MakeThumbnail(imgBase, s_width, imgThunbHeight);
                        fileNewName = fileNameDate + fileExt + s_width.ToString() + "x" + fileExt;
                        ImageFileManager.SaveTo(thumb, savePath, fileNewName);
                        thumb.Dispose();
                    }
                    imgBase.Dispose();
                    img.Dispose();
                }
            }
            return paths;
        }

        //public Task<string> Fileupload(HttpPostedFileBase stream, string saveFilePath)
        //{
        //    return Task.Factory.StartNew<string>(() =>
        //    {
        //        string paths = string.Empty;
        //        if (!string.IsNullOrEmpty(saveFilePath))
        //        {
        //            string fileNewName = string.Empty;
        //            string fileNameDate = Common.DateTimeExtensions.GetRandomString(DateTime.Now); //DateTime.Now.ToString("yyyyMMddHHmmssffffff");
        //            string fileExt = string.Empty;
        //            var savePath = HttpContext.Current.Server.MapPath(saveFilePath);
        //            if (!Directory.Exists(savePath))
        //            {
        //                Directory.CreateDirectory(savePath);
        //            }
        //            fileExt = Path.GetExtension(stream.FileName).ToLower();
        //            fileNewName = fileNameDate + fileExt;
        //            stream.SaveAs(savePath + fileNewName);
        //            paths = saveFilePath + fileNewName;
        //        }
        //        return paths;
        //    });
        //}

        public string Fileupload(HttpPostedFileBase stream, string saveFilePath)
        {
            string paths = string.Empty;
            if (!string.IsNullOrEmpty(saveFilePath))
            {
                string fileNewName = string.Empty;
                string fileNameDate = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                string fileExt = string.Empty;
                var savePath = HttpContext.Current.Server.MapPath(saveFilePath);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                fileExt = Path.GetExtension(stream.FileName).ToLower();
                fileNewName = fileNameDate + fileExt;
                stream.SaveAs(savePath + fileNewName);
                paths = saveFilePath + fileNewName;
            }
            return paths;
        }
    }
}
