Imports System.Text
Imports System.Security.Cryptography


Public Class Encryption

#Region "FIELDS AND OBJECTS"

    'FIELDS AND OBJECTS

    Private _PublicKey As Byte()
    Private _Encryptor As ICryptoTransform
    Private _Decryptor As ICryptoTransform

    Private _Packer As New Serializer

#End Region

#Region "PROPERTIES"

    'PROPERTIES

    Public Property PublicKey As Byte()

        Get

            Return _PublicKey

        End Get

        Set(value As Byte())

            _PublicKey = value

        End Set

    End Property

#End Region

#Region "METHODS"

    'METHODS

#Region "HELPER METHODS"

    Public Function PrepareEncryption() As Byte()

        Dim RSA As New RSACryptoServiceProvider(2048)
        RSA.ImportCspBlob(_PublicKey)

        Dim R As New AesManaged()

        _Encryptor = R.CreateEncryptor()
        _Decryptor = R.CreateDecryptor()

        Dim Data As Byte() = _Packer.Serialize(R.Key, R.IV)
        Data = RSA.Encrypt(Data, True)

        Return Data

    End Function

    'ENCRYPTS DATA
    Public Function Encrypt(_Data As Byte()) As Byte()

        Return _Encryptor.TransformFinalBlock(_Data, 0, _Data.Length)

    End Function

    'DECRYPTS DATA
    Public Function Decrypt(_Data As Byte()) As Byte()

        Return _Decryptor.TransformFinalBlock(_Data, 0, _Data.Length)

    End Function

#End Region

#End Region

End Class
