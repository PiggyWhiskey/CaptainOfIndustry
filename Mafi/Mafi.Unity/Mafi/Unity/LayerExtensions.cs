// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.LayerExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity
{
  public static class LayerExtensions
  {
    public static int ToId(this Layer layer) => (int) layer;

    public static int ToMask(this Layer layer) => 1 << (int) (layer & (Layer) 31);
  }
}
