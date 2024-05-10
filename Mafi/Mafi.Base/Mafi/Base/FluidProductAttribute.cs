// Decompiled with JetBrains decompiler
// Type: Mafi.Base.FluidProductAttribute
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
  public sealed class FluidProductAttribute : ProductAttribute
  {
    private readonly ColorRgba m_color;
    private readonly ColorRgba m_pipeColor;
    private readonly ColorRgba m_accentColor;
    private readonly string m_icon;
    private readonly string m_name;
    private readonly bool m_cannotBeStored;
    private readonly bool m_canBeDiscarded;
    private readonly bool m_isWaste;
    private readonly bool m_pinToHomeScreen;
    private readonly string m_translationComment;

    public FluidProductAttribute(
      int color,
      string icon,
      int pipeColor = -1,
      int accentColor = 3355443,
      string name = null,
      bool cannotBeStored = false,
      bool canBeDiscarded = false,
      bool isWaste = false,
      bool pinToHomeScreen = false,
      string translationComment = null)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_color = (ColorRgba) color;
      this.m_pipeColor = pipeColor < 0 ? this.m_color : (ColorRgba) pipeColor;
      this.m_accentColor = (ColorRgba) accentColor;
      this.m_icon = icon;
      this.m_name = name;
      this.m_cannotBeStored = cannotBeStored;
      this.m_canBeDiscarded = canBeDiscarded;
      this.m_isWaste = isWaste;
      this.m_pinToHomeScreen = pinToHomeScreen;
      this.m_translationComment = translationComment;
    }

    public override Proto Register(ProtoRegistrator registrator, FieldInfo idFieldInfo)
    {
      ProductProto.ID id = (ProductProto.ID) idFieldInfo.GetValue((object) null);
      string name = this.m_name ?? idFieldInfo.Name.CamelCaseToSpacedSentenceCase();
      FluidProductProtoBuilder.State state = registrator.FluidProductProtoBuilder.Start(name, id, this.m_translationComment).SetIsStorable(!this.m_cannotBeStored).SetCanBeDiscarded(this.m_canBeDiscarded).SetIsWaste(this.m_isWaste).SetColor(this.m_color).SetTransportColor(this.m_pipeColor).SetTransportAccentColor(this.m_accentColor).SetCustomIconPath(this.m_icon);
      if (this.m_pinToHomeScreen)
        state = state.PinToHomeScreen();
      return (Proto) state.BuildAndAdd();
    }
  }
}
