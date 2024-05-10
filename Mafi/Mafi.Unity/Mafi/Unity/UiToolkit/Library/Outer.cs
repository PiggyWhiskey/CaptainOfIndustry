// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Outer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class Outer : UiComponent
  {
    public Option<string> WrapClassName;

    public VisualElement ElementToPlace => this.Element;

    public static Outer ShadowAll
    {
      get
      {
        return new Outer(new string[2]
        {
          Cls.outShadow,
          Cls.shadowAll
        });
      }
    }

    public static Outer ShadowBottom
    {
      get
      {
        return new Outer(new string[2]
        {
          Cls.outShadow,
          Cls.shadowBottom
        });
      }
    }

    public static Outer ShadowCutCorner
    {
      get
      {
        return new Outer(new string[2]
        {
          Cls.outShadow,
          Cls.shadowCutCorner
        });
      }
    }

    public static Outer EdgeShadowTop
    {
      get
      {
        return new Outer(new string[2]
        {
          Cls.edgeShadow,
          Cls.top
        });
      }
    }

    public static Outer EdgeShadowBottom
    {
      get
      {
        return new Outer(new string[2]
        {
          Cls.edgeShadow,
          Cls.bottom
        });
      }
    }

    public static Outer InnerShadowTopBottom
    {
      get
      {
        return new Outer(new string[3]
        {
          Cls.edgeShadow,
          Cls.bottom,
          Cls.top
        });
      }
    }

    public static Outer Panel
    {
      get => new Outer(new string[1]{ Cls.background }).WrapClass(Cls.panel);
    }

    public static Outer PanelTab
    {
      get => new Outer(new string[1]{ Cls.background }).WrapClass(Cls.panelTab);
    }

    public static Outer WindowBackground => (Outer) new Mafi.Unity.UiToolkit.Library.WindowBackground();

    public static Outer WindowShadow
    {
      get => new Outer(new string[1]{ Cls.windowShadow });
    }

    public static Outer WindowCloseButton
    {
      get => new Outer(new string[1]{ Cls.outer });
    }

    public static Outer WindowCornerButton
    {
      get => new Outer(new string[1]{ Cls.outer });
    }

    public Outer(params string[] classNames)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.IgnoreInputPicking<Outer>();
      foreach (string className in classNames)
        this.Class<Outer>(className);
    }

    public Outer WrapClass(string wrapClass)
    {
      string str = wrapClass;
      this.WrapClassName = str != null ? (Option<string>) str : Option<string>.None;
      return this;
    }
  }
}
