// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Canvass
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UserInterface;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class Canvass : IUiElement
  {
    private readonly Canvas m_canvas;

    public GameObject GameObject => this.m_canvas.gameObject;

    public RectTransform RectTransform { get; }

    public float ScaleFactor => this.m_canvas.scaleFactor;

    public bool IsLargeScreen => (double) this.m_canvas.scaleFactor > 1.0;

    public Canvass(string name)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      GameObject gameObject = new GameObject(name);
      this.m_canvas = gameObject.AddComponent<Canvas>();
      this.RectTransform = gameObject.GetComponent<RectTransform>();
    }

    public Canvass SetRenderMode(RenderMode renderMode)
    {
      this.m_canvas.renderMode = renderMode;
      return this;
    }

    /// <summary>
    /// The greater the number the greater priority to be on top.
    /// </summary>
    public Canvass SetSortOrder(int order)
    {
      this.m_canvas.sortingOrder = order;
      return this;
    }

    public Canvass SetConstantPixelSize()
    {
      CanvasScaler canvasScaler = this.GameObject.AddComponent<CanvasScaler>();
      canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
      canvasScaler.scaleFactor = UiScaleHelper.GetCurrentScaleFloat();
      return this;
    }

    public Canvass SetConstantPhysicalSize(CanvasScaler.Unit physicalUnit)
    {
      CanvasScaler canvasScaler = this.GameObject.AddComponent<CanvasScaler>();
      canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPhysicalSize;
      canvasScaler.physicalUnit = physicalUnit;
      return this;
    }

    public Canvass MakeInteractive()
    {
      this.GameObject.AddComponent<GraphicRaycaster>();
      return this;
    }

    public void Destroy() => this.GameObject.Destroy();
  }
}
