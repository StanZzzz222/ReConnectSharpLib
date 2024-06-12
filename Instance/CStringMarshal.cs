using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LibReConnect.Instance
{
    internal class CStringMarshal : ICustomMarshaler
    {
        [DllImport("ReConnect", EntryPoint = "Free", CallingConvention = CallingConvention.Cdecl)]
        extern static void Free(IntPtr ptr);

        private static readonly CStringMarshal Instance = new();
        [ThreadStatic] private static IntPtr lastIntPtr;

        public void CleanUpManagedData(object managedObj) {}

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (lastIntPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(lastIntPtr);
                lastIntPtr = IntPtr.Zero;
            }
        }

        public int GetNativeDataSize() { return -1; }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            if (ReferenceEquals(managedObj, null)) return IntPtr.Zero;
            if (!(managedObj is string)) throw new InvalidOperationException();
            var utf8Bytes = Encoding.UTF8.GetBytes(managedObj as string);
            var ptr = Marshal.AllocHGlobal(utf8Bytes.Length + 1);
            Marshal.Copy(utf8Bytes, 0, ptr, utf8Bytes.Length);
            Marshal.WriteByte(ptr, utf8Bytes.Length, 0);
            return lastIntPtr = ptr;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero) return null;
            String res = Marshal.PtrToStringUTF8(pNativeData);
            Free(pNativeData);
            return res;
        }

        public static ICustomMarshaler GetInstance(string cookie) { return Instance; }
    }
}
