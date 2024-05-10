// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Buildings.StatueProto
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Buildings
{
  public class StatueProto : 
    LayoutEntityProto,
    IProtoWithUpgrade<StatueProto>,
    IProtoWithUpgrade,
    IProto
  {
    public Option<ProductProto> InputProduct;
    public Duration DurationPerOneQuantity;
    public LocStr ExtraText;

    public Option<Type> Manager => (Option<Type>) typeof (StatueOfMaintenanceManager);

    public override Type EntityType => typeof (Statue);

    public UpgradeData<StatueProto> Upgrade { get; private set; }

    public IUpgradeData UpgradeNonGeneric => (IUpgradeData) this.Upgrade;

    public StatueProto(
      StaticEntityProto.ID id,
      Proto.Str strings,
      EntityLayout layout,
      EntityCosts costs,
      Option<StatueProto> nextTier,
      Option<ProductProto> inputProduct,
      Duration durationPerOneQuantity,
      LocStr extraText,
      LayoutEntityProto.Gfx graphics)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, strings, layout, costs, graphics);
      this.Upgrade = new UpgradeData<StatueProto>(this, nextTier);
      this.InputProduct = inputProduct;
      this.ExtraText = extraText;
      this.DurationPerOneQuantity = durationPerOneQuantity;
    }

    public void SetNextTier(Option<StatueProto> nextTierProto)
    {
      if (this.IsInitialized)
        Log.Error(string.Format("Failed to set next tier of '{0}', protos are already initialized.", (object) this.Id));
      else
        this.Upgrade = new UpgradeData<StatueProto>(this, nextTierProto);
    }
  }
}
