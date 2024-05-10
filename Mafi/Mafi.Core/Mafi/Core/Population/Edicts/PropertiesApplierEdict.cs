// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.PropertiesApplierEdict
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Offices;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  [GenerateSerializer(false, null, 0)]
  public class PropertiesApplierEdict : Edict
  {
    private readonly EdictWithPropertiesProto m_edictProto;
    private ImmutableArray<KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>>> m_appliers;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PropertiesApplierEdict(
      EdictWithPropertiesProto edictProto,
      UpointsManager upointsManager,
      ICalendar calendar,
      CaptainOfficeManager captainOfficeManager,
      PropsDb propsDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((EdictProto) edictProto, upointsManager, calendar, captainOfficeManager);
      this.m_edictProto = edictProto;
      Lyst<KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>>> list = new Lyst<KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>>>();
      foreach (KeyValuePair<PropertyId<Percent>, Percent> keyValuePair in edictProto.PropertiesToApply)
      {
        IProperty<Percent> property = propsDb.GetProperty<Percent>(keyValuePair.Key);
        PropertyModifier<Percent> propertyModifier = PropertyModifiers.Delta(keyValuePair.Value, this.Prototype.Id.Value, (Option<string>) edictProto.PropertyGroup);
        list.Add<IProperty<Percent>, PropertyModifier<Percent>>(property, propertyModifier);
      }
      this.m_appliers = list.ToImmutableArray();
    }

    [InitAfterLoad(InitPriority.Lowest)]
    private void initSelf(DependencyResolver resolver)
    {
      bool flag = false;
      Lyst<KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>>> list = new Lyst<KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>>>();
      ImmutableArray<KeyValuePair<PropertyId<Percent>, Percent>> propertiesToApply = ((EdictWithPropertiesProto) this.Prototype).PropertiesToApply;
      PropsDb propsDb = resolver.Resolve<PropsDb>();
      for (int index = 0; index < this.m_appliers.Length; ++index)
      {
        if (index >= propertiesToApply.Length)
        {
          Log.Error(string.Format("Out of range! {0} {1}", (object) index, (object) propertiesToApply.Length));
          return;
        }
        KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>> applier = this.m_appliers[index];
        KeyValuePair<PropertyId<Percent>, Percent> keyValuePair = propertiesToApply[index];
        if (keyValuePair.Key.Value != applier.Key.Id)
        {
          flag = true;
          IProperty<Percent> property = propsDb.GetProperty<Percent>(keyValuePair.Key);
          PropertyModifier<Percent> propertyModifier = PropertyModifiers.Delta(keyValuePair.Value, this.Prototype.Id.Value, (Option<string>) this.m_edictProto.PropertyGroup);
          list.Add<IProperty<Percent>, PropertyModifier<Percent>>(property, propertyModifier);
        }
        else if (keyValuePair.Value != applier.Value.Value)
        {
          flag = true;
          PropertyModifier<Percent> propertyModifier = PropertyModifiers.Delta(keyValuePair.Value, this.Prototype.Id.Value, (Option<string>) this.m_edictProto.PropertyGroup);
          list.Add<IProperty<Percent>, PropertyModifier<Percent>>(applier.Key, propertyModifier);
        }
        else
          list.Add(applier);
      }
      if (!flag)
        return;
      if (this.IsActive)
      {
        foreach (KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>> applier in this.m_appliers)
          applier.Key.RemoveModifier(applier.Value);
      }
      this.m_appliers = list.ToImmutableArray();
      if (!this.IsActive)
        return;
      foreach (KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>> applier in this.m_appliers)
        applier.Key.AddModifier(applier.Value);
    }

    protected override bool CanReactivateForNewMonth(out string reasonForNotActive)
    {
      reasonForNotActive = "";
      return true;
    }

    protected override Edict.EdictEnableCheckResult CanBeEnabledInternal()
    {
      return new Edict.EdictEnableCheckResult()
      {
        CanBeEnabled = true,
        Explanation = ""
      };
    }

    protected override void OnActiveChanged(bool isActiveNow)
    {
      if (isActiveNow)
      {
        foreach (KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>> applier in this.m_appliers)
          applier.Key.AddModifier(applier.Value);
      }
      else
      {
        foreach (KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>> applier in this.m_appliers)
          applier.Key.RemoveModifier(applier.Value);
      }
    }

    public static void Serialize(PropertiesApplierEdict value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PropertiesApplierEdict>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PropertiesApplierEdict.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ImmutableArray<KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>>>.Serialize(this.m_appliers, writer);
      writer.WriteGeneric<EdictWithPropertiesProto>(this.m_edictProto);
    }

    public static PropertiesApplierEdict Deserialize(BlobReader reader)
    {
      PropertiesApplierEdict propertiesApplierEdict;
      if (reader.TryStartClassDeserialization<PropertiesApplierEdict>(out propertiesApplierEdict))
        reader.EnqueueDataDeserialization((object) propertiesApplierEdict, PropertiesApplierEdict.s_deserializeDataDelayedAction);
      return propertiesApplierEdict;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_appliers = ImmutableArray<KeyValuePair<IProperty<Percent>, PropertyModifier<Percent>>>.Deserialize(reader);
      reader.SetField<PropertiesApplierEdict>(this, "m_edictProto", (object) reader.ReadGenericAs<EdictWithPropertiesProto>());
      reader.RegisterInitAfterLoad<PropertiesApplierEdict>(this, "initSelf", InitPriority.Lowest);
    }

    static PropertiesApplierEdict()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropertiesApplierEdict.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Edict) obj).SerializeData(writer));
      PropertiesApplierEdict.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Edict) obj).DeserializeData(reader));
    }
  }
}
