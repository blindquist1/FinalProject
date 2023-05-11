using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Services
{
    internal class InputService
    {
        public static int GetIntWithPrompt(string prompt, string errorMessage)
        {
            bool conversionSuccessful = false;
            int userIntInput = 0;
            do
            {
                Console.Write(prompt);
                string? userInput = Console.ReadLine(); // the string? means we can allow a null or emptry string to be entered

                if (userInput == null || userInput == "")
                {
                    Console.WriteLine(errorMessage);
                    continue; //This sends it back to the start of the loop as we know it's not an integer without even running the TryParse
                }

                conversionSuccessful = int.TryParse(userInput, out userIntInput);

                if (!conversionSuccessful)
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!conversionSuccessful);

            return userIntInput;
        }

        public static char GetCharWithPrompt(string prompt, string errorMessage)
        {
            char userCharInput = ' ';
            bool isChar = false;

            if (!string.IsNullOrEmpty(prompt))
            {
                Console.Write(prompt);
            }

            do
            {
                string userInput = Console.ReadLine();
                userInput = userInput.ToUpper();

                if (!string.IsNullOrEmpty(userInput) && userInput.Length == 1)
                {
                    userCharInput = userInput[0];
                    isChar = true;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }

            } while (!isChar);

            return userCharInput;
        }
        public static string GetGenderWithPrompt(string prompt, string errorMessage)
        {
            string userStringInput = "";
            bool isGender = false;

            if (!string.IsNullOrEmpty(prompt))
            {
                Console.Write(prompt);
            }

            do
            {
                string userInput = Console.ReadLine();
                userInput = userInput.ToUpper();

                if (!string.IsNullOrEmpty(userInput) && userInput.Length == 1 && (userInput == "M" || userInput == "F" || userInput == "T" || userInput == "N"))
                {
                    userStringInput = userInput;
                    isGender = true;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }

            } while (!isGender);

            return userStringInput;
        }
        public static string GetZipWithPrompt(string? prompt, string? errorMessage)
        {
            bool isZip = false;
            string? userInput;

            if (!string.IsNullOrEmpty(prompt))
                Console.Write(prompt);

            do
            {
                userInput = Console.ReadLine();
                userInput = userInput?.ToUpper();

                if (!string.IsNullOrEmpty(userInput) && userInput.Length == 5)
                {
                    isZip = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                        Console.WriteLine(errorMessage);
                }
            } while (!isZip);
            return userInput;
        }
        public static string GetStringWithPrompt(string? prompt, string? errorMessage)
        {
            if (!string.IsNullOrEmpty(prompt))
                Console.Write(prompt);

            do
            {
                string? userInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(userInput))
                {
                    return userInput;
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                        Console.WriteLine(errorMessage);
                }
            } while (true);
        }
        public static DateTime GetDateWithPrompt(string prompt, string errorMessage)
        {
            bool conversionSuccessful = false;
            DateTime userDateInput = DateTime.Today;
            do
            {
                Console.Write(prompt);
                string? userInput = Console.ReadLine();

                if (userInput == null || userInput == "")
                {
                    Console.WriteLine(errorMessage);
                    continue;
                }

                conversionSuccessful = DateTime.TryParse(userInput, out userDateInput);

                if (!conversionSuccessful)
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!conversionSuccessful);

            return userDateInput;
        }
    }
}
