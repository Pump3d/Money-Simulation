using System;
using System.Threading;
using System.Threading.Tasks;

class Person {
	public string name;
	public int balance;
	public bool debt = false;
}

class MainClass {
	public static Person js = new Person();
	public static Random random = new Random();
	public static CancellationTokenSource ts = new CancellationTokenSource();
	public static Task debt;
	public static int Salary = 0;

	public static void Ask() {
		Console.WriteLine("\n Salary? (40 hours) (Type 'none' for random generation) \n");
		
		string read = Console.ReadLine();
		
		if (read.ToLower() == "none") {
			Console.WriteLine("\n Defaulting to random... \n");
			return;
		}

		if (!int.TryParse(read, out Salary)) {
			Console.WriteLine("\n Invalid salary, defaulting to random... \n");
			return;
		}
		
		if (Salary < 10000) {
			Console.WriteLine("\n Too low salary, defaulting to random... \n");
			return;
		}
		

		Console.WriteLine($"\n Salary successfully set to ${Salary.ToString()} \n");
		Salary /= 2080;
		return;
	}

	public static void DebtCheck(Person person) {
		for (int i = 0; i < 7; i++) {
			if (ts.Token.IsCancellationRequested) {
				return;
			}

			System.Threading.Thread.Sleep(1000);
		}
		
		Console.WriteLine("\n Bank interference \n");
		System.Environment.Exit(0);
	}

	public static void Main(string[] args) {
		Ask();

		js.name = "Test Dummy";
		js.balance = 0;
		
		while (true) {
			int num;
			int balDif;

			if (js.debt) {
				num = random.Next(125);
			} else {
				if (js.balance >= 100) {
					num = random.Next((1000 - js.balance) / 9);
				} else {
					num = random.Next(100);
				}
			}
	
			bool negative = (num <= 50);

			if (negative == true) {
				balDif = random.Next(100);
				js.balance -= balDif;

				Console.WriteLine(js.balance.ToString());

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"-{balDif.ToString()}");
			} else {
				if (Salary != 0) {
					balDif = Salary;
				} else {
					balDif = random.Next(100);
				}

				js.balance += balDif;

				Console.WriteLine(js.balance.ToString());

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"+{balDif.ToString()}");
			}
			Console.ForegroundColor = ConsoleColor.White;

			if (js.balance < 0 & !js.debt) {
				Console.WriteLine("\n In debt \n");
				js.debt = true;
					

				ts = new CancellationTokenSource();
				debt = null;

				debt = Task.Factory.StartNew(() =>
					DebtCheck(js),
					ts.Token, 
        	TaskCreationOptions.LongRunning, 
        	TaskScheduler.Default
				);
			}

			if (js.balance >= 0 & js.debt) {
				Console.WriteLine("\n Out of debt \n");
				js.debt = false;

				if (debt != null & debt.IsCompleted == false) {
					ts.Cancel();
				}
			}

			System.Threading.Thread.Sleep(2000);
		}
	}
}