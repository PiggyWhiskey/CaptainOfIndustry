// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertyBoolean
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
  public sealed class PropertyBoolean : Property<bool>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public PropertyBoolean(string id, bool value, IPropertiesDbInternal propertiesDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, value, propertiesDb);
    }

    protected override bool ApplyModifier(bool baseValue, bool modifier) => modifier;

    protected override bool CombineModifiersInGroup(bool first, bool second) => second;

    public static void Serialize(PropertyBoolean value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PropertyBoolean>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PropertyBoolean.s_serializeDataDelayedAction);
    }

    public static PropertyBoolean Deserialize(BlobReader reader)
    {
      PropertyBoolean propertyBoolean;
      if (reader.TryStartClassDeserialization<PropertyBoolean>(out propertyBoolean))
        reader.EnqueueDataDeserialization((object) propertyBoolean, PropertyBoolean.s_deserializeDataDelayedAction);
      return propertyBoolean;
    }

    static PropertyBoolean()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropertyBoolean.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Property<bool>) obj).SerializeData(writer));
      PropertyBoolean.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Property<bool>) obj).DeserializeData(reader));
    }
  }
}
