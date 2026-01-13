using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentalClinicManager.Data;
using DentalClinicManager.Models;

namespace DentalClinicManager.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor);

            return View(await appointments.ToListAsync());
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name");
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name");
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,Date,Time,DurationMinutes,Notes,PatientId,DoctorId")] Appointment appointment)
        {
            // احتياط: لو ما وصل من الـ hidden
            if (appointment.DurationMinutes <= 0)
                appointment.DurationMinutes = 30;

            // لو Date ما وصل (لأنه DateTime? الآن)
            if (appointment.Date == null)
                ModelState.AddModelError(nameof(appointment.Date), "تاريخ الموعد مطلوب");

            if (ModelState.IsValid)
            {
                if (await HasConflictAsync(appointment))
                {
                    ModelState.AddModelError(string.Empty, "يوجد تعارض: الطبيب لديه موعد آخر ضمن نفس الفترة.");
                }
                else
                {
                    try
                    {
                        _context.Add(appointment);
                        await _context.SaveChangesAsync();
                        TempData["Saved"] = "تم حفظ الموعد بنجاح";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "DB Error: " + ex.Message);
                    }
                }
            }
            else
            {
                // اجمع الأخطاء في Summary لكي تراها فوق الفورم
                foreach (var kv in ModelState)
                {
                    foreach (var err in kv.Value.Errors)
                    {
                        if (!string.IsNullOrWhiteSpace(err.ErrorMessage))
                            ModelState.AddModelError(string.Empty, $"{kv.Key}: {err.ErrorMessage}");
                    }
                }
            }

            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,Date,Time,DurationMinutes,Notes,PatientId,DoctorId")] Appointment appointment)
        {
            if (id != appointment.AppointmentId) return NotFound();

            if (appointment.DurationMinutes <= 0)
                appointment.DurationMinutes = 30;

            if (appointment.Date == null)
                ModelState.AddModelError(nameof(appointment.Date), "تاريخ الموعد مطلوب");

            if (ModelState.IsValid)
            {
                if (await HasConflictAsync(appointment))
                {
                    ModelState.AddModelError(string.Empty, "يوجد تعارض: الطبيب لديه موعد آخر ضمن نفس الفترة.");
                }
                else
                {
                    try
                    {
                        _context.Update(appointment);
                        await _context.SaveChangesAsync();
                        TempData["Saved"] = "تم تعديل الموعد بنجاح";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Appointments.Any(e => e.AppointmentId == appointment.AppointmentId))
                            return NotFound();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "DB Error: " + ex.Message);
                    }
                }
            }

            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
            return View(appointment);
        }

        // Conflict check
        private async Task<bool> HasConflictAsync(Appointment appointment)
        {
            if (appointment.Date == null)
                return false;

            if (!TimeSpan.TryParse(appointment.Time, out var newTime))
                return false;

            var newStart = appointment.Date.Value.Date + newTime;
            var newEnd = newStart.AddMinutes(appointment.DurationMinutes);

            var existing = await _context.Appointments
                .Where(a => a.DoctorId == appointment.DoctorId &&
                            a.AppointmentId != appointment.AppointmentId)
                .ToListAsync();

            foreach (var a in existing)
            {
                if (a.Date == null) continue;
                if (!TimeSpan.TryParse(a.Time, out var oldTime)) continue;

                var oldStart = a.Date.Value.Date + oldTime;
                var oldEnd = oldStart.AddMinutes(a.DurationMinutes);

                if (newStart < oldEnd && newEnd > oldStart)
                    return true;
            }

            return false;
        }
    }
}
