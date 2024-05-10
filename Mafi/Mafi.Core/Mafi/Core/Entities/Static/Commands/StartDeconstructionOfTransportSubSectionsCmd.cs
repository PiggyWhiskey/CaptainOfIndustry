// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.StartDeconstructionOfTransportSubSectionsCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class StartDeconstructionOfTransportSubSectionsCmd : InputCommand
  {
    public readonly EntityId TransportId;
    public readonly ImmutableArray<Pair<Tile3i, Tile3i>> RemovedSubSections;
    public readonly EntityRemoveReason RemoveReason;
    public readonly bool QuickRemoveWithUnity;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StartDeconstructionOfTransportSubSectionsCmd(
      EntityId transportId,
      ImmutableArray<Pair<Tile3i, Tile3i>> removedSubSections,
      EntityRemoveReason removeReason,
      bool quickRemoveWithUnity = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TransportId = transportId;
      this.RemovedSubSections = removedSubSections;
      this.RemoveReason = removeReason;
      this.QuickRemoveWithUnity = quickRemoveWithUnity;
    }

    public static void Serialize(
      StartDeconstructionOfTransportSubSectionsCmd value,
      BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StartDeconstructionOfTransportSubSectionsCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StartDeconstructionOfTransportSubSectionsCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.QuickRemoveWithUnity);
      ImmutableArray<Pair<Tile3i, Tile3i>>.Serialize(this.RemovedSubSections, writer);
      writer.WriteInt((int) this.RemoveReason);
      EntityId.Serialize(this.TransportId, writer);
    }

    public static StartDeconstructionOfTransportSubSectionsCmd Deserialize(BlobReader reader)
    {
      StartDeconstructionOfTransportSubSectionsCmd transportSubSectionsCmd;
      if (reader.TryStartClassDeserialization<StartDeconstructionOfTransportSubSectionsCmd>(out transportSubSectionsCmd))
        reader.EnqueueDataDeserialization((object) transportSubSectionsCmd, StartDeconstructionOfTransportSubSectionsCmd.s_deserializeDataDelayedAction);
      return transportSubSectionsCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StartDeconstructionOfTransportSubSectionsCmd>(this, "QuickRemoveWithUnity", (object) reader.ReadBool());
      reader.SetField<StartDeconstructionOfTransportSubSectionsCmd>(this, "RemovedSubSections", (object) ImmutableArray<Pair<Tile3i, Tile3i>>.Deserialize(reader));
      reader.SetField<StartDeconstructionOfTransportSubSectionsCmd>(this, "RemoveReason", (object) (EntityRemoveReason) reader.ReadInt());
      reader.SetField<StartDeconstructionOfTransportSubSectionsCmd>(this, "TransportId", (object) EntityId.Deserialize(reader));
    }

    static StartDeconstructionOfTransportSubSectionsCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StartDeconstructionOfTransportSubSectionsCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StartDeconstructionOfTransportSubSectionsCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
