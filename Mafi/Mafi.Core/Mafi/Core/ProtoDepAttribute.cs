// Decompiled with JetBrains decompiler
// Type: Mafi.Core.ProtoDepAttribute
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Core
{
  /// <summary>
  /// This attribute is used to obtain an instance of <see cref="T:Mafi.Core.Prototypes.Proto" /> as a constructor parameter during dependency
  /// resolving phase. This attribute can be used only on a parameter of a constructor of a class the is being created
  /// by <see cref="T:Mafi.DependencyResolver" />.
  /// </summary>
  /// <remarks>
  /// Type of marked class has to be derived type of <see cref="T:Mafi.Core.Prototypes.Proto" /> otherwise <see cref="T:Mafi.DependencyResolverException" /> will be thrown. This exception is also thrown in proto under given ID is not
  /// found in the DB.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Parameter)]
  public class ProtoDepAttribute : Attribute
  {
    public readonly Proto.ID ProtoId;

    public ProtoDepAttribute(string protoId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = new Proto.ID(protoId);
    }

    public static Func<ParameterInfo, Option<object>> GetResolvingFunction(ProtosDb db)
    {
      return (Func<ParameterInfo, Option<object>>) (pi => ProtoDepAttribute.resolveProto(pi, db));
    }

    private static Option<object> resolveProto(ParameterInfo pi, ProtosDb db)
    {
      if (!(((IEnumerable<object>) pi.GetCustomAttributes(typeof (ProtoDepAttribute), false)).FirstOrDefault<object>() is ProtoDepAttribute protoDepAttribute))
        return Option<object>.None;
      if (!pi.ParameterType.IsAssignableTo<Proto>())
        throw new DependencyResolverException("Parameter marked with 'ProtoDepAttribute' is not type of to 'Proto'.");
      Option<Proto> option = db.Get(protoDepAttribute.ProtoId);
      if (option.IsNone)
        throw new DependencyResolverException(string.Format("Failed to resolve proto '{0}'.", (object) protoDepAttribute.ProtoId));
      if (!option.Value.GetType().IsAssignableTo(pi.ParameterType))
        throw new DependencyResolverException(string.Format("Proto '{0}' is not assignable to parameter of type '{1}'.", (object) protoDepAttribute.ProtoId, (object) option.Value.GetType()));
      return (Option<object>) (object) option.Value;
    }
  }
}
