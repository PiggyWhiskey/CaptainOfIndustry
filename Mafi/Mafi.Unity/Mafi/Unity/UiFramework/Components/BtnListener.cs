// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.BtnListener
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  /// <summary>Unity listener script to be attached to a button GO.</summary>
  /// <remarks>
  /// Why do we have this object? Previously we created Event Trigger for each event we needed. But that dude ate all
  /// other events so for instance scrolling events were not propagated. As the remedy we are using this object. See
  /// more in http://answers.unity3d.com/questions/902929/scroll-not-working-when-elements-inside-have-click.html
  /// </remarks>
  public class BtnListener : UiElementListener, IPointerClickHandler, IEventSystemHandler
  {
    private Button m_button;
    private bool m_clickPending;
    private long m_msOnLastClick;

    public Option<Action> DoubleClickAction { get; set; }

    public Option<Action> LeftClickActionWhenDisabled { get; set; }

    [PublicAPI("Unity MB API")]
    private void Awake() => this.m_button = this.gameObject.GetComponent<Button>();

    public override void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button == PointerEventData.InputButton.Left)
      {
        if (!this.m_button.interactable)
        {
          if (!this.LeftClickActionWhenDisabled.HasValue)
            return;
          this.LeftClickActionWhenDisabled.Value();
        }
        else
        {
          Option<Action> option = this.DoubleClickAction;
          if (option.HasValue)
          {
            if (this.m_clickPending && ((long) Environment.TickCount - this.m_msOnLastClick).Abs() < 500L)
            {
              this.m_clickPending = false;
              this.DoubleClickAction.Value();
            }
            else
            {
              this.m_clickPending = true;
              this.LeftClickAction.Value();
              this.m_msOnLastClick = (long) Environment.TickCount;
            }
          }
          else
          {
            option = this.LeftClickAction;
            if (!option.HasValue)
              return;
            this.LeftClickAction.Value();
          }
        }
      }
      else
      {
        if (eventData.button != PointerEventData.InputButton.Right || !this.RightClickAction.HasValue)
          return;
        this.RightClickAction.Value();
      }
    }

    public BtnListener()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
