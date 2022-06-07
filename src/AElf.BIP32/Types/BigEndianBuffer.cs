using System;
using System.Collections.Generic;

namespace AElf.BIP32.Types;

/// <summary>
///     https://github.com/elucidsoft/dotnetstandard-bip32/blob/master/dotnetstandard-bip32/BigEndianBuffer.cs
/// </summary>
public class BigEndianBuffer
{
    private readonly List<byte> _bytes = new();

    public void WriteUInt(uint i)
    {
        _bytes.Add((byte)((i >> 0x18) & 0xff));
        _bytes.Add((byte)((i >> 0x10) & 0xff));
        _bytes.Add((byte)((i >> 8) & 0xff));
        _bytes.Add((byte)(i & 0xff));
    }

    public void Write(byte b)
    {
        _bytes.Add(b);
    }

    public void Write(byte[] bytes)
    {
        Write(bytes, 0, bytes.Length);
    }

    public void Write(byte[] bytes, int offset, int count)
    {
        var newBytes = new byte[count];
        Array.Copy(bytes, offset, newBytes, 0, count);

        _bytes.AddRange(newBytes);
    }

    public byte[] ToArray()
    {
        return _bytes.ToArray();
    }
}