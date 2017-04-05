using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EncryptFilesInSubdirectories {
    class Program {
        /// <summary>
        /// This console application will run through the documents in the specified directory and encrypt them using Triple DES.
        /// Any sub directory that contains files that shouldn't be encrypted must be specified in the proceeding arguments passed in.
        /// Example: "C:/SomeDirectory" "PersonalPhotos" "Transcripts"
        /// The above example will ignore files inside the personal photos and transcripts sub directories.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {
            if (args.Length < 1)
                throw new Exception("You must provide a directory that contains subdirectories of the files you wish to encrypt.");

            string baseDirectory = args[0];

            if (Directory.Exists(baseDirectory) == false)
                throw new Exception("The specified base directory does not exist or this code does not have the require permissions.");

            Console.WriteLine($"Looking in {baseDirectory} for files to encrypt.");

            bool argumentsContainSubDirectoriesToIgnore = args.Length > 1;

            HashSet<string> subDirectoriesToIgnore = new HashSet<string>();

            if (argumentsContainSubDirectoriesToIgnore)
                subDirectoriesToIgnore = new HashSet<string>(args.Skip(1).Select(subDirectory => subDirectory.Trim()));

            var encryptionService = new EncryptionService();

            string[] subDirectories = Directory.GetDirectories(baseDirectory);

            foreach (string subDirectory in subDirectories) {
                string[] filePaths = Directory.GetFiles(subDirectory);

                bool shouldIgnoreSubDirectory = subDirectoriesToIgnore.Contains(new DirectoryInfo(subDirectory).Name);

                if (shouldIgnoreSubDirectory) Console.WriteLine($"Ignoring sub directory {subDirectory}");
                else {
                    Console.WriteLine($"Encrypting files in sub directory {subDirectory}");

                    foreach (string filePath in filePaths)
                        encryptionService.Encrypt(new MemoryStream(File.ReadAllBytes(filePath)), filePath); // Bad way to do this, but this is throwaway code.
                }
            }

            Console.WriteLine("Finished.");
        }
    }
}