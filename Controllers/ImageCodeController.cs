using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace qiushi.Web.Controllers
{
    public class ImageCodeController : Controller
    {
        // GET: ImageCode
        public ActionResult Index()
        {
            Bitmap image = new Bitmap(200, 50);

            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);

            Random random = new Random();
            //画图片的背景噪音点
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
            }

            //画图片的背景噪音线 
            for (int i = 0; i < 2; i++)
            {
                int x1 = 0;
                int x2 = image.Width;
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
               
                    g.DrawLine(new Pen(Color.Black, 2), x1, y1, x2, y2);
               
            }

            //g.FillRectangle(Brushes.Red, 2, 2, 100, 31);


            // var brushColor = Color.Frandom.Next(image.Width / 2);romArgb(random.Next(255), random.Next(255), random.Next(255)));
            // var codeString = String.Concat((GenerateChineseWords(4)));

            //var fontFamilys = new String[] { "Arial", "Arial Black", "Arial Narrow", "Verdana", "Georgia", "Times New Roman", "Trebuchet MS",
            //                                 "Courier New", "Impact", "Comic Sans MS", "Tahoma", "Courier", "Lucida Sans Unicode", "Lucida Console",
            //                                 "Garamond", "MS Sans Serif", "MS Serif", "Palatino Linotype", "Symbol", "Bookman Old Style",
            //                                 "Hiragino Sans GB", "Microsoft YaHei", "WenQuanYi Micro Hei"};

            var fontFamilys = new String[] { "Arial", "Arial Black", "Arial Narrow", "Verdana", "Georgia", "Times New Roman", "Trebuchet MS",
                                             "Courier New", "Impact", "Comic Sans MS", "Tahoma", "Courier", "Lucida Sans Unicode", "Lucida Console",
                                             "Garamond", "MS Sans Serif", "MS Serif", "Palatino Linotype", "Symbol", "Bookman Old Style",
                                             "Hiragino Sans GB", "Microsoft YaHei", "WenQuanYi Micro Hei"};



            var words = GetSimpleWords(4);
            Session["CheckCode"] = "";
            for (int i=0; i<words.Count; i++)
            {
                var codeString = words[i];
                Session["CheckCode"] += codeString;

                random = new Random((int)DateTime.Now.Ticks + i);
                var fontFamily = fontFamilys[random.Next(fontFamilys.Length)];
                var fontSize = random.Next(20) / 2 + 15;

                var xPotint = random.Next(image.Width / 10) + (image.Width / 4) * i;
                var yPoint = random.Next(image.Height / 4);

                g.DrawString(codeString, new Font(fontFamily, fontSize), Brushes.Black, new PointF(xPotint, yPoint));
            }
           // g.DrawString(codeString, new Font(fontFamily, fontSize), Brushes.Black, new PointF(xPotint, yPoint));

            //画图片的前景噪音点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                
                image.SetPixel(x, y, Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));

            }

            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();
            return File(ms.ToArray(), "image/jpeg");
        }

        private List<string> GenerateChineseWords(int count)
        {
            List<string> chineseWords = new List<string>();
            Random rm = new Random();
            Encoding gb = Encoding.GetEncoding("gb2312");

            for (int i = 0; i < count; i++)
            {
                // 获取区码(常用汉字的区码范围为16-55)  
                int regionCode = rm.Next(16, 56);
                // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)  
                int positionCode;
                if (regionCode == 55)
                {
                    // 55区排除90,91,92,93,94  
                    positionCode = rm.Next(1, 90);
                }
                else
                {
                    positionCode = rm.Next(1, 95);
                }

                // 转换区位码为机内码  
                int regionCode_Machine = regionCode + 160;// 160即为十六进制的20H+80H=A0H  
                int positionCode_Machine = positionCode + 160;// 160即为十六进制的20H+80H=A0H  

                // 转换为汉字  
                byte[] bytes = new byte[] { (byte)regionCode_Machine, (byte)positionCode_Machine };
                chineseWords.Add(gb.GetString(bytes));
            }

            return chineseWords;
        }

        private object[] CreateChiCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素   
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            Random rnd = new Random();
            //定义一个object数组用来存放随机生成的汉字  
            object[] bytes = new object[strlength];

            /**/
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中  
         每个汉字有四个区位码组成  
         区位码第1位和区位码第2位作为字节数组第一个元素  
         区位码第3位和区位码第4位作为字节数组第二个元素  
        */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位   
                int r1 = rnd.Next(11, 14);//返回一个介于11到14的随机数  
                string str_r1 = rBase[r1].Trim();

                //区位码第2位   
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的种子避免产生重复值   
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位   
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位   
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码   
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中   
                byte[] str_r = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中   
                bytes.SetValue(str_r, i);
            }
            return bytes;
        }

        private string GetString(int length)  //生成指定位数的汉字验证码，并转换为String类型  
        {
            object[] obj = CreateChiCode(length);
            Encoding gb = Encoding.GetEncoding("gb2312");
            String[] bytes = new String[length];
            String code = "";
            for (int i = 0; i < length; i++)
            {
                bytes[i] = gb.GetString((byte[])Convert.ChangeType(obj[i], typeof(byte[])));
                // bytes[i] = obj[i].ToString();  
                code = code + bytes[i];
            }
            Session["CheckCode"] = code;
            return code;
        }

        private List<String> GetSimpleWords(int count)
        {
            //number 48-57
            //char  65-90
            Random rnd = new Random();
            List<String> words = new List<string>();
            for (var i = 0; i < 100; i++)
            {
                rnd = new Random(unchecked((int)DateTime.Now.Ticks) + i);
                int wordInt = rnd.Next(48, 90);
                if(wordInt > 57  && wordInt<65)
                {
                    continue;
                }

                String work = Encoding.ASCII.GetString(new byte[] { (byte)wordInt });
                words.Add(work);

                if(words.Count() == count)
                {
                    return words;
                }
            }

            return words;
        }
    }
}