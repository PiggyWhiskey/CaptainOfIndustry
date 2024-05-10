// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.IncompatibleSaveVersionException
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Serialization
{
  public class IncompatibleSaveVersionException : Exception
  {
    public readonly int SaveFileVersion;

    public IncompatibleSaveVersionException(int saveFileVersion, int minVersion, int maxVersion)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector(string.Format("Incompatible save version {0}, min compatible is {1}, ", (object) saveFileVersion, (object) minVersion) + string.Format("max compatible is {0}", (object) maxVersion));
      this.SaveFileVersion = saveFileVersion;
    }
  }
}
