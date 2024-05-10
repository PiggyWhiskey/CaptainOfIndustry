// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.TerrainMaterialsSlimIdManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class TerrainMaterialsSlimIdManager : 
    SlimIdManagerBase<TerrainMaterialProto, TerrainMaterialSlimId>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public TerrainMaterialsSlimIdManager(ProtosDb db)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(db);
    }

    public override TerrainMaterialProto PhantomProto
    {
      get => TerrainMaterialProto.PhantomTerrainMaterialProto;
    }

    public override int MaxIdValue => (int) byte.MaxValue;

    protected override TerrainMaterialSlimId CreateSlimId(int index)
    {
      return new TerrainMaterialSlimId((byte) index);
    }

    protected override int GetIndex(TerrainMaterialSlimId slimId) => (int) slimId.Value;

    public static void Serialize(TerrainMaterialsSlimIdManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainMaterialsSlimIdManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainMaterialsSlimIdManager.s_serializeDataDelayedAction);
    }

    public static TerrainMaterialsSlimIdManager Deserialize(BlobReader reader)
    {
      TerrainMaterialsSlimIdManager materialsSlimIdManager;
      if (reader.TryStartClassDeserialization<TerrainMaterialsSlimIdManager>(out materialsSlimIdManager))
        reader.EnqueueDataDeserialization((object) materialsSlimIdManager, TerrainMaterialsSlimIdManager.s_deserializeDataDelayedAction);
      return materialsSlimIdManager;
    }

    static TerrainMaterialsSlimIdManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainMaterialsSlimIdManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SlimIdManagerBase<TerrainMaterialProto, TerrainMaterialSlimId>) obj).SerializeData(writer));
      TerrainMaterialsSlimIdManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SlimIdManagerBase<TerrainMaterialProto, TerrainMaterialSlimId>) obj).DeserializeData(reader));
    }
  }
}
