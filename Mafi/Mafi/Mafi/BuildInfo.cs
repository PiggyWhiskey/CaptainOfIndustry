// Decompiled with JetBrains decompiler
// Type: Mafi.BuildInfo
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  public static class BuildInfo
  {
    public const bool IS_DEBUG = false;
    public const bool IS_DEV_ONLY = false;
    public const bool IS_ALPHA_ONLY = true;
    public const bool CHEATS_ENABLED = false;
    public const bool SHORT_DURATION_BUILD = false;
    public static int COUNT;

    public static BuildInfoData Data => new BuildInfoData(false, false, true, true, false);
  }
}
