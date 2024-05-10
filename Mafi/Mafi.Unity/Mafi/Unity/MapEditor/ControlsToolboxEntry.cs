// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.ControlsToolboxEntry
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class ControlsToolboxEntry : Column
  {
    private readonly Func<ShortcutsManager, KeyBindings> m_getBindings;
    private readonly Row m_container;

    public ControlsToolboxEntry(Func<ShortcutsManager, KeyBindings> getBindings, string iconPath)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_getBindings = getBindings;
      this.AlignItemsCenter<ControlsToolboxEntry>();
      this.JustifyItemsEnd<ControlsToolboxEntry>();
      this.Gap<ControlsToolboxEntry>(new Px?(2.pt()));
      Column column1 = new Column(2.pt());
      column1.Add<Column>((Action<Column>) (c => c.AlignItemsCenter<Column>().PaddingBottom<Column>(6.px())));
      Column column2 = new Column(Outer.Panel);
      column2.Add<Column>((Action<Column>) (c =>
      {
        Column component = c;
        Px? nullable = new Px?(-18.px());
        Px? top = new Px?();
        Px? bottom = nullable;
        component.AbsolutePositionCenter<Column>(top, bottom).Size<Column>(40.px(), 60.px());
      }));
      column1.Add((UiComponent) column2);
      Row component1 = new Row(Outer.ShadowAll, gap: new Px?(2.px()));
      component1.Add<Row>((Action<Row>) (c => c.Background<Row>(new ColorRgba?(Theme.PanelBackground)).BorderRadius<Row>(1.pt()).Padding<Row>(1.pt())));
      Row child = component1;
      this.m_container = component1;
      column1.Add((UiComponent) child);
      column1.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Icon(iconPath));
      this.Add((UiComponent) column1);
    }

    public ControlsToolboxEntry(KeyBindings bindings, string iconPath)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector((Func<ShortcutsManager, KeyBindings>) (mgr => bindings), iconPath);
    }

    public ControlsToolboxEntry Update(ShortcutsManager manager)
    {
      this.m_container.SetChildren(this.m_getBindings(manager).Primary.Keys.AsEnumerable().SelectMany<KeyCode, UiComponent>((Func<KeyCode, int, IEnumerable<UiComponent>>) ((k, idx) => (IEnumerable<UiComponent>) new UiComponent[2]
      {
        idx > 0 ? (UiComponent) new Label("+".AsLoc()) : (UiComponent) null,
        this.Key(k)
      })));
      return this;
    }

    private UiComponent Key(KeyCode keyCode)
    {
      UiComponent uiComponent;
      switch (keyCode)
      {
        case KeyCode.Mouse0:
          uiComponent = this.Icon("Assets/Unity/UserInterface/General/MouseLeft.svg");
          break;
        case KeyCode.Mouse1:
          uiComponent = this.Icon("Assets/Unity/UserInterface/General/MouseRight.svg");
          break;
        case (KeyCode) 10077:
          uiComponent = this.Icon("Assets/Unity/UserInterface/General/MouseScroll.svg");
          break;
        default:
          uiComponent = (UiComponent) new Label(keyCode.ToNiceString().AsLoc()).FontBold<Label>().Color<Label>(new ColorRgba?(Theme.PrimaryColor)).Background<Label>(new ColorRgba?(Theme.PanelBackground)).Padding<Label>(1.pt(), 1.pt()).MinWidth<Label>(new Px?(30.px())).AlignTextCenter<Label>().BorderRadius<Label>(1.pt()).Border<Label>(1, 1, 10, 1).BorderColor<Label>(Theme.BorderColorBright);
          break;
      }
      return uiComponent;
    }

    private UiComponent Icon(string path)
    {
      return (UiComponent) new Mafi.Unity.UiToolkit.Library.Icon(path).Height<Mafi.Unity.UiToolkit.Library.Icon>(new Px?(36.px())).Color<Mafi.Unity.UiToolkit.Library.Icon>(new ColorRgba?(Theme.PrimaryColor));
    }
  }
}
