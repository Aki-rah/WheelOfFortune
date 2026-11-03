using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;

namespace WheelOfFortune_Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Wheel of Fortune");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            readContestants();
        
            string mainMenu = Console.ReadLine();

            Console.WriteLine("Please enter the number");

            switch (mainMenu)
            {
                case "1":
                    Console.WriteLine("1 Play");
                    break;
                case "2": Console.WriteLine("2 Contestants");
                    break;
                case "3": Console.WriteLine("3 Reset all scores to 0");
                    break;
                case "0": Console.WriteLine("0 Exit");
                    break;
  
            }


        }

        static Contestants[] contestants = new Contestants[23];
        static void readContestants()
        {
            string aline;
            int i = 0;
            Random random = new Random();

            StreamReader sr = new StreamReader("wheelOfFortune.txt");
            while (!sr.EndOfStream && i < contestants.Length)
            {


                contestants[i].firstName = sr.ReadLine();
                contestants[i].lastName = sr.ReadLine();
                contestants[i].interest = "Programming";
                contestants[i].score = random.Next(1, 5); // change

                i++;
            }
            sr.Close();
        }
        



       struct Contestants
        {
            public string firstName;
            public string lastName;
            public string interest;
            public int score;
        }

        //static void 

        //
       





    }
}
