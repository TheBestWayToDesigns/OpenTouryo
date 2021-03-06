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
    class Program
    {
        public static void hogehoge(string[] args)
        {
            try
            {
                #region Variables

                #region Env

                // https://github.com/dotnet/corefx/issues/29404#issuecomment-385287947
                //   *.pfxから証明書を開く場合、X509KeyStorageFlags.Exportableの指定が必要な場合がある。
                //   Linuxのキーは常にエクスポート可能だが、WindowsやMacOSでは必ずしもそうではない。
                X509KeyStorageFlags x509KSF = 0;
                x509KSF = X509KeyStorageFlags.DefaultKeySet;
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
                //CngKey privateKeyOfCng = null;
                ECParameters eCParameters = new ECParameters();
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
                privateX509Key = new X509Certificate2(privateX509Path, "test", x509KSF);
                publicX509Key = new X509Certificate2(publicX509Path, "", x509KSF);
                WriteLine.PrivateX509KeyInspector("RSA", privateX509Key);
                WriteLine.PublicX509KeyInspector("RSA", publicX509Key);
                #endregion

                #region DSA
                // https://github.com/dotnet/corefx/issues/18733#issuecomment-296723615
                privateX509Path = @"SHA256DSA.pfx";
                publicX509Path = @"SHA256DSA.cer";
                privateX509Key = new X509Certificate2(privateX509Path, "test");
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
                privateX509Key = new X509Certificate2(privateX509Path, "test");
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

                #endregion

                WriteLine.OutPutDebugAndConsole("----------------------------------------------------------------------------------------------------");

                #region Test of the OpenTouryo.Public.Security.

                DigitalSignParam dsParam = null;
                DigitalSignXML dsXML = null;
                DigitalSignX509 dsX509 = null;

                #region RSA
                dsParam = new DigitalSignParam(EnumDigitalSignAlgorithm.RsaOpenSsl_SHA256);
                sign = dsParam.Sign(data);
                WriteLine.OutPutDebugAndConsole("DigitalSignParam.Verify(RS256)",
                    dsParam.Verify(data, sign).ToString());

                dsXML = new DigitalSignXML(EnumDigitalSignAlgorithm.RsaOpenSsl_SHA256);
                sign = dsXML.Sign(data);
                WriteLine.OutPutDebugAndConsole("DigitalSignXML.Verify(RS256)",
                    dsXML.Verify(data, sign).ToString());

                dsX509 = new DigitalSignX509(@"SHA256RSA.pfx", "test", "SHA256");
                sign = dsX509.Sign(data);
                WriteLine.OutPutDebugAndConsole("DigitalSignX509.Verify(RSA)",
                    dsX509.Verify(data, sign).ToString());

                // 鍵の相互変換
                jwk = RsaPublicKeyConverter.ParamToJwk(
                    ((RSA)dsX509.AsymmetricAlgorithm).ExportParameters(false));

                WriteLine.OutPutDebugAndConsole("RSA JWK", jwk);

                dsParam = new DigitalSignParam(
                    RsaPublicKeyConverter.JwkToParam(jwk),
                    EnumDigitalSignAlgorithm.RsaCSP_SHA256);

                WriteLine.OutPutDebugAndConsole("DigitalSignX509.Verify(RSA JWK)",
                   dsParam.Verify(data, sign).ToString());
                #endregion

                #region DSA
                dsParam = new DigitalSignParam(EnumDigitalSignAlgorithm.DsaOpenSsl_SHA1);
                sign = dsParam.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignParam.Verify(DS1)",
                dsParam.Verify(data, sign).ToString());

                dsXML = new DigitalSignXML(EnumDigitalSignAlgorithm.DsaOpenSsl_SHA1);
                sign = dsXML.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignXML.Verify(DS1)",
                dsXML.Verify(data, sign).ToString());

                dsX509 = new DigitalSignX509(@"SHA256DSA.pfx", "test", "SHA256");
                sign = dsX509.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignX509.Verify(DSA)",
                dsX509.Verify(data, sign).ToString());
                #endregion

                #region ECDSA
                // .NET Core on Linux
                DigitalSignECDsaX509 ecDsX509 = new DigitalSignECDsaX509(
                    @"SHA256ECDSA.pfx", "test", HashAlgorithmName.SHA256);

                sign = ecDsX509.Sign(data);
                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignX509.Verify(ECDSA)",
                ecDsX509.Verify(data, sign).ToString());

                token = "";
                token = JWT.Encode(payload, ((ECDsa)ecDsX509.AsymmetricAlgorithm), JwsAlgorithm.ES256);

                // 鍵の相互変換
                jwk = EccPublicKeyConverter.ParamToJwk(
                    ((ECDsa)ecDsX509.AsymmetricAlgorithm).ExportParameters(false));

                WriteLine.OutPutDebugAndConsole("ECDSA JWK", jwk);

                DigitalSignECDsaOpenSsl ecDsParam = 
                    new DigitalSignECDsaOpenSsl(
                        EccPublicKeyConverter.JwkToParam(jwk),
                        HashAlgorithmCmnFunc.GetHashAlgorithmFromNameString(HashNameConst.SHA256));

                WriteLine.OutPutDebugAndConsole(
                    "DigitalSignX509.Verify(ECDSA JWK)",
                    ecDsParam.Verify(data, sign).ToString());
               
                Program.VerifyResult("JwsAlgorithm.ES256", token, ecDsParam.AsymmetricAlgorithm);

                #endregion

                #endregion

                WriteLine.OutPutDebugAndConsole("----------------------------------------------------------------------------------------------------");

                #region Test of the jose-jwt

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
                privateX509Key = new X509Certificate2(privateX509Path, "test", x509KSF);
                publicX509Key = new X509Certificate2(publicX509Path, "", x509KSF);

                token = "";

                rsa = (RSA)privateX509Key.PrivateKey;
                token = JWT.Encode(payload, rsa, JwsAlgorithm.RS256);
                Program.VerifyResult("JwsAlgorithm.RS256", token, rsa);

                #endregion

                #region ES- * family
                // ES256, ES384, ES512 ECDSA signatures
                // https://github.com/dvsekhvalnov/jose-jwt#es---family

                x = new byte[] { 4, 114, 29, 223, 58, 3, 191, 170, 67, 128, 229, 33, 242, 178, 157, 150, 133, 25, 209, 139, 166, 69, 55, 26, 84, 48, 169, 165, 67, 232, 98, 9 };
                y = new byte[] { 131, 116, 8, 14, 22, 150, 18, 75, 24, 181, 159, 78, 90, 51, 71, 159, 214, 186, 250, 47, 207, 246, 142, 127, 54, 183, 72, 72, 253, 21, 88, 53 };
                d = new byte[] { 42, 148, 231, 48, 225, 196, 166, 201, 23, 190, 229, 199, 20, 39, 226, 70, 209, 148, 29, 70, 125, 14, 174, 66, 9, 198, 80, 251, 95, 107, 98, 206 };

                eCParameters = new ECParameters();

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

                try
                {
                    privateX509Path = @"SHA256ECDSA.pfx";
                    publicX509Path = @"SHA256ECDSA.cer";
                    privateX509Key = new X509Certificate2(privateX509Path, "test");
                    publicX509Key = new X509Certificate2(publicX509Path, "");

                    // ECCurveを分析してみる。
                    ECCurve eCCurve = ((ECDsaOpenSsl)privateX509Key.GetECDsaPrivateKey()).ExportExplicitParameters(true).Curve;
                    WriteLine.OutPutDebugAndConsole("Inspect ECCurve", ObjectInspector.Inspect(eCCurve));

                    token = "";
                    token = JWT.Encode(payload, privateX509Key.GetECDsaPrivateKey(), JwsAlgorithm.ES256);
                    Program.VerifyResult("JwsAlgorithm.ES256", token, publicX509Key.GetECDsaPublicKey());
                }
                catch (Exception ex)
                {
                    WriteLine.OutPutDebugAndConsole("JwsAlgorithm.ES256", ex.GetType().ToString() + ", " + ex.Message);
                }

                #endregion

                #endregion

                #region JWE (Creating encrypted Tokens)
                // https://github.com/dvsekhvalnov/jose-jwt#creating-encrypted-tokens

                #region RSA-* key management family of algorithms
                // RSA-OAEP-256, RSA-OAEP and RSA1_5 key
                // https://github.com/dvsekhvalnov/jose-jwt#rsa--key-management-family-of-algorithms

                privateX509Path = @"SHA256RSA.pfx";
                publicX509Path = @"SHA256RSA.cer";
                privateX509Key = new X509Certificate2(privateX509Path, "test", x509KSF);
                publicX509Key = new X509Certificate2(publicX509Path, "", x509KSF);

                // RSAES-PKCS1-v1_5 and AES_128_CBC_HMAC_SHA_256
                token = "";
                token = JWT.Encode(payload, publicX509Key.PublicKey.Key, JweAlgorithm.RSA1_5, JweEncryption.A128CBC_HS256);
                Program.VerifyResult("JweAlgorithm.RSA1_5, JweEncryption.A128CBC_HS256", token, privateX509Key.PrivateKey);

                // RSAES-OAEP and AES GCM
                try
                {
                    token = "";
                    token = JWT.Encode(payload, publicX509Key.PublicKey.Key, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
                    Program.VerifyResult("JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM", token, privateX509Key.PrivateKey);
                }
                catch (Exception ex)
                {
                    // Unhandled Exception: System.DllNotFoundException: Unable to load DLL 'bcrypt.dll' at ubunntu
                    WriteLine.OutPutDebugAndConsole("JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM", ex.GetType().ToString() + ", " + ex.Message);
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
                    WriteLine.OutPutDebugAndConsole("JweAlgorithm.A256GCMKW, JweEncryption.A256CBC_HS512", ex.GetType().ToString() + ", " + ex.Message);
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
                    WriteLine.OutPutDebugAndConsole("JweAlgorithm.ECDH_ES, JweEncryption.A256GCM", ex.GetType().ToString() + ", " + ex.Message);
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

                #endregion
            }
            catch (Exception ex)
            {
                WriteLine.OutPutDebugAndConsole(ex.ToString());
            }
        }

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
