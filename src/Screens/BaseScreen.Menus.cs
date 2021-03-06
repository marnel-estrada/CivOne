// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System.Collections.Generic;
using System.Linq;

namespace CivOne.Screens
{
	public abstract partial class BaseScreen
	{
		protected readonly List<IMenu> _menus = new List<IMenu>();

		protected bool HasMenu => _menus.Any();
		
		protected void AddMenu(IMenu menu)
		{
			_menus.Add(menu);
			Common.AddScreen(menu);
		}

		protected void CloseMenus(string menuId = null)
		{
			foreach (IMenu menu in _menus)
			{
				if (menuId != null && menu.Id != menuId) continue;
				menu.Close();
			}
			_menus.RemoveAll(x => menuId == null || x.Id == menuId);
		}
	}
}