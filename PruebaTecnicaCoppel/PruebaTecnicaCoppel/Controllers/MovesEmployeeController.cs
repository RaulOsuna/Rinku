using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaCoppel.DAL;
using PruebaTecnicaCoppel.Models;
using System.Text.Json;

namespace PruebaTecnicaCoppel.Controllers
{
    public class MovesEmployeeController : Controller
    {
        // GET: MovesEmployeeController
        public ActionResult Index()
        {

            return View();
        }

        // GET: MovesEmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MovesEmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MovesEmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovesEmployee collection)
        {
            MoveEmployeeDAL moveEmployeeDAL = new MoveEmployeeDAL();
           
            if (collection.EmployeeId==0 ||collection.DateMove==default(DateTime) || collection.Deliver <= 0)
            {
                TempData["Error"] = "Error, revisar los datos e intentar nuevamente!";
                return View();
            }
            ResponseData response = new ResponseData();
            try
            {
                response = moveEmployeeDAL.SaveMoveEmployee(collection);
                if (response.ResponseCode==1)
                {
                    return RedirectToAction(nameof(Index));
                }else
                {
                    TempData["Error"] = response.Message;
                }
            }
            catch
            {
                TempData["Error"] = "Error, revisar los datos e intentar nuevamente!";
            }
            
            return View();  
        }

        // GET: MovesEmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.IdMoveEmployee=id;
            return View();
        }

        // POST: MovesEmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovesEmployee collection)
        {
            ResponseData response = new ResponseData();
            if (collection.EmployeeId == 0 || collection.DateMove == default(DateTime) || collection.Deliver<=0)
            {
                TempData["Error"] = "Error, revisar los datos e intentar nuevamente!";
                return View();
            }
            MoveEmployeeDAL moveEmployeeDAL = new MoveEmployeeDAL();
            try
            {
                response = moveEmployeeDAL.UpdateMoveEmployee(collection);
                if (response.ResponseCode==1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = response.Message;
                }
                
            }
            catch
            {
                TempData["Error"] = "Error, revisar los datos e intentar nuevamente!";
            }
            
            return View();
        }

        // GET: MovesEmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MovesEmployeeController/Delete/5
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
        public List<Employee> GetEmployeesAvailable()
        {
            List<Employee> employees = new List<Employee>();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            try
            {
                employees=employeeDAL.GetEmployeesActivated();
            }
            catch (Exception)
            {

                
            }


            return employees;
        }

        [HttpPost]
        public List<MovesEmployeeCalculated> GetMoveEmployeeCalculatesByDateAndIdEmployee(int Id,int Month, int Year)
        {
            
            List<MovesEmployeeCalculated> employees = new List<MovesEmployeeCalculated>();
            if (Id==0||Month==0||Year==0)
            {
                return employees;
            }
            MoveEmployeeDAL employeeDAL = new MoveEmployeeDAL();
            try
            {
                employees = employeeDAL.GetMoveEmployeeCalculatesByDateAndIdEmployee(Id,Month,Year);
            }
            catch (Exception)
            {


            }


            return employees;
        }

        [HttpPost]
        public List<MovesEmployeeCalculated> GetMovesEmployeeAvailablesALL()
        {
            List<MovesEmployeeCalculated> employees = new List<MovesEmployeeCalculated>();
            MoveEmployeeDAL employeeDAL = new MoveEmployeeDAL();
            try
            {
                employees = employeeDAL.GetMovesEmployeeAvailablesALL();
            }
            catch (Exception)
            {


            }


            return employees;
        }

        
        [HttpPost]
        public MovesEmployee GetDataFromMoveById(long IdMove)
        {
            MovesEmployee MovesEmployees = new MovesEmployee();
            if (IdMove==0)
            {
                return MovesEmployees;
            }
            MoveEmployeeDAL MovesEmployeesDAL = new MoveEmployeeDAL();
            try
            {
                MovesEmployees = MovesEmployeesDAL.GetDataFromMoveById(IdMove);
            }
            catch (Exception)
            {


            }


            return MovesEmployees;
        }

        [HttpPost]
        public ResponseData DeleteMoveEmployee(long IdMove)
        {
            ResponseData response = new ResponseData();
            MoveEmployeeDAL MovesEmployeesDAL = new MoveEmployeeDAL();

            try
            {
                response = MovesEmployeesDAL.DeleteMoveEmployee(IdMove);
                if (response.ResponseCode ==1)
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

                TempData["Error"] = "Error: "+e.Message;
            }


            return response;
        }
    }

}
