// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.Md5Utils
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Mafi.Utils
{
  public static class Md5Utils
  {
    [ThreadStatic]
    private static MD5 s_md5;

    public static ulong ComputeMd5(byte[] buffer)
    {
      MD5 md5 = Md5Utils.s_md5;
      if (md5 == null)
        Md5Utils.s_md5 = md5 = MD5.Create();
      return BitConverter.ToUInt64(md5.ComputeHash(buffer), 0);
    }

    public static ulong ComputeMd5(Stream stream)
    {
      MD5 md5 = Md5Utils.s_md5;
      if (md5 == null)
        Md5Utils.s_md5 = md5 = MD5.Create();
      return BitConverter.ToUInt64(md5.ComputeHash(stream), 0);
    }

    public static ulong ComputeMd5FromFile(string filePath)
    {
      using (FileStream fileStream = File.OpenRead(filePath))
        return Md5Utils.ComputeMd5((Stream) fileStream);
    }
  }
}
