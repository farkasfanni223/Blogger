using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Reflection.Metadata.Ecma335;

namespace BlogApi.Controllers
{
    [Route("blogger")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        public readonly string ConnectionString = "server=localhost;database=blog;user=root;password=";

        [HttpGet("bloggers")]
        public object GetAllBlogger()
        {
            List<Blogger> lista = new List<Blogger>();

            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT * FROM blogger";

            var cmd = new MySqlCommand(sql, connector);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var blogger = new Blogger
                {
                    Id = datareader.GetInt32(0),
                    Name = datareader.GetString(1),
                    Email = datareader.GetString(2),
                    Age = datareader.GetInt32(3),
                    Password = datareader.GetString(4),
                    RegistrationTime = datareader.GetDateTime(5)
                };

                lista.Add(blogger);
            }

            connector.Close();

            return new { message = "Sikeres lekérdezés.", result = lista };
        }

        [HttpGet("byId/{id}")]
        public object GetBloggerById([FromRoute] int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT * FROM blogger WHERE  id = @id";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            object result = null;

            if (datareader.Read())
            {
                var blogger = new Blogger
                {
                    Id = datareader.GetInt32(0),
                    Name = datareader.GetString(1),
                    Email = datareader.GetString(2),
                    Age = datareader.GetInt32(3),
                    Password = datareader.GetString(4),
                    RegistrationTime = datareader.GetDateTime(5)
                };

                result = new { message = "Sikeres lekérdezés.", result = blogger };
            }
            else
            {
                result = new { message = "Sikertlen lekérdezés.", result = "" };
            }

            connector.Close();

            return result;
        }

        [HttpPost("login")]
        public object PostBloggerLogin([FromBody] LoginBloggerDto loginBloggerDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT * FROM blogger WHERE  email = @email AND password = @password";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@email", loginBloggerDto.Email);
            cmd.Parameters.AddWithValue("@password", loginBloggerDto.Password);

            var datareader = cmd.ExecuteReader();
            object result = null;

            if (datareader.Read())
            {
                var blogger = new Blogger
                {
                    Id = datareader.GetInt32(0),
                    Name = datareader.GetString(1),
                    Email = datareader.GetString(2),
                    Age = datareader.GetInt32(3),
                    Password = datareader.GetString(4),
                    RegistrationTime = datareader.GetDateTime(5)
                };

                result = new { message = "Regisztrált tag.", result = blogger };
            }
            else
            {
                result = new { message = "Nem rgisztrált tag.", result = "" };
            }

            connector.Close();

            return result;
        }

        [HttpPost("register")]
        public object AddNewBlogger([FromBody] RegisterBloggerDto registerBloggerDto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"INSERT INTO `blogger`(`name`, `email`, `age`, `password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationTime)";

            var cmd = new MySqlCommand(@sql, connector);

            cmd.Parameters.AddWithValue("@name", registerBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", registerBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", registerBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", registerBloggerDto.Password);
            cmd.Parameters.AddWithValue("@registrationTime", DateTime.Now);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new { message = "Sikeres hozzáadás", result = registerBloggerDto };
        }

        [HttpPut("update/{id}")]
        public object UpdateBlogger([FromRoute]int id)
        { 
        
        }
    }


}
