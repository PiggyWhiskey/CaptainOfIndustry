// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ports.Io.HighlightId
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ports.Io
{
  public readonly struct HighlightId : IEquatable<HighlightId>
  {
    public static readonly HighlightId Empty;
    public readonly int Value;

    public HighlightId(int value)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Value = value;
    }

    public override string ToString() => this.Value.ToString();

    public static bool operator ==(HighlightId lhs, HighlightId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(HighlightId lhs, HighlightId rhs) => lhs.Value != rhs.Value;

    public bool Equals(HighlightId other) => this.Value == other.Value;

    public override bool Equals(object obj) => obj is HighlightId other && this.Equals(other);

    public override int GetHashCode() => this.Value;

    static HighlightId() => xxhJUtQyC9HnIshc6H.OukgcisAbr();
  }
}
