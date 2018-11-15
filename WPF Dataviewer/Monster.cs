using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Dataviewer
{
    public class Monster
    {
        public int number { get; set; }
        public string name { get; set; }
        public Uri url { get; set; }
    }

    public class RootObject
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<Monster> results { get; set; }
    }
}
