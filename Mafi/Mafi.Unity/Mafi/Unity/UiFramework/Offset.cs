// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Offset
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Diagnostics;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  /// <summary>Offset of UI element in pixels.</summary>
  [DebuggerStepThrough]
  [DebuggerDisplay("(r:{RightOffset}, t:{TopOffset}, l:{LeftOffset}, b:{BottomOffset})")]
  public struct Offset : IEquatable<Offset>
  {
    public static readonly Offset Zero;
    /// <summary>Positive X offset.</summary>
    public readonly float RightOffset;
    /// <summary>Positive Y offset.</summary>
    public readonly float TopOffset;
    /// <summary>Negative X offset.</summary>
    public readonly float LeftOffset;
    /// <summary>Negative Y offset.</summary>
    public readonly float BottomOffset;

    public static Offset Right(float rightOffset) => new Offset(rightOffset, 0.0f, 0.0f, 0.0f);

    public static Offset Top(float topOffset) => new Offset(0.0f, topOffset, 0.0f, 0.0f);

    public static Offset Left(float leftOffset) => new Offset(0.0f, 0.0f, leftOffset, 0.0f);

    public static Offset Bottom(float bottomOffset) => new Offset(0.0f, 0.0f, 0.0f, bottomOffset);

    public static Offset BottomRight(float bottomOffset, float rightOffset)
    {
      return new Offset(rightOffset, 0.0f, 0.0f, bottomOffset);
    }

    public static Offset BottomLeft(float bottomOffset, float leftOffset)
    {
      return new Offset(0.0f, 0.0f, leftOffset, bottomOffset);
    }

    public static Offset LeftRight(float leftRightOffset)
    {
      return new Offset(leftRightOffset, 0.0f, leftRightOffset, 0.0f);
    }

    public static Offset TopBottom(float topBottomOffset)
    {
      return new Offset(0.0f, topBottomOffset, 0.0f, topBottomOffset);
    }

    public static Offset TopRight(float topOffset, float rightOffset)
    {
      return new Offset(rightOffset, topOffset, 0.0f, 0.0f);
    }

    public static Offset TopLeft(float topOffset, float leftOffset)
    {
      return new Offset(0.0f, topOffset, leftOffset, 0.0f);
    }

    public static Offset All(float allOffsets)
    {
      return new Offset(allOffsets, allOffsets, allOffsets, allOffsets);
    }

    public Offset(float right, float top, float left, float bottom)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.RightOffset = right;
      this.TopOffset = top;
      this.LeftOffset = left;
      this.BottomOffset = bottom;
    }

    /// <summary>Horizontal offset.</summary>
    public float LeftRightOffset => this.LeftOffset + this.RightOffset;

    /// <summary>Vertical offset.</summary>
    public float TopBottomOffset => this.TopOffset + this.BottomOffset;

    [Pure]
    public Offset AddRight(float rightOffset)
    {
      return new Offset(this.RightOffset + rightOffset, this.TopOffset, this.LeftOffset, this.BottomOffset);
    }

    [Pure]
    public Offset AddTop(float topOffset)
    {
      return new Offset(this.RightOffset, this.TopOffset + topOffset, this.LeftOffset, this.BottomOffset);
    }

    [Pure]
    public Offset AddLeft(float leftOffset)
    {
      return new Offset(this.RightOffset, this.TopOffset, this.LeftOffset + leftOffset, this.BottomOffset);
    }

    [Pure]
    public Offset AddBottom(float bottomOffset)
    {
      return new Offset(this.RightOffset, this.TopOffset, this.LeftOffset, this.BottomOffset + bottomOffset);
    }

    public static Offset operator +(Offset lhs, Offset rhs)
    {
      return new Offset(lhs.RightOffset + rhs.RightOffset, lhs.TopOffset + rhs.TopOffset, lhs.LeftOffset + rhs.LeftOffset, lhs.BottomOffset + rhs.BottomOffset);
    }

    public static Offset operator -(Offset lhs, Offset rhs)
    {
      return new Offset(lhs.RightOffset - rhs.RightOffset, lhs.TopOffset - rhs.TopOffset, lhs.LeftOffset - rhs.LeftOffset, lhs.BottomOffset - rhs.BottomOffset);
    }

    public static Offset operator *(float lhs, Offset rhs)
    {
      return new Offset(lhs * rhs.RightOffset, lhs * rhs.TopOffset, lhs * rhs.LeftOffset, lhs * rhs.BottomOffset);
    }

    public static Offset operator *(Offset lhs, float rhs)
    {
      return new Offset(lhs.RightOffset * rhs, lhs.TopOffset * rhs, lhs.LeftOffset * rhs, lhs.BottomOffset * rhs);
    }

    public bool Equals(Offset other)
    {
      return (double) this.RightOffset == (double) other.RightOffset && (double) this.TopOffset == (double) other.TopOffset && (double) this.LeftOffset == (double) other.LeftOffset && (double) this.BottomOffset == (double) other.BottomOffset;
    }
  }
}
