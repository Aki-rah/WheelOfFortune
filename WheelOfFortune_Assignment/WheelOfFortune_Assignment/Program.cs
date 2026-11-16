using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;

namespace WheelOfFortune_Assignment
{
    internal class Program
    {

        struct Contestant
        {
            public string firstName;
            public string lastName;
            public string interest;
            public int score;
        }

            // Stores all contestants
            static Contestant[] contestants = new Contestant[23];
            static int contestantCount = 0;

            // Stores the 10 finalist positions
            static int[] finalists = new int[10];
            static bool finalistsCreated = false;
            static int selectedPlayer = -1;

            static Random random = new Random();
            
            static string fileName = "WheelOfFortune.txt";
          


            static void Main(string[] args)
            {
                LoadFile();

                int choice = 0;

                do
                {
                    Console.Clear();
                    Console.WriteLine("==============================");
                    Console.WriteLine("       WHEEL OF FORTUNE");
                    Console.WriteLine("==============================");
                    Console.WriteLine("1. List all contestants");
                    Console.WriteLine("2. Update contestant interest");
                    Console.WriteLine("3. Generate 10 finalists");
                    Console.WriteLine("4. Select random player");
                    Console.WriteLine("5. Play Wheel of Fortune");
                    Console.WriteLine("6. Display top five scores");
                    Console.WriteLine("7. Exit");
                    Console.WriteLine("==============================");
                    Console.Write("Enter your choice: ");

                    // Check that the user enters a number
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                ListContestants();
                                break;

                            case 2:
                                UpdateInterest();
                                break;

                            case 3:
                                GenerateFinalists();
                                break;

                            case 4:
                                SelectPlayer();
                                break;

                            case 5:
                                PlayGame();
                                break;

                            case 6:
                                DisplayTopFive();
                                break;

                            case 7:
                                SaveFile();
                                Console.WriteLine("Goodbye!");
                                break;

                            default:
                                Console.WriteLine("Invalid option. Please choose 1-7.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid number.");
                    }

                    if (choice != 7)
                    {
                        Console.WriteLine("\nPress ENTER to continue...");
                        Console.ReadLine();
                    }

                } while (choice != 7);
            }

            // Reads the contestant information from the text file
            static void LoadFile()
            {
            // Open the text file
            StreamReader sr = new StreamReader(@"wheelOfFortune.txt");

            contestantCount = 0;

            Console.WriteLine("\n===== WHEEL OF FORTUNE CONTESTANTS =====");

            // Read contestant information
            while (!sr.EndOfStream && contestantCount < 23)
            {
                contestants[contestantCount].firstName = sr.ReadLine();
                contestants[contestantCount].lastName = sr.ReadLine();
                contestants[contestantCount].interest = sr.ReadLine();

                string score = sr.ReadLine();

                contestants[contestantCount].score = Convert.ToInt32(score);

                // Display contestant details in console
                Console.WriteLine("\nContestant " + (contestantCount + 1));
                Console.WriteLine("First Name: " + contestants[contestantCount].firstName);
                Console.WriteLine("Last Name: " + contestants[contestantCount].lastName);
                Console.WriteLine("Interest: " + contestants[contestantCount].interest);
                Console.WriteLine("Score: " + contestants[contestantCount].score);

                contestantCount++;
            }

            sr.Close();

            Console.WriteLine("\nTotal contestants loaded: " + contestantCount);
        }


        // Saves all contestant information back to the text file
        static void SaveFile()
        {
            // Open the file for writing
            StreamWriter sw = new StreamWriter(@"wheelOfFortune.txt");

            // Write each contestant's information to the file
            for (int i = 0; i < contestantCount; i++)
            {
                sw.WriteLine(contestants[i].firstName);
                sw.WriteLine(contestants[i].lastName);
                sw.WriteLine(contestants[i].interest);
                sw.WriteLine(contestants[i].score);
            }

            // Close the file
            sw.Close();

            Console.WriteLine("File saved successfully!");
        }

        // Displays contestants in alphabetical order by last name
        static void ListContestants()
            {
                // Make a copy so the original array is not changed
                Contestant[] sorted = new Contestant[contestantCount];

                Array.Copy(contestants, sorted, contestantCount);

                // Simple bubble sort
                for (int i = 0; i < contestantCount - 1; i++)
                {
                    for (int j = 0; j < contestantCount - 1 - i; j++)
                    {
                        if (string.Compare(
                            sorted[j].lastName,
                            sorted[j + 1].lastName,
                            StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            Contestant temp = sorted[j];
                            sorted[j] = sorted[j + 1];
                            sorted[j + 1] = temp;
                        }
                    }
                }

                Console.WriteLine("\n--- ALL CONTESTANTS ---");

                for (int i = 0; i < contestantCount; i++)
                {
                    Console.WriteLine(
                        sorted[i].firstName + " " +
                        sorted[i].lastName + " | " +
                        sorted[i].interest + " | Score: " +
                        sorted[i].score);
                }
            }

            // Finds a contestant and changes their interest
            static void UpdateInterest()
            {
                Console.Write("Enter the first name: ");
                string first = Console.ReadLine();

                Console.Write("Enter the last name: ");
                string last = Console.ReadLine();

                bool found = false;

                for (int i = 0; i < contestantCount; i++)
                {
                    if (contestants[i].firstName.Equals(
                        first, StringComparison.OrdinalIgnoreCase) &&
                        contestants[i].lastName.Equals(
                        last, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Current interest: " +
                            contestants[i].interest);

                        Console.Write("Enter new interest: ");
                        contestants[i].interest = Console.ReadLine();

                        SaveFile();

                        Console.WriteLine("Interest updated successfully.");
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Contestant was not found.");
                }
            }

            // Creates 10 different finalists without duplicates
            static void GenerateFinalists()
            {
                if (contestantCount < 10)
                {
                    Console.WriteLine("There are not enough contestants.");
                    return;
                }

                bool duplicate;

                for (int i = 0; i < 10; i++)
                {
                    do
                    {
                        finalists[i] = random.Next(0, contestantCount);
                        duplicate = false;

                        // Check that this number has not been selected already
                        for (int j = 0; j < i; j++)
                        {
                            if (finalists[i] == finalists[j])
                            {
                                duplicate = true;
                            }
                        }

                    } while (duplicate);
                }

                finalistsCreated = true;
                selectedPlayer = -1;

                Console.WriteLine("\n--- THE 10 FINALISTS ---");

                for (int i = 0; i < 10; i++)
                {
                    int number = finalists[i];

                    Console.WriteLine((i + 1) + ". " +
                        contestants[number].firstName + " " +
                        contestants[number].lastName);
                }
            }

            // Randomly chooses one person from the finalists
            static void SelectPlayer()
            {
                if (!finalistsCreated)
                {
                    Console.WriteLine("Please generate finalists first.");
                    return;
                }

                int chosen = random.Next(0, 10);

                selectedPlayer = finalists[chosen];

                Console.WriteLine("\nThe lucky player is:");
                Console.WriteLine(contestants[selectedPlayer].firstName +
                    " " + contestants[selectedPlayer].lastName);
            }

            // Runs the Wheel of Fortune game
            static void PlayGame()
            {
                if (selectedPlayer == -1)
                {
                    Console.WriteLine("Please select a player first.");
                    return;
                }

                // Words must contain at least 10 characters
                string[] puzzles =
                {
                 "PROGRAMMING",
                 "CHOCOLATECAKE",
                 "MOUNTAINBIKING",
                 "COMPUTERSCIENCE",
                 "NEWZEALAND",
                 "BASKETBALLCOURT"
                };

                string word = puzzles[random.Next(puzzles.Length)];

                char[] hiddenWord = new char[word.Length];

                for (int i = 0; i < word.Length; i++)
                {
                    hiddenWord[i] = '_';
                }

                // The wheel has 22 cash spaces and 2 losing spaces
                int[] wheel =
                {
                  300, 400, 500, 600, 700, 800,
                  900, 1000, 1200, 1400, 1600,
                  1800, 2000, 2200, 2400, 2600,
                  2800, 3000, 3500, 4000, 4500,
                   5000, -2000, -3000
                };

                string usedLetters = "";
                int playerScore = 0;

                Console.WriteLine("\nWelcome to Wheel of Fortune!");
                Console.WriteLine("Guess the hidden word.");

                while (new string(hiddenWord) != word)
                {
                    Console.WriteLine("\nWord: " + new string(hiddenWord));
                    Console.WriteLine("Available letters: ");

                    for (char letter = 'A'; letter <= 'Z'; letter++)
                    {
                        if (!usedLetters.Contains(letter))
                        {
                            Console.Write(letter + " ");
                        }
                    }

                    Console.WriteLine("\nUsed letters: " + usedLetters);
                    Console.WriteLine("Current score: $" + playerScore);

                    Console.WriteLine("\nPress ENTER to spin the wheel.");
                    Console.ReadLine();

                    // Randomly spin the wheel
                    int spin = wheel[random.Next(wheel.Length)];

                    if (spin == -2000 || spin == -3000)
                    {
                        Console.WriteLine("Oh no! You landed on Lose $" +
                            Math.Abs(spin));

                        playerScore += spin;

                        if (playerScore < 0)
                        {
                            playerScore = 0;
                        }
                    }
                    else
                    {
                        Console.WriteLine("You spun: $" + spin);
                    }

                    char guess;

                    while (true)
                    {
                        Console.Write("Guess a letter: ");
                        string answer = Console.ReadLine().ToUpper();

                        if (answer.Length == 1 &&
                            answer[0] >= 'A' && answer[0] <= 'Z')
                        {
                            guess = answer[0];

                            if (usedLetters.Contains(guess))
                            {
                                Console.WriteLine("You have already used that letter.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter one letter only.");
                        }
                    }

                    usedLetters += guess;

                    // Check how many times the letter appears
                    int occurrences = 0;

                    for (int i = 0; i < word.Length; i++)
                    {
                        if (word[i] == guess)
                        {
                            occurrences++;
                            hiddenWord[i] = guess;
                        }
                    }

                    // Vowels cost $250 each time they are selected
                    if ("AEIOU".Contains(guess))
                    {
                        playerScore -= 250;

                        if (playerScore < 0)
                        {
                            playerScore = 0;
                        }

                        Console.WriteLine("Vowel selected. $250 deducted.");
                    }
                    else if (occurrences > 0)
                    {
                        int winnings = spin * occurrences;
                        playerScore += winnings;

                        Console.WriteLine("Correct! " + occurrences +
                            " letter(s) found.");
                        Console.WriteLine("You earned $" + winnings);
                    }
                    else
                    {
                        Console.WriteLine("Sorry, that letter is not in the word.");
                    }
                }

                Console.WriteLine("\nCONGRATULATIONS!");
                Console.WriteLine("The word was: " + word);
                Console.WriteLine("Your final score is: $" + playerScore);

                // Save the player's score
                contestants[selectedPlayer].score = playerScore;
                SaveFile();

                Console.WriteLine("Your score has been saved.");

                // Reset selection so another game requires a new player
                selectedPlayer = -1;
            }

            // Displays the five highest scoring contestants
            static void DisplayTopFive()
            {
                Contestant[] sorted = new Contestant[contestantCount];

                Array.Copy(contestants, sorted, contestantCount);

                // Sort from highest score to lowest score
                for (int i = 0; i < contestantCount - 1; i++)
                {
                    for (int j = 0; j < contestantCount - 1 - i; j++)
                    {
                        if (sorted[j].score < sorted[j + 1].score)
                        {
                            Contestant temp = sorted[j];
                            sorted[j] = sorted[j + 1];
                            sorted[j + 1] = temp;
                        }
                    }
                }

                Console.WriteLine("\n--- TOP FIVE PLAYERS ---");

                int numberToShow = Math.Min(5, contestantCount);

                for (int i = 0; i < numberToShow; i++)
                {
                    Console.WriteLine((i + 1) + ". " +
                        sorted[i].firstName + " " +
                        sorted[i].lastName +
                        " - $" + sorted[i].score);
                }
            }

        
    }
}

