// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.MemoryBlobWriter
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using System.IO;

#nullable disable
namespace Mafi.Serialization
{
  public class MemoryBlobWriter : BlobWriter
  {
    public long Length => this.OutputStream.Length;

    public MemoryStream BaseStream => (MemoryStream) this.OutputStream;

    public MemoryBlobWriter(
      ImmutableArray<ISpecialSerializerFactory>? specialSerializers = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector((Stream) new MemoryStream(1024), specialSerializers);
    }

    /// <summary>
    /// Resets the internal stream to zero length, effectively wiping all data but keeping
    /// the internal stream allocated.
    /// WARNING: This only works if the internal stream is a MemoryStream.
    /// </summary>
    public override void Reset()
    {
      base.Reset();
      this.OutputStream.SetLength(0L);
    }

    public override void Dispose()
    {
      base.Dispose();
      this.OutputStream.Dispose();
    }

    /// <summary>
    /// Finalizes serialization and returns the internal stream.
    /// WARNING: After this operation, it is no longer safe to write to this writer!
    /// </summary>
    public Stream FinalizeAndReturnStream()
    {
      this.FinalizeSerialization();
      base.Dispose();
      this.OutputStream.Position = 0L;
      return this.OutputStream;
    }

    /// <summary>
    /// Appends the internal stream to the given stream.
    /// WARNING: After this operation, it is no longer safe to write to this writer!
    /// </summary>
    public void AppendTo(Stream stream)
    {
      this.Flush();
      this.OutputStream.Position = 0L;
      this.OutputStream.CopyToFast(stream);
    }

    /// <summary>
    /// Returns a stream for appending to different stream. Returned stream position is reset to zero.
    /// WARNING: After this operation, it is no longer safe to write to this writer!
    /// </summary>
    public Stream GetStreamForAppending()
    {
      this.Flush();
      this.OutputStream.Position = 0L;
      return this.OutputStream;
    }

    /// <summary>
    /// Returns contents of the writer as an array, regardless of the stream position.
    /// WARNING: This only works if the internal stream is a MemoryStream.
    /// </summary>
    public byte[] ToArray()
    {
      this.Flush();
      return ((MemoryStream) this.OutputStream).ToArray();
    }
  }
}
