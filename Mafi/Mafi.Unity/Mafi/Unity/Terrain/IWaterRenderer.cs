// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.IWaterRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.Terrain
{
  public interface IWaterRenderer
  {
    bool TryInitializeAndRegister(WaterRendererManager manager);

    void UnregisterAndDispose();

    void SetSpecularIntensity(float percent);

    void SetLightCookieEnabled(bool enabled);

    void NotifyChunkUpdated(Chunk2i chunk);

    void SetOpacity(float opacity);

    void SetOceanRenderingState(bool isEnabled);
  }
}
