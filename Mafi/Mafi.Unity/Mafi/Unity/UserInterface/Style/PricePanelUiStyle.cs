// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.PricePanelUiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  public class PricePanelUiStyle : BaseUiStyle
  {
    public readonly PricePanelUiStyle.PricePanelStyle MenuPricePanelStyle;
    public readonly PricePanelUiStyle.PricePanelStyle MenuPricePanelSellStyle;
    public readonly PricePanelUiStyle.PricePanelStyle VehiclesBuyPricePanelStyle;
    public readonly PricePanelUiStyle.PricePanelStyle VehiclesSellProfitPanelStyle;
    public readonly PricePanelUiStyle.PricePanelStyle QuickTradeToPayStyle;
    public readonly PricePanelUiStyle.PricePanelStyle QuickTradeToReceiveStyle;

    public PricePanelUiStyle(IconsPaths icons, GlobalUiStyle global)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(icons, global);
      this.MenuPricePanelStyle = new PricePanelUiStyle.PricePanelStyle(this.Global.Title.Extend(new ColorRgba?(this.Global.BlueForDark)), Offset.LeftRight(20f), 14f, 15f);
      this.MenuPricePanelSellStyle = new PricePanelUiStyle.PricePanelStyle(this.Global.Title.Extend(new ColorRgba?(this.Global.GreenForDark)), Offset.LeftRight(20f), 14f, 15f);
      this.VehiclesBuyPricePanelStyle = new PricePanelUiStyle.PricePanelStyle(this.Global.Title.Extend(new ColorRgba?(this.Global.BlueForDark)), Offset.LeftRight(20f), 14f, 5f);
      this.VehiclesSellProfitPanelStyle = new PricePanelUiStyle.PricePanelStyle(this.Global.Title.Extend(new ColorRgba?(this.Global.GreenForDark)), Offset.LeftRight(20f), 14f, 5f);
      this.QuickTradeToPayStyle = new PricePanelUiStyle.PricePanelStyle(this.Global.Title.Extend(new ColorRgba?(this.Global.BlueForDark)), Offset.LeftRight(20f), 14f, 5f);
      this.QuickTradeToReceiveStyle = new PricePanelUiStyle.PricePanelStyle(this.Global.Title.Extend(new ColorRgba?(this.Global.GreenForDark)), Offset.LeftRight(20f), 14f, 5f);
    }

    public class PricePanelStyle
    {
      public readonly TextStyle TextStyle;
      public readonly Offset InnerPadding;
      public readonly float PlusSignSize;
      public readonly float PlusSignMargin;

      public PricePanelStyle(
        TextStyle textStyle,
        Offset innerPadding,
        float plusSignSize,
        float plusSignMargin)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.TextStyle = textStyle;
        this.InnerPadding = innerPadding;
        this.PlusSignSize = plusSignSize;
        this.PlusSignMargin = plusSignMargin;
      }
    }
  }
}
