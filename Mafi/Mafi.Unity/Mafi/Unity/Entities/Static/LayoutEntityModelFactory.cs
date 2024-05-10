// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.LayoutEntityModelFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  /// <summary>
  /// Default factory that fetches models for layout entities based on their prototype. It can also generate a model
  /// from the layout that is used as a backup in case of missing models or as model creation blueprint.
  /// </summary>
  /// <remarks>Use <see cref="T:Mafi.Unity.Entities.ProtoModelFactory" /> to invoke this factory.</remarks>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class LayoutEntityModelFactory : 
    IProtoModelFactory<ILayoutEntityProto>,
    IFactory<ILayoutEntityProto, GameObject>
  {
    private readonly AssetsDb m_assetsDb;
    private readonly Material m_vertexColorMaterial;
    /// <summary>
    /// A cache of prefabs after we've stripped empty children from them. These children
    /// are often used for animation, and our instanced animations don't need them.
    /// Cost of placing a entity with many children was reduced from ~1.5ms to ~0.5ms.
    /// There is also a memory saving to not having all these empty children around.
    /// </summary>
    private readonly Dict<string, GameObject> m_instancedPrefabCache;

    public bool AlwaysIncludeGeneratedLayout { get; set; }

    public LayoutEntityModelFactory(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_instancedPrefabCache = new Dict<string, GameObject>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      this.m_vertexColorMaterial = assetsDb.GetSharedMaterial("Assets/Core/Materials/VertexColor.mat");
    }

    private void stripEmptyChildren(GameObject entityGo, ILayoutEntityProto proto)
    {
      Lyst<GameObject> outGos = new Lyst<GameObject>();
      entityGo.GetAllChildren((Predicate<GameObject>) (c => c.transform.childCount == 0), outGos);
      bool flag = true;
      int num = 0;
      while (flag)
      {
        flag = false;
        ++num;
        if (num > 100)
        {
          Log.Error("While loop overflow");
          break;
        }
        foreach (GameObject go in outGos)
        {
          if (go.IsNullOrDestroyed())
            Log.Warning("Destroyed leaf should not be here " + go.name);
          else if (go.GetComponents<Component>().Length == 1)
          {
            go.Destroy();
            flag = true;
          }
        }
        if (flag)
        {
          outGos.Clear();
          entityGo.GetAllChildren((Predicate<GameObject>) (c => !c.IsNullOrDestroyed() && !anyAliveChildren(c)), outGos);
        }
      }

      static bool anyAliveChildren(GameObject go)
      {
        for (int index = 0; index < go.transform.childCount; ++index)
        {
          if (!go.transform.GetChild(index).gameObject.IsNullOrDestroyed())
            return true;
        }
        return false;
      }
    }

    /// <summary>
    /// Creates a new model based on prefab from the asset DB and makes sure it has a collider attached. If no model
    /// is found a generated layout mesh is returned.
    /// </summary>
    /// <remarks>Implementation of <see cref="T:Mafi.IFactory`2" />.</remarks>
    public GameObject Create(ILayoutEntityProto proto)
    {
      string prefabPath = proto.Graphics.PrefabPath;
      if (string.IsNullOrEmpty(prefabPath))
      {
        Log.Error(string.Format("No prefab(s) specified for '{0}'.", (object) proto.Id));
        return this.GenerateLayoutMesh(proto, true);
      }
      GameObject prefab;
      if (proto.Graphics.UseSemiInstancedRendering && !proto.Graphics.DisableEmptyChildrenStripping)
      {
        if (!this.m_instancedPrefabCache.TryGetValue(prefabPath, out prefab))
        {
          if (!this.m_assetsDb.TryGetClonedPrefab(prefabPath, out prefab))
            return this.GenerateLayoutMesh(proto, true);
          this.stripEmptyChildren(prefab, proto);
          prefab.SetActive(false);
          this.m_instancedPrefabCache.AddAndAssertNew(prefabPath, prefab);
        }
        prefab = UnityEngine.Object.Instantiate<GameObject>(prefab);
      }
      else if (!this.m_assetsDb.TryGetClonedPrefab(prefabPath, out prefab))
        return this.GenerateLayoutMesh(proto, true);
      prefab.SetActive(true);
      if (prefab.transform.position != Vector3.zero)
        prefab.transform.position = Vector3.zero;
      if (prefab.transform.rotation != Quaternion.identity)
        prefab.transform.rotation = Quaternion.identity;
      if (!(bool) (UnityEngine.Object) prefab.GetComponent<Collider>())
      {
        Log.Warning(string.Format("No collider found for layout entity '{0}' (prefab: '{1}').", (object) proto.Id, (object) prefabPath));
        BoxCollider boxCollider = prefab.AddComponent<BoxCollider>();
        boxCollider.size = proto.Layout.CoreSize.ExtendHeight(proto.Layout.LayoutSize.Thickness).ToVector3();
        boxCollider.center = new Vector3(0.0f, (float) proto.Layout.LayoutSize.Z.TilesToUnityUnits() / 2f, 0.0f);
        if (proto.Layout.OriginTile.HasValue)
        {
          Vector3 vector3 = (proto.Layout.GetCenter(TileTransform.Identity) - proto.Layout.GetModelOrigin(TileTransform.Identity)).ToVector3();
          boxCollider.center = new Vector3(vector3.x, boxCollider.center.y, vector3.z);
        }
      }
      if (this.AlwaysIncludeGeneratedLayout)
      {
        GameObject layoutMesh = this.GenerateLayoutMesh(proto, false);
        layoutMesh.transform.SetParent(prefab.transform, false);
        layoutMesh.transform.position = proto.Graphics.PrefabOrigin.ToVector3();
      }
      return prefab;
    }

    /// <summary>
    /// Creates layout mesh from given <paramref name="layoutEntityProto" />.
    /// </summary>
    public GameObject GenerateLayoutMesh(ILayoutEntityProto layoutEntityProto, bool includeCollider)
    {
      GameObject layoutMesh = new GameObject(layoutEntityProto.Id.Value);
      MeshBuilder instance = MeshBuilder.Instance;
      EntityLayout layout = layoutEntityProto.Layout;
      instance.SetTransform(layout.TransformRelative(RelTile2i.Zero, TileTransform.Identity).ExtendZ(0).ToVector3() - layout.GetCenter(TileTransform.Identity).ToVector3());
      this.AppendLayoutMesh(layout, instance);
      instance.UpdateMbAndClear((IBuildable) layoutMesh.AddComponent<BuildableMb>());
      layoutMesh.GetComponent<MeshRenderer>().sharedMaterial = this.m_vertexColorMaterial;
      if (includeCollider)
        layoutMesh.AddComponent<BoxCollider>();
      return layoutMesh;
    }

    /// <summary>
    /// Appends layout mesh generated from given <paramref name="layout" /> to given <paramref name="builder" />.
    /// </summary>
    public void AppendLayoutMesh(EntityLayout layout, MeshBuilder builder)
    {
      Color32 color32_1 = new Color32((byte) 192, (byte) 192, (byte) 192, byte.MaxValue);
      Color32 color32_2 = new Color32((byte) 64, (byte) 64, (byte) 64, byte.MaxValue);
      Vector3 vector3_1 = (Tile3f.One / (Fix32) 2).ToVector3();
      Dict<RelTile2i, ThicknessTilesF> dict1 = layout.VehicleSurfaceHeights.AsEnumerable().ToDict<RelTile2i, ThicknessTilesF>();
      foreach (LayoutTile layoutTile in layout.LayoutTiles)
      {
        int num = layoutTile.OccupiedThickness.To.Value;
        bool flag = layoutTile.HasVehicleSurface;
        ThicknessTilesF thicknessTilesF1;
        RelTile2i relTile2i;
        if (flag)
        {
          thicknessTilesF1 = dict1[layoutTile.Coord];
          ref ThicknessTilesF local1 = ref thicknessTilesF1;
          Dict<RelTile2i, ThicknessTilesF> dict2 = dict1;
          relTile2i = layoutTile.Coord;
          RelTile2i incrementX = relTile2i.IncrementX;
          ThicknessTilesF rhs1 = dict2[incrementX];
          thicknessTilesF1 = local1.Min(rhs1);
          ref ThicknessTilesF local2 = ref thicknessTilesF1;
          Dict<RelTile2i, ThicknessTilesF> dict3 = dict1;
          relTile2i = layoutTile.Coord;
          RelTile2i incrementY1 = relTile2i.IncrementY;
          ThicknessTilesF rhs2 = dict3[incrementY1];
          thicknessTilesF1 = local2.Min(rhs2);
          ref ThicknessTilesF local3 = ref thicknessTilesF1;
          Dict<RelTile2i, ThicknessTilesF> dict4 = dict1;
          relTile2i = layoutTile.Coord;
          relTile2i = relTile2i.IncrementX;
          RelTile2i incrementY2 = relTile2i.IncrementY;
          ThicknessTilesF rhs3 = dict4[incrementY2];
          ThicknessTilesF thicknessTilesF2 = local3.Min(rhs3);
          if (thicknessTilesF2 > EntityLayout.VEHICLE_INACCESSIBLE_HEIGHT_REL)
            num = thicknessTilesF2.FlooredThicknessTilesI.Value;
          else
            flag = false;
        }
        Tile2i tile2i;
        for (int z = layoutTile.OccupiedThickness.From.Value; z < num; ++z)
        {
          tile2i = Tile2i.Zero + layoutTile.Coord;
          Tile3i tile = tile2i.ExtendZ(z);
          Color32 color = tile.Sum % 2 == 0 ? color32_1 : color32_2;
          if (layoutTile.Constraint.HasFlag((Enum) LayoutTileConstraint.Ocean))
            color.b += (byte) 63;
          if (layoutTile.Constraint.HasFlag((Enum) LayoutTileConstraint.Ground))
            color.b += (byte) 63;
          builder.AddAaBox(tile.ToCenterVector3(), vector3_1, color);
        }
        if (flag)
        {
          tile2i = Tile2i.Zero + layoutTile.Coord;
          Vector3 cornerVector3 = tile2i.ExtendZ(0).ToCornerVector3();
          float unityUnits1 = RelTile1i.One.ToUnityUnits();
          Vector3 v0 = cornerVector3;
          Vector3 vector3_2 = cornerVector3 + new Vector3(unityUnits1, 0.0f, 0.0f);
          Vector3 vector3_3 = cornerVector3 + new Vector3(0.0f, 0.0f, unityUnits1);
          Vector3 vector3_4 = cornerVector3 + new Vector3(unityUnits1, 0.0f, unityUnits1);
          float unityUnits2 = dict1[layoutTile.Coord].ToUnityUnits();
          Vector3 vector3_5 = v0 + new Vector3(0.0f, unityUnits2, 0.0f);
          Dict<RelTile2i, ThicknessTilesF> dict5 = dict1;
          relTile2i = layoutTile.Coord;
          RelTile2i incrementX = relTile2i.IncrementX;
          float unityUnits3 = dict5[incrementX].ToUnityUnits();
          Vector3 vector3_6 = vector3_2 + new Vector3(0.0f, unityUnits3, 0.0f);
          Dict<RelTile2i, ThicknessTilesF> dict6 = dict1;
          relTile2i = layoutTile.Coord;
          RelTile2i incrementY3 = relTile2i.IncrementY;
          float unityUnits4 = dict6[incrementY3].ToUnityUnits();
          Vector3 vector3_7 = vector3_3 + new Vector3(0.0f, unityUnits4, 0.0f);
          Dict<RelTile2i, ThicknessTilesF> dict7 = dict1;
          relTile2i = layoutTile.Coord;
          relTile2i = relTile2i.IncrementX;
          RelTile2i incrementY4 = relTile2i.IncrementY;
          float unityUnits5 = dict7[incrementY4].ToUnityUnits();
          Vector3 v2 = vector3_4 + new Vector3(0.0f, unityUnits5, 0.0f);
          thicknessTilesF1 = dict1[layoutTile.Coord];
          int z = thicknessTilesF1.FlooredThicknessTilesI.Value;
          relTile2i = layoutTile.Coord;
          Color32 color = relTile2i.ExtendZ(z).Sum % 2 == 0 ? color32_1 : color32_2;
          if (layoutTile.OccupiedThickness.From.Value < num)
          {
            builder.AddQuad(v0, vector3_5, vector3_6, vector3_2, color);
            builder.AddQuad(vector3_3, vector3_4, v2, vector3_7, color);
            builder.AddQuad(v0, vector3_3, vector3_7, vector3_5, color);
            builder.AddQuad(vector3_2, vector3_6, v2, vector3_4, color);
          }
          color.g += (byte) 63;
          builder.AddQuad(vector3_5, vector3_7, v2, vector3_6, color);
        }
      }
    }
  }
}
