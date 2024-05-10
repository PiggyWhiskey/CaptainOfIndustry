// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PropertiesDb.PropertyModifier`1
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
  public sealed class PropertyModifier<T>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public string Owner { get; private set; }

    /// <summary>
    /// Modifiers in the same group can have different aggregation semantics.
    /// Read more info in Property implementations you are using.
    /// </summary>
    public Option<string> Group { get; private set; }

    public T Value { get; private set; }

    public PropertyModifier(T value, string owner, Option<string> group)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Value = value;
      this.Owner = owner;
      this.Group = group;
    }

    public static void Serialize(PropertyModifier<T> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PropertyModifier<T>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PropertyModifier<T>.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Option<string>.Serialize(this.Group, writer);
      writer.WriteString(this.Owner);
      writer.WriteGeneric<T>(this.Value);
    }

    public static PropertyModifier<T> Deserialize(BlobReader reader)
    {
      PropertyModifier<T> propertyModifier;
      if (reader.TryStartClassDeserialization<PropertyModifier<T>>(out propertyModifier))
        reader.EnqueueDataDeserialization((object) propertyModifier, PropertyModifier<T>.s_deserializeDataDelayedAction);
      return propertyModifier;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.Group = Option<string>.Deserialize(reader);
      this.Owner = reader.ReadString();
      this.Value = reader.ReadGenericAs<T>();
    }

    static PropertyModifier()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PropertyModifier<T>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PropertyModifier<T>) obj).SerializeData(writer));
      PropertyModifier<T>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PropertyModifier<T>) obj).DeserializeData(reader));
    }
  }
}
