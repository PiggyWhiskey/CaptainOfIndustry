﻿// Decompiled with JetBrains decompiler
// Type: RTG.QuaternionRef
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class QuaternionRef
  {
    private Quaternion _value;

    public Quaternion Value
    {
      get => this._value;
      set => this._value = value;
    }

    public QuaternionRef()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._value = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public QuaternionRef(Quaternion quat)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._value = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._value = quat;
    }
  }
}
