// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.ProtosSerializerFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public sealed class ProtosSerializerFactory : ISpecialSerializerFactory
  {
    private readonly ProtosDb m_protosDb;
    private readonly Dict<Proto.ID, Proto> m_phantoms;
    private readonly Dict<Type, Proto> m_typeToPhantoms;
    private readonly Func<BlobReader, Type, Proto> m_customNewObjFactory;

    public ProtosSerializerFactory(ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_phantoms = Proto.AllPhantoms.AsEnumerable().ToDict<Proto, Proto.ID, Proto>((Func<Proto, Proto.ID>) (x => x.Id), (Func<Proto, Proto>) (x => x));
      Assert.That<int>(this.m_phantoms.Count).IsEqualTo(Proto.AllPhantoms.Count, "More than one phantom with identical ID.");
      this.m_typeToPhantoms = Proto.AllPhantoms.AsEnumerable().ToDict<Proto, Type, Proto>((Func<Proto, Type>) (x => x.GetType()), (Func<Proto, Proto>) (x => x));
      Assert.That<int>(this.m_typeToPhantoms.Count).IsEqualTo(Proto.AllPhantoms.Count, "More than one phantom for one proto type.");
      this.m_customNewObjFactory = new Func<BlobReader, Type, Proto>(this.deserializeNewProto);
    }

    public bool CanSerialize<T>() => typeof (T).IsAssignableTo<Proto>();

    public Action<T, BlobWriter> GetSerializeAction<T>()
    {
      Assert.That<Type>(typeof (T)).IsAssignableTo<Proto>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return ProtosSerializerFactory.\u003CGetSerializeAction\u003EO__6_0<T>.\u003C0\u003E__serialize ?? (ProtosSerializerFactory.\u003CGetSerializeAction\u003EO__6_0<T>.\u003C0\u003E__serialize = new Action<T, BlobWriter>(ProtosSerializerFactory.serialize<T>));
    }

    private static void serialize<T>(T proto, BlobWriter writer)
    {
      Proto proto1 = (Proto) (object) proto;
      if (!writer.TryStartClassSerialization<Proto>(proto1))
        return;
      Assert.That<string>(proto1.Id.Value).IsNotNullOrEmpty("Serializing proto with empty or null string ID. It will be deserialized as null!");
      writer.WriteStringNoRef(proto1.Id.Value);
    }

    public Func<BlobReader, T> GetDeserializeFunction<T>()
    {
      Assert.That<Type>(typeof (T)).IsAssignableTo<Proto>();
      return new Func<BlobReader, T>(this.deserialize<T>);
    }

    private T deserialize<T>(BlobReader reader)
    {
      Proto proto;
      reader.TryStartClassDeserialization<Proto>(out proto, this.m_customNewObjFactory);
      if (proto is T)
        return proto as T;
      if (proto == (Proto) null)
        return default (T);
      throw new CorruptedSaveException(string.Format("Failed cast loaded proto '{0}' to '{1}'.", (object) proto, (object) typeof (T).Name));
    }

    private Proto deserializeNewProto(BlobReader reader, Type protoType)
    {
      string str = reader.ReadStringNoRef();
      if (string.IsNullOrEmpty(str))
        return (Proto) null;
      Proto.ID id = new Proto.ID(str);
      Proto proto1;
      if (this.m_protosDb.TryGetProto<Proto>(id, out proto1))
        return proto1;
      Proto proto2;
      if (this.m_phantoms.TryGetValue(id, out proto2))
        return proto2;
      if (this.m_typeToPhantoms.TryGetValue(protoType, out proto2))
      {
        Log.Warning("Failed find proto '" + id.Value + "' (" + protoType.Name + "), returning phantom proto.");
        return proto2;
      }
      throw new CorruptedSaveException("Failed to find proto '" + id.Value + "' (" + protoType.Name + ") and no phantom proto of this type exists.");
    }
  }
}
