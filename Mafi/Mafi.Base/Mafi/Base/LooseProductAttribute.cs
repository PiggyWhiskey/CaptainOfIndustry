// Decompiled with JetBrains decompiler
// Type: Mafi.Base.LooseProductAttribute
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Reflection;

#nullable disable
namespace Mafi.Base
{
  [AttributeUsage(AttributeTargets.Field)]
  public sealed class LooseProductAttribute : ProductAttribute
  {
    private readonly string m_name;
    private readonly string m_materialPath;
    private readonly bool m_useRoughPileMeshes;
    private readonly Option<string> m_customIconPath;
    private readonly bool m_dumpByDefault;
    private readonly bool m_cannotBeStored;
    private readonly ColorRgba m_resourceColor;
    private readonly bool m_pinToHomeScreen;
    private readonly bool m_isWaste;
    private readonly bool m_isRecyclable;
    private readonly bool m_doNotTrackSourceProducts;
    private readonly string m_translationComment;
    private readonly ProductProto.ID? m_sourceProductId;
    private readonly PartialQuantity m_sourceProductQuantity;

    public LooseProductAttribute(
      string material,
      string name = null,
      bool useRoughPileMeshes = false,
      string icon = null,
      bool isWaste = false,
      bool dumpByDefault = false,
      bool cannotBeStored = false,
      int resourceColor = -1,
      bool pinToHomeScreen = false,
      bool isRecyclable = false,
      bool doNotTrackSourceProducts = false,
      string translationComment = null,
      string sourceProductId = null,
      double sourceProductQuantity = 1.0)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_name = name;
      this.m_cannotBeStored = cannotBeStored;
      this.m_resourceColor = resourceColor == -1 ? ColorRgba.Empty : new ColorRgba(resourceColor);
      this.m_materialPath = material;
      this.m_useRoughPileMeshes = useRoughPileMeshes;
      this.m_customIconPath = (Option<string>) icon;
      this.m_isWaste = isWaste;
      this.m_dumpByDefault = dumpByDefault;
      this.m_pinToHomeScreen = pinToHomeScreen;
      this.m_isRecyclable = isRecyclable;
      this.m_doNotTrackSourceProducts = doNotTrackSourceProducts;
      this.m_translationComment = translationComment;
      this.m_sourceProductId = sourceProductId != null ? new ProductProto.ID?(new ProductProto.ID(sourceProductId)) : new ProductProto.ID?();
      this.m_sourceProductQuantity = this.m_sourceProductQuantity = new PartialQuantity(sourceProductQuantity.ToFix32());
    }

    public override Proto Register(ProtoRegistrator registrator, FieldInfo idFieldInfo)
    {
      ProductProto.ID id1 = (ProductProto.ID) idFieldInfo.GetValue((object) null);
      string name = this.m_name ?? idFieldInfo.Name.CamelCaseToSpacedSentenceCase();
      LooseProductProto.Gfx gfx1 = new LooseProductProto.Gfx(this.m_useRoughPileMeshes ? "Assets/Base/Transports/ConveyorLoose/PileRough.prefab" : "Assets/Base/Transports/ConveyorLoose/PileSmooth.prefab", this.m_materialPath, this.m_useRoughPileMeshes, this.m_resourceColor, this.m_customIconPath);
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto.ID id2 = id1;
      Proto.Str str = Proto.CreateStr((Proto.ID) id1, name, translationComment: this.m_translationComment ?? "loose product");
      LooseProductProto.Gfx gfx2 = gfx1;
      bool isWaste = this.m_isWaste;
      int num1 = this.m_dumpByDefault ? 1 : 0;
      int num2 = !this.m_cannotBeStored ? 1 : 0;
      LooseProductProto.Gfx graphics = gfx2;
      int num3 = isWaste ? 1 : 0;
      bool isRecyclable = this.m_isRecyclable;
      bool trackSourceProducts = this.m_doNotTrackSourceProducts;
      int num4 = this.m_pinToHomeScreen ? 1 : 0;
      int num5 = isRecyclable ? 1 : 0;
      int num6 = trackSourceProducts ? 1 : 0;
      ProductProto.ID? sourceProductId = this.m_sourceProductId;
      PartialQuantity? sourceProductQuantity = this.m_sourceProductId.HasValue ? new PartialQuantity?(this.m_sourceProductQuantity) : new PartialQuantity?();
      Quantity? maxQuantityPerTransportedProduct = new Quantity?();
      LooseProductProto proto = new LooseProductProto(id2, str, num1 != 0, num2 != 0, graphics, num3 != 0, num4 != 0, num5 != 0, num6 != 0, sourceProductId, sourceProductQuantity, maxQuantityPerTransportedProduct);
      return (Proto) prototypesDb.Add<LooseProductProto>(proto);
    }
  }
}
