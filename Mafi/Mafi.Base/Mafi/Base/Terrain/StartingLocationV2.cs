// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.StartingLocationV2
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Map;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public class StartingLocationV2 : 
    IEditableTerrainFeature,
    ITerrainFeatureBase,
    IStartingLocationV2,
    ITerrainFeatureWithSimUpdate
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly StartingLocationV2.Configuration ConfigMutable;
    [DoNotSave(0, null)]
    public bool ValidationPerformed;
    [DoNotSave(0, null)]
    private RelTile2f m_unaccountedTranslation;
    [DoNotSave(0, null)]
    private Lyst<Pair<Tile3i, Option<string>>> m_validationResultsTmp;

    public static void Serialize(StartingLocationV2 value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StartingLocationV2>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StartingLocationV2.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      StartingLocationV2.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      writer.WriteString(this.Name);
    }

    public static StartingLocationV2 Deserialize(BlobReader reader)
    {
      StartingLocationV2 startingLocationV2;
      if (reader.TryStartClassDeserialization<StartingLocationV2>(out startingLocationV2))
        reader.EnqueueDataDeserialization((object) startingLocationV2, StartingLocationV2.s_deserializeDataDelayedAction);
      return startingLocationV2;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<StartingLocationV2>(this, "ConfigMutable", (object) StartingLocationV2.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.Name = reader.ReadString();
    }

    public string Name { get; set; }

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => true;

    public bool Is2D => true;

    public bool CanRotate => false;

    public int Order => this.ConfigMutable.Order;

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public StartingLocationV2(StartingLocationV2.Configuration initialConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: reference to a compiler-generated field
      this.\u003CName\u003Ek__BackingField = "Starting Location";
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = initialConfig;
    }

    public HandleData? GetHandleData()
    {
      return new HandleData?(new HandleData(this.ConfigMutable.Position.Xy.CenterTile2f, this.ConfigMutable.IsValid ? ColorRgba.DarkGreen : ColorRgba.Red, (Option<string>) "Assets/Unity/UserInterface/Toolbar/WorldMap.svg"));
    }

    RectangleTerrainArea2i? ITerrainFeatureBase.GetBoundingBox()
    {
      return new RectangleTerrainArea2i?(new RectangleTerrainArea2i(this.ConfigMutable.Position.Tile2i, RelTile2i.Zero));
    }

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors)
    {
      if (this.ConfigMutable.IsValid)
        return true;
      errors.Add(this.ConfigMutable.ValidationError ?? "Invalid starting location");
      return false;
    }

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      return true;
    }

    public void Reset() => this.ValidationPerformed = false;

    public void ClearCaches()
    {
    }

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      this.m_unaccountedTranslation += delta.Xy;
      RelTile2i roundedRelTile2i = this.m_unaccountedTranslation.RoundedRelTile2i;
      this.ConfigMutable.Position += roundedRelTile2i.ExtendZ(0);
      this.m_unaccountedTranslation -= roundedRelTile2i;
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
    }

    public bool SimUpdate(IResolver resolver)
    {
      if (this.ValidationPerformed)
        return false;
      this.ValidationPerformed = true;
      string error;
      Option<Lyst<string>> results;
      if (this.isValid(resolver, out error, out results))
        this.ConfigMutable.SetValid(true, "", Option<Lyst<string>>.None);
      else
        this.ConfigMutable.SetValid(false, error, results);
      return true;
    }

    private bool isValid(IResolver resolver, out string error, out Option<Lyst<string>> results)
    {
      Tile2i xy = this.ConfigMutable.Position.Xy;
      TerrainManager terrainManager = resolver.Resolve<TerrainManager>();
      if (!terrainManager.IsValidCoord(xy))
      {
        error = "Invalid position";
        results = Option<Lyst<string>>.None;
        return false;
      }
      if (!terrainManager.IsFlat(terrainManager.GetTileIndex(xy)))
      {
        error = "Tile is not flat";
        results = Option<Lyst<string>>.None;
        return false;
      }
      StartingFactoryPlacer startingFactoryPlacer = resolver.Resolve<StartingFactoryPlacer>();
      if (this.m_validationResultsTmp == null)
        this.m_validationResultsTmp = new Lyst<Pair<Tile3i, Option<string>>>(true);
      this.m_validationResultsTmp.Clear();
      bool plan = startingFactoryPlacer.TryCreatePlan(xy, this.ConfigMutable.Direction.ToRotation().Rotated180, out StartingFactoryPlan _, out error, (Option<Lyst<Pair<Tile3i, Option<string>>>>) this.m_validationResultsTmp);
      results = (Option<Lyst<string>>) this.m_validationResultsTmp.SelectValues<string, Pair<Tile3i, Option<string>>>((Func<Pair<Tile3i, Option<string>>, Option<string>>) (x => x.Second)).ToLyst<string>();
      results.Value.Reverse();
      return plan;
    }

    public StartingLocation ToV1()
    {
      return new StartingLocation(this.ConfigMutable.Position.Xy, this.ConfigMutable.Direction);
    }

    public StartingLocationPreview ToPreview()
    {
      return new StartingLocationPreview(this.ConfigMutable.Position, this.ConfigMutable.Direction, this.ConfigMutable.Description, this.ConfigMutable.Difficulty, new int?(this.ConfigMutable.StartingLocationArea));
    }

    static StartingLocationV2()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      StartingLocationV2.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StartingLocationV2) obj).SerializeData(writer));
      StartingLocationV2.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StartingLocationV2) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(StartingLocationV2.Configuration value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<StartingLocationV2.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, StartingLocationV2.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Option<string>.Serialize(this.Description, writer);
        writer.WriteInt((int) this.Difficulty);
        Direction90.Serialize(this.Direction, writer);
        writer.WriteInt(this.Order);
        Tile3i.Serialize(this.Position, writer);
        writer.WriteInt(this.StartingLocationArea);
      }

      public static StartingLocationV2.Configuration Deserialize(BlobReader reader)
      {
        StartingLocationV2.Configuration configuration;
        if (reader.TryStartClassDeserialization<StartingLocationV2.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, StartingLocationV2.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.Description = Option<string>.Deserialize(reader);
        this.Difficulty = (StartingLocationDifficulty) reader.ReadInt();
        this.Direction = Direction90.Deserialize(reader);
        this.Order = reader.LoadedSaveVersion >= 159 ? reader.ReadInt() : (int) this.Difficulty;
        this.Position = Tile3i.Deserialize(reader);
        this.StartingLocationArea = reader.ReadInt();
      }

      [EditorEnforceOrder(27)]
      public Tile3i Position { get; set; }

      [EditorEnforceOrder(33)]
      [EditorLabel(null, "Set direction pointing from land to the ocean. Coastline should be straight and perpendicular to this direction. Note that +X is denoted by red gizmo arrows, +Y has blue arrows.", false, false)]
      public Direction90 Direction { get; set; }

      [EditorEnforceOrder(37)]
      [EditorLabel("Difficulty of starting playthrough at this location.", null, false, false)]
      public StartingLocationDifficulty Difficulty { get; set; }

      [EditorEnforceOrder(41)]
      [EditorTextArea(4, true)]
      public Option<string> Description { get; set; }

      [EditorEnforceOrder(45)]
      [NewInSaveVersion(159, null, "(int)Difficulty", null, null)]
      public int Order { get; set; }

      [EditorLabel(null, "Size of flat area around this starting location. This gets recomputed on map publish.", false, false)]
      [EditorEnforceOrder(49)]
      [EditorReadonly]
      public int StartingLocationArea { get; set; }

      [DoNotSave(0, null)]
      [EditorIgnore]
      public bool IsValid { get; private set; }

      [DoNotSave(0, null)]
      [EditorEnforceOrder(59)]
      [EditorReadonly]
      public string ValidationStatus { get; private set; }

      [EditorEnforceOrder(64)]
      [EditorLabel(null, null, false, true)]
      [DoNotSave(0, null)]
      public string ValidationError { get; private set; }

      [DoNotSave(0, null)]
      [EditorEnforceOrder(69)]
      [EditorLabel(null, "Shows validation results from all placement attempts.", false, false)]
      public Lyst<string> PlacementAttemptResults { get; private set; }

      public void SetValid(bool isValid, string message, Option<Lyst<string>> results)
      {
        this.IsValid = isValid;
        this.ValidationStatus = isValid ? "Starting location is valid! Ships Ahoy!" : "Starting location is invalid, check placement and make sure that the direction is configured correctly.";
        this.ValidationError = message;
        this.PlacementAttemptResults = results.ValueOrNull;
      }

      public void SetStartingLocationArea(int startingLocationArea)
      {
        this.StartingLocationArea = startingLocationArea;
      }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        StartingLocationV2.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StartingLocationV2.Configuration) obj).SerializeData(writer));
        StartingLocationV2.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StartingLocationV2.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
