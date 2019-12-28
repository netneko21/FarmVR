//
// ShaderGraphEssentials for Unity
// (c) 2019 PH Graphics
// Source code may be used and modified for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 
// *** A NOTE ABOUT PIRACY ***
// 
// If you got this asset from a pirate site, please consider buying it from the Unity asset store. This asset is only legally available from the Unity Asset Store.
// 
// I'm a single indie dev supporting my family by spending hundreds and thousands of hours on this and other assets. It's very offensive, rude and just plain evil to steal when I (and many others) put so much hard work into the software.
// 
// Thank you.
//
// *** END NOTE ABOUT PIRACY ***

using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SGELoader.Editor
{
    #if UNITY_2019_1_OR_NEWER
    [InitializeOnLoad]
    public class SGEAssemblyLoader
    {
        private const bool LogWhatsHappening = false;
        static SGEAssemblyLoader()
        {
            RegisterSGE(LogWhatsHappening);
        }
        
        [MenuItem("Tools/ShaderGraph Essentials/Debug registration")]
        private static void RegisterSGEMenuItem()
        {
            RegisterSGE(true);
        }

        static void RegisterSGE(bool log)
        {
            var basePath = Path.GetFullPath(Application.dataPath);
            var projectFolder = Directory.GetParent(basePath);
            var packageCacheFolder = Path.Combine(new[] {projectFolder.FullName, "Library", "PackageCache"});
            
            // let's find what shadergraph package is installed
            // there's no way to find that through an API at the moment
            // so I scan the folder "PackageCache" to see if one contains "shadergraph"
            // I verified that when you upgrade, the old folder is removed from here

            if (!Directory.Exists(packageCacheFolder))
            {
                Debug.Log("PackageCache folder doesn't exist at " + packageCacheFolder + "! How is it possible ? Please contact me at ph.graphics.unity@gmail.com and I'll help you");
                return;
            }

            var packageDirectories = Directory.GetDirectories(packageCacheFolder);
            bool shadergraphPackageFound = false;
            string shadergraphPackageDirectory = null;
            for (int i = 0; i < packageDirectories.Length; i++)
            {
                var directory = packageDirectories[i];
                if (Regex.IsMatch(directory, "shadergraph"))
                {
                    shadergraphPackageFound = true;
                    shadergraphPackageDirectory = directory;
                    if (log)
                        Debug.Log("ShaderGraph package directory found at " + shadergraphPackageDirectory);
                    break;
                }
            }

            if (!shadergraphPackageFound)
            {
                Debug.LogError("ShaderGraph package not found. Are you sure you imported ShaderGraph package before adding ShaderGraphEssentials ?");
                return;
            }
            
            var filepath = Path.Combine(new[] {packageCacheFolder, shadergraphPackageDirectory, "Editor", "AssemblyInfo.cs"});

            if (!File.Exists(filepath))
            {
                Debug.LogError("ShaderGraph package file not found at" + filepath + "\n Are you sure you imported ShaderGraph package before adding ShaderGraphEssentials ?");
                return;
            }
            
            var text = File.ReadAllText(filepath);
            if (Regex.IsMatch(text, "ShaderGraphEssentials"))
            {
                if (log)
                    Debug.Log("ShaderGraphEssentials already registered ! Everything is fine !");
                return;
            }
                
            // get permission to write the file
            // add access for everyone
            // it's ok, this file is erased every time Unity is open
            // so if someone doesn't use SGE anymore, permissions will revert back next time you close / open unity
#if UNITY_EDITOR_WIN
            var assemblyFile = new FileInfo(filepath);
            var security = assemblyFile.GetAccessControl();
            security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), 
                FileSystemRights.FullControl, AccessControlType.Allow));
            assemblyFile.SetAccessControl(security);
                    
            if (log)
                Debug.Log("Changed rights to this file on windows: " + filepath);
#elif UNITY_EDITOR_OSX
                    // not so clean way but I haven't found any other way on mac
                    Process.Start("chmod", "777 " + filepath);

                    if (log)
                        Debug.Log("Changed rights to this file on osx: " + filepath);
#endif
                
            // also remove the readonly flag
            File.SetAttributes(filepath, File.GetAttributes(filepath) & ~FileAttributes.ReadOnly);
                
            using (var stream = File.AppendText(filepath))
            {
                stream.Write("\n[assembly: InternalsVisibleTo(\"ShaderGraphEssentials\")]");
                if (log)
                    Debug.Log("ShaderGraphEssentials correctly registered ! Everything is fine !");
            }
            
            // now reimport ShaderGraphEssentials asmdef to be sure it's taking the changes we just made
            // right now the ShaderGraph path is hardcoded everywhere, this should be fixed at some point
            // it's quite easy in C#, much more difficult in shaders
            const string shaderGraphAsmDef = "Assets/Plugins/ShaderGraphEssentials/Plugin/Editor/ShaderGraphEssentials.asmdef";
            if (!File.Exists(shaderGraphAsmDef))
            {
                Debug.LogError("ShaderGraphEssentials asmdef can't be found at path " + shaderGraphAsmDef);
                return;
            }
            
            AssetDatabase.ImportAsset(shaderGraphAsmDef, ImportAssetOptions.ForceUpdate);
        }
    }
    #endif
}
