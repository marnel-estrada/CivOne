// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System;
using System.Drawing;
using System.Windows.Forms;
using CivOne.Enums;
using CivOne.GFX;
using CivOne.Interfaces;
using CivOne.Templates;

namespace CivOne.Screens
{
	internal class GamePlay : BaseScreen
	{
		private readonly MenuBar _menuBar;
		private readonly SideBar _sideBar;
		private GameMenu _gameMenu = null;
		private int _menuX, _menuY;
		private bool _update = true;
		private bool _redraw = false;
		
		private void MenuBarGame(object sender, EventArgs args)
		{
			_gameMenu = new GameMenu(_canvas.Image.Palette.Entries);
			_gameMenu.Items.Add(new GameMenu.Item("Tax Rate"));
			_gameMenu.Items.Add(new GameMenu.Item("Luxuries Rate"));
			_gameMenu.Items.Add(new GameMenu.Item("FindCity"));
			_gameMenu.Items.Add(new GameMenu.Item("Options"));
			_gameMenu.Items.Add(new GameMenu.Item("Save Game") { Enabled = false });
			_gameMenu.Items.Add(new GameMenu.Item("REVOLUTION!"));
			_gameMenu.Items.Add(new GameMenu.Item(null));
			_gameMenu.Items.Add(new GameMenu.Item("Retire"));
			_gameMenu.Items.Add(new GameMenu.Item("QUIT to DOS"));
			
			_menuX = 16;
			_menuY = 8;
			
			_update = true;
		}
		
		private void MenuBarOrders(object sender, EventArgs args)
		{
			_gameMenu = new GameMenu(_canvas.Image.Palette.Entries);
			_gameMenu.Items.Add(new GameMenu.Item("No Orders", "space"));
			_gameMenu.Items.Add(new GameMenu.Item("Found New City", "b"));
			_gameMenu.Items.Add(new GameMenu.Item("Build Road", "r"));
			_gameMenu.Items.Add(new GameMenu.Item("Build Irrigation", "i"));
			_gameMenu.Items.Add(new GameMenu.Item("Change to Forest", "m"));
			_gameMenu.Items.Add(new GameMenu.Item("Build Fortress", "f") { Enabled = false });
			_gameMenu.Items.Add(new GameMenu.Item("Wait", "w"));
			_gameMenu.Items.Add(new GameMenu.Item("Sentry", "s"));
			_gameMenu.Items.Add(new GameMenu.Item("GoTo"));
			_gameMenu.Items.Add(new GameMenu.Item(null));
			_gameMenu.Items.Add(new GameMenu.Item("Disband Unit", "D"));
			
			_menuX = 72;
			_menuY = 8;
			
			_update = true;
		}
		
		private void MenuBarAdvisors(object sender, EventArgs args)
		{
			_gameMenu = new GameMenu(_canvas.Image.Palette.Entries);
			_gameMenu.Items.Add(new GameMenu.Item("City Status (F1)"));
			_gameMenu.Items.Add(new GameMenu.Item("Military Advisor (F2)"));
			_gameMenu.Items.Add(new GameMenu.Item("Intelligence Advisor (F3)"));
			_gameMenu.Items.Add(new GameMenu.Item("Attitude Advisor (F4)"));
			_gameMenu.Items.Add(new GameMenu.Item("Trade Advisor (F5)"));
			_gameMenu.Items.Add(new GameMenu.Item("Science Advisor (F6)"));
			
			_menuX = 112;
			_menuY = 8;
			
			_update = true;
		}
		
		private void MenuBarWorld(object sender, EventArgs args)
		{
			_gameMenu = new GameMenu(_canvas.Image.Palette.Entries);
			_gameMenu.Items.Add(new GameMenu.Item("Wonders of the World (F7)"));
			_gameMenu.Items.Add(new GameMenu.Item("Top 5 Cities (F8)"));
			_gameMenu.Items.Add(new GameMenu.Item("Civilization Score (F9)"));
			_gameMenu.Items.Add(new GameMenu.Item("World Map (F10)"));
			_gameMenu.Items.Add(new GameMenu.Item("Demographics"));
			_gameMenu.Items.Add(new GameMenu.Item("SpaceShips") { Enabled = false });
			
			_menuX = 144;
			_menuY = 8;
			
			_update = true;
		}
		
		private void MenuBarCivilopedia(object sender, EventArgs args)
		{
			_gameMenu = new GameMenu(_canvas.Image.Palette.Entries);
			_gameMenu.Items.Add(new GameMenu.Item("Complete"));
			_gameMenu.Items.Add(new GameMenu.Item("Civilization Advances"));
			_gameMenu.Items.Add(new GameMenu.Item("City Improvements"));
			_gameMenu.Items.Add(new GameMenu.Item("Military Units"));
			_gameMenu.Items.Add(new GameMenu.Item("Terrain Types"));
			_gameMenu.Items.Add(new GameMenu.Item("Miscellaneous"));
			
			_menuX = 182;
			_menuY = 8;
			
			_update = true;
		}
		
		private void DrawLayer(IScreen layer, uint gameTick, int x, int y)
		{
			if (layer == null) return;
			if (!layer.HasUpdate(gameTick) && !_redraw) return;
			_canvas.AddLayer(layer.Canvas.Image, x, y);
		}
		
		public override bool HasUpdate(uint gameTick)
		{
			if (!_update && !_redraw) return false;
			
			_canvas.FillRectangle(5, 80, 8, 240, 192);
			
			DrawLayer(_menuBar, gameTick, 0, 0);
			DrawLayer(_sideBar, gameTick, 0, 8);
			DrawLayer(_gameMenu, gameTick, _menuX, _menuY);
			
			_redraw = false;
			_update = false;
			return true;
		}
		
		public override bool MouseDown(MouseEventArgs args)
		{
			if (args.Y < 8)
			{
				return _menuBar.MouseDown(args);
			}
			return false;
		}
		
		public override bool MouseUp(MouseEventArgs args)
		{
			if (_gameMenu == null) return false;
			
			_gameMenu = null;
			_redraw = true;
			return true;
		}
		
		public override bool MouseDrag(MouseEventArgs args)
		{
			if (_gameMenu == null) return false;
			
			MouseArgsOffset(ref args, _menuX, _menuY);
			_update |= _gameMenu.MouseDrag(args);
			return _update;
		}
		
		public GamePlay()
		{
			Cursor = MouseCursor.Pointer;
			
			Color[] palette = Resources.Instance.LoadPIC("SP257").Image.Palette.Entries;
			
			_canvas = new Picture(320, 200, palette);
			_canvas.FillRectangle(1, 0, 0, 320, 200);
			_canvas.DrawText("Gameplay placeholder", 3, 15, 160, 160, TextAlign.Center);
			
			_menuBar = new MenuBar(palette);
			_sideBar = new SideBar(palette);
			
			_menuBar.GameSelected += MenuBarGame;
			_menuBar.OrdersSelected += MenuBarOrders;
			_menuBar.AdvisorsSelected += MenuBarAdvisors;
			_menuBar.WorldSelected += MenuBarWorld;
			_menuBar.CivilopediaSelected += MenuBarCivilopedia;
		}
	}
}