// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.RecoverOceanResult
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct RecoverOceanResult
  {
    public readonly int BlockedByTerrainCount;
    public readonly EntityId BlockingEntityId;

    public RecoverOceanResult(int blockedByTerrainCount, EntityId blockingEntityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.BlockedByTerrainCount = blockedByTerrainCount;
      this.BlockingEntityId = blockingEntityId;
    }

    public static void Serialize(RecoverOceanResult value, BlobWriter writer)
    {
      writer.WriteInt(value.BlockedByTerrainCount);
      EntityId.Serialize(value.BlockingEntityId, writer);
    }

    public static RecoverOceanResult Deserialize(BlobReader reader)
    {
      return new RecoverOceanResult(reader.ReadInt(), EntityId.Deserialize(reader));
    }
  }
}
