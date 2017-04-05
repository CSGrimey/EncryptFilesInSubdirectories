using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptFilesInSubdirectories {
    public class EncryptionService {
        private byte[] GetKey() => Encoding.ASCII.GetBytes("Enter some hash here that is 24 characters long");

        private byte[] GetInitialisationVector() => Encoding.ASCII.GetBytes("Enter a hash here that is 8 characters long");

        public void Encrypt(string inputFileName, string outputFileName) {
            Encrypt(inputFileName, outputFileName, GetKey(), GetInitialisationVector());
        }

        public void Encrypt(Stream inputStream, string outputFileName) {
            FileStream outputFileStream = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);

            outputFileStream.SetLength(0);

            Transform(inputStream, outputFileStream, GetKey(), GetInitialisationVector(), new TripleDESCryptoServiceProvider().CreateEncryptor);
        }

        public void Encrypt(string inputFileName, string outputFileName, byte[] key, byte[] initialisationVector) {
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
            FileStream outputFileStream = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            outputFileStream.SetLength(0);

            Transform(inputFileStream, outputFileStream, key, initialisationVector, new TripleDESCryptoServiceProvider().CreateEncryptor);
        }

        private void Transform(Stream inputFileStream, Stream outputFileStream, byte[] key, byte[] initialisationVector,
            Func<byte[], byte[], ICryptoTransform> transformerCreationFunc) {
            byte[] immediateEncryptionStorage = new byte[1000];
            int numberOfBytesToWriteAtATime = 0;
            long totalBytesWritten = 0;
            long inputFileSize = inputFileStream.Length;
            const int offset = 0;

            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();

            ICryptoTransform transformer = transformerCreationFunc(key, initialisationVector);

            using (var cryptoStream = new CryptoStream(outputFileStream, transformer, CryptoStreamMode.Write)) {
                while (totalBytesWritten < inputFileSize) {
                    numberOfBytesToWriteAtATime = inputFileStream.Read(immediateEncryptionStorage, offset, count: 100);

                    cryptoStream.Write(immediateEncryptionStorage, offset, numberOfBytesToWriteAtATime);

                    totalBytesWritten += numberOfBytesToWriteAtATime;
                }
            }
        }

        public void Decrypt(string inputFileName, string outputFileName) {
            Decrypt(inputFileName, outputFileName, GetKey(), GetInitialisationVector());
        }

        public void Decrypt(string inputFileName, string outputFileName, byte[] key, byte[] initialisationVector) {
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
            FileStream outputFileStream = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            outputFileStream.SetLength(0);

            Transform(inputFileStream, outputFileStream, key, initialisationVector, new TripleDESCryptoServiceProvider().CreateDecryptor);
        }
    }
}