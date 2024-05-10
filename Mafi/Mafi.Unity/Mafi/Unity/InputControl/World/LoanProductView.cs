// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.LoanProductView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  internal class LoanProductView : IUiElement
  {
    private readonly IconContainer m_container;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    internal LoanProductView(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_container = builder.NewIconContainer("Icon");
    }

    public void SetData(ProductProto product, bool isUnlocked)
    {
      this.m_container.SetIcon(product.IconPath);
      this.m_container.SetColor(isUnlocked ? ColorRgba.White : ColorRgba.White.SetA((byte) 70));
    }
  }
}
