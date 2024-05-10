// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.ScriptedAiPlayerConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Ai.Scripted
{
  [GenerateSerializer(false, null, 0)]
  public class ScriptedAiPlayerConfig : IConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ImmutableArray<IScriptedAiPlayerAction> Actions { get; set; }

    public Tile2i FirstBuildingPosition { get; set; }

    public Tile2i FirstBuildingPositionAlt { get; set; }

    public Tile2i VehicleDepotPosition { get; set; }

    public RelTile1i BuildingsGridSpacing { get; set; }

    /// <summary>
    /// When specified, insta-build cheat will be enabled until specified stage to quickly skip previous stages.
    /// Default value 0 performs no skipping.
    /// </summary>
    public int StartAtStage { get; set; }

    /// <summary>
    /// When specified and positive, the player will terminate executing actions after specified stage was finished.
    /// Default value 0 (or any negative value) performs no termination.
    /// </summary>
    public int TerminateAfterStage { get; set; }

    public static void Serialize(ScriptedAiPlayerConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ScriptedAiPlayerConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ScriptedAiPlayerConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<IScriptedAiPlayerAction>.Serialize(this.Actions, writer);
      RelTile1i.Serialize(this.BuildingsGridSpacing, writer);
      Tile2i.Serialize(this.FirstBuildingPosition, writer);
      Tile2i.Serialize(this.FirstBuildingPositionAlt, writer);
      writer.WriteInt(this.StartAtStage);
      writer.WriteInt(this.TerminateAfterStage);
      Tile2i.Serialize(this.VehicleDepotPosition, writer);
    }

    public static ScriptedAiPlayerConfig Deserialize(BlobReader reader)
    {
      ScriptedAiPlayerConfig scriptedAiPlayerConfig;
      if (reader.TryStartClassDeserialization<ScriptedAiPlayerConfig>(out scriptedAiPlayerConfig))
        reader.EnqueueDataDeserialization((object) scriptedAiPlayerConfig, ScriptedAiPlayerConfig.s_deserializeDataDelayedAction);
      return scriptedAiPlayerConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Actions = ImmutableArray<IScriptedAiPlayerAction>.Deserialize(reader);
      this.BuildingsGridSpacing = RelTile1i.Deserialize(reader);
      this.FirstBuildingPosition = Tile2i.Deserialize(reader);
      this.FirstBuildingPositionAlt = Tile2i.Deserialize(reader);
      this.StartAtStage = reader.ReadInt();
      this.TerminateAfterStage = reader.ReadInt();
      this.VehicleDepotPosition = Tile2i.Deserialize(reader);
    }

    public ScriptedAiPlayerConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ScriptedAiPlayerConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ScriptedAiPlayerConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ScriptedAiPlayerConfig) obj).SerializeData(writer));
      ScriptedAiPlayerConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ScriptedAiPlayerConfig) obj).DeserializeData(reader));
    }
  }
}
