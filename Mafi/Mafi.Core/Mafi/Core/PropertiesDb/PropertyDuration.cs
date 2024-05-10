// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertyDuration
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
  [GenerateSerializer(false, null, 0)]
  public sealed class PropertyDuration : Property<Duration>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PropertyDuration(string id, Duration value, IPropertiesDbInternal propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, value, propertiesDb);
    }

    protected override Duration ApplyModifier(Duration baseValue, Duration modifier)
    {
      return baseValue + modifier;
    }

    protected override Duration CombineModifiersInGroup(Duration first, Duration second)
    {
      return first + second;
    }

    public static void Serialize(PropertyDuration value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PropertyDuration>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PropertyDuration.s_serializeDataDelayedAction);
    }

    public static PropertyDuration Deserialize(BlobReader reader)
    {
      PropertyDuration propertyDuration;
      if (reader.TryStartClassDeserialization<PropertyDuration>(out propertyDuration))
        reader.EnqueueDataDeserialization((object) propertyDuration, PropertyDuration.s_deserializeDataDelayedAction);
      return propertyDuration;
    }

    static PropertyDuration()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropertyDuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Property<Duration>) obj).SerializeData(writer));
      PropertyDuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Property<Duration>) obj).DeserializeData(reader));
    }
  }
}
