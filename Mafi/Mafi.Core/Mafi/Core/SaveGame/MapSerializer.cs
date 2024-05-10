// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.MapSerializer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Mafi.Core.SaveGame
{
  public class MapSerializer : IAsyncSavable
  {
    public static readonly byte[] HEADER_MAP_ASCII;
    public static readonly ulong HEADER_MAP;
    public static readonly byte[] HEADER_MAP_PREVIEW_ASCII;
    public static readonly ulong HEADER_MAP_PREVIEW;
    public static readonly byte[] HEADER_MAP_EXTRA_ASCII;
    public static readonly ulong HEADER_MAP_EXTRA;
    public static readonly byte[] HEADER_MAP_DATA_ASCII;
    public static readonly ulong HEADER_MAP_DATA;
    public const int MIN_COMPATIBLE_SAVE_VERSION = 140;
    private readonly ImmutableArray<ISpecialSerializerFactory> m_specialSerializers;
    private readonly Stopwatch m_stopwatch;
    private SaveCompressionType m_compressionType;
    private Option<MemoryBlobWriter> m_mainWriter;

    public TimeSpan LastSaveStartDuration { get; private set; }

    public TimeSpan LastSaveFinalizeDuration { get; private set; }

    public TimeSpan LastSaveTotalDuration
    {
      get => this.LastSaveStartDuration + this.LastSaveFinalizeDuration;
    }

    public MapSerializer(
      ImmutableArray<ISpecialSerializerFactory> specialSerializers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_stopwatch = new Stopwatch();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_specialSerializers = specialSerializers;
    }

    public void StartSave(
      IWorldRegionMap map,
      IWorldRegionMapPreviewData previewData,
      IWorldRegionMapAdditionalData additionalData,
      SaveCompressionType saveCompressionType = SaveCompressionType.Gzip)
    {
      Mafi.Assert.That<Option<MemoryBlobWriter>>(this.m_mainWriter).IsNone<MemoryBlobWriter>("Save was already started!");
      this.Reset();
      this.m_stopwatch.Restart();
      MemoryBlobWriter memoryBlobWriter = new MemoryBlobWriter(new ImmutableArray<ISpecialSerializerFactory>?(this.m_specialSerializers));
      memoryBlobWriter.WriteULongNonVariable(MapSerializer.HEADER_MAP_PREVIEW);
      memoryBlobWriter.WriteGeneric<IWorldRegionMapPreviewData>(previewData);
      memoryBlobWriter.FinalizeSerialization();
      memoryBlobWriter.WriteULongNonVariable(MapSerializer.HEADER_MAP_EXTRA);
      memoryBlobWriter.WriteGeneric<IWorldRegionMapAdditionalData>(additionalData);
      memoryBlobWriter.FinalizeSerialization();
      memoryBlobWriter.WriteULongNonVariable(MapSerializer.HEADER_MAP_DATA);
      memoryBlobWriter.WriteGeneric<IWorldRegionMap>(map);
      memoryBlobWriter.FinalizeSerialization();
      memoryBlobWriter.WriteULongNonVariable(SaveHeaders.HEADER_SAVE_END_LE);
      this.m_mainWriter = (Option<MemoryBlobWriter>) memoryBlobWriter;
      this.m_compressionType = saveCompressionType;
      this.m_stopwatch.Stop();
      this.LastSaveStartDuration = this.m_stopwatch.Elapsed;
      Mafi.Log.Info("MapSerializer: Save took " + ((float) this.m_stopwatch.Elapsed.TotalMilliseconds).RoundToSigDigits(3, false, false, false) + " ms");
    }

    /// <remarks>WARNING: This is invoked from a background thread.</remarks>
    public void FinishSaveWriteToStream(Stream outputStream)
    {
      MemoryBlobWriter valueOrNull = this.m_mainWriter.ValueOrNull;
      SaveCompressionType compressionType = this.m_compressionType;
      if (valueOrNull == null)
      {
        Mafi.Log.Error("Save not started");
      }
      else
      {
        Stopwatch stopwatch = Stopwatch.StartNew();
        MapSerializer.finishSaveWriteToStreamNoState(outputStream, valueOrNull, compressionType);
        this.LastSaveFinalizeDuration = stopwatch.Elapsed;
        Mafi.Log.Info("MapSerializer: Save finalize took " + ((float) stopwatch.Elapsed.TotalMilliseconds).RoundToSigDigits(3, false, false, false) + " ms");
      }
    }

    /// <summary>
    /// Safer state-less method for saving in separate thread.
    /// </summary>
    private static void finishSaveWriteToStreamNoState(
      Stream outputStream,
      MemoryBlobWriter writer,
      SaveCompressionType compression)
    {
      ISaveCompressor compressor;
      if (compression == SaveCompressionType.NoCompression)
        compressor = (ISaveCompressor) null;
      else if (!GameLoader.TryGetCompressor(compression, out compressor))
        Mafi.Log.Error(string.Format("Unknown compression '{0}', using gzip.", (object) compression));
      Stream stream = writer.GetStreamForAppending();
      long readBytes1;
      uint dataCrc32ChecksumBeforeCompression = Crc32.Compute(stream, out readBytes1);
      stream.Seek(0L, SeekOrigin.Begin);
      Mafi.Assert.That<long>(stream.Length).IsEqualTo(readBytes1);
      uint dataCrc32Checksum = dataCrc32ChecksumBeforeCompression;
      if (compressor != null)
      {
        MemoryStream memoryStream = new MemoryStream(65536);
        using (Stream compressingStream = compressor.CreateCompressingStream((Stream) memoryStream))
          stream.CopyToFast(compressingStream);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        long readBytes2;
        dataCrc32Checksum = Crc32.Compute((Stream) memoryStream, out readBytes2);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        Mafi.Assert.That<long>(memoryStream.Length).IsEqualTo(readBytes2);
        stream = (Stream) memoryStream;
      }
      new SaveHeader(MapSerializer.HEADER_MAP, 168, compression, outputStream.Length, dataCrc32Checksum, readBytes1, dataCrc32ChecksumBeforeCompression).SerializeCustom(outputStream);
      stream.CopyToFast(outputStream);
    }

    public static bool ValidateChecksumSafe(Stream stream, out SaveHeader headers)
    {
      headers = new SaveHeader();
      try
      {
        headers = SaveHeader.DeserializeCustom(stream);
        long readBytes;
        return headers.DataSize <= 0L || (int) Crc32.Compute(stream, out readBytes) == (int) headers.DataCrc32Checksum && headers.DataSize == readBytes;
      }
      catch
      {
        return false;
      }
    }

    private BlobReader startMapLoad(Stream stream, bool skipNewVersionCheck = false)
    {
      SaveHeader saveHeader = SaveHeader.DeserializeCustom(stream);
      if ((long) saveHeader.Header != (long) MapSerializer.HEADER_MAP)
        throw new CorruptedSaveException("Invalid map file header " + SaveHeaders.HeaderToStringInclHex(saveHeader.Header) + ", expected " + SaveHeaders.HeaderToStringInclHex(MapSerializer.HEADER_MAP) + ".");
      if (saveHeader.Version < 140)
        throw new IncompatibleSaveVersionException(saveHeader.Version, 140, 168);
      if (saveHeader.Version > 168 && !skipNewVersionCheck)
        throw new IncompatibleSaveVersionException(saveHeader.Version, 140, 168);
      Mafi.Log.Info(string.Format("Loading map from save version {0} (current is ", (object) saveHeader.Version) + string.Format("{0}) using '{1}' compression type.", (object) 168, (object) saveHeader.CompressionType));
      ISaveCompressor compressor;
      if (!GameLoader.TryGetCompressor(saveHeader.CompressionType, out compressor))
        throw new CorruptedSaveException(string.Format("Fail to load save compressor with ID '{0}'.", (object) saveHeader.CompressionType));
      BlobReader blobReader = new BlobReader(compressor.CreateDecompressingStream(stream), saveHeader.Version);
      blobReader.SetSpecialSerializers(this.m_specialSerializers);
      return blobReader;
    }

    private void continueLoadUntilHeader(
      BlobReader reader,
      ulong finalReadHeader,
      out IWorldRegionMapPreviewData preview,
      out IWorldRegionMapAdditionalData previewExtras,
      out IWorldRegionMap map)
    {
      preview = (IWorldRegionMapPreviewData) null;
      previewExtras = (IWorldRegionMapAdditionalData) null;
      map = (IWorldRegionMap) null;
      ulong num;
      do
      {
        num = reader.ReadULongNonVariable();
        if ((long) num == (long) MapSerializer.HEADER_MAP_PREVIEW)
        {
          preview = reader.ReadGenericAs<IWorldRegionMapPreviewData>();
          reader.FinalizeLoading(Option<DependencyResolver>.None);
        }
        else if ((long) num == (long) MapSerializer.HEADER_MAP_EXTRA)
        {
          previewExtras = reader.ReadGenericAs<IWorldRegionMapAdditionalData>();
          reader.FinalizeLoading(Option<DependencyResolver>.None);
        }
        else if ((long) num == (long) MapSerializer.HEADER_MAP_DATA)
        {
          map = reader.ReadGenericAs<IWorldRegionMap>();
          reader.FinalizeLoading(Option<DependencyResolver>.None);
        }
        else
        {
          if ((long) num != (long) SaveHeaders.HEADER_SAVE_END_LE)
            throw new CorruptedSaveException(string.Format("Unknown header '{0}' (0x{1:X16}) in the save file.", (object) SaveHeaders.HeaderToAscii(num), (object) num));
          if ((long) finalReadHeader != (long) SaveHeaders.HEADER_SAVE_END_LE)
            throw new CorruptedSaveException("Failed to find header '" + SaveHeaders.HeaderToAscii(num) + "' " + string.Format("(0x{0:X16}) in the save file.", (object) num));
          goto label_12;
        }
      }
      while ((long) num != (long) finalReadHeader);
      goto label_13;
label_12:
      return;
label_13:;
    }

    public bool TryLoadPreviewMinimalFromFile(
      string filePath,
      out IWorldRegionMapPreviewData previewData,
      out Option<Exception> exception)
    {
      try
      {
        filePath = Path.GetFullPath(filePath);
        using (FileStream fileStream = File.OpenRead(filePath))
        {
          previewData = this.LoadPreviewMinimal((Stream) fileStream);
          previewData.SetMapFilePath(filePath);
          exception = Option<Exception>.None;
          return true;
        }
      }
      catch (Exception ex)
      {
        exception = (Option<Exception>) ex;
        previewData = (IWorldRegionMapPreviewData) null;
        return false;
      }
    }

    public IWorldRegionMapPreviewData LoadPreviewMinimal(Stream stream)
    {
      IWorldRegionMapPreviewData preview;
      IWorldRegionMapAdditionalData previewExtras;
      IWorldRegionMap map;
      this.continueLoadUntilHeader(this.startMapLoad(stream), MapSerializer.HEADER_MAP_PREVIEW, out preview, out previewExtras, out map);
      Mafi.Assert.That<IWorldRegionMapAdditionalData>(previewExtras).IsNull<IWorldRegionMapAdditionalData>("Unexpected chunk ordering.");
      Mafi.Assert.That<IWorldRegionMap>(map).IsNull<IWorldRegionMap>("Unexpected chunk ordering.");
      return preview;
    }

    public IWorldRegionMapAdditionalData LoadPreviewFull(
      Stream stream,
      bool skipNewVersionCheck,
      out IWorldRegionMapPreviewData preview)
    {
      IWorldRegionMapAdditionalData previewExtras;
      IWorldRegionMap map;
      this.continueLoadUntilHeader(this.startMapLoad(stream, skipNewVersionCheck), MapSerializer.HEADER_MAP_EXTRA, out preview, out previewExtras, out map);
      Mafi.Assert.That<IWorldRegionMap>(map).IsNull<IWorldRegionMap>("Unexpected chunk ordering.");
      return previewExtras;
    }

    public bool TryLoadMapFromFile(
      string filePath,
      out IWorldRegionMapPreviewData preview,
      out IWorldRegionMapAdditionalData previewExtras,
      out IWorldRegionMap map,
      out Option<Exception> exception)
    {
      try
      {
        filePath = Path.GetFullPath(filePath);
        using (FileStream fileStream = File.OpenRead(filePath))
        {
          map = this.LoadMap((Stream) fileStream, out preview, out previewExtras);
          preview.SetMapFilePath(filePath);
          exception = Option<Exception>.None;
          return true;
        }
      }
      catch (Exception ex)
      {
        exception = (Option<Exception>) ex;
        preview = (IWorldRegionMapPreviewData) null;
        previewExtras = (IWorldRegionMapAdditionalData) null;
        map = (IWorldRegionMap) null;
        return false;
      }
    }

    public IWorldRegionMap LoadMap(
      Stream stream,
      out IWorldRegionMapPreviewData preview,
      out IWorldRegionMapAdditionalData previewExtras)
    {
      IWorldRegionMap map;
      this.continueLoadUntilHeader(this.startMapLoad(stream), SaveHeaders.HEADER_SAVE_END_LE, out preview, out previewExtras, out map);
      return map;
    }

    public void Reset()
    {
      if (!this.m_mainWriter.HasValue)
        return;
      this.m_mainWriter.Value.Dispose();
      this.m_mainWriter = Option<MemoryBlobWriter>.None;
    }

    static MapSerializer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MapSerializer.HEADER_MAP_ASCII = SaveHeaders.CreateHeaderV2("MaFiMapa");
      MapSerializer.HEADER_MAP = SaveHeaders.HeaderToUlong(MapSerializer.HEADER_MAP_ASCII);
      MapSerializer.HEADER_MAP_PREVIEW_ASCII = SaveHeaders.CreateHeaderV2("MPreview");
      MapSerializer.HEADER_MAP_PREVIEW = SaveHeaders.HeaderToUlong(MapSerializer.HEADER_MAP_PREVIEW_ASCII);
      MapSerializer.HEADER_MAP_EXTRA_ASCII = SaveHeaders.CreateHeaderV2("MapExtra");
      MapSerializer.HEADER_MAP_EXTRA = SaveHeaders.HeaderToUlong(MapSerializer.HEADER_MAP_EXTRA_ASCII);
      MapSerializer.HEADER_MAP_DATA_ASCII = SaveHeaders.CreateHeaderV2("MapaData");
      MapSerializer.HEADER_MAP_DATA = SaveHeaders.HeaderToUlong(MapSerializer.HEADER_MAP_DATA_ASCII);
    }
  }
}
