using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppVEConector.libs.Proceses
{
	public class MProcess
	{
		/// <summary>
		/// Закрывает Quik терминал
		/// </summary>
		public static void CloseTerminal()
		{
			foreach (var process in Process.GetProcessesByName("info"))
			{
				if (process.MainModule.ModuleName.Contains("info.exe")
					&& process.MainModule.FileVersionInfo.InternalName.Contains("QUIK"))
				{
					process.Kill();
				}
			}
		}

		public static void RestartTerminal()
		{
			foreach (var process in Process.GetProcessesByName("info"))
			{
				if (process.MainModule.ModuleName.Contains("info.exe")
					&& process.MainModule.FileVersionInfo.InternalName.Contains("QUIK"))
				{
					string fileName = process.MainModule.FileName;
					process.Kill();
					Thread.Sleep(5000);
					Process.Start(fileName);
				}
			}
			
		}
	}
}
