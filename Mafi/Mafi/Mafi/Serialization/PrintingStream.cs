// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.PrintingStream
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Mafi.Serialization
{
  public class PrintingStream : Stream
  {
    private readonly StringBuilder m_sb;

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override long Length => (long) this.m_sb.Length;

    public override long Position { get; set; }

    public override void Flush()
    {
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      for (int index = 0; index < count; ++index)
      {
        this.m_sb.Append("0x");
        this.m_sb.Append(buffer[offset + index].ToString("X2"));
        this.m_sb.Append(" ");
      }
      this.m_sb.Append("  | ");
      for (int index = 0; index < count; ++index)
      {
        byte num = buffer[offset + index];
        if (num >= (byte) 32 && num <= (byte) 126)
          this.m_sb.Append((char) num);
        else
          this.m_sb.Append(".");
      }
      this.m_sb.AppendLine();
    }

    public override string ToString() => this.m_sb.ToString();

    public PrintingStream()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_sb = new StringBuilder();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
