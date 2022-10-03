using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace TMall.util
{
    public class Captcha
    {
        // 生成隨機驗證碼 和 對應的圖片
        private const int height = 40;
        private const int width = 100;
        private const int code_len = 4;// 驗證碼字元的個數
        public static void GetCaptcha(out byte[] imgBytes, out string code)
        {
            Bitmap bmp = new Bitmap(width, height);// 初始化圖片
            Random random = new Random();
            getRandom(out code, random);

            Graphics graphics = Graphics.FromImage(bmp);
            graphics.Clear(Color.Snow);//填充背景
            for (int i = 0; i < 30; ++i)
            {
                // 畫干擾線
                graphics.DrawLine(new Pen(Color.Silver), random.Next(width), random.Next(height), random.Next(width), random.Next(height));
            }

            //預選的顏色
            Color[] color = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //預選的字體
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋體" };
            for (int i = 0; i < code_len; ++i)
            {
                Font f = new Font(font[random.Next(4)], 15, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(color[random.Next(6)]);
                graphics.DrawString(code[i].ToString(), f, b, i * 24 + 3, 2);
            }

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imgBytes = ms.ToArray();
        }


        // 生成4位驗證碼
        private static char[] allChar = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static void getRandom(out string code, Random random)
        {
            List<char> allChar = new List<char>();
            for (int i = 0; i < 10; ++i) allChar.Add(Convert.ToChar((Convert.ToInt32('0') + i)));
            for (int i = 0; i < 26; ++i)
            {
                allChar.Add(Convert.ToChar(Convert.ToInt32('a') + i));
                allChar.Add(Convert.ToChar(Convert.ToInt32('A') + i));
            }
            code = "";
            for (int i = 0; i < code_len; ++i)
            {// 隨機取一個字元
                code += allChar[random.Next(61)];
            }
        }
    }
}
