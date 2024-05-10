// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Surfaces.TileSurfaceDecalsSlimIdManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Surfaces
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public sealed class TileSurfaceDecalsSlimIdManager : 
    SlimIdManagerBase<TerrainTileSurfaceDecalProto, TileSurfaceDecalSlimId>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(TileSurfaceDecalsSlimIdManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TileSurfaceDecalsSlimIdManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TileSurfaceDecalsSlimIdManager.s_serializeDataDelayedAction);
    }

    public static TileSurfaceDecalsSlimIdManager Deserialize(BlobReader reader)
    {
      TileSurfaceDecalsSlimIdManager decalsSlimIdManager;
      if (reader.TryStartClassDeserialization<TileSurfaceDecalsSlimIdManager>(out decalsSlimIdManager))
        reader.EnqueueDataDeserialization((object) decalsSlimIdManager, TileSurfaceDecalsSlimIdManager.s_deserializeDataDelayedAction);
      return decalsSlimIdManager;
    }

    public TileSurfaceDecalsSlimIdManager(ProtosDb db)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(db);
    }

    public override TerrainTileSurfaceDecalProto PhantomProto
    {
      get => TerrainTileSurfaceDecalProto.Phantom;
    }

    public override int MaxIdValue => (int) byte.MaxValue;

    protected override TileSurfaceDecalSlimId CreateSlimId(int index)
    {
      return new TileSurfaceDecalSlimId((byte) index);
    }

    protected override int GetIndex(TileSurfaceDecalSlimId slimId) => (int) slimId.Value;

    static TileSurfaceDecalsSlimIdManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TileSurfaceDecalsSlimIdManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SlimIdManagerBase<TerrainTileSurfaceDecalProto, TileSurfaceDecalSlimId>) obj).SerializeData(writer));
      TileSurfaceDecalsSlimIdManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SlimIdManagerBase<TerrainTileSurfaceDecalProto, TileSurfaceDecalSlimId>) obj).DeserializeData(reader));
    }
  }
}
