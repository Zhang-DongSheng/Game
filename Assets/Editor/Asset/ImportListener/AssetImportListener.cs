using UnityEditor.AssetImporters;
using UnityEngine;

namespace UnityEditor
{
    /// <summary>
    /// Scripted Importer 是 Unity Scripting API 的一部分。使用Scripted Importer在C# 中编写自定义资源导入器，用于Unity本身不支持的文件格式。
    /// 应通过专门定义抽象类 ScriptedImporter 并应用 ScriptedImporter 属性来创建自定义导入器。这样会注册您的自定义导入器以处理一个或多个文件扩展名。当资源管线检测到一个与注册的文件扩展名匹配的文件为新文件或已更改的文件时，Unity 会调用自定义导入器的 OnImportAsset 方法。
    /// 注意：Scripted Importer 无法处理已由 Unity 本身处理的文件扩展名
    /// </summary>
    [ScriptedImporter(0, "unity")]
    public class AssetImportListener : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            Debuger.LogError(Author.None, ctx.assetPath);
        }
    }
}
