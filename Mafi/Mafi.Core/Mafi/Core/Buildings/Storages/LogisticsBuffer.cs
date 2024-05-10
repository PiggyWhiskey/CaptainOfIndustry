// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.LogisticsBuffer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  /// <summary>
  /// This buffer provides extra features such as import, export sliders.
  /// It is also recognized by vehicle logistics which instead of just
  /// Quantity or Capacity use QuantityForExport / CapacityForExport to
  /// make the sliders work properly. Use this only if you need those
  /// sliders.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class LogisticsBuffer : ProductBuffer, ILogisticsBufferReadOnly, IProductBufferReadOnly
  {
    /// <summary>
    /// Total number of discrete steps for a import/export sliders.
    /// </summary>
    public const int MAX_STEPS = 10;
    private readonly bool m_usePartialTrucksForHighPriorities;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Single step by which import/export percentage can be modified.
    /// </summary>
    public static Percent SingleStep { get; }

    /// <summary>Current percentage(0-100) occupancy of the storage.</summary>
    public int CurrentQuantityPercent => this.Quantity.Value * 100 / this.Capacity.Value;

    /// <summary>
    /// 0-100% of capacity of buffer until which should be imported.
    /// </summary>
    public Percent ImportUntilPercent { get; private set; }

    /// <summary>
    /// 0-100% of capacity of buffer from which should be exported.
    /// </summary>
    public Percent ExportFromPercent { get; private set; }

    /// <summary>
    /// Whether the buffer should try to fetch out all the quantity ASAP so the product can be after that
    /// automatically removed from storage.
    /// </summary>
    public bool CleaningMode { get; private set; }

    public LogisticsBuffer(
      Quantity bufferCapacity,
      ProductProto product,
      bool usePartialTrucksForHighPriorities = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(bufferCapacity, product);
      this.m_usePartialTrucksForHighPriorities = usePartialTrucksForHighPriorities;
      this.ImportUntilPercent = Percent.Zero;
      this.ExportFromPercent = Percent.Hundred;
    }

    /// <summary>
    /// Allows to "hide" its keep full quantity from logistics.
    /// Can return negative quantity to save perf.
    /// </summary>
    public Quantity QuantityForExport()
    {
      return this.Quantity - this.Capacity.ScaledBy(this.ImportUntilPercent);
    }

    /// <summary>
    /// Allows to "hide" its keep empty capacity from logistics.
    /// Can return negative quantity to save perf.
    /// </summary>
    public Quantity UsableCapacityForImport()
    {
      return this.UsableCapacity - this.Capacity.ScaledBy(this.ExportFromPercent.InverseTo100());
    }

    public void SetImportStep(Percent value) => this.SetImportStep(value.Apply(10));

    public void SetImportStep(int value)
    {
      Assert.That<int>(value).IsWithinIncl(0, 10, "Import step expected to be in 0-" + 10.ToString() + " range.");
      if (this.CleaningMode)
        this.CleaningMode = false;
      this.ImportUntilPercent = (value * LogisticsBuffer.SingleStep).Clamp(Percent.Zero, this.ExportFromPercent);
      if (!this.ImportUntilPercent.IsPositive)
        return;
      this.ExportFromPercent = Percent.Hundred;
    }

    public void SetExportStep(Percent value) => this.SetExportStep(value.Apply(10));

    public void SetExportStep(int value)
    {
      Assert.That<int>(value).IsWithinIncl(0, 10, "Export step expected to be in 0-" + 10.ToString() + " range.");
      if (this.CleaningMode)
        this.CleaningMode = false;
      this.ExportFromPercent = (value * LogisticsBuffer.SingleStep).Clamp(this.ImportUntilPercent, Percent.Hundred);
      if (this.ExportFromPercent.IsNearHundred)
        return;
      this.ImportUntilPercent = Percent.Zero;
    }

    public void SetCleaningMode(bool isEnabled)
    {
      if (this.CleaningMode == isEnabled)
        return;
      this.CleaningMode = isEnabled;
      if (this.CleaningMode)
      {
        this.ImportUntilPercent = Percent.Zero;
        this.ExportFromPercent = Percent.Zero;
      }
      else
      {
        this.ImportUntilPercent = Percent.Zero;
        this.ExportFromPercent = Percent.Hundred;
      }
    }

    public BufferStrategy GetInputPriority(int priorityForImport, Quantity pendingQuantity)
    {
      Quantity quantity1 = this.UsableCapacity - pendingQuantity;
      if (quantity1 <= Quantity.Zero || this.CleaningMode)
        return BufferStrategy.Ignore;
      if (this.ExportFromPercent < Percent.Hundred)
        return (this.UsableCapacityForImport() - pendingQuantity).IsPositive ? BufferStrategy.LowestNoQuantityPreference() : BufferStrategy.Ignore;
      Quantity quantity2 = quantity1.Min(this.Capacity.ScaledBy(this.ImportUntilPercent) - this.Quantity - pendingQuantity);
      if (!(quantity2 > Quantity.Zero))
        return BufferStrategy.LowestNoQuantityPreference();
      return this.m_usePartialTrucksForHighPriorities ? new BufferStrategy(priorityForImport, new Quantity?(quantity2)) : BufferStrategy.NoQuantityPreference(priorityForImport);
    }

    public BufferStrategy GetOutputPriority(int priorityForExport, OutputPriorityRequest request)
    {
      Quantity pendingQuantity = request.PendingQuantity;
      Quantity quantity = this.Quantity - pendingQuantity;
      if (quantity <= Quantity.Zero)
        return BufferStrategy.Ignore;
      if (this.CleaningMode)
        return new BufferStrategy(priorityForExport, new Quantity?(quantity));
      return this.ImportUntilPercent > Percent.Zero ? ((this.QuantityForExport() - pendingQuantity).IsPositive || request.IsForRefuelling ? BufferStrategy.LowestNoQuantityPreference() : BufferStrategy.Ignore) : (quantity.Min(this.Quantity - pendingQuantity - this.Capacity.ScaledBy(this.ExportFromPercent)) > Quantity.Zero ? BufferStrategy.NoQuantityPreference(priorityForExport) : BufferStrategy.LowestNoQuantityPreference());
    }

    public static void Serialize(LogisticsBuffer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<LogisticsBuffer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, LogisticsBuffer.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.CleaningMode);
      Percent.Serialize(this.ExportFromPercent, writer);
      Percent.Serialize(this.ImportUntilPercent, writer);
      writer.WriteBool(this.m_usePartialTrucksForHighPriorities);
    }

    public static LogisticsBuffer Deserialize(BlobReader reader)
    {
      LogisticsBuffer logisticsBuffer;
      if (reader.TryStartClassDeserialization<LogisticsBuffer>(out logisticsBuffer))
        reader.EnqueueDataDeserialization((object) logisticsBuffer, LogisticsBuffer.s_deserializeDataDelayedAction);
      return logisticsBuffer;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CleaningMode = reader.ReadBool();
      this.ExportFromPercent = Percent.Deserialize(reader);
      this.ImportUntilPercent = Percent.Deserialize(reader);
      reader.SetField<LogisticsBuffer>(this, "m_usePartialTrucksForHighPriorities", (object) reader.ReadBool());
    }

    static LogisticsBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LogisticsBuffer.SingleStep = Percent.Hundred / 10;
      LogisticsBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
      LogisticsBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
    }
  }
}
