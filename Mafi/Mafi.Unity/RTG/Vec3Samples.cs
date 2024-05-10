// Decompiled with JetBrains decompiler
// Type: RTG.Vec3Samples
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class Vec3Samples
  {
    private int _maxNumSamples;
    private List<Vector3> _samples;

    public int NumSamples => this._samples.Count;

    public int MaxNumSamples => this._maxNumSamples;

    public void AddSample(Vector3 sample)
    {
      if (this.NumSamples < this.MaxNumSamples)
      {
        this._samples.Insert(0, sample);
      }
      else
      {
        for (int index = 0; index < this.NumSamples - 1; ++index)
          this._samples[index + 1] = this._samples[index];
        this._samples[0] = sample;
      }
    }

    public void SetMaxNumSamples(int maxNumSamples)
    {
      if (maxNumSamples == this.MaxNumSamples || maxNumSamples >= this.NumSamples)
        return;
      int num = maxNumSamples - this.NumSamples;
      for (int index = 0; index < num; ++index)
        this._samples.RemoveAt(this._samples.Count - 1);
    }

    public Vector3 GetAverage()
    {
      Vector3 zero = Vector3.zero;
      foreach (Vector3 sample in this._samples)
        zero += sample;
      return zero.normalized;
    }

    public Vec3Samples()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._maxNumSamples = 2;
      this._samples = new List<Vector3>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
