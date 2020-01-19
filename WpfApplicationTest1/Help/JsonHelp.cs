using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplicationTest1.Entity;

namespace WpfApplicationTest1.Help
{
    public static class JsonHelp
    {
        public static CellListJson LoadJsonFile(string path)
        {
            string jsonText;
            try
            {
                jsonText = File.ReadAllText(path, Encoding.UTF8);
                
            }
            catch (Exception)
            {
                MessageBox.Show("打开文件错误");
                return null;
            }
            CellListJson objJson = JsonConvert.DeserializeObject<CellListJson>(jsonText);
            // 
            FileStream fs = new FileStream(path, FileMode.Open);
            string md5 = GenerateMD5(fs);
            objJson.md5 = md5;
            return objJson;
        }

        public static void ListToFile(CellListJson list, string path)
        {
            string objString = JsonConvert.SerializeObject(list);
            string name = string.Format("\\{0}.json", list.name);
            int num = 0;
            string namePath = path+name;
            try 
	        {
                while(true)
                {
                    if (File.Exists(namePath))
                    {
                        name = string.Format("\\{0}{1}.json", list.name, num);
                        namePath = path + name;
                        num++;
                        continue;
                    }
                    else
                    {
                        File.WriteAllText(namePath, objString, Encoding.UTF8);
                        break;
                    }
                }
	        }
	        catch (Exception)
	        {
		        MessageBox.Show("导出文件错误");
	        }
            MessageBox.Show(name+"导出成功");           
        }

        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// MD5流加密
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static string GenerateMD5(Stream inputStream)
        {
            using (MD5 mi = MD5.Create())
            {
                //开始加密
                byte[] newBuffer = mi.ComputeHash(inputStream);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
