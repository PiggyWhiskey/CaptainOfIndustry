// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.EdictWithPropertiesProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  public class EdictWithPropertiesProto : EdictProto
  {
    public readonly ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> PropertiesToApply;
    public string PropertyGroup;

    public EdictWithPropertiesProto(
      Proto.ID id,
      Proto.Str strings,
      EdictCategoryProto category,
      Upoints monthlyUpointsCost,
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propertiesToApply,
      string propertyGroup,
      Option<EdictProto> previousTier,
      EdictProto.Gfx graphics,
      bool? isGeneratingUnity = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, category, monthlyUpointsCost, typeof (PropertiesApplierEdict), graphics, isGeneratingUnity, new Option<EdictProto>?(previousTier));
      this.PropertiesToApply = propertiesToApply;
      this.PropertyGroup = propertyGroup;
    }
  }
}
