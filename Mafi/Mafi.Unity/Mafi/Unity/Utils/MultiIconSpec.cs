// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.MultiIconSpec
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  public readonly struct MultiIconSpec : IEquatable<MultiIconSpec>
  {
    public readonly ImmutableArray<string> PrefabPaths;
    public readonly ColorRgba Color;
    private readonly int m_hashCode;

    public MultiIconSpec(ImmutableArray<string> prefabPaths)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this = new MultiIconSpec(prefabPaths, ColorRgba.White);
    }

    public MultiIconSpec(ImmutableArray<string> prefabPaths, ColorRgba color)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.PrefabPaths = prefabPaths.GetRange(0, prefabPaths.Length.Min(4));
      this.Color = color;
      this.m_hashCode = 17;
      foreach (object prefabPath in prefabPaths)
        this.m_hashCode = this.m_hashCode * 31 + prefabPath.GetHashCode();
    }

    public bool Equals(MultiIconSpec other) => this.m_hashCode.Equals(other.m_hashCode);

    public override bool Equals(object obj) => obj is MultiIconSpec other && this.Equals(other);

    public override int GetHashCode() => this.m_hashCode;
  }
}
