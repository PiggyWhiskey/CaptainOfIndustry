// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.SerializationUtils
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System.IO;

#nullable disable
namespace Mafi.Core.Utils
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class SerializationUtils
  {
    private readonly Option<DependencyResolver> m_resolver;
    private readonly ImmutableArray<ISpecialSerializerFactory> m_specialSerializers;
    private Option<MemoryBlobWriter> m_snapshotWriter;
    private Option<BlobReader> m_snapshotReader;

    public SerializationUtils(ProtosDb protosDb, Option<DependencyResolver> resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
      this.m_specialSerializers = ImmutableArray.Create<ISpecialSerializerFactory>((ISpecialSerializerFactory) new ProtosSerializerFactory(protosDb));
    }

    public byte[] Serialize<T>(T obj)
    {
      MemoryBlobWriter memoryBlobWriter = this.m_snapshotWriter.ValueOrNull;
      if (memoryBlobWriter == null)
        this.m_snapshotWriter = (Option<MemoryBlobWriter>) (memoryBlobWriter = new MemoryBlobWriter(new ImmutableArray<ISpecialSerializerFactory>?(this.m_specialSerializers)));
      else
        memoryBlobWriter.Reset();
      memoryBlobWriter.WriteGeneric<T>(obj);
      memoryBlobWriter.FinalizeSerialization();
      return memoryBlobWriter.ToArray();
    }

    public T Deserialize<T>(byte[] data)
    {
      BlobReader blobReader = this.m_snapshotReader.ValueOrNull;
      if (blobReader == null)
        this.m_snapshotReader = (Option<BlobReader>) (blobReader = new BlobReader((Stream) new MemoryStream(), 168, this.m_specialSerializers));
      else
        blobReader.Reset();
      MemoryStream inputStream = (MemoryStream) blobReader.InputStream;
      inputStream.SetLength(0L);
      inputStream.Write(data, 0, data.Length);
      inputStream.Position = 0L;
      T obj = blobReader.ReadGenericAs<T>();
      blobReader.FinalizeLoading(this.m_resolver);
      return obj;
    }

    public T DeepCopyViaSerialization<T>(T obj)
    {
      MemoryBlobWriter memoryBlobWriter = this.m_snapshotWriter.ValueOrNull;
      if (memoryBlobWriter == null)
        this.m_snapshotWriter = (Option<MemoryBlobWriter>) (memoryBlobWriter = new MemoryBlobWriter(new ImmutableArray<ISpecialSerializerFactory>?(this.m_specialSerializers)));
      else
        memoryBlobWriter.Reset();
      memoryBlobWriter.WriteGeneric<T>(obj);
      memoryBlobWriter.FinalizeSerialization();
      MemoryStream baseStream = memoryBlobWriter.BaseStream;
      baseStream.Position = 0L;
      BlobReader blobReader = new BlobReader((Stream) baseStream, 168, this.m_specialSerializers);
      T obj1 = blobReader.ReadGenericAs<T>();
      blobReader.FinalizeLoading(this.m_resolver);
      return obj1;
    }
  }
}
