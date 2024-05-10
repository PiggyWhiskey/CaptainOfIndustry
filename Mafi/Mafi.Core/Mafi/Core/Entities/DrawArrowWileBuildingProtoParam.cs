// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.DrawArrowWileBuildingProtoParam
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  public class DrawArrowWileBuildingProtoParam : IProtoParam
  {
    public readonly float Scale;
    public readonly float AngleDegrees;

    public Type AllowedProtoType => typeof (LayoutEntityProto);

    public DrawArrowWileBuildingProtoParam(float scale, float angleDegrees = 0.0f)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Scale = scale;
      this.AngleDegrees = angleDegrees;
    }
  }
}
