// Decompiled with JetBrains decompiler
// Type: Mafi.Random.Noise.INoise2DExtensions
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using Mafi.Core;

#nullable disable
namespace Mafi.Random.Noise
{
  public static class INoise2DExtensions
  {
    /// <summary>
    /// Returns numeric differentiation of this noise. This operation is relatively expensive as it requires four
    /// noise samples (three noise samples may be used instead for lower quality differentials).
    /// </summary>
    public static string SaveDebugImage(
      this INoise2D noise,
      string name,
      Vector2f from,
      Vector2f to,
      int imgWidth,
      int imgHeight,
      Fix32 blackValue,
      Fix32 whiteValue)
    {
      Rgb[] rowMajorData = new Rgb[imgWidth * imgHeight];
      Vector2f vector2f = to - from;
      Assert.That<Fix32>(vector2f.X).IsNotZero();
      Assert.That<Fix32>(vector2f.Y).IsNotZero();
      for (int numerator1 = 0; numerator1 < imgHeight; ++numerator1)
      {
        for (int numerator2 = 0; numerator2 < imgWidth; ++numerator2)
        {
          byte longFloored = (byte) (256 * (noise.GetValue(from + new Vector2f(Percent.FromRatio(numerator2, imgWidth).Apply(vector2f.X), Percent.FromRatio(numerator1, imgHeight).Apply(vector2f.Y))) - blackValue) / (whiteValue - blackValue)).Clamp((Fix64) 0L, (Fix64) (long) byte.MaxValue).ToLongFloored();
          rowMajorData[numerator1 * imgWidth + numerator2] = new Rgb(longFloored);
        }
      }
      string timestampedFilePath = new FileSystemHelper().GetTimestampedFilePath("_" + name + ".tga", FileType.Debug);
      TgaImageUtils.SaveTgaImage(rowMajorData, imgWidth, imgHeight, timestampedFilePath);
      return timestampedFilePath.Replace("\\", "/");
    }
  }
}
