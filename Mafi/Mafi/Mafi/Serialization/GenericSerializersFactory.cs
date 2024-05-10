// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.GenericSerializersFactory
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Serialization
{
  /// <summary>
  /// Allows serialization of any objects in cases where serialized types cannot be determined during compile time,
  /// for example interfaces or members of generic types.
  /// 
  /// IMPORTANT: If this class is used for serialization, it also MUST be used for deserialization, since in some
  /// cases more data is written/read to/from the data stream.
  /// </summary>
  /// <remarks>
  /// All built-in types are directly written/read to/from the stream, structs are not boxed.
  /// For other types calls `Serialize` and `Deserialize` are made via compiled lambda expressions.
  /// If a type is not built-in and does not have `Serialize` and `Deserialize` methods, exception will be thrown
  /// during serializer class creation.
  /// </remarks>
  public static class GenericSerializersFactory
  {
    public static Action<T, BlobWriter> GetSerializeAction<T>()
    {
      return GenericSerializersFactory.createSerializer<T>().SerializeAction;
    }

    public static Func<BlobReader, T> GetDeserializeFunction<T>()
    {
      return GenericSerializersFactory.createSerializer<T>().DeserializeFunction;
    }

    private static IGenericSerializer<T> createSerializer<T>()
    {
      Type type = typeof (T);
      if (type.IsValueType)
      {
        if (type.IsGenericType)
        {
          Type genericTypeDefinition = type.GetGenericTypeDefinition();
          if (genericTypeDefinition == typeof (Nullable<>))
            return (IGenericSerializer<T>) NullableStructSerializer<T>.Instance;
          if (genericTypeDefinition == typeof (KeyValuePair<,>))
            return (IGenericSerializer<T>) KeyValuePairSerializer<T>.Instance;
        }
        if (type.IsEnum)
          return (IGenericSerializer<T>) EnumSerializer<T>.Instance;
        if (type == typeof (bool))
          return (IGenericSerializer<T>) BoolSerializer.Instance;
        if (type == typeof (byte))
          return (IGenericSerializer<T>) ByteSerializer.Instance;
        if (type == typeof (sbyte))
          return (IGenericSerializer<T>) SByteSerializer.Instance;
        if (type == typeof (char))
          return (IGenericSerializer<T>) CharSerializer.Instance;
        if (type == typeof (short))
          return (IGenericSerializer<T>) ShortSerializer.Instance;
        if (type == typeof (ushort))
          return (IGenericSerializer<T>) UShortSerializer.Instance;
        if (type == typeof (int))
          return (IGenericSerializer<T>) IntSerializer.Instance;
        if (type == typeof (uint))
          return (IGenericSerializer<T>) UIntSerializer.Instance;
        if (type == typeof (long))
          return (IGenericSerializer<T>) LongSerializer.Instance;
        if (type == typeof (ulong))
          return (IGenericSerializer<T>) ULongSerializer.Instance;
        if (type == typeof (float))
          return (IGenericSerializer<T>) FloatSerializer.Instance;
        return type == typeof (double) ? (IGenericSerializer<T>) DoubleSerializer.Instance : (IGenericSerializer<T>) DirectCallSerializer<T>.Instance;
      }
      if (type.IsArray)
        return (IGenericSerializer<T>) ArraySerializer<T>.Instance;
      if (type == typeof (Type))
        return (IGenericSerializer<T>) TypeSerializer.Instance;
      if (type == typeof (string))
        return (IGenericSerializer<T>) StringSerializer.Instance;
      return type.IsInterface || type.IsAbstract || type == typeof (object) ? (IGenericSerializer<T>) DynamicDispatchClassSerializer<T>.Instance : (IGenericSerializer<T>) DirectCallSerializer<T>.Instance;
    }
  }
}
