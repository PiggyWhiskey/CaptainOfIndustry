// Decompiled with JetBrains decompiler
// Type: Mafi.ObjectExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi
{
  public static class ObjectExtensions
  {
    public static string ToStringSafe(this object obj)
    {
      if (obj == null)
        return "null";
      try
      {
        return obj.ToString();
      }
      catch (Exception ex)
      {
        return string.Format("{0}.ToString() threw {1}", (object) obj.GetType(), (object) ex.GetType().Name);
      }
    }
  }
}
