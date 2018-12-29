using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Starts2000.Security
{
    public sealed class VerifyCode
    {
        const int FontSize = 24;
        const int Padding = 10;
        const int ChaosLinePoints = 5;
        static readonly Font Font = new Font("Arial", FontSize, FontStyle.Bold);
        static readonly VerifyCode _instace = new VerifyCode();

        readonly Random _random = new Random();

        private VerifyCode()
        {
        }

        public static VerifyCode Instace
        {
            get { return _instace; }
        }

        public char[] CreateCodes(int length)
        {
            char[] codes = new char[length];

            for (int i = 0; i < length; i++)
            {
                int num = _random.Next(0, 51);
                if (num < 26)
                {
                    num += 65;
                }
                else
                {
                    num += 71;
                }
                codes[i] = (char)num;
            }

            return codes;
        }

        public byte[] CreateVerifyCodeBuffer(char[] codes)
        {
            return CreateVerifyCodeBuffer(codes, true);
        }

        public byte[] CreateVerifyCodeBuffer(char[] codes, bool drawBorder)
        {
            return CreateVerifyCodeBuffer(codes, drawBorder, true);
        }

        /// <summary>
        /// 绘制拥有干扰线的验证码。
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="rotaeAngle"></param>
        /// <param name="drawBorder"></param>
        /// <param name="drawChaosPoints"></param>
        /// <param name="chaosPointCount"></param>
        /// <returns></returns>
        public byte[] CreateVerifyCodeBuffer(char[] codes,
           bool drawBorder, bool drawChaosPoints, int chaosPointCount)
        {
            return CreateVerifyCodeBuffer(codes, drawBorder, drawChaosPoints, true, chaosPointCount);
        }

        /// <summary>
        /// 绘制默认有200个噪点的验证码。
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="rotaeAngle"></param>
        /// <param name="drawBorder"></param>
        /// <param name="drawChaosPoints"></param>
        /// <param name="drawChaosLine"></param>
        /// <returns></returns>
        public byte[] CreateVerifyCodeBuffer(char[] codes, bool drawBorder, bool drawChaosLine)
        {
            return CreateVerifyCodeBuffer(codes, drawBorder, true, drawChaosLine, 200);
        }

        public byte[] CreateVerifyCodeBuffer(char[] codes,
            bool drawBorder, bool drawChaosPoints, bool drawChaosLine, int chaosPointCount)
        {
            int length = codes.Length;
            Color backColor = CreateColor(230, 255);
            Color[] colors = CreateColors(length);

            int width = (int)Math.Ceiling((Font.Size + 2) * codes.Length + Padding * 3);
            int height = Font.Height + Padding * 2;

            float x = Padding;
            float y = Padding;

            using (Bitmap bmp = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    g.Clear(backColor);

                    if (drawChaosPoints)
                    {
                        Chaos[] chaos = CreateChaos(width, height, chaosPointCount);
                        using (Pen pen = new Pen(Color.Empty))
                        {
                            foreach (Chaos c in chaos)
                            {
                                pen.Color = c.Color;
                                g.DrawRectangle(pen, c.X, c.Y, 1, 1);
                            }
                        }
                    }

                    using (SolidBrush brush = new SolidBrush(Color.Empty))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            g.TranslateTransform(x, y);
                            g.ScaleTransform(_random.Next(10, 20) / 15f, _random.Next(10, 20) / 15f);
                            g.RotateTransform(_random.Next(0, 20) - 10f);
                            brush.Color = colors[i];
                            g.DrawString(codes[i].ToString(), Font, brush, 0, 0);
                            g.ResetTransform();
                            x += (Font.Size + 2);
                        }
                    }

                    if (drawChaosLine)
                    {
                        using (Pen pen = new Pen(CreateColor(150, 30, 230), 3f))
                        {
                            g.DrawLines(pen, CreateChaosLinePoints(width, height));
                        }
                    }

                    if (drawBorder)
                    {
                        Color borderColor = CreateColor(100, 220);
                        using (Pen pen = new Pen(borderColor))
                        {
                            g.DrawRectangle(pen, 0, 0, width - 1, height - 1);
                        }
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Jpeg);
                    return ms.GetBuffer();
                }
            }
        }

        private Color CreateColor(int alpha, int min, int max)
        {
            int r = _random.Next(min, max);
            int g = _random.Next(min, max);
            int b = _random.Next(min, max);
            return Color.FromArgb(alpha, r, g, b);
        }

        private Color CreateColor(int min, int max)
        {
            return CreateColor(255, min, max);
        }

        private Color[] CreateColors(int length)
        {
            Color[] colors = new Color[length];
            for (int i = 0; i < length; i++)
            {
                colors[i] = CreateColor(0, 180);
            }
            return colors;
        }

        private PointF[] CreateChaosLinePoints(int width, int height)
        {
            float space = (width - 6) / (float)ChaosLinePoints;
            float x = 2f;
            PointF[] points = new PointF[ChaosLinePoints + 1];
            for (int i = 0; i <= ChaosLinePoints; i++)
            {
                points[i] = new PointF(x, _random.Next(6, height - 6));
                x += space;
            }
            return points;
        }

        private Chaos[] CreateChaos(int width, int height, int count)
        {
            Chaos[] chaos = new Chaos[count];
            for (int i = 0; i < count; i++)
            {
                chaos[i] = new Chaos(
                    _random.Next(2, width - 2),
                    _random.Next(2, height - 2),
                    CreateColor(20, 240));
            }
            return chaos;
        }

        struct Chaos
        {
            int _x;
            int _y;
            Color _color;

            internal int X
            {
                get { return _x; }
                set { _x = value; }
            }

            internal int Y
            {
                get { return _y; }
                set { _y = value; }
            }

            internal Color Color
            {
                get { return _color; }
                set { _color = value; }
            }

            public Chaos(int x, int y, Color color)
            {
                _x = x;
                _y = y;
                _color = color;
            }
        }
    }
}
