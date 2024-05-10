// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.NewGame.MechanicsTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu.NewGame
{
  public class MechanicsTab : Column
  {
    private readonly NewGameConfigForUi m_settings;

    public MechanicsTab(NewGameConfigForUi settings, Action showDifficultySettings)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      MechanicsTab mechanicsTab = this;
      this.m_settings = settings;
      this.AlignItemsStretch<MechanicsTab>();
      this.Padding<MechanicsTab>(1.pt());
      this.Fill<MechanicsTab>();
      this.Name<MechanicsTab>(nameof (MechanicsTab));
      UpdaterBuilder builder = UpdaterBuilder.Start();
      UiComponent[] uiComponentArray = new UiComponent[2];
      ScrollColumn component1 = new ScrollColumn();
      component1.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.FlexGrow<ScrollColumn>(1f).PaddingBottom<ScrollColumn>(5.pt()).Gap<ScrollColumn>(new Px?(5.pt()))));
      component1.Add(section((LocStrFormatted) GameMechanics.GameMechanic__Casual, LocStrFormatted.Empty, new UiComponent[2]
      {
        mechanic(GameMechanics.Casual),
        mechanic(GameMechanics.ResourcesBoost)
      }));
      component1.Add(section((LocStrFormatted) GameMechanics.GameMechanic__Realism, LocStrFormatted.Empty, new UiComponent[3]
      {
        mechanic(GameMechanics.Realism),
        mechanic(GameMechanics.RealismPlus),
        mechanic(GameMechanics.OreSorting)
      }));
      component1.Add(section((LocStrFormatted) GameMechanics.GameMechanic__Challenges, LocStrFormatted.Empty, new UiComponent[1]
      {
        mechanic(GameMechanics.ReducedWorldMines)
      }));
      uiComponentArray[0] = (UiComponent) component1;
      Column component2 = new Column(Outer.EdgeShadowTop, gap: new Px?(3.pt()));
      component2.Add<Column>((Action<Column>) (c => c.FlexNoShrink<Column>().AlignItemsCenter<Column>().PaddingTop<Column>(3.pt())));
      component2.Add((UiComponent) new Label((LocStrFormatted) Tr.CustomizeDifficulty__Description));
      Outer shadowCutCorner = Outer.ShadowCutCorner;
      ButtonRow component3 = new ButtonRow(showDifficultySettings, shadowCutCorner);
      component3.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Tradable128.png").Medium());
      component3.Add((UiComponent) new Label((LocStrFormatted) Tr.Menu__DifficultySettings));
      component2.Add((UiComponent) component3.Gap<ButtonRow>(new Px?(2.pt())).Variant<ButtonRow>(ButtonVariant.Default).Class<ButtonRow>(Cls.bold));
      uiComponentArray[1] = (UiComponent) component2;
      this.Add(uiComponentArray);
      IUiUpdater updater = builder.Build();
      this.Schedule.Execute((Action) (() =>
      {
        updater.SyncUpdate();
        updater.RenderUpdate();
      })).Every(50L);

      static UiComponent section(
        LocStrFormatted title,
        LocStrFormatted subTitle,
        UiComponent[] children)
      {
        Column component = new Column(2.pt());
        component.Add<Column>((Action<Column>) (c => c.AlignItemsStretch<Column>()));
        component.Add((UiComponent) new Title(title).ExtraText(subTitle, new Px?(3.pt())));
        Row row = new Row(2.pt());
        row.Add<Row>((Action<Row>) (c => c.AlignItemsStretch<Row>()));
        row.Add((IEnumerable<UiComponent>) children);
        component.Add((UiComponent) row);
        return (UiComponent) component;
      }

      UiComponent mechanic(GameMechanicApplier mechanic)
      {
        return setting(mechanic.Title, mechanic.IconPath, mechanic.Items, new Func<bool>(isSelected), new Action(toggleSelected));

        bool isSelected()
        {
          return mechanicsTab.m_settings.DifficultyConfig.SelectedMechanics.Contains<GameMechanicApplier>(mechanic);
        }

        void toggleSelected() => mechanicsTab.m_settings.DifficultyConfig.ToggleMechanic(mechanic);
      }

      UiComponent setting(
        LocStrFormatted title,
        string iconPath,
        ImmutableArray<LocStrFormatted> bullets,
        Func<bool> isSelected,
        Action toggleSelected)
      {
        ButtonColumn component = new ButtonColumn(toggleSelected, inner: Inner.GlowAll);
        component.Add<ButtonColumn>((Action<ButtonColumn>) (c => c.Class<ButtonColumn>(Cls.outlined, Cls.toggle, Cls.glowOnHover).Padding<ButtonColumn>(0.px()).Variant<ButtonColumn>(ButtonVariant.Area).AlignItemsStretch<ButtonColumn>().SelectedObserve<ButtonColumn>(builder, isSelected).OverflowHidden<ButtonColumn>().Width<ButtonColumn>(33.Percent())));
        Row row1 = new Row();
        row1.Add<Row>((Action<Row>) (c => c.Padding<Row>(2.pt(), 3.pt()).Background<Row>(new ColorRgba?(Theme.PanelBackgroundAccent)).Apply<Row>((Action<Row>) (c => builder.Observe<bool>(isSelected).Do((Action<bool>) (selected => c.BorderBottom<Row>(1, new ColorRgba?(selected ? Theme.PrimaryBorder : Theme.BorderColor))))))));
        row1.Add(new UiComponent().Size<UiComponent>(20.px()).Border<UiComponent>(1, new ColorRgba?(Theme.BorderColorBright)).BorderRadius<UiComponent>(4.px()).VisibleObserve<UiComponent>(builder, (Func<bool>) (() => !isSelected())));
        row1.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/Checkmark-v2.svg").Medium().Color<Icon>(new ColorRgba?(Theme.PrimaryColor)).VisibleObserve<Icon>(builder, isSelected));
        row1.Add((UiComponent) new Label(title).FontBold<Label>().MarginLeft<Label>(2.pt()).Apply<Label>((Action<Label>) (c => builder.Observe<bool>(isSelected).Do((Action<bool>) (selected => c.Color<Label>(new ColorRgba?(selected ? Theme.PrimaryColor : Theme.Text)))))));
        component.Add((UiComponent) row1);
        Row row2 = new Row();
        row2.Add<Row>((Action<Row>) (c => c.Fill<Row>()));
        row2.Add((UiComponent) new Img(iconPath).MarginLeftRight<Img>(4.pt()).Size<Img>(60.px()).FlexNoShrink<Img>());
        BulletedList bulletedList = new BulletedList();
        bulletedList.Add<BulletedList>((Action<BulletedList>) (c => c.Padding<BulletedList>(2.pt(), 3.pt(), 2.pt(), 0.pt())));
        bulletedList.Add((IEnumerable<UiComponent>) bullets.Select<Label>((Func<LocStrFormatted, Label>) (str => new Label(str).Class<Label>(Cls.body))));
        row2.Add((UiComponent) bulletedList);
        component.Add((UiComponent) row2);
        return (UiComponent) component;
      }
    }
  }
}
