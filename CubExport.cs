using System.IO;
using Voxels;

namespace Vox2Cub
{
    class CubExport
    {
        public static void Export(VoxelData data, string path)
        {
            using (BinaryWriter writeBinary = new BinaryWriter(File.Open
                (path, FileMode.Create)))
            {
                writeBinary.Write(data.size.X);
                writeBinary.Write(data.size.Y);
                writeBinary.Write(data.size.Z);

                for (int z = 0; z < data.size.Z; z++)
                    for (int y = 0; y < data.size.Y; y++)
                        for (int x = 0; x < data.size.X; x++)
                        {
                            var currentPos = new XYZ(x, y, z);
                            if (data.IsValid(currentPos))
                            {
                                var voxelColor = data[currentPos].Color;
                                writeBinary.Write(voxelColor.R);
                                writeBinary.Write(voxelColor.G);
                                writeBinary.Write(voxelColor.B);
                            }
                            else
                                writeBinary.Write(000);
                        }
                writeBinary.Close();
            }
        }
    }
}
