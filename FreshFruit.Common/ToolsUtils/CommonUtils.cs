using System;
using System.Text;

namespace FreshFruit.Common.ToolsUtils
{
    public class CommonUtils
    {
        /// <summary>
        /// Automatically generate random numbers
        /// </summary>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Max value</param>
        /// <param name="isAppend">Not enough digits, whether to append 0</param>
        /// <returns></returns>
        public string GetRandom(int minValue, int maxValue, bool isAppend)
        {
            Random r = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            if (isAppend)
            {
                return r.Next(minValue, maxValue).ToString().PadLeft(4, '0');
            }
            else
            {
                return r.Next(minValue, maxValue).ToString();
            }
        }

        /// <summary>
        /// Automatically generate Chinese
        /// </summary>
        /// <param name="strlength">length</param>
        /// <returns></returns>
        public static string CreateSimplifiedChinese(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            string[] r = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));

            //定义一个object数组用来 
            object[] bytes = new object[strlength];

            //每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中 
            //每个汉字有四个区位码组成 
            //区位码第1位和区位码第2位作为字节数组第一个元素 
            //区位码第3位和区位码第4位作为字节数组第二个元素 

            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位 
                int r1 = rnd.Next(11, 14);
                string str_r1 = r[r1].Trim();

                //区位码第2位 
                rnd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));//更换随机数发生器的种子避免产生重复值 
                int r2;
                if (r1 == 13)
                    r2 = rnd.Next(0, 7);
                else
                    r2 = rnd.Next(0, 16);
                string str_r2 = r[r2].Trim();

                //区位码第3位 
                rnd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
                int r3 = rnd.Next(10, 16);
                string str_r3 = r[r3].Trim();

                //区位码第4位 
                rnd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
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
                string str_r4 = r[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码 
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);

                //将两个字节变量存储在字节数组中 
                byte[] str_r = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);
            }


            //获取GB2312编码页（表） 
            Encoding gb = Encoding.GetEncoding("gb2312");
            Encoding utf8s = Encoding.GetEncoding("utf-8");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strlength; i++)
            {
                sb.Append(gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[]))));
            }

            return sb.ToString();
        }
    }
}
