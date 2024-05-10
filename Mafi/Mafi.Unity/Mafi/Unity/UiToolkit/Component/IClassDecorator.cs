// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.IClassDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public interface IClassDecorator
  {
    void AddClass(string className);

    void RemoveClass(string className);

    void EnableClass(string className, bool isEnabled);

    bool HasClass(string className);
  }
}
