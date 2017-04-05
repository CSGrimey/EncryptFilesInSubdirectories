using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncryptFilesInSubdirectories {
    [TestClass]
    public class EncryptionServiceTests {
        private readonly EncryptionService _encryptionService = new EncryptionService();

        [TestMethod]
        public void ShouldEncryptAndDecryptUsingTDES() {
            const string inputTestFilePath = "Slides07.pdf";
            const string outputTestFilePath = "encryptedFile.pdf";

            _encryptionService.Encrypt(inputTestFilePath, outputTestFilePath);

            byte[] originalPDF = File.ReadAllBytes(inputTestFilePath);
            byte[] encryptedPDF = File.ReadAllBytes(outputTestFilePath);

            Assert.IsFalse(originalPDF.SequenceEqual(encryptedPDF));

            const string decryptedPDFFilePath = "decryptedFile.pdf";

            _encryptionService.Decrypt(outputTestFilePath, decryptedPDFFilePath);

            byte[] decryptedPDF = File.ReadAllBytes(decryptedPDFFilePath);

            Assert.IsTrue(originalPDF.SequenceEqual(decryptedPDF));
            Assert.IsFalse(encryptedPDF.SequenceEqual(decryptedPDF));
        }
    }
}