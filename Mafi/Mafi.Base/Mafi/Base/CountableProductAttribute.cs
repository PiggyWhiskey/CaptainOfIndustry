// Decompiled with JetBrains decompiler
// Type: Mafi.Base.CountableProductAttribute
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
  public sealed class CountableProductAttribute : ProductAttribute
  {
    private readonly string m_prefabPath;
    private readonly string m_name;
    private readonly bool m_pinToHomeScreen;
    private readonly string m_customIconPath;
    private readonly bool m_isWaste;
    private readonly bool m_cannotBeStored;
    private readonly int m_radioactivity;
    private readonly string m_translationComment;
    private readonly ProductProto.ID? m_sourceProductId;
    private readonly PartialQuantity m_sourceProductQuantity;
    private readonly bool m_markNotAvailable;
    private readonly CountableProductStackingMode m_packingMode;
    private readonly bool m_allowPackingNoise;
    private readonly bool m_rotateSecondPackedItem90Degs;
    private readonly bool m_doNotTrackRecyclables;

    public CountableProductAttribute(
      string prefab,
      string name = null,
      string icon = null,
      bool isWaste = false,
      bool cannotBeStored = false,
      bool pinToHomeScreen = false,
      int radioactivity = 0,
      string translationComment = null,
      string sourceProductId = null,
      double sourceProductQuantity = 1.0,
      bool markNotAvailable = false,
      bool doNotTrackRecyclables = false,
      CountableProductStackingMode packingMode = CountableProductStackingMode.Auto,
      bool allowPackingNoise = false,
      bool rotateSecondPackedItem90Degs = false)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_prefabPath = prefab;
      this.m_name = name;
      this.m_customIconPath = icon;
      this.m_isWaste = isWaste;
      this.m_pinToHomeScreen = pinToHomeScreen;
      this.m_cannotBeStored = cannotBeStored;
      this.m_radioactivity = radioactivity;
      this.m_translationComment = translationComment;
      this.m_sourceProductId = sourceProductId != null ? new ProductProto.ID?(new ProductProto.ID(sourceProductId)) : new ProductProto.ID?();
      this.m_sourceProductQuantity = new PartialQuantity(sourceProductQuantity.ToFix32());
      this.m_markNotAvailable = markNotAvailable;
      this.m_doNotTrackRecyclables = doNotTrackRecyclables;
      this.m_packingMode = packingMode;
      this.m_allowPackingNoise = allowPackingNoise;
      this.m_rotateSecondPackedItem90Degs = rotateSecondPackedItem90Degs;
    }

    public override Proto Register(ProtoRegistrator registrator, FieldInfo idFieldInfo)
    {
      ProductProto.ID id1 = (ProductProto.ID) idFieldInfo.GetValue((object) null);
      string name = this.m_name ?? idFieldInfo.Name.CamelCaseToSpacedSentenceCase();
      CountableProductProto.Gfx gfx = new CountableProductProto.Gfx(this.m_prefabPath, this.m_customIconPath.CreateOption<string>(), this.m_packingMode, this.m_allowPackingNoise, this.m_rotateSecondPackedItem90Degs);
      ProtosDb prototypesDb = registrator.PrototypesDb;
      ProductProto.ID id2 = id1;
      Proto.Str str = Proto.CreateStr((Proto.ID) id1, name, translationComment: this.m_translationComment ?? "unit product");
      Quantity maxQuantityPerTransportedProduct = 3.Quantity();
      bool isWaste = this.m_isWaste;
      int num1 = !this.m_cannotBeStored ? 1 : 0;
      int radioactivity1 = this.m_radioactivity;
      bool pinToHomeScreen = this.m_pinToHomeScreen;
      bool trackRecyclables = this.m_doNotTrackRecyclables;
      ProductProto.ID? sourceProductId = this.m_sourceProductId;
      PartialQuantity? nullable = this.m_sourceProductId.HasValue ? new PartialQuantity?(this.m_sourceProductQuantity) : new PartialQuantity?();
      CountableProductProto.Gfx graphics = gfx;
      int radioactivity2 = radioactivity1;
      int num2 = isWaste ? 1 : 0;
      int num3 = pinToHomeScreen ? 1 : 0;
      int num4 = trackRecyclables ? 1 : 0;
      ProductProto.ID? sourceProduct = sourceProductId;
      PartialQuantity? sourceProductQuantity = nullable;
      CountableProductProto proto = new CountableProductProto(id2, str, maxQuantityPerTransportedProduct, num1 != 0, graphics, radioactivity2, num2 != 0, num3 != 0, num4 != 0, sourceProduct, sourceProductQuantity);
      CountableProductProto countableProductProto = prototypesDb.Add<CountableProductProto>(proto);
      if (this.m_markNotAvailable)
        countableProductProto.SetAvailability(false);
      return (Proto) countableProductProto;
    }
  }
}
