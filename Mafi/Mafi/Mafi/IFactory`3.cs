// Decompiled with JetBrains decompiler
// Type: Mafi.IFactory`3
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Factory method that can be used for automatic instantiation based on <typeparamref name="TArg1" /> and
  /// <typeparamref name="TArg2" />. See <see cref="M:Mafi.DependencyResolver.InvokeFactoryHierarchy``1(System.Object,System.Object)" /> for more details.
  /// </summary>
  /// <typeparam name="TArg1">Type of the first argument.</typeparam>
  /// <typeparam name="TArg2">Type of the second argument.</typeparam>
  /// <typeparam name="TResult">Type of result, common for all factory methods in one group.</typeparam>
  public interface IFactory<TArg1, TArg2, TResult>
  {
    TResult Create(TArg1 arg1, TArg2 arg2);
  }
}
