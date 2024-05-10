// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.GameLoader
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Mafi.Core.SaveGame
{
  /// <remarks>
  /// See <see cref="M:Mafi.Core.Game.GameBuilder.BuildLoadedGameTimeSliced(Mafi.Core.Game.LoadGameArgs,System.Boolean,System.Func{Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.SaveGame.ModInfoRaw},Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Mods.ModData}},System.Action{Mafi.DependencyResolver},System.Action{Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Mods.IMod},Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Game.IConfig}},System.Action{Mafi.Core.Mods.ProtoRegistrator,Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Mods.IMod}},System.Action{Mafi.Core.Mods.ProtoRegistrator,Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Mods.IMod}},System.Action{Mafi.DependencyResolverBuilder,Mafi.Core.Prototypes.ProtosDb,Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Core.Mods.IMod}},System.Action{Mafi.DependencyResolver})" /> for loading procedure.
  /// Save file is structured in chunks. Each chunk has 8 byte header. Some chunks may be optional.
  /// 
  /// <code>
  /// Chunk #1 <see cref="F:Mafi.Core.SaveGame.SaveHeaders.HEADER_MAIN" /> (mandatory)
  ///  - Serialized <see cref="T:Mafi.Core.SaveGame.SaveHeader" /> that has save version and compression type. This chunk is read
  ///    before <see cref="T:Mafi.Serialization.BlobReader" /> instance is created.
  ///  - All data after this chunk are encoded with compressor based on <see cref="F:Mafi.Core.SaveGame.SaveHeader.CompressionType" />.
  /// 
  /// Chunk #2 <see cref="F:Mafi.Core.SaveGame.SaveHeaders.HEADER_MOD_TYPES" /> (mandatory)
  ///  - Serialized mod types using <see cref="T:Mafi.Core.SaveGame.ModsListHelper" />. We need to have a list of mods at the very
  ///    beginning to instantiate configs from all assemblies of all mods.
  ///  - Order of mods does not matter.
  /// 
  /// TODO: This is no longer a valid requirement, and if someone subscribes to a config for its changes they end up being
  /// resolved without a working resolver thanks to this foot-gun. Example is subscribing a LoansManager
  /// on DifficultyConfig event for config changes. Most likely the configs should stop receiving this special treatment.
  /// Chunk #3 <see cref="F:Mafi.Core.SaveGame.SaveHeaders.HEADER_CONFIGS" /> (mandatory)
  ///  - All persistent configs (implementations of <see cref="T:Mafi.Core.Game.IConfig" /> that are serializable). We need all configs
  ///    to be able to instantiate mods and create dependency resolver.
  ///  - Serialized simply as <see cref="T:Mafi.Collections.ImmutableCollections.ImmutableArray`1" />.
  /// 
  /// Chunk #4 <see cref="F:Mafi.Core.SaveGame.SaveHeaders.HEADER_RESOLVER" /> (mandatory)
  ///  - Saved instance of <see cref="T:Mafi.DependencyResolver" /> that effectively saves the entire game.
  /// 
  /// Chunk #5 <see cref="F:Mafi.Core.SaveGame.SaveHeaders.HEADER_SAVE_END" /> (mandatory)
  ///  - Final chunk that verifies correctness of the load operation. This chunk has no contents, just a header.
  /// </code>
  /// </remarks>
  [OnlyForSaveCompatibility(null)]
  internal class GameLoader
  {
    public const int REPORT_PROGRESS_STEPS = 20;
    private static readonly KeyValuePair<SaveCompressionType, ISaveCompressor>[] COMPRESSORS;

    public static bool TryGetCompressor(
      SaveCompressionType compressionType,
      out ISaveCompressor compressor)
    {
      foreach (KeyValuePair<SaveCompressionType, ISaveCompressor> keyValuePair in GameLoader.COMPRESSORS)
      {
        if (keyValuePair.Key == compressionType)
        {
          compressor = keyValuePair.Value;
          return true;
        }
      }
      compressor = (ISaveCompressor) null;
      return false;
    }

    public GameLoader()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private BlobReader startGameLoad(Stream stream, ulong? alreadyReadHeader = null)
    {
      SaveHeader saveHeader = SaveHeader.DeserializeCustom(stream, alreadyReadHeader);
      if ((long) saveHeader.Header != (long) SaveHeaders.HEADER_MAIN_LE)
        throw new CorruptedSaveException("Invalid save file header " + SaveHeaders.HeaderToStringInclHex(saveHeader.Header) + ", expected " + SaveHeaders.HeaderToStringInclHex(SaveHeaders.HEADER_MAIN_LE) + ".");
      ISaveCompressor compressor;
      if (!GameLoader.TryGetCompressor(saveHeader.CompressionType, out compressor))
        throw new CorruptedSaveException(string.Format("Fail to load save compressor with ID '{0}'.", (object) saveHeader.CompressionType));
      return new BlobReader(compressor.CreateDecompressingStream(stream), saveHeader.Version);
    }

    private void continueLoadUntilHeader(
      BlobReader reader,
      ulong finalReadHeader,
      out ImmutableArray<ModInfoRaw> modsInfo,
      out GameSaveInfo saveInfo,
      out ImmutableArray<IConfig> configs)
    {
      modsInfo = new ImmutableArray<ModInfoRaw>();
      saveInfo = (GameSaveInfo) null;
      configs = (ImmutableArray<IConfig>) (ImmutableArray.EmptyArray) null;
      ulong num;
      do
      {
        num = reader.ReadULongNonVariable();
        if ((long) num == (long) SaveHeaders.HEADER_MOD_TYPES_LE)
        {
          modsInfo = ModsListHelper.DeserializeCustom(reader);
          reader.FinalizeLoading(Option<DependencyResolver>.None);
        }
        else if ((long) num == (long) SaveHeaders.HEADER_SAVE_INFO_LE)
        {
          saveInfo = GameSaveInfo.Deserialize(reader);
          reader.FinalizeLoading(Option<DependencyResolver>.None);
        }
        else
        {
          if ((long) num != (long) SaveHeaders.HEADER_CONFIGS_LE)
            throw new CorruptedSaveException(string.Format("Unknown header '{0}' (0x{1:X16}) in the save file.", (object) SaveHeaders.HeaderToAscii(num), (object) num));
          if (saveInfo == null)
            saveInfo = GameSaveInfo.Empty;
          if ((long) finalReadHeader == (long) SaveHeaders.HEADER_SAVE_INFO_LE)
            break;
          configs = ImmutableArray<IConfig>.Deserialize(reader);
          reader.FinalizeLoading(Option<DependencyResolver>.None);
        }
      }
      while ((long) num != (long) finalReadHeader);
    }

    public static SaveChecksumValidationResults ValidateChecksumSafe(
      Stream stream,
      out SaveHeader headers,
      out Option<Exception> exception,
      bool alsoValidateNonCompressedData = false)
    {
      headers = new SaveHeader();
      exception = Option<Exception>.None;
      try
      {
        headers = SaveHeader.DeserializeCustom(stream);
        if (headers.DataSize <= 0L)
          return SaveChecksumValidationResults.Success;
        long position = stream.Position;
        long readBytes1;
        uint num1 = Crc32.Compute(stream, out readBytes1);
        if (headers.DataSize != readBytes1)
          return SaveChecksumValidationResults.FailDataSize;
        if ((int) num1 != (int) headers.DataCrc32Checksum)
          return SaveChecksumValidationResults.FailChecksum;
        if (alsoValidateNonCompressedData)
        {
          stream.Seek(position, SeekOrigin.Begin);
          ISaveCompressor compressor;
          if (!GameLoader.TryGetCompressor(headers.CompressionType, out compressor))
            throw new CorruptedSaveException(string.Format("Fail to load save compressor with ID '{0}'.", (object) headers.CompressionType));
          long readBytes2;
          uint num2;
          using (Stream decompressingStream = compressor.CreateDecompressingStream(stream))
            num2 = Crc32.Compute(decompressingStream, out readBytes2);
          if (headers.DataSizeBeforeCompression != readBytes2)
            return SaveChecksumValidationResults.FailDataSizeBeforeCompression;
          if ((int) num2 != (int) headers.DataCrc32ChecksumBeforeCompression)
            return SaveChecksumValidationResults.FailChecksumBeforeCompression;
        }
      }
      catch (Exception ex)
      {
        exception = (Option<Exception>) ex;
        return SaveChecksumValidationResults.FailException;
      }
      return SaveChecksumValidationResults.Success;
    }

    public ImmutableArray<ModInfoRaw> TryLoadMods_MayThrow(Stream stream, out int saveVersion)
    {
      BlobReader reader = this.startGameLoad(stream);
      saveVersion = reader.LoadedSaveVersion;
      ImmutableArray<ModInfoRaw> modsInfo;
      this.continueLoadUntilHeader(reader, SaveHeaders.HEADER_MOD_TYPES_LE, out modsInfo, out GameSaveInfo _, out ImmutableArray<IConfig> _);
      return modsInfo;
    }

    public GameSaveInfo TryLoadSaveInfo_MayThrow(
      Stream stream,
      out ImmutableArray<ModInfoRaw> modsInfo,
      out int saveVersion)
    {
      BlobReader reader = this.startGameLoad(stream);
      saveVersion = reader.LoadedSaveVersion;
      GameSaveInfo saveInfo;
      this.continueLoadUntilHeader(reader, SaveHeaders.HEADER_SAVE_INFO_LE, out modsInfo, out saveInfo, out ImmutableArray<IConfig> _);
      return saveInfo;
    }

    public BlobReader StartGameLoad(
      Stream stream,
      out ImmutableArray<ModInfoRaw> modsInfo,
      out ImmutableArray<IConfig> configs,
      ulong? alreadyReadHeader = null)
    {
      BlobReader reader = this.startGameLoad(stream, alreadyReadHeader);
      if (!SaveVersion.IsCompatibleSaveVersion(reader.LoadedSaveVersion))
        throw new IncompatibleSaveVersionException(reader.LoadedSaveVersion, 96, 168);
      Log.Info(string.Format("Loading game from save version {0} ", (object) reader.LoadedSaveVersion) + string.Format("(current is {0}).", (object) 168));
      reader.SetSpecialSerializers(SpecialSerializerFactories.GetSerializersForConfigs());
      this.continueLoadUntilHeader(reader, SaveHeaders.HEADER_CONFIGS_LE, out modsInfo, out GameSaveInfo _, out configs);
      if (reader.LoadedSaveVersion < 153)
        configs = configs.RemoveAll((Predicate<IConfig>) (x => x.GetType().FullName == "Mafi.Base.Terrain.PostProcessors.ParticleErosionPostProcessor+Configuration"));
      return reader;
    }

    /// <summary>Returns a loaded resolver.</summary>
    /// <remarks>
    /// Taking <see cref="T:Mafi.DependencyResolverBuilder" /> as an argument ensures that the new resolver was not used
    /// for resolving yet.
    /// </remarks>
    public IEnumerator<string> FinishGameLoadAndDisposeTimeSliced(
      BlobReader reader,
      DependencyResolverBuilder resolverBuilder,
      SpecialSerializerFactories specialSerializers,
      Action<DependencyResolver> setResolver,
      Action<DependencyResolver> beforeInitCalls)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new GameLoader.\u003CFinishGameLoadAndDisposeTimeSliced\u003Ed__10(0)
      {
        reader = reader,
        resolverBuilder = resolverBuilder,
        specialSerializers = specialSerializers,
        setResolver = setResolver,
        beforeInitCalls = beforeInitCalls
      };
    }

    static GameLoader()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameLoader.COMPRESSORS = new KeyValuePair<SaveCompressionType, ISaveCompressor>[2]
      {
        Make.Kvp<SaveCompressionType, ISaveCompressor>(SaveCompressionType.NoCompression, (ISaveCompressor) new PassThroughSaveCompressor()),
        Make.Kvp<SaveCompressionType, ISaveCompressor>(SaveCompressionType.Gzip, (ISaveCompressor) new GzipSaveCompressor())
      };
    }
  }
}
