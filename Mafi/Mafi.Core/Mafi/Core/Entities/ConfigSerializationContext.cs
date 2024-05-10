// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.ConfigSerializationContext
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Serialization;
using System;
using System.IO;
using System.Threading;

#nullable disable
namespace Mafi.Core.Entities
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ConfigSerializationContext
  {
    internal readonly UnlockedProtosDb UnlockedProtosDb;
    internal readonly ProtosDb ProtosDb;
    internal readonly TileSurfaceDecalsSlimIdManager TileSurfaceDecalsSlimIdManager;
    private readonly ThreadLocal<MemoryStream> m_memStream;
    private readonly ThreadLocal<BlobWriter> m_writer;
    private readonly ThreadLocal<BlobReader> m_reader;

    public ConfigSerializationContext(
      UnlockedProtosDb unlockedProtosDb,
      ProtosDb protosDb,
      TileSurfaceDecalsSlimIdManager tileSurfaceDecalsSlimIdManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ConfigSerializationContext serializationContext = this;
      this.UnlockedProtosDb = unlockedProtosDb;
      this.TileSurfaceDecalsSlimIdManager = tileSurfaceDecalsSlimIdManager;
      this.ProtosDb = protosDb;
      this.m_memStream = new ThreadLocal<MemoryStream>((Func<MemoryStream>) (() => new MemoryStream()));
      this.m_writer = new ThreadLocal<BlobWriter>((Func<BlobWriter>) (() => new BlobWriter((Stream) serializationContext.m_memStream.Value, new ImmutableArray<ISpecialSerializerFactory>?(ImmutableArray.Create<ISpecialSerializerFactory>((ISpecialSerializerFactory) new ProtosSerializerFactory(protosDb))))));
      this.m_reader = new ThreadLocal<BlobReader>((Func<BlobReader>) (() =>
      {
        BlobReader blobReader = new BlobReader((Stream) serializationContext.m_memStream.Value, 168);
        blobReader.SetSpecialSerializers(ImmutableArray.Create<ISpecialSerializerFactory>((ISpecialSerializerFactory) new ProtosSerializerFactory(protosDb)));
        return blobReader;
      }));
    }

    internal void Set<T>(
      Dict<string, ImmutableArray<byte>> data,
      string key,
      T value,
      Action<T, BlobWriter> writeFunc)
    {
      try
      {
        MemoryStream stream = this.m_memStream.Value;
        BlobWriter blobWriter = this.m_writer.Value;
        stream.SetLength(0L);
        writeFunc(value, blobWriter);
        blobWriter.FinalizeSerialization();
        blobWriter.Reset();
        data[key] = stream.ToImmutableArray();
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to set param '" + key + "' to the config");
      }
    }

    internal bool TryReadFromBytes<T>(
      ImmutableArray<byte> data,
      string key,
      Func<BlobReader, T> readFunc,
      out T result)
    {
      try
      {
        MemoryStream memoryStream = this.m_memStream.Value;
        BlobReader blobReader = this.m_reader.Value;
        memoryStream.SetLength(0L);
        ImmutableArray<byte>.Enumerator enumerator = data.GetEnumerator();
        while (enumerator.MoveNext())
        {
          byte current = enumerator.Current;
          memoryStream.WriteByte(current);
        }
        memoryStream.Seek(0L, SeekOrigin.Begin);
        result = readFunc(blobReader);
        blobReader.FinalizeLoading(Option<DependencyResolver>.None);
        blobReader.Reset();
        return true;
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to read param '" + key + "' from the config");
      }
      result = default (T);
      return false;
    }

    internal void SetArray<T>(
      Dict<string, ImmutableArray<byte>> data,
      string key,
      ImmutableArray<T> array,
      Action<T, BlobWriter> writeFunc)
    {
      try
      {
        MemoryStream stream = this.m_memStream.Value;
        BlobWriter blobWriter = this.m_writer.Value;
        stream.SetLength(0L);
        blobWriter.WriteIntNotNegative(array.Length);
        for (int index = 0; index < array.Length; ++index)
          writeFunc(array[index], blobWriter);
        blobWriter.FinalizeSerialization();
        blobWriter.Reset();
        data[key] = stream.ToImmutableArray();
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to set param '" + key + "' to the config");
      }
    }

    internal ImmutableArray<T> ArrayFromBytes<T>(
      ImmutableArray<byte> data,
      string key,
      Func<BlobReader, T> readFunc)
    {
      try
      {
        MemoryStream memoryStream = this.m_memStream.Value;
        BlobReader blobReader = this.m_reader.Value;
        memoryStream.SetLength(0L);
        ImmutableArray<byte>.Enumerator enumerator = data.GetEnumerator();
        while (enumerator.MoveNext())
        {
          byte current = enumerator.Current;
          memoryStream.WriteByte(current);
        }
        memoryStream.Seek(0L, SeekOrigin.Begin);
        int length = blobReader.ReadIntNotNegative();
        ImmutableArrayBuilder<T> immutableArrayBuilder = new ImmutableArrayBuilder<T>(length);
        for (int i = 0; i < length; ++i)
          immutableArrayBuilder[i] = readFunc(blobReader);
        blobReader.FinalizeLoading(Option<DependencyResolver>.None);
        blobReader.Reset();
        return immutableArrayBuilder.GetImmutableArrayAndClear();
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to read param '" + key + "' from the config");
      }
      return new ImmutableArray<T>();
    }
  }
}
