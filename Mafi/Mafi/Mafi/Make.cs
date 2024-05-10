// Decompiled with JetBrains decompiler
// Type: Mafi.Make
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.Collections.Generic;

#nullable disable
namespace Mafi
{
  public static class Make
  {
    /// <summary>
    /// Convenience method for making pairs. This avoid the need for specifying types of <see cref="T:System.Collections.Generic.KeyValuePair`2" />'s constructor explicitly.
    /// </summary>
    public static KeyValuePair<TKey, TValue> Kvp<TKey, TValue>(TKey key, TValue value)
    {
      return new KeyValuePair<TKey, TValue>(key, value);
    }
  }
}
