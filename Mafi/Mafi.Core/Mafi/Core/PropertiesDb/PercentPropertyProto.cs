﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PercentPropertyProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  public class PercentPropertyProto : PropertyProto
  {
    private readonly Mafi.Core.PropertiesDb.PropertyId<Percent> m_propertyId;
    private readonly Percent m_initialValue;
    private readonly PercentPropertyProto.PropertyType m_type;

    public override string PropertyId => this.m_propertyId.Value;

    public PercentPropertyProto(
      Mafi.Core.PropertiesDb.PropertyId<Percent> propertyId,
      Percent initialValue,
      PercentPropertyProto.PropertyType type = PercentPropertyProto.PropertyType.Multiplier)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(new Proto.ID("Prop" + propertyId.Value));
      this.m_initialValue = initialValue;
      this.m_type = type;
      this.m_propertyId = propertyId;
    }

    public override IProperty CreateProperty(IPropertiesDbInternal propsDb)
    {
      return this.m_type == PercentPropertyProto.PropertyType.Diff ? (IProperty) new PropertyPercentSum(this.m_propertyId.Value, this.m_initialValue, propsDb) : (IProperty) new PropertyPercentMult(this.m_propertyId.Value, this.m_initialValue, propsDb);
    }

    public enum PropertyType
    {
      Multiplier,
      Diff,
    }
  }
}
