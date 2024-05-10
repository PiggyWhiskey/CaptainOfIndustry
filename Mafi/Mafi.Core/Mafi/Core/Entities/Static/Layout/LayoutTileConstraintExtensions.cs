// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutTileConstraintExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public static class LayoutTileConstraintExtensions
  {
    public static bool HasAllConstraints(
      this LayoutTileConstraint x,
      LayoutTileConstraint constraints)
    {
      return (x & constraints) == constraints;
    }

    public static bool HasAnyConstraints(
      this LayoutTileConstraint x,
      LayoutTileConstraint constraints)
    {
      return (x & constraints) != 0;
    }

    public static ColorRgba ToColor(this LayoutTileConstraint x)
    {
      if (x.HasAnyConstraints(LayoutTileConstraint.Ocean))
        return ColorRgba.Blue;
      return x.HasAnyConstraints(LayoutTileConstraint.Ground) ? ColorRgba.Brown : ColorRgba.DarkDarkGray;
    }
  }
}
