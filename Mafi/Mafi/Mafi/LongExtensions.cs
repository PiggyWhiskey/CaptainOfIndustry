// Decompiled with JetBrains decompiler
// Type: Mafi.LongExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Mafi
{
  [DebuggerStepThrough]
  public static class LongExtensions
  {
    public static Fix64 ToFix64(this long value) => Fix64.FromLong(value);

    public static Fix32 SqrtToFix32(this long value) => Fix32.FromDouble(Math.Sqrt((double) value));
  }
}
