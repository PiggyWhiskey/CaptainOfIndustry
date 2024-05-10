// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Loans.AcceptLoanCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World.Loans
{
  [GenerateSerializer(false, null, 0)]
  public class AcceptLoanCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ProductProto.ID ProductId;
    public readonly Quantity ToBorrow;
    public readonly int YearsTotal;

    public static void Serialize(AcceptLoanCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AcceptLoanCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AcceptLoanCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductProto.ID.Serialize(this.ProductId, writer);
      Quantity.Serialize(this.ToBorrow, writer);
      writer.WriteInt(this.YearsTotal);
    }

    public static AcceptLoanCmd Deserialize(BlobReader reader)
    {
      AcceptLoanCmd acceptLoanCmd;
      if (reader.TryStartClassDeserialization<AcceptLoanCmd>(out acceptLoanCmd))
        reader.EnqueueDataDeserialization((object) acceptLoanCmd, AcceptLoanCmd.s_deserializeDataDelayedAction);
      return acceptLoanCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AcceptLoanCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<AcceptLoanCmd>(this, "ToBorrow", (object) Quantity.Deserialize(reader));
      reader.SetField<AcceptLoanCmd>(this, "YearsTotal", (object) reader.ReadInt());
    }

    public AcceptLoanCmd(ProductProto.ID product, Quantity toBorrow, int yearsTotal)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProductId = product;
      this.ToBorrow = toBorrow;
      this.YearsTotal = yearsTotal;
    }

    static AcceptLoanCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AcceptLoanCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AcceptLoanCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
