// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Audio.AudioInfo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Audio
{
  public struct AudioInfo
  {
    public readonly string PrefabPath;
    public readonly AudioChannel Channel;

    public AudioInfo(string prefabPath, AudioChannel channel)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.PrefabPath = prefabPath.CheckNotNullOrEmpty();
      this.Channel = channel;
    }
  }
}
