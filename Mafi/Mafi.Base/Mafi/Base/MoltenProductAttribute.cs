// Decompiled with JetBrains decompiler
// Type: Mafi.Base.MoltenProductAttribute
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Reflection;

#nullable disable
namespace Mafi.Base
{
  [AttributeUsage(AttributeTargets.Field)]
  public sealed class MoltenProductAttribute : ProductAttribute
  {
    private readonly string m_material;
    private readonly string m_prefab;
    private readonly string m_customIconPath;
    private readonly string m_translationComment;

    public MoltenProductAttribute(
      string material,
      string prefab,
      string icon = null,
      string translationComment = null)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_material = material;
      this.m_prefab = prefab;
      this.m_customIconPath = icon;
      this.m_translationComment = translationComment;
    }

    public override Proto Register(ProtoRegistrator registrator, FieldInfo idFieldInfo)
    {
      ProductProto.ID id = (ProductProto.ID) idFieldInfo.GetValue((object) null);
      string spacedSentenceCase = idFieldInfo.Name.CamelCaseToSpacedSentenceCase();
      MoltenProductProtoBuilder.State state = registrator.MoltenProductProtoBuilder.Start(spacedSentenceCase, id, this.m_translationComment).SetMaterial(this.m_material).SetPrefabPath(this.m_prefab);
      if (this.m_customIconPath != null)
        state = state.SetCustomIconPath(this.m_customIconPath);
      return (Proto) state.BuildAndAdd();
    }
  }
}
