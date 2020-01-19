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
    }
}
