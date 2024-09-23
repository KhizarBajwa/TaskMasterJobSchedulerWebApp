using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure.Data;

namespace Presentation.Controllers
{
    public class JobRunLogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobRunLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobRuns
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobRunLogs.ToListAsync());
        }
    }
}
