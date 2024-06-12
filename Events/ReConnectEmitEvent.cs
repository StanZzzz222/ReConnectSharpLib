using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibReConnect.Model;

namespace LibReConnect.Events
{
    /// <summary>
    /// ReConnectEmitEvent 事件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class |
    AttributeTargets.Constructor |
    AttributeTargets.Field |
    AttributeTargets.Method |
    AttributeTargets.Property,
    AllowMultiple = true)]
    public class ReConnectEmitEvent : Attribute 
    {
        public virtual String EventName { get; set; } // 事件名称
        public ReConnectEmitEvent(String eventName)
        {
            this.EventName = eventName;
        }
    }
}
