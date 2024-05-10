// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.ExceptionHelper
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using System;

#nullable disable
namespace Mafi.Utils
{
  public static class ExceptionHelper
  {
    public static void ThrowException(string typeName)
    {
      throw (Exception) Activator.CreateInstance(Type.GetType(typeName));
    }
  }
}
