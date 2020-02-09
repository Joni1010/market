
using MarketObjects;
using System.Collections.Generic;
using System.Linq;

namespace Libs
{
	/// <summary>
	/// Класс списка сохраняемых инструментов
	/// </summary>
	class SaveSec
	{
		/// <summary>
		/// Коллекция элементов
		/// </summary>
		private static Securities[] Collection = null;

		/// <summary>
		/// Возвращает список
		/// </summary>
		public static Securities[] GetListSec()
		{
			if (SaveSec.Collection.NotIsNull()) return SaveSec.Collection;
			return SaveSec.Collection;
		}
		/// <summary>
		/// Проверяет, есть ли элемент в списке разрешенных.
		/// </summary>
		/// <param name="ClassAndSec"></param>
		/// <returns></returns>
		public static bool Contains(string ClassAndSec)
		{
			var list = SaveSec.GetListSec();
			foreach (var el in list)
			{
				if (el.ToString().Contains(ClassAndSec)) return true;
			}
			return false;
		}
		/// <summary>
		/// Перезагружает список из файла
		/// </summary>
		public static void ReloadList(IEnumerable<Securities> listSec)
		{
			if (listSec.NotIsNull()) SaveSec.Collection = listSec.ToArray();
		}
	}
}