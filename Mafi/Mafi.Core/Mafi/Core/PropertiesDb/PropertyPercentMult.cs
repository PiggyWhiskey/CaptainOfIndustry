// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertyPercentMult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.PropertiesDb
{
  /// <summary>
  /// Applies modifiers in multiplicative way but between groups they get summed
  /// before applied.
  /// 
  /// NOTE: You need to provide deltas not absolute values.
  /// Initial value is typically 100% (0% would not make sense due to multiplications).
  /// 
  /// Example:
  /// Initial value: 100%
  /// + GameDifficultyGroup: +20%
  /// + MaintenanceEdictGroup: -5%, -15%
  /// Transformed into the following (adds 100% because values are deltas):
  /// + GameDifficultyGroup =&gt; 20% + 100% =&gt; 120%
  /// + MaintenanceEdictGroup: -5% + (-15%) + 100% =&gt; 80%
  /// And applied like this:
  /// 100% (base) * 120% (diff) * 80% (edicts) =&gt; 96%
  /// 
  /// You set same group for modifiers that should be summed up together. That is typically
  /// used for additive bonuses like edicts. Otherwise you should prefer multiplication of
  /// modifiers to make sure that they scale accordingly.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class PropertyPercentMult : Property<Percent>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PropertyPercentMult(string id, Percent value, IPropertiesDbInternal propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, value, propertiesDb);
    }

    protected override Percent ApplyModifier(Percent baseValue, Percent delta)
    {
      Percent percent = Percent.Hundred + delta;
      return baseValue * percent;
    }

    protected override Percent CombineModifiersInGroup(Percent first, Percent second)
    {
      return first + second;
    }

    public static void Serialize(PropertyPercentMult value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PropertyPercentMult>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PropertyPercentMult.s_serializeDataDelayedAction);
    }

    public static PropertyPercentMult Deserialize(BlobReader reader)
    {
      PropertyPercentMult propertyPercentMult;
      if (reader.TryStartClassDeserialization<PropertyPercentMult>(out propertyPercentMult))
        reader.EnqueueDataDeserialization((object) propertyPercentMult, PropertyPercentMult.s_deserializeDataDelayedAction);
      return propertyPercentMult;
    }

    static PropertyPercentMult()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropertyPercentMult.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Property<Percent>) obj).SerializeData(writer));
      PropertyPercentMult.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Property<Percent>) obj).DeserializeData(reader));
    }
  }
}
