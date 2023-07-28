using Azure.Core.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaCoppel.DAL;
using PruebaTecnicaCoppel.Models;
using System.Text.Json;

namespace PruebaTecnicaCoppel.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: EmployeeController
        public ActionResult Index()
        {
            List<Employee> ListaEmployees = new List<Employee>(); 
            EmployeeDAL employeeDAL = new EmployeeDAL();
            ListaEmployees = employeeDAL.GetEmployeesActivated();
            string jsonString = JsonSerializer.Serialize(ListaEmployees);
            ViewBag.ListaEmpleados = jsonString;
            return View();
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee collection)
        {
            if (collection.Name=="" || collection.EmployeeNumber==0 || collection.Name==null || collection.Name.Trim()=="")
            {
                TempData["Error"] = "Error, revisar los datos e intentar nuevamente!";
                return View();
            }

            Employee employee = new Employee();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            try
            {
                ResponseData responseData = new ResponseData();
                responseData = employeeDAL.SaveEmployee(collection);
                if (responseData.ResponseCode==1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Error, revisar los datos e intentar nuevamente! " + responseData.Message;
                    return View();
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = "Error, revisar los datos e intentar nuevamente! "+ e.Message.ToString();

             

                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            Employee employee = new Employee();
            EmployeeDAL employeeDAL=new EmployeeDAL();
            employee=employeeDAL.GetEmployeeById(id);
            if (employee.Id!=0)
            {
                string jsonString = JsonSerializer.Serialize(employee);
                ViewBag.Employee = jsonString;
                
            }
            

            return View();
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee collection)
        {
            if (collection.Name == "" || collection.EmployeeNumber == 0 || collection.Name == null || collection.Name.Trim() == "")
            {
                TempData["Error"] = "Error, revisar los datos e intentar nuevamente!";
                return View();
            }
            Employee employee = new Employee();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            try
            {
                ResponseData response= new ResponseData();
                response = employeeDAL.UpdateEmployee(collection);
                if (response.ResponseCode==1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Error, ya existe un registro con el numero de empleado escrito";
                }

            }
            catch
            {
                TempData["Error"] = "Error, revisar los datos e intentar nuevamente!";
                
            }
            return View();
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public Employee GetEmployeeById(long Id)
        {
            Employee Employees = new Employee();
            EmployeeDAL EmployeesDAL = new EmployeeDAL();
            try
            {
                Employees = EmployeesDAL.GetEmployeeById(Id);
            }
            catch (Exception)
            {


            }


            return Employees;
        }
        [HttpPost]
        public ResponseData DeleteEmployee(long IdEmployee)
        {
            ResponseData response = new ResponseData();
            EmployeeDAL EmployeesDAL = new EmployeeDAL();

            try
            {
                response = EmployeesDAL.DeleteEmployee(IdEmployee);
                if (response.ResponseCode == 1)
                {
                    return response;

                }
                else
                {
                    TempData["Error"] = "Error, no se logró eliminar el registro!";
                }
            }
            catch (Exception e)
            {

                TempData["Error"] = "Error: " + e.Message;
            }


            return response;
        }
    }
}
