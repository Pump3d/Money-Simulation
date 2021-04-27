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
		js.name = "Test Dummy";
		js.balance = 0;
		
		while (true) {
			int num;

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
				num = random.Next(100);

				js.balance -= num;

				Console.WriteLine(js.balance.ToString());

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"-{num.ToString()}");
			} else {
				num = random.Next(100);

				js.balance += num;

				Console.WriteLine(js.balance.ToString());

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"+{num.ToString()}");
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