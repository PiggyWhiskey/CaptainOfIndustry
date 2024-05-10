// Decompiled with JetBrains decompiler
// Type: RTG.UniversalGizmoConfig
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public class UniversalGizmoConfig : Settings
  {
    [SerializeField]
    private UniversalGizmoSettingsCategory _inheritCategory;
    [SerializeField]
    private UniversalGizmoSettingsType _inheritType;
    [SerializeField]
    private UniversalGizmoSettingsCategory _displayCategory;

    public UniversalGizmoSettingsCategory InheritCategory
    {
      get => this._inheritCategory;
      set => this._inheritCategory = value;
    }

    public UniversalGizmoSettingsType InheritType
    {
      get => this._inheritType;
      set => this._inheritType = value;
    }

    public UniversalGizmoSettingsCategory DisplayCategory
    {
      get => this._displayCategory;
      set => this._displayCategory = value;
    }

    public UniversalGizmoConfig()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._inheritType = UniversalGizmoSettingsType.LookAndFeel3D;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
