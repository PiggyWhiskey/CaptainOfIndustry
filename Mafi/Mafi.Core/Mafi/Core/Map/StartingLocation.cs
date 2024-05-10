// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.StartingLocation
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public sealed class StartingLocation
  {
    public readonly Tile2i Position;
    public readonly Direction90 ShoreDirection;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StartingLocation(Tile2i position, Direction90 shoreDirection)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Position = position;
      this.ShoreDirection = shoreDirection;
    }

    public static void Serialize(StartingLocation value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StartingLocation>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StartingLocation.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Tile2i.Serialize(this.Position, writer);
      Direction90.Serialize(this.ShoreDirection, writer);
    }

    public static StartingLocation Deserialize(BlobReader reader)
    {
      StartingLocation startingLocation;
      if (reader.TryStartClassDeserialization<StartingLocation>(out startingLocation))
        reader.EnqueueDataDeserialization((object) startingLocation, StartingLocation.s_deserializeDataDelayedAction);
      return startingLocation;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<StartingLocation>(this, "Position", (object) Tile2i.Deserialize(reader));
      reader.SetField<StartingLocation>(this, "ShoreDirection", (object) Direction90.Deserialize(reader));
    }

    static StartingLocation()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StartingLocation.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StartingLocation) obj).SerializeData(writer));
      StartingLocation.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StartingLocation) obj).DeserializeData(reader));
    }
  }
}
