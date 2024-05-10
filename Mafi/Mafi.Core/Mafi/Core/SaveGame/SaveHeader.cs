// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.SaveHeader
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Mafi.Core.SaveGame
{
  /// <summary>
  /// Header stored as the beginning of a saved game stream (before compressed game data).
  /// </summary>
  /// <remarks>
  /// The class is not using BlobReader/Writer for its reads/writes to the stream as it is used before these are
  /// configured and can be used.
  /// </remarks>
  public readonly struct SaveHeader
  {
    public readonly ulong Header;
    public readonly int Version;
    public readonly SaveCompressionType CompressionType;
    public readonly long DataSize;
    public readonly uint DataCrc32Checksum;
    public readonly long DataSizeBeforeCompression;
    public readonly uint DataCrc32ChecksumBeforeCompression;

    public SaveHeader(
      ulong header,
      int version,
      SaveCompressionType compressionType,
      long dataSize,
      uint dataCrc32Checksum,
      long dataSizeBeforeCompression,
      uint dataCrc32ChecksumBeforeCompression)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Header = header;
      this.Version = version;
      this.CompressionType = compressionType;
      this.DataSize = dataSize;
      this.DataCrc32Checksum = dataCrc32Checksum;
      this.DataSizeBeforeCompression = dataSizeBeforeCompression;
      this.DataCrc32ChecksumBeforeCompression = dataCrc32ChecksumBeforeCompression;
    }

    public void SerializeCustom(Stream output)
    {
      using (BinaryWriter binaryWriter = new BinaryWriter(output, Encoding.UTF8, true))
      {
        binaryWriter.Write(this.Header);
        binaryWriter.Write(this.Version);
        binaryWriter.Write((int) this.CompressionType);
        binaryWriter.Write(this.DataSize);
        binaryWriter.Write(this.DataCrc32Checksum);
        binaryWriter.Write(this.DataSizeBeforeCompression);
        binaryWriter.Write(this.DataCrc32ChecksumBeforeCompression);
      }
    }

    public static SaveHeader DeserializeCustom(Stream input, ulong? alreadyReadHeader = null)
    {
      using (BinaryReader binaryReader = new BinaryReader(input, Encoding.UTF8, true))
      {
        ulong header = (ulong) ((long) alreadyReadHeader ?? (long) binaryReader.ReadUInt64());
        int version = binaryReader.ReadInt32();
        int num = binaryReader.ReadInt32();
        SaveCompressionType compressionType = Enum.IsDefined(typeof (SaveCompressionType), (object) num) ? (SaveCompressionType) num : throw new CorruptedSaveException(string.Format("Invalid compression type specified: {0}", (object) num));
        long dataSize = 0;
        long dataSizeBeforeCompression = 0;
        uint dataCrc32Checksum = 0;
        uint dataCrc32ChecksumBeforeCompression = 0;
        if (version >= 167)
        {
          dataSize = binaryReader.ReadInt64();
          dataCrc32Checksum = binaryReader.ReadUInt32();
          dataSizeBeforeCompression = binaryReader.ReadInt64();
          dataCrc32ChecksumBeforeCompression = binaryReader.ReadUInt32();
        }
        return new SaveHeader(header, version, compressionType, dataSize, dataCrc32Checksum, dataSizeBeforeCompression, dataCrc32ChecksumBeforeCompression);
      }
    }

    public override string ToString()
    {
      return string.Format("{0} v{1} {2}, ", (object) SaveHeaders.HeaderToStringInclHex(this.Header), (object) this.Version, (object) this.CompressionType) + string.Format("data len {0}, crc 32 0x{1:X8}", (object) this.DataSize, (object) this.DataCrc32Checksum);
    }
  }
}
