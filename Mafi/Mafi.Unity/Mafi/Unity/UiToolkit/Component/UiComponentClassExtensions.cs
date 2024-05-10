// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentClassExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentClassExtensions
  {
    public static T Class<T>(this T component, string className) where T : IComponentWithClass
    {
      component.GetClassDecorator().AddClass(className);
      return component;
    }

    public static T Class<T>(this T component, string className1, string className2) where T : IComponentWithClass
    {
      IClassDecorator classDecorator = component.GetClassDecorator();
      classDecorator.AddClass(className1);
      classDecorator.AddClass(className2);
      return component;
    }

    public static T Class<T>(
      this T component,
      string className1,
      string className2,
      string className3)
      where T : IComponentWithClass
    {
      IClassDecorator classDecorator = component.GetClassDecorator();
      classDecorator.AddClass(className1);
      classDecorator.AddClass(className2);
      classDecorator.AddClass(className3);
      return component;
    }

    public static T ClassIff<T>(this T component, string className, bool isEnabled) where T : IComponentWithClass
    {
      component.GetClassDecorator().EnableClass(className, isEnabled);
      return component;
    }

    public static T ClassRemove<T>(this T component, string className) where T : IComponentWithClass
    {
      component.GetClassDecorator().RemoveClass(className);
      return component;
    }

    public static T ClassRemove<T>(this T component, string className1, string className2) where T : IComponentWithClass
    {
      component.GetClassDecorator().RemoveClass(className1);
      component.GetClassDecorator().RemoveClass(className2);
      return component;
    }

    public static bool HasClass<T>(this T component, string className) where T : IComponentWithClass
    {
      return component.GetClassDecorator().HasClass(className);
    }
  }
}
