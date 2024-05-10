// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.FluidProductProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Products
{
  public class FluidProductProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public FluidProductProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    /// <summary>
    /// Starts building of a new fluid product (liquid or gas) by resetting the builder.
    /// </summary>
    public FluidProductProtoBuilder.State Start(
      string name,
      ProductProto.ID id,
      string translationComment = null)
    {
      return new FluidProductProtoBuilder.State(this, id, name, translationComment);
    }

    public sealed class State : ProductBuilderState<FluidProductProtoBuilder.State>
    {
      private readonly ProductProto.ID m_protoId;

      internal State(
        FluidProductProtoBuilder builder,
        ProductProto.ID protoId,
        string name,
        string translationComment = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, protoId, name, translationComment ?? "fluid product", true);
        this.m_protoId = protoId;
      }

      public FluidProductProto BuildAndAdd()
      {
        ProductProto.ID protoId = this.m_protoId;
        Proto.Str strings = this.Strings;
        int num1 = this.IsStorable ? 1 : 0;
        int num2 = this.CanBeDiscarded ? 1 : 0;
        int num3 = this.IsWaste ? 1 : 0;
        bool homeScreenByDefault = this.PinToHomeScreenByDefault;
        ProductProto.Gfx graphics = this.Graphics;
        int num4 = homeScreenByDefault ? 1 : 0;
        Quantity? maxQuantityPerTransportedProduct = new Quantity?();
        return this.AddToDb<FluidProductProto>(new FluidProductProto(protoId, strings, num1 != 0, num2 != 0, num3 != 0, graphics, num4 != 0, maxQuantityPerTransportedProduct));
      }
    }
  }
}
