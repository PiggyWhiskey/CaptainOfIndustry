// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.QuitConfirmDialog
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu
{
  public class QuitConfirmDialog : Mafi.Unity.UiToolkit.Library.Window
  {
    public QuitConfirmDialog(QuitConfirmFlavor flavor, Action onConfirm)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((LocStrFormatted) (flavor == QuitConfirmFlavor.ExitToMainMenu ? Tr.ExitToMainMenu : Tr.QuitGame), darkMask: true);
      this.Size<QuitConfirmDialog>(350.px(), Px.Auto);
      LocStr locStr;
      switch (flavor)
      {
        case QuitConfirmFlavor.ExitToMainMenu:
          locStr = Tr.ExitToMainMenu;
          break;
        case QuitConfirmFlavor.QuitGame:
          locStr = Tr.QuitGame;
          break;
        default:
          locStr = Tr.Continue;
          break;
      }
      LocStrFormatted text = (LocStrFormatted) locStr;
      Column body = this.Body;
      Panel panel = new Panel();
      panel.Add<Panel>((Action<Panel>) (c => c.Padding<Panel>(5.pt())));
      panel.Add((UiComponent) new Label((LocStrFormatted) Tr.QuitGame__ConfirmationQuestion).AlignTextCenter<Label>());
      Button confirmButton;
      panel.Add((UiComponent) (confirmButton = (Button) new ButtonPrimary(text, onConfirm).MarginTop<ButtonPrimary>(5.pt()).MarginLeftRight<ButtonPrimary>(Px.Auto)));
      body.Add((UiComponent) panel);
      this.OnFirstRender<QuitConfirmDialog>((Action) (() => confirmButton.Focus()));
    }
  }
}
