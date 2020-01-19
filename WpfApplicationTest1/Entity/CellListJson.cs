using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplicationTest1.Entity
{
    public class CellListJson
    {
        public string md5;
        public string name;
        public List<Cell> list;
    }

    public class Cell
    {
        [NonSerialized]
        public int idx;

        [NonSerialized]
        public bool delete = false;

        public string name;
        public int avo;
        public int def;
        public int alt;
        public string other;
    }
}
