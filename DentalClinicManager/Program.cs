using DentalClinicManager.Data;
using DentalClinicManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext واحد للتطبيق + Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // مطلوب لصفحات Identity [web:211]

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// مهم: Authentication قبل Authorization
app.UseAuthentication(); // [web:211]
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // صفحات /Identity/Account/Login ... [web:211]

// -------------------- Seed Data --------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // الأفضل مع migrations + identity
    context.Database.Migrate(); // بدل EnsureCreated [web:211]

    if (!context.Patients.Any())
    {
        var patientsList = new List<Patient>
        {
            new Patient { Name = "عبدالرحمن علي", DateOfBirth = new DateTime(1999, 6, 22), Gender = "ذكر", Phone = "09100000000", Email = "Abdurhman@email.com", Address = "البيضاء" },
            new Patient { Name = "علي محمد حسن", DateOfBirth = new DateTime(2000, 7, 18), Gender = "ذكر", Phone = "0920000000", Email = "Ali@email.com", Address = "شحات" },
            new Patient { Name = "محمد علي", DateOfBirth = new DateTime(1990, 3, 15), Gender = "ذكر", Phone = "0911111111", Email = "mohammed@example.com", Address = "طرابلس" },
            new Patient { Name = "سارة أحمد", DateOfBirth = new DateTime(1985, 5, 22), Gender = "أنثى", Phone = "0922222222", Email = "sarah@example.com", Address = "بنغازي" },
            new Patient { Name = "خالد حسن", DateOfBirth = new DateTime(1983, 12, 19), Gender = "ذكر", Phone = "0943333333", Email = "khaled@example.com", Address = "مصراتة" }
        };
        context.Patients.AddRange(patientsList);
        context.SaveChanges();
    }

    if (!context.Doctors.Any())
    {
        var doctorsList = new List<Doctor>
        {
            new Doctor { Name = "د. أحمد المصري", Specialty = "طب الأسنان العام", Phone = "0911234567", Email = "ahmed@clinic.com" },
            new Doctor { Name = "د. فاطمة سالم", Specialty = "تقويم الأسنان", Phone = "0922345678", Email = "fatma@clinic.com" },
            new Doctor { Name = "د. خالد عمر", Specialty = "جراحة الفم والوجه", Phone = "0933456789", Email = "khaled@clinic.com" },
            new Doctor { Name = "د. ليلى حسن", Specialty = "علاج الجذور", Phone = "0944567890", Email = "layla@clinic.com" },
            new Doctor { Name = "د. محمود علي", Specialty = "تجميل الأسنان", Phone = "0955678901", Email = "mahmoud@clinic.com" }
        };
        context.Doctors.AddRange(doctorsList);
        context.SaveChanges();
    }

    if (!context.Appointments.Any())
    {
        var patients = context.Patients.ToList();
        var doctors = context.Doctors.ToList();

        if (patients.Count >= 2 && doctors.Count >= 2)
        {
            var appointmentsList = new List<Appointment>
            {
                new Appointment { Date = DateTime.Now.AddDays(1), Time = "09:00 صباحاً", Notes = "فحص دوري", PatientId = patients[0].PatientId, DoctorId = doctors[0].DoctorId },
                new Appointment { Date = DateTime.Now.AddDays(2), Time = "10:30 صباحاً", Notes = "تنظيف أسنان", PatientId = patients[1].PatientId, DoctorId = doctors[1].DoctorId }
            };

            if (patients.Count >= 3 && doctors.Count >= 3)
            {
                appointmentsList.Add(new Appointment { Date = DateTime.Now.AddDays(3), Time = "02:00 مساءً", Notes = "خلع ضرس", PatientId = patients[0].PatientId, DoctorId = doctors[2].DoctorId });
            }

            context.Appointments.AddRange(appointmentsList);
            context.SaveChanges();
        }
    }

    if (!context.Treatments.Any())
    {
        var patients = context.Patients.ToList();
        var doctors = context.Doctors.ToList();

        if (patients.Count >= 2 && doctors.Count >= 2)
        {
            var treatmentsList = new List<Treatment>
            {
                new Treatment { Description = "حشوة أسنان", Cost = 150, Date = DateTime.Now.AddDays(-5), PatientId = patients[0].PatientId, DoctorId = doctors[0].DoctorId },
                new Treatment { Description = "تنظيف وتلميع", Cost = 80, Date = DateTime.Now.AddDays(-3), PatientId = patients[1].PatientId, DoctorId = doctors[1].DoctorId }
            };

            if (patients.Count >= 3 && doctors.Count >= 3)
            {
                treatmentsList.Add(new Treatment { Description = "خلع ضرس العقل", Cost = 300, Date = DateTime.Now.AddDays(-7), PatientId = patients[0].PatientId, DoctorId = doctors[2].DoctorId });
            }

            context.Treatments.AddRange(treatmentsList);
            context.SaveChanges();
        }
    }
}

app.Run();
