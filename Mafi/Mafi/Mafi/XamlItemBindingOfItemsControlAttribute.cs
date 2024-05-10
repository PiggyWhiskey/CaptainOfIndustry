﻿// Decompiled with JetBrains decompiler
// Type: Mafi.XamlItemBindingOfItemsControlAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// XAML attribute. Indicates the property of some <c>BindingBase</c>-derived type, that
  /// is used to bind some item of <c>ItemsControl</c>-derived type. This annotation will
  /// enable the <c>DataContext</c> type resolve for XAML bindings for such properties.
  /// </summary>
  /// <remarks>
  /// Property should have the tree ancestor of the <c>ItemsControl</c> type or
  /// marked with the <see cref="T:Mafi.XamlItemsControlAttribute" /> attribute.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Property)]
  public sealed class XamlItemBindingOfItemsControlAttribute : Attribute
  {
    public XamlItemBindingOfItemsControlAttribute()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
