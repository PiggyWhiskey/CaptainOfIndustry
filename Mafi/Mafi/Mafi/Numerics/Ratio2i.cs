// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.Ratio2i
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>
  /// Represents a ratio consisting of two positive integers.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public struct Ratio2i : IEquatable<Ratio2i>
  {
    /// <summary>
    /// Always positive, except for a struct created by default constructor.
    /// </summary>
    public readonly int A;
    /// <summary>
    /// Always positive, except for a struct created by default constructor.
    /// </summary>
    public readonly int B;

    public static void Serialize(Ratio2i value, BlobWriter writer)
    {
      writer.WriteInt(value.A);
      writer.WriteInt(value.B);
    }

    public static Ratio2i Deserialize(BlobReader reader)
    {
      return new Ratio2i(reader.ReadInt(), reader.ReadInt());
    }

    public Ratio2i(int a, int b)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      Assert.That<int>(a).IsPositive();
      Assert.That<int>(b).IsPositive();
      this.A = a;
      this.B = b;
    }

    public int this[int i]
    {
      get
      {
        if (i == 0)
          return this.A;
        if (i == 1)
          return this.B;
        throw new IndexOutOfRangeException();
      }
    }

    /// <summary>
    /// Attempts to parse Ration in format "a:b". Returns null when the parsing fails.
    /// </summary>
    public static Ratio2i? TryParse(string input)
    {
      int result;
      Lyst<int?> lyst = ((IEnumerable<string>) input.Split(':')).Select<string, int?>((Func<string, int?>) (s => !int.TryParse(s, out result) ? new int?() : new int?(result))).ToLyst<int?>();
      return lyst.Count == 2 && lyst.All<int?>((Func<int?, bool>) (n => n.HasValue && n.Value > 0)) ? new Ratio2i?(new Ratio2i(lyst[0].Value, lyst[1].Value)) : new Ratio2i?();
    }

    /// <summary>
    /// Returns reduced ratio.
    /// Example: ratio 2:2 will be reduced to 1:1, ratio 4:2 will be reduced to 2:1
    /// </summary>
    public Ratio2i Reduce()
    {
      if (this.A <= 0 || this.B <= 0)
        return this;
      int num = MafiMath.Gcd(this.A, this.B);
      return new Ratio2i(this.A / num, this.B / num);
    }

    public bool IsValid() => this.A > 0 && this.B > 0;

    public bool Equals(Ratio2i other) => this.A == other.A && this.B == other.B;

    public override string ToString() => string.Format("{0}:{1}", (object) this.A, (object) this.B);

    public override bool Equals(object obj)
    {
      return obj != null && obj is Ratio2i other && this.Equals(other);
    }

    public override int GetHashCode() => this.A * 397 ^ this.B;
  }
}
