using System;
using System.IO;
using Voxels;

namespace Vox2Cub
{
    class Vox2CubApp
    {
        static void Main(string[] args)
        {
            string inputPath = GetInputDirectory();
            DirectoryInfo inputDirectory = new DirectoryInfo(inputPath);
            string outputPath = Environment.CurrentDirectory +
                @"\Vox2Cub Converted Files\";

            StageOutputDirectory(outputPath);
            ExportDirectory(inputDirectory, outputPath);

            Console.WriteLine("Success! All converted files are in \""
                + outputPath + "\"");
            Console.ReadLine();
        }

        static string GetInputDirectory()
        {
            Console.Write("vox/cb file folder: ");
            string inputPath = Console.ReadLine();
            try
            {
                VerifyDirectoryPath(inputPath);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("The path \"" + inputPath + "\" " +
                    "is invalid. Please enter a valid folder path.");
                inputPath = GetInputDirectory();
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("Insufficient permissions to access \"" 
                    + inputPath + "\".");
                Console.WriteLine("Select another folder or run this " +
                    "application as an administrator and try again.");
                inputPath = GetInputDirectory();
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("The path \"" + inputPath + "\" " +
                    "contains a colon ':' outside of the volume identifier." +
                    " Please try again.");
                inputPath = GetInputDirectory();
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("The path \" " + inputPath + "\" " +
                    "is too long. Please enter a valid folder path.");
                inputPath = GetInputDirectory();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The path \" " + inputPath + "\" " +
                    "could not be found. Please enter a valid folder path.");
                inputPath = GetInputDirectory();
            }

            return inputPath;
        }

        static void ExportDirectory(DirectoryInfo inputDirectory,
            string outputPath)
        {
            var inputFiles = inputDirectory.GetFiles();
            int fileCount = inputFiles.Length;
            int progress = 1;

            foreach (var file in inputFiles)
            {
                var importedData = VoxelImport.Import(file.FullName);
                var fileName = Path.GetFileNameWithoutExtension(file.FullName);
                string outputFilePath = outputPath + fileName + ".cub";
                Console.WriteLine(progress + "/" + fileCount
                    + " - " + fileName);

                // Voxels library only works with .vox and .qb files.
                // .vox functionality removed temporarily due to issues with format
                if (file.Extension == ".qb")
                    CubExport.Export(importedData, outputFilePath);
                progress++;
            }
        }

        static void StageOutputDirectory(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
        }

        static void VerifyDirectoryPath(string path)
        {
            Path.GetFullPath(path);
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException();
        }
    }
}
