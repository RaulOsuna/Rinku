using System.Data;
using System.Data.SqlClient;
using Azure;
using Microsoft.Extensions.Configuration;
using PruebaTecnicaCoppel.Models;

namespace PruebaTecnicaCoppel.DAL
{
    public class EmployeeDAL
    {
        string ConString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"].ToString();
         
        public ResponseData SaveEmployee(Employee employeeParam)
        {
            ResponseData response = new ResponseData();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SaveEmployee";
                    command.Parameters.AddWithValue("@NumberEmployee", employeeParam.EmployeeNumber);
                    command.Parameters.AddWithValue("@Name", employeeParam.Name);
                    command.Parameters.AddWithValue("@Role", employeeParam.role);
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtResponse = new DataTable();
                    connection.Open();
                    sqlDA.Fill(dtResponse);
                    connection.Close();
                    response.Message = dtResponse.Rows[0]["Message"].ToString() ?? "";
                    response.ResponseCode = Convert.ToInt32(dtResponse.Rows[0]["ResponseCode"]);
                }
            }
            catch (Exception e)
            {

                response.Message = "Un Error ha ocurrio: "+e.Message.ToString();
                response.ResponseCode = 0;
            }
            
            return response;
            
        }
        public Employee GetEmployeeById(long Id)
        {
            Employee employee = new Employee();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetEmployeeById";
                    command.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtEmployee = new DataTable();

                    connection.Open();
                    sqlDA.Fill(dtEmployee);
                    connection.Close();
                    employee.Id = Convert.ToInt64(dtEmployee.Rows[0]["Id"]);
                    employee.Name = dtEmployee.Rows[0]["Name"].ToString() ?? "";
                    employee.role = Convert.ToInt32(dtEmployee.Rows[0]["role"]);
                    employee.EmployeeNumber = Convert.ToInt64(dtEmployee.Rows[0]["NumberEmployee"]);
                    employee.Status = Convert.ToBoolean(dtEmployee.Rows[0]["Status"]);
                }
            }
            catch (Exception)
            {

                
            }
            

            return employee;
        }
        public ResponseData UpdateEmployee(Employee employeeParam)
        {
            ResponseData response = new ResponseData();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateEmployeeById";
                    command.Parameters.AddWithValue("@Id", employeeParam.Id);
                    command.Parameters.AddWithValue("@NumberEmployee", employeeParam.EmployeeNumber);
                    command.Parameters.AddWithValue("@Name", employeeParam.Name);
                    command.Parameters.AddWithValue("@Role", employeeParam.role);
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtResponse = new DataTable();
                    connection.Open();
                    sqlDA.Fill(dtResponse);
                    connection.Close();
                    response.Message = dtResponse.Rows[0]["Message"].ToString() ?? "";
                    response.ResponseCode = Convert.ToInt32(dtResponse.Rows[0]["ResponseCode"]);
                }
            }
            catch (Exception e)
            {

                response.Message = "Un Error ha ocurrio: " + e.Message.ToString();
                response.ResponseCode = 0;
            }
            
            return response;

        }

        public List<Employee> GetEmployeesActivated()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetEmployeesWithStatusAvailable";
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtEmployee = new DataTable();

                    connection.Open();
                    sqlDA.Fill(dtEmployee);
                    connection.Close();
                    foreach (DataRow dt in dtEmployee.Rows)
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt64(dt["Id"]),
                            Name= dt["Name"].ToString()??"",
                            EmployeeNumber= Convert.ToInt64(dt["NumberEmployee"]),
                            role= Convert.ToInt32(dt["role"]),
                            Status = Convert.ToBoolean(dt["Status"])
                        });
                    }
                    
                }
            }
            catch (Exception)
            {


            }


            return employees;
        }
        public ResponseData DeleteEmployee(long Id)
        {
            ResponseData responseData = new ResponseData();
            using (SqlConnection connection = new SqlConnection(ConString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "DeleteEmployeeById";
                command.Parameters.AddWithValue("@Id", Id);

                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtMove = new DataTable();
                connection.Open();
                sqlDA.Fill(dtMove);
                connection.Close();
                responseData.Message = dtMove.Rows[0]["Message"].ToString() ?? "";
                responseData.ResponseCode = Convert.ToInt32(dtMove.Rows[0]["ResponseCode"]);
            }
            return responseData;

        }

    }
}
