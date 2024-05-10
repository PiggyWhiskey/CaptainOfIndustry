// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.PillarVisualsSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  public readonly struct PillarVisualsSpec
  {
    public readonly PooledArray<PillarLayerSpec> Layers;
    public readonly Tile3i BasePosition;
    public readonly bool IsConstructed;

    public PillarVisualsSpec(
      Tile3i basePosition,
      PooledArray<PillarLayerSpec> layers,
      bool isConstructed)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Layers = layers;
      this.IsConstructed = isConstructed;
      this.BasePosition = basePosition;
    }

    public void ReturnToPool() => this.Layers.ReturnToPool();
  }
}
