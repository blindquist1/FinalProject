using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Services;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MyNamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DatabaseService queries = new DatabaseService();
            int menuSelection = 0;
            int maxMenuItem = 9;
            while (menuSelection != maxMenuItem)
            {
                Console.WriteLine("===================");
                Console.WriteLine("   Menu Options");
                Console.WriteLine("===================");
                Console.WriteLine("1. List Movies");
                Console.WriteLine("2. Update Movie");
                Console.WriteLine("3. Add Movie");
                Console.WriteLine("4. Search Movies");
                Console.WriteLine("5. Delete Movie");
                Console.WriteLine("6. Add User");
                Console.WriteLine("7. Add Movie Rating");
                Console.WriteLine("8. Top movies by occupation");
                Console.WriteLine("9. Exit");
                Console.WriteLine();

                bool validEntry = false;

                //Keep looping through until user chooses a valid entry, an integer and between 1 and maxMenuItem value.
                while (!validEntry)
                {
                    menuSelection = InputService.GetIntWithPrompt("Select an option: ", "Entry must be an integer");
                    if (menuSelection < 1 || menuSelection > maxMenuItem)
                    {
                        Console.WriteLine($"Entry must be between 1 and {maxMenuItem}");
                    }
                    else
                    {
                        validEntry = true;
                    }
                }

                Console.WriteLine();

                // READ Movies
                if (menuSelection == 1)
                {
                    int movieCount = 0;
                    while (movieCount < 1)
                    {
                        movieCount = InputService.GetIntWithPrompt("How many movies do you want to display? Type 0 to select all: ", "Entry must be an integer");
                        if (movieCount == 0)
                        {
                            movieCount = 9999999;
                        }
                    }

                    var movieList = queries.GetMovies(movieCount);

                    ListMovies(movieList);
                }

                // UPDATE Movies
                if (menuSelection == 2)
                {
                    Movie mov = null;
                    do
                    {
                        mov = SelectMovie("update", queries);
                    } while (mov == null);
                    var movieTitle = InputService.GetStringWithPrompt("Enter the updated movie title: ", "Entry must be a string");
                    queries.MovieUpdate(mov, movieTitle);
                    var movieUpdatedList = queries.GetMovieListById(mov.Id);
                    ListMovies(movieUpdatedList);
                }

                // Add Movie
                if (menuSelection == 3)
                {
                    var movieTitle = InputService.GetStringWithPrompt("Enter a movie title: ", "Entry must be a string");

                    var movieList = queries.GetMoviesByName(movieTitle,"Add");

                    if (movieList.Count > 0)
                    {
                        Console.WriteLine("That movie already exists");
                    }
                    else
                    {
                        DateTime releaseDate = InputService.GetDateWithPrompt("Enter a release date: ", "Entry must be a date");
                        var mov = new Movie();
                        mov.Title = movieTitle;
                        mov.ReleaseDate = releaseDate;

                        queries.MovieAdd(mov);

                        //Prompt the user to select from the available genres
                        int genreChoice = 9999;
                        while(genreChoice != 0)
                        {
                            var genreList = queries.GetGenres();
                            ListGenres(genreList);
                       
                            genreChoice = InputService.GetIntWithPrompt("Pick a genre to add to the movie, enter 0 when done: ", "Entry must be an integer");
                        
                            if (genreChoice != 0)
                            {
                                var gen = new Genre();
                                gen = queries.GetGenresById(genreChoice);
                                if (gen != null)
                                {
                                    var movGen = new MovieGenre();
                                    movGen.Movie = mov;
                                    movGen.Genre = gen;

                                    queries.MovieGenreAdd(movGen);
                                }
                                else if (genreChoice != 0)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("That is an invalid selection, please try again");
                                }
                            }
                        }
                        var movieAddedList = queries.GetMovieListById(mov.Id);
                        ListMovies(movieAddedList);
                    }
                }
                // SEARCH Movies
                if (menuSelection == 4)
                {
                    var movieSelection = InputService.GetStringWithPrompt("Enter a movie to search for: ", "Entry must be a string");

                    var movieList = queries.GetMoviesByName(movieSelection,"Search");

                    ListMovies(movieList);
                }

                // DELETE Movies
                if (menuSelection == 5)
                {
                    Movie mov = null;
                    do
                    {
                        mov = SelectMovie("delete", queries);
                    } while (mov == null);
                    var title = mov.Title;
                    queries.MovieDelete(mov);
                    Console.WriteLine();
                    Console.WriteLine($"Movie {title} is now deleted.");
                }

                // Add User
                if (menuSelection == 6)
                {
                    var userAge = InputService.GetIntWithPrompt("Enter the user's age: ", "Entry must be an integer");
                    var userGender = InputService.GetGenderWithPrompt("Enter the user's gender (M,F,T,N): ", "Entry must be M, F, T or N");
                    var userZip = InputService.GetZipWithPrompt("Enter the user's zip code: ", "Entry must be an integer");

                    var user = new User();
                    user.Age = userAge;
                    user.Gender = userGender;
                    user.ZipCode = userZip;

                    //Prompt the user to select from the available occupations
                    var occList = queries.GetOccupations();
                    ListOccupations(occList);
                    
                    bool validChoice = false;
                    var occ = new Occupation();

                    while (!validChoice)
                    {
                        var occChoice = InputService.GetIntWithPrompt("Pick an occupation for the user: ", "Entry must be an integer");
                        occ = queries.GetOccupationsById(occChoice);
                        if (occ != null)
                        {
                            user.Occupation = occ;
                            validChoice = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("That is an invalid selection, please try again");
                            ListOccupations(occList);
                        }
                    }
                    queries.UserAdd(user);

                    Console.WriteLine($"User: {user.Id} {user.Age} {user.Gender} {user.ZipCode} {user.Occupation.Name}");
                }
                // Add Movie Rating
                if (menuSelection == 7)
                {
                    Movie mov = null;
                    do
                    {
                        mov = SelectMovie("add a rating for", queries);
                    } while (mov == null);

                    User? user = null;
                    char correctUser = 'N';
                    do
                    {
                        long userId = InputService.GetIntWithPrompt("Enter the user id: ", "Entry must be an integer");
                        user = queries.GetUserById(userId);
                        if (user == null)
                        {
                            Console.WriteLine("Invalid user id, please try again.");
                        }
                        else
                        {
                            Console.WriteLine($"User: {user.Id} {user.Age} {user.Gender} {user.ZipCode} {user.Occupation.Name}");
                            do
                            {
                                correctUser = InputService.GetCharWithPrompt($"Is this the correct user? (Y/N) ", "Entry must be Y or N");
                            } while (correctUser != 'N' && correctUser != 'Y');

                        }
                    } while (user == null || correctUser == 'N');
                    long rating = 0;
                    while (rating < 1 || rating > 5)
                    {
                        rating = InputService.GetIntWithPrompt("Enter a movie rating (1-5): ", "Entry must be an integer");
                        if (rating < 1 || rating > 5)
                        {
                            Console.WriteLine("Rating must be between 1 and 5");
                        }
                    }
                    DateTime ratingDate = InputService.GetDateWithPrompt("Enter the rating date: ", "Entry must be a date");

                    var userMov = new UserMovie();
                    userMov.Movie = mov;
                    userMov.User = user;
                    userMov.Rating = rating;
                    userMov.RatedAt = ratingDate;

                    queries.MovieRatingAdd(userMov);

                    Console.WriteLine();
                    Console.WriteLine($"Rating for movie {mov.Title} is now added.");
                    Console.WriteLine($"User: {user.Id} {user.Age} {user.Gender} {user.ZipCode} {user.Occupation.Name}");
                    Console.WriteLine($"Rating: {userMov.Rating}");
                    Console.WriteLine($"Rating Date: {userMov.RatedAt:d}");
                }

                if (menuSelection == 8)
                {
                    int ratingMinimum = InputService.GetIntWithPrompt("Enter a minimum number of ratings for a movie to be included: ", "Entry must be an integer");
                    var table = new ConsoleTable("Occupation", "Best Movie", "Rating Count", "Rating Sum", "Rating Avg");
                    var occ = queries.GetOccupations();

                    foreach (var o in occ)
                    {
                        var userMovies = queries.GetUserMovies();

                        var um = userMovies
                            .Where(x => x.User.Occupation.Id == o.Id)
                            .GroupBy(x => x.Movie.Title)
                            .Select(x => new { MovieTitle = x.Key, CountOfRatings = x.Count(), RatingSum = x.Sum(x => x.Rating), RatingAvg = x.Average(x => x.Rating) })
                            .OrderBy(x => x.MovieTitle)
                            .ToList();

                        double bestRatingAvg = 0;
                        var bestMovieTitle = "";
                        var bestRatingCount = 0;
                        long bestRatingSum = 0;
                        foreach (var m in um)
                        {
                            if (m.RatingAvg > bestRatingAvg && m.CountOfRatings >= ratingMinimum)
                            {
                                bestMovieTitle = m.MovieTitle;
                                bestRatingAvg = m.RatingAvg;
                                bestRatingCount = m.CountOfRatings;
                                bestRatingSum = m.RatingSum;
                            }
                        }
                        table.AddRow(o.Name, bestMovieTitle, bestRatingCount, bestRatingSum, string.Format("{0:0.0}", bestRatingAvg));
                    }
                    table.Write();
                }
                Console.WriteLine();
            }

        }
        
        //Display list of movies and their corresponding genres
        static void ListMovies(List<Movie> movieList)
        {
            Console.WriteLine();

            if (movieList.Any())
            {
                if(movieList.Count == 1)
                {
                    Console.WriteLine("The movie is:");
                }
                else
                {
                    Console.WriteLine("The movies are:");
                }
                Console.WriteLine();

                foreach (var movie in movieList)
                {
                    Console.WriteLine($"{movie.Id} {movie.Title} {movie.ReleaseDate:d}");

                    foreach (var movieGenre in movie.MovieGenres)
                    {
                        Console.WriteLine($"  {movieGenre.Genre.Name}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No movies found");
            }

        }
        
        //Display list of genres
        static void ListGenres(List<Genre> genreList)
        {
            Console.WriteLine();

            if (genreList.Any())
            {
                Console.WriteLine("The genres are:");

                foreach (var genre in genreList)
                {
                    Console.WriteLine($"{genre.Id} {genre.Name}");
                }
            }
            else
            {
                Console.WriteLine("No genres found");
            }

        }
        
        //Prompt user for a movie to either update or delete
        static Movie SelectMovie(string selectType, DatabaseService queries)
        {
            var movieSelection = InputService.GetStringWithPrompt($"Enter the title of a movie to {selectType}: ", "Entry must be a string");
            var movieList = queries.GetMoviesByName(movieSelection, "Search");
            var mov = new Movie();
            char correctMovie = 'N';

            while (movieList.Count >= 1 && correctMovie == 'N')
            { 
                if (movieList.Count == 1)
                {
                    mov = queries.GetMoviesById(movieList[0].Id);
                }

                //If more than one movie found, prompt user to select from the reduced list
                else if (movieList.Count > 1)
                {
                    ListMovies(movieList);
                    Console.WriteLine();

                    long movieChoice = InputService.GetIntWithPrompt($"Several movies found, please enter the id number of the one you want to {selectType}: ", "Entry must be an integer");
                    mov = queries.GetMoviesById(movieChoice);
                    if (mov == null)
                    {
                        Console.WriteLine("That id is invalid");
                    }
                }

                //Display the movie title select and ask if it's the correct movie
                if (mov != null)
                {
                    Console.WriteLine($"Movie selected: {mov.Title}");
                    do
                    {
                        correctMovie = InputService.GetCharWithPrompt($"Is this the correct movie? (Y/N) ", "Entry must be Y or N");
                    } while (correctMovie != 'N' && correctMovie != 'Y');
                    if (correctMovie == 'N')
                    {
                        movieSelection = InputService.GetStringWithPrompt($"Enter the title of a movie to {selectType}: ", "Entry must be a string");
                    }
                    movieList = queries.GetMoviesByName(movieSelection, "Search");
                }
            }

            //If no movie found, tell the end user
            if (movieList.Count == 0)
            {
                Console.WriteLine("That movie was not found.");

                //Set to null so the calling program knows a movie was not selected
                mov = queries.GetMoviesById(0);
            }
            return mov;
        }

        //Display list of occupations
        static void ListOccupations(List<Occupation> occList)
        {
            Console.WriteLine();

            if (occList.Any())
            {
                Console.WriteLine("The occupations are:");

                foreach (var occ in occList)
                {
                    Console.WriteLine($"{occ.Id} {occ.Name}");
                }
            }
            else
            {
                Console.WriteLine("No occupations found");
            }
        }
    }
}