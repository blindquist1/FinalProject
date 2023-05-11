using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Services
{
    public class DatabaseService
    {
        MovieContext context;

        public DatabaseService()
        {
            context = new MovieContext();
        }
        //List of movies based on the number requested
        public List<Movie> GetMovies(int numberOfMovies)
        {
            var movies = context.Movies;
            var limitedMovies = movies.Take(numberOfMovies).ToList();
            return limitedMovies;
        }

        //List of movies with a specific name
        public List<Movie> GetMoviesByName(string movieName, string? searchType)
        {
            var movies = context.Movies;
            List<Movie> mov = movies.ToList();
            if (searchType == "Search")
            {
                //Case insensitive when it's for a "Search" request
                mov = movies.ToList().Where(m => m.Title.Contains(movieName, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            else if (searchType == "Add")
            {
                //Must be an exact match when it's for an "Add" request
                mov = movies.ToList().Where(m => m.Title == movieName).ToList();
            }
            return mov;
        }
        
        //Returns a movie object for a specific movie id
        public Movie GetMoviesById(long movieId)
        {
            var mov = context.Movies.FirstOrDefault(x => x.Id == movieId);
            return mov;
        }
        
        //Returns a movie list for a specific movie id
        public List<Movie> GetMovieListById(long movieId)
        {
            var movies = context.Movies;
            List<Movie> mov = movies.ToList();
            mov = movies.ToList().Where(x => x.Id == movieId).ToList();
            return mov;
        }

        //Adds the movie sent
        public void MovieAdd(Movie movie)
        {
            context.Movies.Add(movie);
            context.SaveChanges();
        }
        
        //Updates a movie with a new title
        public void MovieUpdate(Movie movie,string newTitle)
        {
            movie.Title = newTitle;
            context.SaveChanges();
        }

        //Deletes a specific movie
        public void MovieDelete(Movie movie)
        {
            context.Movies.Remove(movie);
            context.SaveChanges();
        }

        //Returns a list of genres
        public List<Genre> GetGenres()
        {
            var genres = context.Genres;
            return genres.ToList();
        }
        
        //Returns a genre that has a particular id
        public Genre GetGenresById(int genreId)
        {
            var gen = context.Genres.FirstOrDefault(x => x.Id == genreId);
            return gen;
        }

        //Add the MovieGenre sent
        public void MovieGenreAdd(MovieGenre movieGenre)
        {
            context.MovieGenres.Add(movieGenre);
            context.SaveChanges();
        }

        //Adds the user sent
        public void UserAdd(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        //Returns a user that has a particular id
        public User GetUserById(long userId)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == userId);
            return user;
        }

        //Returns a list of users
        public List<User> GetUsers()
        {
            var user = context.Users;
            return user.ToList();
        }

        //Returns a list of occupations
        public List<Occupation> GetOccupations()
        {
            var occ = context.Occupations;
            return occ.ToList();
        }

        //Returns an occupation that has a particular id
        public Occupation GetOccupationsById(int occId)
        {
            var occ = context.Occupations.FirstOrDefault(x => x.Id == occId);
            return occ;
        }

        //Add the MovieRating sent
        public void MovieRatingAdd(UserMovie userMovie)
        {
            context.UserMovies.Add(userMovie);
            context.SaveChanges();
        }

        //Returns a list of user movies
        public List<UserMovie> GetUserMovies()
        {
            var um = context.UserMovies;
            return um.ToList();
        }
    }
}
