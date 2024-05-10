// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportedProductMutable
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// Very compact representation of transported product.
  /// WARNING: This struct is mutable, always use ref when trying to change it inside of collections.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public struct TransportedProductMutable
  {
    /// <summary>Relative trajectory index.</summary>
    public short TrajectoryIndexRelative;
    /// <summary>
    /// Last seen index, for use in UI. This totally breaks encapsulation but without it would be hard to know
    /// which products moved from where.
    /// </summary>
    [DoNotSave(0, null)]
    public ushort LastSeenIndexAbsoluteForUi;
    public readonly ProductSlimId SlimId;
    internal byte QuantitySlim;
    public bool IsImmediatelyBehindNextProduct;
    /// <summary>
    /// Sequential number, each new product gets incremented number, this is used for randomization in UI.
    /// </summary>
    public readonly byte SeqNumber;

    [DoNotSave(0, null)]
    public Quantity Quantity
    {
      get => new Quantity((int) this.QuantitySlim);
      set => this.QuantitySlim = (byte) value.Value;
    }

    [LoadCtor]
    private TransportedProductMutable(
      short trajectoryIndexRelative,
      ProductSlimId slimId,
      byte quantitySlim,
      bool isImmediatelyBehindNextProduct,
      byte seqNumber)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.TrajectoryIndexRelative = trajectoryIndexRelative;
      this.LastSeenIndexAbsoluteForUi = (ushort) 0;
      this.SlimId = slimId;
      this.QuantitySlim = quantitySlim;
      this.IsImmediatelyBehindNextProduct = isImmediatelyBehindNextProduct;
      this.SeqNumber = seqNumber;
    }

    public TransportedProductMutable(
      ProductSlimId slimId,
      short trajectoryIndexRelative,
      Quantity quantity,
      ushort absIndexForUi,
      byte seqNumber)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.TrajectoryIndexRelative = trajectoryIndexRelative;
      this.SlimId = slimId;
      this.QuantitySlim = (byte) quantity.Value;
      this.LastSeenIndexAbsoluteForUi = absIndexForUi;
      this.IsImmediatelyBehindNextProduct = false;
      this.SeqNumber = seqNumber;
    }

    public static void Serialize(TransportedProductMutable value, BlobWriter writer)
    {
      writer.WriteShort(value.TrajectoryIndexRelative);
      ProductSlimId.Serialize(value.SlimId, writer);
      writer.WriteByte(value.QuantitySlim);
      writer.WriteBool(value.IsImmediatelyBehindNextProduct);
      writer.WriteByte(value.SeqNumber);
    }

    public static TransportedProductMutable Deserialize(BlobReader reader)
    {
      return new TransportedProductMutable(reader.ReadShort(), ProductSlimId.Deserialize(reader), reader.ReadByte(), reader.ReadBool(), reader.ReadByte());
    }
  }
}
