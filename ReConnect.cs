using System.Diagnostics;
using System.Reflection;
using LibReConnect.Events;
using LibReConnect.Instance;
using LibReConnect.Model;

[assembly: Debuggable(true, false)]
namespace LibReConnect
{
    public class ReConnect
    {
        /// <summary>
        /// 初始化的方法
        /// </summary>
        /// <param name="port">HTTP端口号</param>
        /// <param name="emitServerToken">小程序触发服务端的Token</param>
        /// <param name="assembly">主程序集</param>
        public static void Init(int port, String emitServerToken, Assembly assembly) 
        {
            ReConnectEvents _ = new(assembly);
            LibInstance.InitReConnect(port, emitServerToken); 
        }

        /// <summary>
        /// 上报服务器数据
        /// </summary>
        /// <param name="serverName">服务器名称</param>
        /// <param name="version">服务器版本</param>
        /// <param name="gameMode">游戏模式</param>
        /// <param name="logoUrl">服务器Logo的URL地址</param>
        /// <param name="appItems">小程序应用列表</param>
        /// <param name="onlineCount">在线玩家数量</param>
        public static void UploadServerDataReConnect(String serverName, String version, String gameMode, String logoUrl, IList<AppItem> appItems, int onlineCount) 
        {
            String appItemsJson = "";
            if (appItems != null)
            {
                foreach (AppItem appItem in appItems)
                {
                    String json = appItem.ToJson();
                    appItemsJson += $"{json},";
                }
                appItemsJson = appItemsJson.Substring(0, appItemsJson.Length - 1);
                appItemsJson = $"[{appItemsJson}]";
            }
            LibInstance.UploadServerDataReConnect(serverName, version, gameMode, logoUrl, appItemsJson, onlineCount);
        }

        /// <summary>
        /// 生成Token的方法
        /// </summary>
        /// <param name="expTime">秒</param>
        /// <returns>生成的Token</returns>
        public static String GenToken(int expTime) { return LibInstance.GenTokenReConnect(expTime); }
    }
}
