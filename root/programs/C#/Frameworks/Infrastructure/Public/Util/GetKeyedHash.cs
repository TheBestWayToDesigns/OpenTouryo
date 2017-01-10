//**********************************************************************************
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
//* クラス名        ：GetKeyedHash
//* クラス日本語名  ：ハッシュ（キー付き）を取得するクラス
//*
//* 作成者          ：生技 西野
//* 更新履歴        ：
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  2013/02/15  西野  大介        新規作成
//*  2014/03/13  西野  大介        devps(1703):Createメソッドを使用してcryptoオブジェクトを作成します。
//*  2014/03/13  西野  大介        devps(1725):暗号クラスの使用終了時にデータをクリアする。
//*  2016/01/10  西野  大介        HMAC(HMACMD5、HMACRIPEMD160、HMACSHA256、HMACSHA384、HMACSHA512)を追加
//*  2016/01/10  西野  大介        ストレッチ回数とsaltのpublic property procedureを追加
//*  2016/01/10  西野  大介        全てHMACSHA1になる問題があったため、KeyedHashAlgorithm生成方法を変更。
//**********************************************************************************

// System
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections;

// 業務フレームワーク（循環参照になるため、参照しない）
// フレームワーク（循環参照になるため、参照しない）

// 部品
using Touryo.Infrastructure.Public.Db;
using Touryo.Infrastructure.Public.IO;
using Touryo.Infrastructure.Public.Log;
using Touryo.Infrastructure.Public.Str;
using Touryo.Infrastructure.Public.Util;

using System.Security.Cryptography;

namespace Touryo.Infrastructure.Public.Util
{
    /// <summary>
    /// ハッシュ（キー付き）アルゴリズムのサービスプロバイダの種類
    /// </summary>
    public enum EnumKeyedHashAlgorithm
    {
        /// <summary>Default</summary>
        Default,

        /// <summary>HMACSHA1</summary>
        HMACSHA1,

        /// <summary>HMACMD5</summary>
        HMACMD5,

        /// <summary>HMACRIPEMD160</summary>
        HMACRIPEMD160,

        /// <summary>HMACSHA256</summary>
        HMACSHA256,

        /// <summary>HMACSHA384</summary>
        HMACSHA384,

        /// <summary>HMACSHA512</summary>
        HMACSHA512,

        /// <summary>MACTripleDES</summary>
        MACTripleDES
    };

    /// <summary>ハッシュ（キー付き）を取得するクラス</summary>
    public class GetKeyedHash
    {
        /// <summary>ソルト</summary>
        private static string _salt = "Touryo.Infrastructure.Public.Util.GetKeyedHash.Salt";

        /// <summary>ソルト</summary>
        public static string Salt
        {
            get { return GetKeyedHash._salt; }
            set { GetKeyedHash._salt = value; }
        }

        /// <summary>ストレッチ回数</summary>
        private static int _stretching = 1000;

        /// <summary>ストレッチ回数</summary>
        public static int Stretching
        {
            get { return GetKeyedHash._stretching; }
            set { GetKeyedHash._stretching = value; }
        }

        #region GetKeyedHashString

        /// <summary>文字列のハッシュ値を計算して返す。</summary>
        /// <param name="ss">文字列</param>
        /// <param name="ekha">ハッシュ（キー付き）アルゴリズム列挙型</param>
        /// <param name="password">使用するパスワード</param>
        /// <returns>ハッシュ値（文字列）</returns>
        public static string GetKeyedHashString(string ss, EnumKeyedHashAlgorithm ekha, string password)
        {
            return GetKeyedHash.GetKeyedHashString(
                ss, ekha, password, CustomEncode.StringToByte(GetKeyedHash._salt, CustomEncode.UTF_8), GetKeyedHash.Stretching);
        }

        /// <summary>文字列のハッシュ値を計算して返す。</summary>
        /// <param name="ss">文字列</param>
        /// <param name="ekha">ハッシュ（キー付き）アルゴリズム列挙型</param>
        /// <param name="password">使用するパスワード</param>
        /// <param name="salt">ソルト</param>
        /// <returns>ハッシュ値（文字列）</returns>
        public static string GetKeyedHashString(string ss, EnumKeyedHashAlgorithm ekha, string password, byte[] salt)
        {
            return GetKeyedHash.GetKeyedHashString(
                ss, ekha, password, salt, GetKeyedHash.Stretching);
        }

        /// <summary>文字列のハッシュ値を計算して返す。</summary>
        /// <param name="ss">文字列</param>
        /// <param name="ekha">ハッシュ（キー付き）アルゴリズム列挙型</param>
        /// <param name="password">使用するパスワード</param>
        /// <param name="salt">ソルト</param>
        /// <param name="stretching">ストレッチング</param>
        /// <returns>ハッシュ値（文字列）</returns>
        public static string GetKeyedHashString(string ss, EnumKeyedHashAlgorithm ekha, string password, byte[] salt, int stretching)
        {
            // ハッシュ（Base64）
            return CustomEncode.ToBase64String(
                GetKeyedHashBytes(CustomEncode.StringToByte(ss, CustomEncode.UTF_8), ekha, password, salt, stretching));
        }

        #endregion

        #region GetKeyedHashBytes

        /// <summary>バイト配列のハッシュ値を計算して返す。</summary>
        /// <param name="asb">バイト配列</param>
        /// <param name="ekha">ハッシュ（キー付き）アルゴリズム列挙型</param>
        /// <param name="password">使用するパスワード</param>
        /// <returns>ハッシュ値（バイト配列）</returns>
        public static byte[] GetKeyedHashBytes(byte[] asb, EnumKeyedHashAlgorithm ekha, string password)
        {
            return GetKeyedHash.GetKeyedHashBytes(
                asb, ekha, password, CustomEncode.StringToByte(GetKeyedHash._salt, CustomEncode.UTF_8), GetKeyedHash.Stretching);
        }

        /// <summary>バイト配列のハッシュ値を計算して返す。</summary>
        /// <param name="asb">バイト配列</param>
        /// <param name="ekha">ハッシュ（キー付き）アルゴリズム列挙型</param>
        /// <param name="password">使用するパスワード</param>
        /// <param name="salt">ソルト</param>
        /// <returns>ハッシュ値（バイト配列）</returns>
        public static byte[] GetKeyedHashBytes(byte[] asb, EnumKeyedHashAlgorithm ekha, string password, byte[] salt)
        {
            return GetKeyedHash.GetKeyedHashBytes(
                asb, ekha, password, salt, GetKeyedHash.Stretching);
        }

        /// <summary>バイト配列のハッシュ値を計算して返す。</summary>
        /// <param name="asb">バイト配列</param>
        /// <param name="ekha">ハッシュ（キー付き）アルゴリズム列挙型</param>
        /// <param name="password">使用するパスワード</param>
        /// <param name="salt">ソルト</param>
        /// <param name="stretching">ストレッチング</param>
        /// <returns>ハッシュ値（バイト配列）</returns>
        public static byte[] GetKeyedHashBytes(byte[] asb, EnumKeyedHashAlgorithm ekha, string password, byte[] salt, int stretching)
        {
            // キーを生成する。
            Rfc2898DeriveBytes passwordKey = new Rfc2898DeriveBytes(password, salt, stretching);
            // HMACMD5      ：どのサイズのキーでも受け入れる ◯
            // HMACRIPEMD160：どのサイズのキーでも受け入れる ◯
            // HMACSHA1     ：どのサイズのキーでも受け入れる ◯
            // HMACSHA256   ：どのサイズのキーでも受け入れる ◯
            // HMACSHA384   ：どのサイズのキーでも受け入れる ◯
            // HMACSHA512   ：どのサイズのキーでも受け入れる ◯
            // MACTripleDES ：長さが 16 または 24 バイトのキーを受け入れる

            // ハッシュ（キー付き）サービスプロバイダを生成
            KeyedHashAlgorithm kha = GetKeyedHash.CreateKeyedHashAlgorithmServiceProvider(
                    ekha,
                    passwordKey.GetBytes(24) // 上記より、24 決め打ちとする。
                );

            // ハッシュ（キー付き）を生成して返す。
            byte[] temp = kha.ComputeHash(asb);

            kha.Clear(); // devps(1725)

            return temp;
        }

        #endregion

        #region 内部関数

        /// <summary>ハッシュ（キー付き）サービスプロバイダの生成</summary>
        /// <param name="ekha">ハッシュ（キー付き）サービスプロバイダの列挙型</param>
        /// <param name="key">キー</param>
        /// <returns>ハッシュ（キー付き）サービスプロバイダ</returns>
        private static KeyedHashAlgorithm CreateKeyedHashAlgorithmServiceProvider(EnumKeyedHashAlgorithm ekha, byte[] key)
        {
            // ハッシュ（キー付き）サービスプロバイダ
            KeyedHashAlgorithm kha = null;

            // HMACSHA1.Create(); だと、全部、HMACSHA1になってしまう現象があったので、
            // 全部、= new HMACSHA1(key); のスタイルに変更した。

            if (ekha == EnumKeyedHashAlgorithm.Default)
            {
                // 既定の暗号化サービスプロバイダ
                kha = new HMACSHA1(key); // devps(1703)
            }
            else if (ekha == EnumKeyedHashAlgorithm.HMACSHA1)
            {
                // HMACSHA1サービスプロバイダ
                kha = new HMACSHA1(key); // devps(1703)
            }
            // -- ▼追加▼ --
            else if (ekha == EnumKeyedHashAlgorithm.HMACMD5)
            {
                // HMACMD5サービスプロバイダ
                kha = new HMACMD5(key);
            }
            else if (ekha == EnumKeyedHashAlgorithm.HMACRIPEMD160)
            {
                // HMACRIPEMD160サービスプロバイダ
                kha = new HMACRIPEMD160(key);
            }
            else if (ekha == EnumKeyedHashAlgorithm.HMACSHA256)
            {
                // HMACSHA256サービスプロバイダ
                kha = new HMACSHA256(key);
            }
            else if (ekha == EnumKeyedHashAlgorithm.HMACSHA384)
            {
                // HMACSHA384サービスプロバイダ
                kha = new HMACSHA384(key);
            }
            else if (ekha == EnumKeyedHashAlgorithm.HMACSHA512)
            {
                // HMACSHA512サービスプロバイダ
                kha = new HMACSHA512(key);
            }
            // -- ▲追加▲ --
            else if (ekha == EnumKeyedHashAlgorithm.MACTripleDES)
            {
                // MACTripleDESサービスプロバイダ
                kha = new MACTripleDES(key); // devps(1703)
            }

            return kha;
        }

        #endregion
    }
}
