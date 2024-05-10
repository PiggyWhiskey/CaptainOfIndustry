// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Particles.DustinessData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Particles
{
  public readonly struct DustinessData
  {
    public readonly float Dustiness;
    public readonly Color DustColor;

    public DustinessData(float dustiness, Color dustColor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Dustiness = dustiness;
      this.DustColor = dustColor;
    }

    public DustinessData Lerp(DustinessData data, float weight)
    {
      return new DustinessData(this.Dustiness.Lerp(data.Dustiness, weight), Color.LerpUnclamped(this.DustColor, data.DustColor, weight));
    }
  }
}
