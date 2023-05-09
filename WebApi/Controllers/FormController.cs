using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public FormController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(Form form)
        {
            string query = @"select * from dbo.Contact where ContactEmail = '"+form.ContactEmail+@"' and ContactPhone = '"+form.ContactPhone+@"'";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WebApiAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    
                    myReader.Close();
                    myCon.Close();
                }
            }

            if (table.Rows.Count == 0)
            {
                query = @"insert into dbo.Contact values ('"+form.ContactName+@"', '"+form.ContactEmail+@"', '"+form.ContactPhone+@"')";
                table = new DataTable();
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
                
                query = @"select * from dbo.Contact where ContactEmail = '"+form.ContactEmail+@"' and ContactPhone = '"+form.ContactPhone+@"'";
                table = new DataTable();
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }

            form.ContactId = Int32.Parse(table.Rows[0][0].ToString());

            query = @"insert into dbo.Message values ('"+form.ContactId+@"', '"+form.ThemeId+@"', '"+form.MessageObject+@"')";
            table = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            
            query = @"select * from dbo.Message where MessageId = (select MAX(MessageId) from dbo.Message) and MessageContactId = '"+form.ContactId+@"'";            
            table = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            
            form.MessageObject = table.Rows[0][3].ToString();
            form.MessageId = Int32.Parse(table.Rows[0][0].ToString());

            return new JsonResult(form);
        }
    }
}
