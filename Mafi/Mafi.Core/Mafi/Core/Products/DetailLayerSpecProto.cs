// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.DetailLayerSpecProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  public class DetailLayerSpecProto : Proto
  {
    /// <summary>
    /// Max density is capped at 99% since we only support 2^16 - 1 detail instances but chunks have 2^16 tiles.
    /// </summary>
    public const float MAX_DETAIL_DENSITY = 0.99f;
    /// <summary>
    /// Path for the prefab that specifies the mesh and material to use.
    /// </summary>
    public readonly string PrefabPath;
    /// <summary>Texture path for the prefab.</summary>
    /// <remarks>We specify texture separately to be able to override it easily during runtime.</remarks>
    public readonly string TexturePath;
    /// <summary>
    /// Spawn probability range (0, 1), max value is <see cref="F:Mafi.Core.Products.DetailLayerSpecProto.MAX_DETAIL_DENSITY" />.
    /// </summary>
    public readonly float UniformRandomSpawnProbability;
    public readonly float UniformRandomSpawnProbabilityFar;
    /// <summary>
    /// How much detail weight affects y-offset. Value of 1.0 means that weight of 30% will lower detail by 70% of tile.
    /// Value of 0.5 means that weight of 30% will lower detail by only 35% of tile.
    /// </summary>
    /// <remarks>
    /// Y-offset multiplier is here, not on variant proto, because we don't know variant index during update operation.
    /// </remarks>
    public readonly float YOffsetWeightMult;
    /// <summary>
    /// Sensitivity to wind. If set to 0, no wind animation is performed.
    /// </summary>
    public readonly float WindSensitivity;
    /// <summary>
    /// Weight of UP normal compared to the true mesh normal. When set to 0, mesh normal will be used. When set to
    /// 1, all normals will be overriden wh UP normal. Linear interpolation is used in between. This is used to
    /// make grass and other billboard better blend with terrain.
    /// </summary>
    public readonly float UpNormalWeight;
    public readonly ColorUniversal TintColorAndWeight;
    public readonly ImmutableArray<DetailLayerSpecProto.DetailVariant> Variants;
    public readonly int VariantsTotalWeight;
    public readonly int RandomSeed;

    public DetailLayerSpecProto(
      Proto.ID id,
      string prefabPath,
      string texturePath,
      float uniformSpawnProbability,
      float uniformSpawnProbabilityFar,
      float yOffsetWeightMult,
      float windSensitivity,
      float upNormalWeight,
      ColorRgba tintColorAndWeight,
      ImmutableArray<DetailLayerSpecProto.DetailVariant> variants)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      Assert.That<ImmutableArray<DetailLayerSpecProto.DetailVariant>>(variants).IsNotEmpty<DetailLayerSpecProto.DetailVariant>();
      this.PrefabPath = prefabPath.CheckNotNullOrEmpty();
      this.TexturePath = texturePath.CheckNotNullOrEmpty();
      this.UniformRandomSpawnProbability = uniformSpawnProbability.CheckWithinIncl(1f / 1000f, 0.99f);
      this.UniformRandomSpawnProbabilityFar = uniformSpawnProbabilityFar;
      this.YOffsetWeightMult = yOffsetWeightMult.CheckWithinIncl(0.0f, 1f);
      this.WindSensitivity = windSensitivity.CheckWithinIncl(0.0f, 1f);
      this.UpNormalWeight = upNormalWeight.CheckWithinIncl(0.0f, 1f);
      this.TintColorAndWeight = (ColorUniversal) tintColorAndWeight;
      this.Variants = variants.Filter((Predicate<DetailLayerSpecProto.DetailVariant>) (x => x.SpawnWeight > 0)).Sort((Comparison<DetailLayerSpecProto.DetailVariant>) ((x, y) => -x.SpawnWeight.CompareTo(y.SpawnWeight))).CheckNotEmpty<DetailLayerSpecProto.DetailVariant>();
      this.VariantsTotalWeight = variants.Sum((Func<DetailLayerSpecProto.DetailVariant, int>) (x => x.SpawnWeight));
      this.RandomSeed = prefabPath.GetHashCode() ^ id.GetHashCode();
    }

    public readonly struct DetailVariant
    {
      public readonly int SpawnWeight;
      public readonly float BaseScale;
      public readonly float PositionRandomness;
      public readonly float ScaleVariation;
      public readonly uint UvOriginAndSizePacked;
      /// <summary>
      /// Precomputed value for <c>PositionRandomness / (sbyte.MaxValue + 1)</c>.
      /// </summary>
      public readonly float PositionRandomnessOverrSbyteMaxMult;
      /// <summary>
      /// Precomputed value for <c>ScaleVariation / (short.MaxValue + 1)</c>.
      /// </summary>
      public readonly float ScaleVariationOverShortMaxMult;

      private DetailVariant(
        int spawnWeight,
        float baseScale,
        float positionRandomness,
        float scaleVariation,
        float uvOriginX,
        float uvOriginY,
        float uvSize)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.SpawnWeight = spawnWeight.CheckPositive();
        this.BaseScale = baseScale.CheckPositive();
        this.PositionRandomness = positionRandomness.CheckNotNegative();
        this.ScaleVariation = scaleVariation.CheckNotNegative();
        this.UvOriginAndSizePacked = DetailLayerSpecProto.DetailVariant.PackUvOriginAndSize(uvOriginX, uvOriginY, uvSize);
        this.ScaleVariationOverShortMaxMult = this.ScaleVariation / 32768f;
        this.PositionRandomnessOverrSbyteMaxMult = this.PositionRandomness / 128f;
        if ((double) this.BaseScale + (double) this.ScaleVariation > 2.0)
          throw new InvalidProtoException(string.Format("Base scale + scale ({0} + {1} ", (object) baseScale, (object) scaleVariation) + string.Format("= {0}) variation cannot be larger than 2.", (object) (float) ((double) baseScale + (double) scaleVariation)));
        if ((double) this.ScaleVariation >= (double) this.BaseScale)
          throw new InvalidProtoException(string.Format("Scale variation {0} cannot be greater or equal ", (object) scaleVariation) + string.Format("to base {0}", (object) baseScale));
      }

      public DetailVariant(
        int spawnWeight,
        Percent baseScale,
        Percent positionRandomness,
        Percent scaleVariation,
        Vector2i uvCoord,
        int uvSplitCount)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this = new DetailLayerSpecProto.DetailVariant(spawnWeight, baseScale.ToFloat(), positionRandomness.ToFloat(), scaleVariation.ToFloat(), (float) uvCoord.X / (float) uvSplitCount, (float) uvCoord.Y / (float) uvSplitCount, 1f / (float) uvSplitCount);
      }

      public static uint PackUvOriginAndSize(float originX, float originY, float scale)
      {
        return (uint) ((int) (uint) ((double) originX * 128.0) | (int) (uint) ((double) originY * 128.0) << 8 | (int) (uint) ((double) scale * 128.0) << 16);
      }
    }
  }
}
