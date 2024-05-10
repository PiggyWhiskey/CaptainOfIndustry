// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.BuildInfo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity
{
  public static class BuildInfo
  {
    public const bool IS_DEBUG = false;
    public const bool IS_DEV_ONLY = false;
    public const bool IS_ALPHA_ONLY = true;

    public static BuildInfoData Data => new BuildInfoData(false, false, true, true, false);
  }
}
