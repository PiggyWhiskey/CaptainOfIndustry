// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.IconClickable
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  /// <summary>Icon that has no background but is clickable.</summary>
  public class IconClickable : Button
  {
    private readonly string m_assetPath;
    private Option<Icon> m_shadow;
    private Icon m_icon;

    public IconClickable(string assetPath, Action onClick = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(onClick);
      this.m_assetPath = assetPath;
      this.Class<IconClickable>("iconClickable");
      this.Add((UiComponent) (this.m_icon = new Icon(assetPath)));
    }

    public IconClickable GrowOnHover()
    {
      Icon valueOrNull = this.m_shadow.ValueOrNull;
      if (valueOrNull != null)
        valueOrNull.Class<Icon>(Cls.growOnHover);
      return this.Class<IconClickable>(Cls.growOnHover);
    }

    public IconClickable Large()
    {
      this.m_icon.Large();
      return this;
    }

    public IconClickable Medium()
    {
      this.m_icon.Medium();
      return this;
    }

    public IconClickable Small()
    {
      this.m_icon.Small();
      return this;
    }

    public IconClickable Shadow()
    {
      if (this.m_shadow.IsNone)
      {
        this.m_shadow = (Option<Icon>) new Icon(this.m_assetPath).Class<Icon>(Cls.shadow);
        this.InsertAt(0, (UiComponent) this.m_shadow.Value);
      }
      return this;
    }

    protected override void SetColorInternal(ColorRgba? color) => this.m_icon.Color<Icon>(color);
  }
}
