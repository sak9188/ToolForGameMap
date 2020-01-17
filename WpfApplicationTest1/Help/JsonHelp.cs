using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return objJson;
        }
    }
}
