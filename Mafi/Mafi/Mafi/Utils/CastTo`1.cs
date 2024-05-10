// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.CastTo`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Mafi.Utils
{
  /// <summary>Generic casting utility.</summary>
  /// <remarks>
  /// Inspired from:
  /// https://stackoverflow.com/questions/1189144/c-sharp-non-boxing-conversion-of-generic-enum-to-int
  /// </remarks>
  public static class CastTo<TDst>
  {
    /// <summary>
    /// Casts <see cref="!:TSrc" /> to <see cref="!:TDst" />. This does not cause boxing for value types.
    /// Useful in generic methods.
    /// </summary>
    public static TDst From<TSrc>(TSrc value) => CastTo<TDst>.Cache<TSrc>.Caster(value);

    private static class Cache<TSrc>
    {
      public static readonly Func<TSrc, TDst> Caster;

      private static Func<TSrc, TDst> Get()
      {
        return typeof (TSrc) == typeof (TDst) ? ((Expression<Func<TSrc, TDst>>) (src => src)).Compile() : ((Expression<Func<TSrc, TDst>>) (parameterExpression => (TDst) parameterExpression)).Compile();
      }

      static Cache()
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        CastTo<TDst>.Cache<TSrc>.Caster = CastTo<TDst>.Cache<TSrc>.Get();
      }
    }
  }
}
