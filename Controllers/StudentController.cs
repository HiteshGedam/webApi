using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using WebApiApp.Data;
using WebApiApp.Models;
using WebApiApp.MyLogging;

namespace WebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _Logger; 
        private readonly CollegeDBContext _dbContext;

        public StudentController(ILogger<StudentController> logger, CollegeDBContext dBContext)
        {
            _Logger = logger;
            _dbContext = dBContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _Logger.LogInformation("Index method of student controller caled");
            return Ok();
        }

        [HttpGet]
        [Route("ALL", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudent()
        {
            var obj = _dbContext.students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address,
                DOB = s.DOB
            }).ToList();
            return Ok(obj);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _Logger.LogError("Student Id can not be 0.");
                return BadRequest();
            }
            var s = _dbContext.students.Where(s => s.Id == id).FirstOrDefault();
            if (s == null)
            {
                _Logger.LogWarning("Student not found.");
                return NotFound($"the student id {id} not found.");
            }
            StudentDTO obj = new StudentDTO()
            {
                Id = id,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address,
                DOB = s.DOB
            };
            return Ok(obj);
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogError("Student name can not be blank.");
                return BadRequest();
            }
            var s = _dbContext.students.Where(s => s.Name == name).FirstOrDefault();
            if (s == null)
            {
                _Logger.LogWarning("Student not found.");
                //NotFound --- 404 --- client error
                return NotFound($"the student name {name} not found.");
            }
            StudentDTO obj = new StudentDTO()
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address,
                DOB = s.DOB
            };
            return Ok(obj);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public ActionResult<StudentDTO> CreateStudets([FromBody] StudentDTO model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            if (model == null)
            {
                return BadRequest();
            }

            //if (model.admitionDate < DateTime.Now)
            //{
            //    ModelState.AddModelError("admitionDateError", "admition date must be greater than current date.");
            //    return BadRequest(ModelState);
            //}

            Student s = new Student()
            {
                Name = model.Name,
                Email = model.Email,
                Address = model.Address,
            };
            _dbContext.students.Add(s);
            _dbContext.SaveChanges();


            model.Id = s.Id;
            //return Ok(s);

            //status - 201
            //http://localhost:5220/api/Student/3
            //New student model return
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
        }


        [HttpPut]
        [Route("Update")]
        //asp/Student/Update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDTO value)
        {
            if (value == null || value.Id <= 0)
            {
                return BadRequest();
            }

            var existingStudent  = _dbContext.students.Where(s => s.Id == value.Id).FirstOrDefault();
            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.Name = value.Name;
            existingStudent.Email = value.Email;
            existingStudent.Address = value.Address;
            existingStudent.DOB  = value.DOB;
            _dbContext.SaveChanges();
            return NoContent();

        }


        [HttpPatch]
        [Route("{id:int}UpdatePartial")]
        //api/Student/1/UpdatePrtial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial(int id,[FromBody] JsonPatchDocument<StudentDTO> patchDocumnet)
        {
            if (patchDocumnet == null || id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = _dbContext.students.Where(s => s.Id == id).FirstOrDefault();
            if (existingStudent == null)
            {
                return NotFound();
            }

            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                Name = existingStudent.Name,
                Email = existingStudent.Email,
                Address = existingStudent.Address,
                DOB = existingStudent.DOB,
            };

            patchDocumnet.ApplyTo(studentDTO,ModelState);
            _dbContext.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingStudent.Name = studentDTO.Name;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;
            existingStudent.DOB = studentDTO.DOB;
            _dbContext.SaveChanges();
            return NoContent();

        }



        [HttpDelete]
        [Route("{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeletStudent(int id)
        {
            if (id <= 0)
            {
                //BadRequest --- 400 --- cient error
                return BadRequest();
            }
            Student s = _dbContext.students.Where(s => s.Id == id).FirstOrDefault();
            if (s == null)
            {
                //NotFound --- 404 --- client error
                return NotFound($"the student id {id} not found.");
            }
            _dbContext.students.Remove(s);
            _dbContext.SaveChanges();
            return Ok(true);
        }
    }
}
