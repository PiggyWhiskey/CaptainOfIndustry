// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Localization.Quantity.QuantityFormatter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;

#nullable disable
namespace Mafi.Core.Localization.Quantity
{
  public abstract class QuantityFormatter
  {
    public abstract FormatInfo GetFormatInfo(IProtoWithIconAndName proto, QuantityLarge quantity);

    public LocStrFormatted FormatNumberOnly(IProtoWithIconAndName proto, QuantityLarge quantity)
    {
      return this.FormatNumberOnly(quantity, this.GetFormatInfo(proto, quantity));
    }

    public LocStrFormatted FormatNumberOnly(QuantityLarge quantity, FormatInfo formatInfo)
    {
      return new LocStrFormatted(QuantityFormatter.FormatNumber(quantity.Value.ToFix64() / formatInfo.Denominator));
    }

    public LocStrFormatted FormatNumberAndUnitOnly(
      IProtoWithIconAndName proto,
      QuantityLarge quantity)
    {
      return this.FormatNumberAndUnitOnly(quantity, this.GetFormatInfo(proto, quantity));
    }

    public LocStrFormatted FormatNumberAndUnitOnly(QuantityLarge quantity, FormatInfo formatInfo)
    {
      string str = QuantityFormatter.FormatNumber(quantity.Value.ToFix64() / formatInfo.Denominator);
      return !formatInfo.UnitStr.HasValue ? new LocStrFormatted(str) : formatInfo.UnitStr.Value.Format(str);
    }

    public LocStrFormatted FormatNumberAndUnitOnly(
      QuantityLarge quantity,
      FormatInfo formatInfo,
      int numberFontSize)
    {
      string str = QuantityFormatter.FormatNumber(quantity.Value.ToFix64() / formatInfo.Denominator);
      return !formatInfo.UnitStr.HasValue ? new LocStrFormatted(str) : formatInfo.UnitStr.Value.Format(string.Format("<size={0}>{1}</size>", (object) numberFontSize, (object) str));
    }

    public LocStrFormatted FormatNumberWithNamePlural(ProductProto proto, QuantityLarge quantity)
    {
      return this.FormatNumberWithNamePlural(quantity, this.GetFormatInfo((IProtoWithIconAndName) proto, quantity));
    }

    public LocStrFormatted FormatNumberWithNamePlural(QuantityLarge quantity, FormatInfo formatInfo)
    {
      Fix64 fix64 = quantity.Value.ToFix64() / formatInfo.Denominator;
      string str = QuantityFormatter.FormatNumber(fix64);
      return formatInfo.ProductPluralStr.Format(str, fix64);
    }

    public static string FormatNumber(Fix64 number)
    {
      int decimalDigits;
      if (!(number >= 100))
      {
        Fix64 fix64 = number.FractionalPart;
        if (!fix64.IsZero)
        {
          if (!(number >= 10))
          {
            fix64 = number * 10;
            fix64 = fix64.FractionalPart;
            if (!fix64.IsZero)
            {
              decimalDigits = 2;
              goto label_7;
            }
          }
          decimalDigits = 1;
          goto label_7;
        }
      }
      decimalDigits = 0;
label_7:
      return number.ToStringRounded(decimalDigits);
    }

    protected QuantityFormatter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
