// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.SortingPlantNoSingleProductCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.OreSorting
{
  [GenerateSerializer(false, null, 0)]
  public class SortingPlantNoSingleProductCmd : InputCommand
  {
    public readonly EntityId SortingPlantId;
    public readonly bool DoNotAcceptSingleProduct;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SortingPlantNoSingleProductCmd(
      OreSortingPlant sortingPlant,
      bool doNotAcceptSingleProduct)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(sortingPlant.Id, doNotAcceptSingleProduct);
    }

    private SortingPlantNoSingleProductCmd(EntityId sortingPlantId, bool doNotAcceptSingleProduct)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SortingPlantId = sortingPlantId;
      this.DoNotAcceptSingleProduct = doNotAcceptSingleProduct;
    }

    public static void Serialize(SortingPlantNoSingleProductCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SortingPlantNoSingleProductCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SortingPlantNoSingleProductCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.DoNotAcceptSingleProduct);
      EntityId.Serialize(this.SortingPlantId, writer);
    }

    public static SortingPlantNoSingleProductCmd Deserialize(BlobReader reader)
    {
      SortingPlantNoSingleProductCmd singleProductCmd;
      if (reader.TryStartClassDeserialization<SortingPlantNoSingleProductCmd>(out singleProductCmd))
        reader.EnqueueDataDeserialization((object) singleProductCmd, SortingPlantNoSingleProductCmd.s_deserializeDataDelayedAction);
      return singleProductCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SortingPlantNoSingleProductCmd>(this, "DoNotAcceptSingleProduct", (object) reader.ReadBool());
      reader.SetField<SortingPlantNoSingleProductCmd>(this, "SortingPlantId", (object) EntityId.Deserialize(reader));
    }

    static SortingPlantNoSingleProductCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SortingPlantNoSingleProductCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SortingPlantNoSingleProductCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
