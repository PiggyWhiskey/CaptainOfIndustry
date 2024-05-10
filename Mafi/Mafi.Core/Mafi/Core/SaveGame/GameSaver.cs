// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.GameSaver
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Mafi.Core.SaveGame
{
  /// <remarks>
  /// See <see cref="T:Mafi.Core.SaveGame.GameLoader" /> for save file structure.
  /// </remarks>
  internal class GameSaver : IAsyncSavable
  {
    private readonly ImmutableArray<IMod> m_mods;
    private readonly ImmutableArray<IConfig> m_configs;
    private readonly ImmutableArray<ISpecialSerializerFactory> m_specialSerializersForConfigs;
    private readonly ImmutableArray<ISpecialSerializerFactory> m_specialSerializersForGame;
    private readonly IGameSaveInfoProvider m_gameSaveInfoProvider;
    private readonly Stopwatch m_stopwatch;
    private SaveCompressionType m_compressionType;
    private Option<MemoryBlobWriter> m_mainWriter;

    public TimeSpan LastSaveStartDuration { get; private set; }

    public TimeSpan LastSaveFinalizeDuration { get; private set; }

    public GameSaver(
      ImmutableArray<IMod> mods,
      ImmutableArray<IConfig> configs,
      ImmutableArray<ISpecialSerializerFactory> specialSerializersForConfigs,
      ImmutableArray<ISpecialSerializerFactory> specialSerializersForGame,
      IGameSaveInfoProvider gameSaveInfoProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_stopwatch = new Stopwatch();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_mods = mods;
      this.m_configs = configs;
      this.m_specialSerializersForConfigs = specialSerializersForConfigs;
      this.m_specialSerializersForGame = specialSerializersForGame;
      this.m_gameSaveInfoProvider = gameSaveInfoProvider;
    }

    /// <summary>
    /// Starts the saving process by writing all game data to internal streams. This effectively takes a snapshot of
    /// the game so nothing else can run. The <see cref="M:Mafi.Core.SaveGame.GameSaver.FinishSaveWriteToStream(System.IO.Stream)" /> must be called after this to combine
    /// internal streams and optionally compress them.
    /// </summary>
    public void StartSave(DependencyResolver resolver, SaveCompressionType saveCompressionType)
    {
      StateAssert.IsInSimState(SimLoopState.None);
      Mafi.Assert.That<Option<MemoryBlobWriter>>(this.m_mainWriter).IsNone<MemoryBlobWriter>("Save was already started!");
      this.Reset();
      this.m_stopwatch.Restart();
      MemoryBlobWriter writer = new MemoryBlobWriter(new ImmutableArray<ISpecialSerializerFactory>?(this.m_specialSerializersForConfigs));
      writer.WriteULongNonVariable(SaveHeaders.HEADER_MOD_TYPES_LE);
      ModsListHelper.SerializeCustom(this.m_mods, (BlobWriter) writer);
      writer.FinalizeSerialization();
      writer.WriteULongNonVariable(SaveHeaders.HEADER_SAVE_INFO_LE);
      GameSaveInfo.Serialize(this.m_gameSaveInfoProvider.CreateGameSaveInfo(), (BlobWriter) writer);
      writer.FinalizeSerialization();
      writer.WriteULongNonVariable(SaveHeaders.HEADER_CONFIGS_LE);
      ImmutableArray<IConfig>.Serialize(this.m_configs, (BlobWriter) writer);
      writer.FinalizeSerialization();
      writer.SetSpecialSerializers(this.m_specialSerializersForGame);
      writer.WriteULongNonVariable(SaveHeaders.HEADER_RESOLVER_LE);
      DependencyResolver.Serialize(resolver, (BlobWriter) writer);
      writer.FinalizeSerialization();
      writer.WriteULongNonVariable(SaveHeaders.HEADER_SAVE_END_LE);
      this.m_mainWriter = (Option<MemoryBlobWriter>) writer;
      this.m_compressionType = saveCompressionType;
      this.m_stopwatch.Stop();
      this.LastSaveStartDuration = this.m_stopwatch.Elapsed;
      Mafi.Log.Info("GameSaver: Saving took " + ((float) this.m_stopwatch.Elapsed.TotalMilliseconds).RoundToSigDigits(3, false, false, false) + " ms");
    }

    /// <summary>
    /// Finishes the saving process started in <see cref="M:Mafi.Core.SaveGame.GameSaver.StartSave(Mafi.DependencyResolver,Mafi.Core.SaveGame.SaveCompressionType)" /> by writing the previously obtained snapshot
    /// into the output stream.
    /// Note: This does not clear save data and can be called multiple times. Call <see cref="M:Mafi.Core.SaveGame.GameSaver.Reset" /> when internal
    /// data are not needed anymore.
    /// </summary>
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
        GameSaver.finishSaveWriteToStreamNoState(outputStream, valueOrNull, compressionType);
        this.LastSaveFinalizeDuration = stopwatch.Elapsed;
        Mafi.Log.Info("GameSaver: Save finalize took " + ((float) stopwatch.Elapsed.TotalMilliseconds).RoundToSigDigits(3, false, false, false) + " ms");
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
      {
        Mafi.Log.Error(string.Format("Unknown compression '{0}', falling back to no compression.", (object) compression));
        compressor = (ISaveCompressor) null;
        compression = SaveCompressionType.NoCompression;
      }
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
      new SaveHeader(SaveHeaders.HEADER_MAIN_LE, 168, compression, stream.Length, dataCrc32Checksum, readBytes1, dataCrc32ChecksumBeforeCompression).SerializeCustom(outputStream);
      stream.CopyToFast(outputStream);
    }

    public void Reset()
    {
      if (!this.m_mainWriter.HasValue)
        return;
      this.m_mainWriter.Value.Dispose();
      this.m_mainWriter = Option<MemoryBlobWriter>.None;
    }
  }
}
