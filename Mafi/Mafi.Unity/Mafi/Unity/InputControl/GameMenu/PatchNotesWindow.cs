// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.PatchNotesWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu
{
  public class PatchNotesWindow : Mafi.Unity.UiToolkit.Library.Window
  {
    private readonly ImmutableArray<ChangelogEntry> m_entries;
    private readonly Set<string> m_unseenVersions;
    private readonly Set<string> m_unseenSubVersions;
    private readonly ScrollColumn m_contents;
    private Option<PatchNotesWindow.VersionButton> m_selected;

    public PatchNotesWindow(bool hasNewChanges, Option<string> lastSeenVersion, bool darkMask = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_unseenVersions = new Set<string>();
      this.m_unseenSubVersions = new Set<string>();
      // ISSUE: explicit constructor call
      base.\u002Ector(LocStrFormatted.Empty, darkMask: darkMask);
      PatchNotesWindow patchNotesWindow = this;
      this.m_entries = ChangelogUtils.ParseChangelogEntries().ToImmutableArray<ChangelogEntry>();
      if (lastSeenVersion.HasValue)
      {
        string cleanVersionStr = ChangelogUtils.GetCleanVersionStr(lastSeenVersion.Value, true, true);
        Log.Info(lastSeenVersion.Value + ", " + cleanVersionStr);
        foreach (ChangelogEntry entry in this.m_entries)
        {
          string[] array = entry.SubEntries.TakeWhile<ChangelogSubEntry>((Func<ChangelogSubEntry, bool>) (se => se.SubVersion != lastSeenVersion.Value)).Select<ChangelogSubEntry, string>((Func<ChangelogSubEntry, string>) (se => se.SubVersion)).ToArray<string>();
          if (array.Length != 0)
          {
            this.m_unseenVersions.Add(entry.Version);
            this.m_unseenSubVersions.AddRange((IEnumerable<string>) array);
          }
          if (entry.Version == cleanVersionStr)
            break;
        }
      }
      this.GrowVertically();
      this.Title((LocStrFormatted) (hasNewChanges ? Tr.PatchNotes__New : Tr.PatchNotes));
      int oldestUnseenIndex = 0;
      Column body = this.Body;
      Panel panel = new Panel();
      panel.Add<Panel>((Action<Panel>) (c => c.RootPanel()));
      Row row = new Row(3.pt());
      row.Add<Row>((Action<Row>) (c => c.Fill<Row>().AlignItemsStretch<Row>()));
      ScrollColumn scrollColumn = new ScrollColumn();
      scrollColumn.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.Width<ScrollColumn>(12.Percent()).Class<ScrollColumn>(Cls.listGroup)));
      scrollColumn.Add((IEnumerable<UiComponent>) this.m_entries.Select<PatchNotesWindow.VersionButton>((Func<ChangelogEntry, int, PatchNotesWindow.VersionButton>) ((e, idx) =>
      {
        bool isNew = closure_0.m_unseenVersions.Contains(e.Version);
        if (isNew)
          oldestUnseenIndex = idx;
        return new PatchNotesWindow.VersionButton(e, new Action<PatchNotesWindow.VersionButton>(closure_0.selectEntry)).MarkUnseen(isNew).BorderBottom<PatchNotesWindow.VersionButton>(idx < closure_0.m_entries.Length - 1 ? 1 : 0);
      })));
      ScrollColumn component = scrollColumn;
      row.Add((UiComponent) scrollColumn);
      row.Add((UiComponent) (this.m_contents = new ScrollColumn().Fill<ScrollColumn>().Gap<ScrollColumn>(new Px?(6.pt())).PaddingBottom<ScrollColumn>(20.pt())));
      panel.Add((UiComponent) row);
      body.Add((UiComponent) panel);
      this.selectEntry(component.ChildAtOrDefault<PatchNotesWindow.VersionButton>(oldestUnseenIndex));
    }

    private void updateContent()
    {
      if (this.m_selected.IsNone)
      {
        this.m_contents.Clear();
      }
      else
      {
        ChangelogEntry entry = this.m_selected.Value.Entry;
        this.m_contents.SetChildren((IEnumerable<UiComponent>) (!this.m_unseenVersions.Contains(entry.Version) ? entry.SubEntries.AsEnumerable<ChangelogSubEntry>().Reverse<ChangelogSubEntry>() : (IEnumerable<ChangelogSubEntry>) entry.SubEntries).Select<ChangelogSubEntry, Column>((Func<ChangelogSubEntry, Column>) (se =>
        {
          Column component = new Column();
          component.Add<Column>((Action<Column>) (c => c.AlignItemsStretch<Column>()));
          component.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Title(((this.m_unseenSubVersions.Contains(se.SubVersion) ? "* " : "") + se.Heading).AsLoc()).FontStyle<Mafi.Unity.UiToolkit.Library.Title>(this.m_unseenSubVersions.Contains(se.SubVersion) ? FontStyle.BoldItalic : FontStyle.Bold).UpperCase(false).MarginBottom<Label>(1.pt()));
          component.Add((IEnumerable<UiComponent>) se.Content.SplitToLines().Select<Label>((Func<string, Label>) (line =>
          {
            if (line.StartsWith('-') || line.StartsWith('*'))
            {
              string str = line;
              line = str.Substring(1, str.Length - 1).Trim();
              return new Label(line.AsLoc()).Class<Label>(Cls.bulletedList_item);
            }
            if (!line.StartsWith(' '))
              return new Label(line.AsLoc()).FontBold<Label>().MarginBottom<Label>(1.pt());
            line = line.Trim();
            if (!line.StartsWith('-') && !line.StartsWith('*'))
              return new Label(line.AsLoc()).MarginLeft<Label>(18.px());
            string str2 = line;
            line = str2.Substring(1, str2.Length - 1).Trim();
            return new Label(line.AsLoc()).Class<Label>(Cls.bulletedList_item).MarginLeft<Label>(18.px());
          })));
          return component;
        })));
        this.m_contents.ScrollTo(0.0f);
      }
    }

    private void selectEntry(PatchNotesWindow.VersionButton button)
    {
      PatchNotesWindow.VersionButton valueOrNull1 = this.m_selected.ValueOrNull;
      if (valueOrNull1 != null)
        valueOrNull1.Selected<PatchNotesWindow.VersionButton>(false).MarkUnseen(false);
      this.m_selected = (Option<PatchNotesWindow.VersionButton>) button;
      PatchNotesWindow.VersionButton valueOrNull2 = this.m_selected.ValueOrNull;
      if (valueOrNull2 != null)
        valueOrNull2.Selected<PatchNotesWindow.VersionButton>();
      this.updateContent();
      this.m_unseenVersions.Remove(button.Entry.Version);
      this.m_unseenSubVersions.RemoveRange((IEnumerable<string>) button.Entry.SubEntries.Select<string>((Func<ChangelogSubEntry, string>) (se => se.SubVersion)));
    }

    private class VersionButton : Button
    {
      public readonly ChangelogEntry Entry;
      private readonly Label m_label;

      public VersionButton(ChangelogEntry entry, Action<PatchNotesWindow.VersionButton> onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Entry = entry;
        this.Variant<PatchNotesWindow.VersionButton>(ButtonVariant.Area);
        this.Padding<PatchNotesWindow.VersionButton>(2.pt(), 3.pt());
        this.OnClick<PatchNotesWindow.VersionButton>(onClick);
        this.Add((UiComponent) (this.m_label = new Label(entry.Version.AsLoc())));
      }

      public PatchNotesWindow.VersionButton MarkUnseen(bool isNew)
      {
        if (isNew)
        {
          this.FontBold<PatchNotesWindow.VersionButton>();
          this.m_label.Color<Label>(new ColorRgba?(Theme.PrimaryColor));
          this.m_label.Text<Label>(("* " + this.Entry.Version).AsLoc());
        }
        else
        {
          this.FontNormal<PatchNotesWindow.VersionButton>();
          this.m_label.Color<Label>(new ColorRgba?());
          this.m_label.Text<Label>(this.Entry.Version.AsLoc());
        }
        return this;
      }
    }
  }
}
