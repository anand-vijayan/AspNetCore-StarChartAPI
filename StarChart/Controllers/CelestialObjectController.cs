using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}",Name = "GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if(celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();

            if (celestialObjects == null)
            {
                return NotFound();
            }

            celestialObjects.ForEach(c => c.Satellites = _context.CelestialObjects.Where(co => c.OrbitedObjectId == co.Id).ToList());
            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.ToList();
            celestialObjects.ForEach(c => c.Satellites = _context.CelestialObjects.Where(co => c.OrbitedObjectId == co.Id).ToList());
            return Ok(celestialObjects);
        }
    }
}
