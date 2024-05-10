// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.KeyBinding
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  public readonly struct KeyBinding
  {
    public readonly KbCategory Category;
    public static readonly KeyBinding EMPTY;
    public readonly ImmutableArray<KeyCode> Keys;

    public bool IsEmpty => this.Keys.IsEmpty;

    public KeyBinding(KbCategory category)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Category = category;
      this.Keys = ImmutableArray<KeyCode>.Empty;
    }

    public KeyBinding(KbCategory category, ImmutableArray<KeyCode> keys)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Category = category;
      this.Keys = keys;
    }

    public static KeyBinding Empty(KbCategory category)
    {
      return new KeyBinding(category, ImmutableArray<KeyCode>.Empty);
    }

    public static KeyBinding FromKey(KbCategory category, KeyCode code)
    {
      return new KeyBinding(category, ImmutableArray.Create<KeyCode>(code));
    }

    public static KeyBinding FromKeys(KbCategory category, KeyCode code1, KeyCode code2)
    {
      return new KeyBinding(category, ImmutableArray.Create<KeyCode>(code1, code2));
    }

    public static KeyBinding FromKeys(
      KbCategory category,
      KeyCode code1,
      KeyCode code2,
      KeyCode code3)
    {
      return new KeyBinding(category, ImmutableArray.Create<KeyCode>(code1, code2, code3));
    }

    public override string ToString()
    {
      return this.Keys.Select<string>((Func<KeyCode, string>) (x => x.ToString())).JoinStrings(" + ");
    }

    public string ToNiceString()
    {
      return this.Keys.Select<string>((Func<KeyCode, string>) (x => x.ToNiceString())).JoinStrings(" + ");
    }

    public bool IsCode(KeyCode code) => this.Keys.Length == 1 && this.Keys.First == code;

    public KeyBinding UpdateSelfFrom(string input)
    {
      if (input == null)
        return this;
      if (input.IsEmpty())
        return KeyBinding.EMPTY;
      Lyst<KeyCode> lyst = new Lyst<KeyCode>();
      foreach (string str in input.Split('+', StringSplitOptions.None))
      {
        KeyCode result;
        if (Enum.TryParse<KeyCode>(str, out result))
        {
          lyst.Add(result);
        }
        else
        {
          Log.Error("Failed to parse shortcut '" + input + "'");
          return this;
        }
      }
      return new KeyBinding(this.Category, lyst.ToImmutableArray());
    }

    public bool AreSame(KeyBinding other)
    {
      if (other.Keys.Length != this.Keys.Length)
        return false;
      for (int index = 0; index < this.Keys.Length; ++index)
      {
        if (other.Keys[index] != this.Keys[index])
          return false;
      }
      return true;
    }

    static KeyBinding()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      KeyBinding.EMPTY = new KeyBinding(KbCategory.Camera, ImmutableArray<KeyCode>.Empty);
    }
  }
}
