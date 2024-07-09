namespace Yukami.Extension;

using System.Diagnostics.CodeAnalysis;
using System.Text;


internal static class BinaryExtension
{
    public static long Now(this BinaryReader reader)
    {
        return reader.BaseStream.Position;
    }

    public static void Skip(this BinaryReader reader, long count)
    {
        reader.BaseStream.Seek(count, SeekOrigin.Current);
    }

    public static void SkipCurrentZero(this BinaryReader reader)
    {
        if (reader.ReadByte() == 0)
            return;
        reader.ReWind(1);
    }
    
    public static void ReWind(this BinaryReader reader, long count)
    {
        reader.BaseStream.Seek(-count, SeekOrigin.Current);
    }

    public static byte PeekByte(this BinaryReader reader)
    {
        var current = reader.ReadByte();
        reader.ReWind(1);
        return current;
    }

    public static void GoTo(this BinaryReader reader, long offset)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
    }

    public static short ReadInt16Le(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(2);
        return BitConverter.ToInt16(bytes, 0);
    }

    public static ushort ReadUInt16Le(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(2);
        return BitConverter.ToUInt16(bytes, 0);
    }

    public static int ReadInt32Le(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(4);
        return BitConverter.ToInt32(bytes, 0);
    }

    public static uint ReadUInt32Le(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(4);
        return BitConverter.ToUInt32(bytes, 0);
    }

    public static long ReadInt64Le(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(8);
        return BitConverter.ToInt64(bytes, 0);
    }

    public static ulong ReadUInt64Le(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(8);
        return BitConverter.ToUInt64(bytes, 0);
    }

    public static byte[] ReadCString(this BinaryReader reader)
    {
        var bytes = new List<byte>();
        byte b;
        while ((b = reader.ReadByte()) != 0)
        {
            bytes.Add(b);
        }
        return bytes.ToArray();
    }
    
    public static string ReadCStringToString(this BinaryReader reader)
    {
        var bytes = new List<byte>();
        byte b;
        while ((b = reader.ReadByte()) != 0)
        {
            bytes.Add(b);
        }

        return Encoding.ASCII.GetString(bytes.ToArray()).TrimEnd('\0');
    }

    public static string ReadBytesAsHex(this BinaryReader reader, int count)
    {
        var bytes = reader.ReadBytes(count);
        return BitConverter.ToString(bytes).Replace("-", "");
    }
    
    public static string ReadBytesAsCString(this BinaryReader reader, int count)
    {
        var bytes = reader.ReadBytes(count);
        return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
    }
    
    public static string ReadBytesAsShiftJisString(this BinaryReader reader, int count)
    {
        var bytes = reader.ReadBytes(count);
        return Encoding.GetEncoding(932).GetString(bytes).TrimEnd('\0');
    }
    
    public static BinaryReader XorFromNowToEnd(this BinaryReader reader, byte key)
    {
        var pos = reader.Now();
        var end = reader.BaseStream.Length;
        
        var bytes = new List<byte>();
        while (reader.Now() < end)
        {
            bytes.Add((byte)(reader.ReadByte() ^ key));
        }
        
        reader.GoTo(0);

        var beforeBytes = reader.ReadBytes(Convert.ToInt32(pos));
        bytes.InsertRange(0, beforeBytes);
        
        reader.GoTo(pos);

        return new BinaryReader(new MemoryStream(bytes.ToArray()));
    }
}
