// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.VehiclesAssigner.AssignedElementIcon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.VehiclesAssigner
{
  internal class AssignedElementIcon : IUiElement
  {
    internal int Index;
    private readonly Btn m_button;
    private readonly IconContainer m_icon;

    public GameObject GameObject => this.m_button.GameObject;

    public RectTransform RectTransform => this.m_button.RectTransform;

    internal AssignedElementIcon(
      UiBuilder builder,
      string iconPath,
      Action<int> onClick,
      Action<int> onRightClick = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      AssignedElementIcon assignedElementIcon = this;
      this.m_button = builder.NewBtn("VehicleIcon").OnClick((Action) (() => onClick(assignedElementIcon.Index)));
      if (onRightClick != null)
      {
        this.m_button.OnRightClick((Action) (() => onRightClick(assignedElementIcon.Index)));
        this.m_button.AddToolTip(Tr.RightClickToRemove);
      }
      this.m_icon = builder.NewIconContainer("Icon").PutTo<IconContainer>((IUiElement) this.m_button);
      if (iconPath.IsNotEmpty())
        this.m_icon.SetIcon(iconPath);
      IconContainer showIcon = builder.NewIconContainer("ShowIcon").SetIcon("Assets/Unity/UserInterface/General/Hide128.png").PutToRightTopOf<IconContainer>((IUiElement) this.m_button, 12.Vector2(), Offset.Top(-2f));
      this.m_button.SetOnMouseEnterLeaveActions((Action) (() => showIcon.Show<IconContainer>()), (Action) (() => showIcon.Hide<IconContainer>()));
      showIcon.Hide<IconContainer>();
    }

    public AssignedElementIcon SetIcon(string iconPath)
    {
      this.m_icon.SetIcon(iconPath);
      return this;
    }

    public class Cache : ViewsCacheHomogeneous<AssignedElementIcon>
    {
      public Cache(
        UiBuilder builder,
        string iconPath,
        Action<int> onClick,
        Action<int> onRightClick = null)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Func<AssignedElementIcon>) (() => new AssignedElementIcon(builder, iconPath, onClick, onRightClick)));
      }
    }
  }
}
