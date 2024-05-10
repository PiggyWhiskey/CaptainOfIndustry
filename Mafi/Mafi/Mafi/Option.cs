// Decompiled with JetBrains decompiler
// Type: Mafi.Option
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Helper class that simplifies creation of options by automated type inference.
  /// </summary>
  public sealed class Option
  {
    /// <summary>The default Option type specifying there is no value.</summary>
    public static readonly Option None;

    /// <summary>
    /// Creates an <see cref="T:Mafi.Option`1" /> from given value. The value may be null.
    /// </summary>
    public static Option<T> Create<T>(T value) where T : class => Option<T>.Create(value);

    /// <summary>
    /// Creates an <see cref="T:Mafi.Option`1" /> from given value. The value is expected to be not null.
    /// </summary>
    public static Option<T> Some<T>(T value) where T : class => Option<T>.Some(value);

    public Option()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
