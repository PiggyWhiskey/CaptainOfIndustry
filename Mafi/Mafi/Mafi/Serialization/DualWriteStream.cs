// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.DualWriteStream
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System.IO;

#nullable disable
namespace Mafi.Serialization
{
  internal class DualWriteStream : Stream
  {
    private readonly Stream m_primaryStream;
    private readonly Stream m_secondaryStream;

    public override bool CanRead => this.m_primaryStream.CanRead;

    public override bool CanSeek => this.m_primaryStream.CanSeek && this.m_secondaryStream.CanSeek;

    public override bool CanWrite
    {
      get => this.m_primaryStream.CanWrite && this.m_secondaryStream.CanWrite;
    }

    public override long Length => this.m_primaryStream.Length;

    public override long Position
    {
      get => this.m_primaryStream.Position;
      set => this.m_primaryStream.Position = value;
    }

    public DualWriteStream(Stream primaryStream, Stream secondaryStream)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_primaryStream = primaryStream;
      this.m_secondaryStream = secondaryStream;
    }

    public override void Flush()
    {
      this.m_primaryStream.Flush();
      this.m_secondaryStream.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      this.m_secondaryStream.Seek(offset, origin);
      return this.m_primaryStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
      this.m_primaryStream.SetLength(value);
      this.m_secondaryStream.SetLength(value);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      return this.m_primaryStream.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.m_primaryStream.Write(buffer, offset, count);
      this.m_secondaryStream.Write(buffer, offset, count);
    }

    protected override void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.m_primaryStream.Dispose();
      this.m_secondaryStream.Dispose();
    }
  }
}
