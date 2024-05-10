// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UnityTag
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity
{
  /// <summary>
  /// Helper enum of all common Unity tags. Use <see cref="M:Mafi.Unity.UnityTagExtensions.HasTag(UnityEngine.GameObject,Mafi.Unity.UnityTag)" /> to test these tags.
  /// </summary>
  /// <remarks>
  /// Since Unity does not support multi-tags, we workaround it with tags concatenated to string and define all needed
  /// combinations. Note that new tags and new used combinations has to be added in unity editor.
  /// 
  /// Tags are intentionally short since we test them with <see cref="M:System.String.Contains(System.String)" /> and testing usually happens
  /// multiple times per frame.
  /// </remarks>
  public enum UnityTag
  {
    /// <summary>
    /// No Highlight: Game object marked with this tag will not be highlighted.
    /// It also prevents setting materials (such as making it blue before it gets built).
    /// </summary>
    NoHi,
    /// <summary>
    /// No Highlight: Game object marked with this tag will not be highlighted.
    /// This does not prevent setting materials (e.g. making it blue until built).
    /// </summary>
    NoOnlyHi,
    /// <summary>
    /// Pick Parent: Picker will pick parent object instead of this one.
    /// </summary>
    PiPa,
    /// <summary>Marked GOs will not be rendered in the icon.</summary>
    HdeIcn,
    /// <summary>
    /// Marked GOs will not be cleared on scene change (eg. music)
    /// </summary>
    Persistent,
  }
}
