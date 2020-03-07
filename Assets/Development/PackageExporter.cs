using System.IO;
using UnityEngine;
using UnityEditor;

namespace Development
{
    public static class PackageExporter
    {
        [MenuItem("Export/Export Unity Package")]
        public static void ExportUnityPackage()
        {
            var libraryDirectories = new[]
            {
                "Assets/ApeTest"
            };

            var outputFilePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "ApeTest.unitypackage");

            Debug.LogFormat("Export package to {0}", outputFilePath);
            AssetDatabase.ExportPackage(libraryDirectories, outputFilePath, ExportPackageOptions.Recurse);
            Debug.LogFormat("Success");
        }
    }
}
