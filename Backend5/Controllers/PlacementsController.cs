using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;
using Backend5.Models.ViewModels;

namespace Backend5.Controllers
{
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacementsController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: Placements
        public async Task<IActionResult> Index(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            var items = await this._context.Placements
                .Include(h => h.Patient)
                .Include(h => h.Ward)
                .Where(x => x.Patient.Id == patient.Id)
                .ToListAsync();
            this.ViewBag.Patient = patient;
            return this.View(items);
        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this._context.Placements
                .Include(h => h.Patient)
                .Include(h => h.Ward)
                .Include(h => h.Ward.Hospital)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (placement == null)
            {
                return this.NotFound();
            }
            this.ViewBag.PatientId = placement.Patient.Id;
            return this.View(placement);
        }

        // GET: Placements/Create
        public async Task<IActionResult> Create(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }
            
            this.ViewBag.Patient = patient;

            var wards = this._context.Wards
                .Include(h => h.Hospital);

            this.ViewBag.Wards = wards;

            return this.View(new PlacementCreateModel());
        }

        // POST: Placements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, PlacementCreateModel model)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .Include(h => h.Placements)
                .SingleOrDefaultAsync(x => x.Id == patientId);
            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == model.WardId);

            if (patient == null)
            {
                return this.NotFound();
            }

            foreach (var place in patient.Placements)
            {
                if (place.Bed == model.Bed)
                {
                    this.ModelState.AddModelError("Bed", "This bed is busy, try another one");
                    break;
                }
            }
            if (this.ModelState.ErrorCount == 0)
            {
                if (this.ModelState.IsValid)
                {
                    var placement = new Placement
                    {
                        Patient = patient,
                        Ward = ward,
                        Bed = model.Bed
                    };

                    this._context.Add(placement);
                    await this._context.SaveChangesAsync();
                    return this.RedirectToAction("Index", new { patientId = patient.Id });
                }
            }
            this.ViewBag.Patient = patient;
            var wards = this._context.Wards
                .Include(h => h.Hospital);

            this.ViewBag.Wards = wards;

            return this.View(model);
        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this._context.Placements
                .Include(h => h.Patient)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (placement == null)
            {
                return this.NotFound();
            }
            var model = new PlacementEditModel
            {
                Bed = placement.Bed
            };
            this.ViewBag.Patient = placement.Patient;
            return this.View(model);
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PlacementEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this._context.Placements
                .Include(h => h.Patient)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                placement.Bed = model.Bed;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = placement.Patient.Id });
            }
            this.ViewBag.Patient = placement.Patient;
            return this.View(model);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this._context.Placements
                .Include(h => h.Patient)
                .Include(h => h.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Patient = placement.Patient;
            return this.View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? id)
        {
            var placement = await this._context.Placements
                .Include(h => h.Patient)
                .Include(h => h.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            this._context.Placements.Remove(placement);
            await this._context.SaveChangesAsync();
            this.ViewBag.Patient = placement.Patient;
            return this.RedirectToAction("Index", new { patientId = placement.Patient.Id });
        }
    }
}
