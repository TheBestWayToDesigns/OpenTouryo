﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

// https://www.nuget.org/packages/jose-jwt/
using Jose;
// https://github.com/dvsekhvalnov/jose-jwt/tree/master/jose-jwt/Security/Cryptography
using Security.Cryptography;

using Touryo.Infrastructure.Public.Dbg;
using Touryo.Infrastructure.Public.Str;
using Touryo.Infrastructure.Public.Security;

namespace EncAndDecUtilCUI
{
    /// <summary>
    /// - jose-jwt - マイクロソフト系技術情報 Wiki
    ///   https://techinfoofmicrosofttech.osscons.jp/index.php?jose-jwt
    /// - Certificates
    ///   https://github.com/OpenTouryoProject/OpenTouryo/tree/develop/root/files/resource/X509
    /// </summary>
    public class Program
    {
        private static string PfxPassword = "test";

        public static void Main(string[] args)
        {
            try
            {
                // Hash
                Program.Hash();
                Program.KeyedHash();
                Program.PasswordHash();

                WriteLine.OutPutDebugAndConsole("----------------------------------------------------------------------------------------------------");

                // Cryptography
                Program.PrivateKeyCryptography();
                Program.PublicKeyCryptography();

                WriteLine.OutPutDebugAndConsole("----------------------------------------------------------------------------------------------------");

                // Other
                Program.MyJwt();
                Program.JoseJwt();
            }
            catch (Exception ex)
            {
                WriteLine.OutPutDebugAndConsole(ex.ToString());
            }

            Console.ReadKey();
        }

        #region Test

        #region Hash

        /// <summary>Hash</summary>
        private static void Hash()
        {
            string data = "ynyKeiR9FXWPkNQHvUPZkAlfUmouExBv";

            WriteLine.OutPutDebugAndConsole("HashAlgorithm.Default", GetHash.GetHashString(data, EnumHashAlgorithm.Default));

            WriteLine.OutPutDebugAndConsole("HashAlgorithm.MD5_CSP", GetHash.GetHashString(data, EnumHashAlgorithm.MD5_CSP));

            WriteLine.OutPutDebugAndConsole("HashAlgorithm.RIPEMD160_M", GetHash.GetHashString(data, EnumHashAlgorithm.RIPEMD160_M));

            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA1_CSP", GetHash.GetHashString(data, EnumHashAlgorithm.SHA1_CSP));
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA1_M", GetHash.GetHashString(data, EnumHashAlgorithm.SHA1_M));

            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA256_CSP", GetHash.GetHashString(data, EnumHashAlgorithm.SHA256_CSP));
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA256_M", GetHash.GetHashString(data, EnumHashAlgorithm.SHA256_M));

            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA384_CSP", GetHash.GetHashString(data, EnumHashAlgorithm.SHA384_CSP));
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA384_M", GetHash.GetHashString(data, EnumHashAlgorithm.SHA384_M));

            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA512_CSP", GetHash.GetHashString(data, EnumHashAlgorithm.SHA512_CSP));
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA512_M", GetHash.GetHashString(data, EnumHashAlgorithm.SHA512_M));

#if NETCORE
#else
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.MD5_CNG", GetHash.GetHashString(data, EnumHashAlgorithm.MD5_CNG));
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA1_CNG", GetHash.GetHashString(data, EnumHashAlgorithm.SHA1_CNG));
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA256_CNG", GetHash.GetHashString(data, EnumHashAlgorithm.SHA256_CNG));
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA384_CNG", GetHash.GetHashString(data, EnumHashAlgorithm.SHA384_CNG));
            WriteLine.OutPutDebugAndConsole("HashAlgorithm.SHA512_CNG", GetHash.GetHashString(data, EnumHashAlgorithm.SHA512_CNG));
#endif
        }

        /// <summary>KeyedHash</summary>
        private static void KeyedHash()
        {
            string data = "ynyKeiR9FXWPkNQHvUPZkAlfUmouExBv";
            string key = "ynyKeiR9FXWPkNQHvUPZkAlfUmouExBv";

            WriteLine.OutPutDebugAndConsole("KeyedHashAlgorithm.Default", GetKeyedHash.GetKeyedHashString(data, EnumKeyedHashAlgorithm.Default, key));
            WriteLine.OutPutDebugAndConsole("KeyedHashAlgorithm.HMACMD5", GetKeyedHash.GetKeyedHashString(data, EnumKeyedHashAlgorithm.HMACMD5, key));
            WriteLine.OutPutDebugAndConsole("KeyedHashAlgorithm.HMACRIPEMD160", GetKeyedHash.GetKeyedHashString(data, EnumKeyedHashAlgorithm.HMACRIPEMD160, key));
            WriteLine.OutPutDebugAndConsole("KeyedHashAlgorithm.HMACSHA1", GetKeyedHash.GetKeyedHashString(data, EnumKeyedHashAlgorithm.HMACSHA1, key));
            WriteLine.OutPutDebugAndConsole("KeyedHashAlgorithm.HMACSHA256", GetKeyedHash.GetKeyedHashString(data, EnumKeyedHashAlgorithm.HMACSHA256, key));
            WriteLine.OutPutDebugAndConsole("KeyedHashAlgorithm.HMACSHA384", GetKeyedHash.GetKeyedHashString(data, EnumKeyedHashAlgorithm.HMACSHA384, key));
            WriteLine.OutPutDebugAndConsole("KeyedHashAlgorithm.HMACSHA512", GetKeyedHash.GetKeyedHashString(data, EnumKeyedHashAlgorithm.HMACSHA512, key));
            WriteLine.OutPutDebugAndConsole("KeyedHashAlgorithm.MACTripleDES", GetKeyedHash.GetKeyedHashString(data, EnumKeyedHashAlgorithm.MACTripleDES, key));
        }

        /// <summary>PasswordHash</summary>
        private static void PasswordHash()
        {
            string providedPassword = "Bx@A]p7u";
            string hashedPassword = "$1$.JU5JJFZXQVIqWQ==.Rkg3YW5WVWt4YQ==.MTAwMA==.ODU0MXJPRWJmVzA9";
            string key = "%NI$VWAR*Y";
            bool ret = false;

            // 旧版との互換性を確認
            ret = GetPasswordHashV1.EqualSaltedPassword(
                providedPassword,
                hashedPassword.Substring(4),
                EnumKeyedHashAlgorithm.MACTripleDES);
            WriteLine.OutPutDebugAndConsole("GetPasswordHashV1.EqualSaltedPassword (old)", ret.ToString());

            ret = GetPasswordHashV2.EqualSaltedPassword(
                providedPassword,
                hashedPassword.Substring(4),
                EnumKeyedHashAlgorithm.MACTripleDES);
            WriteLine.OutPutDebugAndConsole("GetPasswordHashV2.EqualSaltedPassword (old)", ret.ToString());

            // 可逆性確認

            // MACTripleDES
            hashedPassword = GetPasswordHashV2.GetSaltedPassword(
                    providedPassword,                    // password
                    EnumKeyedHashAlgorithm.MACTripleDES, // algorithm
                    key,                                 // key(pwd)
                    10,                                  // salt length
                    1000                                 // stretch count
                );

            ret = GetPasswordHashV2.EqualSaltedPassword(
                providedPassword,
                hashedPassword,
                EnumKeyedHashAlgorithm.MACTripleDES);

            WriteLine.OutPutDebugAndConsole("GetPasswordHashV2.EqualSaltedPassword (new)", ret.ToString());

            // HMACSHA512
            hashedPassword = GetPasswordHashV2.GetSaltedPassword(
                    providedPassword,                    // password
                    EnumKeyedHashAlgorithm.HMACSHA512,   // algorithm
                    key,                                 // key(pwd)
                    10,                                  // salt length
                    1000                                 // stretch count
                );

            ret = GetPasswordHashV2.EqualSaltedPassword(
                providedPassword,
                hashedPassword,
                EnumKeyedHashAlgorithm.HMACSHA512);

            WriteLine.OutPutDebugAndConsole("GetPasswordHashV2.EqualSaltedPassword (new)", ret.ToString());
        }

        #endregion

        #region Cryptography

        #region 秘密鍵

        /// <summary>PrivateKeyCryptography</summary>
        private static void PrivateKeyCryptography()
        {
        }

        #endregion

        #region 公開鍵

        /// <summary>PublicKeyCryptography</summary>
        private static void PublicKeyCryptography()
        {
            #region Variables

            #region Env
            OperatingSystem os = Environment.OSVersion;

            // https://github.com/dotnet/corefx/issues/29404#issuecomment-385287947
            //   *.pfxから証明書を開く場合、X509KeyStorageFlags.Exportableの指定が必要な場合がある。
            //   Linuxのキーは常にエクスポート可能だが、WindowsやMacOSでは必ずしもそうではない。
            X509KeyStorageFlags x509KSF = 0;
            if (os.Platform == PlatformID.Win32NT)
            {
                x509KSF = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
            }
            else //if (os.Platform == PlatformID.Unix)
            {
                x509KSF = X509KeyStorageFlags.DefaultKeySet;
            }
            #endregion
            
            #region Keys
            string privateX509Path = "";
            string publicX509Path = "";
            X509Certificate2 publicX509Key = null;
            X509Certificate2 privateX509Key = null;
            
#if NET45 || NET46
                ECParameters eCParameters = new ECParameters();
#else
#endif
            #endregion

            #region DigitalSign
            byte[] data = CustomEncode.StringToByte("hogehoge", CustomEncode.UTF_8);
            byte[] sign = null;
            #endregion

            #endregion

            #region Test of the X.509 Certificates

            #region RSA
            privateX509Path = @"SHA256RSA.pfx";
            publicX509Path = @"SHA256RSA.cer";
            privateX509Key = new X509Certificate2(privateX509Path, Program.PfxPassword , x509KSF);
            publicX509Key = new X509Certificate2(publicX509Path, "", x509KSF);
            WriteLine.PrivateX509KeyInspector("RSA", privateX509Key);
            WriteLine.PublicX509KeyInspector("RSA", publicX509Key);
            #endregion

#if NETCORE || NET47
            #region DSA
            // https://github.com/dotnet/corefx/issues/18733#issuecomment-296723615
            privateX509Path = @"SHA256DSA.pfx";
            publicX509Path = @"SHA256DSA.cer";
            privateX509Key = new X509Certificate2(privateX509Path, Program.PfxPassword );
            publicX509Key = new X509Certificate2(publicX509Path, "");
            WriteLine.PrivateX509KeyInspector("DSA", privateX509Key);
            WriteLine.PublicX509KeyInspector("DSA", publicX509Key);

            DSA privateDSA = privateX509Key.GetDSAPrivateKey();
            WriteLine.OutPutDebugAndConsole("privateDSA",
                (privateDSA == null ? "is null" : "is not null"));
            //DSA publicDSA = null; // publicX509Key.GetDSAPublicKey(); // Internal.Cryptography.CryptoThrowHelper.WindowsCryptographicException
            #endregion

            #region ECDsa
            // https://github.com/dotnet/corefx/issues/18733#issuecomment-296723615
            privateX509Path = @"SHA256ECDSA.pfx";
            publicX509Path = @"SHA256ECDSA.cer";
            privateX509Key = new X509Certificate2(privateX509Path, Program.PfxPassword );
            publicX509Key = new X509Certificate2(publicX509Path, "");
            WriteLine.PrivateX509KeyInspector("ECDsa", privateX509Key);
            WriteLine.PublicX509KeyInspector("ECDsa", publicX509Key);

            ECDsa privateECDsa = privateX509Key.GetECDsaPrivateKey();
            WriteLine.OutPutDebugAndConsole("privateECDsa",
                (privateECDsa == null ? "is null" : "is not null"));
            ECDsa publicECDsa = publicX509Key.GetECDsaPublicKey();
            WriteLine.OutPutDebugAndConsole("publicECDsa",
                (publicECDsa == null ? "is null" : "is not null"));

            #endregion
#endif

            #endregion

            #region Test of the OpenTouryo.Public.Security.

            DigitalSignParam dsParam = null;
            DigitalSignXML dsXML = null;
            DigitalSignX509 dsX509 = null;

            if (os.Platform == PlatformID.Win32NT)
            {
                #region RSA
                dsParam = new DigitalSignParam(EnumDigitalSignAlgorithm.RsaCSP_SHA256);
                sign = dsParam.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignParam.Verify(RS256)",
                    dsParam.Verify(data, sign).ToString());

                dsXML = new DigitalSignXML(EnumDigitalSignAlgorithm.RsaCSP_SHA256);
                sign = dsXML.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignXML.Verify(RS256)",
                    dsXML.Verify(data, sign).ToString());

                dsX509 = new DigitalSignX509(@"SHA256RSA.pfx", Program.PfxPassword , "SHA256", x509KSF);
                sign = dsX509.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignX509.Verify(RS256)",
                    dsX509.Verify(data, sign).ToString());
                #endregion

                #region DSA
                // DSAはFormatterバージョンしか動かない。
                dsParam = new DigitalSignParam(EnumDigitalSignAlgorithm.DsaCSP_SHA1);
                sign = dsParam.SignByFormatter(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignParam.Verify(DSA-SHA1)",
                    dsParam.VerifyByDeformatter(data, sign).ToString());

                dsXML = new DigitalSignXML(EnumDigitalSignAlgorithm.DsaCSP_SHA1);
                sign = dsXML.SignByFormatter(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignXML.Verify(DSA-SHA1)",
                    dsXML.VerifyByDeformatter(data, sign).ToString());

                // WinでDSAのX509が処理できない。
                //dsX509 = new DigitalSignX509(@"SHA256DSA.pfx", Program.PfxPassword , "SHA256", x509KSF);
                //sign = dsX509.Sign(data);
                //WriteLine.OutPutDebugAndConsole(
                //    "DigitalSignX509.Verify(DSA-SHA256)",
                //    dsX509.Verify(data, sign).ToString());
                #endregion

                #region ECDSA
#if NETCORE || NET47
                DigitalSignECDsaX509 dsECDsaX509 = null;

                privateX509Path = @"SHA256ECDSA.pfx";
                dsECDsaX509 = new DigitalSignECDsaX509(
                    privateX509Path, Program.PfxPassword , HashAlgorithmName.SHA256);
                sign = dsECDsaX509.Sign(data);

                publicX509Path = @"SHA256ECDSA.cer";
                dsECDsaX509 = new DigitalSignECDsaX509(
                    publicX509Path, "", HashAlgorithmName.SHA256);

                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignECDsaX509.Verify(ECDSA)",
                    dsECDsaX509.Verify(data, sign).ToString());
#endif
                #endregion
            }
            else //if (os.Platform == PlatformID.Unix)
            {
#if NETCORE
                #region RSA
                dsParam = new DigitalSignParam(EnumDigitalSignAlgorithm.RsaOpenSsl_SHA256);
                sign = dsParam.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignParam.Verify(RS256)",
                    dsParam.Verify(data, sign).ToString());

                dsXML = new DigitalSignXML(EnumDigitalSignAlgorithm.RsaOpenSsl_SHA256);
                sign = dsXML.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignXML.Verify(RS256)",
                    dsXML.Verify(data, sign).ToString());

                dsX509 = new DigitalSignX509(@"SHA256RSA.pfx", Program.PfxPassword , "SHA256");
                sign = dsX509.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignX509.Verify(RS256)",
                    dsX509.Verify(data, sign).ToString());
                #endregion

                #region DSA
                dsParam = new DigitalSignParam(EnumDigitalSignAlgorithm.DsaOpenSsl_SHA1);
                sign = dsParam.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignParam.Verify(DSA-SHA1)",
                    dsParam.Verify(data, sign).ToString());

                dsXML = new DigitalSignXML(EnumDigitalSignAlgorithm.DsaOpenSsl_SHA1);
                sign = dsXML.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignXML.Verify(DSA-SHA1)",
                    dsXML.Verify(data, sign).ToString());

                dsX509 = new DigitalSignX509(@"SHA256DSA.pfx", Program.PfxPassword , "SHA256");
                sign = dsX509.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignX509.Verify(DSA-SHA256)",
                    dsX509.Verify(data, sign).ToString());
                #endregion

                #region ECDSA
                // .NET Core on Linux
                DigitalSignECDsaX509 ecDsX509 = new DigitalSignECDsaX509(
                    @"SHA256ECDSA.pfx", Program.PfxPassword , HashAlgorithmName.SHA256);

                sign = ecDsX509.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignECDsaX509.Verify(ECDSA)",
                    ecDsX509.Verify(data, sign).ToString());
                #endregion
#endif
            }
            #endregion
        }

        #endregion

        #endregion

        #region Jwt

        /// <summary>MyJwt</summary>
        private static void MyJwt()
        {
            #region Variables

            #region Env
            OperatingSystem os = Environment.OSVersion;

            // https://github.com/dotnet/corefx/issues/29404#issuecomment-385287947
            //   *.pfxから証明書を開く場合、X509KeyStorageFlags.Exportableの指定が必要な場合がある。
            //   Linuxのキーは常にエクスポート可能だが、WindowsやMacOSでは必ずしもそうではない。
            X509KeyStorageFlags x509KSF = 0;
            if (os.Platform == PlatformID.Win32NT)
            {
                x509KSF = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
            }
            else //if (os.Platform == PlatformID.Unix)
            {
                x509KSF = X509KeyStorageFlags.DefaultKeySet;
            }
            #endregion

            #region Token
            string token = "";
            IDictionary<string, object> headers = null;
            IDictionary<string, object> payload = null;
            payload = new Dictionary<string, object>()
                {
                    { "sub", "mr.x@contoso.com" },
                    { "exp", 1300819380 }
                };
            #endregion

            #region Keys
            string jwk = "";
            string privateX509Path = "";
            string publicX509Path = "";

#if NET45 || NET46
                ECParameters eCParameters = new ECParameters();
#else
#endif
            #endregion

            #region DigitalSign
            byte[] data = CustomEncode.StringToByte("hogehoge", CustomEncode.UTF_8);
            byte[] sign = null;
            #endregion

            #endregion
            
            DigitalSignParam dsParam = null;
            DigitalSignX509 dsX509 = null;

            if (os.Platform == PlatformID.Win32NT)
            {
                #region RSA
                dsX509 = new DigitalSignX509(@"SHA256RSA.pfx", Program.PfxPassword , "SHA256", x509KSF);
                sign = dsX509.Sign(data);

                // 鍵の相互変換
                jwk = RsaPublicKeyConverter.ParamToJwk(
                    ((RSA)dsX509.AsymmetricAlgorithm).ExportParameters(false));

                WriteLine.OutPutDebugAndConsole("RSA JWK", jwk);

                dsParam = new DigitalSignParam(
                    RsaPublicKeyConverter.JwkToParam(jwk),
                    EnumDigitalSignAlgorithm.RsaCSP_SHA256);

                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignParam.Verify(RSA JWK)",
                    dsParam.Verify(data, sign).ToString());
                #endregion

                // DSA

                #region ECDSA
#if NETCORE || NET47
                privateX509Path = @"SHA256ECDSA.pfx";
                publicX509Path = @"SHA256ECDSA.cer";

                DigitalSignECDsaX509 ecDsX509 = new DigitalSignECDsaX509(
                    privateX509Path, Program.PfxPassword, HashAlgorithmName.SHA256);
                sign = ecDsX509.Sign(data);

                // 鍵の相互変換
                jwk = EccPublicKeyConverter.ParamToJwk(
                    ((ECDsa)ecDsX509.AsymmetricAlgorithm).ExportParameters(false));

                WriteLine.OutPutDebugAndConsole("ECDSA JWK", jwk);


#if NET47 || NETCOREAPP3_0
                // Core2.0-2.2 on WinでVerifyがエラーになる。
                // DigitalSignECDsaOpenSslを試してみるが生成できない、
                // Core on Win に OpenSSLベースのプロバイダは無いため）
                DigitalSignECDsaCng ecDsCng = new DigitalSignECDsaCng(
                    EccPublicKeyConverter.JwkToCng(jwk), false);

                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignECDsaCng.Verify(ECDSA JWK)",
                    ecDsCng.Verify(data, sign).ToString());
#elif NETCOREAPP2_0
                // Core2.0-2.2 on Winで ECDsaCngは動作しない。
#endif

#endif
                #endregion
            }
            else //if (os.Platform == PlatformID.Unix)
            {
#if NETCORE
                #region RSA
                dsX509 = new DigitalSignX509(@"SHA256RSA.pfx", Program.PfxPassword , "SHA256");
                sign = dsX509.Sign(data);

                // 鍵の相互変換
                jwk = RsaPublicKeyConverter.ParamToJwk(
                    ((RSA)dsX509.AsymmetricAlgorithm).ExportParameters(false));

                WriteLine.OutPutDebugAndConsole("RSA JWK", jwk);

                dsParam = new DigitalSignParam(
                    RsaPublicKeyConverter.JwkToParam(jwk),
                    EnumDigitalSignAlgorithm.RsaCSP_SHA256);

                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignParam.Verify(RSA JWK)",
                    dsParam.Verify(data, sign).ToString());
                #endregion

                // DSA

                #region ECDSA
                privateX509Path = @"SHA256ECDSA.pfx";
                publicX509Path = @"SHA256ECDSA.cer";

                DigitalSignECDsaX509 ecDsX509 = new DigitalSignECDsaX509(
                    privateX509Path, Program.PfxPassword , HashAlgorithmName.SHA256);
                sign = ecDsX509.Sign(data);

                // 鍵の相互変換
                jwk = EccPublicKeyConverter.ParamToJwk(
                    ((ECDsa)ecDsX509.AsymmetricAlgorithm).ExportParameters(false));

                WriteLine.OutPutDebugAndConsole("ECDSA JWK", jwk);

                ECParameters ecp = EccPublicKeyConverter.JwkToParam(jwk);

                DigitalSignECDsaOpenSsl ecDsOpenSSL =
                    new DigitalSignECDsaOpenSsl(ecp,
                        HashAlgorithmCmnFunc.GetHashAlgorithmFromNameString(HashNameConst.SHA256));

                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignECDsaOpenSsl.Verify(ECDSA JWK)",
                    ecDsOpenSSL.Verify(data, sign).ToString());
                #endregion
#endif
            }
        }

        /// <summary>JoseJwt</summary>
        private static void JoseJwt()
        {
            #region Variables

            #region Env
            OperatingSystem os = Environment.OSVersion;

            // https://github.com/dotnet/corefx/issues/29404#issuecomment-385287947
            //   *.pfxから証明書を開く場合、X509KeyStorageFlags.Exportableの指定が必要な場合がある。
            //   Linuxのキーは常にエクスポート可能だが、WindowsやMacOSでは必ずしもそうではない。
            X509KeyStorageFlags x509KSF = 0;
            if (os.Platform == PlatformID.Win32NT)
            {
                x509KSF = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
            }
            else //if (os.Platform == PlatformID.Unix)
            {
                x509KSF = X509KeyStorageFlags.DefaultKeySet;
            }
            #endregion

            #region Token
            string token = "";
            IDictionary<string, object> headers = null;
            IDictionary<string, object> payload = null;
            payload = new Dictionary<string, object>()
                {
                    { "sub", "mr.x@contoso.com" },
                    { "exp", 1300819380 }
                };
            #endregion

            #region Keys

            byte[] secretKey = null;
            byte[] x = null;
            byte[] y = null;
            byte[] d = null;

            string privateX509Path = "";
            string publicX509Path = "";
            X509Certificate2 publicX509Key = null;
            X509Certificate2 privateX509Key = null;

            RSA rsa = null;
            //DSA dsa = null;

            CngKey publicKeyOfCng = null;
            CngKey privateKeyOfCng = null;
#if NET45 || NET46
                ECParameters eCParameters = new ECParameters();
#else
#endif
            #endregion

            #endregion

            #region JWT

            #region Unsecured JWT
            // Creating Plaintext (unprotected) Tokens
            // https://github.com/dvsekhvalnov/jose-jwt#creating-plaintext-unprotected-tokens
            token = "";
            token = JWT.Encode(payload, null, JwsAlgorithm.none);
            WriteLine.OutPutDebugAndConsole("JwsAlgorithm.none", token);
            #endregion

            #region JWS (Creating signed Tokens)
            // https://github.com/dvsekhvalnov/jose-jwt#creating-signed-tokens

            #region HS-* family
            // HS256, HS384, HS512
            // https://github.com/dvsekhvalnov/jose-jwt#hs--family
            secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };
            token = "";
            token = JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
            Program.VerifyResult("JwsAlgorithm.HS256", token, secretKey);
            #endregion

            #region RS-* and PS-* family
            // RS256, RS384, RS512 and PS256, PS384, PS512
            // https://github.com/dvsekhvalnov/jose-jwt#rs--and-ps--family
            // X509Certificate2 x509Certificate2 = new X509Certificate2();

            privateX509Path = @"SHA256RSA.pfx";
            publicX509Path = @"SHA256RSA.cer";
            privateX509Key = new X509Certificate2(privateX509Path, Program.PfxPassword , x509KSF);
            publicX509Key = new X509Certificate2(publicX509Path, "", x509KSF);

            token = "";


#if NETCORE
            rsa = (RSA)privateX509Key.PrivateKey;
#else
            // .net frameworkでは、何故かコレが必要。
            rsa = (RSA)AsymmetricAlgorithmCmnFunc.CreateSameKeySizeSP(privateX509Key.PrivateKey);
#endif
            token = JWT.Encode(payload, rsa, JwsAlgorithm.RS256);
            Program.VerifyResult("JwsAlgorithm.RS256", token, rsa);

            #endregion

            #region ES- * family
            // ES256, ES384, ES512 ECDSA signatures
            // https://github.com/dvsekhvalnov/jose-jwt#es---family

            x = new byte[] { 4, 114, 29, 223, 58, 3, 191, 170, 67, 128, 229, 33, 242, 178, 157, 150, 133, 25, 209, 139, 166, 69, 55, 26, 84, 48, 169, 165, 67, 232, 98, 9 };
            y = new byte[] { 131, 116, 8, 14, 22, 150, 18, 75, 24, 181, 159, 78, 90, 51, 71, 159, 214, 186, 250, 47, 207, 246, 142, 127, 54, 183, 72, 72, 253, 21, 88, 53 };
            d = new byte[] { 42, 148, 231, 48, 225, 196, 166, 201, 23, 190, 229, 199, 20, 39, 226, 70, 209, 148, 29, 70, 125, 14, 174, 66, 9, 198, 80, 251, 95, 107, 98, 206 };

            if (os.Platform == PlatformID.Win32NT)
            {
                // https://github.com/dvsekhvalnov/jose-jwt/blob/master/jose-jwt/Security/Cryptography/EccKey.cs
                privateKeyOfCng = EccKey.New(x, y, d);
                publicKeyOfCng = EccKey.New(x, y);

                token = "";
                token = JWT.Encode(payload, privateKeyOfCng, JwsAlgorithm.ES256);
                Program.VerifyResult("JwsAlgorithm.ES256", token, publicKeyOfCng);
            }
            else //if (os.Platform == PlatformID.Unix)
            {
#if NETCORE
                ECParameters eCParameters = new ECParameters();

                // Curve
                eCParameters.Curve =
                    EccPublicKeyConverter.GetECCurveFromCrvString(
                        EccPublicKeyConverter.GetCrvStringFromXCoordinate(x));

                // x, y, d
                eCParameters.Q.X = x;
                eCParameters.Q.Y = y;
                eCParameters.D = d;
                ECDsaOpenSsl eCDsaOpenSsl = new ECDsaOpenSsl(eCParameters.Curve);
                eCDsaOpenSsl.ImportParameters(eCParameters);

                token = "";
                token = JWT.Encode(payload, eCDsaOpenSsl, JwsAlgorithm.ES256);
                Program.VerifyResult("JwsAlgorithm.ES256", token, eCDsaOpenSsl);
#endif
            }

#if NETCORE || NET47
            privateX509Path = @"SHA256ECDSA.pfx";
            publicX509Path = @"SHA256ECDSA.cer";
            privateX509Key = new X509Certificate2(privateX509Path, Program.PfxPassword );
            publicX509Key = new X509Certificate2(publicX509Path, "");

            try
            {
#if NETCORE
                if (os.Platform == PlatformID.Win32NT)
                {
                }
                else //if (os.Platform == PlatformID.Unix)
                {
                    // ECCurveを分析してみる。
                    ECCurve eCCurve = ((ECDsaOpenSsl)privateX509Key.GetECDsaPrivateKey()).ExportExplicitParameters(true).Curve;
                    WriteLine.OutPutDebugAndConsole(
                        "Inspect ECCurve",
                        ObjectInspector.Inspect(eCCurve));
                }
#endif
                token = "";
                token = JWT.Encode(payload, privateX509Key.GetECDsaPrivateKey(), JwsAlgorithm.ES256);
                Program.VerifyResult("JwsAlgorithm.ES256", token, publicX509Key.GetECDsaPublicKey());
            }
            catch (Exception ex)
            {
                WriteLine.OutPutDebugAndConsole(
                    "JwsAlgorithm.ES256",
                    ex.GetType().ToString() + ", " + ex.Message);
            }
#endif

            #endregion

            #endregion

            #region JWE (Creating encrypted Tokens)
            // https://github.com/dvsekhvalnov/jose-jwt#creating-encrypted-tokens

            #region RSA-* key management family of algorithms
            // RSA-OAEP-256, RSA-OAEP and RSA1_5 key
            // https://github.com/dvsekhvalnov/jose-jwt#rsa--key-management-family-of-algorithms

            privateX509Path = @"SHA256RSA.pfx";
            publicX509Path = @"SHA256RSA.cer";
            privateX509Key = new X509Certificate2(privateX509Path, Program.PfxPassword , x509KSF);
            publicX509Key = new X509Certificate2(publicX509Path, "", x509KSF);

            // RSAES-PKCS1-v1_5 and AES_128_CBC_HMAC_SHA_256
            token = "";
            token = JWT.Encode(payload, publicX509Key.PublicKey.Key, JweAlgorithm.RSA1_5, JweEncryption.A128CBC_HS256);
            Program.VerifyResult(
                "JweAlgorithm.RSA1_5, JweEncryption.A128CBC_HS256",
                token, privateX509Key.PrivateKey);

            // RSAES-OAEP and AES GCM
            try
            {
                token = "";
                token = JWT.Encode(payload, publicX509Key.PublicKey.Key, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
                Program.VerifyResult(
                    "JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM",
                    token, privateX509Key.PrivateKey);
            }
            catch (Exception ex)
            {
                // Unhandled Exception: System.DllNotFoundException: Unable to load DLL 'bcrypt.dll' at ubunntu
                WriteLine.OutPutDebugAndConsole(
                    "JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM",
                    ex.GetType().ToString() + ", " + ex.Message);
            }
            #endregion

            #region Other key management family of algorithms

            secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };

            #region DIR direct pre-shared symmetric key family of algorithms
            // https://github.com/dvsekhvalnov/jose-jwt#dir-direct-pre-shared-symmetric-key-family-of-algorithms
            token = "";
            token = JWT.Encode(payload, secretKey, JweAlgorithm.DIR, JweEncryption.A128CBC_HS256);
            Program.VerifyResult("JweAlgorithm.DIR, JweEncryption.A128CBC_HS256", token, secretKey);
            #endregion

            #region AES Key Wrap key management family of algorithms
            // AES128KW, AES192KW and AES256KW key management
            // https://github.com/dvsekhvalnov/jose-jwt#aes-key-wrap-key-management-family-of-algorithms
            token = "";
            token = JWT.Encode(payload, secretKey, JweAlgorithm.A256KW, JweEncryption.A256CBC_HS512);
            Program.VerifyResult("JweAlgorithm.A256KW, JweEncryption.A256CBC_HS512", token, secretKey);
            #endregion

            #region AES GCM Key Wrap key management family of algorithms
            // AES128GCMKW, AES192GCMKW and AES256GCMKW key management
            // https://github.com/dvsekhvalnov/jose-jwt#aes-gcm-key-wrap-key-management-family-of-algorithms
            try
            {
                token = "";
                token = JWT.Encode(payload, secretKey, JweAlgorithm.A256GCMKW, JweEncryption.A256CBC_HS512);
                Program.VerifyResult("JweAlgorithm.A256GCMKW, JweEncryption.A256CBC_HS512", token, secretKey);
            }
            catch (Exception ex)
            {
                // Unhandled Exception: System.DllNotFoundException: Unable to load DLL 'bcrypt.dll' at ubunntu
                WriteLine.OutPutDebugAndConsole(
                    "JweAlgorithm.A256GCMKW, JweEncryption.A256CBC_HS512",
                    ex.GetType().ToString() + ", " + ex.Message);
            }
            #endregion

            #region ECDH-ES and ECDH-ES with AES Key Wrap key management family of algorithms
            // ECDH-ES and ECDH-ES+A128KW, ECDH-ES+A192KW, ECDH-ES+A256KW key management
            // https://github.com/dvsekhvalnov/jose-jwt#ecdh-es-and-ecdh-es-with-aes-key-wrap-key-management-family-of-algorithms
            try
            {
                x = new byte[] { 4, 114, 29, 223, 58, 3, 191, 170, 67, 128, 229, 33, 242, 178, 157, 150, 133, 25, 209, 139, 166, 69, 55, 26, 84, 48, 169, 165, 67, 232, 98, 9 };
                y = new byte[] { 131, 116, 8, 14, 22, 150, 18, 75, 24, 181, 159, 78, 90, 51, 71, 159, 214, 186, 250, 47, 207, 246, 142, 127, 54, 183, 72, 72, 253, 21, 88, 53 };
                publicKeyOfCng = EccKey.New(x, y, usage: CngKeyUsages.KeyAgreement);
                token = "";
                token = JWT.Encode(payload, publicKeyOfCng, JweAlgorithm.ECDH_ES, JweEncryption.A256GCM);
                Program.VerifyResult("JweAlgorithm.ECDH_ES, JweEncryption.A256GCM", token, publicKeyOfCng);
            }
            catch (Exception ex)
            {
                // System.NotImplementedException: 'not yet'
                WriteLine.OutPutDebugAndConsole(
                    "JweAlgorithm.ECDH_ES, JweEncryption.A256GCM",
                    ex.GetType().ToString() + ", " + ex.Message);
            }
            #endregion

            #region PBES2 using HMAC SHA with AES Key Wrap key management family of algorithms
            token = "";
            token = JWT.Encode(payload, "top secret", JweAlgorithm.PBES2_HS256_A128KW, JweEncryption.A256CBC_HS512);
            Program.VerifyResult("JweAlgorithm.PBES2_HS256_A128KW, JweEncryption.A256CBC_HS512", token, "top secret");
            #endregion

            #endregion

            #endregion

            #endregion

            #region ELSE

            #region Additional utilities
            // https://github.com/dvsekhvalnov/jose-jwt#additional-utilities

            #region Adding extra headers
            // https://github.com/dvsekhvalnov/jose-jwt#adding-extra-headers

            headers = new Dictionary<string, object>()
                {
                    { "typ", "JWT" },
                    { "cty", "JWT" },
                    { "keyid", "111-222-333"}
                };

            privateX509Path = @"SHA256RSA.pfx";
            publicX509Path = @"SHA256RSA.cer";
            privateX509Key = new X509Certificate2(privateX509Path, Program.PfxPassword , x509KSF);
            publicX509Key = new X509Certificate2(publicX509Path, "", x509KSF);

#if NETCORE
            rsa = (RSA)privateX509Key.PrivateKey;
#else
            // .net frameworkでは、何故かコレが必要。
            rsa = (RSA)AsymmetricAlgorithmCmnFunc.CreateSameKeySizeSP(privateX509Key.PrivateKey);
#endif

            token = "";
            token = JWT.Encode(payload, rsa, JwsAlgorithm.RS256, extraHeaders: headers);
            Program.VerifyResult("Adding extra headers to RS256", token, rsa);
            #endregion

            #region Strict validation
            // https://github.com/dvsekhvalnov/jose-jwt#strict-validation
            // 厳密な検証では、Algorithmを指定可能
            WriteLine.OutPutDebugAndConsole(
                "Strict validation(RS256)",
                JWT.Decode(token, rsa, JwsAlgorithm.RS256));
            #endregion

            #region Two-phase validation
            // https://github.com/dvsekhvalnov/jose-jwt#two-phase-validation
            // ヘッダのkeyidクレームからキーを取り出して復号化する方法。
            //headers = JWT.Headers(token);
            // ・・・
            //string hoge = JWT.Decode(token, "key");
            #endregion

            #region Working with binary payload
            // https://github.com/dvsekhvalnov/jose-jwt#working-with-binary-payload
            #endregion

            #endregion

            #region Settings
            // https://github.com/dvsekhvalnov/jose-jwt#settings
            // グローバル設定

            #region Example of JWTSettings
            // https://github.com/dvsekhvalnov/jose-jwt#example-of-jwtsettings

            #endregion

            #region Customizing json <-> object parsing & mapping
            // https://github.com/dvsekhvalnov/jose-jwt#customizing-json---object-parsing--mapping
            // マッピング
            // https://github.com/dvsekhvalnov/jose-jwt#example-of-newtonsoftjson-mapper
            // https://github.com/dvsekhvalnov/jose-jwt#example-of-servicestack-mapper

            #endregion

            #region Customizing algorithm implementations
            // https://github.com/dvsekhvalnov/jose-jwt#customizing-algorithm-implementations
            // https://github.com/dvsekhvalnov/jose-jwt#example-of-custom-algorithm-implementation
            #endregion

            #region Providing aliases
            // https://github.com/dvsekhvalnov/jose-jwt#providing-aliases
            #endregion

            #endregion

            #region Dealing with keys
            // https://github.com/dvsekhvalnov/jose-jwt#dealing-with-keys
            // https://github.com/dvsekhvalnov/jose-jwt#rsacryptoserviceprovider
            // - http://stackoverflow.com/questions/7444586/how-can-i-sign-a-file-using-rsa-and-sha256-with-net
            // - http://hintdesk.com/c-how-to-fix-invalid-algorithm-specified-when-signing-with-sha256/
            // https://github.com/dvsekhvalnov/jose-jwt#if-you-have-only-rsa-private-key
            // - http://www.donaldsbaconbytes.com/2016/08/create-jwt-with-a-private-rsa-key/
            #endregion

            #region Strong-Named assembly
            // https://github.com/dvsekhvalnov/jose-jwt#strong-named-assembly
            // - https://github.com/dvsekhvalnov/jose-jwt/issues/5
            // - https://github.com/brutaldev/StrongNameSigner
            #endregion

            #region More examples
            // https://github.com/dvsekhvalnov/jose-jwt#more-examples
            // https://github.com/dvsekhvalnov/jose-jwt/blob/master/UnitTests/TestSuite.cs
            #endregion

            #endregion
        }

        #endregion

        #endregion

        #region Inspector and Verifier

        /// <summary>VerifyResult</summary>
        /// <param name="testLabel">string</param>
        /// <param name="jwt">string</param>
        /// <param name="key">object</param>
        private static void VerifyResult(string testLabel, string jwt, object key)
        {
            WriteLine.OutPutDebugAndConsole(testLabel, "Original:" + jwt);

            WriteLine.JwtInspector(testLabel, jwt);

            WriteLine.OutPutDebugAndConsole(testLabel, "Decoded:" + JWT.Decode(jwt, key));
        }

        #endregion
    }
}
