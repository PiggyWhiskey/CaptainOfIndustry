// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TileSurfaceTextureSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using System;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Terrain
{
  public readonly struct TileSurfaceTextureSpec
  {
    public static readonly TileSurfaceTextureSpec Empty;
    public readonly ImmutableArray<string> AlbedoTexturePaths;
    public readonly ImmutableArray<string> NormalTexturePaths;
    public readonly ImmutableArray<string> SmoothMetalTexturePaths;

    public TileSurfaceTextureSpec(FieldInfo[] surfaceTextures, bool empty = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Lyst<string> lyst1 = new Lyst<string>();
      Lyst<string> lyst2 = new Lyst<string>();
      Lyst<string> lyst3 = new Lyst<string>();
      foreach (FieldInfo surfaceTexture in surfaceTextures)
      {
        string str = !(surfaceTexture.FieldType != typeof (string)) ? (string) surfaceTexture.GetRawConstantValue() : throw new ProtoBuilderException("Texture not as string");
        if (str.IndexOf("albedo", StringComparison.OrdinalIgnoreCase) >= 0)
          lyst1.Add(str);
        else if (str.IndexOf("normal", StringComparison.OrdinalIgnoreCase) >= 0)
          lyst2.Add(str);
        else if (str.IndexOf("smoothmetal", StringComparison.OrdinalIgnoreCase) >= 0)
          lyst3.Add(str);
        else
          Log.Error("Unknown texture type found at " + str);
      }
      this.AlbedoTexturePaths = lyst1.ToImmutableArrayAndClear();
      this.NormalTexturePaths = lyst2.ToImmutableArrayAndClear();
      this.SmoothMetalTexturePaths = lyst3.ToImmutableArrayAndClear();
      if (empty)
        return;
      Assert.That<int>(this.AlbedoTexturePaths.Length).IsEqualTo(8);
      Assert.That<int>(this.NormalTexturePaths.Length).IsEqualTo(8);
      Assert.That<int>(this.SmoothMetalTexturePaths.Length).IsEqualTo(8);
    }

    public TileSurfaceTextureSpec(
      ImmutableArray<string> albedoTexturePaths,
      ImmutableArray<string> normalTexturePaths,
      ImmutableArray<string> smoothMetalTexturePaths,
      bool empty = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.AlbedoTexturePaths = albedoTexturePaths;
      this.NormalTexturePaths = normalTexturePaths;
      this.SmoothMetalTexturePaths = smoothMetalTexturePaths;
      if (empty)
        return;
      Assert.That<int>(this.AlbedoTexturePaths.Length).IsEqualTo(8);
      Assert.That<int>(this.NormalTexturePaths.Length).IsEqualTo(8);
      Assert.That<int>(this.SmoothMetalTexturePaths.Length).IsEqualTo(8);
    }

    static TileSurfaceTextureSpec()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TileSurfaceTextureSpec.Empty = new TileSurfaceTextureSpec(Array.Empty<FieldInfo>(), true);
    }
  }
}
