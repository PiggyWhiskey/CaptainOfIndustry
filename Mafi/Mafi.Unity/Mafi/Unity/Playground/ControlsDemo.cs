// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Playground.ControlsDemo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.InputControl.GameMenu.Settings;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Playground
{
  public class ControlsDemo
  {
    public readonly UiComponent Content;
    private readonly LocStrFormatted TooltipStr;

    public ControlsDemo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.TooltipStr = "I'm a generic tooltip".AsLoc();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ScrollColumn scrollColumn = new ScrollColumn();
      Row row1 = new Row();
      row1.Add<Row>((Action<Row>) (c => c.Fill<Row>().MarginTop<Row>(40.px()).JustifyItemsSpaceAround<Row>().AlignItemsStart<Row>().Gap<Row>(new Px?(20.px()))));
      Column column1 = new Column();
      column1.Add<Column>((Action<Column>) (c => c.Gap<Column>(new Px?(3.pt())).AlignItemsStretch<Column>()));
      column1.Add((UiComponent) new Title("Buttons".AsLoc()));
      Row row2 = new Row();
      row2.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(2.pt()))));
      row2.Add((UiComponent) new ButtonPrimary("Primary".AsLoc()));
      row2.Add((UiComponent) new ButtonPrimary("Disabled".AsLoc()).Enabled<ButtonPrimary>(false));
      row2.Add((UiComponent) new ButtonPrimary("Flipped".AsLoc()).FlipNotches<ButtonPrimary>());
      row2.Add((UiComponent) new ButtonPrimary("Disabled".AsLoc()).FlipNotches<ButtonPrimary>().Enabled<ButtonPrimary>(false));
      row2.Add((UiComponent) new ButtonPrimary("Selected".AsLoc()).Selected<ButtonPrimary>());
      row2.Add((UiComponent) new ButtonPrimary("Toggle".AsLoc()).Class<ButtonPrimary>(Cls.toggle).Selected<ButtonPrimary>());
      column1.Add((UiComponent) row2);
      Row row3 = new Row();
      row3.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(2.pt()))));
      ButtonRow component1 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component1.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component1.Add((UiComponent) new Label("Primary variant".AsLoc()));
      row3.Add((UiComponent) component1.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Primary));
      ButtonRow component2 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component2.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component2.Add((UiComponent) new Label("Disabled".AsLoc()));
      row3.Add((UiComponent) component2.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Primary).Enabled<ButtonRow>(false));
      ButtonRow component3 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component3.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component3.Add((UiComponent) new Label("Flipped".AsLoc()));
      row3.Add((UiComponent) component3.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Primary).FlipNotches<ButtonRow>());
      ButtonRow component4 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component4.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component4.Add((UiComponent) new Label("Disabled".AsLoc()));
      row3.Add((UiComponent) component4.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Primary).Enabled<ButtonRow>(false).FlipNotches<ButtonRow>());
      ButtonRow component5 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component5.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component5.Add((UiComponent) new Label("Selected".AsLoc()));
      row3.Add((UiComponent) component5.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Primary).Selected<ButtonRow>());
      ButtonRow component6 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component6.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component6.Add((UiComponent) new Label("Toggle".AsLoc()));
      row3.Add((UiComponent) component6.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Primary).Class<ButtonRow>(Cls.toggle).Selected<ButtonRow>());
      column1.Add((UiComponent) row3);
      Row row4 = new Row();
      row4.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(2.pt()))));
      row4.Add((UiComponent) new ButtonBold("Bold".AsLoc()));
      row4.Add((UiComponent) new ButtonBold("Disabled".AsLoc()).Enabled<ButtonBold>(false));
      row4.Add((UiComponent) new ButtonBold("Flipped".AsLoc()).FlipNotches<ButtonBold>());
      row4.Add((UiComponent) new ButtonBold("Disabled".AsLoc()).FlipNotches<ButtonBold>().Enabled<ButtonBold>(false));
      row4.Add((UiComponent) new ButtonBold("Selected".AsLoc()).Selected<ButtonBold>());
      row4.Add((UiComponent) new ButtonBold("Toggle".AsLoc()).Class<ButtonBold>(Cls.toggle).Selected<ButtonBold>());
      column1.Add((UiComponent) row4);
      Row row5 = new Row();
      row5.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(2.pt()))));
      row5.Add((UiComponent) new ButtonText("Text".AsLoc()));
      row5.Add((UiComponent) new ButtonText("Disabled".AsLoc()).Enabled<ButtonText>(false));
      row5.Add((UiComponent) new ButtonText("Flipped".AsLoc()).FlipNotches<ButtonText>());
      row5.Add((UiComponent) new ButtonText("Disabled".AsLoc()).FlipNotches<ButtonText>().Enabled<ButtonText>(false));
      row5.Add((UiComponent) new ButtonText("Selected".AsLoc()).Selected<ButtonText>());
      row5.Add((UiComponent) new ButtonText("Toggle".AsLoc()).Class<ButtonText>(Cls.toggle).Selected<ButtonText>());
      column1.Add((UiComponent) row5);
      Row row6 = new Row();
      row6.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(2.pt()))));
      ButtonRow component7 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component7.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component7.Add((UiComponent) new Label("Default variant".AsLoc()));
      row6.Add((UiComponent) component7.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Default));
      ButtonRow component8 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component8.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component8.Add((UiComponent) new Label("Disabled".AsLoc()));
      row6.Add((UiComponent) component8.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Default).Enabled<ButtonRow>(false));
      ButtonRow component9 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component9.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component9.Add((UiComponent) new Label("Flipped".AsLoc()));
      row6.Add((UiComponent) component9.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Default).FlipNotches<ButtonRow>());
      ButtonRow component10 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component10.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component10.Add((UiComponent) new Label("Disabled".AsLoc()));
      row6.Add((UiComponent) component10.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Default).Enabled<ButtonRow>(false).FlipNotches<ButtonRow>());
      ButtonRow component11 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component11.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component11.Add((UiComponent) new Label("Selected".AsLoc()));
      row6.Add((UiComponent) component11.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Default).Selected<ButtonRow>());
      ButtonRow component12 = new ButtonRow(outer: Outer.ShadowCutCorner);
      component12.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component12.Add((UiComponent) new Label("Toggle".AsLoc()));
      row6.Add((UiComponent) component12.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Default).Class<ButtonRow>(Cls.toggle).Selected<ButtonRow>());
      column1.Add((UiComponent) row6);
      Row row7 = new Row();
      row7.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(2.pt())).JustifyItemsStart<Row>()));
      ButtonRow component13 = new ButtonRow(outer: Outer.ShadowAll);
      component13.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component13.Add((UiComponent) new Label("Boxy variant".AsLoc()));
      row7.Add((UiComponent) component13.Gap<ButtonRow>(new Px?(2.pt())).Padding<ButtonRow>(1.pt(), 2.pt()).Variant<ButtonRow>(ButtonVariant.Boxy));
      ButtonRow component14 = new ButtonRow(outer: Outer.ShadowAll);
      component14.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component14.Add((UiComponent) new Label("Disabled".AsLoc()));
      row7.Add((UiComponent) component14.Gap<ButtonRow>(new Px?(2.pt())).Padding<ButtonRow>(1.pt(), 2.pt()).Variant<ButtonRow>(ButtonVariant.Boxy).Enabled<ButtonRow>(false));
      ButtonRow component15 = new ButtonRow(outer: Outer.ShadowAll);
      component15.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component15.Add((UiComponent) new Label("Selected".AsLoc()));
      row7.Add((UiComponent) component15.Gap<ButtonRow>(new Px?(2.pt())).Padding<ButtonRow>(1.pt(), 2.pt()).Variant<ButtonRow>(ButtonVariant.Boxy).Selected<ButtonRow>());
      ButtonRow component16 = new ButtonRow(outer: Outer.ShadowAll);
      component16.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component16.Add((UiComponent) new Label("Toggle".AsLoc()));
      row7.Add((UiComponent) component16.Gap<ButtonRow>(new Px?(2.pt())).Padding<ButtonRow>(1.pt(), 2.pt()).Variant<ButtonRow>(ButtonVariant.Boxy).Toggle<ButtonRow>());
      ButtonRow component17 = new ButtonRow(outer: Outer.ShadowAll);
      component17.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component17.Add((UiComponent) new Label("Selected".AsLoc()));
      row7.Add((UiComponent) component17.Gap<ButtonRow>(new Px?(2.pt())).Padding<ButtonRow>(1.pt(), 2.pt()).Variant<ButtonRow>(ButtonVariant.Boxy).Toggle<ButtonRow>().Selected<ButtonRow>());
      column1.Add((UiComponent) row7);
      Row row8 = new Row();
      row8.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(2.pt()))));
      ButtonRow component18 = new ButtonRow();
      component18.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component18.Add((UiComponent) new Label("Area button".AsLoc()));
      row8.Add((UiComponent) component18.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Area));
      ButtonRow component19 = new ButtonRow();
      component19.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component19.Add((UiComponent) new Label("Area button selected".AsLoc()));
      row8.Add((UiComponent) component19.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Area).Selected<ButtonRow>());
      ButtonRow component20 = new ButtonRow();
      component20.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component20.Add((UiComponent) new Label("Area button toggle".AsLoc()));
      row8.Add((UiComponent) component20.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Area).Toggle<ButtonRow>().Selected<ButtonRow>());
      column1.Add((UiComponent) row8);
      Row row9 = new Row();
      row9.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(2.pt()))));
      row9.Add((UiComponent) new IconButton("Assets/Unity/UserInterface/General/Pin.svg").Tooltip<IconButton>(new LocStrFormatted?("Icon button".AsLoc())));
      row9.Add((UiComponent) new IconButton("Assets/Unity/UserInterface/General/Pin.svg").Enabled<IconButton>(false).Tooltip<IconButton>(new LocStrFormatted?("Icon button disabled".AsLoc())));
      row9.Add((UiComponent) new IconButton("Assets/Unity/UserInterface/General/Pin.svg").Selected<IconButton>().Tooltip<IconButton>(new LocStrFormatted?("Icon button selected".AsLoc())).MarginRight<IconButton>(3.pt()));
      row9.Add((UiComponent) new IconToggleButton("Assets/Unity/UserInterface/General/Pin.svg").Tooltip<IconToggleButton>(new LocStrFormatted?("Icon toggle button".AsLoc())));
      row9.Add((UiComponent) new IconToggleButton("Assets/Unity/UserInterface/General/Pin.svg").Enabled<IconToggleButton>(false).Tooltip<IconToggleButton>(new LocStrFormatted?("Icon toggle button disabled".AsLoc())));
      row9.Add((UiComponent) new IconToggleButton("Assets/Unity/UserInterface/General/Pin.svg").Selected<IconToggleButton>().Tooltip<IconToggleButton>(new LocStrFormatted?("Icon toggle button selected".AsLoc())).MarginRight<IconToggleButton>(3.pt()));
      row9.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Pin.svg").Tooltip<IconClickable>(new LocStrFormatted?("Clickable icon".AsLoc())));
      row9.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Pin.svg").Enabled<IconClickable>(false).Tooltip<IconClickable>(new LocStrFormatted?("Clickable icon disabled".AsLoc())));
      row9.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Pin.svg").Selected<IconClickable>().Tooltip<IconClickable>(new LocStrFormatted?("Clickable icon selected".AsLoc())));
      row9.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Pin.svg").Toggle<IconClickable>().Selected<IconClickable>().Tooltip<IconClickable>(new LocStrFormatted?("Clickable icon toggle selected".AsLoc())).MarginRight<IconClickable>(3.pt()));
      row9.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Pin.svg").Tooltip<IconClickable>(new LocStrFormatted?("Clickable icon growOnHover".AsLoc())).GrowOnHover());
      row9.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Pin.svg").GrowOnHover().Enabled<IconClickable>(false).Tooltip<IconClickable>(new LocStrFormatted?("Clickable icon growOnHover disabled ".AsLoc())));
      row9.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Pin.svg").GrowOnHover().Selected<IconClickable>().Tooltip<IconClickable>(new LocStrFormatted?("Clickable icon growOnHover selected".AsLoc())));
      row9.Add((UiComponent) new IconClickable("Assets/Unity/UserInterface/General/Pin.svg").GrowOnHover().Toggle<IconClickable>().Selected<IconClickable>().Tooltip<IconClickable>(new LocStrFormatted?("Clickable icon growOnHover selected toggle".AsLoc())));
      column1.Add((UiComponent) row9);
      column1.Add((UiComponent) new Title("List group".AsLoc()).MarginTop<Title>(5.pt()));
      Column column2 = new Column();
      column2.Add<Column>((Action<Column>) (c => c.Class<Column>(Cls.listGroup).AlignItemsStretch<Column>()));
      ButtonRow component21 = new ButtonRow();
      component21.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component21.Add((UiComponent) new Label("Item 1".AsLoc()));
      column2.Add((UiComponent) component21.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Area).Selected<ButtonRow>());
      ButtonRow component22 = new ButtonRow();
      component22.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component22.Add((UiComponent) new Label("Item 2".AsLoc()));
      column2.Add((UiComponent) component22.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Area));
      ButtonRow component23 = new ButtonRow();
      component23.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Pin.svg").Small());
      component23.Add((UiComponent) new Label("Item 3".AsLoc()));
      column2.Add((UiComponent) component23.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Area));
      column1.Add((UiComponent) column2);
      column1.Add((UiComponent) new Title("Text fields".AsLoc()).MarginTop<Title>(5.pt()));
      Row row10 = new Row();
      row10.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(20.px()))));
      row10.Add((UiComponent) this.createTextFieldSamples().Background<Column>(new ColorRgba?(Theme.OverlayColor)));
      row10.Add((UiComponent) this.createTextFieldSamples());
      column1.Add((UiComponent) row10);
      column1.Add((UiComponent) this.createEditorColumn());
      row1.Add((UiComponent) column1);
      Column column3 = new Column();
      column3.Add<Column>((Action<Column>) (c => c.AlignItemsStretch<Column>()));
      TabContainer tabContainer1 = new TabContainer();
      tabContainer1.Add<TabContainer>((Action<TabContainer>) (c => c.Height<TabContainer>(new Px?(300.px())).AlignSelf<TabContainer>(Align.Stretch)));
      TabContainer tabContainer2 = tabContainer1;
      column3.Add((UiComponent) tabContainer1);
      column3.Add((UiComponent) new Title("Toggles (standalone)".AsLoc()).MarginTop<Title>(5.pt()));
      Row row11 = new Row();
      row11.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(20.px()))));
      row11.Add((UiComponent) this.createTogglesSamples(true).Background<Column>(new ColorRgba?(Theme.OverlayColor)));
      row11.Add((UiComponent) this.createTogglesSamples(true));
      column3.Add((UiComponent) row11);
      column3.Add((UiComponent) new Title("Toggles".AsLoc()).MarginTop<Title>(5.pt()));
      Row row12 = new Row();
      row12.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(20.px()))));
      row12.Add((UiComponent) this.createTogglesSamples(false).Background<Column>(new ColorRgba?(Theme.OverlayColor)));
      row12.Add((UiComponent) this.createTogglesSamples(false));
      column3.Add((UiComponent) row12);
      column3.Add((UiComponent) new Title("Dropdowns".AsLoc()).MarginTop<Title>(5.pt()));
      Row row13 = new Row();
      row13.Add<Row>((Action<Row>) (c => c.Gap<Row>(new Px?(20.px()))));
      row13.Add((UiComponent) this.createDropdownsSamples().Background<Column>(new ColorRgba?(Theme.OverlayColor)));
      row13.Add((UiComponent) this.createDropdownsSamples());
      column3.Add((UiComponent) row13);
      row1.Add((UiComponent) column3);
      scrollColumn.Add((UiComponent) row1);
      this.Content = (UiComponent) scrollColumn;
      tabContainer2.AddTab("Audio.cs".AsLoc(), (UiComponent) new Column().Background<Column>(new ColorRgba?(ColorRgba.Red.SetA((byte) 5))).Fill<Column>().Height<Column>(new Px?(200.px())).Margin<Column>(1.pt()), "Assets/Unity/UserInterface/General/Audio.svg");
      tabContainer2.AddTab("Audio2.cs".AsLoc(), (UiComponent) new Row().Background<Row>(new ColorRgba?(ColorRgba.Green.SetA((byte) 5))).Fill<Row>().Height<Row>(new Px?(400.px())).Margin<Row>(1.pt()), "Assets/Unity/UserInterface/General/Video.svg");
      tabContainer2.AddTab("Audio3.cs".AsLoc(), (UiComponent) new Row().Background<Row>(new ColorRgba?(ColorRgba.Blue.SetA((byte) 5))).Fill<Row>().Height<Row>(new Px?(800.px())).Margin<Row>(1.pt()), "Assets/Unity/UserInterface/General/Audio.svg");
    }

    private Column createTextFieldSamples()
    {
      Column component = new Column();
      component.Add<Column>((Action<Column>) (c => c.Gap<Column>(new Px?(2.pt())).Padding<Column>(20.px()).Width<Column>(new Px?(240.px())).AlignItemsStretch<Column>()));
      component.Add((UiComponent) new TxtField().Text("I'm here".AsLoc()).Label<TxtField>("Yep:".AsLoc()).Tooltip<TxtField>(new LocStrFormatted?("I select all on focus".AsLoc())).SelectAllOnFocus());
      component.Add((UiComponent) new TxtField().Placeholder("Placeholder ...".AsLoc()));
      component.Add((UiComponent) new TxtField().Text("I'm readonly".AsLoc()).Readonly(true));
      component.Add((UiComponent) new TxtField().Text("I'm disabled".AsLoc()).Label<TxtField>("Yep:".AsLoc()).Enabled<TxtField>(false));
      component.Add((UiComponent) new TxtField().Placeholder("I grow as you type!".AsLoc()).Multiline(doNotScroll: true).Label<TxtField>("Yep:".AsLoc()));
      component.Add((UiComponent) new TxtField().Text("I can scroll!".AsLoc()).Multiline().SetTextAreaHeight(new Px?(60.px())).Label<TxtField>("Yep:".AsLoc()));
      component.Add((UiComponent) new KeyBindingField().Text<KeyBindingField>("Ctrl + F".AsLoc()));
      return component;
    }

    private Column createTogglesSamples(bool standalone)
    {
      Column component = new Column();
      component.Add<Column>((Action<Column>) (c => c.Gap<Column>(new Px?(2.pt())).Padding<Column>(20.px()).Width<Column>(new Px?(240.px())).AlignItemsStretch<Column>()));
      component.Add((UiComponent) new Toggle(standalone).Label<Toggle>("I'm here".AsLoc()).Tooltip<Toggle>(new LocStrFormatted?(this.TooltipStr)));
      component.Add((UiComponent) new Toggle(standalone).Label<Toggle>("I'm checked".AsLoc()).Value(true));
      component.Add((UiComponent) new Toggle(standalone).Label<Toggle>("I'm disabled".AsLoc()).Tooltip<Toggle>(new LocStrFormatted?(this.TooltipStr)).Enabled<Toggle>(false));
      component.Add((UiComponent) new Toggle(standalone).Label<Toggle>("I'm disabled & checked".AsLoc()).Tooltip<Toggle>(new LocStrFormatted?(this.TooltipStr)).Value(true).Enabled<Toggle>(false));
      component.Add((UiComponent) new Toggle(standalone).Label<Toggle>("I'm long and need to wrap".AsLoc()).Tooltip<Toggle>(new LocStrFormatted?(this.TooltipStr)));
      return component;
    }

    private Column createDropdownsSamples()
    {
      Column component = new Column();
      component.Add<Column>((Action<Column>) (c => c.Gap<Column>(new Px?(2.pt())).Padding<Column>(20.px()).Width<Column>(new Px?(240.px())).AlignItemsStretch<Column>()));
      component.Add((UiComponent) this.populateDropdown(new Dropdown<LocStrFormatted>().Label<Dropdown<LocStrFormatted>>("Label".AsLoc()).Tooltip<Dropdown<LocStrFormatted>>(new LocStrFormatted?(this.TooltipStr)).OnValueChanged((Action<LocStrFormatted, int>) ((_, i) => Log.Error(string.Format("Changed {0}", (object) i))))));
      component.Add((UiComponent) this.populateDropdown(new Dropdown<LocStrFormatted>(), true));
      component.Add((UiComponent) this.populateDropdown(new Dropdown<LocStrFormatted>().Label<Dropdown<LocStrFormatted>>("Label".AsLoc()).Enabled<Dropdown<LocStrFormatted>>(false), true));
      return component;
    }

    private Column createEditorColumn()
    {
      Column column = new Column().MarginTop<Column>(20.px()).Padding<Column>(10.px()).Class<Column>(Cls.editor).AlignItemsStretch<Column>().Width<Column>(new Px?(450.px())).Gap<Column>(new Px?(1.pt())).Background<Column>(new ColorRgba?(Theme.OverlayColor));
      column.Add((UiComponent) new Toggle().Label<Toggle>("Compact editor".AsLoc()).Tooltip<Toggle>(new LocStrFormatted?("Make this editor wrap its content".AsLoc())).OnValueChanged((Action<bool>) (isOn => column.Width<Column>(isOn ? new Px?() : new Px?(450.px())))), (UiComponent) new TxtField().Text("I'm here".AsLoc()).Label<TxtField>("Yep:".AsLoc()).Tooltip<TxtField>(new LocStrFormatted?(this.TooltipStr)), (UiComponent) new TxtField().Text("I failed validation".AsLoc()).Label<TxtField>("This will not pass".AsLoc()).MarkAsError(true, this.TooltipStr).Tooltip<TxtField>(new LocStrFormatted?(this.TooltipStr)), (UiComponent) new TxtField().Text("I can scroll".AsLoc()).Label<TxtField>("Label on top:".AsLoc()).Multiline(labelOnTop: true).SetTextAreaHeight(new Px?(60.px())).Tooltip<TxtField>(new LocStrFormatted?(this.TooltipStr)), (UiComponent) this.populateDropdown(new Dropdown<LocStrFormatted>().Label<Dropdown<LocStrFormatted>>("Hello".AsLoc()).Tooltip<Dropdown<LocStrFormatted>>(new LocStrFormatted?(this.TooltipStr)), true));
      return column;
    }

    private Dropdown<LocStrFormatted> populateDropdown(Dropdown<LocStrFormatted> d, bool longList = false)
    {
      d.AddOption("Option A".AsLoc());
      d.AddOption("Option B".AsLoc());
      if (!longList)
        return d;
      d.AddOption("Option C".AsLoc());
      d.AddOption("Option D".AsLoc());
      d.AddOption("Option E".AsLoc());
      d.AddOption("Option F".AsLoc());
      d.AddOption("Option G".AsLoc());
      d.SetOptionViewFactory((Func<LocStrFormatted, int, bool, UiComponent>) ((str, index, isInDropdown) => (UiComponent) new Label(str).Tooltip<Label>(new LocStrFormatted?(this.TooltipStr), index % 2 == 0)));
      return d;
    }
  }
}
