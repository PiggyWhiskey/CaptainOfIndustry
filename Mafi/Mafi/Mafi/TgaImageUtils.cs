// Decompiled with JetBrains decompiler
// Type: Mafi.TgaImageUtils
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System.IO;
using System.Text;

#nullable disable
namespace Mafi
{
  public static class TgaImageUtils
  {
    /// <summary>
    /// Saves given row-major data as a TGA image. Note that the data Y axis is bottom to top.
    /// </summary>
    public static void SaveTgaImage(Rgb[] rowMajorData, int width, int height, string outputPath)
    {
      using (Stream outputStream = (Stream) File.Open(outputPath, FileMode.Create))
        TgaImageUtils.SaveTgaImage(rowMajorData, width, height, outputStream);
    }

    /// <summary>
    /// Saves given row-major data as a TGA image. Note that the data Y axis is bottom to top.
    /// </summary>
    public static void SaveTgaImage(
      Rgb[] rowMajorData,
      int width,
      int height,
      Stream outputStream)
    {
      byte[] buffer = new byte[18];
      buffer[2] = (byte) 2;
      buffer[12] = (byte) (width & (int) byte.MaxValue);
      buffer[13] = (byte) (width >> 8 & (int) byte.MaxValue);
      buffer[14] = (byte) (height & (int) byte.MaxValue);
      buffer[15] = (byte) (height >> 8 & (int) byte.MaxValue);
      buffer[16] = (byte) 24;
      using (BinaryWriter binaryWriter = new BinaryWriter(outputStream, Encoding.ASCII))
      {
        binaryWriter.Write(buffer);
        for (int index1 = 0; index1 < height; ++index1)
        {
          for (int index2 = 0; index2 < width; ++index2)
          {
            Rgb rgb = rowMajorData[index1 * width + index2];
            binaryWriter.Write(rgb.B);
            binaryWriter.Write(rgb.G);
            binaryWriter.Write(rgb.R);
          }
        }
        binaryWriter.Write("\0\0\0\0\0\0\0\0TRUEVISION-XFILE.");
      }
    }

    public static void SaveTgaImage(
      ColorRgba[] rowMajorData,
      int width,
      int height,
      string outputPath)
    {
      using (Stream outputStream = (Stream) File.Open(outputPath, FileMode.Create))
        TgaImageUtils.SaveTgaImage(rowMajorData, width, height, outputStream);
    }

    public static void SaveTgaImage(
      ColorRgba[] rowMajorData,
      int width,
      int height,
      Stream outputStream)
    {
      byte[] buffer = new byte[18];
      buffer[2] = (byte) 2;
      buffer[12] = (byte) (width & (int) byte.MaxValue);
      buffer[13] = (byte) (width >> 8 & (int) byte.MaxValue);
      buffer[14] = (byte) (height & (int) byte.MaxValue);
      buffer[15] = (byte) (height >> 8 & (int) byte.MaxValue);
      buffer[16] = (byte) 32;
      using (BinaryWriter binaryWriter = new BinaryWriter(outputStream, Encoding.ASCII))
      {
        binaryWriter.Write(buffer);
        for (int index1 = 0; index1 < height; ++index1)
        {
          for (int index2 = 0; index2 < width; ++index2)
          {
            ColorRgba colorRgba = rowMajorData[index1 * width + index2];
            binaryWriter.Write(colorRgba.B);
            binaryWriter.Write(colorRgba.G);
            binaryWriter.Write(colorRgba.R);
            binaryWriter.Write(colorRgba.A);
          }
        }
        binaryWriter.Write("\0\0\0\0\0\0\0\0TRUEVISION-XFILE.");
      }
    }
  }
}
