// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.IAreaToolbox
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;

#nullable disable
namespace Mafi.Unity.Mine
{
  public interface IAreaToolbox
  {
    event Action<AreaMode> OnModeChanged;

    void SetMode(AreaMode mode);

    void SetOnRotate(Action action);

    void SetOnIncreaseElevation(Action action);

    void SetOnDecreaseElevation(Action action);

    void Show();

    void Hide();
  }
}
