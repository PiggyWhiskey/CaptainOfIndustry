// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.NullableStructSerializer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class NullableStructSerializer<T> : IGenericSerializer<T>
  {
    private static NullableStructSerializer<T> s_instance;

    public static NullableStructSerializer<T> Instance
    {
      get
      {
        return NullableStructSerializer<T>.s_instance ?? (NullableStructSerializer<T>.s_instance = new NullableStructSerializer<T>());
      }
    }

    public Action<T, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, T> DeserializeFunction { get; }

    private NullableStructSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Type type = typeof (T);
      Type[] typeArguments = type.IsValueType && !(type.GetGenericTypeDefinition() != typeof (Nullable<>)) ? type.GetGenericArguments() : throw new Exception("The 'NullableStructSerializer' was created with invalid type '" + type.Name + "'.");
      ParameterExpression parameterExpression = Expression.Parameter(type);
      ParameterExpression instance = Expression.Parameter(typeof (BlobWriter));
      this.SerializeAction = Expression.Lambda<Action<T, BlobWriter>>((Expression) Expression.Call((Expression) instance, "WriteNullableStruct", typeArguments, (Expression) parameterExpression), parameterExpression, instance).Compile();
      this.DeserializeFunction = ((Expression<Func<BlobReader, T>>) (blobReader => Expression.Call(blobReader, "ReadNullableStruct", typeArguments, Array.Empty<Expression>()))).Compile();
    }
  }
}
