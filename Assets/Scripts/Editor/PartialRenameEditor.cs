using UnityEditor;
using UnityEngine;

public class PartialRenameEditor : EditorWindow
{
    private string textToReplace = "";
    private string replacementText = "";

    [MenuItem("Tools/Partial Rename Assets")]
    public static void ShowWindow()
    {
        GetWindow<PartialRenameEditor>("Partial Rename Assets");
    }

    private void OnGUI()
    {
        GUILayout.Label("Partial Rename Selected Assets", EditorStyles.boldLabel);

        textToReplace = EditorGUILayout.TextField("Text to Replace", textToReplace);
        replacementText = EditorGUILayout.TextField("Replacement Text", replacementText);

        if (GUILayout.Button("Rename Selected Assets"))
        {
            RenameSelectedAssets();
        }
    }

    private void RenameSelectedAssets()
    {
        Object[] selectedAssets = Selection.objects;
        if (selectedAssets.Length == 0)
        {
            EditorUtility.DisplayDialog("No Selection", "Please select assets to rename.", "OK");
            return;
        }

        int renamedCount = 0;

        foreach (Object asset in selectedAssets)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            string originalName = asset.name;

            if (originalName.Contains(textToReplace))
            {
                string newName = originalName.Replace(textToReplace, replacementText);
                AssetDatabase.RenameAsset(path, newName);
                renamedCount++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Renamed {renamedCount} assets.");
    }
}
