// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.IconSpec
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  public struct IconSpec : IEquatable<IconSpec>
  {
    public readonly string PrefabPath;
    public readonly ColorRgba Color;

    public IconSpec(string prefabPath)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this = new IconSpec(prefabPath, ColorRgba.White);
    }

    public IconSpec(string prefabPath, ColorRgba color)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.PrefabPath = prefabPath;
      this.Color = color;
    }

    public bool Equals(IconSpec other)
    {
      return this.PrefabPath == other.PrefabPath && this.Color.Equals(other.Color);
    }

    public override bool Equals(object obj) => obj is IconSpec other && this.Equals(other);

    public override int GetHashCode()
    {
      return Hash.Combine<string, ColorRgba>(this.PrefabPath, this.Color);
    }
  }
}
