// Decompiled with JetBrains decompiler
// Type: Mafi.Base.VirtualProductAttribute
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Localization.Quantity;
using Mafi.Core.Maintenance;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Reflection;

#nullable disable
namespace Mafi.Base
{
  [AttributeUsage(AttributeTargets.Field)]
  public sealed class VirtualProductAttribute : ProductAttribute
  {
    private readonly QuantityFormatter m_formatter;
    private readonly string m_icon;
    private readonly string m_name;
    private readonly bool m_doNotNormalize;
    private readonly bool m_isExcludedFromStats;
    private readonly string m_translationComment;
    private readonly Upoints? m_isMaintenanceRepairCostUpoints;

    public VirtualProductAttribute(
      Type formatter,
      string icon,
      string name = null,
      bool doNotNormalize = false,
      bool isExcludedFromStats = true,
      float isMaintenanceRepairCostUpoints = 0.0f,
      string translationComment = null)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_formatter = (QuantityFormatter) formatter.GetField("Instance")?.GetValue((object) null);
      this.m_icon = icon;
      this.m_name = name;
      this.m_doNotNormalize = doNotNormalize;
      this.m_isExcludedFromStats = isExcludedFromStats;
      this.m_translationComment = translationComment;
      this.m_isMaintenanceRepairCostUpoints = (double) isMaintenanceRepairCostUpoints <= 0.0 ? new Upoints?() : new Upoints?(isMaintenanceRepairCostUpoints.Upoints());
    }

    public override Proto Register(ProtoRegistrator registrator, FieldInfo idFieldInfo)
    {
      ProductProto.ID id1 = (ProductProto.ID) idFieldInfo.GetValue((object) null);
      string name = this.m_name ?? idFieldInfo.Name.CamelCaseToSpacedSentenceCase();
      Assert.That<QuantityFormatter>(this.m_formatter).IsNotNull<QuantityFormatter, ProductProto.ID>("No field `Instance` found on specified for {0}", id1);
      Proto.Str str = !(id1.Value == "Product_Virtual_Upoints") ? Proto.CreateStr((Proto.ID) id1, name, translationComment: this.m_translationComment ?? "virtual product") : new Proto.Str(LocalizationManager.LoadLocalizedString0("Product_Virtual_Upoints__name"));
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto.ID id2 = id1;
      Proto.Str strings = str;
      QuantityFormatter formatter = this.m_formatter;
      bool doNotNormalize = this.m_doNotNormalize;
      bool excludedFromStats = this.m_isExcludedFromStats;
      ProductProto.Gfx graphics = new ProductProto.Gfx(Option<string>.None, (Option<string>) this.m_icon);
      int num1 = doNotNormalize ? 1 : 0;
      int num2 = excludedFromStats ? 1 : 0;
      QuantityFormatter quantityFormatter = formatter;
      VirtualProductProto proto = new VirtualProductProto(id2, strings, graphics, num1 != 0, num2 != 0, quantityFormatter);
      VirtualProductProto virtualProductProto = prototypesDb.Add<VirtualProductProto>(proto);
      if (this.m_isMaintenanceRepairCostUpoints.HasValue)
        virtualProductProto.AddParam((IProtoParam) new MaintenanceProtoParam(this.m_isMaintenanceRepairCostUpoints.Value));
      return (Proto) virtualProductProto;
    }
  }
}
