using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace OPUPMS.Infrastructure.Common
{
    /// <summary>
    /// 水印类型
    /// </summary>
    public enum WatermarkType
    {
        Text = 0,
        Image = 1,
    }

    public abstract class Watermark
    {
        protected WatermarkType _watermarkType;
        protected ContentAlignment _alignment;
        //protected bool _isFixedSize = false;    //是否为固定大小
        protected Size _size = new Size();  //大小
        protected float _ratio = 0;   //非固定大小时的比率，0-1
        protected float _marginPercentage;   //边距 1-100
        protected float _alpha; //透明度 0-1

        public Watermark() { _alpha = 1; _ratio = 0.2f; MarginPercentage = 3f; }

        public virtual WatermarkType WatermarkType { get { return _watermarkType; } }
        public virtual ContentAlignment Alignment { get { return _alignment; } set { _alignment = value; } }
        //public bool IsFixedSize { get; set; }
        internal Size Size { get { return _size; } }
        /// <summary>
        /// 与源图的比例，小于0不加水印
        /// </summary>
        public float Ratio { get { return _ratio; } set { _ratio = value; } }
        public float MarginPercentage { get { return _marginPercentage; } set { _marginPercentage = value; } }
        public float Alpha { get { return _alpha; } set { _alpha = value > 1 ? 1 : value < 0 ? 0 : value; } }

        internal void SetSize(Size size)
        {
            this._size = size;
        }
    }

    public class TextWatermark : Watermark
    {
        protected float _horizontalResolution;  //横向分辨率
        protected float _verticalResolution;    //纵向分辨率
        //private Font _font;
        private Color _textColor = Color.Gray;

        public TextWatermark() : base()
        {
            _watermarkType = WatermarkType.Text;
            //_font = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 1);

        }
        public string Text { get; set; }
        public Color TextColor { get; set; }

        //public Font Font { get { return _font; } set { _font = value; } }
        internal float HorizontalResolution { get { return _horizontalResolution; } set { _horizontalResolution = value; } }
        internal float VerticalResolution { get { return _verticalResolution; } set { _verticalResolution = value; } }

    }

    public class ImageWatermark : Watermark
    {
        public ImageWatermark() : base()
        {
            _watermarkType = WatermarkType.Image;
        }
        public Image Image { get; set; }
    }

    public class ImageWatermarkMaker
    {
        public ImageWatermarkMaker()
        {

        }

        /// <summary>
        /// 设置水印
        /// </summary>
        /// <param name="original"></param>
        /// <param name="watermark"></param>
        /// <returns></returns>
        public Image MakeWatermark(Image original, Watermark watermark)
        {
            //Bitmap b = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            //b.SetResolution(original.HorizontalResolution, original.VerticalResolution);

            // 不加水印
            if (watermark.Ratio < 0) return original;
            //当ratio大于1时，加的部分水印会在图片外面(不显示)
            Size markSize = new Size((int)Math.Round(watermark.Ratio * original.Width), (int)Math.Round(watermark.Ratio * original.Height));
            watermark.SetSize(markSize);

            switch (watermark.WatermarkType)
            {
                case WatermarkType.Text:
                    //添加文字水印
                    var textWm = watermark as TextWatermark;
                    if (!string.IsNullOrEmpty(textWm.Text))
                    {
                        textWm.HorizontalResolution = original.HorizontalResolution; // b.HorizontalResolution;
                        textWm.VerticalResolution = original.VerticalResolution;// b.VerticalResolution;
                        var textMark = this.CreateTextWatermark(textWm);
                        ImageWatermark imageWm = new ImageWatermark()
                        {
                            Alignment = textWm.Alignment,
                            Alpha = textWm.Alpha,
                            Image = textMark,
                            MarginPercentage = textWm.MarginPercentage,
                            Ratio = textWm.Ratio,
                        };
                        imageWm.SetSize(textWm.Size);
                        return MakeImageWatermark(original, imageWm);
                    }
                    break;
                case WatermarkType.Image:
                    return MakeImageWatermark(original, watermark as ImageWatermark);
                    break;
                default:
                    break;
            }
            return original;
        }

        /// <summary>
        /// 添加文字水印
        /// </summary>
        /// <param name="watermark"></param>
        protected Image CreateTextWatermark(TextWatermark watermark)
        {
            var size = watermark.Size;
            var text = watermark.Text;
            Bitmap markBmp = null;

            //画布大小
            Bitmap floatBmp = new Bitmap((int)size.Width, (int)size.Height, PixelFormat.Format32bppArgb);
            floatBmp.SetResolution(watermark.HorizontalResolution, watermark.VerticalResolution);
            Graphics fg = Graphics.FromImage(floatBmp);

            //适合图片的文字大小
            int[] sizes = new int[] { 32, 30, 28, 26, 24, 22, 20, 18, 16, 14, 12, 10, 8, 6, 4 };
            Font font = null;
            SizeF sizeF = new SizeF();
            for (int i = 0; i < sizes.Length; i++)
            {
                font = new Font(new FontFamily(GenericFontFamilies.Monospace), sizes[i]);
                sizeF = fg.MeasureString(text, font);
                if (sizeF.Width <= size.Width && sizeF.Height <= size.Height)
                    break;
            }

            if (sizeF.Width > 0 && sizeF.Height > 0)
            {
                markBmp = new Bitmap((int)sizeF.Width + 1, (int)sizeF.Height + 1, PixelFormat.Format32bppArgb);
                Graphics markGraphics = Graphics.FromImage(markBmp);
                PointF pt = new PointF(0, 0);
                Brush shadowBrush = new SolidBrush(Color.FromArgb(255, watermark.TextColor));
                markGraphics.DrawString(text, font, shadowBrush, pt.X + 1, pt.Y + 1);
                markGraphics.SmoothingMode = SmoothingMode.AntiAlias;   //高清水印
                markGraphics.DrawString(text, font, shadowBrush, pt.X, pt.Y);
                shadowBrush.Dispose();
                markGraphics.Save();
                markGraphics.Dispose();
            }
            return markBmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="watermark"></param>
        /// <returns></returns>
        protected Image MakeImageWatermark(Image original, ImageWatermark watermark)
        {
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap, };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            var alpha = watermark.Alpha;
            float[][] colorMatrixElements = {
                new float[] {1.0f,0.0f,0.0f,0.0f,0.0f },    //red
                new float[] {0.0f,1.0f,0.0f,0.0f,0.0f },    //green
                new float[] {0.0f,0.0f,1.0f,0.0f,0.0f },    //blue
                new float[] {0.0f,0.0f,0.0f,alpha,0.0f },    //alpha
                new float[] {0.0f,0.0f,0.0f,0.0f,1.0f },    //P
            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;
            int markWidth = 0;
            int markHeight = 0;
            int estimatedWidth = (int)Math.Round(original.Width * watermark.Ratio);
            int estimatedHeight = (int)Math.Round(original.Height * watermark.Ratio);
            //watermark.Image.Width > original.Width * watermark.Ratio ? (int)Math.Round(original.Width * watermark.Ratio) : watermark.Image.Width;
            // watermark.Image.Height > original.Height * watermark.Ratio ? (int)Math.Round(original.Height * watermark.Ratio) : watermark.Image.Height;
            if (watermark.Image.Width > estimatedWidth)
            {
                if (watermark.Image.Height > estimatedHeight)
                {
                    if (estimatedHeight > estimatedWidth)
                    {
                        markWidth = estimatedWidth;
                        markHeight = watermark.Image.Height * estimatedWidth / watermark.Image.Width;
                        if (markHeight > estimatedHeight)
                        {
                            markWidth = estimatedHeight * estimatedHeight / markHeight;
                            markHeight = estimatedHeight;
                        }
                    }
                    else
                    {
                        markWidth = watermark.Image.Width * estimatedHeight / watermark.Image.Height;
                        markHeight = estimatedHeight;
                        if (markWidth > estimatedWidth)
                        {
                            markHeight = estimatedHeight * estimatedWidth / markWidth;
                            markWidth = estimatedWidth;
                        }
                    }
                }
                else
                {
                    markWidth = estimatedWidth;
                    markHeight = watermark.Image.Height * estimatedWidth / watermark.Image.Width;
                }
            }
            else
            {
                if (watermark.Image.Height > estimatedHeight)
                {
                    markWidth = watermark.Image.Width * estimatedHeight / watermark.Image.Height;
                    markHeight = estimatedHeight;
                }
                else
                {
                    markWidth = watermark.Image.Width;
                    markHeight = watermark.Image.Height;
                }
            }
            int marginX = (int)Math.Round(watermark.MarginPercentage / 100 * original.Width);
            int marginY = (int)Math.Round(watermark.MarginPercentage / 100 * original.Height);
            switch (watermark.Alignment)
            {
                case ContentAlignment.TopLeft:
                    xpos = marginX;
                    ypos = marginY;
                    break;
                case ContentAlignment.TopCenter:
                    xpos = (original.Width - markWidth) / 2;
                    ypos = marginY;
                    break;
                case ContentAlignment.TopRight:
                    xpos = (original.Width - markWidth) - marginX;
                    ypos = marginY;
                    break;
                case ContentAlignment.MiddleLeft:
                    xpos = marginX;
                    ypos = (original.Height - markHeight) / 2;
                    break;
                case ContentAlignment.MiddleCenter:
                    xpos = (original.Width - markWidth) / 2;
                    ypos = (original.Height - markHeight) / 2;
                    break;
                case ContentAlignment.MiddleRight:
                    xpos = (original.Width - markWidth) - marginX;
                    ypos = (original.Height - markHeight) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                    xpos = marginX;
                    ypos = (original.Height - markHeight) - marginY;
                    break;
                case ContentAlignment.BottomCenter:
                    xpos = (original.Width - markWidth) / 2;
                    ypos = (original.Height - markHeight) - marginY;
                    break;
                case ContentAlignment.BottomRight:
                    xpos = (original.Width - markWidth) - marginX;
                    ypos = (original.Height - markHeight) - marginY;
                    break;
                default:
                    break;
            }

            Bitmap combined = new Bitmap(original);

            // gdi+图片
            Graphics g = Graphics.FromImage(combined);
            // AntiAlias    指定消除锯齿的呈现。  
            // Default      指定不消除锯齿。  
            // HighQuality  指定高质量、低速度呈现。  
            // HighSpeed    指定高速度、低质量呈现。  
            // Invalid      指定一个无效模式。  
            // None         指定不消除锯齿。 
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.InterpolationMode = InterpolationMode.High;
            //g.Clear(Color.White);
            //g.DrawImage(combined, new Rectangle(0, 0, combined.Width, combined.Height));
            g.DrawImage(watermark.Image, new Rectangle() { X = xpos, Y = ypos, Height = markHeight, Width = markWidth, }, 0, 0, watermark.Image.Width, watermark.Image.Height, GraphicsUnit.Pixel);
            imageAttributes.Dispose();
            g.Dispose();
            return combined;
        }
    }
}
