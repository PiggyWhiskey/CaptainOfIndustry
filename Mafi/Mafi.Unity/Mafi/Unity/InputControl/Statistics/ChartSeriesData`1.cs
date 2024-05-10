// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.ChartSeriesData`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  internal class ChartSeriesData<T>
  {
    public readonly LocStrFormatted Label;
    public readonly string IconPath;
    public readonly ColorRgba? Color;
    public readonly T Data;

    public ChartSeriesData(ProductProto product, T data, ColorRgba? color = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Label = (LocStrFormatted) product.Strings.Name;
      this.IconPath = product.Graphics.IconPath;
      this.Data = data;
      this.Color = color;
    }

    public ChartSeriesData(string label, string iconPath, T data, ColorRgba? color = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Label = new LocStrFormatted(label);
      this.IconPath = iconPath;
      this.Color = color;
      this.Data = data;
    }

    public ChartSeriesData(LocStrFormatted label, string iconPath, T data, ColorRgba? color = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Label = label;
      this.IconPath = iconPath;
      this.Color = color;
      this.Data = data;
    }
  }
}
