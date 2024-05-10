// Decompiled with JetBrains decompiler
// Type: RTG.GizmoBehaviour
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public abstract class GizmoBehaviour : IGizmoBehaviour
  {
    protected Gizmo _gizmo;
    protected bool _isEnabled;

    public Gizmo Gizmo => this._gizmo;

    public bool IsEnabled => this._isEnabled;

    public void Init_SystemCall(GizmoBehaviorInitParams initParams)
    {
      this._gizmo = initParams.Gizmo;
    }

    public void SetEnabled(bool enabled)
    {
      if (enabled == this._isEnabled)
        return;
      if (enabled)
      {
        this._isEnabled = enabled;
        this.OnEnabled();
      }
      else
      {
        this._isEnabled = false;
        this.OnDisabled();
      }
    }

    public virtual void OnAttached()
    {
    }

    public virtual void OnDetached()
    {
    }

    public virtual void OnGizmoEnabled()
    {
    }

    public virtual void OnGizmoDisabled()
    {
    }

    public virtual void OnEnabled()
    {
    }

    public virtual void OnDisabled()
    {
    }

    public virtual void OnGizmoHandlePicked(int handleId)
    {
    }

    public virtual bool OnGizmoCanBeginDrag(int handleId) => true;

    public virtual void OnGizmoAttemptHandleDragBegin(int handleId)
    {
    }

    public virtual void OnGizmoDragBegin(int handleId)
    {
    }

    public virtual void OnGizmoDragUpdate(int handleId)
    {
    }

    public virtual void OnGizmoDragEnd(int handleId)
    {
    }

    public virtual void OnGizmoHoverEnter(int handleId)
    {
    }

    public virtual void OnGizmoHoverExit(int handleId)
    {
    }

    public virtual void OnGizmoUpdateBegin()
    {
    }

    public virtual void OnGizmoUpdateEnd()
    {
    }

    public virtual void OnGUI()
    {
    }

    public virtual void OnGizmoRender(Camera camera)
    {
    }

    protected void CheckRequiredBehaviours(List<System.Type> reqBehaviourTypes)
    {
      foreach (System.Type reqBehaviourType in reqBehaviourTypes)
      {
        if (this.Gizmo.GetFirstBehaviourOfType(reqBehaviourType) == null)
        {
          this.ThrowReqBehaviourExeception(reqBehaviourType);
          break;
        }
      }
    }

    private void ThrowReqBehaviourExeception(System.Type reqBehaviorType)
    {
      if (Application.isEditor)
      {
        Debug.Break();
        throw new UnityException(this.GetType().ToString() + " requires a behaviour of type " + reqBehaviorType.ToString());
      }
    }

    protected GizmoBehaviour()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
