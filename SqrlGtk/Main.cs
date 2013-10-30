using System;
using Gtk;

namespace SqrlGtk
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init();
			MainWindow win = new MainWindow(args[0]);
			win.Show();
			Application.Run();
		}
	}
}
