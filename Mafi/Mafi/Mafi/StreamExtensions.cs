// Decompiled with JetBrains decompiler
// Type: Mafi.StreamExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.IO;

#nullable disable
namespace Mafi
{
  public static class StreamExtensions
  {
    [ThreadStatic]
    private static byte[] s_buffer;

    /// <summary>
    /// Copies contents of this stream from its current position to given stream. This is more efficient than <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)" /> since this is allocation-free. This method is thread safe.
    /// </summary>
    public static void CopyToFast(this Stream fromStream, Stream toStream)
    {
      byte[] buffer = StreamExtensions.s_buffer;
      if (buffer == null)
      {
        buffer = new byte[16384];
        StreamExtensions.s_buffer = buffer;
      }
      int count;
      while ((count = fromStream.Read(buffer, 0, buffer.Length)) > 0)
        toStream.Write(buffer, 0, count);
    }
  }
}
