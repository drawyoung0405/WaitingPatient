
using ListingPatient.Data;
using ListingPatient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Drawing.Text;

namespace ListingPatient.Controllers
{
    public class PatientController : Controller
    {
        private readonly MyDbContext _context;
        public PatientController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetPatientNotExamined()
        {
            var patients = await _context.Patients.Where(p=> p.Status == PatientStatus.ChuaKham).OrderBy(p => p.AddedTime).ToListAsync();
            return View(patients);
        }
        public async Task<IActionResult> GetPatientBeExamined()
        {
            var patients = await _context.Patients.Where(p => p.Status == PatientStatus.DaKham).OrderBy(p => p.AddedTime).ToListAsync();
            return View(patients);
        }

        // đánh dấu đã khám
        public async Task<IActionResult> MarkExamined(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound("Không tìm thấy bệnh nhân");
            }

            patient.Status = PatientStatus.DaKham; // Chuyển đổi trạng thái sang "đã khám"

            await _context.SaveChangesAsync();

            return RedirectToAction("GetFirstPatient", new { id });
        }
        // GET: Patients/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("Bệnh nhân không hợp lệ");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound("Không tìm thấy bệnh nhân");
            }

            return View(patient);
        }
        //Edit Get To EDIT

        public async Task<IActionResult> EditView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(int id, [Bind("Id, Name, IdentificationNumber, Age, MedicalHistory")] PatientData patient)
        
       {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GetPatientNotExamined));
            }
            return RedirectToAction("GetPatientNotExamined");
        }

        //Delelete: Patients/Delete/id
        public async Task<IActionResult> DeleteById(int? id)
        {
            if (id == null)
            {
                return NotFound("Bệnh nhân không hợp lệ");
            }
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
                return RedirectToAction("GetPatientNotExamined");
        }
        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }

        //Add Patient
        public IActionResult CreatePatient()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPatient([Bind("Name,IdentificationNumber, AddedTime, Age, MedicalHistory")] PatientData patientData)
        {

            if (ModelState.IsValid)
            {
                _context.Add(patientData);
                await
                    _context.SaveChangesAsync();
                return View("SuccessPopUp");

            }
            return View(patientData);

        }

        //public IActionResult NextPatient()
        //{
        //    var nextPatient = _context.Patients.OrderBy(p => p.AddedTime).FirstOrDefault();
        //    if (nextPatient != null)
        //    {
        //        // Loại bỏ bệnh nhân đầu tiên khỏi danh sách chờ
        //        _context.Patients.Remove(nextPatient);
        //        _context.SaveChanges();

        //        return Json(new { id = nextPatient.Id });
        //    }

        //    return Json(null);
        //}

        public async Task<IActionResult> GetFirstPatient()
        {
            var firstPatient = await _context.Patients.Where(p=>p.Status==PatientStatus.ChuaKham).FirstOrDefaultAsync();

            if (firstPatient == null)
            {
                return NotFound("Không tìm thấy bệnh nhân");
            }

            return View(firstPatient);
        }

    }
}

