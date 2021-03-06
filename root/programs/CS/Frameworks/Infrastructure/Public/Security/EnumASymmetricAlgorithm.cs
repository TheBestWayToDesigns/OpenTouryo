﻿//**********************************************************************************
//* Copyright (C) 2007,2016 Hitachi Solutions,Ltd.
//**********************************************************************************

#region Apache License
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

//**********************************************************************************
//* クラス名        ：EnumASymmetricAlgorithm
//* クラス日本語名  ：非対称アルゴリズムによる暗号化の列挙型
//*
//* 作成者          ：生技 西野
//* 更新履歴        ：
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  2018/10/30  西野 大介         新規作成（分離）
//*  2018/11/09  西野 大介         RSAOpenSsl、DSAOpenSsl、HashAlgorithmName対応
//**********************************************************************************

#region ECDH
// Elliptic Curve Diffie-Hellman – .NET Security Blog
// https://blogs.msdn.microsoft.com/shawnfa/2007/01/22/elliptic-curve-diffie-hellman/
// 楕円曲線ディフィー・ヘルマン鍵共有 - Wikipedia
// https://ja.wikipedia.org/wiki/楕円曲線ディフィー・ヘルマン鍵共有
// アリスとボブ - Wikipedia
// https://ja.wikipedia.org/wiki/アリスとボブ
#endregion

namespace Touryo.Infrastructure.Public.Security
{
    /// <summary>
    /// 非対称アルゴリズムによる暗号化の列挙型
    /// </summary>
    public enum EnumASymmetricAlgorithm
    {
        #region CSP (CryptoServiceProvider)

        /// <summary>RSACryptoServiceProvider</summary>
        RsaCsp,

        #endregion

        #region CNG (CryptographyNextGeneration)

        /// <summary>RSACryptographyNextGeneration</summary>
        RsaCng,

        ///// <summary>ECDiffieHellmanCryptographyNextGeneration</summary>
        //EcdhCng, // 暗号化用ではない

        #endregion

        #region OpenSsl
#if NETSTD
        /// <summary>RSAOpenSsl</summary>
        RsaOpenSsl,

        ///// <summary>ECDiffieHellmanOpenSsl</summary>
        //EcdhOpenSsl, // 暗号化用ではない
#endif
        #endregion
    }
}
