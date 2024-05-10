// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.DesignateHarvestedTreesCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  /// <summary>
  /// Adds or removes all trees in the specified area to/from the set of trees to be harvested. The area is represented
  /// by rectangle area specified by two tile positions.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class DesignateHarvestedTreesCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly RectangleTerrainArea2i Area;
    /// <summary>
    /// Whether trees are to be added to the set of trees to harvest, or removed.
    /// </summary>
    public readonly bool AddToHarvest;
    /// <summary>
    /// If set, only trees with given harvested product will be selected.
    /// </summary>
    public readonly ProductProto.ID? HarvestedProductId;

    public static void Serialize(DesignateHarvestedTreesCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DesignateHarvestedTreesCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DesignateHarvestedTreesCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.AddToHarvest);
      RectangleTerrainArea2i.Serialize(this.Area, writer);
      writer.WriteNullableStruct<ProductProto.ID>(this.HarvestedProductId);
    }

    public static DesignateHarvestedTreesCmd Deserialize(BlobReader reader)
    {
      DesignateHarvestedTreesCmd harvestedTreesCmd;
      if (reader.TryStartClassDeserialization<DesignateHarvestedTreesCmd>(out harvestedTreesCmd))
        reader.EnqueueDataDeserialization((object) harvestedTreesCmd, DesignateHarvestedTreesCmd.s_deserializeDataDelayedAction);
      return harvestedTreesCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<DesignateHarvestedTreesCmd>(this, "AddToHarvest", (object) reader.ReadBool());
      reader.SetField<DesignateHarvestedTreesCmd>(this, "Area", (object) RectangleTerrainArea2i.Deserialize(reader));
      reader.SetField<DesignateHarvestedTreesCmd>(this, "HarvestedProductId", (object) reader.ReadNullableStruct<ProductProto.ID>());
    }

    public DesignateHarvestedTreesCmd(
      RectangleTerrainArea2i area,
      bool addToHarvest,
      ProductProto.ID? harvestedProductId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Area = area;
      this.AddToHarvest = addToHarvest;
      this.HarvestedProductId = harvestedProductId;
    }

    static DesignateHarvestedTreesCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DesignateHarvestedTreesCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      DesignateHarvestedTreesCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
