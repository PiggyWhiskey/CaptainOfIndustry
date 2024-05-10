// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.AnimationTexturesGenerator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using Mafi.Unity.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class AnimationTexturesGenerator
  {
    private readonly ProtosDb m_protosDb;
    private readonly AssetsDb m_assetsDb;
    private readonly ProtoModelFactory m_modelFactory;

    public AnimationTexturesGenerator(
      ProtosDb protosDb,
      AssetsDb assetsDb,
      ProtoModelFactory modelFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb.CheckNotNull<ProtosDb>();
      this.m_assetsDb = assetsDb.CheckNotNull<AssetsDb>();
      this.m_modelFactory = modelFactory.CheckNotNull<ProtoModelFactory>();
    }

    public bool GenerateAll { get; set; }

    public Option<string> NameSubstr { get; set; }

    public void Initialize(GameObject rootGo)
    {
    }

    private bool animationTextureExists(string path)
    {
      return !this.GenerateAll && this.m_assetsDb.ContainsAsset(path);
    }

    private bool shouldSkip(string id)
    {
      return this.NameSubstr.HasValue && !id.ToLowerInvariant().Contains(this.NameSubstr.Value);
    }

    public IEnumerable<string> GenerateLayoutEntities(string outDirPath)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<string>) new AnimationTexturesGenerator.\u003CGenerateLayoutEntities\u003Ed__15(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__outDirPath = outDirPath
      };
    }

    private string generateAnimationTexture(
      string outDirPath,
      string lodName,
      LayoutEntityProto entityProto,
      GameObject entityGo,
      GameObject animatedGo,
      Lyst<GameObject> allObjectsWithMeshes)
    {
      Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(outDirPath, entityProto.Graphics.AnimationGameObjectsPathForLod(lodName))));
      Animator[] componentsInChildren = animatedGo.GetComponentsInChildren<Animator>();
      string path = Path.Combine(outDirPath, entityProto.Graphics.AnimationGameObjectsPathForLod(lodName));
      if (!(entityProto is IProtoWithAnimation protoWithAnimation) || componentsInChildren.Length == 0)
        return "";
      Lyst<Material> lyst1 = new Lyst<Material>();
      Dict<Material, Lyst<GameObject>> dict1 = new Dict<Material, Lyst<GameObject>>();
      Set<GameObject> set = new Set<GameObject>();
      Lyst<GameObject> lyst2 = new Lyst<GameObject>();
      Dict<GameObject, Matrix4x4> dict2 = new Dict<GameObject, Matrix4x4>();
      Dict<GameObject, Vector3> dict3 = new Dict<GameObject, Vector3>();
      Dict<GameObject, Quaternion> dict4 = new Dict<GameObject, Quaternion>();
      Dict<GameObject, Vector3> dict5 = new Dict<GameObject, Vector3>();
      Lyst<string> contents = new Lyst<string>();
      lyst1.Clear();
      dict1.Clear();
      contents.Clear();
      foreach (GameObject allObjectsWithMesh in allObjectsWithMeshes)
      {
        MeshRenderer component = allObjectsWithMesh.GetComponent<MeshRenderer>();
        foreach (Material key in !((UnityEngine.Object) component != (UnityEngine.Object) null) ? allObjectsWithMesh.GetComponent<SkinnedMeshRenderer>().sharedMaterials : component.sharedMaterials)
        {
          if (!lyst1.Contains(key))
            lyst1.Add(key);
          if (!dict1.ContainsKey(key))
            dict1[key] = new Lyst<GameObject>();
          dict1[key].Add(allObjectsWithMesh);
        }
        dict2.Add(allObjectsWithMesh, allObjectsWithMesh.transform.localToWorldMatrix);
        dict3.Add(allObjectsWithMesh, allObjectsWithMesh.transform.position);
        dict5.Add(allObjectsWithMesh, allObjectsWithMesh.transform.lossyScale);
        dict4.Add(allObjectsWithMesh, allObjectsWithMesh.transform.rotation);
      }
      float num1 = 15f;
      foreach (Material key1 in lyst1)
      {
        Lyst<GameObject> lyst3 = dict1[key1];
        set.Clear();
        lyst2.Clear();
        foreach (GameObject gameObject in lyst3)
        {
          if ((UnityEngine.Object) gameObject.GetComponent<SkinnedMeshRenderer>() != (UnityEngine.Object) null)
            set.Add(gameObject);
        }
        Stopwatch stopwatch = new Stopwatch();
        int num2 = 0;
        bool flag = lyst1.Count == 1;
        Dict<GameObject, float> dict6 = new Dict<GameObject, float>();
        Dict<GameObject, Vector3> dict7 = new Dict<GameObject, Vector3>();
        Dict<GameObject, Quaternion> dict8 = new Dict<GameObject, Quaternion>();
        stopwatch.Start();
        ImmutableArray<AnimationParams> animationParams1 = protoWithAnimation.AnimationParams;
        foreach (AnimationParams animationParams2 in animationParams1)
        {
          if (animationParams2 is LoopAnimationParams)
            num1 = 30f;
          foreach (Animator animator in componentsInChildren)
          {
            Option<AnimationClip> clip = this.getClip(animationParams2, animator, entityProto);
            if (!clip.IsNone)
            {
              AnimationClip animationClip = clip.Value;
              int num3 = Mathf.FloorToInt(animationClip.length * num1) + 1;
              num2 += num3;
              for (int index = 1; index < num3; ++index)
              {
                animationClip.SampleAnimation(animator.gameObject, (float) index / num1);
                foreach (GameObject key2 in lyst3)
                {
                  if (flag)
                  {
                    if ((double) key2.transform.position.DistanceToSqr(dict3[key2]) > 1E-06)
                      flag = false;
                    else if ((double) key2.transform.lossyScale.DistanceToSqr(dict5[key2]) > 1E-06)
                      flag = false;
                    float angle;
                    if (index == 1)
                    {
                      dict6[key2] = getDeltaAngle(key2.transform.rotation, dict4[key2]);
                      Vector3 axis;
                      key2.transform.rotation.ToAngleAxis(out angle, out axis);
                      dict7[key2] = axis;
                    }
                    else
                    {
                      float deltaAngle = getDeltaAngle(key2.transform.rotation, dict8[key2]);
                      if ((double) (deltaAngle - dict6[key2]).Abs() > 1.0)
                        flag = false;
                      else if ((double) deltaAngle.Abs() > 0.0099999997764825821 && (double) getDeltaAngle(key2.transform.rotation, dict4[key2]).Abs() > 0.0099999997764825821)
                      {
                        Vector3 axis;
                        key2.transform.rotation.ToAngleAxis(out angle, out axis);
                        if ((double) Vector3.Dot(dict7[key2], axis).Abs() < 0.99000000953674316)
                          flag = false;
                      }
                    }
                    dict8[key2] = key2.transform.rotation;
                  }
                  if (dict2[key2] != key2.transform.localToWorldMatrix)
                    set.Add(key2);
                }
              }
              animationClip.SampleAnimation(animator.gameObject, 0.0f);
            }
          }
        }
        if (set.Count <= 0)
        {
          Mafi.Log.Info(string.Format("'{0}' has no animated children", (object) entityGo));
        }
        else
        {
          if (set.Count > 1)
            ;
          foreach (GameObject gameObject in lyst3)
          {
            if (set.Contains(gameObject))
              lyst2.Add(gameObject);
          }
          int num4 = 0;
          foreach (GameObject gameObject in lyst2)
          {
            SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
            Mesh mesh = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? gameObject.GetComponent<MeshFilter>().sharedMesh : component.sharedMesh;
            if (!mesh.isReadable)
              throw new Exception("Entity '" + entityProto.Id.Value + "': child '" + gameObject.name + "' is an unreadable mesh.");
            Mafi.Assert.That<int>(mesh.subMeshCount).IsEqualTo(1);
            Mafi.Assert.That<int>(mesh.vertices.Length).IsEqualTo(mesh.normals.Length);
            num4 += mesh.vertices.Length;
          }
          if (num4 <= 0)
          {
            Mafi.Log.Info(string.Format("'{0}' has no animated vertices", (object) entityGo));
          }
          else
          {
            int width = num2;
            int multipleOf = num4.CeilToMultipleOf(4);
            Texture2D vertTexXy = new Texture2D(width, multipleOf, TextureFormat.RGHalf, false, true);
            Texture2D vertTexZ = new Texture2D(width, multipleOf, TextureFormat.RHalf, false, true);
            Texture2D normTex = new Texture2D(width, multipleOf, TextureFormat.RGB24, false, true);
            Texture2D tangTex = new Texture2D(width, multipleOf, TextureFormat.RGB24, false, true);
            Mafi.Assert.That<int>(width).IsLessOrEqual(16384.Min(SystemInfo.maxTextureSize));
            Mafi.Assert.That<int>(multipleOf).IsLessOrEqual(16384.Min(SystemInfo.maxTextureSize));
            if (lyst2.IsEmpty)
            {
              Mafi.Log.Info(entityProto.Id.Value + " has no animated children");
            }
            else
            {
              animationParams1 = protoWithAnimation.AnimationParams;
              foreach (AnimationParams animationParams3 in animationParams1)
              {
                int num5 = 0;
                stopwatch.Restart();
                foreach (Animator animator in componentsInChildren)
                {
                  Option<AnimationClip> clip = this.getClip(animationParams3, animator, entityProto);
                  if (!clip.IsNone)
                  {
                    AnimationClip animationClip = clip.Value;
                    float length = animationClip.length;
                    int num6 = Mathf.FloorToInt(animationClip.length * num1) + 1;
                    foreach (GameObject key3 in lyst2)
                    {
                      SkinnedMeshRenderer component = key3.GetComponent<SkinnedMeshRenderer>();
                      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
                      {
                        component.updateWhenOffscreen = true;
                        animationClip.SampleAnimation(animator.gameObject, 0.0f);
                        Mesh mesh = new Mesh();
                        component.BakeMesh(mesh, true);
                        Vector3[] vertices1 = mesh.vertices;
                        for (int x = 0; x < num6; ++x)
                        {
                          animationClip.SampleAnimation(animator.gameObject, (float) x / num1);
                          Matrix4x4 localToWorldMatrix = key3.transform.localToWorldMatrix;
                          mesh.Clear();
                          component.BakeMesh(mesh, true);
                          Vector3[] vertices2 = mesh.vertices;
                          Vector3[] normals = mesh.normals;
                          Vector4[] tangents = mesh.tangents;
                          for (int index = 0; index < vertices2.Length; ++index)
                            setVertex((Vector3) (localToWorldMatrix * (Vector4) (vertices2[index] - vertices1[index])), x, index + num5);
                          for (int index = 0; index < normals.Length; ++index)
                            setNormal(localToWorldMatrix * (Vector4) normals[index], x, index + num5);
                          for (int index = 0; index < tangents.Length; ++index)
                            setTangent(localToWorldMatrix * tangents[index], x, index + num5);
                        }
                        num5 += vertices1.Length;
                      }
                      else
                      {
                        Mesh sharedMesh = key3.GetComponent<MeshFilter>().sharedMesh;
                        animationClip.SampleAnimation(animator.gameObject, 0.0f);
                        Vector3[] vertices = sharedMesh.vertices;
                        Vector3[] normals = sharedMesh.normals;
                        Vector4[] tangents = sharedMesh.tangents;
                        Mafi.Assert.That<int>(normals.Length).IsEqualTo(vertices.Length);
                        Mafi.Assert.That<int>(tangents.Length).IsEqualTo(vertices.Length);
                        Vector4[] vector4Array = new Vector4[vertices.Length];
                        for (int index = 0; index < vertices.Length; ++index)
                          vector4Array[index] = dict2[key3] * this.vertexToVector(vertices[index]);
                        for (int x = 0; x < num6; ++x)
                        {
                          animationClip.SampleAnimation(animator.gameObject, (float) x / num1);
                          Matrix4x4 localToWorldMatrix = key3.transform.localToWorldMatrix;
                          for (int index = 0; index < vertices.Length; ++index)
                            setVertex((Vector3) (localToWorldMatrix * this.vertexToVector(vertices[index]) - vector4Array[index]), x, index + num5);
                          for (int index = 0; index < normals.Length; ++index)
                            setNormal(localToWorldMatrix * (Vector4) normals[index], x, index + num5);
                          for (int index = 0; index < tangents.Length; ++index)
                          {
                            Vector4 vector4 = new Vector4(tangents[index].x, tangents[index].y, tangents[index].z, 0.0f);
                            setTangent(localToWorldMatrix * vector4, x, index + num5);
                          }
                        }
                        num5 += sharedMesh.vertices.Length;
                      }
                      animationClip.SampleAnimation(animator.gameObject, 0.0f);
                    }
                    contents.Add(key1.name);
                    contents.Add(animationParams3.AnimationStateName);
                    contents.Add(length.ToString());
                  }
                }
              }
              foreach (GameObject gameObject in lyst2)
                contents.Add(gameObject.name);
              contents.Add("");
              vertTexXy.Apply();
              vertTexZ.Apply();
              normTex.Apply();
              tangTex.Apply();
              File.WriteAllBytes(Path.Combine(outDirPath, entityProto.Graphics.AnimationTextureVerticesPathForMaterialAndLod(key1.name, lodName, "Xy")), vertTexXy.EncodeToEXR(Texture2D.EXRFlags.CompressZIP));
              File.WriteAllBytes(Path.Combine(outDirPath, entityProto.Graphics.AnimationTextureVerticesPathForMaterialAndLod(key1.name, lodName, "Z")), vertTexZ.EncodeToEXR(Texture2D.EXRFlags.CompressZIP));
              File.WriteAllBytes(Path.Combine(outDirPath, entityProto.Graphics.AnimationTextureNormalsPathForMaterialAndLod(key1.name, lodName)), normTex.EncodeToPNG());
              File.WriteAllBytes(Path.Combine(outDirPath, entityProto.Graphics.AnimationTextureTangentsPathForMaterialAndLod(key1.name, lodName)), tangTex.EncodeToPNG());
            }

            void setVertex(Vector3 vertex, int x, int y)
            {
              vertTexXy.SetPixel(x, y, new Color(vertex.x, vertex.y, 0.0f));
              vertTexZ.SetPixel(x, y, new Color(vertex.z, 0.0f, 0.0f));
            }

            void setNormal(Vector4 normal, int x, int y)
            {
              Vector4 vector4 = (new Vector4(normal.x, normal.y, normal.z, 0.0f).normalized + Vector4.one) / 2f;
              normTex.SetPixel(x, y, (Color) vector4);
            }

            void setTangent(Vector4 tangent, int x, int y)
            {
              Vector3 vector3 = (new Vector3(tangent.x, tangent.y, tangent.z).normalized + Vector3.one) * 0.5f;
              tangTex.SetPixel(x, y, (Color) new Vector4(vector3.x, vector3.y, vector3.z, 0.0f));
            }
          }
        }
      }
      File.WriteAllLines(path, (IEnumerable<string>) contents);
      return entityProto.Id.Value;

      static float getDeltaAngle(Quaternion prev, Quaternion current)
      {
        float deltaAngle = Quaternion.Angle(prev, current);
        while ((double) deltaAngle < -180.0)
          deltaAngle += 360f;
        while ((double) deltaAngle > 180.0)
          deltaAngle -= 360f;
        return deltaAngle;
      }
    }

    private Vector4 vertexToVector(Vector3 v) => new Vector4(v.x, v.y, v.z, 1f);

    private void getAllObjectsWithMeshes(Lyst<GameObject> allObjectsWithMeshes, GameObject entityGo)
    {
      allObjectsWithMeshes.Clear();
      if ((UnityEngine.Object) entityGo.GetComponent<MeshRenderer>() != (UnityEngine.Object) null || (UnityEngine.Object) entityGo.GetComponent<SkinnedMeshRenderer>() != (UnityEngine.Object) null)
        allObjectsWithMeshes.Add(entityGo);
      entityGo.GetAllChildren((Predicate<GameObject>) (c => (UnityEngine.Object) c.GetComponent<MeshRenderer>() != (UnityEngine.Object) null || (UnityEngine.Object) c.GetComponent<SkinnedMeshRenderer>() != (UnityEngine.Object) null), allObjectsWithMeshes);
    }

    private Option<AnimationClip> getClip(
      AnimationParams animationParams,
      Animator animator,
      LayoutEntityProto entityProto)
    {
      int hash = Animator.StringToHash(animationParams.AnimationStateName);
      if (!animator.HasState(0, hash))
      {
        Mafi.Log.Info(animator.name + " does not have state " + animationParams.AnimationStateName);
        return Option<AnimationClip>.None;
      }
      if (animationParams.AnimationStateName == "Main")
      {
        if (animator.runtimeAnimatorController.animationClips.Length != 1)
          Mafi.Log.Warning("Animator of entity '" + entityProto.Id.Value + "' has more than one clip matching for its 'Main' state: " + ((IEnumerable<string>) animator.runtimeAnimatorController.animationClips.MapArray<AnimationClip, string>((Func<AnimationClip, string>) (x => x.name))).JoinStrings(", "));
        return (Option<AnimationClip>) animator.runtimeAnimatorController.animationClips[0];
      }
      AnimationClip animationClip = ((IEnumerable<AnimationClip>) animator.runtimeAnimatorController.animationClips).FirstOrDefault<AnimationClip>((Func<AnimationClip, bool>) (x => x.name == animationParams.AnimationStateName));
      if ((UnityEngine.Object) animationClip == (UnityEngine.Object) null)
        Mafi.Log.Error("Failed to find clip " + animationParams.AnimationStateName);
      return Option<AnimationClip>.Create(animationClip);
    }
  }
}
