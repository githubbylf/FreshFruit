using System.Web.Security; 
     
    ... 
     
    /// <summary> 
    /// SHA1加密字符串 
    /// </summary> 
    /// <param name="source">源字符串</param> 
    /// <returns>加密后的字符串</returns> 
    public string SHA1(string source)
{
    return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "SHA1");
}


/// <summary> 
/// MD5加密字符串 
/// </summary> 
/// <param name="source">源字符串</param> 
/// <returns>加密后的字符串</returns> 
public string MD5(string source)
{
    return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5"); ;
}

 
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45

方法二(可逆加密解密)：
    using System.Security.Cryptography; 
     
    ... 
     
    public string Encode(string data)
{
    byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
    byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
    int i = cryptoProvider.KeySize;
    MemoryStream ms = new MemoryStream();
    CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

    StreamWriter sw = new StreamWriter(cst);
    sw.Write(data);
    sw.Flush();
    cst.FlushFinalBlock();
    sw.Flush();
    return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

}

public string Decode(string data)
{
    byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
    byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

    byte[] byEnc;
    try
    {
        byEnc = Convert.FromBase64String(data);
    }
    catch
    {
        return null;
    }

    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
    MemoryStream ms = new MemoryStream(byEnc);
    CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
    StreamReader sr = new StreamReader(cst);
    return sr.ReadToEnd();
}
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29

方法三(MD5不可逆)：
    using System.Security.Cryptography; 
     
    ... 
     
    //MD5不可逆加密 
     
    //32位加密 
     
    public string GetMD5_32(string s, string _input_charset)
{
    MD5 md5 = new MD5CryptoServiceProvider();
    byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
    StringBuilder sb = new StringBuilder(32);
    for (int i = 0; i < t.Length; i++)
    {
        sb.Append(t[i].ToString("x").PadLeft(2, '0'));
    }
    return sb.ToString();
}

//16位加密 
public static string GetMd5_16(string ConvertString)
{
    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
    string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
    t2 = t2.Replace("-", "");
    return t2;
}
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67
68
69
70
71
72
73
74
75
76
77
78
79
80
81
82
83

方法四(对称加密)：
    using System.IO; 
    using System.Security.Cryptography; 
     
    ... 
     
    private SymmetricAlgorithm mobjCryptoService;
private string Key;
/// <summary>    
/// 对称加密类的构造函数    
/// </summary>    
public SymmetricMethod()
{
    mobjCryptoService = new RijndaelManaged();
    Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
}
/// <summary>    
/// 获得密钥    
/// </summary>    
/// <returns>密钥</returns>    
private byte[] GetLegalKey()
{
    string sTemp = Key;
    mobjCryptoService.GenerateKey();
    byte[] bytTemp = mobjCryptoService.Key;
    int KeyLength = bytTemp.Length;
    if (sTemp.Length > KeyLength)
        sTemp = sTemp.Substring(0, KeyLength);
    else if (sTemp.Length < KeyLength)
        sTemp = sTemp.PadRight(KeyLength, ' ');
    return ASCIIEncoding.ASCII.GetBytes(sTemp);
}
/// <summary>    
/// 获得初始向量IV    
/// </summary>    
/// <returns>初试向量IV</returns>    
private byte[] GetLegalIV()
{
    string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
    mobjCryptoService.GenerateIV();
    byte[] bytTemp = mobjCryptoService.IV;
    int IVLength = bytTemp.Length;
    if (sTemp.Length > IVLength)
        sTemp = sTemp.Substring(0, IVLength);
    else if (sTemp.Length < IVLength)
        sTemp = sTemp.PadRight(IVLength, ' ');
    return ASCIIEncoding.ASCII.GetBytes(sTemp);
}
/// <summary>    
/// 加密方法    
/// </summary>    
/// <param name="Source">待加密的串</param>    
/// <returns>经过加密的串</returns>    
public string Encrypto(string Source)
{
    byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
    MemoryStream ms = new MemoryStream();
    mobjCryptoService.Key = GetLegalKey();
    mobjCryptoService.IV = GetLegalIV();
    ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
    cs.Write(bytIn, 0, bytIn.Length);
    cs.FlushFinalBlock();
    ms.Close();
    byte[] bytOut = ms.ToArray();
    return Convert.ToBase64String(bytOut);
}
/// <summary>    
/// 解密方法    
/// </summary>    
/// <param name="Source">待解密的串</param>    
/// <returns>经过解密的串</returns>    
public string Decrypto(string Source)
{
    byte[] bytIn = Convert.FromBase64String(Source);
    MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
    mobjCryptoService.Key = GetLegalKey();
    mobjCryptoService.IV = GetLegalIV();
    ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
    StreamReader sr = new StreamReader(cs);
    return sr.ReadToEnd();
}
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
	
方法五：
    using System.IO; 
    using System.Security.Cryptography; 
    using System.Text; 
     
    ... 
     
    //默认密钥向量 
    private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
/// <summary> 
/// DES加密字符串 
/// </summary> 
/// <param name="encryptString">待加密的字符串</param> 
/// <param name="encryptKey">加密密钥,要求为8位</param> 
/// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
public static string EncryptDES(string encryptString, string encryptKey)
{
    try
    {
        byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
        byte[] rgbIV = Keys;
        byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
        DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();
        return Convert.ToBase64String(mStream.ToArray());
    }
    catch
    {
        return encryptString;
    }
}

/// <summary> 
/// DES解密字符串 
/// </summary> 
/// <param name="decryptString">待解密的字符串</param> 
/// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param> 
/// <returns>解密成功返回解密后的字符串，失败返源串</returns> 
public static string DecryptDES(string decryptString, string decryptKey)
{
    try
    {
        byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
        byte[] rgbIV = Keys;
        byte[] inputByteArray = Convert.FromBase64String(decryptString);
        DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();
        return Encoding.UTF8.GetString(mStream.ToArray());
    }
    catch
    {
        return decryptString;
    }
}
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67

方法六(文件加密)：
    using System.IO; 
    using System.Security.Cryptography; 
    using System.Text; 
     
    ... 
     
    //加密文件 
    private static void EncryptData(String inName, String outName, byte[] desKey, byte[] desIV)
{
    //Create the file streams to handle the input and output files. 
    FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
    FileStream fout = new FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write);
    fout.SetLength(0);

    //Create variables to help with read and write. 
    byte[] bin = new byte[100]; //This is intermediate storage for the encryption. 
    long rdlen = 0;              //This is the total number of bytes written. 
    long totlen = fin.Length;    //This is the total length of the input file. 
    int len;                     //This is the number of bytes to be written at a time. 

    DES des = new DESCryptoServiceProvider();
    CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write);

    //Read from the input file, then encrypt and write to the output file. 
    while (rdlen < totlen)
    {
        len = fin.Read(bin, 0, 100);
        encStream.Write(bin, 0, len);
        rdlen = rdlen + len;
    }

    encStream.Close();
    fout.Close();
    fin.Close();
}

//解密文件 
private static void DecryptData(String inName, String outName, byte[] desKey, byte[] desIV)
{
    //Create the file streams to handle the input and output files. 
    FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
    FileStream fout = new FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write);
    fout.SetLength(0);

    //Create variables to help with read and write. 
    byte[] bin = new byte[100]; //This is intermediate storage for the encryption. 
    long rdlen = 0;              //This is the total number of bytes written. 
    long totlen = fin.Length;    //This is the total length of the input file. 
    int len;                     //This is the number of bytes to be written at a time. 

    DES des = new DESCryptoServiceProvider();
    CryptoStream encStream = new CryptoStream(fout, des.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write);

    //Read from the input file, then encrypt and write to the output file. 
    while (rdlen < totlen)
    {
        len = fin.Read(bin, 0, 100);
        encStream.Write(bin, 0, len);
        rdlen = rdlen + len;
    }

    encStream.Close();
    fout.Close();
    fin.Close();

}

 
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67
68
69
70
71
72
73
74
75
76
	
using System;
using System.Security.Cryptography;//这个是处理文字编码的前提
using System.Text;
using System.IO;
/// <summary>
/// DES加密方法
/// </summary>
/// <param name="strPlain">明文</param>
/// <param name="strDESKey">密钥</param>
/// <param name="strDESIV">向量</param>
/// <returns>密文</returns>
public string DESEncrypt(string strPlain, string strDESKey, string strDESIV)
{
    //把密钥转换成字节数组
    byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(strDESKey);
    //把向量转换成字节数组
    byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
    //声明1个新的DES对象
    DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
    //开辟一块内存流
    MemoryStream msEncrypt = new MemoryStream();
    //把内存流对象包装成加密流对象
    CryptoStream csEncrypt = new CryptoStream(msEncrypt, desEncrypt.CreateEncryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Write);
    //把加密流对象包装成写入流对象
    StreamWriter swEncrypt = new StreamWriter(csEncrypt);
    //写入流对象写入明文
    swEncrypt.WriteLine(strPlain);
    //写入流关闭
    swEncrypt.Close();
    //加密流关闭
    csEncrypt.Close();
    //把内存流转换成字节数组，内存流现在已经是密文了
    byte[] bytesCipher = msEncrypt.ToArray();
    //内存流关闭
    msEncrypt.Close();
    //把密文字节数组转换为字符串，并返回
    return UnicodeEncoding.Unicode.GetString(bytesCipher);
}




/// <summary>
/// DES解密方法
/// </summary>
/// <param name="strCipher">密文</param>
/// <param name="strDESKey">密钥</param>
/// <param name="strDESIV">向量</param>
/// <returns>明文</returns>
public string DESDecrypt(string strCipher, string strDESKey, string strDESIV)
{
    //把密钥转换成字节数组
    byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(strDESKey);
    //把向量转换成字节数组
    byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
    //把密文转换成字节数组
    byte[] bytesCipher = UnicodeEncoding.Unicode.GetBytes(strCipher);
    //声明1个新的DES对象
    DESCryptoServiceProvider desDecrypt = new DESCryptoServiceProvider();
    //开辟一块内存流，并存放密文字节数组
    MemoryStream msDecrypt = new MemoryStream(bytesCipher);
    //把内存流对象包装成解密流对象
    CryptoStream csDecrypt = new CryptoStream(msDecrypt, desDecrypt.CreateDecryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Read);
    //把解密流对象包装成读出流对象
    StreamReader srDecrypt = new StreamReader(csDecrypt);
    //明文=读出流的读出内容
    string strPlainText = srDecrypt.ReadLine();
    //读出流关闭
    srDecrypt.Close();
    //解密流关闭
    csDecrypt.Close();
    //内存流关闭
    msDecrypt.Close();
    //返回明文
    return strPlainText;
}