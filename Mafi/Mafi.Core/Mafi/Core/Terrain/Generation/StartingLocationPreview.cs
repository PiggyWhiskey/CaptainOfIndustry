// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.StartingLocationPreview
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GenerateSerializer(false, null, 0)]
  public sealed class StartingLocationPreview
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly Tile3i Position;
    public readonly Direction90 ShoreDirection;
    public readonly Option<string> Description;
    public readonly StartingLocationDifficulty Difficulty;
    /// <summary>
    /// Number of flat tiles immediately reachable from the starting location.
    /// </summary>
    public readonly int? StartingLocationArea;
    /// <summary>
    /// Additional resources in the shipyard at the start of the game.
    /// </summary>
    public readonly ImmutableArray<ProductQuantity> ExtraStartingResources;

    public static void Serialize(StartingLocationPreview value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StartingLocationPreview>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StartingLocationPreview.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      Option<string>.Serialize(this.Description, writer);
      writer.WriteInt((int) this.Difficulty);
      ImmutableArray<ProductQuantity>.Serialize(this.ExtraStartingResources, writer);
      Tile3i.Serialize(this.Position, writer);
      Direction90.Serialize(this.ShoreDirection, writer);
      writer.WriteNullableStruct<int>(this.StartingLocationArea);
    }

    public static StartingLocationPreview Deserialize(BlobReader reader)
    {
      StartingLocationPreview startingLocationPreview;
      if (reader.TryStartClassDeserialization<StartingLocationPreview>(out startingLocationPreview))
        reader.EnqueueDataDeserialization((object) startingLocationPreview, StartingLocationPreview.s_deserializeDataDelayedAction);
      return startingLocationPreview;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<StartingLocationPreview>(this, "Description", (object) Option<string>.Deserialize(reader));
      reader.SetField<StartingLocationPreview>(this, "Difficulty", (object) (StartingLocationDifficulty) reader.ReadInt());
      reader.SetField<StartingLocationPreview>(this, "ExtraStartingResources", (object) ImmutableArray<ProductQuantity>.Deserialize(reader));
      reader.SetField<StartingLocationPreview>(this, "Position", (object) Tile3i.Deserialize(reader));
      reader.SetField<StartingLocationPreview>(this, "ShoreDirection", (object) Direction90.Deserialize(reader));
      reader.SetField<StartingLocationPreview>(this, "StartingLocationArea", (object) reader.ReadNullableStruct<int>());
    }

    public StartingLocationPreview(
      Tile3i position,
      Direction90 shoreDirection,
      Option<string> description,
      StartingLocationDifficulty difficulty,
      int? startingLocationArea,
      ImmutableArray<ProductQuantity>? extraStartingResources = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Position = position;
      this.ShoreDirection = shoreDirection;
      this.Description = description;
      this.Difficulty = difficulty;
      this.StartingLocationArea = startingLocationArea;
      this.ExtraStartingResources = extraStartingResources.GetValueOrDefault(ImmutableArray<ProductQuantity>.Empty);
    }

    static StartingLocationPreview()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StartingLocationPreview.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StartingLocationPreview) obj).SerializeData(writer));
      StartingLocationPreview.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StartingLocationPreview) obj).DeserializeData(reader));
    }
  }
}
