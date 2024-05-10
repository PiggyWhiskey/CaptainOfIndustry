// Decompiled with JetBrains decompiler
// Type: qKuRjGaaayM5aPTPCc.zy552yvNy6iYdJTbkI
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Reflection;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace qKuRjGaaayM5aPTPCc
{
  internal class zy552yvNy6iYdJTbkI
  {
    internal static Module zy5v52yNy;

    internal static void A3FgTybbyu(int typemdt)
    {
      Type type = zy552yvNy6iYdJTbkI.zy5v52yNy.ResolveType(33554432 + typemdt);
      foreach (FieldInfo field in type.GetFields())
      {
        MethodInfo method = (MethodInfo) zy552yvNy6iYdJTbkI.zy5v52yNy.ResolveMethod(field.MetadataToken + 100663296);
        field.SetValue((object) null, (object) (MulticastDelegate) Delegate.CreateDelegate(type, method));
      }
    }

    public zy552yvNy6iYdJTbkI()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static zy552yvNy6iYdJTbkI()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      zy552yvNy6iYdJTbkI.zy5v52yNy = typeof (zy552yvNy6iYdJTbkI).Assembly.ManifestModule;
    }

    internal delegate void SFU4mbT3GMret7THonf(object o);
  }
}
