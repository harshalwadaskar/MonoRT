// cs0221-2.cs: Constant value `-9' cannot be converted to a `E' (use `unchecked' syntax to override)
// Line: 10

enum E:byte {
	Min = 9
}

class T {
	static void Main () {
			E error = (E)(-9);
			System.Console.WriteLine (error);
	}
}
