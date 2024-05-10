// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TileSurfacesSlimIdManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class TileSurfacesSlimIdManager : 
    SlimIdManagerBase<TerrainTileSurfaceProto, TileSurfaceSlimId>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(TileSurfacesSlimIdManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TileSurfacesSlimIdManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TileSurfacesSlimIdManager.s_serializeDataDelayedAction);
    }

    public static TileSurfacesSlimIdManager Deserialize(BlobReader reader)
    {
      TileSurfacesSlimIdManager surfacesSlimIdManager;
      if (reader.TryStartClassDeserialization<TileSurfacesSlimIdManager>(out surfacesSlimIdManager))
        reader.EnqueueDataDeserialization((object) surfacesSlimIdManager, TileSurfacesSlimIdManager.s_deserializeDataDelayedAction);
      return surfacesSlimIdManager;
    }

    public TileSurfacesSlimIdManager(ProtosDb db)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(db);
    }

    public override TerrainTileSurfaceProto PhantomProto => TerrainTileSurfaceProto.Phantom;

    public override int MaxIdValue => 31;

    protected override TileSurfaceSlimId CreateSlimId(int index)
    {
      return new TileSurfaceSlimId((byte) index);
    }

    protected override int GetIndex(TileSurfaceSlimId slimId) => (int) slimId.Value;

    static TileSurfacesSlimIdManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TileSurfacesSlimIdManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SlimIdManagerBase<TerrainTileSurfaceProto, TileSurfaceSlimId>) obj).SerializeData(writer));
      TileSurfacesSlimIdManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SlimIdManagerBase<TerrainTileSurfaceProto, TileSurfaceSlimId>) obj).DeserializeData(reader));
    }
  }
}
