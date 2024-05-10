// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.Settings.ControlsTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.Settings
{
  public class ControlsTab : Mafi.Unity.UiToolkit.Library.Column, IComponentWithInputUpdate
  {
    private static readonly Percent LABEL_WIDTH;
    private LystStruct<ControlsTab.KeyBinder> m_bindFields;
    private readonly Mafi.Unity.UiToolkit.Library.Button m_discardButton;
    private readonly ConfirmButton m_applyButton;
    private readonly Option<ShortcutsManager> m_shortcutsManager;
    private Option<ControlsTab.KeyBinder> m_activeBindingSession;
    private int m_changedTotal;
    private int m_conflictsTotal;

    public ControlsTab(Option<ShortcutsManager> shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Px? gap = new Px?();
      // ISSUE: explicit constructor call
      base.\u002Ector(gap: gap);
      this.m_shortcutsManager = shortcutsManager;
      this.AlignItemsStretch<ControlsTab>().Fill<ControlsTab>();
      UiComponent[] uiComponentArray = new UiComponent[2];
      ScrollColumn component1 = new ScrollColumn();
      component1.Add<ScrollColumn>((Action<ScrollColumn>) (c => c.Fill<ScrollColumn>().Gap<ScrollColumn>(new Px?(SettingsWindow.SECTION_GAP))));
      component1.Add((IEnumerable<UiComponent>) ShortcutsMap.Categories.Select<KeyValuePair<KbCategory, LocStrFormatted>, Mafi.Unity.UiToolkit.Library.Column>((Func<KeyValuePair<KbCategory, LocStrFormatted>, Mafi.Unity.UiToolkit.Library.Column>) (cat =>
      {
        Mafi.Unity.UiToolkit.Library.Column component2 = new Mafi.Unity.UiToolkit.Library.Column(2.pt());
        component2.Add<Mafi.Unity.UiToolkit.Library.Column>((Action<Mafi.Unity.UiToolkit.Library.Column>) (c => c.AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>().PaddingLeft<Mafi.Unity.UiToolkit.Library.Column>(SettingsWindow.SECTION_INDENT).AlignItemsStretch<Mafi.Unity.UiToolkit.Library.Column>()));
        component2.Add((UiComponent) new SettingsTitle(cat.Value));
        component2.Add((IEnumerable<UiComponent>) ShortcutsMap.GetKeybindings(cat.Key).Select<KeyValuePair<PropertyInfo, KbAttribute>, Mafi.Unity.UiToolkit.Library.Row>((Func<KeyValuePair<PropertyInfo, KbAttribute>, Mafi.Unity.UiToolkit.Library.Row>) (kvp =>
        {
          Mafi.Unity.UiToolkit.Library.Row row3 = new Mafi.Unity.UiToolkit.Library.Row(2.pt());
          row3.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label(kvp.Value.Title).FlexNoShrink<Mafi.Unity.UiToolkit.Library.Label>().Width<Mafi.Unity.UiToolkit.Library.Label>(ControlsTab.LABEL_WIDTH));
          ControlsTab.KeyBinder child3;
          ControlsTab.KeyBinder keyBinder3 = child3 = new ControlsTab.KeyBinder(this, kvp.Value, kvp.Key, true);
          row3.Add((UiComponent) child3);
          ControlsTab.KeyBinder child4;
          ControlsTab.KeyBinder keyBinder4 = child4 = new ControlsTab.KeyBinder(this, kvp.Value, kvp.Key, false);
          row3.Add((UiComponent) child4);
          Mafi.Unity.UiToolkit.Library.Row row4 = row3;
          this.m_bindFields.Add(keyBinder3);
          this.m_bindFields.Add(keyBinder4);
          return row4;
        })));
        return component2;
      })));
      component1.Add(new UiComponent().PaddingBottom<UiComponent>(10.pt()));
      uiComponentArray[0] = (UiComponent) component1;
      gap = new Px?(2.pt());
      Mafi.Unity.UiToolkit.Library.Row component3 = new Mafi.Unity.UiToolkit.Library.Row(Outer.EdgeShadowTop, gap: gap);
      component3.Add<Mafi.Unity.UiToolkit.Library.Row>((Action<Mafi.Unity.UiToolkit.Library.Row>) (c => c.FlexNoShrink<Mafi.Unity.UiToolkit.Library.Row>().PaddingTop<Mafi.Unity.UiToolkit.Library.Row>(3.pt())));
      component3.Add((UiComponent) new ButtonText((LocStrFormatted) Tr.RestoreDefaults, new Action(this.onRestoreDefaults)).FlipNotches<ButtonText>().Class<ButtonText>(Cls.bold));
      Mafi.Unity.UiToolkit.Library.Row component4 = new Mafi.Unity.UiToolkit.Library.Row(2.pt());
      Mafi.Unity.UiToolkit.Library.Row child5 = new Mafi.Unity.UiToolkit.Library.Row(2.px());
      child5.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/LeftClick.png").Medium());
      child5.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label((LocStrFormatted) Tr.KeybindingHowToEdit));
      component4.Add((UiComponent) child5);
      Mafi.Unity.UiToolkit.Library.Row child6 = new Mafi.Unity.UiToolkit.Library.Row(2.px());
      child6.Add((UiComponent) new Icon("Assets/Unity/UserInterface/General/RightClick.png").Medium());
      child6.Add((UiComponent) new Mafi.Unity.UiToolkit.Library.Label((LocStrFormatted) Tr.KeybindingHowToClear));
      component4.Add((UiComponent) child6);
      component3.Add((UiComponent) component4.MarginLeftRight<Mafi.Unity.UiToolkit.Library.Row>(Px.Auto));
      component3.Add((UiComponent) (this.m_discardButton = (Mafi.Unity.UiToolkit.Library.Button) new ButtonText((LocStrFormatted) Tr.DiscardChanges, new Action(this.onDiscard)).Class<ButtonText>(Cls.bold).VisibleForRender<ButtonText>(false)));
      component3.Add((UiComponent) (this.m_applyButton = new ConfirmButton((LocStrFormatted) Tr.ApplyChanges, (LocStrFormatted) Tr.ApplyChangesConflictPrompt, new Action(this.onApply)).Variant<ConfirmButton>(ButtonVariant.Primary)));
      uiComponentArray[1] = (UiComponent) component3;
      this.Add(uiComponentArray);
      this.checkForConflicts();
    }

    public bool InputUpdate()
    {
      if (!this.m_activeBindingSession.HasValue)
        return false;
      this.m_activeBindingSession.Value.InputUpdate();
      return true;
    }

    private void changeStarted(ControlsTab.KeyBinder binder)
    {
      this.discardActiveBinding();
      this.m_activeBindingSession = (Option<ControlsTab.KeyBinder>) binder;
    }

    private void checkForConflicts()
    {
      this.m_activeBindingSession = (Option<ControlsTab.KeyBinder>) Option.None;
      this.m_changedTotal = 0;
      this.m_conflictsTotal = 0;
      Dict<string, Lyst<ControlsTab.KeyBinder>> dict = new Dict<string, Lyst<ControlsTab.KeyBinder>>();
      foreach (ControlsTab.KeyBinder bindField in this.m_bindFields)
      {
        bindField.ClearConflicts();
        this.m_changedTotal += bindField.HasChanges ? 1 : 0;
        if (!bindField.Binding.IsEmpty)
        {
          string key = bindField.Binding.ToString();
          Lyst<ControlsTab.KeyBinder> lyst;
          if (!dict.TryGetValue(key, out lyst))
          {
            lyst = new Lyst<ControlsTab.KeyBinder>();
            dict.Add(key, lyst);
          }
          lyst.Add(bindField);
        }
      }
      foreach (Lyst<ControlsTab.KeyBinder> lyst in dict.Values)
      {
        if (lyst.Count > 1)
        {
          foreach (ControlsTab.KeyBinder first in lyst)
          {
            bool flag1 = KbCategoriesData.MutuallyExclusiveCategories.Contains(first.Data.Category);
            foreach (ControlsTab.KeyBinder second in lyst)
            {
              if (first != second && (first.Mode & second.Mode) != (ShortcutMode) 0)
              {
                if (second.Data.Category == first.Data.Category)
                  markConflict(first, second);
                else if (!first.Data.IgnoreConflicts && !second.Data.IgnoreConflicts)
                {
                  bool flag2 = KbCategoriesData.MutuallyExclusiveCategories.Contains(second.Data.Category);
                  if (!flag1 || !flag2)
                    markConflict(first, second);
                }
              }
            }
          }
        }
      }
      this.refreshButtons();

      void markConflict(ControlsTab.KeyBinder first, ControlsTab.KeyBinder second)
      {
        first.AddConflictField(second);
        ++this.m_conflictsTotal;
      }
    }

    private void refreshButtons()
    {
      this.m_discardButton.VisibleForRender<Mafi.Unity.UiToolkit.Library.Button>(this.m_changedTotal > 0);
      this.m_applyButton.Enabled<ConfirmButton>(this.m_changedTotal > 0).ConfirmRequired(this.m_conflictsTotal > 0);
    }

    private void onRestoreDefaults()
    {
      this.m_changedTotal = 0;
      this.m_activeBindingSession = Option<ControlsTab.KeyBinder>.None;
      this.m_conflictsTotal = 0;
      ShortcutsStorage.RestoreDefaults();
      foreach (ControlsTab.KeyBinder bindField in this.m_bindFields)
        bindField.RestoreDefault();
      this.refreshButtons();
      this.m_shortcutsManager.ValueOrNull?.NotifyKeyBindingsChanged();
    }

    private void onDiscard()
    {
      this.m_changedTotal = 0;
      foreach (ControlsTab.KeyBinder bindField in this.m_bindFields)
        bindField.DiscardChanges();
      this.checkForConflicts();
    }

    private void onApply()
    {
      this.m_changedTotal = 0;
      this.discardActiveBinding();
      foreach (ControlsTab.KeyBinder bindField in this.m_bindFields)
        bindField.ApplyChanges();
      ShortcutsStorage.ApplyChanges();
      this.checkForConflicts();
      this.m_shortcutsManager.ValueOrNull?.NotifyKeyBindingsChanged();
    }

    private void discardActiveBinding()
    {
      if (!this.m_activeBindingSession.HasValue)
        return;
      this.m_activeBindingSession.Value.Cancel();
      this.m_activeBindingSession = Option<ControlsTab.KeyBinder>.None;
    }

    static ControlsTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ControlsTab.LABEL_WIDTH = 40.Percent();
    }

    private class KeyBinder : KeyBindingField
    {
      private readonly ControlsTab m_tab;
      private readonly bool m_isPrimary;
      private readonly PropertyInfo m_attr;
      public readonly KbAttribute Data;
      public ShortcutMode Mode;
      public KeyBinding Binding;
      public KeyBinding OriginalBinding;
      private Lyst<KeyCode> m_keys;
      private bool m_isEditing;
      private string m_conflictStr;

      public bool HasChanges => !this.Binding.AreSame(this.OriginalBinding);

      public KeyBinder(ControlsTab tab, KbAttribute data, PropertyInfo attr, bool isPrimary)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_keys = new Lyst<KeyCode>();
        this.m_conflictStr = string.Empty;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_tab = tab;
        this.m_isPrimary = isPrimary;
        this.Data = data;
        this.m_attr = attr;
        this.Width<ControlsTab.KeyBinder>(22.Percent()).Height<ControlsTab.KeyBinder>(new Px?(27.px())).MarginLeft<ControlsTab.KeyBinder>(24.px());
        if (!this.m_isPrimary || !this.Data.IsPrimaryReadonly)
        {
          this.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.onMouseUp));
          this.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.onMouseDown));
        }
        else
          this.Enabled<ControlsTab.KeyBinder>(false);
        this.RestoreDefault();
      }

      public void ClearConflicts()
      {
        this.m_conflictStr = string.Empty;
        this.MarkAsError(false);
      }

      public void AddConflictField(ControlsTab.KeyBinder field)
      {
        this.m_conflictStr += string.Format("\n - {0}", (object) field.Data.Title);
        this.MarkAsError(true, string.Format("{0}{1}", (object) Tr.ConflictsWith, (object) this.m_conflictStr).AsLoc());
      }

      public void DiscardChanges()
      {
        this.Binding = this.OriginalBinding;
        this.ClearConflicts();
        this.Cancel();
      }

      public void RestoreDefault()
      {
        KeyBindings keyBindings = (KeyBindings) this.m_attr.GetValue((object) ShortcutsStorage.Current);
        this.Binding = this.m_isPrimary ? keyBindings.Primary : keyBindings.Secondary;
        this.Mode = keyBindings.Mode;
        this.OriginalBinding = this.Binding;
        this.ClearConflicts();
        this.updateAppearance();
      }

      public void ApplyChanges()
      {
        if (this.HasChanges)
        {
          this.OriginalBinding = this.Binding;
          KeyBindings keyBindings = (KeyBindings) this.m_attr.GetValue((object) ShortcutsStorage.Current);
          keyBindings = !this.m_isPrimary ? new KeyBindings(keyBindings.Mode, keyBindings.Primary, this.Binding) : new KeyBindings(keyBindings.Mode, this.Binding, keyBindings.Secondary);
          this.m_attr.SetValue((object) ShortcutsStorage.Current, (object) keyBindings);
        }
        this.m_isEditing = false;
        this.updateAppearance();
      }

      private void updateAppearance()
      {
        this.Text<ControlsTab.KeyBinder>(this.Binding.ToNiceString().AsLoc()).Color<ControlsTab.KeyBinder>(new ColorRgba?(this.HasChanges ? Theme.PrimaryColor : Theme.Text));
      }

      private void onMouseUp(MouseUpEvent evt)
      {
        if (evt.button != 0 || this.m_isEditing)
          return;
        this.m_isEditing = true;
        this.Text<ControlsTab.KeyBinder>(string.Format("{0} ...", (object) Tr.WaitingForKeyPress).AsLoc()).Color<ControlsTab.KeyBinder>(new ColorRgba?(Theme.PrimaryColor));
        this.m_keys.Clear();
        this.m_tab.changeStarted(this);
      }

      private void onMouseDown(MouseDownEvent evt)
      {
        if (evt.button != 1 || this.m_isEditing)
          return;
        this.commit(true);
      }

      public void InputUpdate()
      {
        if (!this.m_isEditing)
          return;
        if (UnityEngine.Input.GetKey(KeyCode.Escape))
        {
          this.Cancel();
        }
        else
        {
          KeyCode[] allKeys = ShortcutsMap.AllKeys;
          int num = 0;
          int count = this.m_keys.Count;
          for (int index = 0; index < allKeys.Length; ++index)
          {
            if (UnityEngine.Input.GetKey(allKeys[index]))
            {
              ++num;
              if (!this.m_keys.Contains(allKeys[index]))
                this.m_keys.Add(allKeys[index]);
            }
          }
          if (count != this.m_keys.Count)
            this.Text<ControlsTab.KeyBinder>((((IEnumerable<string>) this.m_keys.Select<string>((Func<KeyCode, string>) (x => x.ToNiceString()))).JoinStrings(" + ") + " ...").AsLoc());
          if (!this.m_keys.IsNotEmpty || num >= this.m_keys.Count)
            return;
          this.commit();
        }
      }

      public void Cancel()
      {
        this.m_keys.Clear();
        this.m_keys.AddRange(this.Binding.Keys);
        this.commit();
      }

      private void commit(bool clear = false)
      {
        if (this.m_isEditing | clear)
        {
          this.m_isEditing = false;
          this.Binding = !clear ? (this.m_keys.Count <= 0 ? this.OriginalBinding : new KeyBinding(KbCategory.Camera, this.m_keys.ToImmutableArray())) : new KeyBinding(KbCategory.Camera);
          this.m_tab.checkForConflicts();
        }
        this.updateAppearance();
      }
    }
  }
}
