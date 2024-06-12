using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;

namespace LibReConnect.Instance
{
    internal class LibInstance
    {
        [DllImport("ReConnect", EntryPoint = "Init", CallingConvention = CallingConvention.Cdecl)]
        extern static void Init(int port, byte[] emitServerToken);
        [DllImport("ReConnect", EntryPoint = "UploadServerData", CallingConvention = CallingConvention.Cdecl)]
        extern static void UploadServerData(byte[] serverName, byte[] version, byte[] gameMode, byte[] logoUrlBytes, byte[] appItemsBytes, int onlineCount);

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CStringMarshal))]
        [DllImport("ReConnect", EntryPoint = "GenToken", CallingConvention = CallingConvention.Cdecl)]
        extern static String GenToken(int second);

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CStringMarshal))]
        [DllImport("ReConnect", EntryPoint = "Search", CallingConvention = CallingConvention.Cdecl)]
        extern static String Search(byte[] json, byte[] exp);

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CStringMarshal))]
        [DllImport("ReConnect", EntryPoint = "GetEvents", CallingConvention = CallingConvention.Cdecl)]
        extern static String GetEvents();

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CStringMarshal))]
        [DllImport("ReConnect", EntryPoint = "GetEmitEvents", CallingConvention = CallingConvention.Cdecl)]
        extern static String GetEmitEvents();

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CStringMarshal))]
        [DllImport("ReConnect", EntryPoint = "SetEmitResponse", CallingConvention = CallingConvention.Cdecl)]
        extern static void SetEmitResponse(byte[] id, byte[] data);

        public static void InitReConnect(int port, String emitServerToken) { Init(port, CString(emitServerToken)); }
        public static void UploadServerDataReConnect(String serverName, String version, String gameMode, String logoUrl, String appItemsJson, int onlineCount)
        {
            byte[] serverNameBytes = CString(serverName);
            byte[] versionBytes = CString(version);
            byte[] logoUrlBytes = CString(logoUrl);
            byte[] appItemsBytes = CString(appItemsJson);
            byte[] gameModeBytes = CString(gameMode);
            UploadServerData(serverNameBytes, versionBytes, gameModeBytes, logoUrlBytes, appItemsBytes, onlineCount);
        }
        public static void SetEmitResponse(String id, String response) { SetEmitResponse(CString(id), CString(response)); }
        public static String GenTokenReConnect(int second) { return GenToken(second); }
        public static String SearchReConnect(String json, String exp) { return Search(CString(json), CString(exp)); }
        public static String GetEventsReConnect() { return GetEvents(); }
        public static String GetEmitEventsReConnect() { return GetEmitEvents(); }

        // CString
        private static byte[] CString(String str) { return Encoding.UTF8.GetBytes(str); }
    }
}
