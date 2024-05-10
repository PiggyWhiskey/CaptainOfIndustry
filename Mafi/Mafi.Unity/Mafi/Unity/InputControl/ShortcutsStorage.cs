// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ShortcutsStorage
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  /// <summary>Bridge between ShortcutsManager and static stuff</summary>
  public static class ShortcutsStorage
  {
    public static readonly ShortcutsMap Defaults;
    public static readonly ShortcutsMap Current;
    public static int LastVersion;

    public static KeyValuePair<PropertyInfo, KbAttribute>[] GetAllAttributes()
    {
      return ((IEnumerable<PropertyInfo>) typeof (ShortcutsMap).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.HasAttribute<KbAttribute>())).Select<PropertyInfo, KeyValuePair<PropertyInfo, KbAttribute>>((Func<PropertyInfo, KeyValuePair<PropertyInfo, KbAttribute>>) (x => Make.Kvp<PropertyInfo, KbAttribute>(x, (KbAttribute) x.GetCustomAttributes(typeof (KbAttribute), false).First<object>()))).ToArray<KeyValuePair<PropertyInfo, KbAttribute>>();
    }

    static ShortcutsStorage()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ShortcutsStorage.Defaults = new ShortcutsMap();
      ShortcutsStorage.Current = new ShortcutsMap();
      ShortcutsStorage.LastVersion = 1;
      foreach (KeyValuePair<PropertyInfo, KbAttribute> allAttribute in ShortcutsStorage.GetAllAttributes())
      {
        KbAttribute kbAttribute = allAttribute.Value;
        PropertyInfo key = allAttribute.Key;
        KeyBindings keyBindings = (KeyBindings) key.GetValue((object) ShortcutsStorage.Current);
        KeyBinding primary;
        if (PlayerPrefs.HasKey(kbAttribute.PrefsIdPrimary))
        {
          string input = PlayerPrefs.GetString(kbAttribute.PrefsIdPrimary, (string) null);
          primary = keyBindings.Primary.UpdateSelfFrom(input);
        }
        else
          primary = keyBindings.Primary;
        KeyBinding secondary;
        if (PlayerPrefs.HasKey(kbAttribute.PrefsIdSecondary))
        {
          string input = PlayerPrefs.GetString(kbAttribute.PrefsIdSecondary, (string) null);
          secondary = keyBindings.Secondary.UpdateSelfFrom(input);
        }
        else
          secondary = keyBindings.Secondary;
        key.SetValue((object) ShortcutsStorage.Current, (object) new KeyBindings(keyBindings.Mode, primary, secondary));
      }
    }

    public static KeyBindings FindById(string groupId)
    {
      PropertyInfo propertyInfo = ((IEnumerable<KeyValuePair<PropertyInfo, KbAttribute>>) ShortcutsStorage.GetAllAttributes()).Where<KeyValuePair<PropertyInfo, KbAttribute>>((Func<KeyValuePair<PropertyInfo, KbAttribute>, bool>) (x => x.Value.GroupId == groupId)).Select<KeyValuePair<PropertyInfo, KbAttribute>, PropertyInfo>((Func<KeyValuePair<PropertyInfo, KbAttribute>, PropertyInfo>) (x => x.Key)).FirstOrDefault<PropertyInfo>();
      return propertyInfo == (PropertyInfo) null ? KeyBindings.EMPTY : (KeyBindings) propertyInfo.GetValue((object) ShortcutsStorage.Current);
    }

    public static void ApplyChanges()
    {
      ++ShortcutsStorage.LastVersion;
      foreach (KeyValuePair<PropertyInfo, KbAttribute> allAttribute in ShortcutsStorage.GetAllAttributes())
      {
        KbAttribute kbAttribute = allAttribute.Value;
        PropertyInfo key = allAttribute.Key;
        KeyBindings keyBindings1 = (KeyBindings) key.GetValue((object) ShortcutsStorage.Current);
        KeyBindings keyBindings2 = (KeyBindings) key.GetValue((object) ShortcutsStorage.Defaults);
        if (keyBindings1.Primary.AreSame(keyBindings2.Primary))
        {
          if (PlayerPrefs.GetString(kbAttribute.PrefsIdPrimary, (string) null) != null)
            PlayerPrefs.DeleteKey(kbAttribute.PrefsIdPrimary);
        }
        else
          PlayerPrefs.SetString(kbAttribute.PrefsIdPrimary, keyBindings1.Primary.ToString());
        if (keyBindings1.Secondary.AreSame(keyBindings2.Secondary))
        {
          if (PlayerPrefs.GetString(kbAttribute.PrefsIdSecondary, (string) null) != null)
            PlayerPrefs.DeleteKey(kbAttribute.PrefsIdSecondary);
        }
        else
          PlayerPrefs.SetString(kbAttribute.PrefsIdSecondary, keyBindings1.Secondary.ToString());
      }
      PlayerPrefs.Save();
    }

    public static void RestoreDefaults()
    {
      foreach (KeyValuePair<PropertyInfo, KbAttribute> allAttribute in ShortcutsStorage.GetAllAttributes())
      {
        PropertyInfo key = allAttribute.Key;
        KeyBindings keyBindings = (KeyBindings) key.GetValue((object) ShortcutsStorage.Defaults);
        key.SetValue((object) ShortcutsStorage.Current, (object) keyBindings);
      }
      ShortcutsStorage.ApplyChanges();
    }
  }
}
