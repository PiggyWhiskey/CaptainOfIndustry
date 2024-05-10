// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.PassThroughSaveCompressor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System.IO;

#nullable disable
namespace Mafi.Core.SaveGame
{
  public class PassThroughSaveCompressor : ISaveCompressor
  {
    public Stream CreateCompressingStream(Stream outputStream)
    {
      return (Stream) new PassThroughSaveCompressor.PassThroughNonClosingStream(outputStream);
    }

    public Stream CreateDecompressingStream(Stream compressedInputStream)
    {
      return (Stream) new PassThroughSaveCompressor.PassThroughNonClosingStream(compressedInputStream);
    }

    public PassThroughSaveCompressor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    /// <summary>
    /// Stream that just passes its calls to an underlying stream.
    /// Doesn't pass Dispose/Close calls.
    /// </summary>
    private class PassThroughNonClosingStream : Stream
    {
      private readonly Stream m_stream;

      public override bool CanRead => this.m_stream.CanRead;

      public override bool CanSeek => this.m_stream.CanSeek;

      public override bool CanWrite => this.m_stream.CanWrite;

      public override long Length => this.m_stream.Length;

      public override long Position
      {
        get => this.m_stream.Position;
        set => this.m_stream.Position = value;
      }

      public PassThroughNonClosingStream(Stream stream)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_stream = stream;
      }

      public override void Flush() => this.m_stream.Flush();

      public override long Seek(long offset, SeekOrigin origin)
      {
        return this.m_stream.Seek(offset, origin);
      }

      public override void SetLength(long value) => this.m_stream.SetLength(value);

      public override int Read(byte[] buffer, int offset, int count)
      {
        return this.m_stream.Read(buffer, offset, count);
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
        this.m_stream.Write(buffer, offset, count);
      }
    }
  }
}
