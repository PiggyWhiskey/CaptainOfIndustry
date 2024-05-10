// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.SimpleVirtualResource
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public class SimpleVirtualResource : 
    IVirtualTerrainResource,
    ITerrainResource,
    IVirtualTerrainResourceFriend
  {
    private static readonly ThicknessTilesF MIN_THICKNESS_OVERLAY;
    private Percent m_capacityBonus;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public string Name => this.Product.Id.Value;

    public int Priority => 1;

    public VirtualResourceProductProto Product { get; private set; }

    public Quantity ConfiguredCapacity { get; private set; }

    public Quantity Quantity { get; private set; }

    [NewInSaveVersion(140, null, null, null, null)]
    public PartialQuantity EmergencyQuantity { get; set; }

    public Quantity Capacity { get; private set; }

    public Tile3i Position { get; private set; }

    public RelTile1i MaxRadius { get; private set; }

    public ColorRgba ResourceColor => this.Product.Graphics.ResourcesVizColor;

    public SimpleVirtualResource(
      VirtualResourceProductProto product,
      Quantity configuredCapacity,
      Tile3i position,
      RelTile1i maxRadius)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Product = product;
      this.ConfiguredCapacity = configuredCapacity;
      this.Capacity = configuredCapacity;
      this.Quantity = configuredCapacity;
      this.Position = position;
      this.MaxRadius = maxRadius;
    }

    public void Initialize(IslandMap map, bool isEditorPreview = false)
    {
      this.m_capacityBonus = map.DifficultyConfig.MineableResourceSizeBonus;
    }

    public bool IsAt(Tile2i position)
    {
      return position.DistanceSqrTo(this.Position.Tile2i) <= this.MaxRadius.Squared;
    }

    public void AddAsMuchAs(Quantity quantity)
    {
      Assert.That<Quantity>(quantity).IsNotNegative();
      this.Quantity = this.Capacity.Min(this.Quantity + quantity);
    }

    public ThicknessTilesF GetApproxThicknessAt(Tile2i position)
    {
      return this.IsAt(position) ? this.Product.Graphics.ResourceBarsMaxHeight.ScaledBy(this.PercentFull()).Max(this.Product.IsResourceFinal ? ThicknessTilesF.Zero : SimpleVirtualResource.MIN_THICKNESS_OVERLAY) : ThicknessTilesF.Zero;
    }

    public ProductQuantity MineResourceAt(Tile2i position, Quantity maxQuantity)
    {
      Quantity quantity = maxQuantity.Min(this.Quantity);
      this.Quantity -= quantity;
      return this.Product.Product.WithQuantity(quantity);
    }

    void IVirtualTerrainResourceFriend.InitializeScale(Percent depositSizeMultiplier)
    {
      if (this.Quantity != this.Capacity)
      {
        Log.Error(string.Format("Resource {0} is being rescaled after it was already mined!", (object) this.Product.Id));
      }
      else
      {
        depositSizeMultiplier *= Percent.Hundred + this.m_capacityBonus;
        if (depositSizeMultiplier.IsNotPositive)
        {
          Log.Warning(string.Format("Incorrect multiplier {0} for {1}.", (object) depositSizeMultiplier, (object) this.Product.Id));
          depositSizeMultiplier = Percent.Hundred;
        }
        this.Quantity = this.Capacity = this.ConfiguredCapacity.ScaledBy(depositSizeMultiplier);
      }
    }

    public static void Serialize(SimpleVirtualResource value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SimpleVirtualResource>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SimpleVirtualResource.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Quantity.Serialize(this.Capacity, writer);
      Quantity.Serialize(this.ConfiguredCapacity, writer);
      PartialQuantity.Serialize(this.EmergencyQuantity, writer);
      Percent.Serialize(this.m_capacityBonus, writer);
      RelTile1i.Serialize(this.MaxRadius, writer);
      Tile3i.Serialize(this.Position, writer);
      writer.WriteGeneric<VirtualResourceProductProto>(this.Product);
      Quantity.Serialize(this.Quantity, writer);
    }

    public static SimpleVirtualResource Deserialize(BlobReader reader)
    {
      SimpleVirtualResource simpleVirtualResource;
      if (reader.TryStartClassDeserialization<SimpleVirtualResource>(out simpleVirtualResource))
        reader.EnqueueDataDeserialization((object) simpleVirtualResource, SimpleVirtualResource.s_deserializeDataDelayedAction);
      return simpleVirtualResource;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Capacity = Quantity.Deserialize(reader);
      this.ConfiguredCapacity = Quantity.Deserialize(reader);
      this.EmergencyQuantity = reader.LoadedSaveVersion >= 140 ? PartialQuantity.Deserialize(reader) : new PartialQuantity();
      this.m_capacityBonus = Percent.Deserialize(reader);
      this.MaxRadius = RelTile1i.Deserialize(reader);
      this.Position = Tile3i.Deserialize(reader);
      this.Product = reader.ReadGenericAs<VirtualResourceProductProto>();
      this.Quantity = Quantity.Deserialize(reader);
    }

    static SimpleVirtualResource()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SimpleVirtualResource.MIN_THICKNESS_OVERLAY = 0.25.TilesThick();
      SimpleVirtualResource.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SimpleVirtualResource) obj).SerializeData(writer));
      SimpleVirtualResource.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SimpleVirtualResource) obj).DeserializeData(reader));
    }
  }
}
