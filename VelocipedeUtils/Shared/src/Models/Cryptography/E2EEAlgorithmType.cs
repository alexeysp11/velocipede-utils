namespace VelocipedeUtils.Shared.Models.Cryptography;

/// <summary>
/// End to end encryption algorithm type.
/// </summary>
public enum E2EEAlgorithmType
{
    AES,
    DES,
    TrippleDES, 
    RSA, 
    Blowfish, 
    Twofish, 
    RC4,
    TEA, 
    xxTEA
}