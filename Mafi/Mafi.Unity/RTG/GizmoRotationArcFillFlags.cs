// Decompiled with JetBrains decompiler
// Type: RTG.GizmoRotationArcFillFlags
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;

#nullable disable
namespace RTG
{
  [Flags]
  public enum GizmoRotationArcFillFlags
  {
    None = 0,
    Area = 1,
    ExtremitiesBorder = 2,
    ArcBorder = 4,
    All = ArcBorder | ExtremitiesBorder | Area, // 0x00000007
  }
}
