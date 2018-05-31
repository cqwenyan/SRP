using UnityEngine;using UnityEditor;using UnityEditor.Callbacks;

/// <summary>
/// 使用指定应用打开指定后缀为的文件 参考文献http://www.manew.com/thread-116044-1-1.html
/// </summary>
public class MyAssetHandler {
    // 应用路径
    [SerializeField] static string mEXEPath = "C:/Program Files/Sublime Text 3/sublime_text.exe";
    // 文件类型后缀（如：.shader)
    [SerializeField] static string mFileExtension = ".shader";

    [OnOpenAssetAttribute(1)]    public static bool Step1(int instanceID, int line) {
        return false; // we did not handle the open
    }

    // step2 has an attribute with index 2, so will be called after step1
    [OnOpenAssetAttribute(2)]    public static bool Step2(int instanceID, int line) {
        string path = AssetDatabase.GetAssetPath(EditorUtility.InstanceIDToObject(instanceID));        string name = Application.dataPath + "/" + path.Replace("Assets/", "");
        if (name.EndsWith(mFileExtension) == false)
            return false;

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = mEXEPath;
        startInfo.Arguments = name;
        process.StartInfo = startInfo;
        process.Start();
        return true;
    }}