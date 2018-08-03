using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.CrytoHelper
{
    public static class X509CyptoAlgorithm
    {
        static X509Certificate2 LoadCertificate()
        {
            //return new X509Certificate2(
            //    string.Format(@"{0}\Certificates\encrpytcert.pfx",
            //    AppDomain.CurrentDomain.BaseDirectory)
            //);

            return new X509Certificate2(string.Format(@"{0}\Certificates\encrpytcert.pfx",
               AppDomain.CurrentDomain.BaseDirectory), "", X509KeyStorageFlags.MachineKeySet);
        }
        public static string Encrypt(string input)
        {
            //get X509 certificate from store
            X509Certificate2 cert = LoadCertificate();
            
            //use public key to encrypt
            var publicKey = cert.PublicKey.Key as RSACryptoServiceProvider;

            //get XML string of public key from certificate
            RSACryptoServiceProvider rsaEncryptor = new RSACryptoServiceProvider();

            //var xmlStringPublicKey = @"<RSAKeyValue><Modulus>u4zh/l+JL2GzHqsVH95dsZNfqfdI1rHSVFnpJky0e3pEv4/GuifblaME3w9Dcu64js9OzpRX1C4yqMhkTbH3BkeDGXXaI/pwvWWHWfTVAgubLL/5XvlGPWK/GgAwKTYebyTKDOkVWKpGfrd/JKZJKkwY1aXO/+ZM8RfPOYek1wuJ7r3ZGyEnw9UDf+6b/G0mibmXvosftpR6rSb4FSjVD4Uhef2NHTw1z0oGcVT3vAlUBCjmLU2DVWO8OsH5fZV7vevC7qY1o9Quu5kXmI59EN/bL8BU5uOeobDYGvR8yYztDD6KMsHQE37b3+IukjMb5/ca3CpSIUmsZa+GmirN+Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            //rsaEncryptor.FromXmlString(xmlStringPublicKey);

            rsaEncryptor.FromXmlString(publicKey.ToXmlString(false));

            //encrypt input string
            byte[] cipherData = rsaEncryptor.Encrypt(Encoding.UTF8.GetBytes(input), true);

            //dispose object
            rsaEncryptor.Dispose();

            //convert to base64string from byte[] and return
            var toBase64String = Convert.ToBase64String(cipherData);
            return toBase64String;
        }

        public static string Encrypt(byte[] input)
        {
            //get X509 certificate from store
            X509Certificate2 cert = LoadCertificate();

            //use public key to encrypt
            var publicKey = cert.PublicKey.Key as RSACryptoServiceProvider;

            //get XML string of public key from certificate
            RSACryptoServiceProvider rsaEncryptor = new RSACryptoServiceProvider();
            rsaEncryptor.FromXmlString(publicKey.ToXmlString(false));
            
            //encrypt input byte[]
            byte[] cipherData = rsaEncryptor.Encrypt(input, true);

            //dispose object
            rsaEncryptor.Dispose();

            //convert to base64string from byte[] and return
            var toBase64String = Convert.ToBase64String(cipherData);
            return toBase64String;
        }

        public static string Decrypt(byte[] input)
        {
            //get X509 certificate from store
            X509Certificate2 cert = LoadCertificate();
            //var publicKey = cert.PublicKey.Key as RSACryptoServiceProvider;

            //use private key to decrypt
            RSACryptoServiceProvider rsaEncryptor = (RSACryptoServiceProvider)cert.PrivateKey;

            //decrypt input byte[]
            byte[] plainData = rsaEncryptor.Decrypt(input, true);

            //dispose object
            rsaEncryptor.Dispose();

            //return plain text string
            return Encoding.UTF8.GetString(plainData);
        }

        public static byte[] DecryptSerializedObject(byte[] input)
        {
            //get X509 certificate from store
            X509Certificate2 cert = LoadCertificate();
            //var publicKey = cert.PublicKey.Key as RSACryptoServiceProvider;

            //use private key to decrypt
            RSACryptoServiceProvider rsaEncryptor = (RSACryptoServiceProvider)cert.PrivateKey;

            //decrypt input byte[]
            byte[] plainData = rsaEncryptor.Decrypt(input, true);

            //dispose object
            rsaEncryptor.Dispose();

            //return decrypted byte[] which represents serialized POCO object
            return plainData;
        }
 
    }
}
