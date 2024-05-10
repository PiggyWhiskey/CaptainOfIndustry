// Decompiled with JetBrains decompiler
// Type: Mafi.Core.BuildInfo
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core
{
  public static class BuildInfo
  {
    public const bool IS_DEBUG = false;
    public const bool IS_DEV_ONLY = false;
    public const bool IS_ALPHA_ONLY = true;
    public const bool IS_RELEASE_CHEATS = false;

    public static BuildInfoData Data => new BuildInfoData(false, false, true, true, false);
  }
}
