using System;
using Microsoft.Win32;
namespace WindowsColorConverter
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Write mode:\n1-convert hex to system\t\t2-convert from hex to system\nYou can set color to cmd by writing 'bg' or 'fg'.\n\t\tExample: #DDD bg");
			
			int mode = 0;
			string input = Console.ReadLine();
			int affect = 0;
			
			if (input.Contains(" ")){
				string[] spl = input.Split(' ');
				input = spl[0];
				if (spl[1].ToLower() == "bg" || spl[1].ToLower() == "fg"){
					affect = spl[1].ToLower() == "bg" ? 1 : 2;
				}
				
			}
			
			if (int.TryParse(input, out mode)){
				if (mode == 1 || mode == 2){
					if (mode == 1)
						ConvertToSystem(affect);
					else
						ConvertToHex();
					
				}else{
					Console.WriteLine("Acceptable values: 1, 2");
				}
			}else{
				Console.WriteLine("Input isn't number");
			}
			
			Console.Write("Press any key to close . . . ");
			Console.ReadKey(true);
		}
		
		private static void ConvertToSystem(int affect){
			Console.Write("Write hex color:");
			string hex = Console.ReadLine();
			if (hex.StartsWith("#")){
				hex = hex.Substring(1);
			}
			Console.WriteLine(hex.Length);
			if (hex.Length == 3){
				string newHex = "";
				char[] arr = hex.ToCharArray();
				newHex = new string (arr[0],2);
				newHex += new string (arr[1],2);
				newHex += new string (arr[2],2);
				hex = newHex;
			}
			
			if (hex.Length == 6){
				string newHex = hex.Substring(4,2);
				newHex += hex.Substring(2,2);
				newHex += hex.Substring(0,2);
				Console.WriteLine(newHex);
				if (affect != 0)
					SetColorToCMD(newHex, affect);
				
			}else{
				Console.WriteLine("Invalid hex");
				ConvertToSystem(affect);
			}
		}
		
		private static void ConvertToHex(){
			
		}
		
		private static void SetColorToCMD(string color, int affect){
			Console.WriteLine("Setting Color...");
			try {
				RegistryKey cx = Registry.CurrentUser.OpenSubKey("Console\\%SystemRoot%_system32_cmd.exe",true);
				int value = Convert.ToInt32(color, 16);
				if (affect == 1){
					cx.SetValue("ColorTable00", value, RegistryValueKind.DWord);
				}else{
					cx.SetValue("ColorTable01", value, RegistryValueKind.DWord);
				}
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
				Console.WriteLine(@"Something went wrong. Try to apply yourself. To do that open regedit. Go to:\Computer\HKEY_CURRENT_USER\Console\%SystemRoot%_system32_cmd.exe\nAfter, set color to ColorTable01 if it's foreground. If background, set color to ColorTable00");
			}
		}
	}
}