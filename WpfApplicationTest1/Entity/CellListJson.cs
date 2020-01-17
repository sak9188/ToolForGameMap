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
        public class Cell
        {
            public int idx;
            public string name;
            public int avo;
            public int def;
            public int alt;
        }

        public BigInteger md5;
        public List<Cell> list;
    }
}
