// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.DirectCallSerializer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization.Generators;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Mafi.Serialization
{
  /// <summary>
  /// Serializer that calls `Serialize` and `Deserialize` on given type directly. The given type must implement these
  /// methods otherwise Exception is thrown. If you have an interface type that does not implement these methods, use
  /// <see cref="T:Mafi.Serialization.DynamicDispatchClassSerializer`1" />.
  /// </summary>
  /// <remarks>
  /// This class uses compiled lambda expression so that the calls are very efficient, much faster than
  /// `GetMethod().Call()` would be.</remarks>
  public sealed class DirectCallSerializer<T> : IGenericSerializer<T>
  {
    private static DirectCallSerializer<T> m_instance;

    public static DirectCallSerializer<T> Instance
    {
      get
      {
        return DirectCallSerializer<T>.m_instance ?? (DirectCallSerializer<T>.m_instance = new DirectCallSerializer<T>());
      }
    }

    public Action<T, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, T> DeserializeFunction { get; }

    private DirectCallSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      this.\u002Ector(typeof (T));
    }

    /// <summary>
    /// Use this ctor if <typeparamref name="T" /> is an interface (or abstract/derived class) but you wish to call
    /// `Serialize` and `Deserialize` on some concrete implementation <paramref name="typeToSerialize" />. Note that
    /// <paramref name="typeToSerialize" /> must be assignable to <typeparamref name="T" />.
    /// </summary>
    internal DirectCallSerializer(Type typeToSerialize)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (!typeToSerialize.IsAssignableTo<T>())
        throw new Exception("Failed to create direct-call serializer, type '" + typeToSerialize.Name + "' cannot be assigned to '" + typeof (T).Name + "'.");
      MethodInfo method1;
      if (!SerializerGenerator.TryGetSerializeMethod(typeToSerialize, out method1))
        throw new Exception("Public method 'Serialize' is missing on type '" + typeToSerialize.Name + "'.");
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (T));
      Expression expression = (Expression) parameterExpression1;
      if (typeof (T) != typeToSerialize)
        expression = (Expression) Expression.Convert((Expression) parameterExpression1, typeToSerialize);
      ParameterExpression parameterExpression2;
      this.SerializeAction = Expression.Lambda<Action<T, BlobWriter>>((Expression) Expression.Call(method1, expression, (Expression) parameterExpression2), parameterExpression1, parameterExpression2).Compile();
      MethodInfo method2;
      if (!SerializerGenerator.TryGetDeserializeMethod(typeToSerialize, out method2))
        throw new Exception("Public method 'Deserialize' is missing on type '" + typeToSerialize.Name + "'.");
      this.DeserializeFunction = ((Expression<Func<BlobReader, T>>) (blobReader => Expression.Call(method2, blobReader))).Compile();
    }
  }
}
