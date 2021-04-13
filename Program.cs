using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REG_REPLACE
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		static void Main()
		{
			//RegistryKey root = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);//.OpenSubKey("Google"); //software setting
			foreach (var key in Registry.ClassesRoot.GetSubKeyNames())
			{
				try
				{
					GetREGValue(Registry.ClassesRoot.OpenSubKey(key, true));
				}
				catch { continue; }
			}
			foreach (var key in Registry.LocalMachine.GetSubKeyNames())
			{
				try
				{
					GetREGValue(Registry.ClassesRoot.OpenSubKey(key, true));
				}
				catch { continue; }
			}
			foreach (var key in Registry.CurrentUser.GetSubKeyNames())
			{
				try
				{
					GetREGValue(Registry.CurrentUser.OpenSubKey(key, true));
				}
				catch { continue; }				
			}

			//GetREGValue(root);
			Console.ReadLine();
		}

		public static void GetREGValue(RegistryKey key)
		{
			if (key.ValueCount > 0) // get value if has value
			{
				var vk = key.GetValueNames(); // value keys
				foreach (var k in vk)
				{
					var obj = key.GetValue(k); // get obj
					if (obj is string) obj = ((string)obj).Length == 0 ? "[empty]" : ((string)obj);
					Console.WriteLine("Location: " + key.Name + " Value Key: " + k + " => " + obj + " (" + obj.GetType().Name + ")");
					ModifyValue(key, k, obj);
				}
			}
			else // key node
			{
				if (key.SubKeyCount > 0) // if has sub key
				{
					var kk = key.GetSubKeyNames();
					foreach (var k in kk)
					{
						try
						{
							using (var tk = key.OpenSubKey(k, true))
							{
								GetREGValue(tk);
							}
						}catch(Exception e)
						{
							if (e is System.Security.SecurityException)
							{
								Console.WriteLine("SubKey: " + k + " Access Failed.");
							}
						}
					}
				}
				else
				{
					//Console.WriteLine("Key: " + key.Name + " has no deeper data.");
				}
			}
			key.Dispose();
		}

		public static void ModifyValue(RegistryKey reg, string regKey, object value)
		{
			if (value is string)
			{
				var tmp = (string)value;
				if (tmp.Contains("a2540") && !tmp.Contains("@")) // prevent email
				{
					reg.SetValue(regKey, tmp.Replace("a2540", "starx"));
					Console.WriteLine("Replaced: " + reg.Name + " KEY: " + regKey + " OVAL: " + tmp);
				}
			}
		}
	}
}
