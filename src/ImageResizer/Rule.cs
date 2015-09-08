using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageResizer
{
   public class Rule
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string FileNameRule { get; set; }
        public int? Quality { get; set; } = 75;//建议75，如果不想改变此值，可以设置为null
    }
}
