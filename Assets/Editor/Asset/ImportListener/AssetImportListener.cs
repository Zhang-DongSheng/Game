using UnityEditor.AssetImporters;
using UnityEngine;

namespace UnityEditor
{
    /// <summary>
    /// Scripted Importer �� Unity Scripting API ��һ���֡�ʹ��Scripted Importer��C# �б�д�Զ�����Դ������������Unity����֧�ֵ��ļ���ʽ��
    /// Ӧͨ��ר�Ŷ�������� ScriptedImporter ��Ӧ�� ScriptedImporter �����������Զ��嵼������������ע�������Զ��嵼�����Դ���һ�������ļ���չ��������Դ���߼�⵽һ����ע����ļ���չ��ƥ����ļ�Ϊ���ļ����Ѹ��ĵ��ļ�ʱ��Unity ������Զ��嵼������ OnImportAsset ������
    /// ע�⣺Scripted Importer �޷��������� Unity ��������ļ���չ��
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
