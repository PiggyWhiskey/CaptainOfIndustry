// Decompiled with JetBrains decompiler
// Type: Mafi.IAction`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Action that takes one argument and produces no results that can be used for automatic calling based on
  /// <typeparamref name="TArg" />. See <see cref="M:Mafi.DependencyResolver.InvokeActionHierarchy(System.Object)" /> for more details.
  /// </summary>
  /// <typeparam name="TArg">Type of argument.</typeparam>
  public interface IAction<TArg>
  {
    void Invoke(TArg arg);
  }
}
