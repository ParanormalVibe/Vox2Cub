using System;
using System.IO;
using Voxels;

namespace Vox2Cub {
    class Vox2CubApp {
        static void Main(string[] args) {
            string inputPath = GetInputDirectory();
            string outputPath = Environment.CurrentDirectory +
                "\\Vox2Cub Converted Files\\";

            Directory.CreateDirectory(outputPath);
            ConvertDirectory(inputPath, outputPath);

            Console.WriteLine("Success! All converted files are in \"{0}\"\"",
                outputPath);
            Console.ReadLine();
        }

        static string GetInputDirectory() {
            Console.Write("File folder with .vox/.qb files: ");
            string inputPath = Console.ReadLine();
            try {
                VerifyDirectoryPath(inputPath);
            }
            catch (ArgumentException) {
                Console.WriteLine("The path \"{0}\" is invalid. " +
                    "Please enter a valid folder path.", inputPath);
                inputPath = GetInputDirectory();
            }
            catch (System.Security.SecurityException) {
                Console.WriteLine("Insufficient permissions to access" +
                    "\"{0}\".{1}Select another folder or run this " +
                    "application as administrator and try again.",
                    inputPath, Environment.NewLine);
                inputPath = GetInputDirectory();
            }
            catch (NotSupportedException) {
                Console.WriteLine("The path \"{0}\" contains a colon ':' " +
                    "outside of the volume identifier. Please try again.",
                    inputPath);
                inputPath = GetInputDirectory();
            }
            catch (PathTooLongException) {
                Console.WriteLine("The path \"{0}\" is too long. Please " +
                    "enter a valid folder path.", inputPath);
                inputPath = GetInputDirectory();
            }
            catch (DirectoryNotFoundException) {
                Console.WriteLine("The path \"{0}\" could not be found. " +
                    "Please nter a valid folder path.", inputPath);
                inputPath = GetInputDirectory();
            }

            return inputPath;
        }

        static void VerifyDirectoryPath(string path) {
            Path.GetFullPath(path);
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException();
        }

        static void ConvertDirectory(string inputPath, string outputPath) {
            var inputFiles = new DirectoryInfo(inputPath).GetFiles();
            int fileCount = inputFiles.Length;

            int progress = 1;
            foreach (var file in inputFiles) {
                Console.WriteLine("Importing {0}/{1}:\"{2}\"", progress,
                    inputFiles.Length, file.Name);
                var voxData = ImportVoxFile(file.FullName);
                if (voxData != null) {
                    Console.WriteLine("Exporting {0}/{1}:\"{2}\"", progress,
                        inputFiles.Length, file.Name);
                    var outputFilePath = GetOutputFilePath(outputPath,
                        Path.GetFileNameWithoutExtension(file.FullName));
                    CubExport.Export(voxData, outputFilePath);
                }
                progress++;
            }
        }

        static string GetOutputFilePath(string outputDirPath, string fileName) {
            return outputDirPath + fileName + ".cub";
        }

        public static VoxelData ImportVoxFile(string filePath) {
            var importedData = VoxelImport.Import(filePath);
            if (importedData == null)
                Console.WriteLine("Unable to read file {0}", filePath);
            return importedData;
        }
    }
}