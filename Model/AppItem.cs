using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibReConnect.Model
{
    /// <summary>
    /// 小程序项
    /// @RED
    /// </summary>
    public class AppItem
    {
        public virtual String Name { get; set; } // 小程序名称
        public virtual String Desc { get; set; } // 小程序描述
        public virtual String Src { get; set; } // 小程序Web地址
        public virtual String Author { get; set; } // 小程序作者

        public AppItem() { }
        public AppItem(String name, String desc, String src, String author) 
        {
            this.Name = name;
            this.Desc = desc;
            this.Src = src;
            this.Author = author;
        }

        public String ToJson()
        {
            return $"{{\"name\": \"{this.Name}\", \"desc\": \"{this.Desc}\", \"src\": \"{this.Src}\", \"author\": \"{this.Author}\"}}";
        }
    }
}
