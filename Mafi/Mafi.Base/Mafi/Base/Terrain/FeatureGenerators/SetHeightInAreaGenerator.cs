// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FeatureGenerators.SetHeightInAreaGenerator
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain.FeatureGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class SetHeightInAreaGenerator : ITerrainFeatureGenerator, ITerrainFeatureBase
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(SetHeightInAreaGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetHeightInAreaGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetHeightInAreaGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      RectangleTerrainArea2i.Serialize(this.BoundingBox, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      writer.WriteString(this.Name);
      writer.WriteInt((int) this.SetStrategy);
      HeightTilesF.Serialize(this.TargetHeight, writer);
    }

    public static SetHeightInAreaGenerator Deserialize(BlobReader reader)
    {
      SetHeightInAreaGenerator heightInAreaGenerator;
      if (reader.TryStartClassDeserialization<SetHeightInAreaGenerator>(out heightInAreaGenerator))
        reader.EnqueueDataDeserialization((object) heightInAreaGenerator, SetHeightInAreaGenerator.s_deserializeDataDelayedAction);
      return heightInAreaGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.BoundingBox = RectangleTerrainArea2i.Deserialize(reader);
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.Name = reader.ReadString();
      this.SetStrategy = (SetHeightInAreaGenerator.HeightSetStrategy) reader.ReadInt();
      this.TargetHeight = HeightTilesF.Deserialize(reader);
    }

    public string Name { get; set; }

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => true;

    public int SortingPriority => 2000 - this.TargetHeight.Value.ToIntFloored();

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public RectangleTerrainArea2i BoundingBox { get; set; }

    public HeightTilesF TargetHeight { get; set; }

    public SetHeightInAreaGenerator.HeightSetStrategy SetStrategy { get; set; }

    public RectangleTerrainArea2i? GetBoundingBox()
    {
      return new RectangleTerrainArea2i?(this.BoundingBox);
    }

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      Assert.That<int>(this.BoundingBox.AreaTiles).IsPositive("Empty generator area");
      return this.BoundingBox.OverlapsWith(generatedArea.Area2i);
    }

    public TerrainFeatureResourceInfo? GetResourceInfo() => new TerrainFeatureResourceInfo?();

    public void GenerateChunkThreadSafe(TerrainGeneratorChunkData data)
    {
      RectangleTerrainArea2i rectangleTerrainArea2i = data.Area.Intersect(this.BoundingBox);
      HeightTilesF[] heights = data.Heights;
      if (rectangleTerrainArea2i == data.Area)
      {
        for (int index = 0; index < heights.Length; ++index)
          heights[index] = this.TargetHeight;
      }
      else
      {
        RelTile2i relTile2i = rectangleTerrainArea2i.Origin - data.Area.Origin;
        int num1 = 0;
        int num2 = relTile2i.X + relTile2i.Y * 64;
        while (num1 < rectangleTerrainArea2i.Size.Y)
        {
          for (int index = 0; index < rectangleTerrainArea2i.Size.X; ++index)
            heights[num2 + index] = this.TargetHeight;
          ++num1;
          num2 += 64;
        }
      }
    }

    public void Reset()
    {
    }

    public void ClearCaches()
    {
    }

    public void TranslateBy(RelTile3f delta)
    {
      this.BoundingBox = this.BoundingBox.OffsetBy(delta.Xy.RoundedRelTile2i);
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
    }

    public SetHeightInAreaGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: reference to a compiler-generated field
      this.\u003CName\u003Ek__BackingField = "Flat terrain";
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static SetHeightInAreaGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      SetHeightInAreaGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SetHeightInAreaGenerator) obj).SerializeData(writer));
      SetHeightInAreaGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SetHeightInAreaGenerator) obj).DeserializeData(reader));
    }

    public enum HeightSetStrategy
    {
      AlwaysSetHeight,
      SetWhenLower,
      SetWhenHigher,
    }
  }
}
