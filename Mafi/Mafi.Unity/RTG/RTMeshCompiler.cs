// Decompiled with JetBrains decompiler
// Type: RTG.RTMeshCompiler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class RTMeshCompiler
  {
    public static void CompileEntireScene()
    {
      foreach (GameObject sceneObject in MonoSingleton<RTScene>.Get.GetSceneObjects())
        RTMeshCompiler.CompileForObject(sceneObject);
    }

    public static bool CompileForObject(GameObject gameObject)
    {
      if (gameObject.isStatic)
        return false;
      Mesh mesh = gameObject.GetMesh();
      if ((Object) mesh == (Object) null)
        return false;
      RTMesh rtMesh = Singleton<RTMeshDb>.Get.GetRTMesh(mesh);
      if (rtMesh == null)
        return false;
      if (!rtMesh.IsTreeBuilt)
        rtMesh.BuildTree();
      return true;
    }
  }
}
