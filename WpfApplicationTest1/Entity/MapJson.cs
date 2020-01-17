using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplicationTest1.Entity
{
    public class MapJson
    {
        public BigInteger md5;
        public string name;
        public int width;
        public int height;
        public Dictionary<int, string> content;
    }

    /// <summary>
    /// 中间显示实体类
    /// </summary>
    public static class MapJsonView
    {
        public static MapJson Map { set; get; }

        public static Cell GetCellData(int idx)
        {
            throw new NotImplementedException();
        }        
    }
}
