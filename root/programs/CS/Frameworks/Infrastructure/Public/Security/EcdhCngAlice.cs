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
//* クラス名        ：EcdhCngAlice
//* クラス日本語名  ：EcdhCngの「Aliceクラス」
//*
//* 作成者          ：生技 西野
//* 更新履歴        ：
//* 
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  2018/10/30  西野 大介         新規作成
//*  2018/11/09  西野 大介         RSAOpenSsl、DSAOpenSsl、HashAlgorithmName対応
//**********************************************************************************

using System.Security.Cryptography;

namespace Touryo.Infrastructure.Public.Security
{
    /// <summary>EcdhCngの「Aliceクラス」</summary>
    public class EcdhCngAlice : EcdhKeyExchange
    {
        #region constructor
        /// <summary>constructor</summary>
        public EcdhCngAlice()
            : this(ECDiffieHellmanKeyDerivationFunction.Hash, CngAlgorithm.Sha256, null, null, null) { }

        /// <summary>constructor</summary>
        /// <param name="func">キー派生関数（Bob側と合わせる）。</param>
        /// <param name="hash">秘密協定の処理に使用するハッシュ アルゴリズム（Bob側と合わせる）</param>
        public EcdhCngAlice(ECDiffieHellmanKeyDerivationFunction func, CngAlgorithm hash)
            : this(func, hash, null, null, null) { }

        /// <summary>constructor</summary>
        /// <param name="func">キー派生関数（Bob側と合わせる）。</param>
        /// <param name="hash">秘密協定の処理に使用するハッシュ アルゴリズム（Bob側と合わせる）</param>
        /// <param name="hmacKey">HMACキー</param>
        public EcdhCngAlice(ECDiffieHellmanKeyDerivationFunction func, CngAlgorithm hash, byte[] hmacKey)
            : this(func, hash, hmacKey, null, null) { }

        /// <summary>constructor</summary>
        /// <param name="func">キー派生関数（Bob側と合わせる）。</param>
        /// <param name="hash">秘密協定の処理に使用するハッシュ アルゴリズム（Bob側と合わせる）</param>
        /// <param name="hmacKey">HMACキー</param>
        /// <param name="secretPrependOrLabel">SecretPrepend or Label</param>
        /// <param name="secretAppendOrSeed">SecretAppend or Seed</param>
        public EcdhCngAlice(ECDiffieHellmanKeyDerivationFunction func, CngAlgorithm hash,
            byte[] hmacKey, byte[] secretPrependOrLabel, byte[] secretAppendOrSeed)
        {

            ECDiffieHellmanCng ecdh = new ECDiffieHellmanCng();
            this._asa = ecdh; // 保持

            // Bob側と合わせる。
            ecdh.KeyDerivationFunction = func;
            if (func == ECDiffieHellmanKeyDerivationFunction.Hash)
            {
                ecdh.HashAlgorithm = hash;
                ecdh.SecretPrepend = secretPrependOrLabel;
                ecdh.SecretAppend = secretAppendOrSeed;
            }
            else if (func == ECDiffieHellmanKeyDerivationFunction.Hmac)
            {
                ecdh.HashAlgorithm = hash;
                ecdh.HmacKey = hmacKey;
                ecdh.SecretPrepend = secretPrependOrLabel;
                ecdh.SecretAppend = secretAppendOrSeed;
            }
            else if (func == ECDiffieHellmanKeyDerivationFunction.Tls)
            {
                ecdh.Label = secretPrependOrLabel;
                ecdh.Seed = secretAppendOrSeed;
            }

            // Bobと鍵交換する交換鍵
            this._exchangeKey = ecdh.PublicKey.ToByteArray();
        }
        #endregion

        /// <summary>AesCryptoServiceProviderを生成する。</summary>
        public void CreateAesSP()
        {
            this._aes = new AesCryptoServiceProvider();
            this._aes.Key = this._privateKey;
        }
    }
}
