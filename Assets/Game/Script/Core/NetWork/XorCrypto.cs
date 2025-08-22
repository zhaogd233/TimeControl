
using System;
#if UNITY_WP8
using UnityPort;
#endif

public class SendXorCrypto
{
    private static void _Xor(Byte[] buf, UInt32 size, byte[] key)
    {
        int keySize = key.Length;
        for (int i = 0; i < size; ++i)
        {
            buf[i] ^= key[i % keySize];
        }
    }
    public static void XorEncrypt(Byte[] buf, UInt32 size, byte[] key)
    {
        _Xor(buf, size, key);
    }
    public static void XorDecrypt(Byte[] buf, UInt32 size, byte[] key)
    {
        _Xor(buf, size, key);
    }
}

public class ReceiveXorCrypto
{
    //1E95A51FD4C38CD68428186BC5C3E26F
#if UNITY_WP8
        static private byte[] s_Key = PortUtil.StringToASCII("1E95A51FD4C38CD68428186BC5C3E26F");
#else
    static private byte[] s_Key = System.Text.Encoding.ASCII.GetBytes("989EDC24483C4C7B8601B2EA1C349B23");
#endif
    static private int s_KeySize = s_Key.Length;
    private static void _Xor(Byte[] Buf, UInt32 nSize)
    {
        for (int i = 0; i < nSize; ++i)
        {
            Buf[i] ^= s_Key[i % s_KeySize];
        }
    }
    public static void XorEncrypt(Byte[] Buf, UInt32 nSize)
    {
        _Xor(Buf, nSize);
    }
    public static void XorDecrypt(Byte[] Buf, UInt32 nSize)
    {
        _Xor(Buf, nSize);
    }
}