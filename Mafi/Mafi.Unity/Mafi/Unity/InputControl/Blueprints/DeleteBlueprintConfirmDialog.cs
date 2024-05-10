// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Blueprints.DeleteBlueprintConfirmDialog
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Blueprints
{
  internal class DeleteBlueprintConfirmDialog : DialogView
  {
    public DeleteBlueprintConfirmDialog(UiBuilder builder, Action onConfirmClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder);
      DeleteBlueprintConfirmDialog blueprintConfirmDialog = this;
      this.AppendBtnDanger((LocStrFormatted) Tr.BlueprintDelete__Action).OnClick((Action) (() =>
      {
        blueprintConfirmDialog.Hide();
        onConfirmClick();
      }));
      this.AppendBtnGeneral((LocStrFormatted) Tr.Cancel).OnClick(new Action(((DialogView) this).Hide));
      this.HighlightAsDanger();
    }

    public void SetNameAndShow(string itemName)
    {
      this.SetMessage(Tr.BlueprintDelete__Confirmation.Format(itemName));
      this.Show();
    }
  }
}
