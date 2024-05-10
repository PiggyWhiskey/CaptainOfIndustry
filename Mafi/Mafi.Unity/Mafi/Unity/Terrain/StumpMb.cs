// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.StumpMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public class StumpMb : MonoBehaviour
  {
    private float m_sinkRate;
    private AssetsDb m_assetsDb;

    public void Initialize(
      TreeStumpData stump,
      TerrainTile tile,
      ICalendar calendar,
      AssetsDb assetsDb)
    {
      Assert.That<Tile2iSlim>(stump.Id.Position).IsEqualTo<Tile2iSlim>(tile.TileCoordSlim);
      this.m_assetsDb = assetsDb;
      this.transform.rotation = Quaternion.AngleAxis(stump.Rotation.ToUnityAngleDegrees(), Vector3.up);
      this.transform.localScale = Vector3.one * (float) (0.10000000149011612 + (double) stump.Scale.ToFloat() * 0.89999997615814209);
      this.transform.position = stump.Position.ExtendHeight(stump.PlantedAtHeight + stump.GetDeltaHeight(calendar)).ToVector3();
      this.m_sinkRate = -TreesManager.STUMP_SINK_RATE_PER_MONTH.ToFloat() / (float) Duration.OneMonth.Ticks;
    }

    public void LoadPrefabFromTree(TreeMb treeMb)
    {
      GameObject clonedPrefabOrEmptyGo = this.m_assetsDb.GetClonedPrefabOrEmptyGo(treeMb.CutTreePrefabPath);
      Transform transform = clonedPrefabOrEmptyGo.transform.Find("root");
      if (!(bool) (Object) transform)
      {
        Log.Error("No root found in " + treeMb.CutTreePrefabPath);
      }
      else
      {
        GameObject gameObject = Object.Instantiate<GameObject>(transform.gameObject);
        clonedPrefabOrEmptyGo.Destroy();
        Material material = treeMb.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.transform.SetParent(this.transform, false);
      }
    }

    public void LoadPrefabFromProto(TreeProto proto)
    {
      Pair<string, string> first = proto.TreeGraphics.PrefabPaths.First;
      GameObject clonedPrefabOrEmptyGo1 = this.m_assetsDb.GetClonedPrefabOrEmptyGo(first.First);
      GameObject clonedPrefabOrEmptyGo2 = this.m_assetsDb.GetClonedPrefabOrEmptyGo(first.Second);
      Transform transform = clonedPrefabOrEmptyGo2.transform.Find("root");
      if ((Object) transform == (Object) null)
      {
        Log.Error("No root found in " + first.Second);
      }
      else
      {
        GameObject gameObject = Object.Instantiate<GameObject>(transform.gameObject);
        Material material = clonedPrefabOrEmptyGo1.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.transform.SetParent(this.transform, false);
        clonedPrefabOrEmptyGo1.Destroy();
        clonedPrefabOrEmptyGo2.Destroy();
      }
    }

    public void SyncUpdate(GameTime time) => this.transform.Translate(0.0f, this.m_sinkRate, 0.0f);

    public StumpMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
