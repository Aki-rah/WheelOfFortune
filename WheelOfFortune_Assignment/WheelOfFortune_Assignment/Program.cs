namespace WheelOfFortune_Assignment
{
    internal class Program
    {
        // Stores the information for each contestant
        struct Contestant
        {
            public string firstName;
            public string lastName;
            public string interest;
            public int score;
        }

        // Stores the information needed during a Wheel of Fortune game
        struct Game
        {
            public string word;
            public char[] hiddenWord;
            public string usedLetters;
            public int playerScore;
        }

        // Stores all contestant records
        static Contestant[] contestants = new Contestant[23];
        static int contestantCount = 0;

        // Stores the 10 randomly selected finalists
        static int[] finalists = new int[10];
        
        // Keeps track of whether finalists have been generated
        static bool finalistsCreated = false;

        // Stores the selected player's array position (-1 means no player selected)
        static int selectedPlayer = -1;

        // used to generate random numbers for finalists, players and wheel spins
        static Random random = new Random();

        // name of the text file containing contestant information
        static string fileName = "WheelOfFortune.txt";



        static void Main(string[] args)
        {
            // Load contestant information when the program starts
            LoadFile();

            int choice = 0;

            // repeat the menu until the user chooses to exit
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
                            Console.WriteLine("Invalid option. Please enter 1-7 only .");
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
            // check for a missing contestant file
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Contestant file not found.");
                return;
            }

            // Open the text file
            StreamReader sr = new StreamReader(@"wheelOfFortune.txt");

            contestantCount = 0;

            Console.WriteLine("\n===== WHEEL OF FORTUNE CONTESTANTS =====");

            // Read contestant information
            // Stop when the file ends or the array reaches its maximum size
            while (!sr.EndOfStream && contestantCount < 23)
            {
                contestants[contestantCount].firstName = sr.ReadLine().Trim();
                contestants[contestantCount].lastName = sr.ReadLine().Trim();
                contestants[contestantCount].interest = sr.ReadLine().Trim();

                string score = sr.ReadLine();

                contestants[contestantCount].score = Convert.ToInt32(score);

                // Display contestant details in console
                Console.WriteLine("\nContestant ".PadRight(20) + (contestantCount + 1));
                Console.WriteLine("First Name: ".PadRight(20) + contestants[contestantCount].firstName);
                Console.WriteLine("Last Name: ".PadRight(25) + contestants[contestantCount].lastName);
                Console.WriteLine("Interest: ".PadRight(25) + contestants[contestantCount].interest);
                Console.WriteLine("Score: ".PadRight(20) + contestants[contestantCount].score);

                contestantCount++;
            }

            // Close the file after reading all contestant records
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
            // Creates a temporary array to sort contestants without changing the original array
            Contestant[] sorted = new Contestant[contestantCount];

            // Copy all contestants into the temporary array
            Array.Copy(contestants, sorted, contestantCount);

            // Simple bubble sort that arrange contestants alphabetically by last name
            for (int i = 0; i < contestantCount - 1; i++)
            {
                for (int j = 0; j < contestantCount - 1 - i; j++)
                {
                    if (string.Compare(
                        sorted[j].lastName,
                        sorted[j + 1].lastName,
                        StringComparison.OrdinalIgnoreCase) > 0)
                    {

                        // swap contestants when their last names are in the wrong order
                        Contestant temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine(" --- WHEEL OF FORTUNE CONTESTANT LIST ---");
            Console.WriteLine();
            Console.WriteLine();

            // Display table headings
            Console.WriteLine(
                "NO.".PadRight(8) +
                "FIRST NAME".PadRight(20) +
                "LAST NAME".PadRight(25) +
                "INTEREST".PadRight(25) +
                "SCORE".PadRight(10));

            Console.WriteLine();

            // Display contestant information
            for (int i = 0; i < contestantCount; i++)
            {
                Console.WriteLine(
                    (i + 1 + ".").PadRight(8) +
                    sorted[i].firstName.PadRight(20) +
                    sorted[i].lastName.PadRight(25) +
                    sorted[i].interest.PadRight(25) +
                    ("$" + sorted[i].score).PadRight(10));
            }

            Console.WriteLine();
            Console.WriteLine("  Total contestants: " + contestantCount);
            Console.WriteLine();
        }

        // Finds a contestant and changes their interest
        static void UpdateInterest()
        {
            // ask user for contestant's first name
            Console.Write("Enter the first name: ");
            string first = Console.ReadLine();

            // ask user for contestant's last name
            Console.Write("Enter the last name: ");
            string last = Console.ReadLine();

            // Check that both names have been entered
            if (string.IsNullOrWhiteSpace(first) ||
                string.IsNullOrWhiteSpace(last))
            {
                Console.WriteLine("Names cannot be empty.");
                return;
            }

            bool found = false;

            for (int i = 0; i < contestantCount; i++)
            {
                if (contestants[i].firstName.Equals(
                    first.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    contestants[i].lastName.Equals(
                    last.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Current interest: " +
                        contestants[i].interest);

                    // Ask the user for the contestant's new interest
                    Console.Write("Enter new interest: ");
                    string newInterest = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newInterest))
                    {
                        Console.WriteLine("Interest cannot be empty.");
                        return;
                    }

                    // Update the contestant's interest and save the changes to the file
                    contestants[i].interest = newInterest.Trim();

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
            // Check that at least 10 contestants are available
            if (contestantCount < 10)
            {
                Console.WriteLine("There are not enough contestants.");
                return;
            }

            bool duplicate;

            // Generate 10 random contestant positions
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


            // Display the 10 finalist
            Console.WriteLine("\n================================");
            Console.WriteLine("          FINALISTS");
            Console.WriteLine("==================================");

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
            // Check whether the 10 finalists have already been generated
            if (!finalistsCreated)
            {
                Console.WriteLine("Please generate finalists first.");
                return;
            }

            // Randomly select one of the 10 finalists
            int chosen = random.Next(0, 10);

            // Store the selected contestant's position in the original array
            selectedPlayer = finalists[chosen];

            Console.WriteLine("\nThe lucky player is:");
            Console.WriteLine(contestants[selectedPlayer].firstName +
                " " + contestants[selectedPlayer].lastName);
        }

        // Runs the Wheel of Fortune game

        static void PlayGame()
        {
            //user game instructions
            Console.WriteLine("\nGAME RULES");
            Console.WriteLine("1. Guess one letter at a time.");
            Console.WriteLine("2. Each vowel costs $250.");
            Console.WriteLine("3. Correct consonants earn money.");
            Console.WriteLine("4. Avoid the losing spaces!");
            Console.WriteLine("5. Reveal the complete word to win.");

            // Check that a player has been selected before starting the game
            if (selectedPlayer == -1)
            {
                Console.WriteLine("Please select a player first.");
                return;
            }

            // Words must contain at least 10 characters
            string[] puzzles =
            {
            "PROGRAMMING",
            "NETWORKING",
            "MOUNTAINBIKING",
            "COMPUTERSCIENCE",
            "NEWZEALAND",
            "BASKETBALLCOURT"
            };

            // Create a new game
            Game currentGame = new Game();

            currentGame.word = puzzles[random.Next(puzzles.Length)];

            currentGame.hiddenWord = new char[currentGame.word.Length];

            for (int i = 0; i < currentGame.word.Length; i++)
            {
                currentGame.hiddenWord[i] = '_';
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

            currentGame.usedLetters = "";
            currentGame.playerScore = 0;

            Console.WriteLine("\nWelcome to Wheel of Fortune!");
            Console.WriteLine("Guess the hidden word.");

            while (new string(currentGame.hiddenWord) != currentGame.word)
            {
                Console.WriteLine("\nWord: " +
                    new string(currentGame.hiddenWord));

                Console.WriteLine("Current score: $" +
                    currentGame.playerScore);

                Console.WriteLine("\nAVAILABLE LETTERS:");

                // Display all letters that the player has not guessed yet
                for (char letter = 'A'; letter <= 'Z'; letter++)
                {
                    if (!currentGame.usedLetters.Contains(letter))
                    {
                        Console.Write(letter + " ");
                    }
                }

                Console.WriteLine("\n\nUSED LETTERS: " +
                    currentGame.usedLetters);

                Console.WriteLine("\nPress ENTER to spin the wheel.");
                Console.ReadLine();

                // Randomly spin the wheel
                int spin = wheel[random.Next(wheel.Length)];

                if (spin == -2000 || spin == -3000)
                {
                    Console.WriteLine("You landed on a losing space!");

                    currentGame.playerScore += spin;

                    if (currentGame.playerScore < 0)
                    {
                        currentGame.playerScore = 0;
                    }

                    Console.WriteLine("Your score is now $" +
                        currentGame.playerScore);

                    continue;
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

                        if (currentGame.usedLetters.Contains(guess))
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

                // Add the guessed letter to used letters
                currentGame.usedLetters += guess;

                // Check how many times the letter appears
                int occurrences = 0;

                for (int i = 0; i < currentGame.word.Length; i++)
                {
                    if (currentGame.word[i] == guess)
                    {
                        occurrences++;
                        currentGame.hiddenWord[i] = guess;
                    }
                }

                // Deduct $250 when the player selects a vowel
                if ("AEIOU".Contains(guess))
                {
                    currentGame.playerScore -= 250;

                    if (currentGame.playerScore < 0)
                    {
                        currentGame.playerScore = 0;
                    }

                    Console.WriteLine("Vowel selected. $250 deducted.");
                }
                else if (occurrences > 0)
                {
                    int winnings = spin * occurrences;

                    currentGame.playerScore += winnings;

                    Console.WriteLine("Correct! " + occurrences +
                        " letter(s) found.");

                    Console.WriteLine("You earned $" + winnings);
                }
                else
                {
                    Console.WriteLine("Sorry, that letter is not in the word.");
                }
            }

            // User guess the word
            Console.WriteLine("\nCONGRATULATIONS!");

            Console.WriteLine("The word was: " + currentGame.word);

            // Display user final score
            Console.WriteLine("Your final score is: $" +
                currentGame.playerScore);

            // Save the player's score
            contestants[selectedPlayer].score = currentGame.playerScore;

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

            // display 5 players
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

