// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FeatureGenerators.VirtualResourceFeatureGenerator
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain.FeatureGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class VirtualResourceFeatureGenerator : 
    IVirtualTerrainResourceGenerator,
    ITerrainFeatureBase,
    IEditableTerrainFeature,
    IEditableTerrainFeatureWithDisplayedRadius
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly VirtualResourceFeatureGenerator.Configuration ConfigMutable;

    public static void Serialize(VirtualResourceFeatureGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VirtualResourceFeatureGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VirtualResourceFeatureGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      VirtualResourceFeatureGenerator.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
    }

    public static VirtualResourceFeatureGenerator Deserialize(BlobReader reader)
    {
      VirtualResourceFeatureGenerator featureGenerator;
      if (reader.TryStartClassDeserialization<VirtualResourceFeatureGenerator>(out featureGenerator))
        reader.EnqueueDataDeserialization((object) featureGenerator, VirtualResourceFeatureGenerator.s_deserializeDataDelayedAction);
      return featureGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VirtualResourceFeatureGenerator>(this, "ConfigMutable", (object) VirtualResourceFeatureGenerator.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
    }

    public string Name
    {
      get
      {
        return string.Format("Virtual resource: {0} of ", (object) this.ConfigMutable.ConfiguredCapacity) + string.Format("{0} at {1}", (object) this.ConfigMutable.VirtualResource.Strings.Name, (object) this.ConfigMutable.Position.Tile2i);
      }
    }

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => true;

    public bool Is2D => true;

    public bool CanRotate => false;

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public RelTile1i Radius => this.ConfigMutable.MaxRadius;

    public VirtualResourceFeatureGenerator(
      VirtualResourceFeatureGenerator.Configuration initialConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = initialConfig;
    }

    public HandleData? GetHandleData()
    {
      return new HandleData?(new HandleData(this.ConfigMutable.Position, ColorRgba.Gray, (Option<string>) this.ConfigMutable.VirtualResource.Product.Graphics.IconPath));
    }

    RectangleTerrainArea2i? ITerrainFeatureBase.GetBoundingBox()
    {
      return new RectangleTerrainArea2i?(new RectangleTerrainArea2i(this.ConfigMutable.Position.Tile2i, RelTile2i.Zero));
    }

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors) => true;

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      return true;
    }

    public void Reset()
    {
    }

    public void ClearCaches()
    {
    }

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      this.ConfigMutable.Position += delta.Xy;
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
    }

    public ImmutableArray<IVirtualTerrainResource> GenerateResources()
    {
      return ImmutableArray.Create<IVirtualTerrainResource>((IVirtualTerrainResource) new SimpleVirtualResource(this.ConfigMutable.VirtualResource, this.ConfigMutable.ConfiguredCapacity, this.ConfigMutable.Position.Tile2iRounded.ExtendZ(0), this.ConfigMutable.MaxRadius));
    }

    public ImmutableArray<ProductQuantity> GetResourceStats()
    {
      return ImmutableArray.Create<ProductQuantity>(new ProductQuantity(this.ConfigMutable.VirtualResource.Product, this.ConfigMutable.ConfiguredCapacity));
    }

    public TerrainFeatureResourceInfo? GetResourceInfo()
    {
      return new TerrainFeatureResourceInfo?(new TerrainFeatureResourceInfo(this.ConfigMutable.Position.Tile2i, this.ConfigMutable.VirtualResource.Product));
    }

    public void MarkPreviewDirty()
    {
    }

    static VirtualResourceFeatureGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      VirtualResourceFeatureGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VirtualResourceFeatureGenerator) obj).SerializeData(writer));
      VirtualResourceFeatureGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VirtualResourceFeatureGenerator) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        VirtualResourceFeatureGenerator.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<VirtualResourceFeatureGenerator.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, VirtualResourceFeatureGenerator.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Quantity.Serialize(this.ConfiguredCapacity, writer);
        RelTile1i.Serialize(this.MaxRadius, writer);
        Tile2f.Serialize(this.Position, writer);
        writer.WriteGeneric<VirtualResourceProductProto>(this.VirtualResource);
      }

      public static VirtualResourceFeatureGenerator.Configuration Deserialize(BlobReader reader)
      {
        VirtualResourceFeatureGenerator.Configuration configuration;
        if (reader.TryStartClassDeserialization<VirtualResourceFeatureGenerator.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, VirtualResourceFeatureGenerator.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.ConfiguredCapacity = Quantity.Deserialize(reader);
        this.MaxRadius = RelTile1i.Deserialize(reader);
        this.Position = Tile2f.Deserialize(reader);
        this.VirtualResource = reader.ReadGenericAs<VirtualResourceProductProto>();
      }

      [EditorEnforceOrder(33)]
      public VirtualResourceProductProto VirtualResource { get; set; }

      [EditorRange(0.0, 1000000.0)]
      [EditorEnforceOrder(37)]
      public Quantity ConfiguredCapacity { get; set; }

      [EditorEnforceOrder(40)]
      public Tile2f Position { get; set; }

      [EditorEnforceOrder(44)]
      [EditorRange(1.0, 1000.0)]
      public RelTile1i MaxRadius { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        VirtualResourceFeatureGenerator.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VirtualResourceFeatureGenerator.Configuration) obj).SerializeData(writer));
        VirtualResourceFeatureGenerator.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VirtualResourceFeatureGenerator.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
