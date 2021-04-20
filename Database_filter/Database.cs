using System;
using System.Collections.Generic;
using System.IO;
using Npgsql;

namespace Database_filter
{
    public class Database
    {
        private string connection = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=Eroxa";
        private NpgsqlConnection connect;

        public Database()
        {
            connect = new NpgsqlConnection(connection);
            connect.Open();
        }

        public void Menu()
        {
            bool test = true;
            while (test)
            {
                Console.WriteLine(" ----------------------");
                Console.WriteLine("|1.Filter by language  |");
                Console.WriteLine("|2.Filter by year      |");
                Console.WriteLine("|3.Filter by actor     |");
                Console.WriteLine("|4.Filter by category  |");
                Console.WriteLine("|5.Filter by rating    |");
                Console.WriteLine("|6.Filter by cost      |");
                Console.WriteLine("|7.Search by name      |");
                Console.WriteLine("|8.Filter by Lrngth    |");
                Console.WriteLine("|9.All actor in film   |");
                Console.WriteLine("|10.Exit               |");
                Console.WriteLine(" ----------------------");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ListLanguage();
                        Console.WriteLine("Pls select language and enter id");
                        int languageId = int.Parse(Console.ReadLine());
                        FilterByLanguage(languageId);
                        break;
                    case 2:
                        FilterByYear();
                        break;
                    case 3:
                        FilterByActor();
                        break;
                    case 4:
                        ShowCategory();
                        Console.WriteLine("Pls select id of category:");
                        int id = int.Parse(Console.ReadLine());
                        FilterByCategory(id);
                        break;
                    case 5:
                        FilterByRating();
                        break;
                    case 6:
                        FilterByCost();
                        break;
                    case 7:
                        SearchByName();
                        break;
                    case 8:
                        FilterByLenght();
                        break;
                    case 9:
                        AllActorInFilm();
                        break;
                    default:
                        test = false;
                        break;

                }
            }
        }

        public void ListLanguage()
        {
            string command = "SELECT * FROM language";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader[1] + " " +  reader[2]);
            }
            reader.Dispose();
        }
        public void FilterByLanguage(int id)
        {
            string command = $"SELECT * FROM film WHERE film.language_id ={id};\n";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader[1]);
            }
            reader.Dispose();
        }

        public void FilterByYear()
        {
            Console.WriteLine("pls input year of your films");
            int year = int.Parse(Console.ReadLine());
            string command = $"SELECT * FROM film WHERE film.release_year ={year};\n";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[1]);
            }
            reader.Dispose();
        }

        public void  FilterByActor()
        {
            Console.WriteLine("Pls input Name of Actor");
            string name = Console.ReadLine();
            Console.WriteLine("Pls input surName of Actor");
            string surname = Console.ReadLine();
            string command = $"SELECT title FROM film JOIN film_actor ON film_actor.film_id = film.film_id JOIN actor ON actor.actor_id = film_actor.actor_id WHERE actor.first_name = '{name}' AND actor.last_name = '{surname}'";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0]);
            }
            reader.Dispose();
        }

        public void ShowCategory()
        {
            string command = "SELECT * FROM category;";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0] + " " + reader[1]);
            }
            reader.Dispose();
        }
        
        public void FilterByCategory(int id)
        {
            string command = $"SELECT * From film JOIN film_category ON film.film_id = film_category.film_id JOIN category ON film_category.category_id = category.category_id WHERE category.category_id = {id}";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[1]);
            }
            reader.Dispose();
        }

        public void FilterByRating()
        {
            Console.WriteLine("Select raing (NC-17; R; PG-13; PG; G)");
            string rating = Console.ReadLine();
            string command = $"SELECT * FROM film WHERE rating = '{rating}'";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[1]);
            }
            reader.Dispose();
        }

        public void FilterByCost()
        {
            Console.WriteLine("Input cost (9.99-29.99)");
            float cost = float.Parse(Console.ReadLine());
            string command = $"SELECT * FROM film WHERE replacement_cost < {cost.ToString().Replace(',','.')}";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[1]);
            }
            reader.Dispose();
        }

        public void SearchByName()
        {
            Console.WriteLine("Input Film title:");
            string name = Console.ReadLine();
            string command = $"SELECT * FROM film WHERE film.title = '{name}'";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            List<string> outputs = new List<string>();
            while (reader.Read())
            {
                outputs.Add((string)reader[1]);
            }
            if (outputs.Count > 0)
            {
                for (int i = 0; i < outputs.Count; ++i)
                {
                    Console.WriteLine(outputs[i]);
                }
            }
            else
            {
                Console.WriteLine("not found");
            }
            reader.Dispose();
        }

        public void AllActorInFilm()
        {
            Console.WriteLine("Input name of uour film");
            string nameOfFilm = Console.ReadLine();
            string command = $"SELECT * FROM actor JOIN film_actor ON film_actor.actor_id = actor.actor_id JOIN film ON film.film_id = film_actor.film_id  WHERE film.title = '{nameOfFilm}' ";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            List<string> outputs = new List<string>();
            while (reader.Read())
            {
                outputs.Add((string)reader[1] + " " + reader[2]);
            }
            if (outputs.Count > 0)
            {
                for (int i = 0; i < outputs.Count; ++i)
                {
                    Console.WriteLine(outputs[i]);
                }
            }
            else
            {
                Console.WriteLine("not found");
            }
            reader.Dispose();
        }

        public void FilterByLenght()
        {
            Console.WriteLine("Input length");
            int length = int.Parse(Console.ReadLine());
            string command = $"SELECT * FROM film WHERE film.length < {length}";
            var cmd = new NpgsqlCommand(command, connect);
            var reader = cmd.ExecuteReader();
            List<string> outputs = new List<string>();
            while (reader.Read())
            {
                outputs.Add((string)reader[1]);
            }
            if (outputs.Count > 0)
            {
                for (int i = 0; i < outputs.Count; ++i)
                {
                    Console.WriteLine(outputs[i]);
                }
            }
            else
            {
                Console.WriteLine("not found");
            }
            reader.Dispose();
        }
        /*public void AddNewFilm()
        {
            Console.WriteLine("Title:");
            string title = Console.ReadLine();
            Console.WriteLine("Discription:");
            string description = Console.ReadLine();
            Console.WriteLine("Year:");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("Language id 1-6:");
            int languageId = int.Parse(Console.ReadLine());
            Console.WriteLine("rental id:");
            int rentalD = int.Parse(Console.ReadLine());
            Console.WriteLine("Rental rate:");
            int rental_rate = int.Parse(Console.ReadLine());
            Console.WriteLine("Lenght:");
            int length = int.Parse(Console.ReadLine());
            Console.WriteLine("rating (NC-17; R; PG-13; PG; G)");
            string rating = Console.ReadLine();
            Console.WriteLine("Special Futures");
            string specialFutures = Console.ReadLine();
            Console.WriteLine("Full text");
            string fullText = Console.ReadLine();
            var command = "INSERT INTO film (title, description, release_year, language_id, rental_duration, rental_rate, lenght, replacement_cost, rating, special_features, full_text) VALUES(@title, @, )";
            var cmd = new NpgsqlCommand(command, connect);
            cmd.Parameters.AddWithValue("title", film.title);
            cmd.Parameters.AddWithValue("description", film.descriptipn);
            
            cmd.ExecuteNonQuery();
        }*/
    }
}