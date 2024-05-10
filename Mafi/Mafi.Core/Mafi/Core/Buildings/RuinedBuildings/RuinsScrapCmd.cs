// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.RuinedBuildings.RuinsScrapCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.RuinedBuildings
{
  [GenerateSerializer(false, null, 0)]
  public class RuinsScrapCmd : InputCommand
  {
    public readonly EntityId RuinsId;
    public readonly bool IsScrapping;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public RuinsScrapCmd(bool isScrapping)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RuinsId = EntityId.Invalid;
      this.IsScrapping = isScrapping;
    }

    public RuinsScrapCmd(EntityId ruinsId, bool isScrapping)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RuinsId = ruinsId;
      this.IsScrapping = isScrapping;
    }

    public static void Serialize(RuinsScrapCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RuinsScrapCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RuinsScrapCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsScrapping);
      EntityId.Serialize(this.RuinsId, writer);
    }

    public static RuinsScrapCmd Deserialize(BlobReader reader)
    {
      RuinsScrapCmd ruinsScrapCmd;
      if (reader.TryStartClassDeserialization<RuinsScrapCmd>(out ruinsScrapCmd))
        reader.EnqueueDataDeserialization((object) ruinsScrapCmd, RuinsScrapCmd.s_deserializeDataDelayedAction);
      return ruinsScrapCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RuinsScrapCmd>(this, "IsScrapping", (object) reader.ReadBool());
      reader.SetField<RuinsScrapCmd>(this, "RuinsId", (object) EntityId.Deserialize(reader));
    }

    static RuinsScrapCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RuinsScrapCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      RuinsScrapCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
