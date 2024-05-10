// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.AutoSaveNotificationUi
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.SaveGame;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class AutoSaveNotificationUi : IUnityUi
  {
    private readonly SaveManager m_saveManager;
    private readonly StatusBar m_statusBar;
    private Panel m_saveMessageContainer;
    private UiBuilder m_builder;
    private AutoSaveNotificationUi.SaveFailDialog m_saveFailDialog;

    public AutoSaveNotificationUi(SaveManager saveManager, StatusBar statusBar)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_saveManager = saveManager;
      this.m_statusBar = statusBar;
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_builder = builder;
      UiStyle style = builder.Style;
      this.m_saveFailDialog = new AutoSaveNotificationUi.SaveFailDialog(builder);
      this.m_saveMessageContainer = builder.NewPanel("Saving").SetBackground(builder.Style.Global.PanelsBg).Hide<Panel>();
      builder.NewPanel("Shadow").SetBackground(builder.Style.Icons.BorderGradient, new ColorRgba?(builder.Style.Global.PanelsBg)).PutTo<Panel>((IUiElement) this.m_saveMessageContainer, Offset.All(-6f));
      Panel centerOf = builder.NewPanel("CenteredHolder").PutToCenterOf<Panel>((IUiElement) this.m_saveMessageContainer, 0.0f);
      Txt txt = builder.NewTxt("Paused").SetText((LocStrFormatted) Tr.SaveInProgress).SetAlignment(TextAnchor.MiddleRight).SetTextStyle(builder.Style.StatusBar.PausedTextStyle).PutTo<Txt>((IUiElement) centerOf);
      centerOf.SetWidth<Panel>(txt.GetPreferedWidth() + 5f);
      placeSelfTo((IUiElement) builder.MainCanvas);
      this.m_saveManager.OnSaveStart += (Action) (() =>
      {
        if (this.m_builder.GameOverlaySuper.IsVisible())
          placeSelfTo((IUiElement) this.m_builder.GameOverlaySuper);
        else if (this.m_builder.GameOverlay.IsVisible())
          placeSelfTo((IUiElement) this.m_builder.GameOverlay);
        else
          placeSelfTo((IUiElement) this.m_builder.MainCanvas);
        this.m_saveMessageContainer.Show<Panel>();
      });
      this.m_saveManager.OnSaveDone += (Action) (() => this.m_saveMessageContainer.Hide<Panel>());
      this.m_saveManager.OnAutoSaveFail += (Action<LocStrFormatted>) (error => this.m_saveFailDialog.SetErrorAndShow(error));

      void placeSelfTo(IUiElement parent)
      {
        this.m_saveMessageContainer.PutToCenterTopOf<Panel>(parent, new Vector2(160f, 36f), Offset.Top((float) ((double) this.m_statusBar.GetHeight() + 50.0 + 15.0)));
      }
    }

    internal class SaveFailDialog : DialogView
    {
      public SaveFailDialog(UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder);
        this.AppendBtnGeneral((LocStrFormatted) Tr.Dismiss).OnClick((Action) (() => this.Hide()));
        this.HighlightAsDanger();
      }

      public void SetErrorAndShow(LocStrFormatted error)
      {
        this.SetMessage(error);
        this.Show();
      }

      public bool InputUpdate()
      {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          this.Hide();
          return true;
        }
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
          return false;
        this.Hide();
        return true;
      }
    }
  }
}
