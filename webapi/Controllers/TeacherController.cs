using Microsoft.AspNetCore.Mvc;
//using System.Data;
using System.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
using webapi.Models;
//using System.Collections.Generic;


namespace webapi.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class TeacherController : ControllerBase
        {

            private readonly IConfiguration _configuration;
            public TeacherController(IConfiguration configuration)
            {
                _configuration = configuration;
            }

        [HttpGet]
        public IActionResult Get()
        {
            string query = @"SELECT Name FROM dbo.Teachers";

            List<string?> teacherNames = new List<string?>();

            string? sqlDataSource = _configuration.GetConnectionString("TeachersAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            string? teacherName = myReader["Name"]?.ToString();
                            teacherNames.Add(teacherName);
                        }
                    }
                }
            }

            return new JsonResult(teacherNames);
        }

        [HttpPost]
        public IActionResult Post(TeachersDetails tec)
        {
            string query = @"INSERT INTO dbo.Teachers (Name) VALUES (@Name)";

            string? sqlDataSource = _configuration.GetConnectionString("TeachersAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Name", tec.name);
                    myCommand.ExecuteNonQuery();
                }
            }

            return Ok("Added Successfully");
        }
    }      
    
}
