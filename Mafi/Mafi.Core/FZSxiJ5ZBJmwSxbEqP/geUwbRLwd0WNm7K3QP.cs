// Decompiled with JetBrains decompiler
// Type: FZSxiJ5ZBJmwSxbEqP.geUwbRLwd0WNm7K3QP
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Reflection;

#nullable disable
namespace FZSxiJ5ZBJmwSxbEqP
{
  internal class geUwbRLwd0WNm7K3QP
  {
    internal static Module geULwbRwd;

    internal static void Rke3QXrQc(int typemdt)
    {
      Type type = geUwbRLwd0WNm7K3QP.geULwbRwd.ResolveType(33554432 + typemdt);
      foreach (FieldInfo field in type.GetFields())
      {
        MethodInfo method = (MethodInfo) geUwbRLwd0WNm7K3QP.geULwbRwd.ResolveMethod(field.MetadataToken + 100663296);
        field.SetValue((object) null, (object) (MulticastDelegate) Delegate.CreateDelegate(type, method));
      }
    }

    public geUwbRLwd0WNm7K3QP()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static geUwbRLwd0WNm7K3QP()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      geUwbRLwd0WNm7K3QP.geULwbRwd = typeof (geUwbRLwd0WNm7K3QP).Assembly.ManifestModule;
    }

    internal delegate void SFU4mbT3GMret7THonf(object o);
  }
}
