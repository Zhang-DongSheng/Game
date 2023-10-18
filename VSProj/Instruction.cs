/*
 * Tencent is pleased to support the open source community by making InjectFix available.
 * Copyright (C) 2019 THL A29 Limited, a Tencent company.  All rights reserved.
 * InjectFix is licensed under the MIT License, except for the third-party components listed in the file 'LICENSE' which may be subject to their corresponding license terms. 
 * This file is subject to the terms and conditions defined in file 'LICENSE', which is part of this source code package.
 */

namespace IFix.Core
{
    public enum Code
    {
        Localloc,
        Brfalse,
        Cgt_Un,
        Throw,
        Stind_I8,
        Conv_R8,
        Not,
        Conv_U4,
        Ldarg,
        Ldind_R4,
        Conv_Ovf_U4_Un,
        Add,
        Unbox,
        Ldtype, // custom
        Conv_Ovf_I1_Un,
        Mul_Ovf,
        Conv_Ovf_U1_Un,
        Conv_Ovf_I2,
        Xor,
        Neg,
        Conv_Ovf_I_Un,
        Ldsfld,
        Or,
        Stind_I4,
        Conv_R_Un,
        Ldind_I4,
        Bgt,
        Ldelem_U4,
        Leave,
        Stelem_Any,
        Isinst,
        Conv_Ovf_U_Un,
        Ldarga,
        Br,
        Conv_Ovf_I8_Un,
        Shr_Un,
        Newobj,
        Pop,
        Bne_Un,
        Castclass,
        Conv_U,
        Readonly,
        Stind_I1,
        Ckfinite,
        Refanytype,
        Ldstr,
        Ldloc,
        Ldc_I8,

        //Pseudo instruction
        StackSpace,
        Conv_Ovf_U1,
        Ldobj,
        Cgt,
        Ceq,
        //Calli,
        Bgt_Un,
        Cpblk,
        Cpobj,
        Ble,
        Stelem_I2,
        Initobj,
        Mkrefany,
        Initblk,
        Ldc_R8,
        Constrained,
        Brtrue,
        Ldind_I,
        Ret,
        Ldlen,
        Conv_I4,
        Ldnull,
        Stind_R4,
        Call,
        Stind_Ref,
        CallExtern,
        Break,
        Beq,
        Ldelem_Any,
        Conv_I8,
        Arglist,
        No,
        Shr,
        Callvirt,
        Bge_Un,
        Stelem_R8,
        Sub,
        Ldc_I4,
        Div,
        Ldelem_U2,
        Ldelem_I2,
        Conv_Ovf_U2_Un,
        Conv_I1,
        Stelem_I8,
        Conv_Ovf_I4,
        Ldind_I2,
        Ldelem_I4,
        Stelem_I1,
        Endfilter,
        Conv_U1,
        Ldsflda,
        Conv_Ovf_I1,
        Blt_Un,
        Ldflda,
        Jmp,
        Endfinally,
        Bge,
        Stelem_I4,
        Conv_Ovf_U4,
        Unbox_Any,
        Ldfld,
        Starg,
        Ldelem_Ref,
        Ldind_I8,
        Clt,
        Conv_R4,
        Box,
        Ldelem_U1,
        Conv_Ovf_U8_Un,
        Conv_Ovf_I2_Un,
        Ldind_R8,
        Ldind_U1,
        Clt_Un,
        Conv_U8,
        Stind_I,
        Mul_Ovf_Un,
        Stelem_Ref,
        Conv_Ovf_I8,
        Dup,
        Conv_I2,
        Add_Ovf_Un,
        Ldelem_I1,
        Conv_Ovf_I,
        Ldind_U4,
        Conv_Ovf_U2,
        Unaligned,
        Div_Un,
        Conv_Ovf_I4_Un,
        Stelem_I,
        Ble_Un,
        Sub_Ovf_Un,
        Volatile,
        Rem,
        Ldelema,
        Ldc_R4,
        Ldvirtftn,
        Ldelem_R8,
        Stelem_R4,
        Stind_I2,
        Newarr,
        Refanyval,
        Ldloca,
        Conv_Ovf_U,
        Add_Ovf,
        Ldind_Ref,
        Switch,
        Ldelem_R4,
        Ldelem_I,
        Stsfld,
        Stloc,
        Newanon,
        Callvirtvirt,
        Ldtoken,
        Ldftn,
        Stobj,
        Tail,
        Stind_R8,
        Ldind_I1,
        Ldind_U2,
        Blt,
        And,
        Conv_U2,
        Nop,
        Conv_I,
        Conv_Ovf_U8,
        Ldelem_I8,
        Stfld,
        Sub_Ovf,
        Rethrow,
        Shl,
        Mul,
        Rem_Un,
        Ldvirtftn2,
        Sizeof,
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Instruction
    {
        /// <summary>
        /// 指令MAGIC
        /// </summary>
        public const ulong INSTRUCTION_FORMAT_MAGIC = 1137195465085157839;

        /// <summary>
        /// 当前指令
        /// </summary>
        public Code Code;

        /// <summary>
        /// 操作数
        /// </summary>
        public int Operand;
    }

    public enum ExceptionHandlerType
    {
        Catch = 0,
        Filter = 1,
        Finally = 2,
        Fault = 4
    }

    public sealed class ExceptionHandler
    {
        public System.Type CatchType;
        public int CatchTypeId;
        public int HandlerEnd;
        public int HandlerStart;
        public ExceptionHandlerType HandlerType;
        public int TryEnd;
        public int TryStart;
    }
}