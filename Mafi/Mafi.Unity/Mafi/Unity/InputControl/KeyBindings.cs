// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.KeyBindings
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  public readonly struct KeyBindings
  {
    public readonly ShortcutMode Mode;
    public readonly KeyBinding Primary;
    public readonly KeyBinding Secondary;
    public static readonly KeyBindings EMPTY;

    public bool IsEmpty => this.Primary.IsEmpty && this.Secondary.IsEmpty;

    public KeyBindings(ShortcutMode mode, KeyBinding primary, KeyBinding secondary)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Mode = mode;
      this.Primary = primary;
      this.Secondary = secondary;
    }

    public static KeyBindings Empty(KbCategory category, ShortcutMode mode)
    {
      return new KeyBindings(mode, KeyBinding.Empty(category), KeyBinding.Empty(category));
    }

    public static KeyBindings FromKey(KbCategory category, ShortcutMode mode, KeyCode code)
    {
      return new KeyBindings(mode, KeyBinding.FromKey(category, code), KeyBinding.Empty(category));
    }

    public static KeyBindings FromPrimaryKeys(
      KbCategory category,
      ShortcutMode mode,
      KeyCode first,
      KeyCode second)
    {
      return new KeyBindings(mode, KeyBinding.FromKeys(category, first, second), KeyBinding.Empty(category));
    }

    public static KeyBindings FromKeys(
      KbCategory category,
      ShortcutMode mode,
      KeyCode primary,
      KeyCode secondary)
    {
      return new KeyBindings(mode, KeyBinding.FromKey(category, primary), KeyBinding.FromKey(category, secondary));
    }

    public static KeyBindings FromKeys(
      KbCategory category,
      ShortcutMode mode,
      KeyCode primary,
      KeyCode secondary,
      KeyCode secondary2)
    {
      return new KeyBindings(mode, KeyBinding.FromKey(category, primary), KeyBinding.FromKeys(category, secondary, secondary2));
    }

    public bool IsCode(KeyCode code)
    {
      if (!this.Primary.IsEmpty && !this.Primary.IsCode(code))
        return false;
      return this.Secondary.IsEmpty || this.Secondary.IsCode(code);
    }

    public string ToNiceString()
    {
      return this.Primary.IsEmpty ? this.Secondary.ToNiceString() : this.Primary.ToNiceString();
    }

    public string ToNiceStringLong()
    {
      if (!this.Primary.IsEmpty && !this.Secondary.IsEmpty)
        return this.Primary.ToNiceString() + " (" + this.Secondary.ToNiceString() + ")";
      return this.Primary.IsEmpty ? this.Secondary.ToNiceString() : this.Primary.ToNiceString();
    }

    static KeyBindings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      KeyBindings.EMPTY = new KeyBindings(ShortcutMode.Game, KeyBinding.EMPTY, KeyBinding.EMPTY);
    }
  }
}
