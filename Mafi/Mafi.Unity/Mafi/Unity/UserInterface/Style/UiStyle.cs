// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Style.UiStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Style
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class UiStyle
  {
    public readonly CursorsStyles Cursors;
    public readonly IconsPaths Icons;
    public readonly GlobalUiStyle Global;
    public readonly CoreUiStyle Core;
    public readonly ToolbarUiStyle Toolbar;
    public readonly StatusBarUiStyle StatusBar;
    public readonly TradeWindowsUiStyle Trade;
    public readonly GeneralPanelUiStyle Panel;
    public readonly QuantityBarUiStyle QuantityBar;
    public readonly BufferViewUiStyle BufferView;
    public readonly ProductWithIconUiStyle ProductWithIcon;
    public readonly RecipeViewUiStyle RecipeDetail;
    public readonly NotificationsUiStyle Notifications;
    public readonly LayersLegendUiStyle Layers;
    public readonly StatisticsPanelUiStyle Statistics;
    public readonly PricePanelUiStyle PricePanel;
    public readonly SaveLoadPanelUiStyle SaveLoad;
    public readonly MainMenuUiStyle MainMenu;
    public readonly ResearchWindowUiStyle Research;
    public readonly EntitiesMenuUiStyle EntitiesMenu;

    public UiStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Cursors = new CursorsStyles();
      this.Icons = new IconsPaths();
      this.Global = new GlobalUiStyle();
      this.Core = new CoreUiStyle(this.Icons, this.Global);
      this.Toolbar = new ToolbarUiStyle(this.Icons, this.Global);
      this.StatusBar = new StatusBarUiStyle(this.Icons, this.Global);
      this.Trade = new TradeWindowsUiStyle(this.Icons, this.Global);
      this.Panel = new GeneralPanelUiStyle(this.Icons, this.Global);
      this.QuantityBar = new QuantityBarUiStyle(this.Icons, this.Global);
      this.BufferView = new BufferViewUiStyle(this.Icons, this.Global);
      this.ProductWithIcon = new ProductWithIconUiStyle(this.Icons, this.Global);
      this.RecipeDetail = new RecipeViewUiStyle();
      this.Notifications = new NotificationsUiStyle(this.Icons, this.Global);
      this.Layers = new LayersLegendUiStyle(this.Icons, this.Global);
      this.Statistics = new StatisticsPanelUiStyle(this.Icons, this.Global);
      this.PricePanel = new PricePanelUiStyle(this.Icons, this.Global);
      this.SaveLoad = new SaveLoadPanelUiStyle(this.Icons, this.Global);
      this.MainMenu = new MainMenuUiStyle(this.Icons, this.Global);
      this.Research = new ResearchWindowUiStyle(this.Icons, this.Global);
      this.EntitiesMenu = new EntitiesMenuUiStyle(this.Icons, this.Global);
    }
  }
}
