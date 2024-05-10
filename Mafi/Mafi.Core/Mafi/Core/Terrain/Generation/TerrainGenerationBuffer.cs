// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TerrainGenerationBuffer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>Helper class for building contents of a tile.</summary>
  public class TerrainGenerationBuffer
  {
    private static readonly ThicknessTilesF s_minThickness;
    /// <summary>
    /// Product ranges are ordered from bottom to top (ascending order) and are non-overlapping. Exclusive end of
    /// range at index `i` is less or equal than inclusive start of range at `i+1`. Every method must maintain this
    /// invariant.
    /// </summary>
    private readonly Lyst<TerrainGenerationBuffer.ProductRange> m_materials;
    /// <summary>Tree on this tile.</summary>
    public TreeData TreeData;
    /// <summary>Prop on this tile.</summary>
    public TerrainPropData TerrainPropData;

    public bool IsEmpty => this.m_materials.IsEmpty;

    /// <summary>
    /// Base surface height set for this buffer. All the thicknesses are relative to this height.
    /// </summary>
    public HeightTilesF BaseSurfaceHeight { get; private set; }

    /// <summary>
    /// Current surface height. This is equal to the height of the last product or to <see cref="P:Mafi.Core.Terrain.Generation.TerrainGenerationBuffer.BaseSurfaceHeight" />
    /// if there are no products.
    /// </summary>
    public HeightTilesF SurfaceHeight
    {
      get
      {
        return this.BaseSurfaceHeight + (this.m_materials.IsEmpty ? ThicknessTilesF.Zero : this.m_materials.Last.End);
      }
    }

    public HeightTilesF LowestMaterialBottomHeight
    {
      get
      {
        return this.BaseSurfaceHeight + (this.m_materials.IsEmpty ? ThicknessTilesF.Zero : this.m_materials.First.Start);
      }
    }

    public Option<TerrainMaterialProto> TopMaterial
    {
      get
      {
        return !this.m_materials.IsEmpty ? (Option<TerrainMaterialProto>) this.m_materials.Last.Material : Option<TerrainMaterialProto>.None;
      }
    }

    /// <summary>
    /// Initializes this buffer by setting <see cref="P:Mafi.Core.Terrain.Generation.TerrainGenerationBuffer.BaseSurfaceHeight" />. This should be called once before any
    /// other operations. Method <see cref="M:Mafi.Core.Terrain.Generation.TerrainGenerationBuffer.Clear" /> clears the initialization.
    /// </summary>
    public void Initialize(HeightTilesF defaultSurfaceHeight)
    {
      this.BaseSurfaceHeight = defaultSurfaceHeight;
    }

    public void Clear()
    {
      this.BaseSurfaceHeight = HeightTilesF.MaxValue;
      this.TreeData = new TreeData();
      this.TerrainPropData = new TerrainPropData();
      this.m_materials.Clear();
    }

    /// <summary>
    /// Adds given material on top of the current buffer. If there is nothing in the buffer the material is added on
    /// top of <see cref="P:Mafi.Core.Terrain.Generation.TerrainGenerationBuffer.BaseSurfaceHeight" />.
    /// </summary>
    public void DepositOnTop(TerrainMaterialProto material, ThicknessTilesF thickness)
    {
      Assert.That<HeightTilesF>(this.BaseSurfaceHeight).IsNotEqualTo(HeightTilesF.MaxValue, "Not initialized!");
      Assert.That<TerrainMaterialProto>(material).IsNotNullOrPhantom<TerrainMaterialProto>();
      Assert.That<ThicknessTilesF>(thickness).IsPositive();
      ThicknessTilesF start = this.m_materials.IsEmpty ? ThicknessTilesF.Zero : this.m_materials.Last.End;
      this.m_materials.Add(new TerrainGenerationBuffer.ProductRange(material, start, start + thickness));
    }

    /// <summary>
    /// Removes products of given thickness from top. This only clears extra products and does not move relative
    /// buffer height. Does nothing when there are no extra products in the buffer. Logical inverse of <see cref="M:Mafi.Core.Terrain.Generation.TerrainGenerationBuffer.DepositOnTop(Mafi.Core.Products.TerrainMaterialProto,Mafi.ThicknessTilesF)" />.
    /// </summary>
    public void RemoveFromTop(ThicknessTilesF thickness)
    {
      if (this.m_materials.IsEmpty)
        return;
      this.ClearRange(this.m_materials.Last.End - thickness, this.m_materials.Last.End);
    }

    /// <summary>
    /// Sets product in given height range. If <paramref name="replaceOtherProducts" /> is set, other products in
    /// specified range will be removed and this product will replace them. Otherwise, only empty space will be
    /// filled with new product.
    /// </summary>
    public void SetProductInRange(
      TerrainMaterialProto material,
      HeightTilesF bottomHeight,
      HeightTilesF topHeight,
      bool replaceOtherProducts)
    {
      Assert.That<HeightTilesF>(bottomHeight).IsLessOrEqual(topHeight);
      this.SetProductInRange(material, bottomHeight - this.BaseSurfaceHeight, topHeight - this.BaseSurfaceHeight, replaceOtherProducts);
    }

    /// <summary>
    /// Sets product relative to the surface height. If <paramref name="replaceOtherProducts" /> is set, other
    /// products in specified range will be removed and this product will replace them. Otherwise, only empty space
    /// will be filled with new product.
    /// </summary>
    public void SetProductInRange(
      TerrainMaterialProto material,
      ThicknessTilesF bottomHeight,
      ThicknessTilesF topHeight,
      bool replaceOtherProducts)
    {
      Assert.That<HeightTilesF>(this.BaseSurfaceHeight).IsNotEqualTo(HeightTilesF.MaxValue, "Not initialized!");
      Assert.That<TerrainMaterialProto>(material).IsNotNullOrPhantom<TerrainMaterialProto>();
      Assert.That<ThicknessTilesF>(bottomHeight).IsLessOrEqual(topHeight);
      if (bottomHeight >= topHeight)
        return;
      if (replaceOtherProducts)
        this.ClearRange(bottomHeight, topHeight);
      if (this.m_materials.IsEmpty || this.m_materials.Last.End <= bottomHeight)
        this.m_materials.Add(new TerrainGenerationBuffer.ProductRange(material, bottomHeight, topHeight));
      else if (topHeight <= this.m_materials.First.Start)
      {
        this.m_materials.Insert(0, new TerrainGenerationBuffer.ProductRange(material, bottomHeight, topHeight));
      }
      else
      {
        if (bottomHeight < this.m_materials.First.Start)
        {
          Assert.That<ThicknessTilesF>(topHeight).IsGreater(this.m_materials.First.Start, "Not overlapping?");
          this.m_materials.Insert(0, new TerrainGenerationBuffer.ProductRange(material, bottomHeight, this.m_materials.First.Start));
        }
        for (int index = 1; index < this.m_materials.Count; ++index)
        {
          ThicknessTilesF end1 = this.m_materials[index - 1].End;
          ThicknessTilesF start1 = this.m_materials[index].Start;
          if (!(end1 == start1) && !(start1 <= bottomHeight))
          {
            if (!(topHeight <= end1))
            {
              ThicknessTilesF start2 = end1.Max(bottomHeight);
              ThicknessTilesF end2 = start1.Min(topHeight);
              this.m_materials.Insert(index, new TerrainGenerationBuffer.ProductRange(material, start2, end2));
              ++index;
            }
            else
              break;
          }
        }
        if (!(topHeight > this.m_materials.Last.End))
          return;
        Assert.That<ThicknessTilesF>(bottomHeight).IsLess(this.m_materials.Last.End, "Not overlapping?");
        this.m_materials.Add(new TerrainGenerationBuffer.ProductRange(material, this.m_materials.Last.End, topHeight));
      }
    }

    /// <summary>Clear any products from given range.</summary>
    /// <param name="start">Inclusive start of cleared range.</param>
    /// <param name="end">Exclusive end of cleared range.</param>
    public void ClearRange(HeightTilesF start, HeightTilesF end)
    {
      Assert.That<HeightTilesF>(start).IsLessOrEqual(end);
      this.ClearRange(start - this.BaseSurfaceHeight, end - this.BaseSurfaceHeight);
    }

    /// <summary>Clear any products from given range.</summary>
    /// <param name="start">Inclusive start of cleared range.</param>
    /// <param name="end">Exclusive end of cleared range.</param>
    public void ClearRange(ThicknessTilesF start, ThicknessTilesF end)
    {
      Assert.That<HeightTilesF>(this.BaseSurfaceHeight).IsNotEqualTo(HeightTilesF.MaxValue, "Not initialized!");
      Assert.That<ThicknessTilesF>(start).IsLessOrEqual(end);
      if (start >= end)
        return;
      for (int index = 0; index < this.m_materials.Count; ++index)
      {
        TerrainGenerationBuffer.ProductRange material = this.m_materials[index];
        if (!(material.End <= start))
        {
          if (end <= material.Start)
            break;
          if (start <= material.Start)
          {
            if (end < material.End)
            {
              this.m_materials[index] = new TerrainGenerationBuffer.ProductRange(material.Material, end, material.End);
              break;
            }
            this.m_materials.RemoveAt(index);
            --index;
          }
          else
          {
            if (end < material.End)
            {
              this.m_materials[index] = new TerrainGenerationBuffer.ProductRange(material.Material, material.Start, start);
              this.m_materials.Insert(index + 1, new TerrainGenerationBuffer.ProductRange(material.Material, end, material.End));
              break;
            }
            this.m_materials[index] = new TerrainGenerationBuffer.ProductRange(material.Material, material.Start, start);
          }
        }
      }
    }

    /// <summary>
    /// Clears all products from <paramref name="startHeight" /> above.
    /// </summary>
    public void ClearAbove(HeightTilesF startHeight)
    {
      this.ClearRange(startHeight - this.BaseSurfaceHeight, ThicknessTilesF.MaxValue);
    }

    /// <summary>
    /// Adds new product on the surface to the given height with given thickness. This product will replace current
    /// products. The <paramref name="replacementIntensity" /> specifies how strongly to remove current products,
    /// value 1.0 will cause current products to be completely removed and replaced by the new product. Value 0.0
    /// will leave the current product at its height. New product is placed only if the current product is lower then
    /// new one after removal caused by <paramref name="replacementIntensity" />.
    /// </summary>
    public void AddNewSurfaceProduct(
      TerrainMaterialProto material,
      HeightTilesF surfaceHeight,
      ThicknessTilesF thickness,
      Percent replacementIntensity)
    {
      HeightTilesF surfaceHeight1 = this.SurfaceHeight;
      HeightTilesF bottomHeight = surfaceHeight - thickness;
      ThicknessTilesF thickness1 = (bottomHeight - surfaceHeight1).ScaledBy(replacementIntensity);
      if (thickness1.IsPositive)
        this.RemoveFromTop(thickness1);
      this.SetProductInRange(material, bottomHeight, surfaceHeight, false);
    }

    public HeightTilesF GetThicknessesStackAndClear(
      TerrainMaterialProto bedrock,
      ref LystStruct<TerrainMaterialThicknessSlim> outThicknesses,
      out TreeData treeData,
      out TerrainPropData terrainPropData)
    {
      HeightTilesF thicknessesStack = this.GetThicknessesStack(bedrock, ref outThicknesses);
      treeData = this.TreeData;
      terrainPropData = this.TerrainPropData;
      this.Clear();
      return thicknessesStack;
    }

    /// <summary>
    /// Transforms internal representation into list of thicknesses and surface height. This also removes layers of
    /// material that are thinner than 0.5 tiles. Returned thicknesses are from bottom to top.
    /// </summary>
    public HeightTilesF GetThicknessesStack(
      TerrainMaterialProto bedrock,
      ref LystStruct<TerrainMaterialThicknessSlim> outThicknesses)
    {
      Assert.That<HeightTilesF>(this.BaseSurfaceHeight).IsNotEqualTo(HeightTilesF.MaxValue, "Not initialized!");
      if (outThicknesses.IsNotEmpty)
      {
        Assert.Fail("Not empty output array!");
        outThicknesses.Clear();
      }
      if (this.m_materials.IsEmpty)
        return this.SurfaceHeight;
      ref LystStruct<TerrainMaterialThicknessSlim> local1 = ref outThicknesses;
      TerrainMaterialThickness materialThickness = this.m_materials.First.ProductThickness;
      TerrainMaterialThicknessSlim asSlim1 = materialThickness.AsSlim;
      local1.Add(asSlim1);
      ThicknessTilesF end = this.m_materials.First.End;
      for (int index = 1; index < this.m_materials.Count; ++index)
      {
        TerrainGenerationBuffer.ProductRange material = this.m_materials[index];
        ThicknessTilesF thickness = material.Start - end;
        if (thickness < TerrainGenerationBuffer.s_minThickness)
        {
          outThicknesses.Last += thickness;
        }
        else
        {
          ref LystStruct<TerrainMaterialThicknessSlim> local2 = ref outThicknesses;
          materialThickness = new TerrainMaterialThickness(bedrock, thickness);
          TerrainMaterialThicknessSlim asSlim2 = materialThickness.AsSlim;
          local2.Add(asSlim2);
        }
        if (material.Material.SlimId == outThicknesses.Last.SlimId)
        {
          outThicknesses.Last += material.Thickness;
        }
        else
        {
          ref LystStruct<TerrainMaterialThicknessSlim> local3 = ref outThicknesses;
          materialThickness = material.ProductThickness;
          TerrainMaterialThicknessSlim asSlim3 = materialThickness.AsSlim;
          local3.Add(asSlim3);
        }
        end = material.End;
      }
      for (int index1 = 1; index1 < outThicknesses.Count; ++index1)
      {
        TerrainMaterialThicknessSlim materialThicknessSlim1 = outThicknesses[index1 - 1];
        TerrainMaterialSlimId slimId1 = materialThicknessSlim1.SlimId;
        materialThicknessSlim1 = outThicknesses[index1];
        TerrainMaterialSlimId slimId2 = materialThicknessSlim1.SlimId;
        if (!(slimId1 != slimId2))
        {
          ref LystStruct<TerrainMaterialThicknessSlim> local4 = ref outThicknesses;
          int index2 = index1 - 1;
          ref LystStruct<TerrainMaterialThicknessSlim> local5 = ref local4;
          int index3 = index2;
          TerrainMaterialThicknessSlim materialThicknessSlim2 = local4[index2];
          materialThicknessSlim1 = outThicknesses[index1];
          ThicknessTilesF thickness = materialThicknessSlim1.Thickness;
          TerrainMaterialThicknessSlim materialThicknessSlim3 = materialThicknessSlim2 + thickness;
          local5[index3] = materialThicknessSlim3;
          outThicknesses.RemoveAt(index1);
          --index1;
        }
      }
      return this.SurfaceHeight;
    }

    public override string ToString()
    {
      return this.m_materials.AsEnumerable<TerrainGenerationBuffer.ProductRange>().Reverse<TerrainGenerationBuffer.ProductRange>().Select<TerrainGenerationBuffer.ProductRange, string>((Func<TerrainGenerationBuffer.ProductRange, string>) (x => string.Format("[{0} ~ {1} ~ {2})", (object) x.Start.Value.ToStringRounded(1), (object) x.Material.Id, (object) x.End.Value.ToStringRounded(1)))).JoinStrings("\n");
    }

    public TerrainGenerationBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_materials = new Lyst<TerrainGenerationBuffer.ProductRange>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TerrainGenerationBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainGenerationBuffer.s_minThickness = 0.5.TilesThick();
    }

    private readonly struct ProductRange
    {
      public readonly TerrainMaterialProto Material;
      /// <summary>Inclusive start thickness.</summary>
      public readonly ThicknessTilesF Start;
      /// <summary>Exclusive end thickness.</summary>
      public readonly ThicknessTilesF End;

      public ProductRange(
        TerrainMaterialProto material,
        ThicknessTilesF start,
        ThicknessTilesF end)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Assert.That<ThicknessTilesF>(start).IsLess(end);
        this.Material = material.CheckNotNullOrPhantom<TerrainMaterialProto>();
        this.Start = start;
        this.End = end;
      }

      public ThicknessTilesF Thickness => this.End - this.Start;

      public TerrainMaterialThickness ProductThickness
      {
        get => new TerrainMaterialThickness(this.Material, this.Thickness);
      }

      public override string ToString()
      {
        return string.Format("{0} from {1} to {2} (thick: {3})", (object) this.Material, (object) this.Start, (object) this.End, (object) this.Thickness);
      }
    }
  }
}
