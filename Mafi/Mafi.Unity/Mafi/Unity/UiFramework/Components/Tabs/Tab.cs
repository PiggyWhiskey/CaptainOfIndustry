// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Tabs.Tab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Syncers;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Tabs
{
  /// <summary>
  /// Used in <see cref="T:Mafi.Unity.UiFramework.Components.Tabs.TabsContainer" />.
  /// </summary>
  public abstract class Tab : View
  {
    public int AvailableWidth { get; set; }

    public int ViewportHeight { get; set; }

    protected Tab(string id, SyncFrequency syncFrequency = SyncFrequency.Critical)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, syncFrequency);
    }
  }
}
