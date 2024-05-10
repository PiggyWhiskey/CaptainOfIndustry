// Decompiled with JetBrains decompiler
// Type: Mafi.Hash
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class Hash
  {
    /// <summary>
    /// Combines two hashes in significantly better way than just XOR.
    /// </summary>
    [Pure]
    public static int Combine(int h1, int h2) => (h1 << 5) + h1 ^ h2;

    /// <summary>
    /// Combines two hashes in significantly better way than just XOR.
    /// </summary>
    [Pure]
    public static int Combine<T, U>(T t, U u)
    {
      int hashCode = (object) t == null ? 0 : t.GetHashCode();
      return (hashCode << 5) + hashCode ^ ((object) u == null ? 0 : u.GetHashCode());
    }

    /// <summary>
    /// Combines three hashes in significantly better way than just XOR.
    /// </summary>
    [Pure]
    public static int Combine<T, U, V>(T t, U u, V v)
    {
      int hashCode = (object) t == null ? 0 : t.GetHashCode();
      int num = (hashCode << 5) + hashCode ^ ((object) u == null ? 0 : u.GetHashCode());
      return (num << 5) + num ^ ((object) v == null ? 0 : v.GetHashCode());
    }

    /// <summary>
    /// Combines four hashes in significantly better way than just XOR.
    /// </summary>
    [Pure]
    public static int Combine<T, U, V, W>(T t, U u, V v, W w)
    {
      int hashCode = (object) t == null ? 0 : t.GetHashCode();
      int num1 = (hashCode << 5) + hashCode ^ ((object) u == null ? 0 : u.GetHashCode());
      int num2 = (num1 << 5) + num1 ^ ((object) v == null ? 0 : v.GetHashCode());
      return (num2 << 5) + num2 ^ ((object) w == null ? 0 : w.GetHashCode());
    }

    [Pure]
    public static int Combine<T, U, V, W, X>(T t, U u, V v, W w, X x)
    {
      int hashCode = (object) t == null ? 0 : t.GetHashCode();
      int num1 = (hashCode << 5) + hashCode ^ ((object) u == null ? 0 : u.GetHashCode());
      int num2 = (num1 << 5) + num1 ^ ((object) v == null ? 0 : v.GetHashCode());
      int num3 = (num2 << 5) + num2 ^ ((object) w == null ? 0 : w.GetHashCode());
      return (num3 << 5) + num3 ^ ((object) x == null ? 0 : x.GetHashCode());
    }

    [Pure]
    public static int Combine<T, U, V, W, X, Y>(T t, U u, V v, W w, X x, Y y)
    {
      int hashCode = (object) t == null ? 0 : t.GetHashCode();
      int num1 = (hashCode << 5) + hashCode ^ ((object) u == null ? 0 : u.GetHashCode());
      int num2 = (num1 << 5) + num1 ^ ((object) v == null ? 0 : v.GetHashCode());
      int num3 = (num2 << 5) + num2 ^ ((object) w == null ? 0 : w.GetHashCode());
      int num4 = (num3 << 5) + num3 ^ ((object) x == null ? 0 : x.GetHashCode());
      return (num4 << 5) + num4 ^ ((object) y == null ? 0 : y.GetHashCode());
    }

    [Pure]
    public static int Combine<T, U, V, W, X, Y, Z>(T t, U u, V v, W w, X x, Y y, Z z)
    {
      int hashCode = (object) t == null ? 0 : t.GetHashCode();
      int num1 = (hashCode << 5) + hashCode ^ ((object) u == null ? 0 : u.GetHashCode());
      int num2 = (num1 << 5) + num1 ^ ((object) v == null ? 0 : v.GetHashCode());
      int num3 = (num2 << 5) + num2 ^ ((object) w == null ? 0 : w.GetHashCode());
      int num4 = (num3 << 5) + num3 ^ ((object) x == null ? 0 : x.GetHashCode());
      int num5 = (num4 << 5) + num4 ^ ((object) y == null ? 0 : y.GetHashCode());
      return (num5 << 5) + num5 ^ ((object) z == null ? 0 : z.GetHashCode());
    }
  }
}
