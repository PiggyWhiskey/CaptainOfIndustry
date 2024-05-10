// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.DynamicDispatchClassSerializer`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Mafi.Serialization
{
  public sealed class DynamicDispatchClassSerializer<T> : IGenericSerializer<T>
  {
    private static DynamicDispatchClassSerializer<T> s_instance;
    private readonly Dict<Type, Action<T, BlobWriter>> m_cachedSerializeActions;
    private readonly Dict<Type, Func<BlobReader, T>> m_cachedDeserializeFunctions;

    public static DynamicDispatchClassSerializer<T> Instance
    {
      get
      {
        return DynamicDispatchClassSerializer<T>.s_instance ?? (DynamicDispatchClassSerializer<T>.s_instance = new DynamicDispatchClassSerializer<T>());
      }
    }

    public Action<T, BlobWriter> SerializeAction { get; }

    public Func<BlobReader, T> DeserializeFunction { get; }

    private DynamicDispatchClassSerializer()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_cachedSerializeActions = new Dict<Type, Action<T, BlobWriter>>();
      this.m_cachedDeserializeFunctions = new Dict<Type, Func<BlobReader, T>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SerializeAction = (Action<T, BlobWriter>) ((value, writer) =>
      {
        if ((object) value == null)
        {
          Log.Error("Serializing 'null' of type '" + typeof (T).Name + "'.");
          writer.WriteType((Type) null, true);
        }
        else
        {
          Type type = value.GetType();
          if (!type.IsAssignableTo<T>())
            throw new Exception("Failed to serialize type '" + type.Name + "' using dynamic dispatch, it is not assignable to '" + typeof (T).Name + "'.");
          writer.WriteType(type);
          Action<T, BlobWriter> action;
          lock (this.m_cachedSerializeActions)
          {
            if (!this.m_cachedSerializeActions.TryGetValue(type, out action))
            {
              ParameterExpression parameterExpression = Expression.Parameter(typeof (T));
              UnaryExpression unaryExpression = Expression.Convert((Expression) parameterExpression, type);
              ParameterExpression instance = Expression.Parameter(typeof (BlobWriter));
              Type[] andInit = ArrayPool<Type>.GetAndInit(type);
              MethodCallExpression body = Expression.Call((Expression) instance, "WriteGeneric", andInit, (Expression) unaryExpression);
              andInit.ReturnToPool<Type>();
              action = Expression.Lambda<Action<T, BlobWriter>>((Expression) body, parameterExpression, instance).Compile();
              this.m_cachedSerializeActions.Add(type, action);
            }
          }
          action(value, writer);
        }
      });
      this.DeserializeFunction = (Func<BlobReader, T>) (reader =>
      {
        Type type = reader.ReadType(true);
        if (type == (Type) null)
        {
          Log.Error("Deserializing 'null' of type '" + typeof (T).Name + "'.");
          return default (T);
        }
        if (!type.IsAssignableTo<T>())
          throw new Exception("Failed to deserialize type '" + type.Name + "' using dynamic dispatch, it is not assignable to '" + typeof (T).Name + "'.");
        Func<BlobReader, T> func;
        lock (this.m_cachedDeserializeFunctions)
        {
          if (!this.m_cachedDeserializeFunctions.TryGetValue(type, out func))
          {
            ParameterExpression instance = Expression.Parameter(typeof (BlobReader));
            Type[] andInit = ArrayPool<Type>.GetAndInit(type);
            Expression expression = (Expression) Expression.Call((Expression) instance, "ReadGenericAs", andInit);
            andInit.ReturnToPool<Type>();
            if (type.IsValueType && !typeof (T).IsValueType)
              expression = (Expression) Expression.Convert(expression, typeof (T));
            func = Expression.Lambda<Func<BlobReader, T>>(expression, instance).Compile();
            this.m_cachedDeserializeFunctions.Add(type, func);
          }
        }
        return func(reader);
      });
    }
  }
}
