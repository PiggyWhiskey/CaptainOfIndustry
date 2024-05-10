// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.AlertIndicatorNotification
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class AlertIndicatorNotification : AlertIndicator
  {
    private Proto.ID? m_tutorial;

    public AlertIndicatorNotification(
      UiBuilder builder,
      InspectorContext context,
      IUiElement parent)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder, context, parent);
    }

    public void SetNotification(INotification notif)
    {
      this.m_tutorial = !(notif.Proto is EntityNotificationProto proto) ? new Proto.ID?() : proto.Tutorial;
      this.AlertTooltip.SetNotification(notif, this.m_tutorial.HasValue);
    }

    protected override void OnClick()
    {
      if (!this.m_tutorial.HasValue || !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      this.Context.MessagesCenter.ShowMessage(this.m_tutorial.Value);
    }
  }
}
