// Decompiled with JetBrains decompiler
// Type: jOgQY3RGtH5fd9qQao.OBqe2IUAeSpOmlOQ4O
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace jOgQY3RGtH5fd9qQao
{
  internal class OBqe2IUAeSpOmlOQ4O
  {
    internal static OBqe2IUAeSpOmlOQ4O.JWGbihLe59w0Tjpgs7W[] PVGanEWhlM;
    internal static int[] oLdaoiVT7G;
    internal static List<string> tMVaB3hxr3;
    private static BinaryReader omRajHZ0wU;
    private static byte[] WABa0tWy6E;
    private static bool yHDalgja3A;
    private static object Wc3aXvakmy;

    internal static object[] rZnaaJrE25() => new object[1];

    internal static object[] qWAa9mQMya<T>(
      int _param0,
      object[] _param1,
      object _param2,
      ref T _param3)
    {
      OBqe2IUAeSpOmlOQ4O.JWGbihLe59w0Tjpgs7W gbihLe59w0Tjpgs7W = (OBqe2IUAeSpOmlOQ4O.JWGbihLe59w0Tjpgs7W) null;
      lock (OBqe2IUAeSpOmlOQ4O.Wc3aXvakmy)
      {
        if (!OBqe2IUAeSpOmlOQ4O.yHDalgja3A)
        {
          OBqe2IUAeSpOmlOQ4O.yHDalgja3A = true;
          OBqe2IUAeSpOmlOQ4O.cQqa7Eegfx();
        }
        if (OBqe2IUAeSpOmlOQ4O.PVGanEWhlM[_param0] != null)
        {
          gbihLe59w0Tjpgs7W = OBqe2IUAeSpOmlOQ4O.PVGanEWhlM[_param0];
        }
        else
        {
          OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.BaseStream.Position = (long) OBqe2IUAeSpOmlOQ4O.oLdaoiVT7G[_param0];
          gbihLe59w0Tjpgs7W = new OBqe2IUAeSpOmlOQ4O.JWGbihLe59w0Tjpgs7W();
          Module module = typeof (OBqe2IUAeSpOmlOQ4O).Module;
          int metadataToken = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
          int capacity1 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
          int capacity2 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
          int capacity3 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
          gbihLe59w0Tjpgs7W.qpZjJxQ46V = module.ResolveMethod(metadataToken);
          ParameterInfo[] parameters = gbihLe59w0Tjpgs7W.qpZjJxQ46V.GetParameters();
          gbihLe59w0Tjpgs7W.JDJjcB4nud = new OBqe2IUAeSpOmlOQ4O.j9Ha11LJoma4Y5DYntf[parameters.Length];
          for (int index = 0; index < parameters.Length; ++index)
          {
            Type type = parameters[index].ParameterType;
            OBqe2IUAeSpOmlOQ4O.j9Ha11LJoma4Y5DYntf ha11Ljoma4Y5Dyntf = new OBqe2IUAeSpOmlOQ4O.j9Ha11LJoma4Y5DYntf();
            ha11Ljoma4Y5Dyntf.UhNjdqxgic = type.IsByRef;
            ha11Ljoma4Y5Dyntf.UDsjvgp1ei = index;
            gbihLe59w0Tjpgs7W.JDJjcB4nud[index] = ha11Ljoma4Y5Dyntf;
            if (type.IsByRef)
              type = type.GetElementType();
            OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw hzqlQyLcdAbwFxsw = !(type == typeof (string)) ? (!(type == typeof (byte)) ? (!(type == typeof (sbyte)) ? (!(type == typeof (short)) ? (!(type == typeof (ushort)) ? (!(type == typeof (int)) ? (!(type == typeof (uint)) ? (!(type == typeof (long)) ? (!(type == typeof (ulong)) ? (!(type == typeof (float)) ? (!(type == typeof (double)) ? (!(type == typeof (bool)) ? (!(type == typeof (IntPtr)) ? (!(type == typeof (UIntPtr)) ? (!(type == typeof (char)) ? (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 0 : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 13) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 10) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 9) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2) : (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 14;
            ha11Ljoma4Y5Dyntf.RcQjuILAQC = hzqlQyLcdAbwFxsw;
          }
          gbihLe59w0Tjpgs7W.fBcjhfhonR = new List<OBqe2IUAeSpOmlOQ4O.V6kKaOL2XvatnIN8I0Z>(capacity1);
          for (int index = 0; index < capacity1; ++index)
          {
            int num = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
            OBqe2IUAeSpOmlOQ4O.V6kKaOL2XvatnIN8I0Z kaOl2XvatnIn8I0Z = new OBqe2IUAeSpOmlOQ4O.V6kKaOL2XvatnIN8I0Z();
            kaOl2XvatnIn8I0Z.N3YjEEsk0h = (Type) null;
            if (num >= 0 && num < 50)
            {
              kaOl2XvatnIn8I0Z.WhxjRcXFCB = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) (num & 31);
              kaOl2XvatnIn8I0Z.r4GjQb7Vvg = (num & 32) > 0;
            }
            kaOl2XvatnIn8I0Z.XH6jUerVZS = index;
            gbihLe59w0Tjpgs7W.fBcjhfhonR.Add(kaOl2XvatnIn8I0Z);
          }
          gbihLe59w0Tjpgs7W.C4ujeQYOhV = new List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp>(capacity2);
          for (int index = 0; index < capacity2; ++index)
          {
            int num1 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
            int num2 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
            OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp = new OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp();
            i4gLcuIdaB2erwhp.KBbj4IwY3n = num1;
            i4gLcuIdaB2erwhp.CrWjWZF3Io = num2;
            OBqe2IUAeSpOmlOQ4O.bUeJteLhNL76rUQL5ZO jteLhNl76rUqL5Zo = new OBqe2IUAeSpOmlOQ4O.bUeJteLhNL76rUQL5ZO();
            i4gLcuIdaB2erwhp.XFAjTPCIM3 = jteLhNl76rUqL5Zo;
            int num3 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
            int num4 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
            int num5 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
            jteLhNl76rUqL5Zo.F9CjHFtGZY = num3;
            jteLhNl76rUqL5Zo.FTijSNsYde = num4;
            jteLhNl76rUqL5Zo.y6mjtIyqq4 = num5;
            switch (num5)
            {
              case 0:
                jteLhNl76rUqL5Zo.nWdjmd2WlN = module.ResolveType(OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU));
                break;
              case 1:
                jteLhNl76rUqL5Zo.Dpmj6iSp4C = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
                break;
              default:
                OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
                break;
            }
            gbihLe59w0Tjpgs7W.C4ujeQYOhV.Add(i4gLcuIdaB2erwhp);
          }
          gbihLe59w0Tjpgs7W.C4ujeQYOhV.Sort((Comparison<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp>) ((x, y) => x.XFAjTPCIM3.F9CjHFtGZY.CompareTo(y.XFAjTPCIM3.F9CjHFtGZY)));
          gbihLe59w0Tjpgs7W.J96j2oqJsf = new List<OBqe2IUAeSpOmlOQ4O.K14OTLLT4lAAZwtUFKB>(capacity3);
          for (int index1 = 0; index1 < capacity3; ++index1)
          {
            OBqe2IUAeSpOmlOQ4O.K14OTLLT4lAAZwtUFKB otllT4lAaZwtUfkb = new OBqe2IUAeSpOmlOQ4O.K14OTLLT4lAAZwtUFKB();
            byte index2 = OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.ReadByte();
            otllT4lAaZwtUfkb.wk1jFwCKSW = (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) index2;
            int num = index2 < (byte) 176 ? (int) OBqe2IUAeSpOmlOQ4O.WABa0tWy6E[(int) index2] : throw new Exception();
            if (num == 0)
            {
              otllT4lAaZwtUfkb.PTyjO0yydc = (object) null;
            }
            else
            {
              object obj;
              switch (num - 1)
              {
                case 0:
                  obj = (object) OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
                  break;
                case 1:
                  obj = (object) OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.ReadInt64();
                  break;
                case 2:
                  obj = (object) OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.ReadSingle();
                  break;
                case 3:
                  obj = (object) OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.ReadDouble();
                  break;
                case 4:
                  int length = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
                  int[] numArray = new int[length];
                  for (int index3 = 0; index3 < length; ++index3)
                    numArray[index3] = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
                  obj = (object) numArray;
                  break;
                default:
                  throw new Exception();
              }
              otllT4lAaZwtUfkb.PTyjO0yydc = obj;
            }
            gbihLe59w0Tjpgs7W.J96j2oqJsf.Add(otllT4lAaZwtUfkb);
          }
          OBqe2IUAeSpOmlOQ4O.PVGanEWhlM[_param0] = gbihLe59w0Tjpgs7W;
        }
      }
      OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52 x6oonhL1IpoG9H0Sh52 = new OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52();
      x6oonhL1IpoG9H0Sh52.eV7XiE3C4H = gbihLe59w0Tjpgs7W;
      ParameterInfo[] parameters1 = gbihLe59w0Tjpgs7W.qpZjJxQ46V.GetParameters();
      bool flag = false;
      int num6 = 0;
      if ((object) (gbihLe59w0Tjpgs7W.qpZjJxQ46V as MethodInfo) != null && ((MethodInfo) gbihLe59w0Tjpgs7W.qpZjJxQ46V).ReturnType != typeof (void))
        flag = true;
      if (gbihLe59w0Tjpgs7W.qpZjJxQ46V.IsStatic)
      {
        x6oonhL1IpoG9H0Sh52.GL6XCXXAY9 = new OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[parameters1.Length];
        for (int index = 0; index < parameters1.Length; ++index)
        {
          Type parameterType = parameters1[index].ParameterType;
          x6oonhL1IpoG9H0Sh52.GL6XCXXAY9[index] = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameterType, _param1[index]);
          if (parameterType.IsByRef)
            ++num6;
        }
      }
      else
      {
        x6oonhL1IpoG9H0Sh52.GL6XCXXAY9 = new OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[parameters1.Length + 1];
        x6oonhL1IpoG9H0Sh52.GL6XCXXAY9[0] = !gbihLe59w0Tjpgs7W.qpZjJxQ46V.DeclaringType.IsValueType ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB(_param2) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB(_param2), gbihLe59w0Tjpgs7W.qpZjJxQ46V.DeclaringType);
        for (int index = 0; index < parameters1.Length; ++index)
        {
          Type parameterType = parameters1[index].ParameterType;
          if (parameterType.IsByRef)
          {
            x6oonhL1IpoG9H0Sh52.GL6XCXXAY9[index + 1] = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameterType, _param1[index]);
            ++num6;
          }
          else
            x6oonhL1IpoG9H0Sh52.GL6XCXXAY9[index + 1] = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameterType, _param1[index]);
        }
      }
      x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS = new OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[gbihLe59w0Tjpgs7W.fBcjhfhonR.Count];
      for (int index = 0; index < gbihLe59w0Tjpgs7W.fBcjhfhonR.Count; ++index)
      {
        OBqe2IUAeSpOmlOQ4O.V6kKaOL2XvatnIN8I0Z kaOl2XvatnIn8I0Z = gbihLe59w0Tjpgs7W.fBcjhfhonR[index];
        switch (kaOl2XvatnIn8I0Z.WhxjRcXFCB)
        {
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 0:
            x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS[index] = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) null;
            break;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15:
            x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS[index] = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0, kaOl2XvatnIn8I0Z.WhxjRcXFCB);
            break;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8:
            x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS[index] = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(0L, kaOl2XvatnIn8I0Z.WhxjRcXFCB);
            break;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 9:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 10:
            x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS[index] = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(0.0, kaOl2XvatnIn8I0Z.WhxjRcXFCB);
            break;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12:
            x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS[index] = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(IntPtr.Zero);
            break;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 13:
            x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS[index] = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(UIntPtr.Zero);
            break;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 14:
            x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS[index] = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) null;
            break;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 16:
            x6oonhL1IpoG9H0Sh52.Ti3XqqGbnS[index] = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null);
            break;
        }
      }
      try
      {
        x6oonhL1IpoG9H0Sh52.ygDj1mPbak();
      }
      finally
      {
        x6oonhL1IpoG9H0Sh52.liejY9ZWI8();
      }
      int num7 = 0;
      if (flag)
        num7 = 1;
      object[] objArray = new object[num7 + num6];
      if (flag)
        objArray[0] = (object) null;
      if ((object) (gbihLe59w0Tjpgs7W.qpZjJxQ46V as MethodInfo) != null)
      {
        MethodInfo qpZjJxQ46V = (MethodInfo) gbihLe59w0Tjpgs7W.qpZjJxQ46V;
        if (qpZjJxQ46V.ReturnType != typeof (void) && x6oonhL1IpoG9H0Sh52.Gl4XYW9Iw1 != null)
          objArray[0] = x6oonhL1IpoG9H0Sh52.Gl4XYW9Iw1.MNddRugcTR(qpZjJxQ46V.ReturnType);
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
            objArray[index4] = x6oonhL1IpoG9H0Sh52.GL6XCXXAY9[index5] == null ? (object) null : (!gbihLe59w0Tjpgs7W.qpZjJxQ46V.IsStatic ? x6oonhL1IpoG9H0Sh52.GL6XCXXAY9[index5 + 1].MNddRugcTR(elementType) : x6oonhL1IpoG9H0Sh52.GL6XCXXAY9[index5].MNddRugcTR(elementType));
            ++index4;
          }
        }
      }
      if (!gbihLe59w0Tjpgs7W.qpZjJxQ46V.IsStatic && gbihLe59w0Tjpgs7W.qpZjJxQ46V.DeclaringType.IsValueType)
        _param3 = (T) x6oonhL1IpoG9H0Sh52.GL6XCXXAY9[0].MNddRugcTR(gbihLe59w0Tjpgs7W.qpZjJxQ46V.DeclaringType);
      return objArray;
    }

    internal static object[] TyOaFSuuHy(int _param0, object[] _param1, object _param2)
    {
      int num = 0;
      return OBqe2IUAeSpOmlOQ4O.qWAa9mQMya<int>(_param0, _param1, _param2, ref num);
    }

    internal static object[] R2paOF1pXp<T>(int _param0, object[] _param1, ref T _param2)
    {
      return OBqe2IUAeSpOmlOQ4O.qWAa9mQMya<T>(_param0, _param1, (object) _param2, ref _param2);
    }

    internal static void cQqa7Eegfx()
    {
      if (OBqe2IUAeSpOmlOQ4O.oLdaoiVT7G != null)
        return;
      BinaryReader binaryReader = new BinaryReader(typeof (OBqe2IUAeSpOmlOQ4O).Assembly.GetManifestResourceStream("X9QOdhMxcLpPO7IeSX.4RL2YlJy5o4fXFIHVx"));
      binaryReader.BaseStream.Position = 0L;
      byte[] numArray = binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
      binaryReader.Close();
      OBqe2IUAeSpOmlOQ4O.dbeaA8jlro(numArray);
    }

    internal static void dbeaA8jlro(byte[] _param0)
    {
      OBqe2IUAeSpOmlOQ4O.omRajHZ0wU = new BinaryReader((Stream) new MemoryStream(_param0));
      OBqe2IUAeSpOmlOQ4O.WABa0tWy6E = new byte[(int) byte.MaxValue];
      int num1 = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int index2 = (int) OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.ReadByte();
        OBqe2IUAeSpOmlOQ4O.WABa0tWy6E[index2] = OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.ReadByte();
      }
      int capacity = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
      OBqe2IUAeSpOmlOQ4O.tMVaB3hxr3 = new List<string>(capacity);
      for (int index = 0; index < capacity; ++index)
        OBqe2IUAeSpOmlOQ4O.tMVaB3hxr3.Add(Encoding.Unicode.GetString(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.ReadBytes(OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU))));
      int length = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
      OBqe2IUAeSpOmlOQ4O.PVGanEWhlM = new OBqe2IUAeSpOmlOQ4O.JWGbihLe59w0Tjpgs7W[length];
      OBqe2IUAeSpOmlOQ4O.oLdaoiVT7G = new int[length];
      for (int index = 0; index < length; ++index)
      {
        OBqe2IUAeSpOmlOQ4O.PVGanEWhlM[index] = (OBqe2IUAeSpOmlOQ4O.JWGbihLe59w0Tjpgs7W) null;
        OBqe2IUAeSpOmlOQ4O.oLdaoiVT7G[index] = OBqe2IUAeSpOmlOQ4O.wqZaDFXp8N(OBqe2IUAeSpOmlOQ4O.omRajHZ0wU);
      }
      int position = (int) OBqe2IUAeSpOmlOQ4O.omRajHZ0wU.BaseStream.Position;
      for (int index = 0; index < length; ++index)
      {
        int num2 = OBqe2IUAeSpOmlOQ4O.oLdaoiVT7G[index];
        OBqe2IUAeSpOmlOQ4O.oLdaoiVT7G[index] = position;
        position += num2;
      }
    }

    internal static int wqZaDFXp8N(BinaryReader _param0)
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

    public OBqe2IUAeSpOmlOQ4O()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static OBqe2IUAeSpOmlOQ4O()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      OBqe2IUAeSpOmlOQ4O.PVGanEWhlM = (OBqe2IUAeSpOmlOQ4O.JWGbihLe59w0Tjpgs7W[]) null;
      OBqe2IUAeSpOmlOQ4O.oLdaoiVT7G = (int[]) null;
      OBqe2IUAeSpOmlOQ4O.yHDalgja3A = false;
      OBqe2IUAeSpOmlOQ4O.Wc3aXvakmy = new object();
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct hSn0ucLlLMaeFvn5InR
    {
      [FieldOffset(0)]
      public byte C1oBHylShu;
      [FieldOffset(0)]
      public sbyte AosBSnBKRL;
      [FieldOffset(0)]
      public ushort stSBbD5mwM;
      [FieldOffset(0)]
      public short BwOBmMoGP6;
      [FieldOffset(0)]
      public uint MlKB6i90Nn;
      [FieldOffset(0)]
      public int GLgBtRhyOg;
    }

    private class bTUKqtLXplwjOaJo6Hj : OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA
    {
      public OBqe2IUAeSpOmlOQ4O.hSn0ucLlLMaeFvn5InR d37Bh8uTDv;
      public OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw Y69BeUt9vR;

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.d37Bh8uTDv = ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv;
        this.Y69BeUt9vR = ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).Y69BeUt9vR;
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.nOQdl4ODOg(_param1);
      }

      public bTUKqtLXplwjOaJo6Hj(bool _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 1;
        this.d37Bh8uTDv.GLgBtRhyOg = !_param1 ? 0 : 1;
        this.Y69BeUt9vR = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11;
      }

      public bTUKqtLXplwjOaJo6Hj(OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = _param1.tdyvUPSCoA;
        this.d37Bh8uTDv.GLgBtRhyOg = _param1.d37Bh8uTDv.GLgBtRhyOg;
        this.Y69BeUt9vR = _param1.Y69BeUt9vR;
      }

      public override OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA q9qdvQao7g()
      {
        return (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this);
      }

      public bTUKqtLXplwjOaJo6Hj(int _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 1;
        this.d37Bh8uTDv.GLgBtRhyOg = _param1;
        this.Y69BeUt9vR = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5;
      }

      public bTUKqtLXplwjOaJo6Hj(uint _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 1;
        this.d37Bh8uTDv.MlKB6i90Nn = _param1;
        this.Y69BeUt9vR = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6;
      }

      public bTUKqtLXplwjOaJo6Hj(int _param1, OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 1;
        this.d37Bh8uTDv.GLgBtRhyOg = _param1;
        this.Y69BeUt9vR = _param2;
      }

      public bTUKqtLXplwjOaJo6Hj(uint _param1, OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 1;
        this.d37Bh8uTDv.MlKB6i90Nn = _param1;
        this.Y69BeUt9vR = _param2;
      }

      public override bool DpYddoq5nS()
      {
        switch (this.Y69BeUt9vR)
        {
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15:
            return this.d37Bh8uTDv.GLgBtRhyOg == 0;
          default:
            return this.d37Bh8uTDv.MlKB6i90Nn == 0U;
        }
      }

      public override bool vUcduRRnlL() => !this.DpYddoq5nS();

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX sqedUSL72O(
        OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param1)
      {
        switch (_param1)
        {
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.B0Yd4gvqjc();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.X75dWxBQwq();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.dCrdTAgdCS();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.C5idHWm1cv();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.YjldSFyrtV();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.d8IdbKZ3nT();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.c8idQhNv3S();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.wLUB2Orv23();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 16:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.q9qdvQao7g();
          default:
            throw new Exception(((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 4).ToString());
        }
      }

      internal override object MNddRugcTR(Type _param1)
      {
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (_param1 != (Type) null && Nullable.GetUnderlyingType(_param1) != (Type) null)
          _param1 = Nullable.GetUnderlyingType(_param1);
        if (_param1 == (Type) null || _param1 == typeof (object))
        {
          switch (this.Y69BeUt9vR)
          {
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
              return (object) this.d37Bh8uTDv.AosBSnBKRL;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2:
              return (object) this.d37Bh8uTDv.C1oBHylShu;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
              return (object) this.d37Bh8uTDv.BwOBmMoGP6;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4:
              return (object) this.d37Bh8uTDv.stSBbD5mwM;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
              return (object) this.d37Bh8uTDv.GLgBtRhyOg;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6:
              return (object) this.d37Bh8uTDv.MlKB6i90Nn;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7:
              return (object) (long) this.d37Bh8uTDv.GLgBtRhyOg;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8:
              return (object) (ulong) this.d37Bh8uTDv.MlKB6i90Nn;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
              return (object) this.vUcduRRnlL();
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15:
              return (object) (char) this.d37Bh8uTDv.GLgBtRhyOg;
            default:
              return (object) this.d37Bh8uTDv.GLgBtRhyOg;
          }
        }
        else
        {
          if (_param1 == typeof (int))
            return (object) this.d37Bh8uTDv.GLgBtRhyOg;
          if (_param1 == typeof (uint))
            return (object) this.d37Bh8uTDv.MlKB6i90Nn;
          if (_param1 == typeof (short))
            return (object) this.d37Bh8uTDv.BwOBmMoGP6;
          if (_param1 == typeof (ushort))
            return (object) this.d37Bh8uTDv.stSBbD5mwM;
          if (_param1 == typeof (byte))
            return (object) this.d37Bh8uTDv.C1oBHylShu;
          if (_param1 == typeof (sbyte))
            return (object) this.d37Bh8uTDv.AosBSnBKRL;
          if (_param1 == typeof (bool))
            return (object) !this.DpYddoq5nS();
          if (_param1 == typeof (long))
            return (object) (long) this.d37Bh8uTDv.GLgBtRhyOg;
          if (_param1 == typeof (ulong))
            return (object) (ulong) this.d37Bh8uTDv.MlKB6i90Nn;
          if (_param1 == typeof (char))
            return (object) (char) this.d37Bh8uTDv.GLgBtRhyOg;
          if (_param1 == typeof (IntPtr))
            return (object) new IntPtr(this.d37Bh8uTDv.GLgBtRhyOg);
          if (_param1 == typeof (UIntPtr))
            return (object) new UIntPtr(this.d37Bh8uTDv.MlKB6i90Nn);
          return _param1.IsEnum ? this.d6IBJRRp2Z(_param1) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        }
      }

      internal object d6IBJRRp2Z(Type _param1)
      {
        Type underlyingType = Enum.GetUnderlyingType(_param1);
        if (underlyingType == typeof (int))
          return Enum.ToObject(_param1, this.d37Bh8uTDv.GLgBtRhyOg);
        if (underlyingType == typeof (uint))
          return Enum.ToObject(_param1, this.d37Bh8uTDv.MlKB6i90Nn);
        if (underlyingType == typeof (short))
          return Enum.ToObject(_param1, this.d37Bh8uTDv.BwOBmMoGP6);
        if (underlyingType == typeof (ushort))
          return Enum.ToObject(_param1, this.d37Bh8uTDv.stSBbD5mwM);
        if (underlyingType == typeof (byte))
          return Enum.ToObject(_param1, this.d37Bh8uTDv.C1oBHylShu);
        if (underlyingType == typeof (sbyte))
          return Enum.ToObject(_param1, this.d37Bh8uTDv.AosBSnBKRL);
        if (underlyingType == typeof (long))
          return Enum.ToObject(_param1, (long) this.d37Bh8uTDv.GLgBtRhyOg);
        if (underlyingType == typeof (ulong))
          return Enum.ToObject(_param1, (ulong) this.d37Bh8uTDv.MlKB6i90Nn);
        return underlyingType == typeof (char) ? Enum.ToObject(_param1, (ushort) this.d37Bh8uTDv.GLgBtRhyOg) : Enum.ToObject(_param1, this.d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj c8idQhNv3S()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.DpYddoq5nS() ? 0 : 1);
      }

      internal override bool V1kdEyl02V() => this.vUcduRRnlL();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj B0Yd4gvqjc()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) this.d37Bh8uTDv.AosBSnBKRL, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj wLUB2Orv23()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj X75dWxBQwq()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) this.d37Bh8uTDv.C1oBHylShu, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj dCrdTAgdCS()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) this.d37Bh8uTDv.BwOBmMoGP6, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj C5idHWm1cv()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) this.d37Bh8uTDv.stSBbD5mwM, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj YjldSFyrtV()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj d8IdbKZ3nT()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.MlKB6i90Nn, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p j56dmjESoe()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((long) this.d37Bh8uTDv.GLgBtRhyOg, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p kv0d625INd()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((ulong) this.d37Bh8uTDv.MlKB6i90Nn, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj OxXdtNlUQr() => this.B0Yd4gvqjc();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj H7PdJ5iAPU() => this.dCrdTAgdCS();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj RgCd2ry1ac() => this.YjldSFyrtV();

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p YEUdcnGvKX() => this.j56dmjESoe();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj m0QdhS1F5D() => this.X75dWxBQwq();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Gsjdeg8ja3() => this.C5idHWm1cv();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj R3wdfbZewF() => this.d8IdbKZ3nT();

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p wsddpbkEQb() => this.kv0d625INd();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jyudNqrNXJ()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((sbyte) this.d37Bh8uTDv.GLgBtRhyOg), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj LmFdi8gd0R()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((sbyte) this.d37Bh8uTDv.MlKB6i90Nn), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj AYcdCCnBuC()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((short) this.d37Bh8uTDv.GLgBtRhyOg), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj usfdqHavse()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((short) this.d37Bh8uTDv.MlKB6i90Nn), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj K2Gd1PW7Sr()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj fATdY7yOVp()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((int) this.d37Bh8uTDv.MlKB6i90Nn), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p mgbdkYqhCS()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((long) this.d37Bh8uTDv.GLgBtRhyOg, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p t8YdsLWtVo()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((long) this.d37Bh8uTDv.MlKB6i90Nn, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj IYDdrwuQUc()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((byte) this.d37Bh8uTDv.GLgBtRhyOg), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj PvEdZLuSne()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((byte) this.d37Bh8uTDv.MlKB6i90Nn), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Y08dPd87iv()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((ushort) this.d37Bh8uTDv.GLgBtRhyOg), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jW7dKWHyFW()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((ushort) this.d37Bh8uTDv.MlKB6i90Nn), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj g1bdVnKyeV()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((uint) this.d37Bh8uTDv.GLgBtRhyOg), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj NWXd8DtKgs()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.MlKB6i90Nn, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p c5AdgYKqRb()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked ((ulong) this.d37Bh8uTDv.GLgBtRhyOg), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p fcnd3S7gbG()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((ulong) this.d37Bh8uTDv.MlKB6i90Nn, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ EAfdGAC0F3()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((float) this.d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ rcedIif63y()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((double) this.d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ oUfdwd7RBO()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((double) this.d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 u7udyDaiuk()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.YEUdcnGvKX().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.RgCd2ry1ac().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 erPdzHD1s2()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.wsddpbkEQb().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.R3wdfbZewF().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 HhEuMQLBcy()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.mgbdkYqhCS().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.K2Gd1PW7Sr().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 T52uLk6FSX()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.c5AdgYKqRb().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.g1bdVnKyeV().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 fDcu5RVPtU()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.t8YdsLWtVo().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.fATdY7yOVp().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 VVAuxnv6BF()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.fcnd3S7gbG().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.NWXd8DtKgs().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PPauaFcpRf()
      {
        switch (this.Y69BeUt9vR)
        {
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(-this.d37Bh8uTDv.GLgBtRhyOg);
          default:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) -this.d37Bh8uTDv.MlKB6i90Nn);
        }
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX Add(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg + ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).Add((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX rFou9powA0(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked (this.d37Bh8uTDv.GLgBtRhyOg + ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).rFou9powA0((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX TvpuFtHMx3(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked (this.d37Bh8uTDv.MlKB6i90Nn + ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn));
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).TvpuFtHMx3((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX d69uOOjWrX(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg - ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).RvoB8BAV82((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX XJsu7LDPbi(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked (this.d37Bh8uTDv.GLgBtRhyOg - ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).diBBgFLiiU((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX AKNuAZ11DR(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked (this.d37Bh8uTDv.MlKB6i90Nn - ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn));
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).EMWB3yONoS((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PZYuDsqDg5(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg * ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).PZYuDsqDg5((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX OJsungeQZd(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked (this.d37Bh8uTDv.GLgBtRhyOg * ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).OJsungeQZd((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX CrluoqnI7g(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked (this.d37Bh8uTDv.MlKB6i90Nn * ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn));
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).CrluoqnI7g((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX fdRuBgjrLs(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg / ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).A91BGcXunp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX YYHujsF82I(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.MlKB6i90Nn / ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).C1lBIaw5rr((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX HL6u0YI3Yb(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg % ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).A4nBwDQQ4V((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX S5MuleAUoL(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.MlKB6i90Nn % ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).yIwByEsukh((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX pY5uXSxpHK(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg & ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).pY5uXSxpHK((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX qmOuvwnxTA(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg | ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).qmOuvwnxTA((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX LJ3udQlIYO()
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(~this.d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX N4muuK8Z5O(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg ^ ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).N4muuK8Z5O((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX KGbuURteF6(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg << ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).OKCjLo23lH(this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX cSauRN3PjN(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.GLgBtRhyOg >> ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).Cp3jMBVv14(this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX xXAuQJbXWE(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.d37Bh8uTDv.MlKB6i90Nn >> ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).qiFBz81Hu6(this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override string ToString()
      {
        switch (this.Y69BeUt9vR)
        {
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
            return this.d37Bh8uTDv.GLgBtRhyOg.ToString();
          default:
            return this.d37Bh8uTDv.MlKB6i90Nn.ToString();
        }
      }

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this;
      }

      internal override bool gKku4OTsTL() => true;

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.W6wvnQlCGV())
          return _param1.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        if (_param1.DWVuea1EMj())
          return _param1.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = _param1.gRCuEnOnhD();
        if (!lxulLsB1AbCdvkPgx.gKku4OTsTL() || lxulLsB1AbCdvkPgx.E4wvjiQ2aY())
          return false;
        return lxulLsB1AbCdvkPgx.glCvoAFEwW() ? this.d37Bh8uTDv.GLgBtRhyOg == ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) lxulLsB1AbCdvkPgx).d37Bh8uTDv.GLgBtRhyOg : lxulLsB1AbCdvkPgx.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
      }

      private static OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA tJ9BciOew0(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param0)
      {
        if (!(_param0 is OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA) && _param0.DWVuea1EMj())
          wqpLuhZiq5CncNsA = _param0.gRCuEnOnhD() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA;
        return wqpLuhZiq5CncNsA;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.W6wvnQlCGV())
          return false;
        if (_param1.DWVuea1EMj())
          return _param1.BeouTiljCp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = _param1.gRCuEnOnhD();
        if (!lxulLsB1AbCdvkPgx.gKku4OTsTL() || lxulLsB1AbCdvkPgx.E4wvjiQ2aY())
          return false;
        return lxulLsB1AbCdvkPgx.glCvoAFEwW() ? (int) this.d37Bh8uTDv.MlKB6i90Nn != (int) ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) lxulLsB1AbCdvkPgx).d37Bh8uTDv.MlKB6i90Nn : lxulLsB1AbCdvkPgx.BeouTiljCp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
      }

      public override bool a5iuHuxxL9(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return this.d37Bh8uTDv.GLgBtRhyOg >= ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).oj9u6Fhwca((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool AMUuSZ5Bny(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return this.d37Bh8uTDv.MlKB6i90Nn >= ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).kZGutsdjSl((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool s3rubpelFd(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return this.d37Bh8uTDv.GLgBtRhyOg > ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).soGuJAfcec((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool lwlumgaheq(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return this.d37Bh8uTDv.MlKB6i90Nn > ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).UOZu2PVRec((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool oj9u6Fhwca(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return this.d37Bh8uTDv.GLgBtRhyOg <= ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).a5iuHuxxL9((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool kZGutsdjSl(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return this.d37Bh8uTDv.MlKB6i90Nn <= ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).AMUuSZ5Bny((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool soGuJAfcec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return this.d37Bh8uTDv.GLgBtRhyOg < ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).s3rubpelFd((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool UOZu2PVRec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return this.d37Bh8uTDv.MlKB6i90Nn < ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).lwlumgaheq((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct XpYRXgLvS2Z8HELLCVO
    {
      [FieldOffset(0)]
      public byte FCHBfwKmRn;
      [FieldOffset(0)]
      public sbyte hA1BpvTi1k;
      [FieldOffset(0)]
      public ushort QmSBNgiBCc;
      [FieldOffset(0)]
      public short SF9BiX4ruC;
      [FieldOffset(0)]
      public uint oPHBCnNDZA;
      [FieldOffset(0)]
      public int qt6BqjXoyG;
      [FieldOffset(0)]
      public ulong HSsB1KnPq6;
      [FieldOffset(0)]
      public long DoQBY2qDkD;
    }

    private class YaeDDELdekEtZ4qXJ1p : OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA
    {
      public OBqe2IUAeSpOmlOQ4O.XpYRXgLvS2Z8HELLCVO EtUBZQQZRa;
      public OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw WAmBPMB0G0;

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.EtUBZQQZRa = ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa;
        this.WAmBPMB0G0 = ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).WAmBPMB0G0;
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.nOQdl4ODOg(_param1);
      }

      public YaeDDELdekEtZ4qXJ1p(long _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 2;
        this.EtUBZQQZRa.DoQBY2qDkD = _param1;
        this.WAmBPMB0G0 = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7;
      }

      public YaeDDELdekEtZ4qXJ1p(OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = _param1.tdyvUPSCoA;
        this.EtUBZQQZRa.DoQBY2qDkD = _param1.EtUBZQQZRa.DoQBY2qDkD;
        this.WAmBPMB0G0 = _param1.WAmBPMB0G0;
      }

      public override OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA q9qdvQao7g()
      {
        return (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this);
      }

      public YaeDDELdekEtZ4qXJ1p(long _param1, OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 2;
        this.EtUBZQQZRa.DoQBY2qDkD = _param1;
        this.WAmBPMB0G0 = _param2;
      }

      public YaeDDELdekEtZ4qXJ1p(ulong _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 2;
        this.EtUBZQQZRa.HSsB1KnPq6 = _param1;
        this.WAmBPMB0G0 = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8;
      }

      public YaeDDELdekEtZ4qXJ1p(ulong _param1, OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 2;
        this.EtUBZQQZRa.HSsB1KnPq6 = _param1;
        this.WAmBPMB0G0 = _param2;
      }

      public override bool DpYddoq5nS()
      {
        return this.WAmBPMB0G0 == (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7 ? this.EtUBZQQZRa.DoQBY2qDkD == 0L : this.EtUBZQQZRa.HSsB1KnPq6 == 0UL;
      }

      public override bool vUcduRRnlL() => !this.DpYddoq5nS();

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX sqedUSL72O(
        OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param1)
      {
        switch (_param1)
        {
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.B0Yd4gvqjc();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.X75dWxBQwq();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.dCrdTAgdCS();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.C5idHWm1cv();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.YjldSFyrtV();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.d8IdbKZ3nT();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.j56dmjESoe();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.kv0d625INd();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.c8idQhNv3S();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.kHZBssPC3s();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 16:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.q9qdvQao7g();
          default:
            throw new Exception(((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 4).ToString());
        }
      }

      internal override object MNddRugcTR(Type _param1)
      {
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (_param1 == (Type) null || _param1 == typeof (object))
        {
          switch (this.WAmBPMB0G0)
          {
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
              return (object) this.EtUBZQQZRa.hA1BpvTi1k;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2:
              return (object) this.EtUBZQQZRa.FCHBfwKmRn;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
              return (object) this.EtUBZQQZRa.SF9BiX4ruC;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4:
              return (object) this.EtUBZQQZRa.QmSBNgiBCc;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
              return (object) this.EtUBZQQZRa.qt6BqjXoyG;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6:
              return (object) this.EtUBZQQZRa.oPHBCnNDZA;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7:
              return (object) this.EtUBZQQZRa.DoQBY2qDkD;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8:
              return (object) this.EtUBZQQZRa.HSsB1KnPq6;
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
              return (object) this.vUcduRRnlL();
            case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15:
              return (object) (char) this.EtUBZQQZRa.qt6BqjXoyG;
            default:
              return (object) this.EtUBZQQZRa.DoQBY2qDkD;
          }
        }
        else
        {
          if (_param1 == typeof (int))
            return (object) this.EtUBZQQZRa.qt6BqjXoyG;
          if (_param1 == typeof (uint))
            return (object) this.EtUBZQQZRa.oPHBCnNDZA;
          if (_param1 == typeof (short))
            return (object) this.EtUBZQQZRa.SF9BiX4ruC;
          if (_param1 == typeof (ushort))
            return (object) this.EtUBZQQZRa.QmSBNgiBCc;
          if (_param1 == typeof (byte))
            return (object) this.EtUBZQQZRa.FCHBfwKmRn;
          if (_param1 == typeof (sbyte))
            return (object) this.EtUBZQQZRa.hA1BpvTi1k;
          if (_param1 == typeof (bool))
            return (object) !this.DpYddoq5nS();
          if (_param1 == typeof (long))
            return (object) this.EtUBZQQZRa.DoQBY2qDkD;
          if (_param1 == typeof (ulong))
            return (object) this.EtUBZQQZRa.HSsB1KnPq6;
          if (_param1 == typeof (char))
            return (object) (char) this.EtUBZQQZRa.DoQBY2qDkD;
          return _param1.IsEnum ? this.qU2BkdrOYs(_param1) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        }
      }

      internal object qU2BkdrOYs(Type _param1)
      {
        Type underlyingType = Enum.GetUnderlyingType(_param1);
        if (underlyingType == typeof (int))
          return Enum.ToObject(_param1, this.EtUBZQQZRa.qt6BqjXoyG);
        if (underlyingType == typeof (uint))
          return Enum.ToObject(_param1, this.EtUBZQQZRa.oPHBCnNDZA);
        if (underlyingType == typeof (short))
          return Enum.ToObject(_param1, this.EtUBZQQZRa.SF9BiX4ruC);
        if (underlyingType == typeof (ushort))
          return Enum.ToObject(_param1, this.EtUBZQQZRa.QmSBNgiBCc);
        if (underlyingType == typeof (byte))
          return Enum.ToObject(_param1, this.EtUBZQQZRa.FCHBfwKmRn);
        if (underlyingType == typeof (sbyte))
          return Enum.ToObject(_param1, this.EtUBZQQZRa.hA1BpvTi1k);
        if (underlyingType == typeof (long))
          return Enum.ToObject(_param1, this.EtUBZQQZRa.DoQBY2qDkD);
        if (underlyingType == typeof (ulong))
          return Enum.ToObject(_param1, this.EtUBZQQZRa.HSsB1KnPq6);
        return underlyingType == typeof (char) ? Enum.ToObject(_param1, (ushort) this.EtUBZQQZRa.qt6BqjXoyG) : Enum.ToObject(_param1, this.EtUBZQQZRa.DoQBY2qDkD);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj c8idQhNv3S()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.DpYddoq5nS() ? 0 : 1);
      }

      internal override bool V1kdEyl02V() => this.vUcduRRnlL();

      public OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj kHZBssPC3s()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) this.EtUBZQQZRa.hA1BpvTi1k, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj B0Yd4gvqjc()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) this.EtUBZQQZRa.hA1BpvTi1k, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj X75dWxBQwq()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) this.EtUBZQQZRa.FCHBfwKmRn, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj dCrdTAgdCS()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) this.EtUBZQQZRa.SF9BiX4ruC, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj C5idHWm1cv()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) this.EtUBZQQZRa.QmSBNgiBCc, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj YjldSFyrtV()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.EtUBZQQZRa.qt6BqjXoyG, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj d8IdbKZ3nT()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.EtUBZQQZRa.oPHBCnNDZA, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p j56dmjESoe()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p kv0d625INd()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.HSsB1KnPq6, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj OxXdtNlUQr() => this.B0Yd4gvqjc();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj H7PdJ5iAPU() => this.dCrdTAgdCS();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj RgCd2ry1ac() => this.YjldSFyrtV();

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p YEUdcnGvKX() => this.j56dmjESoe();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj m0QdhS1F5D() => this.X75dWxBQwq();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Gsjdeg8ja3() => this.C5idHWm1cv();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj R3wdfbZewF() => this.d8IdbKZ3nT();

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p wsddpbkEQb() => this.kv0d625INd();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jyudNqrNXJ()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((sbyte) this.EtUBZQQZRa.DoQBY2qDkD), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj LmFdi8gd0R()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((sbyte) this.EtUBZQQZRa.HSsB1KnPq6), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj AYcdCCnBuC()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((short) this.EtUBZQQZRa.DoQBY2qDkD), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj usfdqHavse()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((short) this.EtUBZQQZRa.HSsB1KnPq6), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj K2Gd1PW7Sr()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((int) this.EtUBZQQZRa.DoQBY2qDkD), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj fATdY7yOVp()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((int) this.EtUBZQQZRa.HSsB1KnPq6), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p mgbdkYqhCS()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p t8YdsLWtVo()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked ((long) this.EtUBZQQZRa.HSsB1KnPq6), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj IYDdrwuQUc()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((byte) this.EtUBZQQZRa.DoQBY2qDkD), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj PvEdZLuSne()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((byte) this.EtUBZQQZRa.HSsB1KnPq6), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Y08dPd87iv()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((ushort) this.EtUBZQQZRa.DoQBY2qDkD), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jW7dKWHyFW()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((ushort) this.EtUBZQQZRa.HSsB1KnPq6), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj g1bdVnKyeV()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((uint) this.EtUBZQQZRa.DoQBY2qDkD), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj NWXd8DtKgs()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((uint) this.EtUBZQQZRa.HSsB1KnPq6), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p c5AdgYKqRb()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked ((ulong) this.EtUBZQQZRa.DoQBY2qDkD), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p fcnd3S7gbG()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.HSsB1KnPq6, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ EAfdGAC0F3()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((float) this.EtUBZQQZRa.DoQBY2qDkD);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ rcedIif63y()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((double) this.EtUBZQQZRa.DoQBY2qDkD);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ oUfdwd7RBO()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((double) this.EtUBZQQZRa.HSsB1KnPq6);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 u7udyDaiuk()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.YEUdcnGvKX().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.RgCd2ry1ac().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 erPdzHD1s2()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.wsddpbkEQb().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.R3wdfbZewF().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 HhEuMQLBcy()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.mgbdkYqhCS().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.K2Gd1PW7Sr().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 T52uLk6FSX()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.c5AdgYKqRb().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.g1bdVnKyeV().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 fDcu5RVPtU()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.t8YdsLWtVo().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.fATdY7yOVp().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 VVAuxnv6BF()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) checked ((uint) this.EtUBZQQZRa.HSsB1KnPq6));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PPauaFcpRf()
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(-this.EtUBZQQZRa.DoQBY2qDkD);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX Add(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD + ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX rFou9powA0(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked (this.EtUBZQQZRa.DoQBY2qDkD + ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD));
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX TvpuFtHMx3(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked (this.EtUBZQQZRa.HSsB1KnPq6 + ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6));
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX d69uOOjWrX(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD - ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX XJsu7LDPbi(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked (this.EtUBZQQZRa.DoQBY2qDkD - ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD));
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX AKNuAZ11DR(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked (this.EtUBZQQZRa.HSsB1KnPq6 - ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6));
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PZYuDsqDg5(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD * ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX OJsungeQZd(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked (this.EtUBZQQZRa.DoQBY2qDkD * ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD));
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX CrluoqnI7g(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked (this.EtUBZQQZRa.HSsB1KnPq6 * ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6));
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX fdRuBgjrLs(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD / ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX YYHujsF82I(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.HSsB1KnPq6 / ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX HL6u0YI3Yb(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD % ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX S5MuleAUoL(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.HSsB1KnPq6 % ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX pY5uXSxpHK(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD & ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX qmOuvwnxTA(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD | ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX LJ3udQlIYO()
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(~this.EtUBZQQZRa.DoQBY2qDkD);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX N4muuK8Z5O(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD ^ ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX KGbuURteF6(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD << ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.qt6BqjXoyG);
        if (_param1.gyIuhELnxq())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD << ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX cSauRN3PjN(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD >> ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.qt6BqjXoyG);
        if (_param1.gyIuhELnxq())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.DoQBY2qDkD >> ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX xXAuQJbXWE(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.HSsB1KnPq6 >> ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.qt6BqjXoyG);
        if (_param1.gyIuhELnxq())
          return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(this.EtUBZQQZRa.HSsB1KnPq6 >> ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override string ToString()
      {
        return this.WAmBPMB0G0 == (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7 ? this.EtUBZQQZRa.DoQBY2qDkD.ToString() : this.EtUBZQQZRa.HSsB1KnPq6.ToString();
      }

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this;
      }

      internal override bool gKku4OTsTL() => true;

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.W6wvnQlCGV())
          return _param1.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        if (_param1.DWVuea1EMj())
          return _param1.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = _param1.gRCuEnOnhD();
        return lxulLsB1AbCdvkPgx.E4wvjiQ2aY() && this.EtUBZQQZRa.DoQBY2qDkD == ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) lxulLsB1AbCdvkPgx).EtUBZQQZRa.DoQBY2qDkD;
      }

      private static OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA ovaBrj9Y4u(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param0)
      {
        if (!(_param0 is OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA) && _param0.DWVuea1EMj())
          wqpLuhZiq5CncNsA = _param0.gRCuEnOnhD() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA;
        return wqpLuhZiq5CncNsA;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.W6wvnQlCGV())
          return false;
        if (_param1.DWVuea1EMj())
          return _param1.BeouTiljCp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = _param1.gRCuEnOnhD();
        return lxulLsB1AbCdvkPgx.E4wvjiQ2aY() && (long) this.EtUBZQQZRa.HSsB1KnPq6 != (long) ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) lxulLsB1AbCdvkPgx).EtUBZQQZRa.HSsB1KnPq6;
      }

      public override bool a5iuHuxxL9(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return this.EtUBZQQZRa.DoQBY2qDkD >= ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool AMUuSZ5Bny(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return this.EtUBZQQZRa.HSsB1KnPq6 >= ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool s3rubpelFd(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return this.EtUBZQQZRa.DoQBY2qDkD > ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool lwlumgaheq(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return this.EtUBZQQZRa.HSsB1KnPq6 > ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool oj9u6Fhwca(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return this.EtUBZQQZRa.DoQBY2qDkD <= ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool kZGutsdjSl(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return this.EtUBZQQZRa.HSsB1KnPq6 <= ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool soGuJAfcec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return this.EtUBZQQZRa.DoQBY2qDkD < ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.DoQBY2qDkD;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool UOZu2PVRec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.E4wvjiQ2aY())
          return this.EtUBZQQZRa.HSsB1KnPq6 < ((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) _param1).EtUBZQQZRa.HSsB1KnPq6;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }
    }

    private class CydMXSLujvvk8Y3dM14 : OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA
    {
      public OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA z0dj5rkN6f;
      public OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw a5Pjxy25SY;

      internal void L97BKE25N4(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.LkLvBB03rl())
        {
          this.z0dj5rkN6f = ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).z0dj5rkN6f;
          this.a5Pjxy25SY = ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).a5Pjxy25SY;
        }
        else
          this.nOQdl4ODOg(_param1);
      }

      internal override unsafe void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.LkLvBB03rl())
        {
          if (IntPtr.Size == 8)
            *(long*) (void*) new IntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.DoQBY2qDkD) = new IntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).z0dj5rkN6f).EtUBZQQZRa.DoQBY2qDkD).ToInt64();
          else
            *(int*) (void*) new IntPtr(((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) this.z0dj5rkN6f).d37Bh8uTDv.GLgBtRhyOg) = new IntPtr(((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param1).z0dj5rkN6f).d37Bh8uTDv.GLgBtRhyOg).ToInt32();
        }
        else
        {
          object obj = _param1.MNddRugcTR((Type) null);
          if (obj == null)
            return;
          IntPtr num = IntPtr.Size != 8 ? new IntPtr(((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) this.z0dj5rkN6f).d37Bh8uTDv.GLgBtRhyOg) : new IntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.DoQBY2qDkD);
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
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            *(short*) (void*) num = (short) (char) obj;
          }
        }
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.nOQdl4ODOg(_param1);
      }

      public CydMXSLujvvk8Y3dM14(IntPtr _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 3;
        if (IntPtr.Size == 8)
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(_param1.ToInt64());
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12;
        }
        else
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(_param1.ToInt32());
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12;
        }
      }

      public CydMXSLujvvk8Y3dM14(UIntPtr _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 3;
        if (IntPtr.Size == 8)
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(_param1.ToUInt64());
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12;
        }
        else
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(_param1.ToUInt32());
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12;
        }
      }

      public CydMXSLujvvk8Y3dM14()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 3;
        if (IntPtr.Size == 8)
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(0L);
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12;
        }
        else
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0);
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12;
        }
      }

      public override OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA q9qdvQao7g()
      {
        return (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14()
        {
          z0dj5rkN6f = this.z0dj5rkN6f.q9qdvQao7g(),
          a5Pjxy25SY = this.a5Pjxy25SY
        };
      }

      public CydMXSLujvvk8Y3dM14(long _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 3;
        if (IntPtr.Size == 8)
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(_param1);
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12;
        }
        else
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) _param1);
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12;
        }
      }

      public CydMXSLujvvk8Y3dM14(long _param1, OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 3;
        if (IntPtr.Size == 8)
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(_param1);
          this.a5Pjxy25SY = _param2;
        }
        else
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) _param1);
          this.a5Pjxy25SY = _param2;
        }
      }

      public CydMXSLujvvk8Y3dM14(ulong _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 4;
        if (IntPtr.Size == 8)
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(_param1);
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 13;
        }
        else
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) _param1);
          this.a5Pjxy25SY = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 13;
        }
      }

      public CydMXSLujvvk8Y3dM14(ulong _param1, OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 4;
        if (IntPtr.Size == 8)
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(_param1);
          this.a5Pjxy25SY = _param2;
        }
        else
        {
          this.z0dj5rkN6f = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) _param1);
          this.a5Pjxy25SY = _param2;
        }
      }

      public override bool DpYddoq5nS() => this.z0dj5rkN6f.DpYddoq5nS();

      public override bool vUcduRRnlL() => !this.DpYddoq5nS();

      internal override bool V1kdEyl02V() => this.vUcduRRnlL();

      internal override bool lqfucMK0hV() => true;

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX sqedUSL72O(
        OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param1)
      {
        switch (_param1)
        {
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.B0Yd4gvqjc();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.X75dWxBQwq();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.dCrdTAgdCS();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.C5idHWm1cv();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.YjldSFyrtV();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.d8IdbKZ3nT();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.j56dmjESoe();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.kv0d625INd();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.c8idQhNv3S();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 13:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this;
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 16:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.q9qdvQao7g();
          default:
            throw new Exception(((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 4).ToString());
        }
      }

      internal IntPtr NrdBVmtKL7()
      {
        return IntPtr.Size == 8 ? new IntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.DoQBY2qDkD) : new IntPtr(((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) this.z0dj5rkN6f).d37Bh8uTDv.GLgBtRhyOg);
      }

      internal override object MNddRugcTR(Type _param1)
      {
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (_param1 == typeof (IntPtr))
          return IntPtr.Size == 8 ? (object) new IntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.DoQBY2qDkD) : (object) new IntPtr(((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) this.z0dj5rkN6f).d37Bh8uTDv.GLgBtRhyOg);
        if (_param1 == typeof (UIntPtr))
          return IntPtr.Size == 8 ? (object) new UIntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.HSsB1KnPq6) : (object) new UIntPtr(((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) this.z0dj5rkN6f).d37Bh8uTDv.MlKB6i90Nn);
        if (!(_param1 == (Type) null) && !(_param1 == typeof (object)))
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (this.a5Pjxy25SY == (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12 ? (object) new IntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.DoQBY2qDkD) : (object) new UIntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.HSsB1KnPq6)) : (this.a5Pjxy25SY == (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12 ? (object) new IntPtr(((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.qt6BqjXoyG) : (object) new UIntPtr(((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) this.z0dj5rkN6f).d37Bh8uTDv.MlKB6i90Nn));
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj c8idQhNv3S()
      {
        return this.z0dj5rkN6f.c8idQhNv3S();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj B0Yd4gvqjc()
      {
        return this.z0dj5rkN6f.B0Yd4gvqjc();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj X75dWxBQwq()
      {
        return this.z0dj5rkN6f.X75dWxBQwq();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj dCrdTAgdCS()
      {
        return this.z0dj5rkN6f.dCrdTAgdCS();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj C5idHWm1cv()
      {
        return this.z0dj5rkN6f.C5idHWm1cv();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj YjldSFyrtV()
      {
        return this.z0dj5rkN6f.YjldSFyrtV();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj d8IdbKZ3nT()
      {
        return this.z0dj5rkN6f.d8IdbKZ3nT();
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p j56dmjESoe()
      {
        return this.z0dj5rkN6f.j56dmjESoe();
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p kv0d625INd()
      {
        return this.z0dj5rkN6f.kv0d625INd();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj OxXdtNlUQr() => this.B0Yd4gvqjc();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj H7PdJ5iAPU() => this.dCrdTAgdCS();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj RgCd2ry1ac() => this.YjldSFyrtV();

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p YEUdcnGvKX() => this.j56dmjESoe();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj m0QdhS1F5D() => this.X75dWxBQwq();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Gsjdeg8ja3() => this.C5idHWm1cv();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj R3wdfbZewF() => this.d8IdbKZ3nT();

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p wsddpbkEQb() => this.kv0d625INd();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jyudNqrNXJ()
      {
        return this.z0dj5rkN6f.jyudNqrNXJ();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj LmFdi8gd0R()
      {
        return this.z0dj5rkN6f.LmFdi8gd0R();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj AYcdCCnBuC()
      {
        return this.z0dj5rkN6f.AYcdCCnBuC();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj usfdqHavse()
      {
        return this.z0dj5rkN6f.usfdqHavse();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj K2Gd1PW7Sr()
      {
        return this.z0dj5rkN6f.K2Gd1PW7Sr();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj fATdY7yOVp()
      {
        return this.z0dj5rkN6f.fATdY7yOVp();
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p mgbdkYqhCS()
      {
        return this.z0dj5rkN6f.mgbdkYqhCS();
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p t8YdsLWtVo()
      {
        return this.z0dj5rkN6f.t8YdsLWtVo();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj IYDdrwuQUc()
      {
        return this.z0dj5rkN6f.IYDdrwuQUc();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj PvEdZLuSne()
      {
        return this.z0dj5rkN6f.PvEdZLuSne();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Y08dPd87iv()
      {
        return this.z0dj5rkN6f.Y08dPd87iv();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jW7dKWHyFW()
      {
        return this.z0dj5rkN6f.jW7dKWHyFW();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj g1bdVnKyeV()
      {
        return this.z0dj5rkN6f.g1bdVnKyeV();
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj NWXd8DtKgs()
      {
        return this.z0dj5rkN6f.NWXd8DtKgs();
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p c5AdgYKqRb()
      {
        return this.z0dj5rkN6f.c5AdgYKqRb();
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p fcnd3S7gbG()
      {
        return this.z0dj5rkN6f.fcnd3S7gbG();
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ EAfdGAC0F3()
      {
        return this.z0dj5rkN6f.EAfdGAC0F3();
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ rcedIif63y()
      {
        return this.z0dj5rkN6f.rcedIif63y();
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ oUfdwd7RBO()
      {
        return this.z0dj5rkN6f.oUfdwd7RBO();
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 u7udyDaiuk()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.YEUdcnGvKX().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.RgCd2ry1ac().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 erPdzHD1s2()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.wsddpbkEQb().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.R3wdfbZewF().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 HhEuMQLBcy()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.mgbdkYqhCS().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.K2Gd1PW7Sr().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 T52uLk6FSX()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.c5AdgYKqRb().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.g1bdVnKyeV().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 fDcu5RVPtU()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.t8YdsLWtVo().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.fATdY7yOVp().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 VVAuxnv6BF()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.fcnd3S7gbG().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.NWXd8DtKgs().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PPauaFcpRf()
      {
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(-((OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p) this.z0dj5rkN6f).EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) -((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) this.z0dj5rkN6f).d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX Add(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD + ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg + ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD + ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg + ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX rFou9powA0(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD + ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg + ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD + ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg + ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX TvpuFtHMx3(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 + (ulong) ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn + ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 + ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) checked (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn + ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX d69uOOjWrX(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD - ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg - ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD - ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg - ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX RvoB8BAV82(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD - this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg - this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD - this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg - this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX XJsu7LDPbi(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD - ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg - ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD - ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg - ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX diBBgFLiiU(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD - this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg - this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD - this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg - this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX AKNuAZ11DR(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 - (ulong) ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn - ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 - ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) checked (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn - ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX EMWB3yONoS(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked ((ulong) ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn - this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn - this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 - this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) checked (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn - this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PZYuDsqDg5(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD * ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg * ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD * ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg * ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX OJsungeQZd(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD * ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg * ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD * ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg * ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX CrluoqnI7g(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 * (ulong) ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) checked (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn * ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(checked (this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 * ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6)) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) checked (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn * ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX fdRuBgjrLs(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD / ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg / ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD / ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg / ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX A91BGcXunp(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD / this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg / this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD / this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg / this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX YYHujsF82I(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 / ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn / ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 / ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn / ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX C1lBIaw5rr(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 / this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn / this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 / this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn / this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX HL6u0YI3Yb(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD % ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg % ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD % ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg % ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX A4nBwDQQ4V(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD % this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg % this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD % this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg % this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX S5MuleAUoL(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 % ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn % ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 % ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn % ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX yIwByEsukh(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 % this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn % this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 % this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) (((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn % this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX pY5uXSxpHK(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD & ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg & ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD & ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg & ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX qmOuvwnxTA(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD | ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg | ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD | ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg | ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX LJ3udQlIYO()
      {
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(~this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) ~this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX N4muuK8Z5O(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD ^ ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg ^ ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD ^ ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg ^ ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX KGbuURteF6(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD << ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg << ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD << ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.qt6BqjXoyG) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg << ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX cSauRN3PjN(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD >> ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg >> ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD >> ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.qt6BqjXoyG) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg >> ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX xXAuQJbXWE(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 >> ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn >> ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg));
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 >> ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.qt6BqjXoyG) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn >> ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX qiFBz81Hu6(
        OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj _param1)
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (_param1.d37Bh8uTDv.MlKB6i90Nn >> this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX Cp3jMBVv14(
        OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj _param1)
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (_param1.d37Bh8uTDv.GLgBtRhyOg >> this.j56dmjESoe().EtUBZQQZRa.qt6BqjXoyG));
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX OKCjLo23lH(
        OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj _param1)
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) (_param1.d37Bh8uTDv.GLgBtRhyOg << this.j56dmjESoe().EtUBZQQZRa.qt6BqjXoyG));
      }

      public override string ToString() => this.z0dj5rkN6f.ToString();

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this;
      }

      internal override bool gKku4OTsTL() => true;

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.W6wvnQlCGV())
          return false;
        if (_param1.DWVuea1EMj())
          return _param1.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = _param1.gRCuEnOnhD();
        if (!lxulLsB1AbCdvkPgx.gKku4OTsTL())
          return false;
        if (lxulLsB1AbCdvkPgx.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD == ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg == ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        if (!lxulLsB1AbCdvkPgx.LkLvBB03rl())
          return false;
        int size = IntPtr.Size;
        return this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD == ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.W6wvnQlCGV())
          return false;
        if (_param1.DWVuea1EMj())
          return _param1.BeouTiljCp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = _param1.gRCuEnOnhD();
        if (!lxulLsB1AbCdvkPgx.gKku4OTsTL())
          return false;
        if (lxulLsB1AbCdvkPgx.glCvoAFEwW())
          return IntPtr.Size == 8 ? (long) this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 != (long) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : (int) this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn != (int) ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        if (!lxulLsB1AbCdvkPgx.LkLvBB03rl())
          return false;
        int size = IntPtr.Size;
        return (long) this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 != (long) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6;
      }

      public override bool a5iuHuxxL9(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD >= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg >= ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD >= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg >= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg;
      }

      public override bool AMUuSZ5Bny(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 >= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn >= ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 >= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn >= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn;
      }

      public override bool s3rubpelFd(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD > ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg > ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD > ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg > ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg;
      }

      public override bool lwlumgaheq(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 > ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn > ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 > ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn > ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn;
      }

      public override bool oj9u6Fhwca(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD <= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg <= ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD <= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg <= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg;
      }

      public override bool kZGutsdjSl(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 <= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn <= ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 <= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn <= ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn;
      }

      public override bool soGuJAfcec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD < ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg < ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.GLgBtRhyOg;
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD < ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD : this.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg < ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg;
      }

      public override bool UOZu2PVRec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 < ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn < ((OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) _param1).d37Bh8uTDv.MlKB6i90Nn;
        if (!_param1.LkLvBB03rl())
          throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
        return IntPtr.Size == 8 ? this.j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 < ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).j56dmjESoe().EtUBZQQZRa.HSsB1KnPq6 : this.YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn < ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).YjldSFyrtV().d37Bh8uTDv.MlKB6i90Nn;
      }
    }

    private abstract class pcfWqpLUHZiq5CNCNsA : OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX
    {
      public abstract bool DpYddoq5nS();

      public abstract bool vUcduRRnlL();

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX sqedUSL72O(
        OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj c8idQhNv3S();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj B0Yd4gvqjc();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj X75dWxBQwq();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj dCrdTAgdCS();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj C5idHWm1cv();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj YjldSFyrtV();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj d8IdbKZ3nT();

      public abstract OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p j56dmjESoe();

      public abstract OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p kv0d625INd();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj OxXdtNlUQr();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj H7PdJ5iAPU();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj RgCd2ry1ac();

      public abstract OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p YEUdcnGvKX();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj m0QdhS1F5D();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Gsjdeg8ja3();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj R3wdfbZewF();

      public abstract OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p wsddpbkEQb();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jyudNqrNXJ();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj LmFdi8gd0R();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj AYcdCCnBuC();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj usfdqHavse();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj K2Gd1PW7Sr();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj fATdY7yOVp();

      public abstract OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p mgbdkYqhCS();

      public abstract OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p t8YdsLWtVo();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj IYDdrwuQUc();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj PvEdZLuSne();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Y08dPd87iv();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jW7dKWHyFW();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj g1bdVnKyeV();

      public abstract OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj NWXd8DtKgs();

      public abstract OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p c5AdgYKqRb();

      public abstract OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p fcnd3S7gbG();

      public abstract OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ EAfdGAC0F3();

      public abstract OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ rcedIif63y();

      public abstract OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ oUfdwd7RBO();

      public abstract OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 u7udyDaiuk();

      public abstract OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 erPdzHD1s2();

      public abstract OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 HhEuMQLBcy();

      public abstract OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 T52uLk6FSX();

      public abstract OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 fDcu5RVPtU();

      public abstract OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 VVAuxnv6BF();

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PPauaFcpRf();

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX Add(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX rFou9powA0(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX TvpuFtHMx3(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX d69uOOjWrX(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX XJsu7LDPbi(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX AKNuAZ11DR(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PZYuDsqDg5(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX OJsungeQZd(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX CrluoqnI7g(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX fdRuBgjrLs(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX YYHujsF82I(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX HL6u0YI3Yb(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX S5MuleAUoL(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX pY5uXSxpHK(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX qmOuvwnxTA(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX LJ3udQlIYO();

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX N4muuK8Z5O(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA q9qdvQao7g();

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX KGbuURteF6(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX cSauRN3PjN(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX xXAuQJbXWE(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract bool a5iuHuxxL9(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract bool AMUuSZ5Bny(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract bool s3rubpelFd(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract bool lwlumgaheq(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract bool oj9u6Fhwca(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract bool kZGutsdjSl(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract bool soGuJAfcec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      public abstract bool UOZu2PVRec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      internal override bool gyIuhELnxq() => true;

      protected pcfWqpLUHZiq5CNCNsA()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private class j5UMoXLRwIcnpEjt1UJ : OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA
    {
      public double BUuja1QMZO;
      public OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw uFvj9I4fVf;

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.BUuja1QMZO = ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        this.uFvj9I4fVf = ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).uFvj9I4fVf;
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.nOQdl4ODOg(_param1);
      }

      public j5UMoXLRwIcnpEjt1UJ(double _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 5;
        this.uFvj9I4fVf = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 10;
        this.BUuja1QMZO = _param1;
      }

      public j5UMoXLRwIcnpEjt1UJ(OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = _param1.tdyvUPSCoA;
        this.uFvj9I4fVf = _param1.uFvj9I4fVf;
        this.BUuja1QMZO = _param1.BUuja1QMZO;
      }

      public override OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA q9qdvQao7g()
      {
        return (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this);
      }

      public j5UMoXLRwIcnpEjt1UJ(double _param1, OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 5;
        this.BUuja1QMZO = _param1;
        this.uFvj9I4fVf = _param2;
      }

      public j5UMoXLRwIcnpEjt1UJ(float _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 5;
        this.BUuja1QMZO = (double) _param1;
        this.uFvj9I4fVf = (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 9;
      }

      public j5UMoXLRwIcnpEjt1UJ(float _param1, OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 5;
        this.BUuja1QMZO = (double) _param1;
        this.uFvj9I4fVf = _param2;
      }

      public override bool DpYddoq5nS() => this.BUuja1QMZO == 0.0;

      public override bool vUcduRRnlL() => !this.DpYddoq5nS();

      public override string ToString() => this.BUuja1QMZO.ToString();

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX sqedUSL72O(
        OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param1)
      {
        switch (_param1)
        {
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.B0Yd4gvqjc();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.X75dWxBQwq();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.dCrdTAgdCS();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.C5idHWm1cv();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.YjldSFyrtV();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.d8IdbKZ3nT();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.j56dmjESoe();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.kv0d625INd();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 9:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.EAfdGAC0F3();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 10:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.rcedIif63y();
          case (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 11:
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.c8idQhNv3S();
          default:
            throw new Exception(((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 4).ToString());
        }
      }

      internal override object MNddRugcTR(Type _param1)
      {
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (_param1 == typeof (float))
          return (object) (float) this.BUuja1QMZO;
        if (_param1 == typeof (double))
          return (object) this.BUuja1QMZO;
        return (_param1 == (Type) null || _param1 == typeof (object)) && this.uFvj9I4fVf == (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 9 ? (object) (float) this.BUuja1QMZO : (object) this.BUuja1QMZO;
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj c8idQhNv3S()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(this.DpYddoq5nS() ? 1 : 0);
      }

      internal override bool V1kdEyl02V() => this.vUcduRRnlL();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj B0Yd4gvqjc()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (sbyte) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj X75dWxBQwq()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) (byte) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj dCrdTAgdCS()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (short) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj C5idHWm1cv()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) (ushort) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj YjldSFyrtV()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj d8IdbKZ3nT()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p j56dmjESoe()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((long) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p kv0d625INd()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((ulong) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj OxXdtNlUQr() => this.B0Yd4gvqjc();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj H7PdJ5iAPU() => this.dCrdTAgdCS();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj RgCd2ry1ac() => this.YjldSFyrtV();

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p YEUdcnGvKX() => this.j56dmjESoe();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj m0QdhS1F5D() => this.X75dWxBQwq();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Gsjdeg8ja3() => this.C5idHWm1cv();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj R3wdfbZewF() => this.d8IdbKZ3nT();

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p wsddpbkEQb() => this.kv0d625INd();

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jyudNqrNXJ()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((sbyte) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj LmFdi8gd0R()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((sbyte) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj AYcdCCnBuC()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((short) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj usfdqHavse()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((short) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj K2Gd1PW7Sr()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((int) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj fATdY7yOVp()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((int) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p mgbdkYqhCS()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked ((long) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p t8YdsLWtVo()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked ((long) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj IYDdrwuQUc()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((byte) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj PvEdZLuSne()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((byte) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj Y08dPd87iv()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((ushort) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj jW7dKWHyFW()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) checked ((ushort) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj g1bdVnKyeV()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((uint) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj NWXd8DtKgs()
      {
        return new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(checked ((uint) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p c5AdgYKqRb()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked ((ulong) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p fcnd3S7gbG()
      {
        return new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(checked ((ulong) this.BUuja1QMZO), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ EAfdGAC0F3()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((float) this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 9);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ rcedIif63y()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 10);
      }

      public override OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ oUfdwd7RBO()
      {
        return new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 u7udyDaiuk()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.YEUdcnGvKX().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.RgCd2ry1ac().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 erPdzHD1s2()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.wsddpbkEQb().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.R3wdfbZewF().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 HhEuMQLBcy()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.mgbdkYqhCS().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.K2Gd1PW7Sr().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 T52uLk6FSX()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.c5AdgYKqRb().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.g1bdVnKyeV().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 fDcu5RVPtU()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.t8YdsLWtVo().EtUBZQQZRa.DoQBY2qDkD) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) this.fATdY7yOVp().d37Bh8uTDv.GLgBtRhyOg);
      }

      public override OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14 VVAuxnv6BF()
      {
        return IntPtr.Size == 8 ? new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(this.fcnd3S7gbG().EtUBZQQZRa.HSsB1KnPq6) : new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((ulong) this.NWXd8DtKgs().d37Bh8uTDv.MlKB6i90Nn);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PPauaFcpRf()
      {
        return this.uFvj9I4fVf == (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 9 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((float) -this.BUuja1QMZO) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(-this.BUuja1QMZO);
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX Add(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO + ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX rFou9powA0(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO + ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX TvpuFtHMx3(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO + ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX d69uOOjWrX(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO - ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX XJsu7LDPbi(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO - ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX AKNuAZ11DR(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO - ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX PZYuDsqDg5(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() && _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO * ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX OJsungeQZd(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO * ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX CrluoqnI7g(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO * ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX fdRuBgjrLs(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO / ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX YYHujsF82I(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO / ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX HL6u0YI3Yb(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO % ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX S5MuleAUoL(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        return _param1.YVbv0y1oss() ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(this.BUuja1QMZO % ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX pY5uXSxpHK(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX qmOuvwnxTA(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX LJ3udQlIYO()
      {
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX N4muuK8Z5O(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX KGbuURteF6(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX cSauRN3PjN(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX xXAuQJbXWE(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this;
      }

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.W6wvnQlCGV())
          return false;
        if (_param1.DWVuea1EMj())
          return _param1.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = _param1.gRCuEnOnhD();
        return lxulLsB1AbCdvkPgx.YVbv0y1oss() && this.BUuja1QMZO == ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) lxulLsB1AbCdvkPgx).BUuja1QMZO;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.W6wvnQlCGV())
          return false;
        if (_param1.DWVuea1EMj())
          return _param1.BeouTiljCp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this);
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = _param1.gRCuEnOnhD();
        return lxulLsB1AbCdvkPgx.YVbv0y1oss() && this.BUuja1QMZO != ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) lxulLsB1AbCdvkPgx).BUuja1QMZO;
      }

      public override bool a5iuHuxxL9(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.YVbv0y1oss())
          return this.BUuja1QMZO >= ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool AMUuSZ5Bny(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.YVbv0y1oss())
          return this.BUuja1QMZO >= ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool s3rubpelFd(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.YVbv0y1oss())
          return this.BUuja1QMZO > ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool lwlumgaheq(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.YVbv0y1oss())
          return this.BUuja1QMZO > ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool oj9u6Fhwca(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.YVbv0y1oss())
          return this.BUuja1QMZO <= ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool kZGutsdjSl(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.YVbv0y1oss())
          return this.BUuja1QMZO <= ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool soGuJAfcec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.YVbv0y1oss())
          return this.BUuja1QMZO < ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      public override bool UOZu2PVRec(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.YVbv0y1oss())
          return this.BUuja1QMZO < ((OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ) _param1).BUuja1QMZO;
        throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }
    }

    internal enum iQ4HZQLQyLCdABwFXsw : byte
    {
    }

    internal enum MIegr9LEMaB3mn2S8kK : byte
    {
    }

    private class eOTFqXL4MCe1DWExSFT : Exception
    {
      public eOTFqXL4MCe1DWExSFT(string _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector(_param1);
      }
    }

    private class jB7Io5LWnK4vE2iB95i : Exception
    {
      public jB7Io5LWnK4vE2iB95i()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      public jB7Io5LWnK4vE2iB95i(string _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector(_param1);
      }
    }

    internal class K14OTLLT4lAAZwtUFKB
    {
      internal OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg wk1jFwCKSW;
      internal object PTyjO0yydc;

      public override string ToString()
      {
        object wk1jFwCksw = (object) this.wk1jFwCKSW;
        return this.PTyjO0yydc != null ? wk1jFwCksw.ToString() + 'H'.ToString() + this.PTyjO0yydc.ToString() : wk1jFwCksw.ToString();
      }

      public K14OTLLT4lAAZwtUFKB()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.wk1jFwCKSW = (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 126;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal abstract class xlFl6HLHf4Wx0BoEfgp : OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX
    {
      public xlFl6HLHf4Wx0BoEfgp()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      internal override bool DWVuea1EMj() => true;

      internal abstract IntPtr BThufJwh7w();

      internal abstract void Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      internal override bool lqfucMK0hV() => true;
    }

    internal class C50CprLSCELDpjV9aKU : OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp
    {
      private OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52 DSnj7hEZ7a;
      internal int RL9jAF4lBE;

      public C50CprLSCELDpjV9aKU(int _param1, OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52 _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.DSnj7hEZ7a = _param2;
        this.RL9jAF4lBE = _param1;
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 7;
      }

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1 is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
        {
          this.DSnj7hEZ7a = ((OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU) _param1).DSnj7hEZ7a;
          this.RL9jAF4lBE = ((OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU) _param1).RL9jAF4lBE;
        }
        else
        {
          OBqe2IUAeSpOmlOQ4O.V6kKaOL2XvatnIN8I0Z kaOl2XvatnIn8I0Z = this.DSnj7hEZ7a.eV7XiE3C4H.fBcjhfhonR[this.RL9jAF4lBE];
          if (_param1 is OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp && (kaOl2XvatnIn8I0Z.WhxjRcXFCB & (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 226) > (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 0)
            this.Eqfup3BbFp((_param1 as OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp).gRCuEnOnhD());
          else
            this.Eqfup3BbFp(_param1);
        }
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.Eqfup3BbFp(_param1);
      }

      internal override IntPtr BThufJwh7w() => throw new NotImplementedException();

      internal override void Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.DSnj7hEZ7a.Ti3XqqGbnS[this.RL9jAF4lBE] = _param1;
      }

      internal override object MNddRugcTR(Type _param1)
      {
        return this.DSnj7hEZ7a.Ti3XqqGbnS[this.RL9jAF4lBE] == null ? (object) null : this.gRCuEnOnhD().MNddRugcTR(_param1);
      }

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return this.DSnj7hEZ7a.Ti3XqqGbnS[this.RL9jAF4lBE] == null ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null) : this.DSnj7hEZ7a.Ti3XqqGbnS[this.RL9jAF4lBE].gRCuEnOnhD();
      }

      internal override bool gKku4OTsTL() => this.gRCuEnOnhD().gKku4OTsTL();

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        return _param1.DWVuea1EMj() && _param1 is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU && ((OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU) _param1).RL9jAF4lBE == this.RL9jAF4lBE;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        return !_param1.DWVuea1EMj() || !(_param1 is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU) || ((OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU) _param1).RL9jAF4lBE != this.RL9jAF4lBE;
      }

      internal override bool V1kdEyl02V() => this.gRCuEnOnhD().V1kdEyl02V();
    }

    internal class daEbHELbQht9GYRuEJp : OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp
    {
      private Array QA1jDkQJ0I;
      internal int YeejnUEEvx;

      public daEbHELbQht9GYRuEJp(int _param1, Array _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.QA1jDkQJ0I = _param2;
        this.YeejnUEEvx = _param1;
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 7;
      }

      internal override IntPtr BThufJwh7w() => throw new NotImplementedException();

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1 is OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp)
        {
          this.QA1jDkQJ0I = ((OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp) _param1).QA1jDkQJ0I;
          this.YeejnUEEvx = ((OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp) _param1).YeejnUEEvx;
        }
        else
          this.Eqfup3BbFp(_param1);
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.Eqfup3BbFp(_param1);
      }

      internal override void Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.QA1jDkQJ0I.SetValue(_param1.MNddRugcTR((Type) null), this.YeejnUEEvx);
      }

      internal override object MNddRugcTR(Type _param1) => this.gRCuEnOnhD().MNddRugcTR(_param1);

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(this.QA1jDkQJ0I.GetType().GetElementType(), this.QA1jDkQJ0I.GetValue(this.YeejnUEEvx));
      }

      internal override bool gKku4OTsTL() => this.gRCuEnOnhD().gKku4OTsTL();

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (!_param1.DWVuea1EMj() || !(_param1 is OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp))
          return false;
        OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp ebHeLbQht9GyRuEjp = (OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp) _param1;
        return ebHeLbQht9GyRuEjp.YeejnUEEvx == this.YeejnUEEvx && ebHeLbQht9GyRuEjp.QA1jDkQJ0I == this.QA1jDkQJ0I;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (!_param1.DWVuea1EMj() || !(_param1 is OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp))
          return true;
        OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp ebHeLbQht9GyRuEjp = (OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp) _param1;
        return ebHeLbQht9GyRuEjp.YeejnUEEvx != this.YeejnUEEvx || ebHeLbQht9GyRuEjp.QA1jDkQJ0I != this.QA1jDkQJ0I;
      }

      internal override bool V1kdEyl02V() => this.gRCuEnOnhD().V1kdEyl02V();
    }

    internal class dcnLnOLmJmDBGRqeOE8 : OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp
    {
      internal FieldInfo A39joYqGSV;
      internal object nGEjBfyeTD;

      public dcnLnOLmJmDBGRqeOE8(FieldInfo _param1, object _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.A39joYqGSV = _param1;
        this.nGEjBfyeTD = _param2;
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 7;
      }

      internal override IntPtr BThufJwh7w() => throw new NotImplementedException();

      internal override void Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (this.nGEjBfyeTD != null && this.nGEjBfyeTD is OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX)
          this.A39joYqGSV.SetValue(((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.nGEjBfyeTD).MNddRugcTR((Type) null), _param1.MNddRugcTR((Type) null));
        else
          this.A39joYqGSV.SetValue(this.nGEjBfyeTD, _param1.MNddRugcTR((Type) null));
      }

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1 is OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8)
        {
          this.A39joYqGSV = ((OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8) _param1).A39joYqGSV;
          this.nGEjBfyeTD = ((OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8) _param1).nGEjBfyeTD;
        }
        else
          this.Eqfup3BbFp(_param1);
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.Eqfup3BbFp(_param1);
      }

      internal override object MNddRugcTR(Type _param1) => this.gRCuEnOnhD().MNddRugcTR(_param1);

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return this.nGEjBfyeTD != null && this.nGEjBfyeTD is OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX ? OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(this.A39joYqGSV.FieldType, this.A39joYqGSV.GetValue(((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.nGEjBfyeTD).MNddRugcTR((Type) null))) : OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(this.A39joYqGSV.FieldType, this.A39joYqGSV.GetValue(this.nGEjBfyeTD));
      }

      internal override bool gKku4OTsTL() => this.gRCuEnOnhD().gKku4OTsTL();

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (!_param1.DWVuea1EMj() || !(_param1 is OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8))
          return false;
        OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8 lnOlmJmDbgRqeOe8 = (OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8) _param1;
        return !(lnOlmJmDbgRqeOe8.A39joYqGSV != this.A39joYqGSV) && lnOlmJmDbgRqeOe8.nGEjBfyeTD == this.nGEjBfyeTD;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (!_param1.DWVuea1EMj() || !(_param1 is OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8))
          return true;
        OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8 lnOlmJmDbgRqeOe8 = (OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8) _param1;
        return lnOlmJmDbgRqeOe8.A39joYqGSV != this.A39joYqGSV || lnOlmJmDbgRqeOe8.nGEjBfyeTD != this.nGEjBfyeTD;
      }

      internal override bool V1kdEyl02V() => this.gRCuEnOnhD().V1kdEyl02V();
    }

    internal class f6yUdEL63YbYB5aCQOA : OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp
    {
      private OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52 EqvjjieJlV;
      internal int GVpj08wEwV;

      public f6yUdEL63YbYB5aCQOA(int _param1, OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52 _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.EqvjjieJlV = _param2;
        this.GVpj08wEwV = _param1;
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 7;
      }

      internal override IntPtr BThufJwh7w() => throw new NotImplementedException();

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1 is OBqe2IUAeSpOmlOQ4O.f6yUdEL63YbYB5aCQOA)
        {
          this.EqvjjieJlV = ((OBqe2IUAeSpOmlOQ4O.f6yUdEL63YbYB5aCQOA) _param1).EqvjjieJlV;
          this.GVpj08wEwV = ((OBqe2IUAeSpOmlOQ4O.f6yUdEL63YbYB5aCQOA) _param1).GVpj08wEwV;
        }
        else
          this.Eqfup3BbFp(_param1);
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.Eqfup3BbFp(_param1);
      }

      internal override void Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.EqvjjieJlV.GL6XCXXAY9[this.GVpj08wEwV] = _param1;
      }

      internal override object MNddRugcTR(Type _param1)
      {
        return this.EqvjjieJlV.GL6XCXXAY9[this.GVpj08wEwV] == null ? (object) null : this.gRCuEnOnhD().MNddRugcTR(_param1);
      }

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return this.EqvjjieJlV.GL6XCXXAY9[this.GVpj08wEwV] == null ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null) : this.EqvjjieJlV.GL6XCXXAY9[this.GVpj08wEwV].gRCuEnOnhD();
      }

      internal override bool gKku4OTsTL() => this.gRCuEnOnhD().gKku4OTsTL();

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        return _param1.DWVuea1EMj() && _param1 is OBqe2IUAeSpOmlOQ4O.f6yUdEL63YbYB5aCQOA && ((OBqe2IUAeSpOmlOQ4O.f6yUdEL63YbYB5aCQOA) _param1).GVpj08wEwV == this.GVpj08wEwV;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        return !_param1.DWVuea1EMj() || !(_param1 is OBqe2IUAeSpOmlOQ4O.f6yUdEL63YbYB5aCQOA) || ((OBqe2IUAeSpOmlOQ4O.f6yUdEL63YbYB5aCQOA) _param1).GVpj08wEwV != this.GVpj08wEwV;
      }

      internal override bool V1kdEyl02V() => this.gRCuEnOnhD().V1kdEyl02V();
    }

    internal class R3Oma8LtRwKkrYXOrgs : OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp
    {
      private OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX kTXjlPLshT;
      private Type tPBjXYjAJE;

      public R3Oma8LtRwKkrYXOrgs(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1, Type _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.kTXjlPLshT = _param1;
        this.tPBjXYjAJE = _param2;
        this.tdyvUPSCoA = (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 7;
      }

      internal override IntPtr BThufJwh7w() => throw new NotImplementedException();

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1 is OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs)
        {
          this.tPBjXYjAJE = ((OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs) _param1).tPBjXYjAJE;
          this.kTXjlPLshT = ((OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs) _param1).kTXjlPLshT;
        }
        else
          this.kTXjlPLshT.nOQdl4ODOg(_param1);
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.Eqfup3BbFp(_param1);
      }

      internal override void Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.kTXjlPLshT = _param1;
      }

      internal override object MNddRugcTR(Type _param1)
      {
        if (this.kTXjlPLshT == null)
          return (object) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null);
        return _param1 == (Type) null || _param1 == typeof (object) ? this.kTXjlPLshT.MNddRugcTR(this.tPBjXYjAJE) : this.kTXjlPLshT.MNddRugcTR(_param1);
      }

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return this.kTXjlPLshT == null ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null) : this.kTXjlPLshT.gRCuEnOnhD();
      }

      internal override bool gKku4OTsTL() => this.gRCuEnOnhD().gKku4OTsTL();

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (!_param1.DWVuea1EMj() || !(_param1 is OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs))
          return false;
        OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs oma8LtRwKkrYxOrgs = (OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs) _param1;
        if (oma8LtRwKkrYxOrgs.tPBjXYjAJE != this.tPBjXYjAJE)
          return false;
        if (this.kTXjlPLshT != null)
          return this.kTXjlPLshT.SHouWnYaPf(oma8LtRwKkrYxOrgs.kTXjlPLshT);
        return oma8LtRwKkrYxOrgs.kTXjlPLshT == null;
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (!_param1.DWVuea1EMj() || !(_param1 is OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs))
          return true;
        OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs oma8LtRwKkrYxOrgs = (OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs) _param1;
        if (oma8LtRwKkrYxOrgs.tPBjXYjAJE != this.tPBjXYjAJE)
          return true;
        if (this.kTXjlPLshT != null)
          return this.kTXjlPLshT.BeouTiljCp(oma8LtRwKkrYxOrgs.kTXjlPLshT);
        return oma8LtRwKkrYxOrgs.kTXjlPLshT != null;
      }

      internal override bool V1kdEyl02V() => this.gRCuEnOnhD().V1kdEyl02V();
    }

    internal class j9Ha11LJoma4Y5DYntf
    {
      public int UDsjvgp1ei;
      public bool UhNjdqxgic;
      public OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw RcQjuILAQC;

      public j9Ha11LJoma4Y5DYntf()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class V6kKaOL2XvatnIN8I0Z
    {
      public int XH6jUerVZS;
      public OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw WhxjRcXFCB;
      public bool r4GjQb7Vvg;
      public Type N3YjEEsk0h;

      public V6kKaOL2XvatnIN8I0Z()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.N3YjEEsk0h = typeof (object);
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class HrpI4gLcuIdaB2erwhp
    {
      public int KBbj4IwY3n;
      public int CrWjWZF3Io;
      public OBqe2IUAeSpOmlOQ4O.bUeJteLhNL76rUQL5ZO XFAjTPCIM3;

      public HrpI4gLcuIdaB2erwhp()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class bUeJteLhNL76rUQL5ZO
    {
      public int F9CjHFtGZY;
      public int FTijSNsYde;
      public byte LIFjbWNxpq;
      public Type nWdjmd2WlN;
      public int Dpmj6iSp4C;
      public int y6mjtIyqq4;

      public bUeJteLhNL76rUQL5ZO()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    internal class JWGbihLe59w0Tjpgs7W
    {
      internal MethodBase qpZjJxQ46V;
      internal List<OBqe2IUAeSpOmlOQ4O.K14OTLLT4lAAZwtUFKB> J96j2oqJsf;
      internal OBqe2IUAeSpOmlOQ4O.j9Ha11LJoma4Y5DYntf[] JDJjcB4nud;
      internal List<OBqe2IUAeSpOmlOQ4O.V6kKaOL2XvatnIN8I0Z> fBcjhfhonR;
      internal List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp> C4ujeQYOhV;

      public JWGbihLe59w0Tjpgs7W()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private class yDoeNELfV5aXCK5H1Ar
    {
      internal FieldInfo Hpjjf13Ho9;
      internal int ovMjpUjjfa;

      public yDoeNELfV5aXCK5H1Ar(FieldInfo _param1, int _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Hpjjf13Ho9 = _param1;
        this.ovMjpUjjfa = _param2;
      }
    }

    private class Dhe3JWLp0DcXGE8RZ6a
    {
      private List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar> MCsjCH4M8g;
      private MethodBase f1xjqAe01x;

      public Dhe3JWLp0DcXGE8RZ6a(
        MethodBase _param1,
        List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar> _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.MCsjCH4M8g = new List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.MCsjCH4M8g = _param2;
        this.f1xjqAe01x = _param1;
      }

      public Dhe3JWLp0DcXGE8RZ6a(
        MethodBase _param1,
        OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar[] _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.MCsjCH4M8g = new List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.MCsjCH4M8g.AddRange((IEnumerable<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar>) _param2);
      }

      public override bool Equals(object _param1)
      {
        OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a dhe3JwLp0DcXgE8Rz6a = _param1 as OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a;
        if (_param1 == null || this.f1xjqAe01x != dhe3JwLp0DcXgE8Rz6a.f1xjqAe01x || this.MCsjCH4M8g.Count != dhe3JwLp0DcXgE8Rz6a.MCsjCH4M8g.Count)
          return false;
        for (int index = 0; index < this.MCsjCH4M8g.Count; ++index)
        {
          if (this.MCsjCH4M8g[index].Hpjjf13Ho9 != dhe3JwLp0DcXgE8Rz6a.MCsjCH4M8g[index].Hpjjf13Ho9 || this.MCsjCH4M8g[index].ovMjpUjjfa != dhe3JwLp0DcXgE8Rz6a.MCsjCH4M8g[index].ovMjpUjjfa)
            return false;
        }
        return true;
      }

      public override int GetHashCode()
      {
        int hashCode = this.f1xjqAe01x.GetHashCode();
        foreach (OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar doeNeLfV5aXcK5H1Ar in this.MCsjCH4M8g)
        {
          int num = doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.GetHashCode() + doeNeLfV5aXcK5H1Ar.ovMjpUjjfa;
          hashCode = (hashCode ^ num) + num;
        }
        return hashCode;
      }

      public OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar uJ9jNuhOde(int _param1)
      {
        foreach (OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar doeNeLfV5aXcK5H1Ar in this.MCsjCH4M8g)
        {
          if (doeNeLfV5aXcK5H1Ar.ovMjpUjjfa == _param1)
            return doeNeLfV5aXcK5H1Ar;
        }
        return (OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar) null;
      }

      public bool LaJjisNd9u(int _param1)
      {
        foreach (OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar doeNeLfV5aXcK5H1Ar in this.MCsjCH4M8g)
        {
          if (doeNeLfV5aXcK5H1Ar.ovMjpUjjfa == _param1)
            return true;
        }
        return false;
      }
    }

    private delegate object c9qBopLNoZTiN7k24Gr(object target, object[] paramters);

    private delegate object bGs71YLiIW9il9yDL4a(object target);

    private delegate void CyiAdZLCpvyZKhkMmGc(IntPtr a, byte b, int c);

    private delegate void g2JyDeLqftkG6b66Zf8(IntPtr s, IntPtr t, uint c);

    internal class X6oonhL1IpoG9H0Sh52
    {
      internal OBqe2IUAeSpOmlOQ4O.JWGbihLe59w0Tjpgs7W eV7XiE3C4H;
      internal OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[] GL6XCXXAY9;
      internal OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[] Ti3XqqGbnS;
      internal OBqe2IUAeSpOmlOQ4O.g6G7F6LPsY0A2sA8jx9 xvIX12K5P4;
      internal OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX Gl4XYW9Iw1;
      internal Exception GT5XkyXtsY;
      internal List<IntPtr> vBXXs2g5N5;
      private int kO9XrsjAAr;
      private int LHAXZHD3u1;
      private int mJvXPuZWLG;
      private object P6nXKq05L2;
      private bool d6xXV9O6wf;
      private bool WUtX89BpW0;
      private bool wIZXgFO6U7;
      private bool WT9X3IVJtU;
      private static Dictionary<Type, int> us3XG3bSNL;
      private static object hL4XIw57CM;
      private static Dictionary<object, OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX> xRkXwNANCW;
      private static object tnBXycvSYh;
      private static Dictionary<MethodBase, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr> UBIXzg2TS3;
      private static Dictionary<MethodBase, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr> kQfvM9jv6W;
      private static object yeHvLKgEPe;
      private static Dictionary<OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr> schv5EvJl0;
      private static Dictionary<OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr> Rfevxivn5c;
      private static object MTHvairTKI;
      private static Dictionary<OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr> Qe7v9AGJpX;
      private static object OEuvFnCu1u;
      private static Dictionary<Type, OBqe2IUAeSpOmlOQ4O.bGs71YLiIW9il9yDL4a> np5vOHNAn0;
      private static object xRgv7G3cbx;
      private static OBqe2IUAeSpOmlOQ4O.CyiAdZLCpvyZKhkMmGc vQovAna5lx;
      private static OBqe2IUAeSpOmlOQ4O.g2JyDeLqftkG6b66Zf8 hEbvDetTSD;

      internal void ygDj1mPbak()
      {
        bool flag = false;
        this.tbKjk9l4qb(ref flag);
      }

      internal void liejY9ZWI8()
      {
        this.xvIX12K5P4.OyDv4akGg5();
        this.Ti3XqqGbnS = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[]) null;
        if (this.vBXXs2g5N5 == null)
          return;
        foreach (IntPtr hglobal in this.vBXXs2g5N5)
        {
          try
          {
            Marshal.FreeHGlobal(hglobal);
          }
          catch
          {
          }
        }
        this.vBXXs2g5N5.Clear();
        this.vBXXs2g5N5 = (List<IntPtr>) null;
      }

      internal void tbKjk9l4qb(ref bool _param1)
      {
        for (; this.kO9XrsjAAr > -2; ++this.kO9XrsjAAr)
        {
          if (this.d6xXV9O6wf)
          {
            this.d6xXV9O6wf = false;
            int lhaxzhD3u1 = this.LHAXZHD3u1;
            int kO9XrsjAar = this.kO9XrsjAAr;
            this.rEbjr309ae(this.LHAXZHD3u1, this.kO9XrsjAAr);
            this.kO9XrsjAAr = kO9XrsjAar;
            this.LHAXZHD3u1 = lhaxzhD3u1;
          }
          if (this.wIZXgFO6U7)
          {
            this.wIZXgFO6U7 = false;
            return;
          }
          if (this.WUtX89BpW0)
          {
            this.WUtX89BpW0 = false;
            return;
          }
          this.LHAXZHD3u1 = this.kO9XrsjAAr;
          OBqe2IUAeSpOmlOQ4O.K14OTLLT4lAAZwtUFKB otllT4lAaZwtUfkb = this.eV7XiE3C4H.J96j2oqJsf[this.kO9XrsjAAr];
          this.P6nXKq05L2 = otllT4lAaZwtUfkb.PTyjO0yydc;
          try
          {
            this.bbijKtxMDJ(otllT4lAaZwtUfkb);
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
            this.GT5XkyXtsY = exception;
            _param1 = true;
            this.xvIX12K5P4.OyDv4akGg5();
            int lhaxzhD3u1 = this.LHAXZHD3u1;
            OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp1 = this.gBsjZMnfbw(lhaxzhD3u1, exception);
            List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp> collection = this.LedjPYTVOC(lhaxzhD3u1, false);
            List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp> i4gLcuIdaB2erwhpList = new List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp>();
            if (i4gLcuIdaB2erwhp1 != null)
              i4gLcuIdaB2erwhpList.Add(i4gLcuIdaB2erwhp1);
            if (collection != null && collection.Count > 0)
              i4gLcuIdaB2erwhpList.AddRange((IEnumerable<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp>) collection);
            i4gLcuIdaB2erwhpList.Sort((Comparison<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp>) ((x, y) => x.XFAjTPCIM3.F9CjHFtGZY.CompareTo(y.XFAjTPCIM3.F9CjHFtGZY)));
            OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp2 = (OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp) null;
            foreach (OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp3 in i4gLcuIdaB2erwhpList)
            {
              if (i4gLcuIdaB2erwhp3.XFAjTPCIM3.y6mjtIyqq4 == 0)
              {
                i4gLcuIdaB2erwhp2 = i4gLcuIdaB2erwhp3;
                break;
              }
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) exception));
              this.LHAXZHD3u1 = i4gLcuIdaB2erwhp3.XFAjTPCIM3.Dpmj6iSp4C;
              this.kO9XrsjAAr = this.LHAXZHD3u1;
              this.ygDj1mPbak();
              if (this.WT9X3IVJtU)
              {
                this.WT9X3IVJtU = false;
                i4gLcuIdaB2erwhp2 = i4gLcuIdaB2erwhp3;
                break;
              }
            }
            if (i4gLcuIdaB2erwhp2 == null)
              throw exception;
            this.mJvXPuZWLG = i4gLcuIdaB2erwhp2.XFAjTPCIM3.F9CjHFtGZY;
            this.chDjs4NQul(lhaxzhD3u1, i4gLcuIdaB2erwhp2.XFAjTPCIM3.F9CjHFtGZY);
            if (this.mJvXPuZWLG < 0)
              return;
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) exception));
            this.LHAXZHD3u1 = this.mJvXPuZWLG;
            this.kO9XrsjAAr = this.LHAXZHD3u1;
            this.mJvXPuZWLG = -1;
            this.ygDj1mPbak();
            return;
          }
        }
        this.xvIX12K5P4.OyDv4akGg5();
      }

      internal void chDjs4NQul(int _param1, int _param2)
      {
        if (this.eV7XiE3C4H.C4ujeQYOhV == null)
          return;
        foreach (OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp in this.eV7XiE3C4H.C4ujeQYOhV)
        {
          if ((i4gLcuIdaB2erwhp.XFAjTPCIM3.y6mjtIyqq4 == 4 || i4gLcuIdaB2erwhp.XFAjTPCIM3.y6mjtIyqq4 == 2) && i4gLcuIdaB2erwhp.XFAjTPCIM3.F9CjHFtGZY >= _param1 && i4gLcuIdaB2erwhp.XFAjTPCIM3.FTijSNsYde <= _param2)
          {
            this.LHAXZHD3u1 = i4gLcuIdaB2erwhp.XFAjTPCIM3.F9CjHFtGZY;
            this.kO9XrsjAAr = this.LHAXZHD3u1;
            bool flag = false;
            this.tbKjk9l4qb(ref flag);
            if (flag)
              break;
          }
        }
      }

      internal void rEbjr309ae(int _param1, int _param2)
      {
        if (this.eV7XiE3C4H.C4ujeQYOhV == null)
          return;
        foreach (OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp in this.eV7XiE3C4H.C4ujeQYOhV)
        {
          if (i4gLcuIdaB2erwhp.XFAjTPCIM3.y6mjtIyqq4 == 2 && i4gLcuIdaB2erwhp.XFAjTPCIM3.F9CjHFtGZY >= _param1 && i4gLcuIdaB2erwhp.XFAjTPCIM3.FTijSNsYde <= _param2)
          {
            this.LHAXZHD3u1 = i4gLcuIdaB2erwhp.XFAjTPCIM3.F9CjHFtGZY;
            this.kO9XrsjAAr = this.LHAXZHD3u1;
            bool flag = false;
            this.tbKjk9l4qb(ref flag);
            if (flag)
              break;
          }
        }
      }

      internal OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp gBsjZMnfbw(int _param1, Exception _param2)
      {
        OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp1 = (OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp) null;
        if (this.eV7XiE3C4H.C4ujeQYOhV != null)
        {
          foreach (OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp2 in this.eV7XiE3C4H.C4ujeQYOhV)
          {
            if (i4gLcuIdaB2erwhp2.XFAjTPCIM3 != null && i4gLcuIdaB2erwhp2.XFAjTPCIM3.y6mjtIyqq4 == 0 && (i4gLcuIdaB2erwhp2.XFAjTPCIM3.nWdjmd2WlN == _param2.GetType() || i4gLcuIdaB2erwhp2.XFAjTPCIM3.nWdjmd2WlN != (Type) null && (i4gLcuIdaB2erwhp2.XFAjTPCIM3.nWdjmd2WlN.FullName == _param2.GetType().FullName || i4gLcuIdaB2erwhp2.XFAjTPCIM3.nWdjmd2WlN.FullName == typeof (object).FullName || i4gLcuIdaB2erwhp2.XFAjTPCIM3.nWdjmd2WlN.FullName == typeof (Exception).FullName)) && _param1 >= i4gLcuIdaB2erwhp2.KBbj4IwY3n && _param1 <= i4gLcuIdaB2erwhp2.CrWjWZF3Io)
            {
              if (i4gLcuIdaB2erwhp1 == null)
                i4gLcuIdaB2erwhp1 = i4gLcuIdaB2erwhp2;
              else if (i4gLcuIdaB2erwhp2.XFAjTPCIM3.F9CjHFtGZY < i4gLcuIdaB2erwhp1.XFAjTPCIM3.F9CjHFtGZY)
                i4gLcuIdaB2erwhp1 = i4gLcuIdaB2erwhp2;
            }
          }
        }
        return i4gLcuIdaB2erwhp1;
      }

      internal List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp> LedjPYTVOC(int _param1, bool _param2)
      {
        if (this.eV7XiE3C4H.C4ujeQYOhV == null)
          return (List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp>) null;
        List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp> i4gLcuIdaB2erwhpList = new List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp>();
        foreach (OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp i4gLcuIdaB2erwhp in this.eV7XiE3C4H.C4ujeQYOhV)
        {
          if ((i4gLcuIdaB2erwhp.XFAjTPCIM3.y6mjtIyqq4 & 1) == 1 && _param1 >= i4gLcuIdaB2erwhp.KBbj4IwY3n && _param1 <= i4gLcuIdaB2erwhp.CrWjWZF3Io)
            i4gLcuIdaB2erwhpList.Add(i4gLcuIdaB2erwhp);
        }
        return i4gLcuIdaB2erwhpList.Count == 0 ? (List<OBqe2IUAeSpOmlOQ4O.HrpI4gLcuIdaB2erwhp>) null : i4gLcuIdaB2erwhpList;
      }

      private unsafe void bbijKtxMDJ(OBqe2IUAeSpOmlOQ4O.K14OTLLT4lAAZwtUFKB _param1)
      {
        object obj1;
        switch (_param1.wk1jFwCKSW)
        {
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 0:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 15:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 54:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 59:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 98:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 110:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 126:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 166:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx1 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA1 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            Array array1 = (Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null);
            Type elementType = array1.GetType().GetElementType();
            array1.SetValue(lxulLsB1AbCdvkPgx1.MNddRugcTR(elementType), wqpLuhZiq5CncNsA1.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 1:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx2 = this.xvIX12K5P4.CmTvHQWOmB();
            int num1 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).UOZu2PVRec(lxulLsB1AbCdvkPgx2) ? 1 : 0;
            if (num1 != 0)
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1));
            else
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0));
            if (num1 == 0)
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 2:
            this.wIZXgFO6U7 = true;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 3:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).g1bdVnKyeV());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 4:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.f6yUdEL63YbYB5aCQOA((int) this.P6nXKq05L2, this));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 5:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA2 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (double), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA2.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 6:
            FieldInfo fieldInfo1 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveField((int) this.P6nXKq05L2);
            object obj2 = this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR(fieldInfo1.FieldType);
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx3 = this.xvIX12K5P4.CmTvHQWOmB();
            object obj3 = lxulLsB1AbCdvkPgx3.MNddRugcTR((Type) null);
            if (obj3 == null)
            {
              Type type = fieldInfo1.DeclaringType;
              if (type.IsByRef)
                type = type.GetElementType();
              obj3 = type.IsValueType ? Activator.CreateInstance(type) : throw new NullReferenceException();
              if (lxulLsB1AbCdvkPgx3 is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
                ((OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp) lxulLsB1AbCdvkPgx3).Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type, obj3));
            }
            fieldInfo1.SetValue(obj3, obj2);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 8:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx4 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx4.gyIuhELnxq())
              lxulLsB1AbCdvkPgx4 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) lxulLsB1AbCdvkPgx4).H7PdJ5iAPU();
            this.xvIX12K5P4.CmTvHQWOmB().tY3dXGtH5f(lxulLsB1AbCdvkPgx4);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 9:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 19:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 107:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 133:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 140:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 154:
            throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 10:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA3 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA4 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA4 == null || wqpLuhZiq5CncNsA3 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA4.xXAuQJbXWE((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA3));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 12:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx5 = this.xvIX12K5P4.CmTvHQWOmB();
            if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).UOZu2PVRec(lxulLsB1AbCdvkPgx5))
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1));
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 13:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).mgbdkYqhCS());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 14:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).OxXdtNlUQr());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 16:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx6 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA5 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx6);
            if (lxulLsB1AbCdvkPgx6 != null && lxulLsB1AbCdvkPgx6.DWVuea1EMj() && wqpLuhZiq5CncNsA5 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA5.H7PdJ5iAPU());
              break;
            }
            if (wqpLuhZiq5CncNsA5 == null || !wqpLuhZiq5CncNsA5.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) *(short*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA5).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 17:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).fDcu5RVPtU());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 18:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA6 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA7 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA7 == null || wqpLuhZiq5CncNsA6 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA7.fdRuBgjrLs((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA6));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 20:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA8 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA9 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA9 == null || wqpLuhZiq5CncNsA8 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA9.PZYuDsqDg5((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA8));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 21:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA10 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA11 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA11 == null || wqpLuhZiq5CncNsA10 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA11.HL6u0YI3Yb((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA10));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 22:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA12 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (short), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA12.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 23:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx7 = this.xvIX12K5P4.CmTvHQWOmB();
            if (!OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).soGuJAfcec(lxulLsB1AbCdvkPgx7))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 24:
            MethodBase methodBase1 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveMethod((int) this.P6nXKq05L2);
            Type type1 = this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null).GetType();
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
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(methodBase2.MethodHandle.GetFunctionPointer()));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 25:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).LJ3udQlIYO());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 26:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA13 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA14 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA14 == null || wqpLuhZiq5CncNsA13 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA14.KGbuURteF6((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA13));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 27:
            this.nQ3XmWfO7h(true);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 28:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx8 = this.xvIX12K5P4.CmTvHQWOmB();
            if (!OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).a5iuHuxxL9(lxulLsB1AbCdvkPgx8))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 29:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).NWXd8DtKgs());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 30:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx9 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx9 != null && lxulLsB1AbCdvkPgx9.V1kdEyl02V())
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 31:
            throw this.GT5XkyXtsY;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 32:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).Length, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 33:
            int p6nXkq05L2_1 = (int) this.P6nXKq05L2;
            Module module = typeof (OBqe2IUAeSpOmlOQ4O).Module;
            obj1 = (object) null;
            object obj4;
            try
            {
              obj4 = (object) module.ResolveType(p6nXkq05L2_1);
            }
            catch
            {
              try
              {
                obj4 = (object) module.ResolveMethod(p6nXkq05L2_1);
              }
              catch
              {
                try
                {
                  obj4 = (object) module.ResolveField(p6nXkq05L2_1);
                }
                catch
                {
                  obj4 = (object) module.ResolveMember(p6nXkq05L2_1);
                }
              }
            }
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB(obj4));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 34:
            FieldInfo fieldInfo2 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveField((int) this.P6nXKq05L2);
            object obj5 = this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR(fieldInfo2.FieldType);
            fieldInfo2.SetValue((object) null, obj5);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 35:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx10 = this.xvIX12K5P4.CmTvHQWOmB();
            if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).s3rubpelFd(lxulLsB1AbCdvkPgx10))
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1));
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 36:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA15 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            Array array2 = (Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null);
            object obj6 = array2.GetValue(wqpLuhZiq5CncNsA15.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(array2.GetType().GetElementType(), obj6));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 37:
            this.xvIX12K5P4.DjWvWsmuSB(((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) this.xvIX12K5P4.CmTvHQWOmB()).PPauaFcpRf());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 38:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx11 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA16 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx11);
            if (lxulLsB1AbCdvkPgx11 != null && lxulLsB1AbCdvkPgx11.DWVuea1EMj() && wqpLuhZiq5CncNsA16 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA16.Gsjdeg8ja3());
              break;
            }
            if (wqpLuhZiq5CncNsA16 == null || !wqpLuhZiq5CncNsA16.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) *(ushort*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA16).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 39:
            lock (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.tnBXycvSYh)
            {
              object key = this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null);
              OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx12 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) null;
              if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.xRkXwNANCW.TryGetValue(key, out lxulLsB1AbCdvkPgx12))
              {
                this.xvIX12K5P4.DjWvWsmuSB(lxulLsB1AbCdvkPgx12);
                break;
              }
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null));
              break;
            }
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 40:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx13 = this.xvIX12K5P4.CmTvHQWOmB();
            object obj7 = lxulLsB1AbCdvkPgx13.DWVuea1EMj() ? lxulLsB1AbCdvkPgx13.MNddRugcTR((Type) null) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(obj7 == null ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null) : OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(obj7.GetType(), obj7));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 41:
            FieldInfo fieldInfo3 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveField((int) this.P6nXKq05L2);
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx14 = this.xvIX12K5P4.CmTvHQWOmB();
            lxulLsB1AbCdvkPgx14.gRCuEnOnhD();
            object obj8 = lxulLsB1AbCdvkPgx14.MNddRugcTR((Type) null);
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8(fieldInfo3, obj8));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 42:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).u7udyDaiuk());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 43:
            this.xvIX12K5P4.DjWvWsmuSB(this.xvIX12K5P4.HtMvTddAIt());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 44:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA17 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA18 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA18 == null || wqpLuhZiq5CncNsA17 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA18.AKNuAZ11DR((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA17));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 45:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx15 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx15 == null || !lxulLsB1AbCdvkPgx15.V1kdEyl02V())
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 46:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx16 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA19 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx16);
            if (lxulLsB1AbCdvkPgx16 != null && lxulLsB1AbCdvkPgx16.DWVuea1EMj() && wqpLuhZiq5CncNsA19 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA19.m0QdhS1F5D());
              break;
            }
            if (wqpLuhZiq5CncNsA19 == null || !wqpLuhZiq5CncNsA19.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) *(byte*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA19).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 47:
            throw (Exception) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null);
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 48:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx17 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx17.gyIuhELnxq())
              lxulLsB1AbCdvkPgx17 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) lxulLsB1AbCdvkPgx17).EAfdGAC0F3();
            this.xvIX12K5P4.CmTvHQWOmB().tY3dXGtH5f(lxulLsB1AbCdvkPgx17);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 49:
            this.xvIX12K5P4.DjWvWsmuSB(this.xvIX12K5P4.CmTvHQWOmB().gRCuEnOnhD());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 50:
            FieldInfo fieldInfo4 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveField((int) this.P6nXKq05L2);
            object obj9 = this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null);
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(fieldInfo4.FieldType, fieldInfo4.GetValue(obj9)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 51:
            IntPtr num2 = Marshal.AllocHGlobal((this.xvIX12K5P4.CmTvHQWOmB() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
            if (this.vBXXs2g5N5 == null)
              this.vBXXs2g5N5 = new List<IntPtr>();
            this.vBXXs2g5N5.Add(num2);
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(num2));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 52:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA20 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (uint), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA20.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 53:
            int[] p6nXkq05L2_2 = (int[]) this.P6nXKq05L2;
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA21 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            long index1 = wqpLuhZiq5CncNsA21.j56dmjESoe().EtUBZQQZRa.DoQBY2qDkD;
            if ((index1 < 0L || wqpLuhZiq5CncNsA21.YVbv0y1oss()) && IntPtr.Size == 4)
              index1 = (long) (int) index1;
            if (wqpLuhZiq5CncNsA21.glCvoAFEwW())
            {
              OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj kqtLxplwjOaJo6Hj = (OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj) wqpLuhZiq5CncNsA21;
              if (kqtLxplwjOaJo6Hj.Y69BeUt9vR == (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6)
                index1 = (long) kqtLxplwjOaJo6Hj.d37Bh8uTDv.MlKB6i90Nn;
            }
            if (index1 >= (long) p6nXkq05L2_2.Length || index1 < 0L)
              break;
            this.kO9XrsjAAr = p6nXkq05L2_2[index1] - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 55:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA22 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA23 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA22 == null || wqpLuhZiq5CncNsA23 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA22.pY5uXSxpHK((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA23));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 56:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).EAfdGAC0F3());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 57:
            if (!this.xvIX12K5P4.CmTvHQWOmB().BeouTiljCp(this.xvIX12K5P4.CmTvHQWOmB()))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 58:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).fATdY7yOVp());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 60:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA24 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA25 = (OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) this.xvIX12K5P4.CmTvHQWOmB();
            if (wqpLuhZiq5CncNsA25 == null || wqpLuhZiq5CncNsA24 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA25.XJsu7LDPbi((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA24));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 61:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx18 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx18.gyIuhELnxq())
              lxulLsB1AbCdvkPgx18 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) lxulLsB1AbCdvkPgx18).u7udyDaiuk();
            this.xvIX12K5P4.CmTvHQWOmB().tY3dXGtH5f(lxulLsB1AbCdvkPgx18);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 62:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx19 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA26 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx19);
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx20 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA27 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx20);
            if (wqpLuhZiq5CncNsA27 == null || wqpLuhZiq5CncNsA26 == null)
            {
              if (lxulLsB1AbCdvkPgx19.BeouTiljCp(lxulLsB1AbCdvkPgx20))
              {
                this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1));
                break;
              }
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0));
              break;
            }
            if (wqpLuhZiq5CncNsA27.lwlumgaheq(lxulLsB1AbCdvkPgx19))
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1));
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 63:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).IYDdrwuQUc());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 65:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 66:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).wsddpbkEQb());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 67:
            ConstructorInfo constructorInfo = (ConstructorInfo) typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveMethod((int) this.P6nXKq05L2);
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            object[] objArray1 = new object[parameters.Length];
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[] lxulLsB1AbCdvkPgxArray = new OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[parameters.Length];
            List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar> doeNeLfV5aXcK5H1ArList = (List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar>) null;
            OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a dhe3JwLp0DcXgE8Rz6a = (OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a) null;
            int num3 = 0;
            object obj10;
            while (true)
            {
              object obj11;
              object[] objArray2;
              int num4;
              if (num3 < parameters.Length)
              {
                OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx21 = this.xvIX12K5P4.CmTvHQWOmB();
                Type type3 = parameters[parameters.Length - 1 - num3].ParameterType;
                obj11 = (object) null;
                bool flag = false;
                if (type3.IsByRef && lxulLsB1AbCdvkPgx21 is OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8 lnOlmJmDbgRqeOe8)
                {
                  if (doeNeLfV5aXcK5H1ArList == null)
                    doeNeLfV5aXcK5H1ArList = new List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar>();
                  doeNeLfV5aXcK5H1ArList.Add(new OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar(lnOlmJmDbgRqeOe8.A39joYqGSV, parameters.Length - 1 - num3));
                  obj11 = lnOlmJmDbgRqeOe8.nGEjBfyeTD;
                  if (obj11 is OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX)
                    lxulLsB1AbCdvkPgx21 = obj11 as OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX;
                  else
                    flag = true;
                }
                if (!flag)
                {
                  if (lxulLsB1AbCdvkPgx21 != null)
                    obj11 = lxulLsB1AbCdvkPgx21.MNddRugcTR(type3);
                  if (obj11 == null)
                  {
                    if (type3.IsByRef)
                      type3 = type3.GetElementType();
                    if (type3.IsValueType)
                    {
                      obj11 = Activator.CreateInstance(type3);
                      if (lxulLsB1AbCdvkPgx21 is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
                        ((OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp) lxulLsB1AbCdvkPgx21).Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type3, obj11));
                    }
                  }
                }
                lxulLsB1AbCdvkPgxArray[objArray1.Length - 1 - num3] = lxulLsB1AbCdvkPgx21;
                objArray2 = objArray1;
                num4 = objArray1.Length - 1;
              }
              else
                goto label_473;
label_471:
              int num5 = num3;
              int index2 = num4 - num5;
              object obj12 = obj11;
              objArray2[index2] = obj12;
              ++num3;
              continue;
label_473:
              OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr bopLnoZtiN7k24Gr = (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr) null;
              if (doeNeLfV5aXcK5H1ArList != null)
              {
                dhe3JwLp0DcXgE8Rz6a = new OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a((MethodBase) constructorInfo, doeNeLfV5aXcK5H1ArList);
                bopLnoZtiN7k24Gr = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.p5CXJfBK9B((MethodBase) constructorInfo, true, dhe3JwLp0DcXgE8Rz6a);
              }
              obj1 = (object) null;
              obj10 = bopLnoZtiN7k24Gr == null ? constructorInfo.Invoke(objArray1) : bopLnoZtiN7k24Gr((object) null, objArray1);
              for (int index3 = 0; index3 < parameters.Length; ++index3)
              {
                if (parameters[index3].ParameterType.IsByRef)
                {
                  if (dhe3JwLp0DcXgE8Rz6a == null || !dhe3JwLp0DcXgE8Rz6a.LaJjisNd9u(index3))
                  {
                    if (lxulLsB1AbCdvkPgxArray[index3].LkLvBB03rl())
                      ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) lxulLsB1AbCdvkPgxArray[index3]).L97BKE25N4(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameters[index3].ParameterType, objArray1[index3]));
                    else if (lxulLsB1AbCdvkPgxArray[index3] is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
                      lxulLsB1AbCdvkPgxArray[index3].nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameters[index3].ParameterType.GetElementType(), objArray1[index3]));
                    else
                      lxulLsB1AbCdvkPgxArray[index3].nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameters[index3].ParameterType, objArray1[index3]));
                  }
                  else
                    goto label_471;
                }
              }
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(constructorInfo.DeclaringType, obj10));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 68:
            if (!this.xvIX12K5P4.CmTvHQWOmB().SHouWnYaPf(this.xvIX12K5P4.CmTvHQWOmB()))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 69:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA28 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.HtMvTddAIt());
            if (wqpLuhZiq5CncNsA28 == null)
              throw new ArithmeticException(((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 0).ToString());
            if (!(wqpLuhZiq5CncNsA28 is OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ umoXlRwIcnpEjt1Uj))
              break;
            if (double.IsNaN(umoXlRwIcnpEjt1Uj.BUuja1QMZO))
              throw new OverflowException(((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 2).ToString());
            if (!double.IsInfinity(umoXlRwIcnpEjt1Uj.BUuja1QMZO))
              break;
            throw new OverflowException(((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 1).ToString());
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 70:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA29 = this.xvIX12K5P4.CmTvHQWOmB() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA;
            IntPtr num6 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.bltXeOrHnB(this.xvIX12K5P4.CmTvHQWOmB());
            IntPtr num7 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.bltXeOrHnB(this.xvIX12K5P4.CmTvHQWOmB());
            if (!(num6 != IntPtr.Zero) || !(num7 != IntPtr.Zero))
              break;
            uint mlKb6i90Nn1 = wqpLuhZiq5CncNsA29.d8IdbKZ3nT().d37Bh8uTDv.MlKB6i90Nn;
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.KbHXNoolqk(num7, num6, mlKb6i90Nn1);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 71:
            int p6nXkq05L2_3 = (int) this.P6nXKq05L2;
            this.Ti3XqqGbnS[p6nXkq05L2_3] = this.Tf1j86bleG(this.xvIX12K5P4.CmTvHQWOmB(), this.eV7XiE3C4H.fBcjhfhonR[p6nXkq05L2_3].WhxjRcXFCB, this.eV7XiE3C4H.fBcjhfhonR[p6nXkq05L2_3].r4GjQb7Vvg);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 72:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA30 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA31 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA30 == null || wqpLuhZiq5CncNsA31 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA30.qmOuvwnxTA((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA31));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 73:
            if (OBqe2IUAeSpOmlOQ4O.tMVaB3hxr3.Count == 0)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.iGQH0gLZd16LDXi2iXN(typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveString((int) this.P6nXKq05L2 | 1879048192)));
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.iGQH0gLZd16LDXi2iXN(OBqe2IUAeSpOmlOQ4O.tMVaB3hxr3[(int) this.P6nXKq05L2]));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 74:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA32 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (long), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA32.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 75:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx22 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA33 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx22);
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx23 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA34 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx23);
            if (wqpLuhZiq5CncNsA34 == null || wqpLuhZiq5CncNsA33 == null)
            {
              if (!lxulLsB1AbCdvkPgx22.BeouTiljCp(lxulLsB1AbCdvkPgx23))
                break;
              this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
              break;
            }
            if (!wqpLuhZiq5CncNsA34.lwlumgaheq(lxulLsB1AbCdvkPgx22))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 76:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).jW7dKWHyFW());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 77:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA35 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (IntPtr), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA35.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 78:
            this.WT9X3IVJtU = (bool) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR(typeof (bool));
            this.WUtX89BpW0 = true;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 79:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).T52uLk6FSX());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 80:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).RgCd2ry1ac());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 81:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA36 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (sbyte), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA36.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 82:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((double) this.P6nXKq05L2));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 83:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).rcedIif63y());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 84:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).usfdqHavse());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 85:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx24 = this.xvIX12K5P4.CmTvHQWOmB();
            if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).soGuJAfcec(lxulLsB1AbCdvkPgx24))
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1));
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 86:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA37 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (ushort), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA37.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 87:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx25 = this.xvIX12K5P4.CmTvHQWOmB();
            this.xvIX12K5P4.CmTvHQWOmB().tY3dXGtH5f(lxulLsB1AbCdvkPgx25);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 88:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA38 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA39 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA39 == null || wqpLuhZiq5CncNsA38 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA39.OJsungeQZd((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA38));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 89:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx26 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA40 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx26);
            if (lxulLsB1AbCdvkPgx26 != null && lxulLsB1AbCdvkPgx26.DWVuea1EMj() && wqpLuhZiq5CncNsA40 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA40.RgCd2ry1ac());
              break;
            }
            if (wqpLuhZiq5CncNsA40 == null || !wqpLuhZiq5CncNsA40.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(*(int*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA40).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 91:
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 92:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx27 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx27.gyIuhELnxq())
              lxulLsB1AbCdvkPgx27 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) lxulLsB1AbCdvkPgx27).rcedIif63y();
            this.xvIX12K5P4.CmTvHQWOmB().tY3dXGtH5f(lxulLsB1AbCdvkPgx27);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 93:
            Type type4 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            if (!(this.xvIX12K5P4.CmTvHQWOmB() is OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp fl6HlHf4Wx0BoEfgp1))
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            if (type4.IsValueType)
            {
              object instance = Activator.CreateInstance(type4);
              fl6HlHf4Wx0BoEfgp1.Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type4, instance));
              break;
            }
            fl6HlHf4Wx0BoEfgp1.Eqfup3BbFp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 94:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx28 = this.xvIX12K5P4.CmTvHQWOmB();
            if (!OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).kZGutsdjSl(lxulLsB1AbCdvkPgx28))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 95:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx29 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA41 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx29);
            if (lxulLsB1AbCdvkPgx29 != null && lxulLsB1AbCdvkPgx29.DWVuea1EMj() && wqpLuhZiq5CncNsA41 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA41.u7udyDaiuk());
              break;
            }
            IntPtr num8 = wqpLuhZiq5CncNsA41 != null && wqpLuhZiq5CncNsA41.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA41).NrdBVmtKL7() : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            if (IntPtr.Size == 8)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(*(long*) (void*) num8, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12));
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((long) *(int*) (void*) num8, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 12));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 96:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).oUfdwd7RBO());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 97:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA42 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (byte), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA42.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 99:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx30 = this.xvIX12K5P4.CmTvHQWOmB();
            if (!OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).s3rubpelFd(lxulLsB1AbCdvkPgx30))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 100:
            if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.UaBXcOcakv(this.xvIX12K5P4.CmTvHQWOmB()).SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.UaBXcOcakv(this.xvIX12K5P4.CmTvHQWOmB())))
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1));
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 101:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx31 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA43 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx31);
            if (lxulLsB1AbCdvkPgx31 != null && lxulLsB1AbCdvkPgx31.DWVuea1EMj() && wqpLuhZiq5CncNsA43 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA43.OxXdtNlUQr());
              break;
            }
            if (wqpLuhZiq5CncNsA43 == null || !wqpLuhZiq5CncNsA43.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) *(sbyte*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA43).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 102:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).AYcdCCnBuC());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 103:
            Type type5 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            object obj13 = this.xvIX12K5P4.CmTvHQWOmB().gRCuEnOnhD().MNddRugcTR(type5);
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type5, obj13));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 104:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA44 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA45 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA45 == null || wqpLuhZiq5CncNsA44 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA45.rFou9powA0((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA44));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 105:
            Type type6 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            object obj14 = this.xvIX12K5P4.CmTvHQWOmB() is OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp fl6HlHf4Wx0BoEfgp2 ? fl6HlHf4Wx0BoEfgp2.MNddRugcTR(type6) : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx32;
            if (obj14 != null)
            {
              if (type6.IsValueType)
                obj14 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.BSmXfQAMYn(obj14);
              lxulLsB1AbCdvkPgx32 = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type6, obj14);
            }
            else if (type6.IsValueType)
            {
              object instance = Activator.CreateInstance(type6);
              lxulLsB1AbCdvkPgx32 = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type6, instance);
            }
            else
              lxulLsB1AbCdvkPgx32 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null);
            this.xvIX12K5P4.DjWvWsmuSB(lxulLsB1AbCdvkPgx32);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 108:
            FieldInfo fieldInfo5 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveField((int) this.P6nXKq05L2);
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(fieldInfo5.FieldType, fieldInfo5.GetValue((object) null)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 109:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU((int) this.P6nXKq05L2, this));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 111:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).K2Gd1PW7Sr());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 112:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).HhEuMQLBcy());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 113:
            Type type7 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx33 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA46 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).SetValue(lxulLsB1AbCdvkPgx33.MNddRugcTR(type7), wqpLuhZiq5CncNsA46.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 114:
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 142:
            Type type8 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            object obj15 = this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR(type8);
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx34;
            if (obj15 != null)
            {
              if (type8.IsValueType)
                obj15 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.BSmXfQAMYn(obj15);
              lxulLsB1AbCdvkPgx34 = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type8, obj15);
            }
            else if (type8.IsValueType)
            {
              object instance = Activator.CreateInstance(type8);
              lxulLsB1AbCdvkPgx34 = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type8, instance);
            }
            else
              lxulLsB1AbCdvkPgx34 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null);
            if (!(this.xvIX12K5P4.CmTvHQWOmB() is OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp fl6HlHf4Wx0BoEfgp3))
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            fl6HlHf4Wx0BoEfgp3.nOQdl4ODOg(lxulLsB1AbCdvkPgx34);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 115:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx35 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA47 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx35);
            if (lxulLsB1AbCdvkPgx35 != null && lxulLsB1AbCdvkPgx35.DWVuea1EMj() && wqpLuhZiq5CncNsA47 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA47.EAfdGAC0F3());
              break;
            }
            if (wqpLuhZiq5CncNsA47 == null || !wqpLuhZiq5CncNsA47.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(*(float*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA47).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 9));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 116:
            this.xvIX12K5P4.DjWvWsmuSB(this.GL6XCXXAY9[(int) this.P6nXKq05L2]);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 117:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx36 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA48 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx36);
            if (lxulLsB1AbCdvkPgx36 != null && lxulLsB1AbCdvkPgx36.DWVuea1EMj() && wqpLuhZiq5CncNsA48 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA48.R3wdfbZewF());
              break;
            }
            if (wqpLuhZiq5CncNsA48 == null || !wqpLuhZiq5CncNsA48.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(*(uint*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA48).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 118:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA49 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA50 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA50 == null || wqpLuhZiq5CncNsA49 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA50.CrluoqnI7g((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA49));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 119:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA51 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA52 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA52 == null || wqpLuhZiq5CncNsA51 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA52.Add((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA51));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 120:
            Type type9 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx37 = this.xvIX12K5P4.CmTvHQWOmB();
            object obj16 = lxulLsB1AbCdvkPgx37.MNddRugcTR((Type) null);
            if (obj16 == null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null));
              break;
            }
            if (!type9.IsAssignableFrom(obj16.GetType()))
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) null));
              break;
            }
            this.xvIX12K5P4.DjWvWsmuSB(lxulLsB1AbCdvkPgx37);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 121:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA53 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA54 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA54 == null || wqpLuhZiq5CncNsA53 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA54.d69uOOjWrX((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA53));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 122:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).Gsjdeg8ja3());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 123:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).erPdzHD1s2());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 124:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA55 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA56 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA56 == null || wqpLuhZiq5CncNsA55 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA56.YYHujsF82I((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA55));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 125:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((long) this.P6nXKq05L2));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 127:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx38 = this.xvIX12K5P4.CmTvHQWOmB();
            if (!OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).oj9u6Fhwca(lxulLsB1AbCdvkPgx38))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 128:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).PvEdZLuSne());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 129:
            int p6nXkq05L2_4 = (int) this.P6nXKq05L2;
            if (this.eV7XiE3C4H.qpZjJxQ46V.IsStatic)
            {
              this.GL6XCXXAY9[p6nXkq05L2_4] = this.Tf1j86bleG(this.xvIX12K5P4.CmTvHQWOmB(), this.eV7XiE3C4H.JDJjcB4nud[p6nXkq05L2_4].RcQjuILAQC);
              break;
            }
            this.GL6XCXXAY9[p6nXkq05L2_4] = this.Tf1j86bleG(this.xvIX12K5P4.CmTvHQWOmB(), this.eV7XiE3C4H.JDJjcB4nud[p6nXkq05L2_4 - 1].RcQjuILAQC);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 130:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx39 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA57 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx39);
            if (lxulLsB1AbCdvkPgx39 != null && lxulLsB1AbCdvkPgx39.DWVuea1EMj() && wqpLuhZiq5CncNsA57 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA57.rcedIif63y());
              break;
            }
            if (wqpLuhZiq5CncNsA57 == null || !wqpLuhZiq5CncNsA57.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ(*(double*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA57).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 10));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 131:
            this.kO9XrsjAAr = -3;
            if (this.xvIX12K5P4.hMWvSoG03q() <= 0)
              break;
            this.Gl4XYW9Iw1 = this.xvIX12K5P4.CmTvHQWOmB();
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 132:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).fcnd3S7gbG());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 134:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) Array.CreateInstance(typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2), OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 136:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).t8YdsLWtVo());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 137:
            Type type10 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA58 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            object obj17 = ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA58.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg);
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type10, obj17));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 138:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((float) this.P6nXKq05L2));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 139:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).LmFdi8gd0R());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 141:
            this.nQ3XmWfO7h(false);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 143:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA59 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA60 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA60 == null || wqpLuhZiq5CncNsA59 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA60.S5MuleAUoL((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA59));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 144:
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            this.d6xXV9O6wf = true;
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 145:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA61 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA62 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA61 == null || wqpLuhZiq5CncNsA62 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA61.N4muuK8Z5O((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA62));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 146:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA63 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA64 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA64 == null || wqpLuhZiq5CncNsA63 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA64.cSauRN3PjN((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA63));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 147:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).Y08dPd87iv());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 148:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.iy9X7mlNkM(typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2)), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 149:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).VVAuxnv6BF());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 150:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx40 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx40.gyIuhELnxq())
              lxulLsB1AbCdvkPgx40 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) lxulLsB1AbCdvkPgx40).OxXdtNlUQr();
            this.xvIX12K5P4.CmTvHQWOmB().tY3dXGtH5f(lxulLsB1AbCdvkPgx40);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 151:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA65 = this.xvIX12K5P4.CmTvHQWOmB() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA;
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA66 = this.xvIX12K5P4.CmTvHQWOmB() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA;
            IntPtr num9 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.bltXeOrHnB(this.xvIX12K5P4.CmTvHQWOmB());
            if (!(num9 != IntPtr.Zero))
              break;
            byte c1oBhylShu = wqpLuhZiq5CncNsA66.X75dWxBQwq().d37Bh8uTDv.C1oBHylShu;
            uint mlKb6i90Nn2 = wqpLuhZiq5CncNsA65.d8IdbKZ3nT().d37Bh8uTDv.MlKB6i90Nn;
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.xoaXpMBebu(num9, c1oBhylShu, (int) mlKb6i90Nn2);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 153:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).YEUdcnGvKX());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 155:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).H7PdJ5iAPU());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 156:
            lock (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.tnBXycvSYh)
            {
              OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx41 = this.xvIX12K5P4.CmTvHQWOmB();
              object key = this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null);
              OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.xRkXwNANCW[key] = lxulLsB1AbCdvkPgx41;
              break;
            }
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 157:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8(typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveField((int) this.P6nXKq05L2), (object) null));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 158:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx42 = this.xvIX12K5P4.CmTvHQWOmB();
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA67 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(lxulLsB1AbCdvkPgx42);
            if (lxulLsB1AbCdvkPgx42 != null && lxulLsB1AbCdvkPgx42.DWVuea1EMj() && wqpLuhZiq5CncNsA67 != null)
            {
              this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA67.YEUdcnGvKX());
              break;
            }
            if (wqpLuhZiq5CncNsA67 == null || !wqpLuhZiq5CncNsA67.LkLvBB03rl())
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(*(long*) (void*) ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) wqpLuhZiq5CncNsA67).NrdBVmtKL7(), (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 159:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).R3wdfbZewF());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 160:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA68 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA69 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            if (wqpLuhZiq5CncNsA69 == null || wqpLuhZiq5CncNsA68 == null)
              throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
            this.xvIX12K5P4.DjWvWsmuSB(wqpLuhZiq5CncNsA69.TvpuFtHMx3((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) wqpLuhZiq5CncNsA68));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 161:
            typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA70 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            Array array3 = (Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null);
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.daEbHELbQht9GYRuEJp(wqpLuhZiq5CncNsA70.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg, array3));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 162:
            this.xvIX12K5P4.CmTvHQWOmB();
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 163:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) this.P6nXKq05L2));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 164:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).jyudNqrNXJ());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 165:
            Type type11 = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveType((int) this.P6nXKq05L2);
            object obj18 = this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR(type11) ?? Activator.CreateInstance(type11);
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB((object) OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type11, OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.BSmXfQAMYn(obj18))));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 167:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14(typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveMethod((int) this.P6nXKq05L2).MethodHandle.GetFunctionPointer()));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 168:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx43 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx43.gyIuhELnxq())
              lxulLsB1AbCdvkPgx43 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) lxulLsB1AbCdvkPgx43).YEUdcnGvKX();
            this.xvIX12K5P4.CmTvHQWOmB().tY3dXGtH5f(lxulLsB1AbCdvkPgx43);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 169:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA71 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (float), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA71.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 170:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx44 = this.xvIX12K5P4.CmTvHQWOmB();
            if (lxulLsB1AbCdvkPgx44.gyIuhELnxq())
              lxulLsB1AbCdvkPgx44 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) lxulLsB1AbCdvkPgx44).RgCd2ry1ac();
            this.xvIX12K5P4.CmTvHQWOmB().tY3dXGtH5f(lxulLsB1AbCdvkPgx44);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 171:
            this.xvIX12K5P4.DjWvWsmuSB(this.Ti3XqqGbnS[(int) this.P6nXKq05L2]);
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 172:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).m0QdhS1F5D());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 173:
            OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA72 = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB());
            this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(typeof (int), ((Array) this.xvIX12K5P4.CmTvHQWOmB().MNddRugcTR((Type) null)).GetValue(wqpLuhZiq5CncNsA72.YjldSFyrtV().d37Bh8uTDv.GLgBtRhyOg)));
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 174:
            this.xvIX12K5P4.DjWvWsmuSB((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()) ?? throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i()).c5AdgYKqRb());
            break;
          case (OBqe2IUAeSpOmlOQ4O.mP2oxFLYK5cKBXZuceg) 175:
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx45 = this.xvIX12K5P4.CmTvHQWOmB();
            if (!OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.emQXRxn40M(this.xvIX12K5P4.CmTvHQWOmB()).AMUuSZ5Bny(lxulLsB1AbCdvkPgx45))
              break;
            this.kO9XrsjAAr = (int) this.P6nXKq05L2 - 1;
            break;
        }
      }

      private OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX Tf1j86bleG(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1,
        OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw _param2,
        bool _param3 = false)
      {
        if (!_param3 && _param1.DWVuea1EMj())
          _param1 = _param1.gRCuEnOnhD();
        if (_param1.glCvoAFEwW())
          return ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).sqedUSL72O(_param2);
        if (_param1.E4wvjiQ2aY())
          return ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).sqedUSL72O(_param2);
        if (_param1.YVbv0y1oss())
          return ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).sqedUSL72O(_param2);
        return _param1.LkLvBB03rl() ? ((OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA) _param1).sqedUSL72O(_param2) : _param1;
      }

      private OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX R5TlnuhCDh(int _param1)
      {
        return this.Ti3XqqGbnS[_param1];
      }

      private void YJaloo5Icr(int _param1)
      {
        this.iOjXUP0iwQ(_param1, this.xvIX12K5P4.CmTvHQWOmB());
      }

      private static int iy9X7mlNkM(Type _param0)
      {
        lock (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.hL4XIw57CM)
        {
          if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.us3XG3bSNL == null)
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.us3XG3bSNL = new Dictionary<Type, int>();
          try
          {
            int num1 = 0;
            if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.us3XG3bSNL.TryGetValue(_param0, out num1))
              return num1;
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (int), Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Sizeof, _param0);
            ilGenerator.Emit(OpCodes.Ret);
            int num2 = (int) dynamicMethod.Invoke((object) null, (object[]) null);
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.us3XG3bSNL[_param0] = num2;
            return num2;
          }
          catch
          {
            return 0;
          }
        }
      }

      private void iOjXUP0iwQ(int _param1, OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param2)
      {
        this.Ti3XqqGbnS[_param1] = this.Tf1j86bleG(_param2, this.eV7XiE3C4H.fBcjhfhonR[_param1].WhxjRcXFCB, this.eV7XiE3C4H.fBcjhfhonR[_param1].r4GjQb7Vvg);
      }

      private static OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA emQXRxn40M(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param0)
      {
        if (!(_param0 is OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA) && _param0.DWVuea1EMj())
          wqpLuhZiq5CncNsA = _param0.gRCuEnOnhD() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA;
        return wqpLuhZiq5CncNsA;
      }

      private void nQ3XmWfO7h(bool _param1)
      {
        MethodBase methodBase = typeof (OBqe2IUAeSpOmlOQ4O).Module.ResolveMethod((int) this.P6nXKq05L2);
        MethodInfo methodInfo = methodBase as MethodInfo;
        ParameterInfo[] parameters = methodBase.GetParameters();
        object[] objArray = new object[parameters.Length];
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[] lxulLsB1AbCdvkPgxArray = new OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[parameters.Length];
        List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar> doeNeLfV5aXcK5H1ArList = (List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar>) null;
        OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a dhe3JwLp0DcXgE8Rz6a = (OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a) null;
        for (int index = 0; index < parameters.Length; ++index)
        {
          OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = this.xvIX12K5P4.CmTvHQWOmB();
          Type type = parameters[parameters.Length - 1 - index].ParameterType;
          object obj = (object) null;
          bool flag = false;
          if (type.IsByRef && lxulLsB1AbCdvkPgx is OBqe2IUAeSpOmlOQ4O.dcnLnOLmJmDBGRqeOE8 lnOlmJmDbgRqeOe8)
          {
            if (doeNeLfV5aXcK5H1ArList == null)
              doeNeLfV5aXcK5H1ArList = new List<OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar>();
            doeNeLfV5aXcK5H1ArList.Add(new OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar(lnOlmJmDbgRqeOe8.A39joYqGSV, parameters.Length - 1 - index));
            obj = lnOlmJmDbgRqeOe8.nGEjBfyeTD;
            if (obj is OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX)
            {
              lxulLsB1AbCdvkPgx = obj as OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX;
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
                  obj = !lnOlmJmDbgRqeOe8.A39joYqGSV.IsStatic ? Activator.CreateInstance(type) : lnOlmJmDbgRqeOe8.A39joYqGSV.GetValue((object) null);
                  if (lxulLsB1AbCdvkPgx is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
                    ((OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp) lxulLsB1AbCdvkPgx).Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type, obj));
                }
              }
            }
          }
          if (!flag)
          {
            if (lxulLsB1AbCdvkPgx != null)
              obj = lxulLsB1AbCdvkPgx.MNddRugcTR(type);
            if (obj == null)
            {
              if (type.IsByRef)
                type = type.GetElementType();
              if (type.IsValueType)
              {
                obj = Activator.CreateInstance(type);
                if (lxulLsB1AbCdvkPgx is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
                  ((OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp) lxulLsB1AbCdvkPgx).Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type, obj));
              }
            }
          }
          lxulLsB1AbCdvkPgxArray[objArray.Length - 1 - index] = lxulLsB1AbCdvkPgx;
          objArray[objArray.Length - 1 - index] = obj;
        }
        OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr bopLnoZtiN7k24Gr = (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr) null;
        if (doeNeLfV5aXcK5H1ArList != null)
        {
          dhe3JwLp0DcXgE8Rz6a = new OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a(methodBase, doeNeLfV5aXcK5H1ArList);
          bopLnoZtiN7k24Gr = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.VjkXtf3BeQ(methodBase, _param1, dhe3JwLp0DcXgE8Rz6a);
        }
        else if (methodInfo != (MethodInfo) null && methodInfo.ReturnType.IsByRef)
          bopLnoZtiN7k24Gr = OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.ngsX61877u(methodBase, _param1);
        object target = (object) null;
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx1 = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) null;
        if (!methodBase.IsStatic)
        {
          lxulLsB1AbCdvkPgx1 = this.xvIX12K5P4.CmTvHQWOmB();
          if (lxulLsB1AbCdvkPgx1 != null)
            target = lxulLsB1AbCdvkPgx1.MNddRugcTR(methodBase.DeclaringType);
          if (target == null)
          {
            Type type = methodBase.DeclaringType;
            if (type.IsByRef)
              type = type.GetElementType();
            target = type.IsValueType ? Activator.CreateInstance(type) : throw new NullReferenceException();
            if (target == null && Nullable.GetUnderlyingType(type) != (Type) null)
              target = FormatterServices.GetUninitializedObject(type);
            if (lxulLsB1AbCdvkPgx1 is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
              ((OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp) lxulLsB1AbCdvkPgx1).Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(type, target));
          }
        }
        object obj1;
        if ((object) (methodBase as ConstructorInfo) != null && Nullable.GetUnderlyingType(methodBase.DeclaringType) != (Type) null)
        {
          obj1 = objArray[0];
          if (lxulLsB1AbCdvkPgx1 != null && lxulLsB1AbCdvkPgx1 is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
            ((OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp) lxulLsB1AbCdvkPgx1).Eqfup3BbFp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(Nullable.GetUnderlyingType(methodBase.DeclaringType), obj1));
        }
        else
          obj1 = bopLnoZtiN7k24Gr == null ? methodBase.Invoke(target, objArray) : bopLnoZtiN7k24Gr(target, objArray);
        for (int index = 0; index < parameters.Length; ++index)
        {
          if (parameters[index].ParameterType.IsByRef && (dhe3JwLp0DcXgE8Rz6a == null || !dhe3JwLp0DcXgE8Rz6a.LaJjisNd9u(index)))
          {
            if (lxulLsB1AbCdvkPgxArray[index].LkLvBB03rl())
              ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) lxulLsB1AbCdvkPgxArray[index]).L97BKE25N4(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameters[index].ParameterType, objArray[index]));
            else if (lxulLsB1AbCdvkPgxArray[index] is OBqe2IUAeSpOmlOQ4O.C50CprLSCELDpjV9aKU)
              lxulLsB1AbCdvkPgxArray[index].nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameters[index].ParameterType.GetElementType(), objArray[index]));
            else
              lxulLsB1AbCdvkPgxArray[index].nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(parameters[index].ParameterType, objArray[index]));
          }
        }
        if (!(methodInfo != (MethodInfo) null) || !(methodInfo.ReturnType != typeof (void)))
          return;
        this.xvIX12K5P4.DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(methodInfo.ReturnType, obj1));
      }

      private static OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr ngsX61877u(
        MethodBase _param0,
        bool _param1)
      {
        lock (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.yeHvLKgEPe)
        {
          OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr bopLnoZtiN7k24Gr1 = (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr) null;
          if (_param1)
          {
            if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.UBIXzg2TS3.TryGetValue(_param0, out bopLnoZtiN7k24Gr1))
              return bopLnoZtiN7k24Gr1;
          }
          else if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.kQfvM9jv6W.TryGetValue(_param0, out bopLnoZtiN7k24Gr1))
            return bopLnoZtiN7k24Gr1;
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
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
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
              OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
              ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
              if (localBuilderArray[index].LocalType.IsValueType)
                ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
              ilGenerator.Emit(OpCodes.Stelem_Ref);
            }
          }
          ilGenerator.Emit(OpCodes.Ret);
          OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr bopLnoZtiN7k24Gr2 = (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr) dynamicMethod.CreateDelegate(typeof (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr));
          if (_param1)
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.UBIXzg2TS3.Add(_param0, bopLnoZtiN7k24Gr2);
          else
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.kQfvM9jv6W.Add(_param0, bopLnoZtiN7k24Gr2);
          return bopLnoZtiN7k24Gr2;
        }
      }

      private static OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr VjkXtf3BeQ(
        MethodBase _param0,
        bool _param1,
        OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a _param2)
      {
        lock (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.MTHvairTKI)
        {
          OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr bopLnoZtiN7k24Gr1 = (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr) null;
          if (_param1)
          {
            if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.schv5EvJl0.TryGetValue(_param2, out bopLnoZtiN7k24Gr1))
              return bopLnoZtiN7k24Gr1;
          }
          else if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.Rfevxivn5c.TryGetValue(_param2, out bopLnoZtiN7k24Gr1))
            return bopLnoZtiN7k24Gr1;
          MethodInfo methodInfo = _param0 as MethodInfo;
          DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), new Type[2]
          {
            typeof (object),
            typeof (object[])
          }, typeof (OBqe2IUAeSpOmlOQ4O), true);
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
            localBuilderArray[index] = !_param2.LaJjisNd9u(index) ? ilGenerator.DeclareLocal(typeArray[index]) : ilGenerator.DeclareLocal(typeof (object));
          if (_param0.DeclaringType.IsValueType)
            localBuilderArray[localBuilderArray.Length - 1] = ilGenerator.DeclareLocal(_param0.DeclaringType.MakeByRefType());
          for (int index = 0; index < typeArray.Length; ++index)
          {
            ilGenerator.Emit(OpCodes.Ldarg_1);
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
            ilGenerator.Emit(OpCodes.Ldelem_Ref);
            if (!_param2.LaJjisNd9u(index))
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
            if (!_param2.LaJjisNd9u(index))
            {
              if (parameters[index].ParameterType.IsByRef)
                ilGenerator.Emit(OpCodes.Ldloca_S, localBuilderArray[index]);
              else
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
            }
            else
            {
              OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar doeNeLfV5aXcK5H1Ar = _param2.uJ9jNuhOde(index);
              if (doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.IsStatic)
                ilGenerator.Emit(OpCodes.Ldsflda, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9);
              else if (doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.DeclaringType.IsValueType)
              {
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                ilGenerator.Emit(OpCodes.Unbox, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.DeclaringType);
                ilGenerator.Emit(OpCodes.Ldflda, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9);
              }
              else
              {
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                ilGenerator.Emit(OpCodes.Castclass, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.DeclaringType);
                ilGenerator.Emit(OpCodes.Ldflda, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9);
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
              if (!_param2.LaJjisNd9u(index))
              {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                if (localBuilderArray[index].LocalType.IsValueType)
                  ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
                ilGenerator.Emit(OpCodes.Stelem_Ref);
              }
              else
              {
                OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar doeNeLfV5aXcK5H1Ar = _param2.uJ9jNuhOde(index);
                if (doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.IsStatic)
                {
                  ilGenerator.Emit(OpCodes.Ldarg_1);
                  OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
                  ilGenerator.Emit(OpCodes.Ldsfld, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9);
                  if (doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.FieldType.IsValueType)
                    ilGenerator.Emit(OpCodes.Box, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.FieldType);
                  ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
                else
                {
                  ilGenerator.Emit(OpCodes.Ldarg_1);
                  OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
                  ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                  if (localBuilderArray[index].LocalType.IsValueType)
                    ilGenerator.Emit(OpCodes.Box, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.FieldType);
                  ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
              }
            }
          }
          ilGenerator.Emit(OpCodes.Ret);
          OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr bopLnoZtiN7k24Gr2 = (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr) dynamicMethod.CreateDelegate(typeof (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr));
          if (_param1)
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.schv5EvJl0.Add(_param2, bopLnoZtiN7k24Gr2);
          else
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.Rfevxivn5c.Add(_param2, bopLnoZtiN7k24Gr2);
          return bopLnoZtiN7k24Gr2;
        }
      }

      private static OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr p5CXJfBK9B(
        MethodBase _param0,
        bool _param1,
        OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a _param2)
      {
        lock (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.OEuvFnCu1u)
        {
          OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr bopLnoZtiN7k24Gr1 = (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr) null;
          if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.Qe7v9AGJpX.TryGetValue(_param2, out bopLnoZtiN7k24Gr1))
            return bopLnoZtiN7k24Gr1;
          ConstructorInfo constructorInfo = _param0 as ConstructorInfo;
          DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), new Type[2]
          {
            typeof (object),
            typeof (object[])
          }, typeof (OBqe2IUAeSpOmlOQ4O), true);
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
            localBuilderArray[index] = !_param2.LaJjisNd9u(index) ? ilGenerator.DeclareLocal(typeArray[index]) : ilGenerator.DeclareLocal(typeof (object));
          if (_param0.DeclaringType.IsValueType)
            localBuilderArray[localBuilderArray.Length - 1] = ilGenerator.DeclareLocal(_param0.DeclaringType.MakeByRefType());
          for (int index = 0; index < typeArray.Length; ++index)
          {
            ilGenerator.Emit(OpCodes.Ldarg_1);
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
            ilGenerator.Emit(OpCodes.Ldelem_Ref);
            if (!_param2.LaJjisNd9u(index))
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
            if (!_param2.LaJjisNd9u(index))
            {
              if (parameters[index].ParameterType.IsByRef)
                ilGenerator.Emit(OpCodes.Ldloca_S, localBuilderArray[index]);
              else
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
            }
            else
            {
              OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar doeNeLfV5aXcK5H1Ar = _param2.uJ9jNuhOde(index);
              if (doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.IsStatic)
                ilGenerator.Emit(OpCodes.Ldsflda, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9);
              else if (doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.DeclaringType.IsValueType)
              {
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                ilGenerator.Emit(OpCodes.Unbox, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.DeclaringType);
                ilGenerator.Emit(OpCodes.Ldflda, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9);
              }
              else
              {
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                ilGenerator.Emit(OpCodes.Castclass, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.DeclaringType);
                ilGenerator.Emit(OpCodes.Ldflda, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9);
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
              if (!_param2.LaJjisNd9u(index))
              {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
                ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                if (localBuilderArray[index].LocalType.IsValueType)
                  ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
                ilGenerator.Emit(OpCodes.Stelem_Ref);
              }
              else
              {
                OBqe2IUAeSpOmlOQ4O.yDoeNELfV5aXCK5H1Ar doeNeLfV5aXcK5H1Ar = _param2.uJ9jNuhOde(index);
                if (doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.IsStatic)
                {
                  ilGenerator.Emit(OpCodes.Ldarg_1);
                  OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
                  ilGenerator.Emit(OpCodes.Ldsfld, doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9);
                  if (doeNeLfV5aXcK5H1Ar.Hpjjf13Ho9.FieldType.IsValueType)
                    ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
                  ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
                else
                {
                  ilGenerator.Emit(OpCodes.Ldarg_1);
                  OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.mduX2x281b(ilGenerator, index);
                  ilGenerator.Emit(OpCodes.Ldloc, localBuilderArray[index]);
                  if (localBuilderArray[index].LocalType.IsValueType)
                    ilGenerator.Emit(OpCodes.Box, localBuilderArray[index].LocalType);
                  ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
              }
            }
          }
          ilGenerator.Emit(OpCodes.Ret);
          OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr bopLnoZtiN7k24Gr2 = (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr) dynamicMethod.CreateDelegate(typeof (OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr));
          OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.Qe7v9AGJpX.Add(_param2, bopLnoZtiN7k24Gr2);
          return bopLnoZtiN7k24Gr2;
        }
      }

      private static void mduX2x281b(ILGenerator _param0, int _param1)
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

      private static OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX UaBXcOcakv(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param0)
      {
        if (_param0.gRCuEnOnhD().W6wvnQlCGV())
        {
          object obj1 = _param0.MNddRugcTR((Type) null);
          if (obj1 != null && obj1.GetType().IsEnum)
          {
            Type underlyingType = Enum.GetUnderlyingType(obj1.GetType());
            object obj2 = Convert.ChangeType(obj1, underlyingType);
            OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.tdGXhyXof8(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(underlyingType, obj2));
            if (lxulLsB1AbCdvkPgx != null)
              return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (lxulLsB1AbCdvkPgx as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA);
          }
        }
        return _param0;
      }

      private static OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA tdGXhyXof8(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param0)
      {
        if (!(_param0 is OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA) && _param0.DWVuea1EMj())
          wqpLuhZiq5CncNsA = _param0.gRCuEnOnhD() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA;
        return wqpLuhZiq5CncNsA;
      }

      private static IntPtr bltXeOrHnB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param0)
      {
        if (_param0 == null)
          return IntPtr.Zero;
        if (_param0.LkLvBB03rl())
          return ((OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14) _param0).NrdBVmtKL7();
        if (_param0.DWVuea1EMj())
        {
          OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp fl6HlHf4Wx0BoEfgp = (OBqe2IUAeSpOmlOQ4O.xlFl6HLHf4Wx0BoEfgp) _param0;
          try
          {
            return fl6HlHf4Wx0BoEfgp.BThufJwh7w();
          }
          catch
          {
          }
        }
        object obj = _param0.MNddRugcTR(typeof (IntPtr));
        return obj != null && obj.GetType() == typeof (IntPtr) ? (IntPtr) obj : throw new OBqe2IUAeSpOmlOQ4O.jB7Io5LWnK4vE2iB95i();
      }

      private static object BSmXfQAMYn(object _param0)
      {
        lock (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.xRgv7G3cbx)
        {
          if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.np5vOHNAn0 == null)
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.np5vOHNAn0 = new Dictionary<Type, OBqe2IUAeSpOmlOQ4O.bGs71YLiIW9il9yDL4a>();
          if (_param0 == null)
            return (object) null;
          try
          {
            Type type = _param0.GetType();
            OBqe2IUAeSpOmlOQ4O.bGs71YLiIW9il9yDL4a gs71YliIw9il9yDl4a1;
            if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.np5vOHNAn0.TryGetValue(type, out gs71YliIw9il9yDl4a1))
              return gs71YliIw9il9yDl4a1(_param0);
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), new Type[1]
            {
              typeof (object)
            }, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Unbox_Any, type);
            ilGenerator.Emit(OpCodes.Box, type);
            ilGenerator.Emit(OpCodes.Ret);
            OBqe2IUAeSpOmlOQ4O.bGs71YLiIW9il9yDL4a gs71YliIw9il9yDl4a2 = (OBqe2IUAeSpOmlOQ4O.bGs71YLiIW9il9yDL4a) dynamicMethod.CreateDelegate(typeof (OBqe2IUAeSpOmlOQ4O.bGs71YLiIW9il9yDL4a));
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.np5vOHNAn0.Add(type, gs71YliIw9il9yDl4a2);
            return gs71YliIw9il9yDl4a2(_param0);
          }
          catch
          {
            return (object) null;
          }
        }
      }

      private static void xoaXpMBebu(IntPtr _param0, byte _param1, int _param2)
      {
        lock (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.xRgv7G3cbx)
        {
          if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.vQovAna5lx == null)
          {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (void), new Type[3]
            {
              typeof (IntPtr),
              typeof (byte),
              typeof (int)
            }, typeof (OBqe2IUAeSpOmlOQ4O), true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Initblk);
            ilGenerator.Emit(OpCodes.Ret);
            OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.vQovAna5lx = (OBqe2IUAeSpOmlOQ4O.CyiAdZLCpvyZKhkMmGc) dynamicMethod.CreateDelegate(typeof (OBqe2IUAeSpOmlOQ4O.CyiAdZLCpvyZKhkMmGc));
          }
          OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.vQovAna5lx(_param0, _param1, _param2);
        }
      }

      private static void KbHXNoolqk(IntPtr _param0, IntPtr _param1, uint _param2)
      {
        if (OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.hEbvDetTSD == null)
        {
          DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (void), new Type[3]
          {
            typeof (IntPtr),
            typeof (IntPtr),
            typeof (uint)
          }, typeof (OBqe2IUAeSpOmlOQ4O), true);
          ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
          ilGenerator.Emit(OpCodes.Ldarg_0);
          ilGenerator.Emit(OpCodes.Ldarg_1);
          ilGenerator.Emit(OpCodes.Ldarg_2);
          ilGenerator.Emit(OpCodes.Cpblk);
          ilGenerator.Emit(OpCodes.Ret);
          OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.hEbvDetTSD = (OBqe2IUAeSpOmlOQ4O.g2JyDeLqftkG6b66Zf8) dynamicMethod.CreateDelegate(typeof (OBqe2IUAeSpOmlOQ4O.g2JyDeLqftkG6b66Zf8));
        }
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.hEbvDetTSD(_param0, _param1, _param2);
      }

      public X6oonhL1IpoG9H0Sh52()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.GL6XCXXAY9 = new OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[0];
        this.Ti3XqqGbnS = new OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX[0];
        this.xvIX12K5P4 = new OBqe2IUAeSpOmlOQ4O.g6G7F6LPsY0A2sA8jx9();
        this.mJvXPuZWLG = -1;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static X6oonhL1IpoG9H0Sh52()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.hL4XIw57CM = new object();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.xRkXwNANCW = new Dictionary<object, OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX>();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.tnBXycvSYh = new object();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.UBIXzg2TS3 = new Dictionary<MethodBase, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr>();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.kQfvM9jv6W = new Dictionary<MethodBase, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr>();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.yeHvLKgEPe = new object();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.schv5EvJl0 = new Dictionary<OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr>();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.Rfevxivn5c = new Dictionary<OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr>();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.MTHvairTKI = new object();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.Qe7v9AGJpX = new Dictionary<OBqe2IUAeSpOmlOQ4O.Dhe3JWLp0DcXGE8RZ6a, OBqe2IUAeSpOmlOQ4O.c9qBopLNoZTiN7k24Gr>();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.OEuvFnCu1u = new object();
        OBqe2IUAeSpOmlOQ4O.X6oonhL1IpoG9H0Sh52.xRgv7G3cbx = new object();
      }
    }

    internal enum mP2oxFLYK5cKBXZuceg : byte
    {
    }

    internal enum jpakqYLktEb8W04L94Q : byte
    {
    }

    internal abstract class ntLxulLsB1ABCdvkPGX
    {
      internal OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q tdyvUPSCoA;

      public ntLxulLsB1ABCdvkPGX()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      internal bool W6wvnQlCGV() => this.tdyvUPSCoA == (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 0;

      internal bool glCvoAFEwW() => this.tdyvUPSCoA == (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 1;

      internal bool LkLvBB03rl()
      {
        return this.tdyvUPSCoA == (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 3 || this.tdyvUPSCoA == (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 4;
      }

      internal bool E4wvjiQ2aY() => this.tdyvUPSCoA == (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 2;

      internal bool YVbv0y1oss() => this.tdyvUPSCoA == (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 5;

      internal bool nGhvlaVjva() => this.tdyvUPSCoA == (OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 6;

      internal virtual bool DWVuea1EMj() => false;

      internal virtual bool lqfucMK0hV() => false;

      internal abstract void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      internal virtual bool gyIuhELnxq() => false;

      internal ntLxulLsB1ABCdvkPGX(OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.tdyvUPSCoA = _param1;
      }

      internal abstract object MNddRugcTR(Type _param1);

      internal abstract bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      internal abstract bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      internal abstract bool V1kdEyl02V();

      internal abstract OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD();

      internal virtual bool gKku4OTsTL() => false;

      internal abstract void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1);

      internal static OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK hewvXrsnJ8(Type _param0)
      {
        Type nullableType = _param0;
        if (!(nullableType != (Type) null))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 18;
        if (nullableType.IsByRef)
          nullableType = nullableType.GetElementType();
        if (nullableType != (Type) null && Nullable.GetUnderlyingType(nullableType) != (Type) null)
          nullableType = Nullable.GetUnderlyingType(nullableType);
        if (nullableType == typeof (string))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 14;
        if (nullableType == typeof (byte))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 2;
        if (nullableType == typeof (sbyte))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 1;
        if (nullableType == typeof (short))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 3;
        if (nullableType == typeof (ushort))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 4;
        if (nullableType == typeof (int))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 5;
        if (nullableType == typeof (uint))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 6;
        if (nullableType == typeof (long))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 7;
        if (nullableType == typeof (ulong))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 8;
        if (nullableType == typeof (float))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 9;
        if (nullableType == typeof (double))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 10;
        if (nullableType == typeof (bool))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11;
        if (nullableType == typeof (IntPtr))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 12;
        if (nullableType == typeof (UIntPtr))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 13;
        if (nullableType == typeof (char))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15;
        if (nullableType == typeof (object))
          return (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 0;
        return nullableType.IsEnum ? (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 16 : (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 17;
      }

      internal static OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX UUIvvnLNCA(
        Type _param0,
        object _param1)
      {
        OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK miegr9LeMaB3mn2S8kK1 = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.hewvXrsnJ8(_param0);
        OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK miegr9LeMaB3mn2S8kK2 = (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 18;
        if (_param1 != null)
          miegr9LeMaB3mn2S8kK2 = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.hewvXrsnJ8(_param1.GetType());
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) null;
        switch (miegr9LeMaB3mn2S8kK1)
        {
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 0:
            lxulLsB1AbCdvkPgx = miegr9LeMaB3mn2S8kK2 != (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15 ? OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.CFovdms9ca(_param1) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB(_param1);
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 1:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 1:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (sbyte) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 2:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (sbyte) (byte) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = !(bool) _param1 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (sbyte) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 1);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 2:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 1:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (byte) (sbyte) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 2:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (byte) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = !(bool) _param1 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (byte) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 2);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 3:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 3:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (short) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = !(bool) _param1 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (short) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 3);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 4:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 4:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (ushort) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = !(bool) _param1 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 4);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 5:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 5:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = !(bool) _param1 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 5);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 6:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 6:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = !(bool) _param1 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(0U, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(1U, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 6);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 7:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 7:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((long) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = !(bool) _param1 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(0L, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(1L, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((long) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 7);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 8:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 8:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((ulong) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = !(bool) _param1 ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(0UL, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8) : (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p(1UL, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.YaeDDELdekEtZ4qXJ1p((ulong) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 8);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 9:
            if (miegr9LeMaB3mn2S8kK2 != (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 9)
              throw new InvalidCastException();
            lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((float) _param1);
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 10:
            if (miegr9LeMaB3mn2S8kK2 != (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 10)
              throw new InvalidCastException();
            lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.j5UMoXLRwIcnpEjt1UJ((double) _param1);
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 1:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((sbyte) _param1 != (sbyte) 0);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 2:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((byte) _param1 > (byte) 0);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 3:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((short) _param1 != (short) 0);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 4:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((ushort) _param1 > (ushort) 0);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 5:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) _param1 != 0);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 6:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((uint) _param1 > 0U);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 7:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((long) _param1 != 0L);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 8:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((ulong) _param1 > 0UL);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 9:
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 10:
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 12:
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 13:
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 14:
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 16:
                throw new InvalidCastException();
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 11:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((bool) _param1);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 18:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(false);
                break;
              default:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj(_param1 != null);
                break;
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 12:
            if (miegr9LeMaB3mn2S8kK2 != (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 12)
              throw new InvalidCastException();
            lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((IntPtr) _param1);
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 13:
            if (miegr9LeMaB3mn2S8kK2 != (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 13)
              throw new InvalidCastException();
            lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.CydMXSLujvvk8Y3dM14((UIntPtr) _param1);
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 14:
            lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.iGQH0gLZd16LDXi2iXN(_param1 as string);
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
            switch (miegr9LeMaB3mn2S8kK2)
            {
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 1:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (sbyte) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 2:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (byte) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 3:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (short) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 4:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (ushort) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 5:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 6:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (uint) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
                break;
              case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 15:
                lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.bTUKqtLXplwjOaJo6Hj((int) (char) _param1, (OBqe2IUAeSpOmlOQ4O.iQ4HZQLQyLCdABwFXsw) 15);
                break;
              default:
                throw new InvalidCastException();
            }
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 16:
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 17:
            lxulLsB1AbCdvkPgx = OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.CFovdms9ca(_param1);
            break;
          case (OBqe2IUAeSpOmlOQ4O.MIegr9LEMaB3mn2S8kK) 18:
            throw new InvalidCastException();
        }
        if (_param0.IsByRef)
          lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.R3Oma8LtRwKkrYXOrgs(lxulLsB1AbCdvkPgx, _param0.GetElementType());
        return lxulLsB1AbCdvkPgx;
      }

      private static OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX CFovdms9ca(object _param0)
      {
        if (_param0 != null && _param0.GetType().IsEnum)
        {
          Type underlyingType = Enum.GetUnderlyingType(_param0.GetType());
          object obj = Convert.ChangeType(_param0, underlyingType);
          OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.wxXvux1dbZ(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX.UUIvvnLNCA(underlyingType, obj));
          if (lxulLsB1AbCdvkPgx != null)
            return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) (lxulLsB1AbCdvkPgx as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA);
        }
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) new OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB(_param0);
      }

      private static OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wxXvux1dbZ(
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param0)
      {
        if (!(_param0 is OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA wqpLuhZiq5CncNsA) && _param0.DWVuea1EMj())
          wqpLuhZiq5CncNsA = _param0.gRCuEnOnhD() as OBqe2IUAeSpOmlOQ4O.pcfWqpLUHZiq5CNCNsA;
        return wqpLuhZiq5CncNsA;
      }
    }

    private class VagEUnLrg8uVZICmTDB : OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX
    {
      public object RJYvRBRDtP;
      public Type jyuvQjLcik;

      public VagEUnLrg8uVZICmTDB()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        this.\u002Ector((object) null);
      }

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        if (_param1 is OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB)
        {
          this.RJYvRBRDtP = ((OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB) _param1).RJYvRBRDtP;
          this.jyuvQjLcik = ((OBqe2IUAeSpOmlOQ4O.VagEUnLrg8uVZICmTDB) _param1).jyuvQjLcik;
        }
        else
          this.RJYvRBRDtP = (object) _param1.gRCuEnOnhD();
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.nOQdl4ODOg(_param1);
      }

      public VagEUnLrg8uVZICmTDB(object _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector((OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 0);
        this.RJYvRBRDtP = _param1;
        this.jyuvQjLcik = (Type) null;
      }

      public VagEUnLrg8uVZICmTDB(object _param1, Type _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector((OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 0);
        this.RJYvRBRDtP = _param1;
        this.jyuvQjLcik = _param2;
      }

      public override string ToString()
      {
        return this.RJYvRBRDtP != null ? this.RJYvRBRDtP.ToString() : ((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 5).ToString();
      }

      internal override object MNddRugcTR(Type _param1)
      {
        if (this.RJYvRBRDtP == null)
          return (object) null;
        if (_param1 != (Type) null && _param1.IsByRef)
          _param1 = _param1.GetElementType();
        if (!(this.RJYvRBRDtP is OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX))
        {
          object obj = this.RJYvRBRDtP;
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
        if (this.jyuvQjLcik != (Type) null)
          return ((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.RJYvRBRDtP).MNddRugcTR(this.jyuvQjLcik);
        object obj1 = ((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this.RJYvRBRDtP).MNddRugcTR(_param1);
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

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        return _param1.DWVuea1EMj() ? _param1.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : this.MNddRugcTR((Type) null) == _param1.MNddRugcTR((Type) null);
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        return _param1.DWVuea1EMj() ? _param1.BeouTiljCp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : this.MNddRugcTR((Type) null) != _param1.MNddRugcTR((Type) null);
      }

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return !(this.RJYvRBRDtP is OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX rjYvRbrDtP) ? (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this : rjYvRbrDtP.gRCuEnOnhD();
      }

      internal override bool V1kdEyl02V()
      {
        return this.RJYvRBRDtP != null && (!(this.RJYvRBRDtP is OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX rjYvRbrDtP) || rjYvRbrDtP.MNddRugcTR((Type) null) != null);
      }
    }

    private class iGQH0gLZd16LDXi2iXN : OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX
    {
      public string SgfvEZDVji;

      public iGQH0gLZd16LDXi2iXN(string _param1)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector((OBqe2IUAeSpOmlOQ4O.jpakqYLktEb8W04L94Q) 6);
        this.SgfvEZDVji = _param1;
      }

      internal override void nOQdl4ODOg(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.SgfvEZDVji = ((OBqe2IUAeSpOmlOQ4O.iGQH0gLZd16LDXi2iXN) _param1).SgfvEZDVji;
      }

      internal override void tY3dXGtH5f(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.nOQdl4ODOg(_param1);
      }

      public override string ToString()
      {
        if (this.SgfvEZDVji == null)
          return ((OBqe2IUAeSpOmlOQ4O.V6VagRLKaUE7SYvTS0L) 5).ToString();
        char ch = '*';
        string str1 = ch.ToString();
        string sgfvEzdVji = this.SgfvEZDVji;
        ch = '*';
        string str2 = ch.ToString();
        return str1 + sgfvEzdVji + str2;
      }

      internal override bool V1kdEyl02V() => this.SgfvEZDVji != null;

      internal override object MNddRugcTR(Type _param1) => (object) this.SgfvEZDVji;

      internal override bool SHouWnYaPf(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        return _param1.DWVuea1EMj() ? _param1.SHouWnYaPf((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : (object) this.SgfvEZDVji == _param1.MNddRugcTR((Type) null);
      }

      internal override bool BeouTiljCp(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        return _param1.DWVuea1EMj() ? _param1.BeouTiljCp((OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this) : (object) this.SgfvEZDVji != _param1.MNddRugcTR((Type) null);
      }

      internal override OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX gRCuEnOnhD()
      {
        return (OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX) this;
      }
    }

    internal class g6G7F6LPsY0A2sA8jx9
    {
      private List<OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX> aKuvmhnH37;

      [SpecialName]
      public int hMWvSoG03q() => this.aKuvmhnH37.Count;

      public void OyDv4akGg5() => this.aKuvmhnH37.Clear();

      public void DjWvWsmuSB(OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX _param1)
      {
        this.aKuvmhnH37.Add(_param1);
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX HtMvTddAIt()
      {
        return this.aKuvmhnH37[this.aKuvmhnH37.Count - 1];
      }

      public OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX CmTvHQWOmB()
      {
        OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX lxulLsB1AbCdvkPgx = this.HtMvTddAIt();
        if (this.aKuvmhnH37.Count == 0)
          return lxulLsB1AbCdvkPgx;
        this.aKuvmhnH37.RemoveAt(this.aKuvmhnH37.Count - 1);
        return lxulLsB1AbCdvkPgx;
      }

      public g6G7F6LPsY0A2sA8jx9()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.aKuvmhnH37 = new List<OBqe2IUAeSpOmlOQ4O.ntLxulLsB1ABCdvkPGX>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private struct d8DC64911393CA78
    {
      private StringBuilder sb;

      public d8DC64911393CA78(int _param1, int _param2)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.sb = new StringBuilder();
      }

      public d8DC64911393CA78(int _param1, int _param2, IFormatProvider _param3)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
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

    internal enum V6VagRLKaUE7SYvTS0L
    {
    }
  }
}
