using System;
using System.IO;
using Voxels;

namespace Vox2Cub
{
    class Vox2CubApp
    {
        static void Main(string[] args)
        {
            string inputDirPath = GetInputDirectory();
            DirectoryInfo inputDirectory = new DirectoryInfo(inputDirPath);
            string outputDir = Environment.CurrentDirectory +
                @"\Vox2Cub Converted Files\";

            StageOutputDirectory(outputDir);

            var inputFiles = inputDirectory.GetFiles();
            int progress = 1;
            int fileCount = inputFiles.Length;

            foreach (var file in inputFiles)
            {
                var importedData = VoxelImport.Import(file.FullName);
                var origFileName = Path.GetFileNameWithoutExtension(file.FullName);
                string outputFilePath = outputDir + origFileName + ".cub";
                Console.WriteLine(progress + "/" + fileCount
                    + " - " + origFileName);

                // Voxels library only works with .vox and .qb files.
                // .vox functionality removed temporarily due to issues with format
                if (file.Extension == ".qb")
                    CubExport.Export(importedData, outputFilePath);
                progress++;
            }

            Console.WriteLine("Success! All converted files are in \""
                + outputDir + "\"");
            Console.ReadLine();
        }

        static string GetInputDirectory()
        {
            Console.Write("vox/cb file folder: ");
            string targetDir = Console.ReadLine();
            try
            {
                VerifyDirectoryPath(targetDir);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("The path \"" + targetDir + "\" " +
                    "is invalid. Please enter a valid folder path.");
                targetDir = GetInputDirectory();
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("Insufficient permissions to access \"" 
                    + targetDir + "\".");
                Console.WriteLine("Select another folder or run this " +
                    "application as an administrator and try again.");
                targetDir = GetInputDirectory();
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("The path \"" + targetDir + "\" " +
                    "contains a colon ':' outside of the volume identifier." +
                    " Please try again.");
                targetDir = GetInputDirectory();
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("The path \" " + targetDir + "\" " +
                    "is too long. Please enter a valid folder path.");
                targetDir = GetInputDirectory();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The path \" " + targetDir + "\" " +
                    "could not be found. Please enter a valid folder path.");
                targetDir = GetInputDirectory();
            }

            return targetDir;
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
