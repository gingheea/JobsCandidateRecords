﻿using JobsCandidateRecords.Data;
using JobsCandidateRecords.Models;
using JobsCandidateRecords.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobsCandidateRecords.Models.DTO;

namespace JobsCandidateRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GetDepartments.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            var departments = await _context.Departments
                            .Include(d => d.Company)
                            .Include(d => d.Positions)
                            .ToListAsync();

            var departmentDTOs = departments.Select(d => new DepartmentDTO(
                d.Id,
                d.Name,
                d.Description,
                d.CompanyId,
                d.Company?.Name
            )).ToList();

            return Ok(departmentDTOs);
        }

        /// <summary>
        /// GetDepartment.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            var department = await _context.Departments
                                        .Include(d => d.Company)
                                        .Include(d => d.Positions)
                                        .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            var departmentDTO = new DepartmentDTO(
                department.Id,
                department.Name,
                department.Description,
                department.CompanyId,
                department.Company?.Name
            );

            return Ok(departmentDTO);
        }

        /// <summary>
        /// PutDepartment.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, UpdateDepartmentDTO departmentDTO)
        {
            if (id != departmentDTO.Id)
            {
                return BadRequest();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.Name = departmentDTO.Name;
            department.Description = departmentDTO.Description;
            department.CompanyId = departmentDTO.CompanyId;

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// PostDepartment.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(CreateDepartmentDTO createDepartmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = new Department
            {
                Name = createDepartmentDTO.Name,
                Description = createDepartmentDTO.Description,
                CompanyId = createDepartmentDTO.CompanyId
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, new DepartmentDTO(
                department.Id,
                department.Name,
                department.Description,
                department.CompanyId,
                department.Company?.Name
            ));
        }

        /// <summary>
        /// DeleteDepartment.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
