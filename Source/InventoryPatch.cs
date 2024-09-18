using System;
using System.IO;
using System.Reflection;
using System.Xml;
using HarmonyLib;

[HarmonyPatch]
public class InventoryPatch
{
	private static int _ToolbeltSlots = 12;

	[HarmonyPatch(typeof(Inventory))]
	[HarmonyPatch("get_PUBLIC_SLOTS_PLAYMODE")]
	private static bool Prefix(Inventory __instance, ref int __result)
	{
		__result = _ToolbeltSlots;
		string xmlFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Config/XUi/windows.xml");
		if (File.Exists(xmlFile))
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(xmlFile);
				int ToolbeltSlots = int.Parse(xmlDocument.GetElementsByTagName("set")[0].InnerText.Trim());
				_ToolbeltSlots = ToolbeltSlots;
				__result = _ToolbeltSlots;
			}
			catch (Exception)
			{
			}
		}
		return false;
	}
}
