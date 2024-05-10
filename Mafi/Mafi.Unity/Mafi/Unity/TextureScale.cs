// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.TextureScale
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Threading;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public static class TextureScale
  {
    private static Color[] texColors;
    private static Color[] newColors;
    private static int w;
    private static float ratioX;
    private static float ratioY;
    private static int w2;
    private static int finishCount;
    private static Mutex mutex;

    public static void Bilinear(ref Texture2D tex, int newWidth, int newHeight)
    {
      TextureScale.ThreadedScale(ref tex, newWidth, newHeight, true);
    }

    private static void ThreadedScale(
      ref Texture2D tex,
      int newWidth,
      int newHeight,
      bool useBilinear)
    {
      TextureScale.texColors = tex.GetPixels();
      TextureScale.newColors = new Color[newWidth * newHeight];
      if (useBilinear)
      {
        TextureScale.ratioX = (float) (1.0 / ((double) newWidth / (double) (tex.width - 1)));
        TextureScale.ratioY = (float) (1.0 / ((double) newHeight / (double) (tex.height - 1)));
      }
      else
      {
        TextureScale.ratioX = (float) tex.width / (float) newWidth;
        TextureScale.ratioY = (float) tex.height / (float) newHeight;
      }
      TextureScale.w = tex.width;
      TextureScale.w2 = newWidth;
      int num1 = Mathf.Min(SystemInfo.processorCount, newHeight);
      int num2 = newHeight / num1;
      TextureScale.finishCount = 0;
      if (TextureScale.mutex == null)
        TextureScale.mutex = new Mutex(false);
      if (num1 > 1)
      {
        int num3;
        for (num3 = 0; num3 < num1 - 1; ++num3)
        {
          TextureScale.ThreadData state = new TextureScale.ThreadData(num2 * num3, num2 * (num3 + 1));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ThreadPool.QueueUserWorkItem(TextureScale.\u003C\u003EO.\u003C0\u003E__BilinearScale ?? (TextureScale.\u003C\u003EO.\u003C0\u003E__BilinearScale = new WaitCallback(TextureScale.BilinearScale)), (object) state);
        }
        TextureScale.ThreadData threadData = new TextureScale.ThreadData(num2 * num3, newHeight);
        if (useBilinear)
          TextureScale.BilinearScale((object) threadData);
        else
          TextureScale.PointScale((object) threadData);
        while (TextureScale.finishCount < num1)
          Thread.Sleep(1);
      }
      else
      {
        TextureScale.ThreadData threadData = new TextureScale.ThreadData(0, newHeight);
        if (useBilinear)
          TextureScale.BilinearScale((object) threadData);
        else
          TextureScale.PointScale((object) threadData);
      }
      if (tex.format == TextureFormat.RGBA32)
      {
        tex.Reinitialize(newWidth, newHeight);
        tex.SetPixels(TextureScale.newColors);
        tex.Apply();
      }
      else if (tex.format == TextureFormat.RGB24)
      {
        tex = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, tex.mipmapCount > 0);
        tex.SetPixels(TextureScale.newColors);
        tex.Apply();
      }
      else
      {
        if (tex.format != TextureFormat.DXT5)
          throw new NotSupportedException(string.Format("Resizing texture '{0}' in format '{1}' is not supported.", (object) tex.name, (object) tex.format));
        tex = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, tex.mipmapCount > 0);
        tex.SetPixels(TextureScale.newColors);
        tex.Apply();
        tex.Compress(false);
      }
      TextureScale.texColors = (Color[]) null;
      TextureScale.newColors = (Color[]) null;
    }

    public static void BilinearScale(object obj)
    {
      TextureScale.ThreadData threadData = (TextureScale.ThreadData) obj;
      for (int start = threadData.start; start < threadData.end; ++start)
      {
        int num1 = (int) Mathf.Floor((float) start * TextureScale.ratioY);
        int num2 = num1 * TextureScale.w;
        int num3 = (num1 + 1) * TextureScale.w;
        int num4 = start * TextureScale.w2;
        for (int index = 0; index < TextureScale.w2; ++index)
        {
          int num5 = (int) Mathf.Floor((float) index * TextureScale.ratioX);
          float num6 = (float) index * TextureScale.ratioX - (float) num5;
          TextureScale.newColors[num4 + index] = TextureScale.ColorLerpUnclamped(TextureScale.ColorLerpUnclamped(TextureScale.texColors[num2 + num5], TextureScale.texColors[num2 + num5 + 1], num6), TextureScale.ColorLerpUnclamped(TextureScale.texColors[num3 + num5], TextureScale.texColors[num3 + num5 + 1], num6), (float) start * TextureScale.ratioY - (float) num1);
        }
      }
      TextureScale.mutex.WaitOne();
      ++TextureScale.finishCount;
      TextureScale.mutex.ReleaseMutex();
    }

    public static void PointScale(object obj)
    {
      TextureScale.ThreadData threadData = (TextureScale.ThreadData) obj;
      for (int start = threadData.start; start < threadData.end; ++start)
      {
        int num1 = (int) ((double) TextureScale.ratioY * (double) start) * TextureScale.w;
        int num2 = start * TextureScale.w2;
        for (int index = 0; index < TextureScale.w2; ++index)
          TextureScale.newColors[num2 + index] = TextureScale.texColors[(int) ((double) num1 + (double) TextureScale.ratioX * (double) index)];
      }
      TextureScale.mutex.WaitOne();
      ++TextureScale.finishCount;
      TextureScale.mutex.ReleaseMutex();
    }

    private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
    {
      return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
    }

    public class ThreadData
    {
      public int start;
      public int end;

      public ThreadData(int s, int e)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.start = s;
        this.end = e;
      }
    }
  }
}
