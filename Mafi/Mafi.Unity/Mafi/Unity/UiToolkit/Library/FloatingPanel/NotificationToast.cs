// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.FloatingPanel.NotificationToast
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.MainMenu;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.FloatingPanel
{
  public class NotificationToast : Mafi.Unity.UiToolkit.Library.Row
  {
    private Option<IVisualElementScheduledItem> m_notificationTask;
    private ErrorWindow m_errorWindow;

    public NotificationToast()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(Outer.Panel);
      this.Gap<NotificationToast>(new Px?(2.pt())).WrapClass(Cls.slideIn).WrapClass(Cls.fromTop).MarginTop<UiComponentDecorated<VisualElement>>(10.px()).AlignSelf<UiComponentDecorated<VisualElement>>(Mafi.Unity.UiToolkit.Component.Align.Center).Padding<UiComponentDecorated<VisualElement>>(3.pt(), 4.pt());
      this.m_errorWindow = new ErrorWindow();
      this.m_errorWindow.OnHide((Action) (() => this.m_errorWindow.RemoveFromHierarchy()));
    }

    public void ShowSuccess(LocStrFormatted message, LocStrFormatted details = default (LocStrFormatted))
    {
      this.update(message, details);
      this.WrapClass(Cls.show);
      this.m_notificationTask.ValueOrNull?.Pause();
      this.m_notificationTask = this.Schedule.Execute(new Action(this.Hide)).SomeOption<IVisualElementScheduledItem>();
      this.m_notificationTask.Value.ExecuteLater(3000L);
    }

    public void ShowGeneral(LocStrFormatted message, bool showForever = false)
    {
      this.SetChildren(new UiComponent[1]
      {
        (UiComponent) new Mafi.Unity.UiToolkit.Library.Label(message).FontSize<Mafi.Unity.UiToolkit.Library.Label>(16).Color<Mafi.Unity.UiToolkit.Library.Label>(new ColorRgba?(Theme.Text))
      });
      this.WrapClass(Cls.show);
      this.m_notificationTask.ValueOrNull?.Pause();
      if (showForever)
        return;
      this.m_notificationTask = this.Schedule.Execute(new Action(this.Hide)).SomeOption<IVisualElementScheduledItem>();
      this.m_notificationTask.Value.ExecuteLater(3000L);
    }

    public void ShowError(LocStrFormatted message, LocStrFormatted details = default (LocStrFormatted))
    {
      if (details.IsNotEmpty)
        this.m_errorWindow.AddErrorThreadSafe(message, details);
      this.update(message, details, false);
      this.WrapClass(Cls.show);
      this.m_notificationTask.ValueOrNull?.Pause();
    }

    public void Hide() => this.WrapClass(Cls.show, false);

    private void update(LocStrFormatted message, LocStrFormatted tooltip = default (LocStrFormatted), bool success = true)
    {
      this.SetChildren((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(message).FontSize<Mafi.Unity.UiToolkit.Library.Label>(16).Color<Mafi.Unity.UiToolkit.Library.Label>(new ColorRgba?(success ? Theme.PositiveColor : Theme.NegativeColor)).Tooltip<Mafi.Unity.UiToolkit.Library.Label>(new LocStrFormatted?(tooltip)), success || tooltip.IsEmptyOrNull ? (UiComponent) null : (UiComponent) new ButtonText((LocStrFormatted) Tr.Error__View, new Action(this.showErrorWindow)).MarginLeft<ButtonText>(6.pt()), success ? (UiComponent) null : (UiComponent) new IconButton("Assets/Unity/UserInterface/General/Close.svg", new Action(this.Hide)).Padding<IconButton>(5.px()).PaddingTop<IconButton>(6.px()).Small());
    }

    private void showErrorWindow()
    {
      this.RunWithBuilder((Action<UiBuilder>) (b => b.AddComponent((UiComponent) this.m_errorWindow)));
      this.m_errorWindow.Show<ErrorWindow>();
    }
  }
}
