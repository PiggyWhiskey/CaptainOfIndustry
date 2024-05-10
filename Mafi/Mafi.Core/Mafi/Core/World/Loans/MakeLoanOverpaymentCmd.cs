// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.Loans.MakeLoanOverpaymentCmd
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
  public class MakeLoanOverpaymentCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ProductProto.ID ProductId;
    public readonly uint LoanId;
    public readonly Quantity ToPay;

    public static void Serialize(MakeLoanOverpaymentCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MakeLoanOverpaymentCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MakeLoanOverpaymentCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteUInt(this.LoanId);
      ProductProto.ID.Serialize(this.ProductId, writer);
      Quantity.Serialize(this.ToPay, writer);
    }

    public static MakeLoanOverpaymentCmd Deserialize(BlobReader reader)
    {
      MakeLoanOverpaymentCmd loanOverpaymentCmd;
      if (reader.TryStartClassDeserialization<MakeLoanOverpaymentCmd>(out loanOverpaymentCmd))
        reader.EnqueueDataDeserialization((object) loanOverpaymentCmd, MakeLoanOverpaymentCmd.s_deserializeDataDelayedAction);
      return loanOverpaymentCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MakeLoanOverpaymentCmd>(this, "LoanId", (object) reader.ReadUInt());
      reader.SetField<MakeLoanOverpaymentCmd>(this, "ProductId", (object) ProductProto.ID.Deserialize(reader));
      reader.SetField<MakeLoanOverpaymentCmd>(this, "ToPay", (object) Quantity.Deserialize(reader));
    }

    public MakeLoanOverpaymentCmd(ProductProto.ID product, uint loanId, Quantity toPay)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProductId = product;
      this.LoanId = loanId;
      this.ToPay = toPay;
    }

    static MakeLoanOverpaymentCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MakeLoanOverpaymentCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      MakeLoanOverpaymentCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
