// ��������������ռ�
using System.Reflection;
using HarmonyLib;

// ����һ����ΪModStartup�Ĺ����࣬�̳���IModApi�ӿ�
public class ModStartup : IModApi
{
    // ��ʼ������
    public void InitMod(Mod modInstance)
    {
        // ����Assembly-CSharp.dll
        Assembly executeAssembly = Assembly.GetExecutingAssembly();

        // ����Harmonyʵ��
        Harmony harmony = new Harmony(executeAssembly.GetName().Name);

        // �򲹶�
        harmony.PatchAll(executeAssembly);
    }
}