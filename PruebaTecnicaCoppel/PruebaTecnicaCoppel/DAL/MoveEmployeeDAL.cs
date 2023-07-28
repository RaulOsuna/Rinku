using Azure;
using PruebaTecnicaCoppel.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace PruebaTecnicaCoppel.DAL
{

    public class MoveEmployeeDAL
    {
        string ConString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"].ToString();
        public ResponseData SaveMoveEmployee(MovesEmployee employeeParam)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SaveMoveEmployee";
                    command.Parameters.AddWithValue("@IdEmployee", employeeParam.EmployeeId);
                    command.Parameters.AddWithValue("@DateMove", employeeParam.DateMove.Date);
                    command.Parameters.AddWithValue("@Role", employeeParam.Role);
                    command.Parameters.AddWithValue("@Deliver", employeeParam.Deliver);
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtMoveEmployee = new DataTable();
                    connection.Open();
                    sqlDA.Fill(dtMoveEmployee);
                    connection.Close();
                    responseData.Message = dtMoveEmployee.Rows[0]["Message"].ToString() ?? "";
                    responseData.ResponseCode = Convert.ToInt32(dtMoveEmployee.Rows[0]["ResponseCode"]);

                }
            }
            catch (Exception e)
            {

                responseData.Message = e.Message;
                responseData.ResponseCode = 0;
            }
            
            return responseData;

        }
        public ResponseData UpdateMoveEmployee(MovesEmployee employeeParam)
        {
            ResponseData responseData = new ResponseData();
            using (SqlConnection connection = new SqlConnection(ConString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UpdateMoveEmployeeById";
                command.Parameters.AddWithValue("@Id", employeeParam.Id);
                command.Parameters.AddWithValue("@IdEmployee", employeeParam.EmployeeId);
                command.Parameters.AddWithValue("@DateMove", employeeParam.DateMove.Date);
                command.Parameters.AddWithValue("@Role", employeeParam.Role);
                command.Parameters.AddWithValue("@Deliver", employeeParam.Deliver);

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

        public List<MovesEmployeeCalculated> GetMoveEmployeeCalculatesByDateAndIdEmployee(int IdEmployee, int Month, int Year)
        {
            List<MovesEmployeeCalculated> moveEmployees = new List<MovesEmployeeCalculated>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetMovesEmployeeAvailablesByIdEmployee";
                    command.Parameters.AddWithValue("@IdEmployee", IdEmployee);
                    command.Parameters.AddWithValue("@Month", Month);
                    command.Parameters.AddWithValue("@Year", Year);
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtMoveEmployee = new DataTable();
                   
                    connection.Open();
                    sqlDA.Fill(dtMoveEmployee);
                    connection.Close();
                    foreach (DataRow dt in dtMoveEmployee.Rows)
                    {
                        moveEmployees.Add(new MovesEmployeeCalculated
                        {
                            IdMove = Convert.ToInt64(dt["IdMove"]),
                            Name = dt["Name"].ToString() ?? "",
                            Role= Convert.ToInt32(dt["role"]),
                            DateMove= Convert.ToDateTime(dt["DateMove"]),
                            DateMoveSTR = dt["DateMoveSTR"].ToString() ?? "",
                            SalaryBase =Convert.ToDecimal(dt["SalaryBase"]),
                            SalaryPerMonth = Convert.ToDecimal(dt["SalaryPerMonth"]),
                            Deliver = Convert.ToDecimal(dt["Deliver"]),
                            DeliverBonus = Convert.ToDecimal(dt["DeliverBonus"]),
                            HourBonus = Convert.ToDecimal(dt["HourBonus"]),
                            ISR = Convert.ToDecimal(dt["ISR"]),
                            VoucherBonus = Convert.ToDecimal(dt["VouchersBonus"]),
                            Total = Convert.ToDecimal(dt["Total"])
                        });
                    }

                }
            }
            catch (Exception)
            {


            }


            return moveEmployees;
        }

        public List<MovesEmployeeCalculated> GetMovesEmployeeAvailablesALL()
        {
            List<MovesEmployeeCalculated> moveEmployees = new List<MovesEmployeeCalculated>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetMovesEmployeeAvailablesALL";
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtEmployee = new DataTable();

                    connection.Open();
                    sqlDA.Fill(dtEmployee);
                    connection.Close();
                    foreach (DataRow dt in dtEmployee.Rows)
                    {
                        moveEmployees.Add(new MovesEmployeeCalculated
                        {
                            IdMove=Convert.ToInt64(dt["IdMove"]),
                            Name = dt["Name"].ToString() ?? "",
                            Role = Convert.ToInt32(dt["role"]),
                            DateMove = Convert.ToDateTime(dt["DateMove"]),
                            DateMoveSTR = dt["DateMoveSTR"].ToString() ?? "",
                            SalaryBase = Convert.ToDecimal(dt["SalaryBase"]),
                            SalaryPerMonth = Convert.ToDecimal(dt["SalaryPerMonth"]),
                            Deliver = Convert.ToDecimal(dt["Deliver"]),
                            DeliverBonus = Convert.ToDecimal(dt["DeliverBonus"]),
                            HourBonus = Convert.ToDecimal(dt["HourBonus"]),
                            ISR = Convert.ToDecimal(dt["ISR"]),
                            VoucherBonus = Convert.ToDecimal(dt["VouchersBonus"]),
                            Total = Convert.ToDecimal(dt["Total"])
                        });
                    }

                }
            }
            catch (Exception)
            {


            }


            return moveEmployees;
        }

        public MovesEmployee GetDataFromMoveById(long Id)
        {
            MovesEmployee MovesEmployee = new MovesEmployee();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetMoveEmployeeById";
                    command.Parameters.AddWithValue("@Id", Id);
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtMove = new DataTable();

                    connection.Open();
                    sqlDA.Fill(dtMove);
                    connection.Close();
                    MovesEmployee.Id = Convert.ToInt64(dtMove.Rows[0]["Id"]);
                    MovesEmployee.EmployeeId= Convert.ToInt64(dtMove.Rows[0]["IdEmployee"]);
                    MovesEmployee.DateMove = Convert.ToDateTime(dtMove.Rows[0]["DateMove"]);
                    MovesEmployee.DateMoveSTR = dtMove.Rows[0]["DateMoveSTR"].ToString() ?? "";
                    MovesEmployee.Role = Convert.ToInt32(dtMove.Rows[0]["role"]);
                    MovesEmployee.Status = Convert.ToBoolean(dtMove.Rows[0]["Status"]);
                    MovesEmployee.Deliver = Convert.ToInt32(dtMove.Rows[0]["Deliver"]);
                    MovesEmployee.Name = dtMove.Rows[0]["Name"].ToString() ?? "";
                }
            }
            catch (Exception)
            {


            }


            return MovesEmployee;
        }

        public ResponseData DeleteMoveEmployee(long Id)
        {
            ResponseData responseData = new ResponseData();
            using (SqlConnection connection = new SqlConnection(ConString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "DeleteMovesEmployeeById";
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