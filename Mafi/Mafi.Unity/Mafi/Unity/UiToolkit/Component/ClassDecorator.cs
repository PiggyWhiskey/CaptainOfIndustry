// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.ClassDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class ClassDecorator : IClassDecorator
  {
    private static readonly ClassDecorator s_instance;
    private VisualElement m_element;

    public static ClassDecorator GetSharedInstance(VisualElement element)
    {
      ClassDecorator.s_instance.m_element = element;
      return ClassDecorator.s_instance;
    }

    public void AddClass([CanBeNull] string className)
    {
      if (className == null)
        return;
      this.m_element.AddToClassList(className);
    }

    public void EnableClass([CanBeNull] string className, bool isEnabled)
    {
      if (className == null)
        return;
      this.m_element.EnableInClassList(className, isEnabled);
    }

    public void RemoveClass([CanBeNull] string className)
    {
      if (className == null)
        return;
      this.m_element.RemoveFromClassList(className);
    }

    public bool HasClass(string className) => this.m_element.ClassListContains(className);

    public ClassDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ClassDecorator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ClassDecorator.s_instance = new ClassDecorator();
    }
  }
}
