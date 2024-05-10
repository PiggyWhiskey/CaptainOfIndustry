// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.MineTowerAreaChangeCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Mine
{
  /// <summary>
  /// Command to change the range (area) of <see cref="T:Mafi.Core.Buildings.Mine.MineTower" />.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class MineTowerAreaChangeCmd : InputCommand
  {
    public readonly EntityId MineTowerId;
    public readonly RectangleTerrainArea2i Area;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MineTowerAreaChangeCmd(EntityId mineTowerId, RectangleTerrainArea2i area)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MineTowerId = mineTowerId;
      this.Area = area;
    }

    public static void Serialize(MineTowerAreaChangeCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MineTowerAreaChangeCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MineTowerAreaChangeCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RectangleTerrainArea2i.Serialize(this.Area, writer);
      EntityId.Serialize(this.MineTowerId, writer);
    }

    public static MineTowerAreaChangeCmd Deserialize(BlobReader reader)
    {
      MineTowerAreaChangeCmd towerAreaChangeCmd;
      if (reader.TryStartClassDeserialization<MineTowerAreaChangeCmd>(out towerAreaChangeCmd))
        reader.EnqueueDataDeserialization((object) towerAreaChangeCmd, MineTowerAreaChangeCmd.s_deserializeDataDelayedAction);
      return towerAreaChangeCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MineTowerAreaChangeCmd>(this, "Area", (object) RectangleTerrainArea2i.Deserialize(reader));
      reader.SetField<MineTowerAreaChangeCmd>(this, "MineTowerId", (object) EntityId.Deserialize(reader));
    }

    static MineTowerAreaChangeCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MineTowerAreaChangeCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      MineTowerAreaChangeCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
