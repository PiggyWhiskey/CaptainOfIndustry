// Decompiled with JetBrains decompiler
// Type: RTG.GameObjectTypeHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class GameObjectTypeHelper
  {
    private static int _numTypes;
    private static List<GameObjectType> _allObjectTypes;
    private static GameObjectType _allCombined;

    static GameObjectTypeHelper()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GameObjectTypeHelper._allCombined = GameObjectType.Mesh | GameObjectType.Terrain | GameObjectType.Sprite | GameObjectType.Camera | GameObjectType.Light | GameObjectType.ParticleSystem | GameObjectType.Empty;
      Array values = Enum.GetValues(typeof (GameObjectType));
      GameObjectTypeHelper._numTypes = values.Length;
      GameObjectTypeHelper._allObjectTypes = new List<GameObjectType>(GameObjectTypeHelper._numTypes);
      foreach (object obj in values)
        GameObjectTypeHelper._allObjectTypes.Add((GameObjectType) obj);
    }

    public static int NumTypes => GameObjectTypeHelper._numTypes;

    public static GameObjectType[] AllObjectTypes => GameObjectTypeHelper._allObjectTypes.ToArray();

    public static GameObjectType AllCombined => GameObjectTypeHelper._allCombined;

    public static bool Is3DObjectType(GameObjectType objectType)
    {
      return objectType != GameObjectType.Sprite;
    }

    public static bool Is2DObjectType(GameObjectType objectType)
    {
      return objectType == GameObjectType.Sprite;
    }

    public static bool HasVolume(GameObjectType objectType)
    {
      return objectType == GameObjectType.Terrain || objectType == GameObjectType.Mesh || objectType == GameObjectType.Sprite;
    }

    public static bool IsTypeBitSet(int objectTypeMask, GameObjectType typeBit)
    {
      return ((GameObjectType) objectTypeMask & typeBit) != 0;
    }

    public static int SetTypeBit(int objectTypeMask, GameObjectType typeBit)
    {
      return (int) ((GameObjectType) objectTypeMask | typeBit);
    }

    public static int ClearTypeBit(int objectTypeMask, GameObjectType typeBit)
    {
      return (int) ((GameObjectType) objectTypeMask & ~typeBit);
    }
  }
}
