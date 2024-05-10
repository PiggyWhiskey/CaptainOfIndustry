// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Audio.AudioChannel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.Audio
{
  /// <summary>Represents IDs of groups in Unity's audio mixer.</summary>
  /// <remarks>
  /// IDs must have default values since they are used as indices to arrays
  /// 
  /// If you are adding new channel here you need to create exposed variable via Unity. The name of the variable
  /// must exactly match "{ChannelName}Volume" (right click on volume of the channel in mixer, expose, then rename).
  /// See https://docs.unity3d.com/Manual/AudioMixerInspectors.html
  /// </remarks>
  public enum AudioChannel
  {
    /// <summary>
    /// Controls master volume of the entire game, do not put audio directly here.
    /// </summary>
    Master,
    Music,
    /// <summary>
    /// EffectsGroup controls sub-groups. Don't put audio directly here.
    /// </summary>
    EffectsGroup,
    UserInterface,
    Machines,
    Weather,
  }
}
