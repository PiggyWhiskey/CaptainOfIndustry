// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertyModifiers
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  public static class PropertyModifiers
  {
    public static Option<string> NO_GROUP;

    /// <summary>
    /// Set relative values such as -20% or +10%. That will be mapped to multipliers or not depends
    /// what the property is implementing.
    /// Read documentation for <see cref="T:Mafi.Core.PropertiesDb.PropertyPercentMult" /> and <see cref="T:Mafi.Core.PropertiesDb.PropertyPercentSum" />
    /// to understand how this works.
    /// </summary>
    public static PropertyModifier<Percent> Delta(
      Percent value,
      string owner,
      Option<string> group)
    {
      return new PropertyModifier<Percent>(value, owner, group);
    }

    static PropertyModifiers()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropertyModifiers.NO_GROUP = Option<string>.None;
    }
  }
}
