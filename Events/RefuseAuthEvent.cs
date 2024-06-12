using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibReConnect.Model;

namespace LibReConnect.Events
{
    /// <summary>
    /// RefuseAuth 事件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class |
    AttributeTargets.Constructor |
    AttributeTargets.Field |
    AttributeTargets.Method |
    AttributeTargets.Property,
    AllowMultiple = true)]
    public class RefuseAuthEvent : Attribute { }
}
