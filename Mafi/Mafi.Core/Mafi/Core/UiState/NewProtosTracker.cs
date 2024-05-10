// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UiState.NewProtosTracker
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.UiState
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class NewProtosTracker
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Set<Proto> m_alreadySeenProtos;

    public static void Serialize(NewProtosTracker value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NewProtosTracker>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NewProtosTracker.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Set<Proto>.Serialize(this.m_alreadySeenProtos, writer);
    }

    public static NewProtosTracker Deserialize(BlobReader reader)
    {
      NewProtosTracker newProtosTracker;
      if (reader.TryStartClassDeserialization<NewProtosTracker>(out newProtosTracker))
        reader.EnqueueDataDeserialization((object) newProtosTracker, NewProtosTracker.s_deserializeDataDelayedAction);
      return newProtosTracker;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<NewProtosTracker>(this, "m_alreadySeenProtos", (object) Set<Proto>.Deserialize(reader));
    }

    public NewProtosTracker(UnlockedProtosDb unlockedProtosDb, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_alreadySeenProtos = new Set<Proto>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Proto proto1;
      if (protosDb.TryGetProto<Proto>(new Proto.ID("Housing"), out proto1))
        this.m_alreadySeenProtos.Add(proto1);
      Proto proto2;
      if (!protosDb.TryGetProto<Proto>(new Proto.ID("SettlementFoodModule"), out proto2))
        return;
      this.m_alreadySeenProtos.Add(proto2);
    }

    public void MarkProtoAsSeen(Proto proto) => this.m_alreadySeenProtos.Add(proto);

    public bool IsNew(Proto proto) => !this.m_alreadySeenProtos.Contains(proto);

    static NewProtosTracker()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NewProtosTracker.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NewProtosTracker) obj).SerializeData(writer));
      NewProtosTracker.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NewProtosTracker) obj).DeserializeData(reader));
    }
  }
}
