using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

[HarmonyPatch]
public class ToolbeltShortcutPatch
{
    [HarmonyPatch(typeof(PlayerMoveController), "Update")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codeList = new List<CodeInstruction>(instructions);

        // 查找ShiftKeyPressed和AltKeyPressed属性的Get方法
        PropertyInfo shiftKeyPressedProperty = typeof(InputUtils).GetProperty("ShiftKeyPressed");
        PropertyInfo altKeyPressedProperty = typeof(InputUtils).GetProperty("AltKeyPressed");

        if (shiftKeyPressedProperty == null)
        {
            Debug.LogError("<color=#00FF00>CATUI [ToolbeltShortcutPatch] 无法找到InputUtils.ShiftKeyPressed属性</color>");
            return codeList;
        }

        if (altKeyPressedProperty == null)
        {
            Debug.LogError("<color=#00FF00>CATUI [ToolbeltShortcutPatch] 无法找到InputUtils.AltKeyPressed属性</color>");
            return codeList;
        }

        MethodInfo shiftGetter = shiftKeyPressedProperty.GetGetMethod();
        MethodInfo altGetter = altKeyPressedProperty.GetGetMethod();

        int replaceCount = 0;

        // 查找并替换所有ShiftKeyPressed的调用
        for (int i = 0; i < codeList.Count; i++)
        {
            // 检查是否 调用ShiftKeyPressed的get方法
            if (codeList[i].opcode == OpCodes.Call && (codeList[i].operand as MethodInfo) == shiftGetter)
            {
                codeList[i].operand = altGetter;
                replaceCount++;
                Debug.Log("<color=#00FF00>CATUI [ToolbeltShortcutPatch] 替换了一处ShiftKeyPressed调用 (OpCodes.Call)</color>");
            }
            // 检查是否 virt调用（一般情况下不会出现）
            else if (codeList[i].opcode == OpCodes.Callvirt && (codeList[i].operand as MethodInfo) == shiftGetter)
            {
                codeList[i].operand = altGetter;
                replaceCount++;
                Debug.Log("<color=#00FF00>CATUI [ToolbeltShortcutPatch] 替换了一处ShiftKeyPressed调用 (OpCodes.Callvirt)</color>");
            }
        }

        Debug.Log("<color=#00FF00>CATUI [ToolbeltShortcutPatch] 共替换了 " + replaceCount + " 处ShiftKeyPressed调用</color>");
        return codeList;
    }
}