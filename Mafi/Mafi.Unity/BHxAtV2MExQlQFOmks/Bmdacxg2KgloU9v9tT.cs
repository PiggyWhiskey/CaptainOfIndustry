// Decompiled with JetBrains decompiler
// Type: BHxAtV2MExQlQFOmks.Bmdacxg2KgloU9v9tT
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace BHxAtV2MExQlQFOmks
{
  internal class Bmdacxg2KgloU9v9tT
  {
    internal static Bmdacxg2KgloU9v9tT.hrYJqyvpMWJRBsclFfW[] c2fiEAifZm;
    internal static int[] e1jiMnmwc8;
    internal static List<string> W7vikF85fr;
    private static BinaryReader GBui1b8l1s;
    private static byte[] iNDiKKsy8x;
    private static bool TakiI8Q6Rq;
    private static object E30iXcOJwE;

    internal static object[] D9niinDqpe() => new object[1];

    internal static object[] KoniQphE6s<T>(
      int _param0,
      object[] _param1,
      object _param2,
      ref T _param3)
    {
      Bmdacxg2KgloU9v9tT.hrYJqyvpMWJRBsclFfW yjqyvpMwjrBsclFfW = (Bmdacxg2KgloU9v9tT.hrYJqyvpMWJRBsclFfW) null;
      lock (Bmdacxg2KgloU9v9tT.E30iXcOJwE)
      {
        if (!Bmdacxg2KgloU9v9tT.TakiI8Q6Rq)
        {
          Bmdacxg2KgloU9v9tT.TakiI8Q6Rq = true;
          Bmdacxg2KgloU9v9tT.Vh8ib9t5N1();
        }
        if (Bmdacxg2KgloU9v9tT.c2fiEAifZm[_param0] != null)
        {
          yjqyvpMwjrBsclFfW = Bmdacxg2KgloU9v9tT.c2fiEAifZm[_param0];
        }
        else
        {
          Bmdacxg2KgloU9v9tT.GBui1b8l1s.BaseStream.Position = (long) Bmdacxg2KgloU9v9tT.e1jiMnmwc8[_param0];
          yjqyvpMwjrBsclFfW = new Bmdacxg2KgloU9v9tT.hrYJqyvpMWJRBsclFfW();
          Module module = typeof (Bmdacxg2KgloU9v9tT).Module;
          int metadataToken = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
          int capacity1 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
          int capacity2 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
          int capacity3 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
          yjqyvpMwjrBsclFfW.c1I1Y59VD3 = module.ResolveMethod(metadataToken);
          ParameterInfo[] parameters = yjqyvpMwjrBsclFfW.c1I1Y59VD3.GetParameters();
          yjqyvpMwjrBsclFfW.A6N1NPB7k9 = new Bmdacxg2KgloU9v9tT.c1XGWVvY6wNnQ8CbqqX[parameters.Length];
          for (int index = 0; index < parameters.Length; ++index)
          {
            Type type = parameters[index].ParameterType;
            Bmdacxg2KgloU9v9tT.c1XGWVvY6wNnQ8CbqqX xgwVvY6wNnQ8CbqqX = new Bmdacxg2KgloU9v9tT.c1XGWVvY6wNnQ8CbqqX();
            xgwVvY6wNnQ8CbqqX.rnf1U6Te88 = type.IsByRef;
            xgwVvY6wNnQ8CbqqX.vsE15XSYqq = index;
            yjqyvpMwjrBsclFfW.A6N1NPB7k9[index] = xgwVvY6wNnQ8CbqqX;
            if (type.IsByRef)
              type = type.GetElementType();
            Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv k5DvLjvcEqTiB4Vv = !(type == typeof (string)) ? (!(type == typeof (byte)) ? (!(type == typeof (sbyte)) ? (!(type == typeof (short)) ? (!(type == typeof (ushort)) ? (!(type == typeof (int)) ? (!(type == typeof (uint)) ? (!(type == typeof (long)) ? (!(type == typeof (ulong)) ? (!(type == typeof (float)) ? (!(type == typeof (double)) ? (!(type == typeof (bool)) ? (!(type == typeof (IntPtr)) ? (!(type == typeof (UIntPtr)) ? (!(type == typeof (char)) ? (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 0 : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 13) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 10) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 9) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2) : (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 14;
            xgwVvY6wNnQ8CbqqX.BQw1Cc3bH0 = k5DvLjvcEqTiB4Vv;
          }
          yjqyvpMwjrBsclFfW.hb71qhcL9p = new List<Bmdacxg2KgloU9v9tT.jWIgTkvhDxGD2JTupj2>(capacity1);
          for (int index = 0; index < capacity1; ++index)
          {
            int num = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
            Bmdacxg2KgloU9v9tT.jWIgTkvhDxGD2JTupj2 wigTkvhDxGd2Jtupj2 = new Bmdacxg2KgloU9v9tT.jWIgTkvhDxGD2JTupj2();
            wigTkvhDxGd2Jtupj2.zsf1O1MsJp = (Type) null;
            if (num >= 0 && num < 50)
            {
              wigTkvhDxGd2Jtupj2.On712TVRMl = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) (num & 31);
              wigTkvhDxGd2Jtupj2.f8S1LJGfI1 = (num & 32) > 0;
            }
            wigTkvhDxGd2Jtupj2.irh1gs4hgA = index;
            yjqyvpMwjrBsclFfW.hb71qhcL9p.Add(wigTkvhDxGd2Jtupj2);
          }
          yjqyvpMwjrBsclFfW.yPi1pye7aO = new List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad>(capacity2);
          for (int index = 0; index < capacity2; ++index)
          {
            int num1 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
            int num2 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
            Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad = new Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad();
            ime2cvNluVvVun8vad.kxu10mh8H0 = num1;
            ime2cvNluVvVun8vad.BVa1GI0rhE = num2;
            Bmdacxg2KgloU9v9tT.tWRYqQvqSCFAC7p6maF yqQvqScfaC7p6maF = new Bmdacxg2KgloU9v9tT.tWRYqQvqSCFAC7p6maF();
            ime2cvNluVvVun8vad.a3i1VrsFOu = yqQvqScfaC7p6maF;
            int num3 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
            int num4 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
            int num5 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
            yqQvqScfaC7p6maF.MSW1rkABlh = num3;
            yqQvqScfaC7p6maF.Tvd1SZGoAq = num4;
            yqQvqScfaC7p6maF.olp1oNVS7m = num5;
            switch (num5)
            {
              case 0:
                yqQvqScfaC7p6maF.CZ118rhm4y = module.ResolveType(Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s));
                break;
              case 1:
                yqQvqScfaC7p6maF.wuT13lKOjC = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
                break;
              default:
                Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
                break;
            }
            yjqyvpMwjrBsclFfW.yPi1pye7aO.Add(ime2cvNluVvVun8vad);
          }
          yjqyvpMwjrBsclFfW.yPi1pye7aO.Sort((Comparison<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad>) ((x, y) => x.a3i1VrsFOu.MSW1rkABlh.CompareTo(y.a3i1VrsFOu.MSW1rkABlh)));
          yjqyvpMwjrBsclFfW.yN41ho6cU2 = new List<Bmdacxg2KgloU9v9tT.lQv41fvVaiFQIGDARSB>(capacity3);
          for (int index1 = 0; index1 < capacity3; ++index1)
          {
            Bmdacxg2KgloU9v9tT.lQv41fvVaiFQIGDARSB qv41fvVaiFqigdarsb = new Bmdacxg2KgloU9v9tT.lQv41fvVaiFQIGDARSB();
            byte index2 = Bmdacxg2KgloU9v9tT.GBui1b8l1s.ReadByte();
            qv41fvVaiFqigdarsb.Jjq1TPhy1P = (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) index2;
            int num = index2 < (byte) 176 ? (int) Bmdacxg2KgloU9v9tT.iNDiKKsy8x[(int) index2] : throw new Exception();
            if (num == 0)
            {
              qv41fvVaiFqigdarsb.fx11sYMeHi = (object) null;
            }
            else
            {
              object obj;
              switch (num - 1)
              {
                case 0:
                  obj = (object) Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
                  break;
                case 1:
                  obj = (object) Bmdacxg2KgloU9v9tT.GBui1b8l1s.ReadInt64();
                  break;
                case 2:
                  obj = (object) Bmdacxg2KgloU9v9tT.GBui1b8l1s.ReadSingle();
                  break;
                case 3:
                  obj = (object) Bmdacxg2KgloU9v9tT.GBui1b8l1s.ReadDouble();
                  break;
                case 4:
                  int length = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
                  int[] numArray = new int[length];
                  for (int index3 = 0; index3 < length; ++index3)
                    numArray[index3] = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
                  obj = (object) numArray;
                  break;
                default:
                  throw new Exception();
              }
              qv41fvVaiFqigdarsb.fx11sYMeHi = obj;
            }
            yjqyvpMwjrBsclFfW.yN41ho6cU2.Add(qv41fvVaiFqigdarsb);
          }
          Bmdacxg2KgloU9v9tT.c2fiEAifZm[_param0] = yjqyvpMwjrBsclFfW;
        }
      }
      Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp r72hDv7XqltkgXgRlp = new Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp();
      r72hDv7XqltkgXgRlp.xevXHJGR3n = yjqyvpMwjrBsclFfW;
      ParameterInfo[] parameters1 = yjqyvpMwjrBsclFfW.c1I1Y59VD3.GetParameters();
      bool flag = false;
      int num6 = 0;
      if ((object) (yjqyvpMwjrBsclFfW.c1I1Y59VD3 as MethodInfo) != null && ((MethodInfo) yjqyvpMwjrBsclFfW.c1I1Y59VD3).ReturnType != typeof (void))
        flag = true;
      if (yjqyvpMwjrBsclFfW.c1I1Y59VD3.IsStatic)
      {
        r72hDv7XqltkgXgRlp.zeQXjGK0kf = new Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[parameters1.Length];
        for (int index = 0; index < parameters1.Length; ++index)
        {
          Type parameterType = parameters1[index].ParameterType;
          r72hDv7XqltkgXgRlp.zeQXjGK0kf[index] = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameterType, _param1[index]);
          if (parameterType.IsByRef)
            ++num6;
        }
      }
      else
      {
        r72hDv7XqltkgXgRlp.zeQXjGK0kf = new Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[parameters1.Length + 1];
        r72hDv7XqltkgXgRlp.zeQXjGK0kf[0] = !yjqyvpMwjrBsclFfW.c1I1Y59VD3.DeclaringType.IsValueType ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab(_param2) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab(_param2), yjqyvpMwjrBsclFfW.c1I1Y59VD3.DeclaringType);
        for (int index = 0; index < parameters1.Length; ++index)
        {
          Type parameterType = parameters1[index].ParameterType;
          if (parameterType.IsByRef)
          {
            r72hDv7XqltkgXgRlp.zeQXjGK0kf[index + 1] = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameterType, _param1[index]);
            ++num6;
          }
          else
            r72hDv7XqltkgXgRlp.zeQXjGK0kf[index + 1] = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameterType, _param1[index]);
        }
      }
      r72hDv7XqltkgXgRlp.m2bXDcC4Eq = new Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[yjqyvpMwjrBsclFfW.hb71qhcL9p.Count];
      for (int index = 0; index < yjqyvpMwjrBsclFfW.hb71qhcL9p.Count; ++index)
      {
        Bmdacxg2KgloU9v9tT.jWIgTkvhDxGD2JTupj2 wigTkvhDxGd2Jtupj2 = yjqyvpMwjrBsclFfW.hb71qhcL9p[index];
        switch (wigTkvhDxGd2Jtupj2.On712TVRMl)
        {
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 0:
            r72hDv7XqltkgXgRlp.m2bXDcC4Eq[index] = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) null;
            break;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15:
            r72hDv7XqltkgXgRlp.m2bXDcC4Eq[index] = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0, wigTkvhDxGd2Jtupj2.On712TVRMl);
            break;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8:
            r72hDv7XqltkgXgRlp.m2bXDcC4Eq[index] = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(0L, wigTkvhDxGd2Jtupj2.On712TVRMl);
            break;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 9:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 10:
            r72hDv7XqltkgXgRlp.m2bXDcC4Eq[index] = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(0.0, wigTkvhDxGd2Jtupj2.On712TVRMl);
            break;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12:
            r72hDv7XqltkgXgRlp.m2bXDcC4Eq[index] = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(IntPtr.Zero);
            break;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 13:
            r72hDv7XqltkgXgRlp.m2bXDcC4Eq[index] = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(UIntPtr.Zero);
            break;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 14:
            r72hDv7XqltkgXgRlp.m2bXDcC4Eq[index] = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) null;
            break;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 16:
            r72hDv7XqltkgXgRlp.m2bXDcC4Eq[index] = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null);
            break;
        }
      }
      try
      {
        r72hDv7XqltkgXgRlp.Xr317km48e();
      }
      finally
      {
        r72hDv7XqltkgXgRlp.BsJ16rMxkW();
      }
      int num7 = 0;
      if (flag)
        num7 = 1;
      object[] objArray = new object[num7 + num6];
      if (flag)
        objArray[0] = (object) null;
      if ((object) (yjqyvpMwjrBsclFfW.c1I1Y59VD3 as MethodInfo) != null)
      {
        MethodInfo c1I1Y59Vd3 = (MethodInfo) yjqyvpMwjrBsclFfW.c1I1Y59VD3;
        if (c1I1Y59Vd3.ReturnType != typeof (void) && r72hDv7XqltkgXgRlp.BoDX6FHCap != null)
          objArray[0] = r72hDv7XqltkgXgRlp.BoDX6FHCap.csZxJgmksq(c1I1Y59Vd3.ReturnType);
      }
      if (num6 > 0)
      {
        int index4 = 0;
        if (flag)
          ++index4;
        for (int index5 = 0; index5 < parameters1.Length; ++index5)
        {
          Type parameterType = parameters1[index5].ParameterType;
          if (parameterType.IsByRef)
          {
            Type elementType = parameterType.GetElementType();
            objArray[index4] = r72hDv7XqltkgXgRlp.zeQXjGK0kf[index5] == null ? (object) null : (!yjqyvpMwjrBsclFfW.c1I1Y59VD3.IsStatic ? r72hDv7XqltkgXgRlp.zeQXjGK0kf[index5 + 1].csZxJgmksq(elementType) : r72hDv7XqltkgXgRlp.zeQXjGK0kf[index5].csZxJgmksq(elementType));
            ++index4;
          }
        }
      }
      if (!yjqyvpMwjrBsclFfW.c1I1Y59VD3.IsStatic && yjqyvpMwjrBsclFfW.c1I1Y59VD3.DeclaringType.IsValueType)
        _param3 = (T) r72hDv7XqltkgXgRlp.zeQXjGK0kf[0].csZxJgmksq(yjqyvpMwjrBsclFfW.c1I1Y59VD3.DeclaringType);
      return objArray;
    }

    internal static object[] LOPiTZStoR(int _param0, object[] _param1, object _param2)
    {
      int num = 0;
      return Bmdacxg2KgloU9v9tT.KoniQphE6s<int>(_param0, _param1, _param2, ref num);
    }

    internal static object[] qKJisOc8PZ<T>(int _param0, object[] _param1, ref T _param2)
    {
      return Bmdacxg2KgloU9v9tT.KoniQphE6s<T>(_param0, _param1, (object) _param2, ref _param2);
    }

    internal static void Vh8ib9t5N1()
    {
      if (Bmdacxg2KgloU9v9tT.e1jiMnmwc8 != null)
        return;
      BinaryReader binaryReader = new BinaryReader(typeof (Bmdacxg2KgloU9v9tT).Assembly.GetManifestResourceStream("n1PCmVgdOvJjeRgC2as.4c9EQ3glr8ojl0tNnD5"));
      binaryReader.BaseStream.Position = 0L;
      byte[] numArray = binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
      binaryReader.Close();
      Bmdacxg2KgloU9v9tT.MY8iPGtq0d(numArray);
    }

    internal static void MY8iPGtq0d(byte[] _param0)
    {
      Bmdacxg2KgloU9v9tT.GBui1b8l1s = new BinaryReader((Stream) new MemoryStream(_param0));
      Bmdacxg2KgloU9v9tT.iNDiKKsy8x = new byte[(int) byte.MaxValue];
      int num1 = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int index2 = (int) Bmdacxg2KgloU9v9tT.GBui1b8l1s.ReadByte();
        Bmdacxg2KgloU9v9tT.iNDiKKsy8x[index2] = Bmdacxg2KgloU9v9tT.GBui1b8l1s.ReadByte();
      }
      int capacity = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
      Bmdacxg2KgloU9v9tT.W7vikF85fr = new List<string>(capacity);
      for (int index = 0; index < capacity; ++index)
        Bmdacxg2KgloU9v9tT.W7vikF85fr.Add(Encoding.Unicode.GetString(Bmdacxg2KgloU9v9tT.GBui1b8l1s.ReadBytes(Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s))));
      int length = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
      Bmdacxg2KgloU9v9tT.c2fiEAifZm = new Bmdacxg2KgloU9v9tT.hrYJqyvpMWJRBsclFfW[length];
      Bmdacxg2KgloU9v9tT.e1jiMnmwc8 = new int[length];
      for (int index = 0; index < length; ++index)
      {
        Bmdacxg2KgloU9v9tT.c2fiEAifZm[index] = (Bmdacxg2KgloU9v9tT.hrYJqyvpMWJRBsclFfW) null;
        Bmdacxg2KgloU9v9tT.e1jiMnmwc8[index] = Bmdacxg2KgloU9v9tT.h1jiWCCugT(Bmdacxg2KgloU9v9tT.GBui1b8l1s);
      }
      int position = (int) Bmdacxg2KgloU9v9tT.GBui1b8l1s.BaseStream.Position;
      for (int index = 0; index < length; ++index)
      {
        int num2 = Bmdacxg2KgloU9v9tT.e1jiMnmwc8[index];
        Bmdacxg2KgloU9v9tT.e1jiMnmwc8[index] = position;
        position += num2;
      }
    }

    internal static int h1jiWCCugT(BinaryReader _param0)
    {
      bool flag = false;
      uint num1 = 0;
      uint num2 = (uint) _param0.ReadByte();
      uint num3 = num1 | num2 & 63U;
      if (((int) num2 & 64) != 0)
        flag = true;
      if (num2 < 128U)
        return flag ? ~(int) num3 : (int) num3;
      int num4 = 0;
      while (true)
      {
        uint num5 = (uint) _param0.ReadByte();
        num3 |= (uint) (((int) num5 & (int) sbyte.MaxValue) << 7 * num4 + 6);
        if (num5 >= 128U)
          ++num4;
        else
          break;
      }
      return flag ? ~(int) num3 : (int) num3;
    }

    public Bmdacxg2KgloU9v9tT()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static Bmdacxg2KgloU9v9tT()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Bmdacxg2KgloU9v9tT.c2fiEAifZm = (Bmdacxg2KgloU9v9tT.hrYJqyvpMWJRBsclFfW[]) null;
      Bmdacxg2KgloU9v9tT.e1jiMnmwc8 = (int[]) null;
      Bmdacxg2KgloU9v9tT.TakiI8Q6Rq = false;
      Bmdacxg2KgloU9v9tT.E30iXcOJwE = new object();
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct PLq36KvI50OKiAIi89r
    {
      [FieldOffset(0)]
      public byte DSakr1M1Zd;
      [FieldOffset(0)]
      public sbyte k8xkS9Ie0M;
      [FieldOffset(0)]
      public ushort Atmk9LL7nS;
      [FieldOffset(0)]
      public short OQEk8DqsxE;
      [FieldOffset(0)]
      public uint eC7k3JH9ai;
      [FieldOffset(0)]
      public int I8ZkohiOvo;
    }

    private class okOOfhvXAxlsKuDwlgC : Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN
    {
      public Bmdacxg2KgloU9v9tT.PLq36KvI50OKiAIi89r CJIkqRsjPJ;
      public Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv tPmkpCFndg;

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.CJIkqRsjPJ = ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ;
        this.tPmkpCFndg = ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).tPmkpCFndg;
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.VPUxGtPN74(_param1);
      }

      public okOOfhvXAxlsKuDwlgC(bool _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 1;
        this.CJIkqRsjPJ.I8ZkohiOvo = !_param1 ? 0 : 1;
        this.tPmkpCFndg = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11;
      }

      public okOOfhvXAxlsKuDwlgC(Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = _param1.tdv5g7bctk;
        this.CJIkqRsjPJ.I8ZkohiOvo = _param1.CJIkqRsjPJ.I8ZkohiOvo;
        this.tPmkpCFndg = _param1.tPmkpCFndg;
      }

      public override Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN qh0xUMsisJ()
      {
        return (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this);
      }

      public okOOfhvXAxlsKuDwlgC(int _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 1;
        this.CJIkqRsjPJ.I8ZkohiOvo = _param1;
        this.tPmkpCFndg = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5;
      }

      public okOOfhvXAxlsKuDwlgC(uint _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 1;
        this.CJIkqRsjPJ.eC7k3JH9ai = _param1;
        this.tPmkpCFndg = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6;
      }

      public okOOfhvXAxlsKuDwlgC(int _param1, Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 1;
        this.CJIkqRsjPJ.I8ZkohiOvo = _param1;
        this.tPmkpCFndg = _param2;
      }

      public okOOfhvXAxlsKuDwlgC(uint _param1, Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 1;
        this.CJIkqRsjPJ.eC7k3JH9ai = _param1;
        this.tPmkpCFndg = _param2;
      }

      public override bool xlJxxFWp5b()
      {
        switch (this.tPmkpCFndg)
        {
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15:
            return this.CJIkqRsjPJ.I8ZkohiOvo == 0;
          default:
            return this.CJIkqRsjPJ.eC7k3JH9ai == 0U;
        }
      }

      public override bool nNGx0hUd1S() => !this.xlJxxFWp5b();

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD aHlx1rUpXO(
        Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param1)
      {
        switch (_param1)
        {
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.ot6xjGUuHF();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.N9oxsvuhdW();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.xA7xKXq5xu();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.R60xQa0kEN();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.R54x68lelm();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.REix24O3Rn();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.klKxAPPf0t();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.PNWkhJ9Tma();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 16:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.qh0xUMsisJ();
          default:
            throw new Exception(((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 4).ToString());
        }
      }

      internal override object csZxJgmksq(Type _param1)
      {
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (_param1 != (Type) null && Nullable.GetUnderlyingType(_param1) != (Type) null)
          _param1 = Nullable.GetUnderlyingType(_param1);
        if (_param1 == (Type) null || _param1 == typeof (object))
        {
          switch (this.tPmkpCFndg)
          {
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
              return (object) this.CJIkqRsjPJ.k8xkS9Ie0M;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2:
              return (object) this.CJIkqRsjPJ.DSakr1M1Zd;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
              return (object) this.CJIkqRsjPJ.OQEk8DqsxE;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4:
              return (object) this.CJIkqRsjPJ.Atmk9LL7nS;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
              return (object) this.CJIkqRsjPJ.I8ZkohiOvo;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6:
              return (object) this.CJIkqRsjPJ.eC7k3JH9ai;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7:
              return (object) (long) this.CJIkqRsjPJ.I8ZkohiOvo;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8:
              return (object) (ulong) this.CJIkqRsjPJ.eC7k3JH9ai;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
              return (object) this.nNGx0hUd1S();
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15:
              return (object) (char) this.CJIkqRsjPJ.I8ZkohiOvo;
            default:
              return (object) this.CJIkqRsjPJ.I8ZkohiOvo;
          }
        }
        else
        {
          if (_param1 == typeof (int))
            return (object) this.CJIkqRsjPJ.I8ZkohiOvo;
          if (_param1 == typeof (uint))
            return (object) this.CJIkqRsjPJ.eC7k3JH9ai;
          if (_param1 == typeof (short))
            return (object) this.CJIkqRsjPJ.OQEk8DqsxE;
          if (_param1 == typeof (ushort))
            return (object) this.CJIkqRsjPJ.Atmk9LL7nS;
          if (_param1 == typeof (byte))
            return (object) this.CJIkqRsjPJ.DSakr1M1Zd;
          if (_param1 == typeof (sbyte))
            return (object) this.CJIkqRsjPJ.k8xkS9Ie0M;
          if (_param1 == typeof (bool))
            return (object) !this.xlJxxFWp5b();
          if (_param1 == typeof (long))
            return (object) (long) this.CJIkqRsjPJ.I8ZkohiOvo;
          if (_param1 == typeof (ulong))
            return (object) (ulong) this.CJIkqRsjPJ.eC7k3JH9ai;
          if (_param1 == typeof (char))
            return (object) (char) this.CJIkqRsjPJ.I8ZkohiOvo;
          if (_param1 == typeof (IntPtr))
            return (object) new IntPtr(this.CJIkqRsjPJ.I8ZkohiOvo);
          if (_param1 == typeof (UIntPtr))
            return (object) new UIntPtr(this.CJIkqRsjPJ.eC7k3JH9ai);
          return _param1.IsEnum ? this.A5UkYjgiMW(_param1) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        }
      }

      internal object A5UkYjgiMW(Type _param1)
      {
        Type underlyingType = Enum.GetUnderlyingType(_param1);
        if (underlyingType == typeof (int))
          return Enum.ToObject(_param1, this.CJIkqRsjPJ.I8ZkohiOvo);
        if (underlyingType == typeof (uint))
          return Enum.ToObject(_param1, this.CJIkqRsjPJ.eC7k3JH9ai);
        if (underlyingType == typeof (short))
          return Enum.ToObject(_param1, this.CJIkqRsjPJ.OQEk8DqsxE);
        if (underlyingType == typeof (ushort))
          return Enum.ToObject(_param1, this.CJIkqRsjPJ.Atmk9LL7nS);
        if (underlyingType == typeof (byte))
          return Enum.ToObject(_param1, this.CJIkqRsjPJ.DSakr1M1Zd);
        if (underlyingType == typeof (sbyte))
          return Enum.ToObject(_param1, this.CJIkqRsjPJ.k8xkS9Ie0M);
        if (underlyingType == typeof (long))
          return Enum.ToObject(_param1, (long) this.CJIkqRsjPJ.I8ZkohiOvo);
        if (underlyingType == typeof (ulong))
          return Enum.ToObject(_param1, (ulong) this.CJIkqRsjPJ.eC7k3JH9ai);
        return underlyingType == typeof (char) ? Enum.ToObject(_param1, (ushort) this.CJIkqRsjPJ.I8ZkohiOvo) : Enum.ToObject(_param1, this.CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC klKxAPPf0t()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.xlJxxFWp5b() ? 0 : 1);
      }

      internal override bool KOTxELbEBi() => this.nNGx0hUd1S();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ot6xjGUuHF()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) this.CJIkqRsjPJ.k8xkS9Ie0M, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC PNWkhJ9Tma()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC N9oxsvuhdW()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) this.CJIkqRsjPJ.DSakr1M1Zd, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC xA7xKXq5xu()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) this.CJIkqRsjPJ.OQEk8DqsxE, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R60xQa0kEN()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) this.CJIkqRsjPJ.Atmk9LL7nS, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R54x68lelm()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC REix24O3Rn()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.eC7k3JH9ai, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VyUxevyYW6()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((long) this.CJIkqRsjPJ.I8ZkohiOvo, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 fQfxiEUWtx()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((ulong) this.CJIkqRsjPJ.eC7k3JH9ai, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC TYoxcQ8ydQ() => this.ot6xjGUuHF();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC cacxITg5J8() => this.xA7xKXq5xu();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC YJ0x3w8q27() => this.R54x68lelm();

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 Y61x8yfPbH() => this.VyUxevyYW6();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Cl6xWVWMfh() => this.N9oxsvuhdW();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC hPFxRK7l3X() => this.R60xQa0kEN();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC qCFxH1Sb7q() => this.REix24O3Rn();

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 hVhxveNBwW() => this.fQfxiEUWtx();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC jyyxw6yGFH()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((sbyte) this.CJIkqRsjPJ.I8ZkohiOvo), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Gj7xdvyCKW()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((sbyte) this.CJIkqRsjPJ.eC7k3JH9ai), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pgZxfwQuEP()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((short) this.CJIkqRsjPJ.I8ZkohiOvo), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pxZxmQMP9n()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((short) this.CJIkqRsjPJ.eC7k3JH9ai), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC q8txPhiOdM()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ObkxhydOXc()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((int) this.CJIkqRsjPJ.eC7k3JH9ai), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 TWGxrqtIUh()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((long) this.CJIkqRsjPJ.I8ZkohiOvo, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 ghqxVF9bNj()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((long) this.CJIkqRsjPJ.eC7k3JH9ai, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC mO5xoicFZZ()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((byte) this.CJIkqRsjPJ.I8ZkohiOvo), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC r4Jxu3ic6x()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((byte) this.CJIkqRsjPJ.eC7k3JH9ai), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC KT4xqQtVPi()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((ushort) this.CJIkqRsjPJ.I8ZkohiOvo), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DX2xkrAct0()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((ushort) this.CJIkqRsjPJ.eC7k3JH9ai), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DStxB4OhTs()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((uint) this.CJIkqRsjPJ.I8ZkohiOvo), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC uagxYI5BKM()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.eC7k3JH9ai, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 zG9xDa5XWa()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked ((ulong) this.CJIkqRsjPJ.I8ZkohiOvo), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VeAxgljEYN()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((ulong) this.CJIkqRsjPJ.eC7k3JH9ai, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G m18xyPS2QZ()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((float) this.CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G n27x71nDuy()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((double) this.CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G U0KxNmsThj()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((double) this.CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VZTxtIrfkH()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.Y61x8yfPbH().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.YJ0x3w8q27().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VhvxzNwEWd()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.hVhxveNBwW().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.qCFxH1Sb7q().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx efa0lbWjvg()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.TWGxrqtIUh().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.q8txPhiOdM().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx TL00pic9Va()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.zG9xDa5XWa().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.DStxB4OhTs().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx StD05mXPiK()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.ghqxVF9bNj().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.ObkxhydOXc().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx Kut0CpUwqp()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VeAxgljEYN().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.uagxYI5BKM().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TiQ0bUSCaT()
      {
        switch (this.tPmkpCFndg)
        {
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(-this.CJIkqRsjPJ.I8ZkohiOvo);
          default:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) -this.CJIkqRsjPJ.eC7k3JH9ai);
        }
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Add(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo + ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).Add((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TXD0ZXksHZ(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked (this.CJIkqRsjPJ.I8ZkohiOvo + ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).TXD0ZXksHZ((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD OPa0LZv7Qy(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked (this.CJIkqRsjPJ.eC7k3JH9ai + ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai));
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).OPa0LZv7Qy((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD vra0nNVSAN(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo - ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).heFkxY7UpU((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD W8C0F651o0(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked (this.CJIkqRsjPJ.I8ZkohiOvo - ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).vZRk4h5SVe((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD NmO04Bshno(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked (this.CJIkqRsjPJ.eC7k3JH9ai - ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai));
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).CR5kdkagBc((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD w1K0O2tpMM(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo * ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).w1K0O2tpMM((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD y610MW78h1(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked (this.CJIkqRsjPJ.I8ZkohiOvo * ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).y610MW78h1((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Qi30aYIBJc(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked (this.CJIkqRsjPJ.eC7k3JH9ai * ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai));
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).Qi30aYIBJc((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD lEV0TscmDj(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo / ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).SZZkArTPCY((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD JcH0S7ZgWe(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.eC7k3JH9ai / ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).S7gkmKrUIG((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD XF10XBRNax(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo % ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).bmZknLs6mV((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD wjZ0Gc5iXg(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.eC7k3JH9ai % ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).L9CkthgLlF((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD EH309H4f3U(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo & ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).EH309H4f3U((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD P0M0UAL1ro(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo | ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).P0M0UAL1ro((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD woo0xx3hQ9()
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(~this.CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD FPe00NLJLE(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo ^ ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).FPe00NLJLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD MHo01vyORd(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo << ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).HJw1vMQuj4(this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Dti0JwAExR(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.I8ZkohiOvo >> ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).SBh1u7YJ9w(this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD A5O0Af5Pfa(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.CJIkqRsjPJ.eC7k3JH9ai >> ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).PNqkz8TsTM(this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override string ToString()
      {
        switch (this.tPmkpCFndg)
        {
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
            return this.CJIkqRsjPJ.I8ZkohiOvo.ToString();
          default:
            return this.CJIkqRsjPJ.eC7k3JH9ai.ToString();
        }
      }

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this;
      }

      internal override bool A6a0jc67Sw() => true;

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.riO5EmiZ8q())
          return _param1.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        if (_param1.U4w0RYoANI())
          return _param1.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = _param1.BXg0EpmNON();
        if (!jr2vZqxigsruTjoD.A6a0jc67Sw() || jr2vZqxigsruTjoD.lIM51cVqgP())
          return false;
        return jr2vZqxigsruTjoD.oBL5MGf29A() ? this.CJIkqRsjPJ.I8ZkohiOvo == ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) jr2vZqxigsruTjoD).CJIkqRsjPJ.I8ZkohiOvo : jr2vZqxigsruTjoD.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
      }

      private static Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN qypkNuLklj(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param0)
      {
        if (!(_param0 is Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN) && _param0.U4w0RYoANI())
          kp63vgBuPycmxljcN = _param0.BXg0EpmNON() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN;
        return kp63vgBuPycmxljcN;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.riO5EmiZ8q())
          return false;
        if (_param1.U4w0RYoANI())
          return _param1.FIt0KIs55w((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = _param1.BXg0EpmNON();
        if (!jr2vZqxigsruTjoD.A6a0jc67Sw() || jr2vZqxigsruTjoD.lIM51cVqgP())
          return false;
        return jr2vZqxigsruTjoD.oBL5MGf29A() ? (int) this.CJIkqRsjPJ.eC7k3JH9ai != (int) ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) jr2vZqxigsruTjoD).CJIkqRsjPJ.eC7k3JH9ai : jr2vZqxigsruTjoD.FIt0KIs55w((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
      }

      public override bool Mnr0QSnYqf(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return this.CJIkqRsjPJ.I8ZkohiOvo >= ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).Cfn0iX05uU((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool Cnj06t1SA6(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return this.CJIkqRsjPJ.eC7k3JH9ai >= ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).RRp0cxhL37((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool zof028Rdly(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return this.CJIkqRsjPJ.I8ZkohiOvo > ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).AlG0IKVMxG((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool w2j0ekTbeL(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return this.CJIkqRsjPJ.eC7k3JH9ai > ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).rpw03KsSbE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool Cfn0iX05uU(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return this.CJIkqRsjPJ.I8ZkohiOvo <= ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).Mnr0QSnYqf((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool RRp0cxhL37(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return this.CJIkqRsjPJ.eC7k3JH9ai <= ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).Cnj06t1SA6((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool AlG0IKVMxG(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return this.CJIkqRsjPJ.I8ZkohiOvo < ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).zof028Rdly((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool rpw03KsSbE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return this.CJIkqRsjPJ.eC7k3JH9ai < ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).w2j0ekTbeL((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct Qa3yVQv5SP93sSEW5Ci
    {
      [FieldOffset(0)]
      public byte pAXkFs7qhB;
      [FieldOffset(0)]
      public sbyte Wytkcid1yj;
      [FieldOffset(0)]
      public ushort pSmklQsEFa;
      [FieldOffset(0)]
      public short GnfkH1WnRC;
      [FieldOffset(0)]
      public uint NDJkjpUtcZ;
      [FieldOffset(0)]
      public int GoekDKXc4D;
      [FieldOffset(0)]
      public ulong O5Bk7Tuue4;
      [FieldOffset(0)]
      public long TLWk6n9U4L;
    }

    private class itBkGFvUfKC0vY7aX10 : Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN
    {
      public Bmdacxg2KgloU9v9tT.Qa3yVQv5SP93sSEW5Ci t2Ukewt69e;
      public Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv kZtkyHEGGL;

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.t2Ukewt69e = ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e;
        this.kZtkyHEGGL = ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).kZtkyHEGGL;
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.VPUxGtPN74(_param1);
      }

      public itBkGFvUfKC0vY7aX10(long _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 2;
        this.t2Ukewt69e.TLWk6n9U4L = _param1;
        this.kZtkyHEGGL = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7;
      }

      public itBkGFvUfKC0vY7aX10(Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = _param1.tdv5g7bctk;
        this.t2Ukewt69e.TLWk6n9U4L = _param1.t2Ukewt69e.TLWk6n9U4L;
        this.kZtkyHEGGL = _param1.kZtkyHEGGL;
      }

      public override Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN qh0xUMsisJ()
      {
        return (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this);
      }

      public itBkGFvUfKC0vY7aX10(long _param1, Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 2;
        this.t2Ukewt69e.TLWk6n9U4L = _param1;
        this.kZtkyHEGGL = _param2;
      }

      public itBkGFvUfKC0vY7aX10(ulong _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 2;
        this.t2Ukewt69e.O5Bk7Tuue4 = _param1;
        this.kZtkyHEGGL = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8;
      }

      public itBkGFvUfKC0vY7aX10(ulong _param1, Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 2;
        this.t2Ukewt69e.O5Bk7Tuue4 = _param1;
        this.kZtkyHEGGL = _param2;
      }

      public override bool xlJxxFWp5b()
      {
        return this.kZtkyHEGGL == (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7 ? this.t2Ukewt69e.TLWk6n9U4L == 0L : this.t2Ukewt69e.O5Bk7Tuue4 == 0UL;
      }

      public override bool nNGx0hUd1S() => !this.xlJxxFWp5b();

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD aHlx1rUpXO(
        Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param1)
      {
        switch (_param1)
        {
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.ot6xjGUuHF();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.N9oxsvuhdW();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.xA7xKXq5xu();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.R60xQa0kEN();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.R54x68lelm();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.REix24O3Rn();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.VyUxevyYW6();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.fQfxiEUWtx();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.klKxAPPf0t();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.EwwkZTE7Rn();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 16:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.qh0xUMsisJ();
          default:
            throw new Exception(((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 4).ToString());
        }
      }

      internal override object csZxJgmksq(Type _param1)
      {
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (_param1 == (Type) null || _param1 == typeof (object))
        {
          switch (this.kZtkyHEGGL)
          {
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
              return (object) this.t2Ukewt69e.Wytkcid1yj;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2:
              return (object) this.t2Ukewt69e.pAXkFs7qhB;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
              return (object) this.t2Ukewt69e.GnfkH1WnRC;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4:
              return (object) this.t2Ukewt69e.pSmklQsEFa;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
              return (object) this.t2Ukewt69e.GoekDKXc4D;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6:
              return (object) this.t2Ukewt69e.NDJkjpUtcZ;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7:
              return (object) this.t2Ukewt69e.TLWk6n9U4L;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8:
              return (object) this.t2Ukewt69e.O5Bk7Tuue4;
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
              return (object) this.nNGx0hUd1S();
            case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15:
              return (object) (char) this.t2Ukewt69e.GoekDKXc4D;
            default:
              return (object) this.t2Ukewt69e.TLWk6n9U4L;
          }
        }
        else
        {
          if (_param1 == typeof (int))
            return (object) this.t2Ukewt69e.GoekDKXc4D;
          if (_param1 == typeof (uint))
            return (object) this.t2Ukewt69e.NDJkjpUtcZ;
          if (_param1 == typeof (short))
            return (object) this.t2Ukewt69e.GnfkH1WnRC;
          if (_param1 == typeof (ushort))
            return (object) this.t2Ukewt69e.pSmklQsEFa;
          if (_param1 == typeof (byte))
            return (object) this.t2Ukewt69e.pAXkFs7qhB;
          if (_param1 == typeof (sbyte))
            return (object) this.t2Ukewt69e.Wytkcid1yj;
          if (_param1 == typeof (bool))
            return (object) !this.xlJxxFWp5b();
          if (_param1 == typeof (long))
            return (object) this.t2Ukewt69e.TLWk6n9U4L;
          if (_param1 == typeof (ulong))
            return (object) this.t2Ukewt69e.O5Bk7Tuue4;
          if (_param1 == typeof (char))
            return (object) (char) this.t2Ukewt69e.TLWk6n9U4L;
          return _param1.IsEnum ? this.aIOkwrZp1g(_param1) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        }
      }

      internal object aIOkwrZp1g(Type _param1)
      {
        Type underlyingType = Enum.GetUnderlyingType(_param1);
        if (underlyingType == typeof (int))
          return Enum.ToObject(_param1, this.t2Ukewt69e.GoekDKXc4D);
        if (underlyingType == typeof (uint))
          return Enum.ToObject(_param1, this.t2Ukewt69e.NDJkjpUtcZ);
        if (underlyingType == typeof (short))
          return Enum.ToObject(_param1, this.t2Ukewt69e.GnfkH1WnRC);
        if (underlyingType == typeof (ushort))
          return Enum.ToObject(_param1, this.t2Ukewt69e.pSmklQsEFa);
        if (underlyingType == typeof (byte))
          return Enum.ToObject(_param1, this.t2Ukewt69e.pAXkFs7qhB);
        if (underlyingType == typeof (sbyte))
          return Enum.ToObject(_param1, this.t2Ukewt69e.Wytkcid1yj);
        if (underlyingType == typeof (long))
          return Enum.ToObject(_param1, this.t2Ukewt69e.TLWk6n9U4L);
        if (underlyingType == typeof (ulong))
          return Enum.ToObject(_param1, this.t2Ukewt69e.O5Bk7Tuue4);
        return underlyingType == typeof (char) ? Enum.ToObject(_param1, (ushort) this.t2Ukewt69e.GoekDKXc4D) : Enum.ToObject(_param1, this.t2Ukewt69e.TLWk6n9U4L);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC klKxAPPf0t()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.xlJxxFWp5b() ? 0 : 1);
      }

      internal override bool KOTxELbEBi() => this.nNGx0hUd1S();

      public Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC EwwkZTE7Rn()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) this.t2Ukewt69e.Wytkcid1yj, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ot6xjGUuHF()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) this.t2Ukewt69e.Wytkcid1yj, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC N9oxsvuhdW()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) this.t2Ukewt69e.pAXkFs7qhB, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC xA7xKXq5xu()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) this.t2Ukewt69e.GnfkH1WnRC, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R60xQa0kEN()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) this.t2Ukewt69e.pSmklQsEFa, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R54x68lelm()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.t2Ukewt69e.GoekDKXc4D, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC REix24O3Rn()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.t2Ukewt69e.NDJkjpUtcZ, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VyUxevyYW6()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 fQfxiEUWtx()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.O5Bk7Tuue4, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC TYoxcQ8ydQ() => this.ot6xjGUuHF();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC cacxITg5J8() => this.xA7xKXq5xu();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC YJ0x3w8q27() => this.R54x68lelm();

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 Y61x8yfPbH() => this.VyUxevyYW6();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Cl6xWVWMfh() => this.N9oxsvuhdW();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC hPFxRK7l3X() => this.R60xQa0kEN();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC qCFxH1Sb7q() => this.REix24O3Rn();

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 hVhxveNBwW() => this.fQfxiEUWtx();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC jyyxw6yGFH()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((sbyte) this.t2Ukewt69e.TLWk6n9U4L), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Gj7xdvyCKW()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((sbyte) this.t2Ukewt69e.O5Bk7Tuue4), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pgZxfwQuEP()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((short) this.t2Ukewt69e.TLWk6n9U4L), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pxZxmQMP9n()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((short) this.t2Ukewt69e.O5Bk7Tuue4), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC q8txPhiOdM()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((int) this.t2Ukewt69e.TLWk6n9U4L), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ObkxhydOXc()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((int) this.t2Ukewt69e.O5Bk7Tuue4), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 TWGxrqtIUh()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 ghqxVF9bNj()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked ((long) this.t2Ukewt69e.O5Bk7Tuue4), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC mO5xoicFZZ()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((byte) this.t2Ukewt69e.TLWk6n9U4L), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC r4Jxu3ic6x()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((byte) this.t2Ukewt69e.O5Bk7Tuue4), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC KT4xqQtVPi()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((ushort) this.t2Ukewt69e.TLWk6n9U4L), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DX2xkrAct0()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((ushort) this.t2Ukewt69e.O5Bk7Tuue4), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DStxB4OhTs()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((uint) this.t2Ukewt69e.TLWk6n9U4L), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC uagxYI5BKM()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((uint) this.t2Ukewt69e.O5Bk7Tuue4), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 zG9xDa5XWa()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked ((ulong) this.t2Ukewt69e.TLWk6n9U4L), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VeAxgljEYN()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.O5Bk7Tuue4, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G m18xyPS2QZ()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((float) this.t2Ukewt69e.TLWk6n9U4L);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G n27x71nDuy()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((double) this.t2Ukewt69e.TLWk6n9U4L);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G U0KxNmsThj()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((double) this.t2Ukewt69e.O5Bk7Tuue4);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VZTxtIrfkH()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.Y61x8yfPbH().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.YJ0x3w8q27().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VhvxzNwEWd()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.hVhxveNBwW().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.qCFxH1Sb7q().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx efa0lbWjvg()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.TWGxrqtIUh().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.q8txPhiOdM().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx TL00pic9Va()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.zG9xDa5XWa().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.DStxB4OhTs().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx StD05mXPiK()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.ghqxVF9bNj().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.ObkxhydOXc().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx Kut0CpUwqp()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) checked ((uint) this.t2Ukewt69e.O5Bk7Tuue4));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TiQ0bUSCaT()
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(-this.t2Ukewt69e.TLWk6n9U4L);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Add(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L + ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TXD0ZXksHZ(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked (this.t2Ukewt69e.TLWk6n9U4L + ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L));
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD OPa0LZv7Qy(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked (this.t2Ukewt69e.O5Bk7Tuue4 + ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4));
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD vra0nNVSAN(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L - ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD W8C0F651o0(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked (this.t2Ukewt69e.TLWk6n9U4L - ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L));
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD NmO04Bshno(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked (this.t2Ukewt69e.O5Bk7Tuue4 - ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4));
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD w1K0O2tpMM(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L * ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD y610MW78h1(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked (this.t2Ukewt69e.TLWk6n9U4L * ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L));
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Qi30aYIBJc(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked (this.t2Ukewt69e.O5Bk7Tuue4 * ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4));
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD lEV0TscmDj(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L / ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD JcH0S7ZgWe(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.O5Bk7Tuue4 / ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD XF10XBRNax(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L % ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD wjZ0Gc5iXg(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.O5Bk7Tuue4 % ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD EH309H4f3U(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L & ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD P0M0UAL1ro(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L | ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD woo0xx3hQ9()
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(~this.t2Ukewt69e.TLWk6n9U4L);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD FPe00NLJLE(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L ^ ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD MHo01vyORd(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L << ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.GoekDKXc4D);
        if (_param1.raO0WHR6j4())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L << ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Dti0JwAExR(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L >> ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.GoekDKXc4D);
        if (_param1.raO0WHR6j4())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.TLWk6n9U4L >> ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD A5O0Af5Pfa(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.O5Bk7Tuue4 >> ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.GoekDKXc4D);
        if (_param1.raO0WHR6j4())
          return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(this.t2Ukewt69e.O5Bk7Tuue4 >> ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override string ToString()
      {
        return this.kZtkyHEGGL == (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7 ? this.t2Ukewt69e.TLWk6n9U4L.ToString() : this.t2Ukewt69e.O5Bk7Tuue4.ToString();
      }

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this;
      }

      internal override bool A6a0jc67Sw() => true;

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.riO5EmiZ8q())
          return _param1.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        if (_param1.U4w0RYoANI())
          return _param1.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = _param1.BXg0EpmNON();
        return jr2vZqxigsruTjoD.lIM51cVqgP() && this.t2Ukewt69e.TLWk6n9U4L == ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) jr2vZqxigsruTjoD).t2Ukewt69e.TLWk6n9U4L;
      }

      private static Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN vIYkRN0mWc(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param0)
      {
        if (!(_param0 is Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN) && _param0.U4w0RYoANI())
          kp63vgBuPycmxljcN = _param0.BXg0EpmNON() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN;
        return kp63vgBuPycmxljcN;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.riO5EmiZ8q())
          return false;
        if (_param1.U4w0RYoANI())
          return _param1.FIt0KIs55w((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = _param1.BXg0EpmNON();
        return jr2vZqxigsruTjoD.lIM51cVqgP() && (long) this.t2Ukewt69e.O5Bk7Tuue4 != (long) ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) jr2vZqxigsruTjoD).t2Ukewt69e.O5Bk7Tuue4;
      }

      public override bool Mnr0QSnYqf(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return this.t2Ukewt69e.TLWk6n9U4L >= ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool Cnj06t1SA6(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return this.t2Ukewt69e.O5Bk7Tuue4 >= ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool zof028Rdly(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return this.t2Ukewt69e.TLWk6n9U4L > ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool w2j0ekTbeL(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return this.t2Ukewt69e.O5Bk7Tuue4 > ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool Cfn0iX05uU(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return this.t2Ukewt69e.TLWk6n9U4L <= ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool RRp0cxhL37(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return this.t2Ukewt69e.O5Bk7Tuue4 <= ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool AlG0IKVMxG(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return this.t2Ukewt69e.TLWk6n9U4L < ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.TLWk6n9U4L;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool rpw03KsSbE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.lIM51cVqgP())
          return this.t2Ukewt69e.O5Bk7Tuue4 < ((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) _param1).t2Ukewt69e.O5Bk7Tuue4;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }
    }

    private class jNKJmIvCmU1E1D3nvyx : Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN
    {
      public Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN syL1aZb9is;
      public Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv prC1Jwly74;

      internal void RgmkBBF8qv(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.mwu5kK3wkv())
        {
          this.syL1aZb9is = ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).syL1aZb9is;
          this.prC1Jwly74 = ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).prC1Jwly74;
        }
        else
          this.VPUxGtPN74(_param1);
      }

      internal override unsafe void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.mwu5kK3wkv())
        {
          if (IntPtr.Size == 8)
            *(long*) (void*) new IntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.TLWk6n9U4L) = new IntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).syL1aZb9is).t2Ukewt69e.TLWk6n9U4L).ToInt64();
          else
            *(int*) (void*) new IntPtr(((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) this.syL1aZb9is).CJIkqRsjPJ.I8ZkohiOvo) = new IntPtr(((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param1).syL1aZb9is).CJIkqRsjPJ.I8ZkohiOvo).ToInt32();
        }
        else
        {
          object obj = _param1.csZxJgmksq((Type) null);
          if (obj == null)
            return;
          IntPtr num = IntPtr.Size != 8 ? new IntPtr(((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) this.syL1aZb9is).CJIkqRsjPJ.I8ZkohiOvo) : new IntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.TLWk6n9U4L);
          Type type = obj.GetType();
          if (type == typeof (string))
            return;
          if (type == typeof (byte))
            *(sbyte*) (void*) num = (sbyte) (byte) obj;
          else if (type == typeof (sbyte))
            *(sbyte*) (void*) num = (sbyte) obj;
          else if (type == typeof (short))
            *(short*) (void*) num = (short) obj;
          else if (type == typeof (ushort))
            *(short*) (void*) num = (short) (ushort) obj;
          else if (type == typeof (int))
            *(int*) (void*) num = (int) obj;
          else if (type == typeof (uint))
            *(int*) (void*) num = (int) (uint) obj;
          else if (type == typeof (long))
            *(long*) (void*) num = (long) obj;
          else if (type == typeof (ulong))
            *(long*) (void*) num = (long) (ulong) obj;
          else if (type == typeof (float))
            *(float*) (void*) num = (float) obj;
          else if (type == typeof (double))
            *(double*) (void*) num = (double) obj;
          else if (type == typeof (bool))
            *(sbyte*) (void*) num = (sbyte) (bool) obj;
          else if (type == typeof (IntPtr))
            *(IntPtr*) (void*) num = (IntPtr) obj;
          else if (type == typeof (UIntPtr))
          {
            *(IntPtr*) (void*) num = (IntPtr) obj;
          }
          else
          {
            if (!(type == typeof (char)))
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            *(short*) (void*) num = (short) (char) obj;
          }
        }
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.VPUxGtPN74(_param1);
      }

      public jNKJmIvCmU1E1D3nvyx(IntPtr _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 3;
        if (IntPtr.Size == 8)
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(_param1.ToInt64());
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12;
        }
        else
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(_param1.ToInt32());
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12;
        }
      }

      public jNKJmIvCmU1E1D3nvyx(UIntPtr _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 3;
        if (IntPtr.Size == 8)
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(_param1.ToUInt64());
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12;
        }
        else
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(_param1.ToUInt32());
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12;
        }
      }

      public jNKJmIvCmU1E1D3nvyx()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 3;
        if (IntPtr.Size == 8)
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(0L);
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12;
        }
        else
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0);
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12;
        }
      }

      public override Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN qh0xUMsisJ()
      {
        return (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx()
        {
          syL1aZb9is = this.syL1aZb9is.qh0xUMsisJ(),
          prC1Jwly74 = this.prC1Jwly74
        };
      }

      public jNKJmIvCmU1E1D3nvyx(long _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 3;
        if (IntPtr.Size == 8)
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(_param1);
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12;
        }
        else
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) _param1);
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12;
        }
      }

      public jNKJmIvCmU1E1D3nvyx(long _param1, Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 3;
        if (IntPtr.Size == 8)
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(_param1);
          this.prC1Jwly74 = _param2;
        }
        else
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) _param1);
          this.prC1Jwly74 = _param2;
        }
      }

      public jNKJmIvCmU1E1D3nvyx(ulong _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 4;
        if (IntPtr.Size == 8)
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(_param1);
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 13;
        }
        else
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) _param1);
          this.prC1Jwly74 = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 13;
        }
      }

      public jNKJmIvCmU1E1D3nvyx(ulong _param1, Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 4;
        if (IntPtr.Size == 8)
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(_param1);
          this.prC1Jwly74 = _param2;
        }
        else
        {
          this.syL1aZb9is = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) _param1);
          this.prC1Jwly74 = _param2;
        }
      }

      public override bool xlJxxFWp5b() => this.syL1aZb9is.xlJxxFWp5b();

      public override bool nNGx0hUd1S() => !this.xlJxxFWp5b();

      internal override bool KOTxELbEBi() => this.nNGx0hUd1S();

      internal override bool buM08ZtT1C() => true;

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD aHlx1rUpXO(
        Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param1)
      {
        switch (_param1)
        {
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.ot6xjGUuHF();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.N9oxsvuhdW();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.xA7xKXq5xu();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.R60xQa0kEN();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.R54x68lelm();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.REix24O3Rn();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.VyUxevyYW6();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.fQfxiEUWtx();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.klKxAPPf0t();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 13:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this;
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 16:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.qh0xUMsisJ();
          default:
            throw new Exception(((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 4).ToString());
        }
      }

      internal IntPtr m8XkfgZhhX()
      {
        return IntPtr.Size == 8 ? new IntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.TLWk6n9U4L) : new IntPtr(((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) this.syL1aZb9is).CJIkqRsjPJ.I8ZkohiOvo);
      }

      internal override object csZxJgmksq(Type _param1)
      {
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (_param1 == typeof (IntPtr))
          return IntPtr.Size == 8 ? (object) new IntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.TLWk6n9U4L) : (object) new IntPtr(((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) this.syL1aZb9is).CJIkqRsjPJ.I8ZkohiOvo);
        if (_param1 == typeof (UIntPtr))
          return IntPtr.Size == 8 ? (object) new UIntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.O5Bk7Tuue4) : (object) new UIntPtr(((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) this.syL1aZb9is).CJIkqRsjPJ.eC7k3JH9ai);
        if (!(_param1 == (Type) null) && !(_param1 == typeof (object)))
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (this.prC1Jwly74 == (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12 ? (object) new IntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.TLWk6n9U4L) : (object) new UIntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.O5Bk7Tuue4)) : (this.prC1Jwly74 == (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12 ? (object) new IntPtr(((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.GoekDKXc4D) : (object) new UIntPtr(((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) this.syL1aZb9is).CJIkqRsjPJ.eC7k3JH9ai));
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC klKxAPPf0t()
      {
        return this.syL1aZb9is.klKxAPPf0t();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ot6xjGUuHF()
      {
        return this.syL1aZb9is.ot6xjGUuHF();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC N9oxsvuhdW()
      {
        return this.syL1aZb9is.N9oxsvuhdW();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC xA7xKXq5xu()
      {
        return this.syL1aZb9is.xA7xKXq5xu();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R60xQa0kEN()
      {
        return this.syL1aZb9is.R60xQa0kEN();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R54x68lelm()
      {
        return this.syL1aZb9is.R54x68lelm();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC REix24O3Rn()
      {
        return this.syL1aZb9is.REix24O3Rn();
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VyUxevyYW6()
      {
        return this.syL1aZb9is.VyUxevyYW6();
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 fQfxiEUWtx()
      {
        return this.syL1aZb9is.fQfxiEUWtx();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC TYoxcQ8ydQ() => this.ot6xjGUuHF();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC cacxITg5J8() => this.xA7xKXq5xu();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC YJ0x3w8q27() => this.R54x68lelm();

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 Y61x8yfPbH() => this.VyUxevyYW6();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Cl6xWVWMfh() => this.N9oxsvuhdW();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC hPFxRK7l3X() => this.R60xQa0kEN();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC qCFxH1Sb7q() => this.REix24O3Rn();

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 hVhxveNBwW() => this.fQfxiEUWtx();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC jyyxw6yGFH()
      {
        return this.syL1aZb9is.jyyxw6yGFH();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Gj7xdvyCKW()
      {
        return this.syL1aZb9is.Gj7xdvyCKW();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pgZxfwQuEP()
      {
        return this.syL1aZb9is.pgZxfwQuEP();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pxZxmQMP9n()
      {
        return this.syL1aZb9is.pxZxmQMP9n();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC q8txPhiOdM()
      {
        return this.syL1aZb9is.q8txPhiOdM();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ObkxhydOXc()
      {
        return this.syL1aZb9is.ObkxhydOXc();
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 TWGxrqtIUh()
      {
        return this.syL1aZb9is.TWGxrqtIUh();
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 ghqxVF9bNj()
      {
        return this.syL1aZb9is.ghqxVF9bNj();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC mO5xoicFZZ()
      {
        return this.syL1aZb9is.mO5xoicFZZ();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC r4Jxu3ic6x()
      {
        return this.syL1aZb9is.r4Jxu3ic6x();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC KT4xqQtVPi()
      {
        return this.syL1aZb9is.KT4xqQtVPi();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DX2xkrAct0()
      {
        return this.syL1aZb9is.DX2xkrAct0();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DStxB4OhTs()
      {
        return this.syL1aZb9is.DStxB4OhTs();
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC uagxYI5BKM()
      {
        return this.syL1aZb9is.uagxYI5BKM();
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 zG9xDa5XWa()
      {
        return this.syL1aZb9is.zG9xDa5XWa();
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VeAxgljEYN()
      {
        return this.syL1aZb9is.VeAxgljEYN();
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G m18xyPS2QZ()
      {
        return this.syL1aZb9is.m18xyPS2QZ();
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G n27x71nDuy()
      {
        return this.syL1aZb9is.n27x71nDuy();
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G U0KxNmsThj()
      {
        return this.syL1aZb9is.U0KxNmsThj();
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VZTxtIrfkH()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.Y61x8yfPbH().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.YJ0x3w8q27().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VhvxzNwEWd()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.hVhxveNBwW().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.qCFxH1Sb7q().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx efa0lbWjvg()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.TWGxrqtIUh().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.q8txPhiOdM().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx TL00pic9Va()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.zG9xDa5XWa().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.DStxB4OhTs().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx StD05mXPiK()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.ghqxVF9bNj().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.ObkxhydOXc().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx Kut0CpUwqp()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VeAxgljEYN().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.uagxYI5BKM().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TiQ0bUSCaT()
      {
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(-((Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10) this.syL1aZb9is).t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) -((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) this.syL1aZb9is).CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Add(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L + ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo + ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L + ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo + ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TXD0ZXksHZ(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L + ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo + ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L + ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo + ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD OPa0LZv7Qy(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 + (ulong) ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai + ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 + ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) checked (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai + ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD vra0nNVSAN(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L - ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo - ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L - ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo - ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD heFkxY7UpU(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L - this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo - this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L - this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo - this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD W8C0F651o0(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L - ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo - ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L - ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo - ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD vZRk4h5SVe(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L - this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo - this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L - this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo - this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD NmO04Bshno(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 - (ulong) ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai - ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 - ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) checked (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai - ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD CR5kdkagBc(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked ((ulong) ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai - this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai - this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 - this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) checked (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai - this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD w1K0O2tpMM(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L * ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo * ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L * ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo * ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD y610MW78h1(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L * ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo * ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L * ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo * ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Qi30aYIBJc(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 * (ulong) ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) checked (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai * ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(checked (this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 * ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4)) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) checked (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai * ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD lEV0TscmDj(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L / ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo / ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L / ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo / ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD SZZkArTPCY(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L / this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo / this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L / this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo / this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD JcH0S7ZgWe(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 / ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai / ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 / ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai / ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD S7gkmKrUIG(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 / this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai / this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 / this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai / this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD XF10XBRNax(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L % ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo % ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L % ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo % ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD bmZknLs6mV(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L % this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo % this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L % this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo % this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD wjZ0Gc5iXg(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 % ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai % ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 % ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai % ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD L9CkthgLlF(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 % this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai % this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 % this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) (((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai % this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD EH309H4f3U(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L & ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo & ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L & ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo & ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD P0M0UAL1ro(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L | ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo | ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L | ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo | ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD woo0xx3hQ9()
      {
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(~this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) ~this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD FPe00NLJLE(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L ^ ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo ^ ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L ^ ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo ^ ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD MHo01vyORd(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L << ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo << ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L << ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.GoekDKXc4D) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo << ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Dti0JwAExR(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L >> ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo >> ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L >> ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.GoekDKXc4D) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo >> ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD A5O0Af5Pfa(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 >> ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai >> ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo));
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 >> ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.GoekDKXc4D) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai >> ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD PNqkz8TsTM(
        Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC _param1)
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (_param1.CJIkqRsjPJ.eC7k3JH9ai >> this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD SBh1u7YJ9w(
        Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC _param1)
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (_param1.CJIkqRsjPJ.I8ZkohiOvo >> this.VyUxevyYW6().t2Ukewt69e.GoekDKXc4D));
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD HJw1vMQuj4(
        Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC _param1)
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) (_param1.CJIkqRsjPJ.I8ZkohiOvo << this.VyUxevyYW6().t2Ukewt69e.GoekDKXc4D));
      }

      public override string ToString() => this.syL1aZb9is.ToString();

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this;
      }

      internal override bool A6a0jc67Sw() => true;

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.riO5EmiZ8q())
          return false;
        if (_param1.U4w0RYoANI())
          return _param1.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = _param1.BXg0EpmNON();
        if (!jr2vZqxigsruTjoD.A6a0jc67Sw())
          return false;
        if (jr2vZqxigsruTjoD.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L == ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo == ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        if (!jr2vZqxigsruTjoD.mwu5kK3wkv())
          return false;
        int size = IntPtr.Size;
        return this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L == ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.riO5EmiZ8q())
          return false;
        if (_param1.U4w0RYoANI())
          return _param1.FIt0KIs55w((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = _param1.BXg0EpmNON();
        if (!jr2vZqxigsruTjoD.A6a0jc67Sw())
          return false;
        if (jr2vZqxigsruTjoD.oBL5MGf29A())
          return IntPtr.Size == 8 ? (long) this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 != (long) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : (int) this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai != (int) ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        if (!jr2vZqxigsruTjoD.mwu5kK3wkv())
          return false;
        int size = IntPtr.Size;
        return (long) this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 != (long) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4;
      }

      public override bool Mnr0QSnYqf(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L >= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo >= ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L >= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo >= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo;
      }

      public override bool Cnj06t1SA6(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 >= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai >= ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 >= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai >= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai;
      }

      public override bool zof028Rdly(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L > ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo > ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L > ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo > ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo;
      }

      public override bool w2j0ekTbeL(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 > ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai > ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 > ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai > ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai;
      }

      public override bool Cfn0iX05uU(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L <= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo <= ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L <= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo <= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo;
      }

      public override bool RRp0cxhL37(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 <= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai <= ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 <= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai <= ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai;
      }

      public override bool AlG0IKVMxG(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L < ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo < ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.I8ZkohiOvo;
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L < ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L : this.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo < ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo;
      }

      public override bool rpw03KsSbE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 < ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai < ((Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) _param1).CJIkqRsjPJ.eC7k3JH9ai;
        if (!_param1.mwu5kK3wkv())
          throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
        return IntPtr.Size == 8 ? this.VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 < ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).VyUxevyYW6().t2Ukewt69e.O5Bk7Tuue4 : this.R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai < ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).R54x68lelm().CJIkqRsjPJ.eC7k3JH9ai;
      }
    }

    private abstract class W1KP63vgBUPycmxljcN : Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD
    {
      public abstract bool xlJxxFWp5b();

      public abstract bool nNGx0hUd1S();

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD aHlx1rUpXO(
        Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param1);

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC klKxAPPf0t();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ot6xjGUuHF();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC N9oxsvuhdW();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC xA7xKXq5xu();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R60xQa0kEN();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R54x68lelm();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC REix24O3Rn();

      public abstract Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VyUxevyYW6();

      public abstract Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 fQfxiEUWtx();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC TYoxcQ8ydQ();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC cacxITg5J8();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC YJ0x3w8q27();

      public abstract Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 Y61x8yfPbH();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Cl6xWVWMfh();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC hPFxRK7l3X();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC qCFxH1Sb7q();

      public abstract Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 hVhxveNBwW();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC jyyxw6yGFH();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Gj7xdvyCKW();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pgZxfwQuEP();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pxZxmQMP9n();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC q8txPhiOdM();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ObkxhydOXc();

      public abstract Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 TWGxrqtIUh();

      public abstract Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 ghqxVF9bNj();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC mO5xoicFZZ();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC r4Jxu3ic6x();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC KT4xqQtVPi();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DX2xkrAct0();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DStxB4OhTs();

      public abstract Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC uagxYI5BKM();

      public abstract Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 zG9xDa5XWa();

      public abstract Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VeAxgljEYN();

      public abstract Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G m18xyPS2QZ();

      public abstract Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G n27x71nDuy();

      public abstract Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G U0KxNmsThj();

      public abstract Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VZTxtIrfkH();

      public abstract Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VhvxzNwEWd();

      public abstract Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx efa0lbWjvg();

      public abstract Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx TL00pic9Va();

      public abstract Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx StD05mXPiK();

      public abstract Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx Kut0CpUwqp();

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TiQ0bUSCaT();

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Add(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TXD0ZXksHZ(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD OPa0LZv7Qy(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD vra0nNVSAN(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD W8C0F651o0(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD NmO04Bshno(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD w1K0O2tpMM(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD y610MW78h1(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Qi30aYIBJc(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD lEV0TscmDj(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD JcH0S7ZgWe(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD XF10XBRNax(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD wjZ0Gc5iXg(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD EH309H4f3U(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD P0M0UAL1ro(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD woo0xx3hQ9();

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD FPe00NLJLE(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN qh0xUMsisJ();

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD MHo01vyORd(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Dti0JwAExR(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD A5O0Af5Pfa(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract bool Mnr0QSnYqf(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract bool Cnj06t1SA6(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract bool zof028Rdly(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract bool w2j0ekTbeL(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract bool Cfn0iX05uU(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract bool RRp0cxhL37(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract bool AlG0IKVMxG(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      public abstract bool rpw03KsSbE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      internal override bool raO0WHR6j4() => true;

      protected W1KP63vgBUPycmxljcN()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private class QYRNeVv2bRMHypRuM1G : Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN
    {
      public double UKy1i7YC4g;
      public Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv xLq1Qq3ghO;

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.UKy1i7YC4g = ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        this.xLq1Qq3ghO = ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).xLq1Qq3ghO;
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.VPUxGtPN74(_param1);
      }

      public QYRNeVv2bRMHypRuM1G(double _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 5;
        this.xLq1Qq3ghO = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 10;
        this.UKy1i7YC4g = _param1;
      }

      public QYRNeVv2bRMHypRuM1G(Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = _param1.tdv5g7bctk;
        this.xLq1Qq3ghO = _param1.xLq1Qq3ghO;
        this.UKy1i7YC4g = _param1.UKy1i7YC4g;
      }

      public override Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN qh0xUMsisJ()
      {
        return (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this);
      }

      public QYRNeVv2bRMHypRuM1G(double _param1, Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 5;
        this.UKy1i7YC4g = _param1;
        this.xLq1Qq3ghO = _param2;
      }

      public QYRNeVv2bRMHypRuM1G(float _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 5;
        this.UKy1i7YC4g = (double) _param1;
        this.xLq1Qq3ghO = (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 9;
      }

      public QYRNeVv2bRMHypRuM1G(float _param1, Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 5;
        this.UKy1i7YC4g = (double) _param1;
        this.xLq1Qq3ghO = _param2;
      }

      public override bool xlJxxFWp5b() => this.UKy1i7YC4g == 0.0;

      public override bool nNGx0hUd1S() => !this.xlJxxFWp5b();

      public override string ToString() => this.UKy1i7YC4g.ToString();

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD aHlx1rUpXO(
        Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param1)
      {
        switch (_param1)
        {
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.ot6xjGUuHF();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.N9oxsvuhdW();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.xA7xKXq5xu();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.R60xQa0kEN();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.R54x68lelm();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.REix24O3Rn();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.VyUxevyYW6();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.fQfxiEUWtx();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 9:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.m18xyPS2QZ();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 10:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.n27x71nDuy();
          case (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 11:
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.klKxAPPf0t();
          default:
            throw new Exception(((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 4).ToString());
        }
      }

      internal override object csZxJgmksq(Type _param1)
      {
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (_param1 == typeof (float))
          return (object) (float) this.UKy1i7YC4g;
        if (_param1 == typeof (double))
          return (object) this.UKy1i7YC4g;
        return (_param1 == (Type) null || _param1 == typeof (object)) && this.xLq1Qq3ghO == (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 9 ? (object) (float) this.UKy1i7YC4g : (object) this.UKy1i7YC4g;
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC klKxAPPf0t()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(this.xlJxxFWp5b() ? 1 : 0);
      }

      internal override bool KOTxELbEBi() => this.nNGx0hUd1S();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ot6xjGUuHF()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (sbyte) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC N9oxsvuhdW()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) (byte) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC xA7xKXq5xu()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (short) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R60xQa0kEN()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) (ushort) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC R54x68lelm()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC REix24O3Rn()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VyUxevyYW6()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((long) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 fQfxiEUWtx()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((ulong) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC TYoxcQ8ydQ() => this.ot6xjGUuHF();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC cacxITg5J8() => this.xA7xKXq5xu();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC YJ0x3w8q27() => this.R54x68lelm();

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 Y61x8yfPbH() => this.VyUxevyYW6();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Cl6xWVWMfh() => this.N9oxsvuhdW();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC hPFxRK7l3X() => this.R60xQa0kEN();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC qCFxH1Sb7q() => this.REix24O3Rn();

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 hVhxveNBwW() => this.fQfxiEUWtx();

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC jyyxw6yGFH()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((sbyte) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC Gj7xdvyCKW()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((sbyte) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pgZxfwQuEP()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((short) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC pxZxmQMP9n()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((short) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC q8txPhiOdM()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((int) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC ObkxhydOXc()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((int) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 TWGxrqtIUh()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked ((long) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 ghqxVF9bNj()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked ((long) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC mO5xoicFZZ()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((byte) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC r4Jxu3ic6x()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((byte) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC KT4xqQtVPi()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((ushort) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DX2xkrAct0()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) checked ((ushort) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC DStxB4OhTs()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((uint) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC uagxYI5BKM()
      {
        return new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(checked ((uint) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 zG9xDa5XWa()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked ((ulong) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10 VeAxgljEYN()
      {
        return new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(checked ((ulong) this.UKy1i7YC4g), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G m18xyPS2QZ()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((float) this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 9);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G n27x71nDuy()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 10);
      }

      public override Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G U0KxNmsThj()
      {
        return new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VZTxtIrfkH()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.Y61x8yfPbH().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.YJ0x3w8q27().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx VhvxzNwEWd()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.hVhxveNBwW().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.qCFxH1Sb7q().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx efa0lbWjvg()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.TWGxrqtIUh().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.q8txPhiOdM().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx TL00pic9Va()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.zG9xDa5XWa().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.DStxB4OhTs().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx StD05mXPiK()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.ghqxVF9bNj().t2Ukewt69e.TLWk6n9U4L) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) this.ObkxhydOXc().CJIkqRsjPJ.I8ZkohiOvo);
      }

      public override Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx Kut0CpUwqp()
      {
        return IntPtr.Size == 8 ? new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(this.VeAxgljEYN().t2Ukewt69e.O5Bk7Tuue4) : new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((ulong) this.uagxYI5BKM().CJIkqRsjPJ.eC7k3JH9ai);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TiQ0bUSCaT()
      {
        return this.xLq1Qq3ghO == (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 9 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((float) -this.UKy1i7YC4g) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(-this.UKy1i7YC4g);
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Add(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g + ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD TXD0ZXksHZ(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g + ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD OPa0LZv7Qy(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g + ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD vra0nNVSAN(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g - ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD W8C0F651o0(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g - ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD NmO04Bshno(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g - ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD w1K0O2tpMM(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() && _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g * ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD y610MW78h1(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g * ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Qi30aYIBJc(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g * ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD lEV0TscmDj(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g / ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD JcH0S7ZgWe(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g / ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD XF10XBRNax(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g % ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD wjZ0Gc5iXg(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        return _param1.jHU5KDfZOE() ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(this.UKy1i7YC4g % ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD EH309H4f3U(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD P0M0UAL1ro(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD woo0xx3hQ9()
      {
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD FPe00NLJLE(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD MHo01vyORd(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Dti0JwAExR(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD A5O0Af5Pfa(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this;
      }

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.riO5EmiZ8q())
          return false;
        if (_param1.U4w0RYoANI())
          return _param1.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = _param1.BXg0EpmNON();
        return jr2vZqxigsruTjoD.jHU5KDfZOE() && this.UKy1i7YC4g == ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) jr2vZqxigsruTjoD).UKy1i7YC4g;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.riO5EmiZ8q())
          return false;
        if (_param1.U4w0RYoANI())
          return _param1.FIt0KIs55w((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this);
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = _param1.BXg0EpmNON();
        return jr2vZqxigsruTjoD.jHU5KDfZOE() && this.UKy1i7YC4g != ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) jr2vZqxigsruTjoD).UKy1i7YC4g;
      }

      public override bool Mnr0QSnYqf(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.jHU5KDfZOE())
          return this.UKy1i7YC4g >= ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool Cnj06t1SA6(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.jHU5KDfZOE())
          return this.UKy1i7YC4g >= ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool zof028Rdly(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.jHU5KDfZOE())
          return this.UKy1i7YC4g > ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool w2j0ekTbeL(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.jHU5KDfZOE())
          return this.UKy1i7YC4g > ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool Cfn0iX05uU(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.jHU5KDfZOE())
          return this.UKy1i7YC4g <= ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool RRp0cxhL37(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.jHU5KDfZOE())
          return this.UKy1i7YC4g <= ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool AlG0IKVMxG(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.jHU5KDfZOE())
          return this.UKy1i7YC4g < ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      public override bool rpw03KsSbE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.jHU5KDfZOE())
          return this.UKy1i7YC4g < ((Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G) _param1).UKy1i7YC4g;
        throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }
    }

    internal enum PS9K5DvLjvcEqTiB4Vv : byte
    {
    }

    internal enum NM23HIvOiE3tQLFFD0q : byte
    {
    }

    private class AklhOJv0snb5J6RcyN4 : Exception
    {
      public AklhOJv0snb5J6RcyN4(string _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(_param1);
      }
    }

    private class Vas8K4vGXT59byIK3fW : Exception
    {
      public Vas8K4vGXT59byIK3fW()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      public Vas8K4vGXT59byIK3fW(string _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(_param1);
      }
    }

    internal class lQv41fvVaiFQIGDARSB
    {
      internal Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0 Jjq1TPhy1P;
      internal object fx11sYMeHi;

      public override string ToString()
      {
        object jjq1Tphy1P = (object) this.Jjq1TPhy1P;
        return this.fx11sYMeHi != null ? jjq1Tphy1P.ToString() + 'H'.ToString() + this.fx11sYMeHi.ToString() : jjq1Tphy1P.ToString();
      }

      public lQv41fvVaiFQIGDARSB()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Jjq1TPhy1P = (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 126;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal abstract class jYOWmGvrtl0DafmkxCB : Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD
    {
      public jYOWmGvrtl0DafmkxCB()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      internal override bool U4w0RYoANI() => true;

      internal abstract IntPtr eKG0Hbm1QH();

      internal abstract void Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      internal override bool buM08ZtT1C() => true;
    }

    internal class zK4tiEvSAiB3StdGBuZ : Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB
    {
      private Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp Y4n1b1BGVG;
      internal int kLc1P7Q0vm;

      public zK4tiEvSAiB3StdGBuZ(int _param1, Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Y4n1b1BGVG = _param2;
        this.kLc1P7Q0vm = _param1;
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 7;
      }

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1 is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
        {
          this.Y4n1b1BGVG = ((Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ) _param1).Y4n1b1BGVG;
          this.kLc1P7Q0vm = ((Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ) _param1).kLc1P7Q0vm;
        }
        else
        {
          Bmdacxg2KgloU9v9tT.jWIgTkvhDxGD2JTupj2 wigTkvhDxGd2Jtupj2 = this.Y4n1b1BGVG.xevXHJGR3n.hb71qhcL9p[this.kLc1P7Q0vm];
          if (_param1 is Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB && (wigTkvhDxGd2Jtupj2.On712TVRMl & (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 226) > (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 0)
            this.Qhj0vDlKOA((_param1 as Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB).BXg0EpmNON());
          else
            this.Qhj0vDlKOA(_param1);
        }
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.Qhj0vDlKOA(_param1);
      }

      internal override IntPtr eKG0Hbm1QH() => throw new NotImplementedException();

      internal override void Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.Y4n1b1BGVG.m2bXDcC4Eq[this.kLc1P7Q0vm] = _param1;
      }

      internal override object csZxJgmksq(Type _param1)
      {
        return this.Y4n1b1BGVG.m2bXDcC4Eq[this.kLc1P7Q0vm] == null ? (object) null : this.BXg0EpmNON().csZxJgmksq(_param1);
      }

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return this.Y4n1b1BGVG.m2bXDcC4Eq[this.kLc1P7Q0vm] == null ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null) : this.Y4n1b1BGVG.m2bXDcC4Eq[this.kLc1P7Q0vm].BXg0EpmNON();
      }

      internal override bool A6a0jc67Sw() => this.BXg0EpmNON().A6a0jc67Sw();

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        return _param1.U4w0RYoANI() && _param1 is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ && ((Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ) _param1).kLc1P7Q0vm == this.kLc1P7Q0vm;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        return !_param1.U4w0RYoANI() || !(_param1 is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ) || ((Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ) _param1).kLc1P7Q0vm != this.kLc1P7Q0vm;
      }

      internal override bool KOTxELbEBi() => this.BXg0EpmNON().KOTxELbEBi();
    }

    internal class xsyWZYv984UQApwplkS : Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB
    {
      private Array kuf1WJJe2V;
      internal int Ab31EXZQU6;

      public xsyWZYv984UQApwplkS(int _param1, Array _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.kuf1WJJe2V = _param2;
        this.Ab31EXZQU6 = _param1;
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 7;
      }

      internal override IntPtr eKG0Hbm1QH() => throw new NotImplementedException();

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1 is Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS)
        {
          this.kuf1WJJe2V = ((Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS) _param1).kuf1WJJe2V;
          this.Ab31EXZQU6 = ((Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS) _param1).Ab31EXZQU6;
        }
        else
          this.Qhj0vDlKOA(_param1);
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.Qhj0vDlKOA(_param1);
      }

      internal override void Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.kuf1WJJe2V.SetValue(_param1.csZxJgmksq((Type) null), this.Ab31EXZQU6);
      }

      internal override object csZxJgmksq(Type _param1) => this.BXg0EpmNON().csZxJgmksq(_param1);

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(this.kuf1WJJe2V.GetType().GetElementType(), this.kuf1WJJe2V.GetValue(this.Ab31EXZQU6));
      }

      internal override bool A6a0jc67Sw() => this.BXg0EpmNON().A6a0jc67Sw();

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (!_param1.U4w0RYoANI() || !(_param1 is Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS))
          return false;
        Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS wzYv984UqApwplkS = (Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS) _param1;
        return wzYv984UqApwplkS.Ab31EXZQU6 == this.Ab31EXZQU6 && wzYv984UqApwplkS.kuf1WJJe2V == this.kuf1WJJe2V;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (!_param1.U4w0RYoANI() || !(_param1 is Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS))
          return true;
        Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS wzYv984UqApwplkS = (Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS) _param1;
        return wzYv984UqApwplkS.Ab31EXZQU6 != this.Ab31EXZQU6 || wzYv984UqApwplkS.kuf1WJJe2V != this.kuf1WJJe2V;
      }

      internal override bool KOTxELbEBi() => this.BXg0EpmNON().KOTxELbEBi();
    }

    internal class oiYEAcv8Vi2vmka2Koo : Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB
    {
      internal FieldInfo Vle1MxXTxu;
      internal object zjM1kFcOPO;

      public oiYEAcv8Vi2vmka2Koo(FieldInfo _param1, object _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Vle1MxXTxu = _param1;
        this.zjM1kFcOPO = _param2;
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 7;
      }

      internal override IntPtr eKG0Hbm1QH() => throw new NotImplementedException();

      internal override void Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (this.zjM1kFcOPO != null && this.zjM1kFcOPO is Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD)
          this.Vle1MxXTxu.SetValue(((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.zjM1kFcOPO).csZxJgmksq((Type) null), _param1.csZxJgmksq((Type) null));
        else
          this.Vle1MxXTxu.SetValue(this.zjM1kFcOPO, _param1.csZxJgmksq((Type) null));
      }

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1 is Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo)
        {
          this.Vle1MxXTxu = ((Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo) _param1).Vle1MxXTxu;
          this.zjM1kFcOPO = ((Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo) _param1).zjM1kFcOPO;
        }
        else
          this.Qhj0vDlKOA(_param1);
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.Qhj0vDlKOA(_param1);
      }

      internal override object csZxJgmksq(Type _param1) => this.BXg0EpmNON().csZxJgmksq(_param1);

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return this.zjM1kFcOPO != null && this.zjM1kFcOPO is Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD ? Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(this.Vle1MxXTxu.FieldType, this.Vle1MxXTxu.GetValue(((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.zjM1kFcOPO).csZxJgmksq((Type) null))) : Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(this.Vle1MxXTxu.FieldType, this.Vle1MxXTxu.GetValue(this.zjM1kFcOPO));
      }

      internal override bool A6a0jc67Sw() => this.BXg0EpmNON().A6a0jc67Sw();

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (!_param1.U4w0RYoANI() || !(_param1 is Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo))
          return false;
        Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo yeAcv8Vi2vmka2Koo = (Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo) _param1;
        return !(yeAcv8Vi2vmka2Koo.Vle1MxXTxu != this.Vle1MxXTxu) && yeAcv8Vi2vmka2Koo.zjM1kFcOPO == this.zjM1kFcOPO;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (!_param1.U4w0RYoANI() || !(_param1 is Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo))
          return true;
        Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo yeAcv8Vi2vmka2Koo = (Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo) _param1;
        return yeAcv8Vi2vmka2Koo.Vle1MxXTxu != this.Vle1MxXTxu || yeAcv8Vi2vmka2Koo.zjM1kFcOPO != this.zjM1kFcOPO;
      }

      internal override bool KOTxELbEBi() => this.BXg0EpmNON().KOTxELbEBi();
    }

    internal class OmLvBrv3hR5OabwvL1H : Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB
    {
      private Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp wTW11CMhex;
      internal int mYY1KsdCT6;

      public OmLvBrv3hR5OabwvL1H(int _param1, Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.wTW11CMhex = _param2;
        this.mYY1KsdCT6 = _param1;
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 7;
      }

      internal override IntPtr eKG0Hbm1QH() => throw new NotImplementedException();

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1 is Bmdacxg2KgloU9v9tT.OmLvBrv3hR5OabwvL1H)
        {
          this.wTW11CMhex = ((Bmdacxg2KgloU9v9tT.OmLvBrv3hR5OabwvL1H) _param1).wTW11CMhex;
          this.mYY1KsdCT6 = ((Bmdacxg2KgloU9v9tT.OmLvBrv3hR5OabwvL1H) _param1).mYY1KsdCT6;
        }
        else
          this.Qhj0vDlKOA(_param1);
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.Qhj0vDlKOA(_param1);
      }

      internal override void Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.wTW11CMhex.zeQXjGK0kf[this.mYY1KsdCT6] = _param1;
      }

      internal override object csZxJgmksq(Type _param1)
      {
        return this.wTW11CMhex.zeQXjGK0kf[this.mYY1KsdCT6] == null ? (object) null : this.BXg0EpmNON().csZxJgmksq(_param1);
      }

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return this.wTW11CMhex.zeQXjGK0kf[this.mYY1KsdCT6] == null ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null) : this.wTW11CMhex.zeQXjGK0kf[this.mYY1KsdCT6].BXg0EpmNON();
      }

      internal override bool A6a0jc67Sw() => this.BXg0EpmNON().A6a0jc67Sw();

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        return _param1.U4w0RYoANI() && _param1 is Bmdacxg2KgloU9v9tT.OmLvBrv3hR5OabwvL1H && ((Bmdacxg2KgloU9v9tT.OmLvBrv3hR5OabwvL1H) _param1).mYY1KsdCT6 == this.mYY1KsdCT6;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        return !_param1.U4w0RYoANI() || !(_param1 is Bmdacxg2KgloU9v9tT.OmLvBrv3hR5OabwvL1H) || ((Bmdacxg2KgloU9v9tT.OmLvBrv3hR5OabwvL1H) _param1).mYY1KsdCT6 != this.mYY1KsdCT6;
      }

      internal override bool KOTxELbEBi() => this.BXg0EpmNON().KOTxELbEBi();
    }

    internal class DTlXqRvotj3wod6FC4n : Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB
    {
      private Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Xo41I5ZXeB;
      private Type mlV1X3UZaP;

      public DTlXqRvotj3wod6FC4n(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1, Type _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Xo41I5ZXeB = _param1;
        this.mlV1X3UZaP = _param2;
        this.tdv5g7bctk = (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 7;
      }

      internal override IntPtr eKG0Hbm1QH() => throw new NotImplementedException();

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1 is Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n)
        {
          this.mlV1X3UZaP = ((Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n) _param1).mlV1X3UZaP;
          this.Xo41I5ZXeB = ((Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n) _param1).Xo41I5ZXeB;
        }
        else
          this.Xo41I5ZXeB.VPUxGtPN74(_param1);
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.Qhj0vDlKOA(_param1);
      }

      internal override void Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.Xo41I5ZXeB = _param1;
      }

      internal override object csZxJgmksq(Type _param1)
      {
        if (this.Xo41I5ZXeB == null)
          return (object) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null);
        return _param1 == (Type) null || _param1 == typeof (object) ? this.Xo41I5ZXeB.csZxJgmksq(this.mlV1X3UZaP) : this.Xo41I5ZXeB.csZxJgmksq(_param1);
      }

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return this.Xo41I5ZXeB == null ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null) : this.Xo41I5ZXeB.BXg0EpmNON();
      }

      internal override bool A6a0jc67Sw() => this.BXg0EpmNON().A6a0jc67Sw();

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (!_param1.U4w0RYoANI() || !(_param1 is Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n))
          return false;
        Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n xqRvotj3wod6Fc4n = (Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n) _param1;
        if (xqRvotj3wod6Fc4n.mlV1X3UZaP != this.mlV1X3UZaP)
          return false;
        if (this.Xo41I5ZXeB != null)
          return this.Xo41I5ZXeB.SXw0swpnLE(xqRvotj3wod6Fc4n.Xo41I5ZXeB);
        return xqRvotj3wod6Fc4n.Xo41I5ZXeB == null;
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (!_param1.U4w0RYoANI() || !(_param1 is Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n))
          return true;
        Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n xqRvotj3wod6Fc4n = (Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n) _param1;
        if (xqRvotj3wod6Fc4n.mlV1X3UZaP != this.mlV1X3UZaP)
          return true;
        if (this.Xo41I5ZXeB != null)
          return this.Xo41I5ZXeB.FIt0KIs55w(xqRvotj3wod6Fc4n.Xo41I5ZXeB);
        return xqRvotj3wod6Fc4n.Xo41I5ZXeB != null;
      }

      internal override bool KOTxELbEBi() => this.BXg0EpmNON().KOTxELbEBi();
    }

    internal class c1XGWVvY6wNnQ8CbqqX
    {
      public int vsE15XSYqq;
      public bool rnf1U6Te88;
      public Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv BQw1Cc3bH0;

      public c1XGWVvY6wNnQ8CbqqX()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class jWIgTkvhDxGD2JTupj2
    {
      public int irh1gs4hgA;
      public Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv On712TVRMl;
      public bool f8S1LJGfI1;
      public Type zsf1O1MsJp;

      public jWIgTkvhDxGD2JTupj2()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.zsf1O1MsJp = typeof (object);
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class xIMe2cvNLuVvVun8vad
    {
      public int kxu10mh8H0;
      public int BVa1GI0rhE;
      public Bmdacxg2KgloU9v9tT.tWRYqQvqSCFAC7p6maF a3i1VrsFOu;

      public xIMe2cvNLuVvVun8vad()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class tWRYqQvqSCFAC7p6maF
    {
      public int MSW1rkABlh;
      public int Tvd1SZGoAq;
      public byte B7K19F7uZm;
      public Type CZ118rhm4y;
      public int wuT13lKOjC;
      public int olp1oNVS7m;

      public tWRYqQvqSCFAC7p6maF()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class hrYJqyvpMWJRBsclFfW
    {
      internal MethodBase c1I1Y59VD3;
      internal List<Bmdacxg2KgloU9v9tT.lQv41fvVaiFQIGDARSB> yN41ho6cU2;
      internal Bmdacxg2KgloU9v9tT.c1XGWVvY6wNnQ8CbqqX[] A6N1NPB7k9;
      internal List<Bmdacxg2KgloU9v9tT.jWIgTkvhDxGD2JTupj2> hb71qhcL9p;
      internal List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad> yPi1pye7aO;

      public hrYJqyvpMWJRBsclFfW()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private class LLVqX5vFSGw3JpGZJL2
    {
      internal FieldInfo wwd1FAYGTS;
      internal int sfl1ctQi2m;

      public LLVqX5vFSGw3JpGZJL2(FieldInfo _param1, int _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.wwd1FAYGTS = _param1;
        this.sfl1ctQi2m = _param2;
      }
    }

    private class N40wiAvcU3KnSgBj8RY
    {
      private List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2> YL41jcZqpE;
      private MethodBase STo1DmpRue;

      public N40wiAvcU3KnSgBj8RY(
        MethodBase _param1,
        List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2> _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.YL41jcZqpE = new List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.YL41jcZqpE = _param2;
        this.STo1DmpRue = _param1;
      }

      public N40wiAvcU3KnSgBj8RY(
        MethodBase _param1,
        Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2[] _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.YL41jcZqpE = new List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.YL41jcZqpE.AddRange((IEnumerable<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2>) _param2);
      }

      public override bool Equals(object _param1)
      {
        Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY n40wiAvcU3KnSgBj8Ry = _param1 as Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY;
        if (_param1 == null || this.STo1DmpRue != n40wiAvcU3KnSgBj8Ry.STo1DmpRue || this.YL41jcZqpE.Count != n40wiAvcU3KnSgBj8Ry.YL41jcZqpE.Count)
          return false;
        for (int index = 0; index < this.YL41jcZqpE.Count; ++index)
        {
          if (this.YL41jcZqpE[index].wwd1FAYGTS != n40wiAvcU3KnSgBj8Ry.YL41jcZqpE[index].wwd1FAYGTS || this.YL41jcZqpE[index].sfl1ctQi2m != n40wiAvcU3KnSgBj8Ry.YL41jcZqpE[index].sfl1ctQi2m)
            return false;
        }
        return true;
      }

      public override int GetHashCode()
      {
        int hashCode = this.STo1DmpRue.GetHashCode();
        foreach (Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2 vqX5vFsGw3JpGzjL2 in this.YL41jcZqpE)
        {
          int num = vqX5vFsGw3JpGzjL2.wwd1FAYGTS.GetHashCode() + vqX5vFsGw3JpGzjL2.sfl1ctQi2m;
          hashCode = (hashCode ^ num) + num;
        }
        return hashCode;
      }

      public Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2 V4W1lwKj5l(int _param1)
      {
        foreach (Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2 vqX5vFsGw3JpGzjL2 in this.YL41jcZqpE)
        {
          if (vqX5vFsGw3JpGzjL2.sfl1ctQi2m == _param1)
            return vqX5vFsGw3JpGzjL2;
        }
        return (Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2) null;
      }

      public bool KeR1HeJLTk(int _param1)
      {
        foreach (Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2 vqX5vFsGw3JpGzjL2 in this.YL41jcZqpE)
        {
          if (vqX5vFsGw3JpGzjL2.sfl1ctQi2m == _param1)
            return true;
        }
        return false;
      }
    }

    private delegate object H5Q2MlvlVOFqkHslKTO(object target, object[] paramters);

    private delegate object PachvPvHCyoyZLygZ3G(object target);

    private delegate void qd6j41vjSCTQZfupK3g(IntPtr a, byte b, int c);

    private delegate void xeSy0nvDBi7Um10QQWP(IntPtr s, IntPtr t, uint c);

    internal class gR72hDv7XQltkgXgRlp
    {
      internal Bmdacxg2KgloU9v9tT.hrYJqyvpMWJRBsclFfW xevXHJGR3n;
      internal Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[] zeQXjGK0kf;
      internal Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[] m2bXDcC4Eq;
      internal Bmdacxg2KgloU9v9tT.v2STNJvydw751BYOUX4 fEbX7JSSW8;
      internal Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BoDX6FHCap;
      internal Exception LCEXwZCAWl;
      internal List<IntPtr> hAlXZSQxJN;
      private int CAVXR58N0b;
      private int R1hXe4a9VY;
      private int utjXyWaDwK;
      private object B1fXBBCnkj;
      private bool dw0XfTIcpf;
      private bool iQmXxrTu9n;
      private bool L9DX4iwue2;
      private bool J2MXdZ5ps1;
      private static Dictionary<Type, int> AIIXARK6iP;
      private static object UxMXm7pNtX;
      private static Dictionary<object, Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD> T5LXnnjUM8;
      private static object I6TXt25Fro;
      private static Dictionary<MethodBase, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO> hNyXze0LxK;
      private static Dictionary<MethodBase, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO> APB5uME0jL;
      private static object xs25vnvuVR;
      private static Dictionary<Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO> uBo5a3c77c;
      private static Dictionary<Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO> rhC5JomQI4;
      private static object Ldb5ixhB4X;
      private static Dictionary<Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO> l8U5QBixHo;
      private static object x1O5TkrH6u;
      private static Dictionary<Type, Bmdacxg2KgloU9v9tT.PachvPvHCyoyZLygZ3G> tUx5shZpvN;
      private static object NTf5bU5PVY;
      private static Bmdacxg2KgloU9v9tT.qd6j41vjSCTQZfupK3g jSL5P12kQw;
      private static Bmdacxg2KgloU9v9tT.xeSy0nvDBi7Um10QQWP eCT5WLagGQ;

      internal void Xr317km48e()
      {
        bool flag = false;
        this.AZm1wy02We(ref flag);
      }

      internal void BsJ16rMxkW()
      {
        this.fEbX7JSSW8.q7750uuUI6();
        this.m2bXDcC4Eq = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[]) null;
        if (this.hAlXZSQxJN == null)
          return;
        foreach (IntPtr hglobal in this.hAlXZSQxJN)
        {
          try
          {
            Marshal.FreeHGlobal(hglobal);
          }
          catch
          {
          }
        }
        this.hAlXZSQxJN.Clear();
        this.hAlXZSQxJN = (List<IntPtr>) null;
      }

      internal void AZm1wy02We(ref bool _param1)
      {
        for (; this.CAVXR58N0b > -2; ++this.CAVXR58N0b)
        {
          if (this.dw0XfTIcpf)
          {
            this.dw0XfTIcpf = false;
            int r1hXe4a9Vy = this.R1hXe4a9VY;
            int cavxR58N0b = this.CAVXR58N0b;
            this.CnN1RxoJ1Y(this.R1hXe4a9VY, this.CAVXR58N0b);
            this.CAVXR58N0b = cavxR58N0b;
            this.R1hXe4a9VY = r1hXe4a9Vy;
          }
          if (this.L9DX4iwue2)
          {
            this.L9DX4iwue2 = false;
            return;
          }
          if (this.iQmXxrTu9n)
          {
            this.iQmXxrTu9n = false;
            return;
          }
          this.R1hXe4a9VY = this.CAVXR58N0b;
          Bmdacxg2KgloU9v9tT.lQv41fvVaiFQIGDARSB qv41fvVaiFqigdarsb = this.xevXHJGR3n.yN41ho6cU2[this.CAVXR58N0b];
          this.B1fXBBCnkj = qv41fvVaiFqigdarsb.fx11sYMeHi;
          try
          {
            this.D4H1BiIAs3(qv41fvVaiFqigdarsb);
          }
          catch (Exception ex)
          {
            Exception exception = ex;
            if (exception is TargetInvocationException)
            {
              TargetInvocationException invocationException = (TargetInvocationException) exception;
              if (invocationException.InnerException != null)
                exception = invocationException.InnerException;
            }
            this.LCEXwZCAWl = exception;
            _param1 = true;
            this.fEbX7JSSW8.q7750uuUI6();
            int r1hXe4a9Vy = this.R1hXe4a9VY;
            Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad1 = this.IyY1eBYTfe(r1hXe4a9Vy, exception);
            List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad> collection = this.H2e1y4vnTf(r1hXe4a9Vy, false);
            List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad> ime2cvNluVvVun8vadList = new List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad>();
            if (ime2cvNluVvVun8vad1 != null)
              ime2cvNluVvVun8vadList.Add(ime2cvNluVvVun8vad1);
            if (collection != null && collection.Count > 0)
              ime2cvNluVvVun8vadList.AddRange((IEnumerable<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad>) collection);
            ime2cvNluVvVun8vadList.Sort((Comparison<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad>) ((x, y) => x.a3i1VrsFOu.MSW1rkABlh.CompareTo(y.a3i1VrsFOu.MSW1rkABlh)));
            Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad2 = (Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad) null;
            foreach (Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad3 in ime2cvNluVvVun8vadList)
            {
              if (ime2cvNluVvVun8vad3.a3i1VrsFOu.olp1oNVS7m == 0)
              {
                ime2cvNluVvVun8vad2 = ime2cvNluVvVun8vad3;
                break;
              }
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) exception));
              this.R1hXe4a9VY = ime2cvNluVvVun8vad3.a3i1VrsFOu.wuT13lKOjC;
              this.CAVXR58N0b = this.R1hXe4a9VY;
              this.Xr317km48e();
              if (this.J2MXdZ5ps1)
              {
                this.J2MXdZ5ps1 = false;
                ime2cvNluVvVun8vad2 = ime2cvNluVvVun8vad3;
                break;
              }
            }
            if (ime2cvNluVvVun8vad2 == null)
              throw exception;
            this.utjXyWaDwK = ime2cvNluVvVun8vad2.a3i1VrsFOu.MSW1rkABlh;
            this.g1t1ZsHgwq(r1hXe4a9Vy, ime2cvNluVvVun8vad2.a3i1VrsFOu.MSW1rkABlh);
            if (this.utjXyWaDwK < 0)
              return;
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) exception));
            this.R1hXe4a9VY = this.utjXyWaDwK;
            this.CAVXR58N0b = this.R1hXe4a9VY;
            this.utjXyWaDwK = -1;
            this.Xr317km48e();
            return;
          }
        }
        this.fEbX7JSSW8.q7750uuUI6();
      }

      internal void g1t1ZsHgwq(int _param1, int _param2)
      {
        if (this.xevXHJGR3n.yPi1pye7aO == null)
          return;
        foreach (Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad in this.xevXHJGR3n.yPi1pye7aO)
        {
          if ((ime2cvNluVvVun8vad.a3i1VrsFOu.olp1oNVS7m == 4 || ime2cvNluVvVun8vad.a3i1VrsFOu.olp1oNVS7m == 2) && ime2cvNluVvVun8vad.a3i1VrsFOu.MSW1rkABlh >= _param1 && ime2cvNluVvVun8vad.a3i1VrsFOu.Tvd1SZGoAq <= _param2)
          {
            this.R1hXe4a9VY = ime2cvNluVvVun8vad.a3i1VrsFOu.MSW1rkABlh;
            this.CAVXR58N0b = this.R1hXe4a9VY;
            bool flag = false;
            this.AZm1wy02We(ref flag);
            if (flag)
              break;
          }
        }
      }

      internal void CnN1RxoJ1Y(int _param1, int _param2)
      {
        if (this.xevXHJGR3n.yPi1pye7aO == null)
          return;
        foreach (Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad in this.xevXHJGR3n.yPi1pye7aO)
        {
          if (ime2cvNluVvVun8vad.a3i1VrsFOu.olp1oNVS7m == 2 && ime2cvNluVvVun8vad.a3i1VrsFOu.MSW1rkABlh >= _param1 && ime2cvNluVvVun8vad.a3i1VrsFOu.Tvd1SZGoAq <= _param2)
          {
            this.R1hXe4a9VY = ime2cvNluVvVun8vad.a3i1VrsFOu.MSW1rkABlh;
            this.CAVXR58N0b = this.R1hXe4a9VY;
            bool flag = false;
            this.AZm1wy02We(ref flag);
            if (flag)
              break;
          }
        }
      }

      internal Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad IyY1eBYTfe(int _param1, Exception _param2)
      {
        Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad1 = (Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad) null;
        if (this.xevXHJGR3n.yPi1pye7aO != null)
        {
          foreach (Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad2 in this.xevXHJGR3n.yPi1pye7aO)
          {
            if (ime2cvNluVvVun8vad2.a3i1VrsFOu != null && ime2cvNluVvVun8vad2.a3i1VrsFOu.olp1oNVS7m == 0 && (ime2cvNluVvVun8vad2.a3i1VrsFOu.CZ118rhm4y == _param2.GetType() || ime2cvNluVvVun8vad2.a3i1VrsFOu.CZ118rhm4y != (Type) null && (ime2cvNluVvVun8vad2.a3i1VrsFOu.CZ118rhm4y.FullName == _param2.GetType().FullName || ime2cvNluVvVun8vad2.a3i1VrsFOu.CZ118rhm4y.FullName == typeof (object).FullName || ime2cvNluVvVun8vad2.a3i1VrsFOu.CZ118rhm4y.FullName == typeof (Exception).FullName)) && _param1 >= ime2cvNluVvVun8vad2.kxu10mh8H0 && _param1 <= ime2cvNluVvVun8vad2.BVa1GI0rhE)
            {
              if (ime2cvNluVvVun8vad1 == null)
                ime2cvNluVvVun8vad1 = ime2cvNluVvVun8vad2;
              else if (ime2cvNluVvVun8vad2.a3i1VrsFOu.MSW1rkABlh < ime2cvNluVvVun8vad1.a3i1VrsFOu.MSW1rkABlh)
                ime2cvNluVvVun8vad1 = ime2cvNluVvVun8vad2;
            }
          }
        }
        return ime2cvNluVvVun8vad1;
      }

      internal List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad> H2e1y4vnTf(int _param1, bool _param2)
      {
        if (this.xevXHJGR3n.yPi1pye7aO == null)
          return (List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad>) null;
        List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad> ime2cvNluVvVun8vadList = new List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad>();
        foreach (Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad ime2cvNluVvVun8vad in this.xevXHJGR3n.yPi1pye7aO)
        {
          if ((ime2cvNluVvVun8vad.a3i1VrsFOu.olp1oNVS7m & 1) == 1 && _param1 >= ime2cvNluVvVun8vad.kxu10mh8H0 && _param1 <= ime2cvNluVvVun8vad.BVa1GI0rhE)
            ime2cvNluVvVun8vadList.Add(ime2cvNluVvVun8vad);
        }
        return ime2cvNluVvVun8vadList.Count == 0 ? (List<Bmdacxg2KgloU9v9tT.xIMe2cvNLuVvVun8vad>) null : ime2cvNluVvVun8vadList;
      }

      private unsafe void D4H1BiIAs3(Bmdacxg2KgloU9v9tT.lQv41fvVaiFQIGDARSB _param1)
      {
        object obj1;
        switch (_param1.Jjq1TPhy1P)
        {
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 0:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 15:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 54:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 59:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 98:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 110:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 126:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 166:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD1 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN1 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Array array1 = (Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null);
            Type elementType = array1.GetType().GetElementType();
            array1.SetValue(jr2vZqxigsruTjoD1.csZxJgmksq(elementType), kp63vgBuPycmxljcN1.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 1:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD2 = this.fEbX7JSSW8.Wv25rccvJo();
            int num1 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).rpw03KsSbE(jr2vZqxigsruTjoD2) ? 1 : 0;
            if (num1 != 0)
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1));
            else
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0));
            if (num1 == 0)
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 2:
            this.L9DX4iwue2 = true;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 3:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).DStxB4OhTs());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 4:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.OmLvBrv3hR5OabwvL1H((int) this.B1fXBBCnkj, this));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 5:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN2 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (double), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN2.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 6:
            FieldInfo fieldInfo1 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveField((int) this.B1fXBBCnkj);
            object obj2 = this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq(fieldInfo1.FieldType);
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD3 = this.fEbX7JSSW8.Wv25rccvJo();
            object obj3 = jr2vZqxigsruTjoD3.csZxJgmksq((Type) null);
            if (obj3 == null)
            {
              Type type = fieldInfo1.DeclaringType;
              if (type.IsByRef)
                type = type.GetElementType();
              obj3 = type.IsValueType ? Activator.CreateInstance(type) : throw new NullReferenceException();
              if (jr2vZqxigsruTjoD3 is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
                ((Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB) jr2vZqxigsruTjoD3).Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type, obj3));
            }
            fieldInfo1.SetValue(obj3, obj2);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 8:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD4 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD4.raO0WHR6j4())
              jr2vZqxigsruTjoD4 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) jr2vZqxigsruTjoD4).cacxITg5J8();
            this.fEbX7JSSW8.Wv25rccvJo().aIpx9u9Kmq(jr2vZqxigsruTjoD4);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 9:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 19:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 107:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 133:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 140:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 154:
            throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 10:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN3 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN4 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN4 == null || kp63vgBuPycmxljcN3 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN4.A5O0Af5Pfa((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN3));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 12:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD5 = this.fEbX7JSSW8.Wv25rccvJo();
            if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).rpw03KsSbE(jr2vZqxigsruTjoD5))
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1));
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 13:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).TWGxrqtIUh());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 14:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).TYoxcQ8ydQ());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 16:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD6 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN5 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD6);
            if (jr2vZqxigsruTjoD6 != null && jr2vZqxigsruTjoD6.U4w0RYoANI() && kp63vgBuPycmxljcN5 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN5.cacxITg5J8());
              break;
            }
            if (kp63vgBuPycmxljcN5 == null || !kp63vgBuPycmxljcN5.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) *(short*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN5).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 17:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).StD05mXPiK());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 18:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN6 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN7 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN7 == null || kp63vgBuPycmxljcN6 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN7.lEV0TscmDj((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN6));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 20:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN8 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN9 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN9 == null || kp63vgBuPycmxljcN8 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN9.w1K0O2tpMM((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN8));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 21:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN10 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN11 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN11 == null || kp63vgBuPycmxljcN10 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN11.XF10XBRNax((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN10));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 22:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN12 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (short), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN12.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 23:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD7 = this.fEbX7JSSW8.Wv25rccvJo();
            if (!Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).AlG0IKVMxG(jr2vZqxigsruTjoD7))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 24:
            MethodBase methodBase1 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveMethod((int) this.B1fXBBCnkj);
            Type type1 = this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null).GetType();
            List<Type> typeList = new List<Type>();
            do
            {
              typeList.Add(type1);
              type1 = type1.BaseType;
            }
            while (type1 != (Type) null && type1 != methodBase1.DeclaringType);
            typeList.Reverse();
            MethodBase methodBase2 = methodBase1;
            foreach (Type type2 in typeList)
            {
              foreach (MethodInfo method in type2.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
              {
                if ((MethodBase) method.GetBaseDefinition() == methodBase2)
                {
                  methodBase2 = (MethodBase) method;
                  break;
                }
              }
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(methodBase2.MethodHandle.GetFunctionPointer()));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 25:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).woo0xx3hQ9());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 26:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN13 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN14 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN14 == null || kp63vgBuPycmxljcN13 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN14.MHo01vyORd((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN13));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 27:
            this.F6OX8o0nsL(true);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 28:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD8 = this.fEbX7JSSW8.Wv25rccvJo();
            if (!Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).Mnr0QSnYqf(jr2vZqxigsruTjoD8))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 29:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).uagxYI5BKM());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 30:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD9 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD9 != null && jr2vZqxigsruTjoD9.KOTxELbEBi())
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 31:
            throw this.LCEXwZCAWl;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 32:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).Length, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 33:
            int b1fXbbCnkj1 = (int) this.B1fXBBCnkj;
            Module module = typeof (Bmdacxg2KgloU9v9tT).Module;
            obj1 = (object) null;
            object obj4;
            try
            {
              obj4 = (object) module.ResolveType(b1fXbbCnkj1);
            }
            catch
            {
              try
              {
                obj4 = (object) module.ResolveMethod(b1fXbbCnkj1);
              }
              catch
              {
                try
                {
                  obj4 = (object) module.ResolveField(b1fXbbCnkj1);
                }
                catch
                {
                  obj4 = (object) module.ResolveMember(b1fXbbCnkj1);
                }
              }
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab(obj4));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 34:
            FieldInfo fieldInfo2 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveField((int) this.B1fXBBCnkj);
            object obj5 = this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq(fieldInfo2.FieldType);
            fieldInfo2.SetValue((object) null, obj5);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 35:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD10 = this.fEbX7JSSW8.Wv25rccvJo();
            if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).zof028Rdly(jr2vZqxigsruTjoD10))
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1));
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 36:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN15 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Array array2 = (Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null);
            object obj6 = array2.GetValue(kp63vgBuPycmxljcN15.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(array2.GetType().GetElementType(), obj6));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 37:
            this.fEbX7JSSW8.x1C5GFNwKo(((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) this.fEbX7JSSW8.Wv25rccvJo()).TiQ0bUSCaT());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 38:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD11 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN16 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD11);
            if (jr2vZqxigsruTjoD11 != null && jr2vZqxigsruTjoD11.U4w0RYoANI() && kp63vgBuPycmxljcN16 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN16.hPFxRK7l3X());
              break;
            }
            if (kp63vgBuPycmxljcN16 == null || !kp63vgBuPycmxljcN16.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) *(ushort*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN16).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 39:
            lock (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.I6TXt25Fro)
            {
              object key = this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null);
              Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD12 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) null;
              if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.T5LXnnjUM8.TryGetValue(key, out jr2vZqxigsruTjoD12))
              {
                this.fEbX7JSSW8.x1C5GFNwKo(jr2vZqxigsruTjoD12);
                break;
              }
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null));
              break;
            }
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 40:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD13 = this.fEbX7JSSW8.Wv25rccvJo();
            object obj7 = jr2vZqxigsruTjoD13.U4w0RYoANI() ? jr2vZqxigsruTjoD13.csZxJgmksq((Type) null) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(obj7 == null ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null) : Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(obj7.GetType(), obj7));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 41:
            FieldInfo fieldInfo3 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveField((int) this.B1fXBBCnkj);
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD14 = this.fEbX7JSSW8.Wv25rccvJo();
            jr2vZqxigsruTjoD14.BXg0EpmNON();
            object obj8 = jr2vZqxigsruTjoD14.csZxJgmksq((Type) null);
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo(fieldInfo3, obj8));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 42:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).VZTxtIrfkH());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 43:
            this.fEbX7JSSW8.x1C5GFNwKo(this.fEbX7JSSW8.wFI5VtoADD());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 44:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN17 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN18 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN18 == null || kp63vgBuPycmxljcN17 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN18.NmO04Bshno((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN17));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 45:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD15 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD15 == null || !jr2vZqxigsruTjoD15.KOTxELbEBi())
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 46:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD16 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN19 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD16);
            if (jr2vZqxigsruTjoD16 != null && jr2vZqxigsruTjoD16.U4w0RYoANI() && kp63vgBuPycmxljcN19 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN19.Cl6xWVWMfh());
              break;
            }
            if (kp63vgBuPycmxljcN19 == null || !kp63vgBuPycmxljcN19.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) *(byte*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN19).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 47:
            throw (Exception) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null);
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 48:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD17 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD17.raO0WHR6j4())
              jr2vZqxigsruTjoD17 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) jr2vZqxigsruTjoD17).m18xyPS2QZ();
            this.fEbX7JSSW8.Wv25rccvJo().aIpx9u9Kmq(jr2vZqxigsruTjoD17);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 49:
            this.fEbX7JSSW8.x1C5GFNwKo(this.fEbX7JSSW8.Wv25rccvJo().BXg0EpmNON());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 50:
            FieldInfo fieldInfo4 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveField((int) this.B1fXBBCnkj);
            object obj9 = this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null);
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(fieldInfo4.FieldType, fieldInfo4.GetValue(obj9)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 51:
            IntPtr num2 = Marshal.AllocHGlobal((this.fEbX7JSSW8.Wv25rccvJo() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
            if (this.hAlXZSQxJN == null)
              this.hAlXZSQxJN = new List<IntPtr>();
            this.hAlXZSQxJN.Add(num2);
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(num2));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 52:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN20 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (uint), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN20.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 53:
            int[] b1fXbbCnkj2 = (int[]) this.B1fXBBCnkj;
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN21 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            long index1 = kp63vgBuPycmxljcN21.VyUxevyYW6().t2Ukewt69e.TLWk6n9U4L;
            if ((index1 < 0L || kp63vgBuPycmxljcN21.jHU5KDfZOE()) && IntPtr.Size == 4)
              index1 = (long) (int) index1;
            if (kp63vgBuPycmxljcN21.oBL5MGf29A())
            {
              Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC oofhvXaxlsKuDwlgC = (Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC) kp63vgBuPycmxljcN21;
              if (oofhvXaxlsKuDwlgC.tPmkpCFndg == (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6)
                index1 = (long) oofhvXaxlsKuDwlgC.CJIkqRsjPJ.eC7k3JH9ai;
            }
            if (index1 >= (long) b1fXbbCnkj2.Length || index1 < 0L)
              break;
            this.CAVXR58N0b = b1fXbbCnkj2[index1] - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 55:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN22 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN23 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN22 == null || kp63vgBuPycmxljcN23 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN22.EH309H4f3U((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN23));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 56:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).m18xyPS2QZ());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 57:
            if (!this.fEbX7JSSW8.Wv25rccvJo().FIt0KIs55w(this.fEbX7JSSW8.Wv25rccvJo()))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 58:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).ObkxhydOXc());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 60:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN24 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN25 = (Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) this.fEbX7JSSW8.Wv25rccvJo();
            if (kp63vgBuPycmxljcN25 == null || kp63vgBuPycmxljcN24 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN25.W8C0F651o0((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN24));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 61:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD18 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD18.raO0WHR6j4())
              jr2vZqxigsruTjoD18 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) jr2vZqxigsruTjoD18).VZTxtIrfkH();
            this.fEbX7JSSW8.Wv25rccvJo().aIpx9u9Kmq(jr2vZqxigsruTjoD18);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 62:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD19 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN26 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD19);
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD20 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN27 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD20);
            if (kp63vgBuPycmxljcN27 == null || kp63vgBuPycmxljcN26 == null)
            {
              if (jr2vZqxigsruTjoD19.FIt0KIs55w(jr2vZqxigsruTjoD20))
              {
                this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1));
                break;
              }
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0));
              break;
            }
            if (kp63vgBuPycmxljcN27.w2j0ekTbeL(jr2vZqxigsruTjoD19))
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1));
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 63:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).mO5xoicFZZ());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 65:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 66:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).hVhxveNBwW());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 67:
            ConstructorInfo constructorInfo = (ConstructorInfo) typeof (Bmdacxg2KgloU9v9tT).Module.ResolveMethod((int) this.B1fXBBCnkj);
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            object[] objArray1 = new object[parameters.Length];
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[] jr2vZqxigsruTjoDArray = new Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[parameters.Length];
            List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2> vqX5vFsGw3JpGzjL2List = (List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2>) null;
            Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY n40wiAvcU3KnSgBj8Ry = (Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY) null;
            int num3 = 0;
            object obj10;
            while (true)
            {
              object obj11;
              object[] objArray2;
              int num4;
              if (num3 < parameters.Length)
              {
                Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD21 = this.fEbX7JSSW8.Wv25rccvJo();
                Type type3 = parameters[parameters.Length - 1 - num3].ParameterType;
                obj11 = (object) null;
                bool flag = false;
                if (type3.IsByRef && jr2vZqxigsruTjoD21 is Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo yeAcv8Vi2vmka2Koo)
                {
                  if (vqX5vFsGw3JpGzjL2List == null)
                    vqX5vFsGw3JpGzjL2List = new List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2>();
                  vqX5vFsGw3JpGzjL2List.Add(new Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2(yeAcv8Vi2vmka2Koo.Vle1MxXTxu, parameters.Length - 1 - num3));
                  obj11 = yeAcv8Vi2vmka2Koo.zjM1kFcOPO;
                  if (obj11 is Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD)
                    jr2vZqxigsruTjoD21 = obj11 as Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD;
                  else
                    flag = true;
                }
                if (!flag)
                {
                  if (jr2vZqxigsruTjoD21 != null)
                    obj11 = jr2vZqxigsruTjoD21.csZxJgmksq(type3);
                  if (obj11 == null)
                  {
                    if (type3.IsByRef)
                      type3 = type3.GetElementType();
                    if (type3.IsValueType)
                    {
                      obj11 = Activator.CreateInstance(type3);
                      if (jr2vZqxigsruTjoD21 is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
                        ((Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB) jr2vZqxigsruTjoD21).Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type3, obj11));
                    }
                  }
                }
                jr2vZqxigsruTjoDArray[objArray1.Length - 1 - num3] = jr2vZqxigsruTjoD21;
                objArray2 = objArray1;
                num4 = objArray1.Length - 1;
              }
              else
                goto label_93;
label_91:
              int num5 = num3;
              int index2 = num4 - num5;
              object obj12 = obj11;
              objArray2[index2] = obj12;
              ++num3;
              continue;
label_93:
              Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO q2MlvlVoFqkHslKto = (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO) null;
              if (vqX5vFsGw3JpGzjL2List != null)
              {
                n40wiAvcU3KnSgBj8Ry = new Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY((MethodBase) constructorInfo, vqX5vFsGw3JpGzjL2List);
                q2MlvlVoFqkHslKto = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.IpdXYZiq5U((MethodBase) constructorInfo, true, n40wiAvcU3KnSgBj8Ry);
              }
              obj1 = (object) null;
              obj10 = q2MlvlVoFqkHslKto == null ? constructorInfo.Invoke(objArray1) : q2MlvlVoFqkHslKto((object) null, objArray1);
              for (int index3 = 0; index3 < parameters.Length; ++index3)
              {
                if (parameters[index3].ParameterType.IsByRef)
                {
                  if (n40wiAvcU3KnSgBj8Ry == null || !n40wiAvcU3KnSgBj8Ry.KeR1HeJLTk(index3))
                  {
                    if (jr2vZqxigsruTjoDArray[index3].mwu5kK3wkv())
                      ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) jr2vZqxigsruTjoDArray[index3]).RgmkBBF8qv(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameters[index3].ParameterType, objArray1[index3]));
                    else if (jr2vZqxigsruTjoDArray[index3] is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
                      jr2vZqxigsruTjoDArray[index3].VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameters[index3].ParameterType.GetElementType(), objArray1[index3]));
                    else
                      jr2vZqxigsruTjoDArray[index3].VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameters[index3].ParameterType, objArray1[index3]));
                  }
                  else
                    goto label_91;
                }
              }
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(constructorInfo.DeclaringType, obj10));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 68:
            if (!this.fEbX7JSSW8.Wv25rccvJo().SXw0swpnLE(this.fEbX7JSSW8.Wv25rccvJo()))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 69:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN28 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.wFI5VtoADD());
            if (kp63vgBuPycmxljcN28 == null)
              throw new ArithmeticException(((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 0).ToString());
            if (!(kp63vgBuPycmxljcN28 is Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G neVv2bRmHypRuM1G))
              break;
            if (double.IsNaN(neVv2bRmHypRuM1G.UKy1i7YC4g))
              throw new OverflowException(((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 2).ToString());
            if (!double.IsInfinity(neVv2bRmHypRuM1G.UKy1i7YC4g))
              break;
            throw new OverflowException(((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 1).ToString());
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 70:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN29 = this.fEbX7JSSW8.Wv25rccvJo() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN;
            IntPtr num6 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.s4eXp9AKpI(this.fEbX7JSSW8.Wv25rccvJo());
            IntPtr num7 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.s4eXp9AKpI(this.fEbX7JSSW8.Wv25rccvJo());
            if (!(num6 != IntPtr.Zero) || !(num7 != IntPtr.Zero))
              break;
            uint eC7k3Jh9ai1 = kp63vgBuPycmxljcN29.REix24O3Rn().CJIkqRsjPJ.eC7k3JH9ai;
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.kTbXl5pg1j(num7, num6, eC7k3Jh9ai1);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 71:
            int b1fXbbCnkj3 = (int) this.B1fXBBCnkj;
            this.m2bXDcC4Eq[b1fXbbCnkj3] = this.v2v1xHiZvI(this.fEbX7JSSW8.Wv25rccvJo(), this.xevXHJGR3n.hb71qhcL9p[b1fXbbCnkj3].On712TVRMl, this.xevXHJGR3n.hb71qhcL9p[b1fXbbCnkj3].f8S1LJGfI1);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 72:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN30 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN31 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN30 == null || kp63vgBuPycmxljcN31 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN30.P0M0UAL1ro((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN31));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 73:
            if (Bmdacxg2KgloU9v9tT.W7vikF85fr.Count == 0)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mEOZvFveD19JmJrXYIC(typeof (Bmdacxg2KgloU9v9tT).Module.ResolveString((int) this.B1fXBBCnkj | 1879048192)));
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mEOZvFveD19JmJrXYIC(Bmdacxg2KgloU9v9tT.W7vikF85fr[(int) this.B1fXBBCnkj]));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 74:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN32 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (long), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN32.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 75:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD22 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN33 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD22);
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD23 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN34 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD23);
            if (kp63vgBuPycmxljcN34 == null || kp63vgBuPycmxljcN33 == null)
            {
              if (!jr2vZqxigsruTjoD22.FIt0KIs55w(jr2vZqxigsruTjoD23))
                break;
              this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
              break;
            }
            if (!kp63vgBuPycmxljcN34.w2j0ekTbeL(jr2vZqxigsruTjoD22))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 76:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).DX2xkrAct0());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 77:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN35 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (IntPtr), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN35.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 78:
            this.J2MXdZ5ps1 = (bool) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq(typeof (bool));
            this.iQmXxrTu9n = true;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 79:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).TL00pic9Va());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 80:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).YJ0x3w8q27());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 81:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN36 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (sbyte), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN36.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 82:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((double) this.B1fXBBCnkj));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 83:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).n27x71nDuy());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 84:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).pxZxmQMP9n());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 85:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD24 = this.fEbX7JSSW8.Wv25rccvJo();
            if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).AlG0IKVMxG(jr2vZqxigsruTjoD24))
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1));
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 86:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN37 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (ushort), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN37.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 87:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD25 = this.fEbX7JSSW8.Wv25rccvJo();
            this.fEbX7JSSW8.Wv25rccvJo().aIpx9u9Kmq(jr2vZqxigsruTjoD25);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 88:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN38 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN39 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN39 == null || kp63vgBuPycmxljcN38 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN39.y610MW78h1((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN38));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 89:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD26 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN40 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD26);
            if (jr2vZqxigsruTjoD26 != null && jr2vZqxigsruTjoD26.U4w0RYoANI() && kp63vgBuPycmxljcN40 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN40.YJ0x3w8q27());
              break;
            }
            if (kp63vgBuPycmxljcN40 == null || !kp63vgBuPycmxljcN40.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(*(int*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN40).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 91:
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 92:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD27 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD27.raO0WHR6j4())
              jr2vZqxigsruTjoD27 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) jr2vZqxigsruTjoD27).n27x71nDuy();
            this.fEbX7JSSW8.Wv25rccvJo().aIpx9u9Kmq(jr2vZqxigsruTjoD27);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 93:
            Type type4 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            if (!(this.fEbX7JSSW8.Wv25rccvJo() is Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB wmGvrtl0DafmkxCb1))
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            if (type4.IsValueType)
            {
              object instance = Activator.CreateInstance(type4);
              wmGvrtl0DafmkxCb1.Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type4, instance));
              break;
            }
            wmGvrtl0DafmkxCb1.Qhj0vDlKOA((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 94:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD28 = this.fEbX7JSSW8.Wv25rccvJo();
            if (!Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).RRp0cxhL37(jr2vZqxigsruTjoD28))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 95:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD29 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN41 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD29);
            if (jr2vZqxigsruTjoD29 != null && jr2vZqxigsruTjoD29.U4w0RYoANI() && kp63vgBuPycmxljcN41 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN41.VZTxtIrfkH());
              break;
            }
            IntPtr num8 = kp63vgBuPycmxljcN41 != null && kp63vgBuPycmxljcN41.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN41).m8XkfgZhhX() : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            if (IntPtr.Size == 8)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(*(long*) (void*) num8, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12));
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((long) *(int*) (void*) num8, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 12));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 96:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).U0KxNmsThj());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 97:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN42 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (byte), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN42.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 99:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD30 = this.fEbX7JSSW8.Wv25rccvJo();
            if (!Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).zof028Rdly(jr2vZqxigsruTjoD30))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 100:
            if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.tJgXN0fXs0(this.fEbX7JSSW8.Wv25rccvJo()).SXw0swpnLE(Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.tJgXN0fXs0(this.fEbX7JSSW8.Wv25rccvJo())))
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1));
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 101:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD31 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN43 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD31);
            if (jr2vZqxigsruTjoD31 != null && jr2vZqxigsruTjoD31.U4w0RYoANI() && kp63vgBuPycmxljcN43 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN43.TYoxcQ8ydQ());
              break;
            }
            if (kp63vgBuPycmxljcN43 == null || !kp63vgBuPycmxljcN43.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) *(sbyte*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN43).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 102:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).pgZxfwQuEP());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 103:
            Type type5 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            object obj13 = this.fEbX7JSSW8.Wv25rccvJo().BXg0EpmNON().csZxJgmksq(type5);
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type5, obj13));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 104:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN44 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN45 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN45 == null || kp63vgBuPycmxljcN44 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN45.TXD0ZXksHZ((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN44));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 105:
            Type type6 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            object obj14 = this.fEbX7JSSW8.Wv25rccvJo() is Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB wmGvrtl0DafmkxCb2 ? wmGvrtl0DafmkxCb2.csZxJgmksq(type6) : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD32;
            if (obj14 != null)
            {
              if (type6.IsValueType)
                obj14 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.VNEXFM1JWb(obj14);
              jr2vZqxigsruTjoD32 = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type6, obj14);
            }
            else if (type6.IsValueType)
            {
              object instance = Activator.CreateInstance(type6);
              jr2vZqxigsruTjoD32 = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type6, instance);
            }
            else
              jr2vZqxigsruTjoD32 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null);
            this.fEbX7JSSW8.x1C5GFNwKo(jr2vZqxigsruTjoD32);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 108:
            FieldInfo fieldInfo5 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveField((int) this.B1fXBBCnkj);
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(fieldInfo5.FieldType, fieldInfo5.GetValue((object) null)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 109:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ((int) this.B1fXBBCnkj, this));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 111:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).q8txPhiOdM());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 112:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).efa0lbWjvg());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 113:
            Type type7 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD33 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN46 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).SetValue(jr2vZqxigsruTjoD33.csZxJgmksq(type7), kp63vgBuPycmxljcN46.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 114:
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 142:
            Type type8 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            object obj15 = this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq(type8);
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD34;
            if (obj15 != null)
            {
              if (type8.IsValueType)
                obj15 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.VNEXFM1JWb(obj15);
              jr2vZqxigsruTjoD34 = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type8, obj15);
            }
            else if (type8.IsValueType)
            {
              object instance = Activator.CreateInstance(type8);
              jr2vZqxigsruTjoD34 = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type8, instance);
            }
            else
              jr2vZqxigsruTjoD34 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null);
            if (!(this.fEbX7JSSW8.Wv25rccvJo() is Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB wmGvrtl0DafmkxCb3))
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            wmGvrtl0DafmkxCb3.VPUxGtPN74(jr2vZqxigsruTjoD34);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 115:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD35 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN47 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD35);
            if (jr2vZqxigsruTjoD35 != null && jr2vZqxigsruTjoD35.U4w0RYoANI() && kp63vgBuPycmxljcN47 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN47.m18xyPS2QZ());
              break;
            }
            if (kp63vgBuPycmxljcN47 == null || !kp63vgBuPycmxljcN47.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(*(float*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN47).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 9));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 116:
            this.fEbX7JSSW8.x1C5GFNwKo(this.zeQXjGK0kf[(int) this.B1fXBBCnkj]);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 117:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD36 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN48 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD36);
            if (jr2vZqxigsruTjoD36 != null && jr2vZqxigsruTjoD36.U4w0RYoANI() && kp63vgBuPycmxljcN48 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN48.qCFxH1Sb7q());
              break;
            }
            if (kp63vgBuPycmxljcN48 == null || !kp63vgBuPycmxljcN48.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(*(uint*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN48).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 118:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN49 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN50 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN50 == null || kp63vgBuPycmxljcN49 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN50.Qi30aYIBJc((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN49));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 119:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN51 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN52 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN52 == null || kp63vgBuPycmxljcN51 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN52.Add((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN51));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 120:
            Type type9 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD37 = this.fEbX7JSSW8.Wv25rccvJo();
            object obj16 = jr2vZqxigsruTjoD37.csZxJgmksq((Type) null);
            if (obj16 == null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null));
              break;
            }
            if (!type9.IsAssignableFrom(obj16.GetType()))
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) null));
              break;
            }
            this.fEbX7JSSW8.x1C5GFNwKo(jr2vZqxigsruTjoD37);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 121:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN53 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN54 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN54 == null || kp63vgBuPycmxljcN53 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN54.vra0nNVSAN((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN53));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 122:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).hPFxRK7l3X());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 123:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).VhvxzNwEWd());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 124:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN55 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN56 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN56 == null || kp63vgBuPycmxljcN55 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN56.JcH0S7ZgWe((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN55));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 125:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((long) this.B1fXBBCnkj));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 127:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD38 = this.fEbX7JSSW8.Wv25rccvJo();
            if (!Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).Cfn0iX05uU(jr2vZqxigsruTjoD38))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 128:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).r4Jxu3ic6x());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 129:
            int b1fXbbCnkj4 = (int) this.B1fXBBCnkj;
            if (this.xevXHJGR3n.c1I1Y59VD3.IsStatic)
            {
              this.zeQXjGK0kf[b1fXbbCnkj4] = this.v2v1xHiZvI(this.fEbX7JSSW8.Wv25rccvJo(), this.xevXHJGR3n.A6N1NPB7k9[b1fXbbCnkj4].BQw1Cc3bH0);
              break;
            }
            this.zeQXjGK0kf[b1fXbbCnkj4] = this.v2v1xHiZvI(this.fEbX7JSSW8.Wv25rccvJo(), this.xevXHJGR3n.A6N1NPB7k9[b1fXbbCnkj4 - 1].BQw1Cc3bH0);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 130:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD39 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN57 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD39);
            if (jr2vZqxigsruTjoD39 != null && jr2vZqxigsruTjoD39.U4w0RYoANI() && kp63vgBuPycmxljcN57 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN57.n27x71nDuy());
              break;
            }
            if (kp63vgBuPycmxljcN57 == null || !kp63vgBuPycmxljcN57.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G(*(double*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN57).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 10));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 131:
            this.CAVXR58N0b = -3;
            if (this.fEbX7JSSW8.stX5SwJdbc() <= 0)
              break;
            this.BoDX6FHCap = this.fEbX7JSSW8.Wv25rccvJo();
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 132:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).VeAxgljEYN());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 134:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) Array.CreateInstance(typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj), Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 136:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).ghqxVF9bNj());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 137:
            Type type10 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN58 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            object obj17 = ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN58.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo);
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type10, obj17));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 138:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((float) this.B1fXBBCnkj));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 139:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).Gj7xdvyCKW());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 141:
            this.F6OX8o0nsL(false);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 143:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN59 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN60 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN60 == null || kp63vgBuPycmxljcN59 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN60.wjZ0Gc5iXg((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN59));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 144:
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            this.dw0XfTIcpf = true;
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 145:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN61 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN62 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN61 == null || kp63vgBuPycmxljcN62 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN61.FPe00NLJLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN62));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 146:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN63 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN64 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN64 == null || kp63vgBuPycmxljcN63 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN64.Dti0JwAExR((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN63));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 147:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).KT4xqQtVPi());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 148:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.bVBXbfhbsd(typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj)), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 149:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).Kut0CpUwqp());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 150:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD40 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD40.raO0WHR6j4())
              jr2vZqxigsruTjoD40 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) jr2vZqxigsruTjoD40).TYoxcQ8ydQ();
            this.fEbX7JSSW8.Wv25rccvJo().aIpx9u9Kmq(jr2vZqxigsruTjoD40);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 151:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN65 = this.fEbX7JSSW8.Wv25rccvJo() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN;
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN66 = this.fEbX7JSSW8.Wv25rccvJo() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN;
            IntPtr num9 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.s4eXp9AKpI(this.fEbX7JSSW8.Wv25rccvJo());
            if (!(num9 != IntPtr.Zero))
              break;
            byte dsakr1M1Zd = kp63vgBuPycmxljcN66.N9oxsvuhdW().CJIkqRsjPJ.DSakr1M1Zd;
            uint eC7k3Jh9ai2 = kp63vgBuPycmxljcN65.REix24O3Rn().CJIkqRsjPJ.eC7k3JH9ai;
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.rTZXcueo3m(num9, dsakr1M1Zd, (int) eC7k3Jh9ai2);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 153:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).Y61x8yfPbH());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 155:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).cacxITg5J8());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 156:
            lock (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.I6TXt25Fro)
            {
              Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD41 = this.fEbX7JSSW8.Wv25rccvJo();
              object key = this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null);
              Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.T5LXnnjUM8[key] = jr2vZqxigsruTjoD41;
              break;
            }
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 157:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo(typeof (Bmdacxg2KgloU9v9tT).Module.ResolveField((int) this.B1fXBBCnkj), (object) null));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 158:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD42 = this.fEbX7JSSW8.Wv25rccvJo();
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN67 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(jr2vZqxigsruTjoD42);
            if (jr2vZqxigsruTjoD42 != null && jr2vZqxigsruTjoD42.U4w0RYoANI() && kp63vgBuPycmxljcN67 != null)
            {
              this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN67.Y61x8yfPbH());
              break;
            }
            if (kp63vgBuPycmxljcN67 == null || !kp63vgBuPycmxljcN67.mwu5kK3wkv())
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(*(long*) (void*) ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) kp63vgBuPycmxljcN67).m8XkfgZhhX(), (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 159:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).qCFxH1Sb7q());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 160:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN68 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN69 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            if (kp63vgBuPycmxljcN69 == null || kp63vgBuPycmxljcN68 == null)
              throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
            this.fEbX7JSSW8.x1C5GFNwKo(kp63vgBuPycmxljcN69.OPa0LZv7Qy((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) kp63vgBuPycmxljcN68));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 161:
            typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN70 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            Array array3 = (Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null);
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.xsyWZYv984UQApwplkS(kp63vgBuPycmxljcN70.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo, array3));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 162:
            this.fEbX7JSSW8.Wv25rccvJo();
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 163:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) this.B1fXBBCnkj));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 164:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).jyyxw6yGFH());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 165:
            Type type11 = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveType((int) this.B1fXBBCnkj);
            object obj18 = this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq(type11) ?? Activator.CreateInstance(type11);
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab((object) Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type11, Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.VNEXFM1JWb(obj18))));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 167:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx(typeof (Bmdacxg2KgloU9v9tT).Module.ResolveMethod((int) this.B1fXBBCnkj).MethodHandle.GetFunctionPointer()));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 168:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD43 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD43.raO0WHR6j4())
              jr2vZqxigsruTjoD43 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) jr2vZqxigsruTjoD43).Y61x8yfPbH();
            this.fEbX7JSSW8.Wv25rccvJo().aIpx9u9Kmq(jr2vZqxigsruTjoD43);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 169:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN71 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (float), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN71.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 170:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD44 = this.fEbX7JSSW8.Wv25rccvJo();
            if (jr2vZqxigsruTjoD44.raO0WHR6j4())
              jr2vZqxigsruTjoD44 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) jr2vZqxigsruTjoD44).YJ0x3w8q27();
            this.fEbX7JSSW8.Wv25rccvJo().aIpx9u9Kmq(jr2vZqxigsruTjoD44);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 171:
            this.fEbX7JSSW8.x1C5GFNwKo(this.m2bXDcC4Eq[(int) this.B1fXBBCnkj]);
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 172:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).Cl6xWVWMfh());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 173:
            Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN72 = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo());
            this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(typeof (int), ((Array) this.fEbX7JSSW8.Wv25rccvJo().csZxJgmksq((Type) null)).GetValue(kp63vgBuPycmxljcN72.R54x68lelm().CJIkqRsjPJ.I8ZkohiOvo)));
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 174:
            this.fEbX7JSSW8.x1C5GFNwKo((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()) ?? throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW()).zG9xDa5XWa());
            break;
          case (Bmdacxg2KgloU9v9tT.lWPfrBv6CkxICGYpnH0) 175:
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD45 = this.fEbX7JSSW8.Wv25rccvJo();
            if (!Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.JD2X2IYd95(this.fEbX7JSSW8.Wv25rccvJo()).Cnj06t1SA6(jr2vZqxigsruTjoD45))
              break;
            this.CAVXR58N0b = (int) this.B1fXBBCnkj - 1;
            break;
        }
      }

      private Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD v2v1xHiZvI(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1,
        Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv _param2,
        bool _param3 = false)
      {
        if (!_param3 && _param1.U4w0RYoANI())
          _param1 = _param1.BXg0EpmNON();
        if (_param1.oBL5MGf29A())
          return ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).aHlx1rUpXO(_param2);
        if (_param1.lIM51cVqgP())
          return ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).aHlx1rUpXO(_param2);
        if (_param1.jHU5KDfZOE())
          return ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).aHlx1rUpXO(_param2);
        return _param1.mwu5kK3wkv() ? ((Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN) _param1).aHlx1rUpXO(_param2) : _param1;
      }

      private Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD RUtIEk13kx(int _param1)
      {
        return this.m2bXDcC4Eq[_param1];
      }

      private void wj7IMKZH7n(int _param1)
      {
        this.CE4XgToOQV(_param1, this.fEbX7JSSW8.Wv25rccvJo());
      }

      private static int bVBXbfhbsd(Type _param0)
      {
        lock (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.UxMXm7pNtX)
        {
          if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.AIIXARK6iP == null)
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.AIIXARK6iP = new Dictionary<Type, int>();
          try
          {
            int num1 = 0;
            if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.AIIXARK6iP.TryGetValue(_param0, out num1))
              return num1;
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (int), Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Sizeof, _param0);
            ilGenerator.Emit(OpCodes.Ret);
            int num2 = (int) dynamicMethod.Invoke((object) null, (object[]) null);
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.AIIXARK6iP[_param0] = num2;
            return num2;
          }
          catch
          {
            return 0;
          }
        }
      }

      private void CE4XgToOQV(int _param1, Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param2)
      {
        this.m2bXDcC4Eq[_param1] = this.v2v1xHiZvI(_param2, this.xevXHJGR3n.hb71qhcL9p[_param1].On712TVRMl, this.xevXHJGR3n.hb71qhcL9p[_param1].f8S1LJGfI1);
      }

      private static Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN JD2X2IYd95(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param0)
      {
        if (!(_param0 is Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN) && _param0.U4w0RYoANI())
          kp63vgBuPycmxljcN = _param0.BXg0EpmNON() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN;
        return kp63vgBuPycmxljcN;
      }

      private void F6OX8o0nsL(bool _param1)
      {
        MethodBase methodBase = typeof (Bmdacxg2KgloU9v9tT).Module.ResolveMethod((int) this.B1fXBBCnkj);
        MethodInfo methodInfo = methodBase as MethodInfo;
        ParameterInfo[] parameters = methodBase.GetParameters();
        object[] objArray = new object[parameters.Length];
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[] jr2vZqxigsruTjoDArray = new Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[parameters.Length];
        List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2> vqX5vFsGw3JpGzjL2List = (List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2>) null;
        Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY n40wiAvcU3KnSgBj8Ry = (Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY) null;
        for (int index = 0; index < parameters.Length; ++index)
        {
          Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = this.fEbX7JSSW8.Wv25rccvJo();
          Type type = parameters[parameters.Length - 1 - index].ParameterType;
          object obj = (object) null;
          bool flag = false;
          if (type.IsByRef && jr2vZqxigsruTjoD is Bmdacxg2KgloU9v9tT.oiYEAcv8Vi2vmka2Koo yeAcv8Vi2vmka2Koo)
          {
            if (vqX5vFsGw3JpGzjL2List == null)
              vqX5vFsGw3JpGzjL2List = new List<Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2>();
            vqX5vFsGw3JpGzjL2List.Add(new Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2(yeAcv8Vi2vmka2Koo.Vle1MxXTxu, parameters.Length - 1 - index));
            obj = yeAcv8Vi2vmka2Koo.zjM1kFcOPO;
            if (obj is Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD)
            {
              jr2vZqxigsruTjoD = obj as Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD;
            }
            else
            {
              flag = true;
              if (obj == null)
              {
                if (type.IsByRef)
                  type = type.GetElementType();
                if (type.IsValueType)
                {
                  obj = !yeAcv8Vi2vmka2Koo.Vle1MxXTxu.IsStatic ? Activator.CreateInstance(type) : yeAcv8Vi2vmka2Koo.Vle1MxXTxu.GetValue((object) null);
                  if (jr2vZqxigsruTjoD is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
                    ((Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB) jr2vZqxigsruTjoD).Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type, obj));
                }
              }
            }
          }
          if (!flag)
          {
            if (jr2vZqxigsruTjoD != null)
              obj = jr2vZqxigsruTjoD.csZxJgmksq(type);
            if (obj == null)
            {
              if (type.IsByRef)
                type = type.GetElementType();
              if (type.IsValueType)
              {
                obj = Activator.CreateInstance(type);
                if (jr2vZqxigsruTjoD is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
                  ((Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB) jr2vZqxigsruTjoD).Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type, obj));
              }
            }
          }
          jr2vZqxigsruTjoDArray[objArray.Length - 1 - index] = jr2vZqxigsruTjoD;
          objArray[objArray.Length - 1 - index] = obj;
        }
        Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO q2MlvlVoFqkHslKto = (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO) null;
        if (vqX5vFsGw3JpGzjL2List != null)
        {
          n40wiAvcU3KnSgBj8Ry = new Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY(methodBase, vqX5vFsGw3JpGzjL2List);
          q2MlvlVoFqkHslKto = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.C76XoUNrMu(methodBase, _param1, n40wiAvcU3KnSgBj8Ry);
        }
        else if (methodInfo != (MethodInfo) null && methodInfo.ReturnType.IsByRef)
          q2MlvlVoFqkHslKto = Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.qjkX3Yl9Pq(methodBase, _param1);
        object target = (object) null;
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD1 = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) null;
        if (!methodBase.IsStatic)
        {
          jr2vZqxigsruTjoD1 = this.fEbX7JSSW8.Wv25rccvJo();
          if (jr2vZqxigsruTjoD1 != null)
            target = jr2vZqxigsruTjoD1.csZxJgmksq(methodBase.DeclaringType);
          if (target == null)
          {
            Type type = methodBase.DeclaringType;
            if (type.IsByRef)
              type = type.GetElementType();
            target = type.IsValueType ? Activator.CreateInstance(type) : throw new NullReferenceException();
            if (target == null && Nullable.GetUnderlyingType(type) != (Type) null)
              target = FormatterServices.GetUninitializedObject(type);
            if (jr2vZqxigsruTjoD1 is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
              ((Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB) jr2vZqxigsruTjoD1).Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(type, target));
          }
        }
        object obj1;
        if ((object) (methodBase as ConstructorInfo) != null && Nullable.GetUnderlyingType(methodBase.DeclaringType) != (Type) null)
        {
          obj1 = objArray[0];
          if (jr2vZqxigsruTjoD1 != null && jr2vZqxigsruTjoD1 is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
            ((Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB) jr2vZqxigsruTjoD1).Qhj0vDlKOA(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(Nullable.GetUnderlyingType(methodBase.DeclaringType), obj1));
        }
        else
          obj1 = q2MlvlVoFqkHslKto == null ? methodBase.Invoke(target, objArray) : q2MlvlVoFqkHslKto(target, objArray);
        for (int index = 0; index < parameters.Length; ++index)
        {
          if (parameters[index].ParameterType.IsByRef && (n40wiAvcU3KnSgBj8Ry == null || !n40wiAvcU3KnSgBj8Ry.KeR1HeJLTk(index)))
          {
            if (jr2vZqxigsruTjoDArray[index].mwu5kK3wkv())
              ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) jr2vZqxigsruTjoDArray[index]).RgmkBBF8qv(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameters[index].ParameterType, objArray[index]));
            else if (jr2vZqxigsruTjoDArray[index] is Bmdacxg2KgloU9v9tT.zK4tiEvSAiB3StdGBuZ)
              jr2vZqxigsruTjoDArray[index].VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameters[index].ParameterType.GetElementType(), objArray[index]));
            else
              jr2vZqxigsruTjoDArray[index].VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(parameters[index].ParameterType, objArray[index]));
          }
        }
        if (!(methodInfo != (MethodInfo) null) || !(methodInfo.ReturnType != typeof (void)))
          return;
        this.fEbX7JSSW8.x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(methodInfo.ReturnType, obj1));
      }

      private static Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO qjkX3Yl9Pq(
        MethodBase _param0,
        bool _param1)
      {
        lock (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.xs25vnvuVR)
        {
          Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO q2MlvlVoFqkHslKto1 = (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO) null;
          if (_param1)
          {
            if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.hNyXze0LxK.TryGetValue(_param0, out q2MlvlVoFqkHslKto1))
              return q2MlvlVoFqkHslKto1;
          }
          else if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.APB5uME0jL.TryGetValue(_param0, out q2MlvlVoFqkHslKto1))
            return q2MlvlVoFqkHslKto1;
          MethodInfo methodInfo = _param0 as MethodInfo;
          DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), new Type[2]
          {
            typeof (object),
            typeof (object[])
          }, true);
          ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
          ParameterInfo[] parameters = _param0.GetParameters();
          Type[] typeArray = new Type[parameters.Length];
          for (int index = 0; index < typeArray.Length; ++index)
            typeArray[index] = !parameters[index].ParameterType.IsByRef ? parameters[index].ParameterType : parameters[index].ParameterType.GetElementType();
          int length = typeArray.Length;
          if (_param0.DeclaringType.IsValueType)
            ++length;
          LocalBuilder[] localBuilderArray = new LocalBuilder[length];
          for (int index = 0; index < typeArray.Length; ++index)
            localBuilderArray[index] = ilGenerator.DeclareLocal(typeArray[index]);
          if (_param0.DeclaringType.IsValueType)
            localBuilderArray[localBuilderArray.Length - 1] = ilGenerator.DeclareLocal(_param0.DeclaringType.MakeByRefType());
          for (int index = 0; index < typeArray.Length; ++index)
          {
            ilGenerator.Emit(OpCodes.Ldarg_1);
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
            ilGenerator.Emit(OpCodes.Ldelem_Ref);
            if (typeArray[index].IsValueType)
              ilGenerator.Emit(OpCodes.Unbox_Any, typeArray[index]);
            else if (typeArray[index] != typeof (object))
              ilGenerator.Emit(OpCodes.Castclass, typeArray[index]);
            ilGenerator.Emit(OpCodes.Stloc, localBuilderArray[index]);
          }
          if (!_param0.IsStatic)
          {
            ilGenerator.Emit(OpCodes.Ldarg_0);
            if (_param0.DeclaringType.IsValueType)
            {
              ilGenerator.Emit(OpCodes.Unbox, _param0.DeclaringType);
              ilGenerator.Emit(OpCodes.Stloc, localBuilderArray[localBuilderArray.Length - 1]);
              ilGenerator.Emit(OpCodes.Ldloc_S, localBuilderArray[localBuilderArray.Length - 1]);
            }
            else
              ilGenerator.Emit(OpCodes.Castclass, _param0.DeclaringType);
          }
          for (int index = 0; index < typeArray.Length; ++index)
          {
            if (parameters[index].ParameterType.IsByRef)
              ilGenerator.Emit(OpCodes.Ldloca_S, localBuilderArray[index]);
            else
              ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
          }
          if (_param1)
          {
            if (methodInfo != (MethodInfo) null)
              ilGenerator.EmitCall(OpCodes.Call, methodInfo, (Type[]) null);
            else
              ilGenerator.Emit(OpCodes.Call, _param0 as ConstructorInfo);
          }
          else if (methodInfo != (MethodInfo) null)
            ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, (Type[]) null);
          else
            ilGenerator.Emit(OpCodes.Callvirt, _param0 as ConstructorInfo);
          if (methodInfo == (MethodInfo) null || methodInfo.ReturnType == typeof (void))
            ilGenerator.Emit(OpCodes.Ldnull);
          else if (methodInfo.ReturnType.IsByRef)
          {
            Type elementType = methodInfo.ReturnType.GetElementType();
            if (elementType.IsValueType)
              ilGenerator.Emit(OpCodes.Ldobj, elementType);
            else
              ilGenerator.Emit(OpCodes.Ldind_Ref, elementType);
            if (elementType.IsValueType)
              ilGenerator.Emit(OpCodes.Box, elementType);
          }
          else if (methodInfo.ReturnType.IsValueType)
            ilGenerator.Emit(OpCodes.Box, methodInfo.ReturnType);
          for (int index = 0; index < typeArray.Length; ++index)
          {
            if (parameters[index].ParameterType.IsByRef)
            {
              ilGenerator.Emit(OpCodes.Ldarg_1);
              Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
              ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
              if (localBuilderArray[index].LocalType.IsValueType)
                ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
              ilGenerator.Emit(OpCodes.Stelem_Ref);
            }
          }
          ilGenerator.Emit(OpCodes.Ret);
          Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO q2MlvlVoFqkHslKto2 = (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO) dynamicMethod.CreateDelegate(typeof (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO));
          if (_param1)
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.hNyXze0LxK.Add(_param0, q2MlvlVoFqkHslKto2);
          else
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.APB5uME0jL.Add(_param0, q2MlvlVoFqkHslKto2);
          return q2MlvlVoFqkHslKto2;
        }
      }

      private static Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO C76XoUNrMu(
        MethodBase _param0,
        bool _param1,
        Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY _param2)
      {
        lock (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.Ldb5ixhB4X)
        {
          Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO q2MlvlVoFqkHslKto1 = (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO) null;
          if (_param1)
          {
            if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.uBo5a3c77c.TryGetValue(_param2, out q2MlvlVoFqkHslKto1))
              return q2MlvlVoFqkHslKto1;
          }
          else if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.rhC5JomQI4.TryGetValue(_param2, out q2MlvlVoFqkHslKto1))
            return q2MlvlVoFqkHslKto1;
          MethodInfo methodInfo = _param0 as MethodInfo;
          DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), new Type[2]
          {
            typeof (object),
            typeof (object[])
          }, typeof (Bmdacxg2KgloU9v9tT), true);
          ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
          ParameterInfo[] parameters = _param0.GetParameters();
          Type[] typeArray = new Type[parameters.Length];
          for (int index = 0; index < typeArray.Length; ++index)
            typeArray[index] = !parameters[index].ParameterType.IsByRef ? parameters[index].ParameterType : parameters[index].ParameterType.GetElementType();
          int length = typeArray.Length;
          if (_param0.DeclaringType.IsValueType)
            ++length;
          LocalBuilder[] localBuilderArray = new LocalBuilder[length];
          for (int index = 0; index < typeArray.Length; ++index)
            localBuilderArray[index] = !_param2.KeR1HeJLTk(index) ? ilGenerator.DeclareLocal(typeArray[index]) : ilGenerator.DeclareLocal(typeof (object));
          if (_param0.DeclaringType.IsValueType)
            localBuilderArray[localBuilderArray.Length - 1] = ilGenerator.DeclareLocal(_param0.DeclaringType.MakeByRefType());
          for (int index = 0; index < typeArray.Length; ++index)
          {
            ilGenerator.Emit(OpCodes.Ldarg_1);
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
            ilGenerator.Emit(OpCodes.Ldelem_Ref);
            if (!_param2.KeR1HeJLTk(index))
            {
              if (typeArray[index].IsValueType)
                ilGenerator.Emit(OpCodes.Unbox_Any, typeArray[index]);
              else if (typeArray[index] != typeof (object))
                ilGenerator.Emit(OpCodes.Castclass, typeArray[index]);
            }
            ilGenerator.Emit(OpCodes.Stloc, localBuilderArray[index]);
          }
          if (!_param0.IsStatic)
          {
            ilGenerator.Emit(OpCodes.Ldarg_0);
            if (_param0.DeclaringType.IsValueType)
            {
              ilGenerator.Emit(OpCodes.Unbox, _param0.DeclaringType);
              ilGenerator.Emit(OpCodes.Stloc, localBuilderArray[localBuilderArray.Length - 1]);
              ilGenerator.Emit(OpCodes.Ldloc_S, localBuilderArray[localBuilderArray.Length - 1]);
            }
            else
              ilGenerator.Emit(OpCodes.Castclass, _param0.DeclaringType);
          }
          for (int index = 0; index < typeArray.Length; ++index)
          {
            if (!_param2.KeR1HeJLTk(index))
            {
              if (parameters[index].ParameterType.IsByRef)
                ilGenerator.Emit(OpCodes.Ldloca_S, localBuilderArray[index]);
              else
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
            }
            else
            {
              Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2 vqX5vFsGw3JpGzjL2 = _param2.V4W1lwKj5l(index);
              if (vqX5vFsGw3JpGzjL2.wwd1FAYGTS.IsStatic)
                ilGenerator.Emit(OpCodes.Ldsflda, vqX5vFsGw3JpGzjL2.wwd1FAYGTS);
              else if (vqX5vFsGw3JpGzjL2.wwd1FAYGTS.DeclaringType.IsValueType)
              {
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                ilGenerator.Emit(OpCodes.Unbox, vqX5vFsGw3JpGzjL2.wwd1FAYGTS.DeclaringType);
                ilGenerator.Emit(OpCodes.Ldflda, vqX5vFsGw3JpGzjL2.wwd1FAYGTS);
              }
              else
              {
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                ilGenerator.Emit(OpCodes.Castclass, vqX5vFsGw3JpGzjL2.wwd1FAYGTS.DeclaringType);
                ilGenerator.Emit(OpCodes.Ldflda, vqX5vFsGw3JpGzjL2.wwd1FAYGTS);
              }
            }
          }
          if (_param1)
          {
            if (methodInfo != (MethodInfo) null)
              ilGenerator.EmitCall(OpCodes.Call, methodInfo, (Type[]) null);
            else
              ilGenerator.Emit(OpCodes.Call, _param0 as ConstructorInfo);
          }
          else if (methodInfo != (MethodInfo) null)
            ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, (Type[]) null);
          else
            ilGenerator.Emit(OpCodes.Callvirt, _param0 as ConstructorInfo);
          if (methodInfo == (MethodInfo) null || methodInfo.ReturnType == typeof (void))
            ilGenerator.Emit(OpCodes.Ldnull);
          else if (methodInfo.ReturnType.IsByRef)
          {
            Type elementType = methodInfo.ReturnType.GetElementType();
            if (elementType.IsValueType)
              ilGenerator.Emit(OpCodes.Ldobj, elementType);
            else
              ilGenerator.Emit(OpCodes.Ldind_Ref, elementType);
            if (elementType.IsValueType)
              ilGenerator.Emit(OpCodes.Box, elementType);
          }
          else if (methodInfo.ReturnType.IsValueType)
            ilGenerator.Emit(OpCodes.Box, methodInfo.ReturnType);
          for (int index = 0; index < typeArray.Length; ++index)
          {
            if (parameters[index].ParameterType.IsByRef)
            {
              if (!_param2.KeR1HeJLTk(index))
              {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                if (localBuilderArray[index].LocalType.IsValueType)
                  ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
                ilGenerator.Emit(OpCodes.Stelem_Ref);
              }
              else
              {
                Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2 vqX5vFsGw3JpGzjL2 = _param2.V4W1lwKj5l(index);
                if (vqX5vFsGw3JpGzjL2.wwd1FAYGTS.IsStatic)
                {
                  ilGenerator.Emit(OpCodes.Ldarg_1);
                  Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
                  ilGenerator.Emit(OpCodes.Ldsfld, vqX5vFsGw3JpGzjL2.wwd1FAYGTS);
                  if (vqX5vFsGw3JpGzjL2.wwd1FAYGTS.FieldType.IsValueType)
                    ilGenerator.Emit(OpCodes.Box, vqX5vFsGw3JpGzjL2.wwd1FAYGTS.FieldType);
                  ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
                else
                {
                  ilGenerator.Emit(OpCodes.Ldarg_1);
                  Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
                  ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                  if (localBuilderArray[index].LocalType.IsValueType)
                    ilGenerator.Emit(OpCodes.Box, vqX5vFsGw3JpGzjL2.wwd1FAYGTS.FieldType);
                  ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
              }
            }
          }
          ilGenerator.Emit(OpCodes.Ret);
          Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO q2MlvlVoFqkHslKto2 = (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO) dynamicMethod.CreateDelegate(typeof (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO));
          if (_param1)
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.uBo5a3c77c.Add(_param2, q2MlvlVoFqkHslKto2);
          else
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.rhC5JomQI4.Add(_param2, q2MlvlVoFqkHslKto2);
          return q2MlvlVoFqkHslKto2;
        }
      }

      private static Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO IpdXYZiq5U(
        MethodBase _param0,
        bool _param1,
        Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY _param2)
      {
        lock (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.x1O5TkrH6u)
        {
          Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO q2MlvlVoFqkHslKto1 = (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO) null;
          if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.l8U5QBixHo.TryGetValue(_param2, out q2MlvlVoFqkHslKto1))
            return q2MlvlVoFqkHslKto1;
          ConstructorInfo constructorInfo = _param0 as ConstructorInfo;
          DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), new Type[2]
          {
            typeof (object),
            typeof (object[])
          }, typeof (Bmdacxg2KgloU9v9tT), true);
          ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
          ParameterInfo[] parameters = _param0.GetParameters();
          Type[] typeArray = new Type[parameters.Length];
          for (int index = 0; index < typeArray.Length; ++index)
            typeArray[index] = !parameters[index].ParameterType.IsByRef ? parameters[index].ParameterType : parameters[index].ParameterType.GetElementType();
          int length = typeArray.Length;
          if (_param0.DeclaringType.IsValueType)
            ++length;
          LocalBuilder[] localBuilderArray = new LocalBuilder[length];
          for (int index = 0; index < typeArray.Length; ++index)
            localBuilderArray[index] = !_param2.KeR1HeJLTk(index) ? ilGenerator.DeclareLocal(typeArray[index]) : ilGenerator.DeclareLocal(typeof (object));
          if (_param0.DeclaringType.IsValueType)
            localBuilderArray[localBuilderArray.Length - 1] = ilGenerator.DeclareLocal(_param0.DeclaringType.MakeByRefType());
          for (int index = 0; index < typeArray.Length; ++index)
          {
            ilGenerator.Emit(OpCodes.Ldarg_1);
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
            ilGenerator.Emit(OpCodes.Ldelem_Ref);
            if (!_param2.KeR1HeJLTk(index))
            {
              if (typeArray[index].IsValueType)
                ilGenerator.Emit(OpCodes.Unbox_Any, typeArray[index]);
              else if (typeArray[index] != typeof (object))
                ilGenerator.Emit(OpCodes.Castclass, typeArray[index]);
            }
            ilGenerator.Emit(OpCodes.Stloc, localBuilderArray[index]);
          }
          for (int index = 0; index < typeArray.Length; ++index)
          {
            if (!_param2.KeR1HeJLTk(index))
            {
              if (parameters[index].ParameterType.IsByRef)
                ilGenerator.Emit(OpCodes.Ldloca_S, localBuilderArray[index]);
              else
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
            }
            else
            {
              Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2 vqX5vFsGw3JpGzjL2 = _param2.V4W1lwKj5l(index);
              if (vqX5vFsGw3JpGzjL2.wwd1FAYGTS.IsStatic)
                ilGenerator.Emit(OpCodes.Ldsflda, vqX5vFsGw3JpGzjL2.wwd1FAYGTS);
              else if (vqX5vFsGw3JpGzjL2.wwd1FAYGTS.DeclaringType.IsValueType)
              {
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                ilGenerator.Emit(OpCodes.Unbox, vqX5vFsGw3JpGzjL2.wwd1FAYGTS.DeclaringType);
                ilGenerator.Emit(OpCodes.Ldflda, vqX5vFsGw3JpGzjL2.wwd1FAYGTS);
              }
              else
              {
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                ilGenerator.Emit(OpCodes.Castclass, vqX5vFsGw3JpGzjL2.wwd1FAYGTS.DeclaringType);
                ilGenerator.Emit(OpCodes.Ldflda, vqX5vFsGw3JpGzjL2.wwd1FAYGTS);
              }
            }
          }
          ilGenerator.Emit(OpCodes.Newobj, _param0 as ConstructorInfo);
          if (constructorInfo.DeclaringType.IsValueType)
            ilGenerator.Emit(OpCodes.Box, constructorInfo.DeclaringType);
          for (int index = 0; index < typeArray.Length; ++index)
          {
            if (parameters[index].ParameterType.IsByRef)
            {
              if (!_param2.KeR1HeJLTk(index))
              {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                if (localBuilderArray[index].LocalType.IsValueType)
                  ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
                ilGenerator.Emit(OpCodes.Stelem_Ref);
              }
              else
              {
                Bmdacxg2KgloU9v9tT.LLVqX5vFSGw3JpGZJL2 vqX5vFsGw3JpGzjL2 = _param2.V4W1lwKj5l(index);
                if (vqX5vFsGw3JpGzjL2.wwd1FAYGTS.IsStatic)
                {
                  ilGenerator.Emit(OpCodes.Ldarg_1);
                  Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
                  ilGenerator.Emit(OpCodes.Ldsfld, vqX5vFsGw3JpGzjL2.wwd1FAYGTS);
                  if (vqX5vFsGw3JpGzjL2.wwd1FAYGTS.FieldType.IsValueType)
                    ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
                  ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
                else
                {
                  ilGenerator.Emit(OpCodes.Ldarg_1);
                  Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.FewXhiE9tJ(ilGenerator, index);
                  ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                  if (localBuilderArray[index].LocalType.IsValueType)
                    ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
                  ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
              }
            }
          }
          ilGenerator.Emit(OpCodes.Ret);
          Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO q2MlvlVoFqkHslKto2 = (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO) dynamicMethod.CreateDelegate(typeof (Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO));
          Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.l8U5QBixHo.Add(_param2, q2MlvlVoFqkHslKto2);
          return q2MlvlVoFqkHslKto2;
        }
      }

      private static void FewXhiE9tJ(ILGenerator _param0, int _param1)
      {
        switch (_param1)
        {
          case -1:
            _param0.Emit(OpCodes.Ldc_I4_M1);
            break;
          case 0:
            _param0.Emit(OpCodes.Ldc_I4_0);
            break;
          case 1:
            _param0.Emit(OpCodes.Ldc_I4_1);
            break;
          case 2:
            _param0.Emit(OpCodes.Ldc_I4_2);
            break;
          case 3:
            _param0.Emit(OpCodes.Ldc_I4_3);
            break;
          case 4:
            _param0.Emit(OpCodes.Ldc_I4_4);
            break;
          case 5:
            _param0.Emit(OpCodes.Ldc_I4_5);
            break;
          case 6:
            _param0.Emit(OpCodes.Ldc_I4_6);
            break;
          case 7:
            _param0.Emit(OpCodes.Ldc_I4_7);
            break;
          case 8:
            _param0.Emit(OpCodes.Ldc_I4_8);
            break;
          default:
            if (_param1 > -129 && _param1 < 128)
            {
              _param0.Emit(OpCodes.Ldc_I4_S, (sbyte) _param1);
              break;
            }
            _param0.Emit(OpCodes.Ldc_I4, _param1);
            break;
        }
      }

      private static Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD tJgXN0fXs0(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param0)
      {
        if (_param0.BXg0EpmNON().riO5EmiZ8q())
        {
          object obj1 = _param0.csZxJgmksq((Type) null);
          if (obj1 != null && obj1.GetType().IsEnum)
          {
            Type underlyingType = Enum.GetUnderlyingType(obj1.GetType());
            object obj2 = Convert.ChangeType(obj1, underlyingType);
            Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.WRCXqyyfiu(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(underlyingType, obj2));
            if (jr2vZqxigsruTjoD != null)
              return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (jr2vZqxigsruTjoD as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN);
          }
        }
        return _param0;
      }

      private static Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN WRCXqyyfiu(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param0)
      {
        if (!(_param0 is Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN) && _param0.U4w0RYoANI())
          kp63vgBuPycmxljcN = _param0.BXg0EpmNON() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN;
        return kp63vgBuPycmxljcN;
      }

      private static IntPtr s4eXp9AKpI(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param0)
      {
        if (_param0 == null)
          return IntPtr.Zero;
        if (_param0.mwu5kK3wkv())
          return ((Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx) _param0).m8XkfgZhhX();
        if (_param0.U4w0RYoANI())
        {
          Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB wmGvrtl0DafmkxCb = (Bmdacxg2KgloU9v9tT.jYOWmGvrtl0DafmkxCB) _param0;
          try
          {
            return wmGvrtl0DafmkxCb.eKG0Hbm1QH();
          }
          catch
          {
          }
        }
        object obj = _param0.csZxJgmksq(typeof (IntPtr));
        return obj != null && obj.GetType() == typeof (IntPtr) ? (IntPtr) obj : throw new Bmdacxg2KgloU9v9tT.Vas8K4vGXT59byIK3fW();
      }

      private static object VNEXFM1JWb(object _param0)
      {
        lock (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.NTf5bU5PVY)
        {
          if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.tUx5shZpvN == null)
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.tUx5shZpvN = new Dictionary<Type, Bmdacxg2KgloU9v9tT.PachvPvHCyoyZLygZ3G>();
          if (_param0 == null)
            return (object) null;
          try
          {
            Type type = _param0.GetType();
            Bmdacxg2KgloU9v9tT.PachvPvHCyoyZLygZ3G pachvPvHcyoyZlygZ3G1;
            if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.tUx5shZpvN.TryGetValue(type, out pachvPvHcyoyZlygZ3G1))
              return pachvPvHcyoyZlygZ3G1(_param0);
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), new Type[1]
            {
              typeof (object)
            }, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Unbox_Any, type);
            ilGenerator.Emit(OpCodes.Box, type);
            ilGenerator.Emit(OpCodes.Ret);
            Bmdacxg2KgloU9v9tT.PachvPvHCyoyZLygZ3G pachvPvHcyoyZlygZ3G2 = (Bmdacxg2KgloU9v9tT.PachvPvHCyoyZLygZ3G) dynamicMethod.CreateDelegate(typeof (Bmdacxg2KgloU9v9tT.PachvPvHCyoyZLygZ3G));
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.tUx5shZpvN.Add(type, pachvPvHcyoyZlygZ3G2);
            return pachvPvHcyoyZlygZ3G2(_param0);
          }
          catch
          {
            return (object) null;
          }
        }
      }

      private static void rTZXcueo3m(IntPtr _param0, byte _param1, int _param2)
      {
        lock (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.NTf5bU5PVY)
        {
          if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.jSL5P12kQw == null)
          {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (void), new Type[3]
            {
              typeof (IntPtr),
              typeof (byte),
              typeof (int)
            }, typeof (Bmdacxg2KgloU9v9tT), true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Initblk);
            ilGenerator.Emit(OpCodes.Ret);
            Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.jSL5P12kQw = (Bmdacxg2KgloU9v9tT.qd6j41vjSCTQZfupK3g) dynamicMethod.CreateDelegate(typeof (Bmdacxg2KgloU9v9tT.qd6j41vjSCTQZfupK3g));
          }
          Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.jSL5P12kQw(_param0, _param1, _param2);
        }
      }

      private static void kTbXl5pg1j(IntPtr _param0, IntPtr _param1, uint _param2)
      {
        if (Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.eCT5WLagGQ == null)
        {
          DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (void), new Type[3]
          {
            typeof (IntPtr),
            typeof (IntPtr),
            typeof (uint)
          }, typeof (Bmdacxg2KgloU9v9tT), true);
          ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
          ilGenerator.Emit(OpCodes.Ldarg_0);
          ilGenerator.Emit(OpCodes.Ldarg_1);
          ilGenerator.Emit(OpCodes.Ldarg_2);
          ilGenerator.Emit(OpCodes.Cpblk);
          ilGenerator.Emit(OpCodes.Ret);
          Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.eCT5WLagGQ = (Bmdacxg2KgloU9v9tT.xeSy0nvDBi7Um10QQWP) dynamicMethod.CreateDelegate(typeof (Bmdacxg2KgloU9v9tT.xeSy0nvDBi7Um10QQWP));
        }
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.eCT5WLagGQ(_param0, _param1, _param2);
      }

      public gR72hDv7XQltkgXgRlp()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.zeQXjGK0kf = new Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[0];
        this.m2bXDcC4Eq = new Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD[0];
        this.fEbX7JSSW8 = new Bmdacxg2KgloU9v9tT.v2STNJvydw751BYOUX4();
        this.utjXyWaDwK = -1;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static gR72hDv7XQltkgXgRlp()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.UxMXm7pNtX = new object();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.T5LXnnjUM8 = new Dictionary<object, Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD>();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.I6TXt25Fro = new object();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.hNyXze0LxK = new Dictionary<MethodBase, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO>();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.APB5uME0jL = new Dictionary<MethodBase, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO>();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.xs25vnvuVR = new object();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.uBo5a3c77c = new Dictionary<Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO>();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.rhC5JomQI4 = new Dictionary<Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO>();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.Ldb5ixhB4X = new object();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.l8U5QBixHo = new Dictionary<Bmdacxg2KgloU9v9tT.N40wiAvcU3KnSgBj8RY, Bmdacxg2KgloU9v9tT.H5Q2MlvlVOFqkHslKTO>();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.x1O5TkrH6u = new object();
        Bmdacxg2KgloU9v9tT.gR72hDv7XQltkgXgRlp.NTf5bU5PVY = new object();
      }
    }

    internal enum lWPfrBv6CkxICGYpnH0 : byte
    {
    }

    internal enum y8tPEovwuEAmGj2wcSe : byte
    {
    }

    internal abstract class iJPJr2vZQxigsruTJoD
    {
      internal Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe tdv5g7bctk;

      public iJPJr2vZQxigsruTJoD()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      internal bool riO5EmiZ8q() => this.tdv5g7bctk == (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 0;

      internal bool oBL5MGf29A() => this.tdv5g7bctk == (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 1;

      internal bool mwu5kK3wkv()
      {
        return this.tdv5g7bctk == (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 3 || this.tdv5g7bctk == (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 4;
      }

      internal bool lIM51cVqgP() => this.tdv5g7bctk == (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 2;

      internal bool jHU5KDfZOE() => this.tdv5g7bctk == (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 5;

      internal bool v3X5I7VrMk() => this.tdv5g7bctk == (Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 6;

      internal virtual bool U4w0RYoANI() => false;

      internal virtual bool buM08ZtT1C() => false;

      internal abstract void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      internal virtual bool raO0WHR6j4() => false;

      internal iJPJr2vZQxigsruTJoD(Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdv5g7bctk = _param1;
      }

      internal abstract object csZxJgmksq(Type _param1);

      internal abstract bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      internal abstract bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      internal abstract bool KOTxELbEBi();

      internal abstract Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON();

      internal virtual bool A6a0jc67Sw() => false;

      internal abstract void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1);

      internal static Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q qkD5Xvjwyq(Type _param0)
      {
        Type nullableType = _param0;
        if (!(nullableType != (Type) null))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 18;
        if (nullableType.IsByRef)
          nullableType = nullableType.GetElementType();
        if (nullableType != (Type) null && Nullable.GetUnderlyingType(nullableType) != (Type) null)
          nullableType = Nullable.GetUnderlyingType(nullableType);
        if (nullableType == typeof (string))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 14;
        if (nullableType == typeof (byte))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 2;
        if (nullableType == typeof (sbyte))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 1;
        if (nullableType == typeof (short))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 3;
        if (nullableType == typeof (ushort))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 4;
        if (nullableType == typeof (int))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 5;
        if (nullableType == typeof (uint))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 6;
        if (nullableType == typeof (long))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 7;
        if (nullableType == typeof (ulong))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 8;
        if (nullableType == typeof (float))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 9;
        if (nullableType == typeof (double))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 10;
        if (nullableType == typeof (bool))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11;
        if (nullableType == typeof (IntPtr))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 12;
        if (nullableType == typeof (UIntPtr))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 13;
        if (nullableType == typeof (char))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15;
        if (nullableType == typeof (object))
          return (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 0;
        return nullableType.IsEnum ? (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 16 : (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 17;
      }

      internal static Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD VZn55a16XL(
        Type _param0,
        object _param1)
      {
        Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q nm23HivOiE3tQlffD0q1 = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.qkD5Xvjwyq(_param0);
        Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q nm23HivOiE3tQlffD0q2 = (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 18;
        if (_param1 != null)
          nm23HivOiE3tQlffD0q2 = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.qkD5Xvjwyq(_param1.GetType());
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) null;
        switch (nm23HivOiE3tQlffD0q1)
        {
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 0:
            jr2vZqxigsruTjoD = nm23HivOiE3tQlffD0q2 != (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15 ? Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.sOT5UaVDDM(_param1) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab(_param1);
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 1:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 1:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (sbyte) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 2:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (sbyte) (byte) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = !(bool) _param1 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (sbyte) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 1);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 2:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 1:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (byte) (sbyte) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 2:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (byte) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = !(bool) _param1 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (byte) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 2);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 3:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 3:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (short) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = !(bool) _param1 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (short) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 3);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 4:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 4:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (ushort) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = !(bool) _param1 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 4);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 5:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 5:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = !(bool) _param1 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 5);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 6:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 6:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = !(bool) _param1 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(0U, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(1U, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 6);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 7:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 7:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((long) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = !(bool) _param1 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(0L, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(1L, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((long) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 7);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 8:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 8:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((ulong) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = !(bool) _param1 ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(0UL, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8) : (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10(1UL, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.itBkGFvUfKC0vY7aX10((ulong) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 8);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 9:
            if (nm23HivOiE3tQlffD0q2 != (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 9)
              throw new InvalidCastException();
            jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((float) _param1);
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 10:
            if (nm23HivOiE3tQlffD0q2 != (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 10)
              throw new InvalidCastException();
            jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.QYRNeVv2bRMHypRuM1G((double) _param1);
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 1:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((sbyte) _param1 != (sbyte) 0);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 2:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((byte) _param1 > (byte) 0);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 3:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((short) _param1 != (short) 0);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 4:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((ushort) _param1 > (ushort) 0);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 5:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) _param1 != 0);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 6:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((uint) _param1 > 0U);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 7:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((long) _param1 != 0L);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 8:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((ulong) _param1 > 0UL);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 9:
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 10:
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 12:
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 13:
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 14:
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 16:
                throw new InvalidCastException();
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 11:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((bool) _param1);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 18:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(false);
                break;
              default:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC(_param1 != null);
                break;
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 12:
            if (nm23HivOiE3tQlffD0q2 != (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 12)
              throw new InvalidCastException();
            jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((IntPtr) _param1);
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 13:
            if (nm23HivOiE3tQlffD0q2 != (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 13)
              throw new InvalidCastException();
            jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.jNKJmIvCmU1E1D3nvyx((UIntPtr) _param1);
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 14:
            jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mEOZvFveD19JmJrXYIC(_param1 as string);
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
            switch (nm23HivOiE3tQlffD0q2)
            {
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 1:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (sbyte) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 2:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (byte) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 3:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (short) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 4:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (ushort) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 5:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 6:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (uint) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
                break;
              case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 15:
                jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.okOOfhvXAxlsKuDwlgC((int) (char) _param1, (Bmdacxg2KgloU9v9tT.PS9K5DvLjvcEqTiB4Vv) 15);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 16:
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 17:
            jr2vZqxigsruTjoD = Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.sOT5UaVDDM(_param1);
            break;
          case (Bmdacxg2KgloU9v9tT.NM23HIvOiE3tQLFFD0q) 18:
            throw new InvalidCastException();
        }
        if (_param0.IsByRef)
          jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.DTlXqRvotj3wod6FC4n(jr2vZqxigsruTjoD, _param0.GetElementType());
        return jr2vZqxigsruTjoD;
      }

      private static Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD sOT5UaVDDM(object _param0)
      {
        if (_param0 != null && _param0.GetType().IsEnum)
        {
          Type underlyingType = Enum.GetUnderlyingType(_param0.GetType());
          object obj = Convert.ChangeType(_param0, underlyingType);
          Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.HQA5ClGVNq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD.VZn55a16XL(underlyingType, obj));
          if (jr2vZqxigsruTjoD != null)
            return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) (jr2vZqxigsruTjoD as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN);
        }
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) new Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab(_param0);
      }

      private static Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN HQA5ClGVNq(
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param0)
      {
        if (!(_param0 is Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN kp63vgBuPycmxljcN) && _param0.U4w0RYoANI())
          kp63vgBuPycmxljcN = _param0.BXg0EpmNON() as Bmdacxg2KgloU9v9tT.W1KP63vgBUPycmxljcN;
        return kp63vgBuPycmxljcN;
      }
    }

    private class mseRstvRCYFr1IHpBab : Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD
    {
      public object Vsk52uBQBq;
      public Type TTo5LRAMnI;

      public mseRstvRCYFr1IHpBab()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        this.\u002Ector((object) null);
      }

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        if (_param1 is Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab)
        {
          this.Vsk52uBQBq = ((Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab) _param1).Vsk52uBQBq;
          this.TTo5LRAMnI = ((Bmdacxg2KgloU9v9tT.mseRstvRCYFr1IHpBab) _param1).TTo5LRAMnI;
        }
        else
          this.Vsk52uBQBq = (object) _param1.BXg0EpmNON();
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.VPUxGtPN74(_param1);
      }

      public mseRstvRCYFr1IHpBab(object _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 0);
        this.Vsk52uBQBq = _param1;
        this.TTo5LRAMnI = (Type) null;
      }

      public mseRstvRCYFr1IHpBab(object _param1, Type _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 0);
        this.Vsk52uBQBq = _param1;
        this.TTo5LRAMnI = _param2;
      }

      public override string ToString()
      {
        return this.Vsk52uBQBq != null ? this.Vsk52uBQBq.ToString() : ((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 5).ToString();
      }

      internal override object csZxJgmksq(Type _param1)
      {
        if (this.Vsk52uBQBq == null)
          return (object) null;
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (!(this.Vsk52uBQBq is Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD))
        {
          object obj = this.Vsk52uBQBq;
          if (obj != null && _param1 != (Type) null && obj.GetType() != _param1)
          {
            if (_param1 == typeof (RuntimeFieldHandle) && (object) (obj as FieldInfo) != null)
              obj = (object) ((FieldInfo) obj).FieldHandle;
            else if (_param1 == typeof (RuntimeTypeHandle) && (object) (obj as Type) != null)
              obj = (object) ((Type) obj).TypeHandle;
            else if (_param1 == typeof (RuntimeMethodHandle) && (object) (obj as MethodBase) != null)
              obj = (object) ((MethodBase) obj).MethodHandle;
          }
          return obj;
        }
        if (this.TTo5LRAMnI != (Type) null)
          return ((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.Vsk52uBQBq).csZxJgmksq(this.TTo5LRAMnI);
        object obj1 = ((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this.Vsk52uBQBq).csZxJgmksq(_param1);
        if (obj1 != null && _param1 != (Type) null && obj1.GetType() != _param1)
        {
          if (_param1 == typeof (RuntimeFieldHandle) && (object) (obj1 as FieldInfo) != null)
            obj1 = (object) ((FieldInfo) obj1).FieldHandle;
          else if (_param1 == typeof (RuntimeTypeHandle) && (object) (obj1 as Type) != null)
            obj1 = (object) ((Type) obj1).TypeHandle;
          else if (_param1 == typeof (RuntimeMethodHandle) && (object) (obj1 as MethodBase) != null)
            obj1 = (object) ((MethodBase) obj1).MethodHandle;
        }
        return obj1;
      }

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        return _param1.U4w0RYoANI() ? _param1.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : this.csZxJgmksq((Type) null) == _param1.csZxJgmksq((Type) null);
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        return _param1.U4w0RYoANI() ? _param1.FIt0KIs55w((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : this.csZxJgmksq((Type) null) != _param1.csZxJgmksq((Type) null);
      }

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return !(this.Vsk52uBQBq is Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD vsk52uBqBq) ? (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this : vsk52uBqBq.BXg0EpmNON();
      }

      internal override bool KOTxELbEBi()
      {
        return this.Vsk52uBQBq != null && (!(this.Vsk52uBQBq is Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD vsk52uBqBq) || vsk52uBqBq.csZxJgmksq((Type) null) != null);
      }
    }

    private class mEOZvFveD19JmJrXYIC : Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD
    {
      public string FWy5OZs7uY;

      public mEOZvFveD19JmJrXYIC(string _param1)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector((Bmdacxg2KgloU9v9tT.y8tPEovwuEAmGj2wcSe) 6);
        this.FWy5OZs7uY = _param1;
      }

      internal override void VPUxGtPN74(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.FWy5OZs7uY = ((Bmdacxg2KgloU9v9tT.mEOZvFveD19JmJrXYIC) _param1).FWy5OZs7uY;
      }

      internal override void aIpx9u9Kmq(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.VPUxGtPN74(_param1);
      }

      public override string ToString()
      {
        if (this.FWy5OZs7uY == null)
          return ((Bmdacxg2KgloU9v9tT.xdlXOAvBJabyT4OJnWn) 5).ToString();
        char ch = '*';
        string str1 = ch.ToString();
        string fwy5Ozs7uY = this.FWy5OZs7uY;
        ch = '*';
        string str2 = ch.ToString();
        return str1 + fwy5Ozs7uY + str2;
      }

      internal override bool KOTxELbEBi() => this.FWy5OZs7uY != null;

      internal override object csZxJgmksq(Type _param1) => (object) this.FWy5OZs7uY;

      internal override bool SXw0swpnLE(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        return _param1.U4w0RYoANI() ? _param1.SXw0swpnLE((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : (object) this.FWy5OZs7uY == _param1.csZxJgmksq((Type) null);
      }

      internal override bool FIt0KIs55w(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        return _param1.U4w0RYoANI() ? _param1.FIt0KIs55w((Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this) : (object) this.FWy5OZs7uY != _param1.csZxJgmksq((Type) null);
      }

      internal override Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD BXg0EpmNON()
      {
        return (Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD) this;
      }
    }

    internal class v2STNJvydw751BYOUX4
    {
      private List<Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD> qRO58lF384;

      [SpecialName]
      public int stX5SwJdbc() => this.qRO58lF384.Count;

      public void q7750uuUI6() => this.qRO58lF384.Clear();

      public void x1C5GFNwKo(Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD _param1)
      {
        this.qRO58lF384.Add(_param1);
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD wFI5VtoADD()
      {
        return this.qRO58lF384[this.qRO58lF384.Count - 1];
      }

      public Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD Wv25rccvJo()
      {
        Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD jr2vZqxigsruTjoD = this.wFI5VtoADD();
        if (this.qRO58lF384.Count == 0)
          return jr2vZqxigsruTjoD;
        this.qRO58lF384.RemoveAt(this.qRO58lF384.Count - 1);
        return jr2vZqxigsruTjoD;
      }

      public v2STNJvydw751BYOUX4()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.qRO58lF384 = new List<Bmdacxg2KgloU9v9tT.iJPJr2vZQxigsruTJoD>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private struct d8DC6491193A5335
    {
      private StringBuilder sb;

      public d8DC6491193A5335(int _param1, int _param2)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.sb = new StringBuilder();
      }

      public d8DC6491193A5335(int _param1, int _param2, IFormatProvider _param3)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.sb = new StringBuilder();
      }

      public void AppendLiteral(string _param1)
      {
        if (_param1 == null)
          return;
        this.sb.Append(_param1);
      }

      public void AppendFormatted<T>(T value)
      {
        if ((object) value == null)
          return;
        this.sb.Append((object) value);
      }

      public void AppendFormatted<T>(T value, string _param2)
      {
        if (_param2 != null)
          this.sb.AppendFormat(_param2, (object) value);
        else
          this.sb.Append((object) value);
      }

      public void AppendFormatted<T>(T value, int _param2)
      {
        if ((object) value == null)
          return;
        this.sb.Append((object) value);
      }

      public void AppendFormatted<T>(T value, int _param2, string _param3)
      {
        if (_param3 != null)
          this.sb.AppendFormat(_param3, (object) value);
        else
          this.sb.Append((object) value);
      }

      public void AppendFormatted(string _param1)
      {
        if (_param1 == null)
          return;
        this.sb.Append(_param1);
      }

      public void AppendFormatted(string _param1, int _param2 = 0, string _param3 = null)
      {
        if (_param3 != null)
          this.sb.AppendFormat(_param3, (object) _param1);
        else
          this.sb.Append(_param1);
      }

      public void AppendFormatted(object _param1, int _param2 = 0, string _param3 = null)
      {
        if (_param3 != null)
          this.sb.AppendFormat(_param3, _param1);
        else
          this.sb.Append(_param1);
      }

      public string ToStringAndClear()
      {
        string stringAndClear = this.sb.ToString();
        this.sb.Clear();
        return stringAndClear;
      }
    }

    internal enum xdlXOAvBJabyT4OJnWn
    {
    }
  }
}
