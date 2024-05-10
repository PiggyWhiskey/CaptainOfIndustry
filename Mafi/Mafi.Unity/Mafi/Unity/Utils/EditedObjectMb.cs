// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.EditedObjectMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  public sealed class EditedObjectMb : MonoBehaviour
  {
    private ImmutableArray<FieldInfo> m_fieldsCache;
    private ImmutableArray<PropertyInfo> m_propertiesCache;

    public event Action<object> OnObjectEdited;

    public Option<object> EditedObj { get; private set; }

    public ImmutableArray<FieldInfo> Fields
    {
      get
      {
        if (this.m_fieldsCache.IsNotValid)
        {
          if (this.EditedObj.IsNone)
            return ImmutableArray<FieldInfo>.Empty;
          this.m_fieldsCache = ((ICollection<FieldInfo>) this.EditedObj.Value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)).ToImmutableArray<FieldInfo>();
        }
        return this.m_fieldsCache;
      }
    }

    public ImmutableArray<PropertyInfo> Properties
    {
      get
      {
        if (this.m_propertiesCache.IsNotValid)
        {
          if (this.EditedObj.IsNone)
            return ImmutableArray<PropertyInfo>.Empty;
          this.m_propertiesCache = ((ICollection<PropertyInfo>) this.EditedObj.Value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)).ToImmutableArray<PropertyInfo>();
        }
        return this.m_propertiesCache;
      }
    }

    public Option<Mafi.Core.Prototypes.ProtosDb> ProtosDb { get; private set; }

    public Option<IRandom> Random { get; private set; }

    public void Initialize(object editedObj, Mafi.Core.Prototypes.ProtosDb protosDb = null, IRandom random = null)
    {
      this.EditedObj = (Option<object>) editedObj;
      this.ProtosDb = (Option<Mafi.Core.Prototypes.ProtosDb>) protosDb;
      this.Random = random.CreateOption<IRandom>();
      this.m_fieldsCache = new ImmutableArray<FieldInfo>();
      this.m_propertiesCache = new ImmutableArray<PropertyInfo>();
    }

    public void Clear()
    {
      this.EditedObj = Option<object>.None;
      this.m_fieldsCache = new ImmutableArray<FieldInfo>();
      this.m_propertiesCache = new ImmutableArray<PropertyInfo>();
    }

    public void NotifyWasEdited()
    {
      Action<object> onObjectEdited = this.OnObjectEdited;
      if (onObjectEdited == null)
        return;
      onObjectEdited(this.EditedObj.Value);
    }

    public EditedObjectMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
