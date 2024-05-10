// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.NewInSaveVersionAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class NewInSaveVersionAttribute : Attribute
  {
    public readonly int Version;
    public readonly Option<string> CustomSortingName;
    public readonly Option<string> CustomValueWhenNotLoaded;
    public readonly Option<Type> DefaultValueFromResolver;
    public readonly Option<Type> GlobalDepTypeOverride;

    public NewInSaveVersionAttribute(
      int version,
      string customSortingName = null,
      string defaultValue = null,
      Type defaultValueFromResolver = null,
      Type globalDepTypeOverride = null)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Version = version;
      this.CustomSortingName = (Option<string>) customSortingName;
      this.CustomValueWhenNotLoaded = (Option<string>) defaultValue;
      this.DefaultValueFromResolver = (Option<Type>) defaultValueFromResolver;
      this.GlobalDepTypeOverride = (Option<Type>) globalDepTypeOverride;
    }
  }
}
