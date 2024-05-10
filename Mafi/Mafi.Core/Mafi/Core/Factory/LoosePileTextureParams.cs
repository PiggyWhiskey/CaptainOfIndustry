// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.LoosePileTextureParams
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Factory
{
  public readonly struct LoosePileTextureParams
  {
    public static readonly LoosePileTextureParams Default;
    /// <summary>Scale multiplier of the pile texture.</summary>
    public readonly float Scale;
    /// <summary>Offset applied to the pile texture.</summary>
    public readonly float OffsetX;
    public readonly float OffsetY;

    public LoosePileTextureParams(float scale, float offsetX = 0.0f, float offsetY = 0.0f)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Scale = scale;
      this.OffsetX = offsetX;
      this.OffsetY = offsetY;
    }

    static LoosePileTextureParams()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      LoosePileTextureParams.Default = new LoosePileTextureParams(1f);
    }
  }
}
