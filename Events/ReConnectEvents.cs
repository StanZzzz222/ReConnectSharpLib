using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using LibReConnect.Instance;
using LibReConnect.Model;

namespace LibReConnect.Events
{
    internal class MethodData
    {
        public virtual MethodInfo MethodInfo { get; set; }
        public virtual Type Type { get; set; }
        public MethodData() { }
        public MethodData(MethodInfo methodInfo, Type type) 
        {
            this.MethodInfo = methodInfo;
            this.Type = type;
        }
    }

    internal class ReConnectEvents
    {
        private static IList<MethodData> methodDatas = new List<MethodData>();
        internal ReConnectEvents(Assembly assembly)
        {
            InitMethods(assembly);
            EventListen();
            EmitEventListen();
        }

        /// <summary>
        /// Emit事件监听
        /// </summary>
        private static void EmitEventListen()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Task.Delay(1);
                    String data = LibInstance.GetEmitEventsReConnect();
                    String eventId = LibInstance.SearchReConnect(data, "id");
                    String eventName = LibInstance.SearchReConnect(data, "name");
                    String eventArgs = LibInstance.SearchReConnect(data, "data");
                    foreach (MethodData methodData in methodDatas)
                    {
                        ReConnectEmitEvent objEvent = (ReConnectEmitEvent)methodData.MethodInfo.GetCustomAttribute(typeof(ReConnectEmitEvent));
                        if (objEvent == null) continue;
                        if (objEvent.EventName.Equals(eventName))
                        {
                            int objEventParamLen = methodData.MethodInfo.GetParameters().Length;
                            int eventArgsLen = Convert.ToInt32(LibInstance.SearchReConnect(data, "data.#"));
                            if (objEventParamLen < eventArgsLen || objEventParamLen > eventArgsLen)
                            {
                                Logger.Logger.Error($"{eventName} 通知失败, 原因: 缺少入参或含有多余参数");
                                continue;
                            }
                            Object[] args = new Object[eventArgsLen];
                            for (int i = 0; i < eventArgsLen; i++)
                            {
                                String obj = Convert.ToString(LibInstance.SearchReConnect(data, $"data.{i}"));
                                Regex numberRex = new(@"^(0|[1-9][0-9]*|-[1-9][0-9]*)$");
                                Regex floatNumRex = new(@"^(\-)?\d+(\.\d{1,12})$");
                                if (numberRex.IsMatch(obj))
                                {
                                    args[i] = Convert.ToInt64(obj);
                                    continue;
                                }
                                if (floatNumRex.IsMatch(obj))
                                {
                                    args[i] = Convert.ToDouble(obj);
                                    continue;
                                }
                                if (String.Equals(obj.ToLower(), "true") || String.Equals(obj.ToLower(), "false"))
                                {
                                    args[i] = Convert.ToBoolean(obj);
                                    continue;
                                }
                                args[i] = obj;
                            }
                            Object obj1 = Activator.CreateInstance(methodData.Type);
                            Object resObj = methodData.MethodInfo.Invoke(obj1, eventArgsLen <= 0 ? null : args);
                            if (resObj == null) LibInstance.SetEmitResponse(eventId, "");
                            if (resObj != null) LibInstance.SetEmitResponse(eventId, JsonSerializer.Serialize(resObj));
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 基础事件监听
        /// </summary>
        private static void EventListen()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Task.Delay(1);
                    String data = LibInstance.GetEventsReConnect();
                    String eventName = LibInstance.SearchReConnect(data, "name");
                    if (eventName == "NULL")
                    {
                        Logger.Logger.Error($"发生错误, 事件为 NULL");
                        continue;
                    }
                    switch (eventName)
                    {
                        // 接受授权
                        case "AcceptAuth":
                            {
                                String account = LibInstance.SearchReConnect(data, "data.account");
                                String password = LibInstance.SearchReConnect(data, "data.password");
                                String token = LibInstance.SearchReConnect(data, "data.token");
                                Auth auth = new(account, password, token);
                                foreach (MethodData methodData in methodDatas)
                                {
                                    AcceptAuthEvent objEvent = (AcceptAuthEvent)methodData.MethodInfo.GetCustomAttribute(typeof(AcceptAuthEvent));
                                    if (objEvent == null) continue;
                                    int objEventParamLen = methodData.MethodInfo.GetParameters().Length;
                                    Object[] args = new Object[] { auth };
                                    if (objEventParamLen < 1 || objEventParamLen > 1)
                                    {
                                        Logger.Logger.Error("AcceptAuth 通知失败, 原因: 缺少Auth入参或含有多余参数");
                                        continue;
                                    }
                                    Object obj1 = Activator.CreateInstance(methodData.Type);
                                    if (methodData.MethodInfo != null) methodData.MethodInfo.Invoke(obj1, args);
                                }
                            }
                            continue;
                        // 拒绝授权
                        case "RefuseAuth":
                            {
                                String account = LibInstance.SearchReConnect(data, "data.account");
                                String password = LibInstance.SearchReConnect(data, "data.password");
                                String token = LibInstance.SearchReConnect(data, "data.token");
                                Auth auth = new(account, password, token);
                                foreach (MethodData methodData in methodDatas)
                                {
                                    RefuseAuthEvent objEvent = (RefuseAuthEvent)methodData.MethodInfo.GetCustomAttribute(typeof(RefuseAuthEvent));
                                    if (objEvent == null) continue;
                                    int objEventParamLen = methodData.MethodInfo.GetParameters().Length;
                                    Object[] args = new Object[] { auth };
                                    if (objEventParamLen < 1 || objEventParamLen > 1)
                                    {
                                        Logger.Logger.Error("RefuseAuth 通知失败, 原因: 缺少Auth入参或含有多余参数");
                                        continue;
                                    }
                                    Object obj1 = Activator.CreateInstance(methodData.Type);
                                    if (methodData.MethodInfo != null) methodData.MethodInfo.Invoke(obj1, args);
                                }
                            }
                            continue;
                        // 未知事件
                        default:
                            Logger.Logger.Error($"发生未处理的事件: {eventName}");
                            continue;
                    }
                }
            });
        }

        /// <summary>
        /// 初始化Methods
        /// </summary>
        /// <param name="assembly"></param>
        private static void InitMethods(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    methodDatas.Add(new(method, type));
                }
            }
        }
    }
}
