// Decompiled with JetBrains decompiler
// Type: Mafi.Collections.Tupple
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi.Collections
{
  public static class Tupple
  {
    public static Tupple<T1, T2, T3> Create<T1, T2, T3>(T1 first, T2 second, T3 third)
    {
      return new Tupple<T1, T2, T3>(first, second, third);
    }

    public static Tupple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(
      T1 first,
      T2 second,
      T3 third,
      T4 fourth)
    {
      return new Tupple<T1, T2, T3, T4>(first, second, third, fourth);
    }
  }
}
