// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Numerics.ExtraChecks
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Numerics
{
  /// <summary>
  /// Extra custom checks that are not generated. Consider adding these to the generator.
  /// </summary>
  public static class ExtraChecks
  {
    [Pure]
    public static RelTile1i CheckGreaterThan(this RelTile1i value, RelTile1i minValue)
    {
      if (!(value <= minValue))
        return value;
      Log.Error(string.Format("CHECK FAIL: Value {0} is not greater than {1}.", (object) value, (object) minValue));
      return minValue + RelTile1i.One;
    }
  }
}
