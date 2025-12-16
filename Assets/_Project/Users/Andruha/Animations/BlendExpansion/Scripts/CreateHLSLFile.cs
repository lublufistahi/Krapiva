using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateHLSLFile
{
    [MenuItem("Assets/Create/HLSL File", false, 80)]
    private static void CreateHLSL()
    {
        string defaultContent =
@"// Íמגûי HLSL פאיכ
// Author: Unity
// Date: " + System.DateTime.Now.ToString("yyyy-MM-dd") + @"

";

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (!Directory.Exists(path))
        {
            path = Path.GetDirectoryName(path);
        }

        string filePath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, "NewShader.hlsl"));

        File.WriteAllText(filePath, defaultContent);
        AssetDatabase.Refresh();

        // Âûהוכÿול םמגûי פאיכ ג Project
        Object asset = AssetDatabase.LoadAssetAtPath<Object>(filePath);
        Selection.activeObject = asset;
        EditorGUIUtility.PingObject(asset);
    }
}
