// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Farms.MoveAnimalsCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Farms
{
  [GenerateSerializer(false, null, 0)]
  public class MoveAnimalsCmd : InputCommand
  {
    public readonly EntityId AnimalFarmId;
    public readonly int NumberToMove;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MoveAnimalsCmd(AnimalFarm farm, int numberToMove)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(farm.Id, numberToMove);
    }

    public MoveAnimalsCmd(EntityId farmId, int numberToMove)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AnimalFarmId = farmId;
      this.NumberToMove = numberToMove;
    }

    public static void Serialize(MoveAnimalsCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MoveAnimalsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MoveAnimalsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.AnimalFarmId, writer);
      writer.WriteInt(this.NumberToMove);
    }

    public static MoveAnimalsCmd Deserialize(BlobReader reader)
    {
      MoveAnimalsCmd moveAnimalsCmd;
      if (reader.TryStartClassDeserialization<MoveAnimalsCmd>(out moveAnimalsCmd))
        reader.EnqueueDataDeserialization((object) moveAnimalsCmd, MoveAnimalsCmd.s_deserializeDataDelayedAction);
      return moveAnimalsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MoveAnimalsCmd>(this, "AnimalFarmId", (object) EntityId.Deserialize(reader));
      reader.SetField<MoveAnimalsCmd>(this, "NumberToMove", (object) reader.ReadInt());
    }

    static MoveAnimalsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MoveAnimalsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      MoveAnimalsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
