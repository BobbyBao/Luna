using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using SharpLuna.Unity;
using System.IO;
using System.Text;

[ScriptedImporter(1, new[] { "lua", "luna", "pbc", "pb" })]
public class ByteAssetImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var fileName = System.IO.Path.GetFileNameWithoutExtension(ctx.assetPath);
        BytesAsset ba = ScriptableObject.CreateInstance<BytesAsset>();
        ba.bytes = File.ReadAllBytes(ctx.assetPath);
        ctx.AddObjectToAsset(fileName, ba);
        ctx.SetMainObject(ba);
    }


    [MenuItem("Assets/Create/LuaScript")]
    static void CreateLuaScript()
    {
        string _path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (File.Exists(_path))
            _path = Path.GetDirectoryName(_path);

        string assetPath = Path.Combine(_path, "LuaScript.lua");
        FileInfo fileInfo = new FileInfo(assetPath);
        if (fileInfo.Exists)
        {
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

        }

        using (File.Open(assetPath, FileMode.CreateNew))
        {

        }

        AssetDatabase.Refresh();
    }

}

[CustomEditor(typeof(BytesAsset))]
public class ByteAssetEditor : UnityEditor.Editor
{
    Vector2 scrololPos;
    public override void OnInspectorGUI()
    {
        BytesAsset text = (BytesAsset)target;
        scrololPos = GUILayout.BeginScrollView(scrololPos);
        GUILayout.TextArea(Encoding.UTF8.GetString(text.bytes));
        GUILayout.EndScrollView();
    }
}
