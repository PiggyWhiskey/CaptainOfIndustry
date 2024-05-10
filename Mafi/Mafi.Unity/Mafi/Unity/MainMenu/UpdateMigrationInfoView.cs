// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.UpdateMigrationInfoView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.IO;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  internal class UpdateMigrationInfoView : WindowView
  {
    private readonly Action m_onClose;
    private readonly string m_oldSavePath;
    private readonly string m_newSavePath;
    private readonly Option<string> m_newBlueprintsPath;
    private bool m_doNotShowAgain;

    private static UpdateMigrationInfoView.MigrationState getRegistryState(
      IFileSystemHelper fileSystemHelper)
    {
      if (!Directory.Exists(fileSystemHelper.GameDataRootDirPathLegacy))
        return UpdateMigrationInfoView.MigrationState.MigratedAndAcceptedOrNotNeed;
      string path = Path.Combine(fileSystemHelper.GameDataRootDirPathLegacy, fileSystemHelper.GetDirName(FileType.GameSave));
      return !Directory.Exists(path) || Directory.GetFiles(path, "*" + fileSystemHelper.GetExtension(FileType.GameSave)).Length == 0 ? UpdateMigrationInfoView.MigrationState.MigratedAndAcceptedOrNotNeed : (UpdateMigrationInfoView.MigrationState) PlayerPrefs.GetInt("MIGRATION_DONE", 0);
    }

    private static void setRegistryState(UpdateMigrationInfoView.MigrationState state)
    {
      PlayerPrefs.SetInt("MIGRATION_DONE", (int) state);
      PlayerPrefs.Save();
    }

    /// <summary>
    /// Return view to show if migration was done otherwise None.
    /// NOTE: This is no longer in use, delete after Update 2
    /// </summary>
    public static Option<UpdateMigrationInfoView> TryPerformMigration(
      UiBuilder builder,
      IFileSystemHelper fsHelper,
      Action onClose)
    {
      UpdateMigrationInfoView.MigrationState registryState = UpdateMigrationInfoView.getRegistryState(fsHelper);
      if (registryState == UpdateMigrationInfoView.MigrationState.MigratedAndAcceptedOrNotNeed)
        return Option<UpdateMigrationInfoView>.None;
      Option<string> newBlueprintsPath = Option<string>.None;
      if (registryState == UpdateMigrationInfoView.MigrationState.None)
      {
        try
        {
          newBlueprintsPath = UpdateMigrationInfoView.performMigration(fsHelper);
          UpdateMigrationInfoView.setRegistryState(UpdateMigrationInfoView.MigrationState.Migrated);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Migration failed");
        }
      }
      UpdateMigrationInfoView migrationInfoView = new UpdateMigrationInfoView(onClose, fsHelper.GameDataRootDirPathLegacy, fsHelper.GameDataRootDirPath, newBlueprintsPath);
      migrationInfoView.BuildUi(builder);
      return (Option<UpdateMigrationInfoView>) migrationInfoView;
    }

    private static Option<string> performMigration(IFileSystemHelper fsHelper)
    {
      string dirPath = fsHelper.GetDirPath(FileType.Blueprints, true);
      string path = dirPath.Replace(fsHelper.GameDataRootDirPath, fsHelper.GameDataRootDirPathLegacy);
      if (!Directory.Exists(path))
        return Option<string>.None;
      int num = 0;
      foreach (string file in Directory.GetFiles(path))
      {
        string str = file.Replace(fsHelper.GameDataRootDirPathLegacy, fsHelper.GameDataRootDirPath);
        if (!File.Exists(str))
        {
          try
          {
            File.Copy(file, str);
            ++num;
          }
          catch (Exception ex)
          {
            Log.Exception(ex, "Failed to migrate file from '" + file + "' to '" + str + "'.");
          }
        }
      }
      return num != 0 ? (Option<string>) dirPath : Option<string>.None;
    }

    private UpdateMigrationInfoView(
      Action onClose,
      string oldSavePath,
      string newSavePath,
      Option<string> newBlueprintsPath)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("UpdateMigrationView");
      this.m_onClose = onClose;
      this.m_oldSavePath = oldSavePath;
      this.m_newSavePath = newSavePath;
      this.m_newBlueprintsPath = newBlueprintsPath;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.ImportantAnnouncementTitle);
      this.SetTitleAsBig();
      Vector2 vector2 = this.ResolveWindowSizeWithScale(new float?(120f));
      float width1 = vector2.x.Min(700f);
      float width2 = width1 - 30f;
      ScrollableContainer scrollableContainer = this.Builder.NewScrollableContainer("ScrollView", (IUiElement) this.GetContentPanel()).AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this.GetContentPanel());
      StackContainer container = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).SetInnerPadding(Offset.All(15f));
      scrollableContainer.AddItemTop((IUiElement) container);
      Txt objectToPlace1 = this.Builder.NewTxt("Text").SetTextStyle(this.Builder.Style.Global.TextBig).SetText(Tr.SaveMigration__Intro.Format("Update 1", SaveVersion.BRANCH_MAP.First.SteamBranchName).ToString() + "\n\n" + Tr.SaveMigration__BlueprintsNote.ToString());
      objectToPlace1.AppendTo<Txt>(container, new float?(objectToPlace1.GetPreferedHeight(width2)));
      this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/MainMenu/SteamVersionGuide.png").AppendTo<IconContainer>(container, new float?(width2 / 2f), Offset.Top(5f));
      Txt objectToPlace2 = this.Builder.NewTxt("Text").SetTextStyle(this.Builder.Style.Global.TextBig).SetText((LocStrFormatted) Tr.Update1__LocationChange);
      objectToPlace2.AppendTo<Txt>(container, new float?(objectToPlace2.GetPreferedHeight(width2)), Offset.Top(10f));
      addFolderView(this.m_oldSavePath, (LocStrFormatted) Tr.Update1__OldSaveLocation);
      addFolderView(this.m_newSavePath, (LocStrFormatted) Tr.Update1__NewSaveLocation);
      if (this.m_newBlueprintsPath.HasValue)
      {
        Txt objectToPlace3 = this.Builder.NewTxt("Text").SetTextStyle(this.Builder.Style.Global.TextBig).SetText((LocStrFormatted) Tr.Update1__BlueprintsCopied);
        objectToPlace3.AppendTo<Txt>(container, new float?(objectToPlace3.GetPreferedHeight(width2)), Offset.Top(10f));
        addFolderView(this.m_newBlueprintsPath.Value, (LocStrFormatted) Tr.Update1__NewBlueprintsLocation);
      }
      Txt objectToPlace4 = this.Builder.NewTxt("Text").SetTextStyle(this.Builder.Style.Global.TextBig).SetText((LocStrFormatted) Tr.Update1__OldLocationStillExists);
      objectToPlace4.AppendTo<Txt>(container, new float?(objectToPlace4.GetPreferedHeight(width2)), Offset.Top(10f));
      Panel panel = this.Builder.NewPanel("BtnsPanel").AppendTo<Panel>(container, new Vector2?(new Vector2(0.0f, 30f)), ContainerPosition.MiddleOrCenter, Offset.Top(15f));
      this.m_doNotShowAgain = false;
      SwitchBtn objectToPlace5 = this.Builder.NewSwitchBtn((IUiElement) container).SetText((LocStrFormatted) Tr.DoNotShowAgain).SetOnToggleAction((Action<bool>) (isOn => this.m_doNotShowAgain = isOn));
      objectToPlace5.PutToLeftOf<SwitchBtn>((IUiElement) panel, objectToPlace5.GetWidth());
      Btn btn = this.Builder.NewBtnPrimary("Btn").OnClick(new Action(this.onAcceptClick)).SetText((LocStrFormatted) Tr.Close);
      btn.PutToRightOf<Btn>((IUiElement) panel, btn.GetOptimalWidth() + 20f);
      panel.SetWidth<Panel>((float) ((double) objectToPlace5.GetWidth() + (double) btn.GetWidth() + 40.0));
      float height = vector2.y.Min(container.GetDynamicHeight());
      this.SetContentSize(width1, height);
      this.PositionSelfToCenter(true);
      this.Show();

      void addFolderView(string path, LocStrFormatted prefix)
      {
        Panel parent = this.Builder.NewPanel("FolderPanel").AppendTo<Panel>(container, new float?(30f));
        Btn btn = this.Builder.NewBtnGeneral("show").SetIcon("Assets/Unity/UserInterface/General/OpenInFolder.svg", new Offset?(Offset.All(3f))).AddToolTip(Tr.ShowInExplorer).OnClick((Action) (() => Application.OpenURL("file://" + path)));
        btn.PutToRightMiddleOf<Btn>((IUiElement) parent, new Vector2(36f, 25f));
        Txt txt = this.Builder.NewTxt("Text").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(this.Builder.Style.Global.TextMedium).SetText(prefix);
        txt.PutToLeftOf<Txt>((IUiElement) parent, txt.GetPreferedWidth(), Offset.Right(btn.GetWidth()));
        this.Builder.NewTxtField("SaveDir", (IUiElement) this.GetContentPanel()).SetStyle(this.Builder.Style.Global.LightTxtFieldStyle).SetText(path).PutTo<TxtField>((IUiElement) parent, Offset.Left(txt.GetWidth() + 10f) + Offset.Right(btn.GetWidth() + 10f)).SetReadonlyReally();
      }
    }

    private void onAcceptClick()
    {
      if (this.m_doNotShowAgain)
        UpdateMigrationInfoView.setRegistryState(UpdateMigrationInfoView.MigrationState.MigratedAndAcceptedOrNotNeed);
      this.m_onClose();
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      if (!Input.GetKeyDown(KeyCode.Escape))
        return;
      this.m_onClose();
    }

    public enum MigrationState
    {
      None,
      Migrated,
      MigratedAndAcceptedOrNotNeed,
    }
  }
}
