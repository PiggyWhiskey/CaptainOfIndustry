// Decompiled with JetBrains decompiler
// Type: RTG.Singleton`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class Singleton<T> where T : class, new()
  {
    private static T _instance;

    public static T Get => Singleton<T>._instance;

    protected Singleton()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static Singleton()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Singleton<T>._instance = new T();
    }
  }
}
