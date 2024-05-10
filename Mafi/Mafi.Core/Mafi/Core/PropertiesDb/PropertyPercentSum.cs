// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertyPercentSum
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
  /// Sum property just simply sums up all its modifiers.
  /// Groups don't actually matter in this case.
  /// 
  /// This is used a bit more rarely in cases where multiplier makes no sense.
  /// 
  /// Initial value is typically 0% or some other initial value.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class PropertyPercentSum : Property<Percent>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PropertyPercentSum(string id, Percent value, IPropertiesDbInternal propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, value, propertiesDb);
    }

    protected override Percent ApplyModifier(Percent baseValue, Percent delta) => baseValue + delta;

    protected override Percent CombineModifiersInGroup(Percent first, Percent second)
    {
      return first + second;
    }

    public static void Serialize(PropertyPercentSum value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PropertyPercentSum>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PropertyPercentSum.s_serializeDataDelayedAction);
    }

    public static PropertyPercentSum Deserialize(BlobReader reader)
    {
      PropertyPercentSum propertyPercentSum;
      if (reader.TryStartClassDeserialization<PropertyPercentSum>(out propertyPercentSum))
        reader.EnqueueDataDeserialization((object) propertyPercentSum, PropertyPercentSum.s_deserializeDataDelayedAction);
      return propertyPercentSum;
    }

    static PropertyPercentSum()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropertyPercentSum.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Property<Percent>) obj).SerializeData(writer));
      PropertyPercentSum.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Property<Percent>) obj).DeserializeData(reader));
    }
  }
}
