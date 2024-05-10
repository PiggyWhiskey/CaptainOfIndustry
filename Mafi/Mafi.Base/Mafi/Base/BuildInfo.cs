// Decompiled with JetBrains decompiler
// Type: Mafi.Base.BuildInfo
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

#nullable disable
namespace Mafi.Base
{
  public static class BuildInfo
  {
    public const bool IS_DEBUG = false;
    public const bool IS_DEV_ONLY = false;
    public const bool IS_ALPHA_ONLY = true;

    public static BuildInfoData Data => new BuildInfoData(false, false, true, true, false);
  }
}
