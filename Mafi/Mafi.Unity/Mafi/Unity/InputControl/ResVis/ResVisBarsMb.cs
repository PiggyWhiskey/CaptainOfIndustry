// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ResVis.ResVisBarsMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.ResVis
{
  /// <summary>
  /// Represents all resource bars of an individual products.
  /// </summary>
  [RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
  internal class ResVisBarsMb : BuildableMb
  {
    private readonly MeshBuilder m_builder;
    private int m_barsToBuild;
    private int m_heightDivider;
    private RelTile1f m_radius;
    private bool m_useCylinders;

    public static ResVisBarsMb CreateInstance(
      string name,
      Material material,
      GameObject parent,
      RelTile1f radius,
      int heightDivider = 1,
      bool useCylinders = false)
    {
      ResVisBarsMb instance = new GameObject("Res bars " + name).AddComponent<ResVisBarsMb>();
      instance.m_radius = radius;
      instance.m_heightDivider = heightDivider;
      instance.m_useCylinders = useCylinders;
      MeshRenderer component = instance.GetComponent<MeshRenderer>();
      component.sharedMaterial = material;
      component.shadowCastingMode = ShadowCastingMode.Off;
      instance.transform.parent = parent.transform;
      return instance;
    }

    public ThicknessTilesF AppendBar(
      Tile2i tile,
      ThicknessTilesF productThickness,
      HeightTilesF height)
    {
      ThicknessTilesF thicknessTilesF = productThickness / this.m_heightDivider / 2;
      Tile3f tile1 = tile.CornerTile2f.ExtendZ((height + thicknessTilesF).Value);
      RelTile3f tile2 = new RelTile3f(this.m_radius.Value, this.m_radius.Value, thicknessTilesF.Value);
      if (this.m_useCylinders)
        this.m_builder.AddCylinderAlongZ(tile1.ToVector3(), tile2.ToVector3(), new Color32((byte) 192, (byte) 192, (byte) 192, byte.MaxValue), omitBottomLid: true);
      else
        this.m_builder.AddAaBox(tile1.ToVector3(), tile2.ToVector3(), new Color32((byte) 192, (byte) 192, (byte) 192, byte.MaxValue), ~BoxFaceMask.MinusY);
      ++this.m_barsToBuild;
      return thicknessTilesF * 2;
    }

    public void ApplyChanges()
    {
      if (this.m_barsToBuild <= 0)
        return;
      this.m_builder.UpdateMbAndClear((IBuildable) this);
      this.m_barsToBuild = 0;
    }

    public ResVisBarsMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_builder = new MeshBuilder();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
