// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.ISaveCompressor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.IO;

#nullable disable
namespace Mafi.Core.SaveGame
{
  /// <summary>
  /// Interface for creation of compressing and decompressing stream for game saving.
  /// </summary>
  /// <remarks>
  /// Call to close on the streams created through this interface does not close the underlying stream that was
  /// passed to <see cref="M:Mafi.Core.SaveGame.ISaveCompressor.CreateCompressingStream(System.IO.Stream)" /> or <see cref="M:Mafi.Core.SaveGame.ISaveCompressor.CreateDecompressingStream(System.IO.Stream)" />
  /// </remarks>
  public interface ISaveCompressor
  {
    Stream CreateCompressingStream(Stream outputStream);

    Stream CreateDecompressingStream(Stream compressedInputStream);
  }
}
