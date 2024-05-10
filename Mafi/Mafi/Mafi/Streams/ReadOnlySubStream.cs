// Decompiled with JetBrains decompiler
// Type: Mafi.Streams.ReadOnlySubStream
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.IO;

#nullable disable
namespace Mafi.Streams
{
  public class ReadOnlySubStream : Stream
  {
    private Stream m_baseStream;
    private readonly long m_length;
    private readonly long? m_startPosition;
    private long m_relativePosition;

    public bool IsDone => this.m_relativePosition >= this.m_length;

    public override bool CanWrite => false;

    public override bool CanRead
    {
      get
      {
        this.checkDisposed();
        return true;
      }
    }

    public override long Length
    {
      get
      {
        this.checkDisposed();
        return this.m_length;
      }
    }

    public override bool CanSeek
    {
      get
      {
        this.checkDisposed();
        return this.m_baseStream.CanSeek;
      }
    }

    public override long Position
    {
      get
      {
        this.checkDisposed();
        return this.m_relativePosition;
      }
      set => throw new NotSupportedException();
    }

    public ReadOnlySubStream(Stream baseStream, long length)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (length < 0L)
        throw new ArgumentOutOfRangeException(nameof (length), (object) length, "Cannot be negative");
      if (baseStream.CanSeek && baseStream.Position + length > baseStream.Length)
        throw new ArgumentOutOfRangeException(nameof (length), (object) length, string.Format("Cannot point further than {0}", (object) baseStream.Length));
      this.m_baseStream = baseStream.CanRead ? baseStream.CheckNotNull<Stream>() : throw new ArgumentException("Cannot read base stream.");
      if (baseStream.CanSeek)
        this.m_startPosition = new long?(baseStream.Position);
      this.m_length = length;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      this.checkDisposed();
      long num1 = this.m_length - this.m_relativePosition;
      if (num1 <= 0L)
        return 0;
      if (num1 < (long) count)
        count = (int) num1;
      int num2 = this.m_baseStream.Read(buffer, offset, count);
      this.m_relativePosition += (long) num2;
      return num2;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      if (!this.m_startPosition.HasValue)
        throw new NotSupportedException();
      long num;
      switch (origin)
      {
        case SeekOrigin.Begin:
          num = this.m_baseStream.Seek(this.m_startPosition.Value + offset, SeekOrigin.Begin);
          break;
        case SeekOrigin.Current:
          num = this.m_baseStream.Seek(offset, SeekOrigin.Current);
          break;
        case SeekOrigin.End:
          num = this.m_baseStream.Seek(this.m_startPosition.Value + this.m_length + offset, SeekOrigin.Begin);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (origin), (object) origin, (string) null);
      }
      if (num < this.m_startPosition.Value || num > this.m_startPosition.Value + this.m_length)
        throw new IndexOutOfRangeException();
      this.m_relativePosition = num - this.m_startPosition.Value;
      return num;
    }

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Flush() => this.m_baseStream.Flush();

    public override void Close()
    {
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      throw new NotSupportedException();
    }

    public void SkipRest()
    {
      if (this.m_baseStream.CanSeek)
      {
        this.Seek(this.m_length - this.m_relativePosition, SeekOrigin.Current);
      }
      else
      {
        for (; this.m_relativePosition < this.m_length; ++this.m_relativePosition)
          this.m_baseStream.ReadByte();
      }
    }

    protected override void Dispose(bool disposing)
    {
      this.checkDisposed();
      base.Dispose(disposing);
      if (!disposing)
        return;
      this.m_baseStream = (Stream) null;
    }

    private void checkDisposed()
    {
      if (this.m_baseStream == null)
        throw new ObjectDisposedException(this.GetType().Name);
    }
  }
}
