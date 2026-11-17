using System;
using System.Collections.Generic;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;

namespace JobApplicationTracker
{
    class Program
    {
        private static readonly CompanyService _companyService = new CompanyService();
        private static readonly JobService _jobService = new JobService(_companyService);
        private static readonly CandidateService _candidateService = new CandidateService();
        private static readonly ApplicationService _applicationService =
            new ApplicationService(_jobService, _candidateService, _companyService);

        static void Main(string[] args)
        {
            // E1 + Scenario 1: تحميل البيانات الأولية عند بدء التشغيل
            SeedData();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n==================================================");
                Console.WriteLine("       JOB APPLICATION TRACKER - MAIN MENU        ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. Manage Companies (List / Add / Update / Remove / Approve / Undo)");
                Console.WriteLine("2. Manage Recruiters (List / Add / Update / Remove)");
                Console.WriteLine("3. Manage Jobs (List / Find by ID / Add / Update / Remove)");
                Console.WriteLine("4. Manage Candidates (List / Add / Update / Remove)");
                Console.WriteLine("5. Manage Applications (List / Apply / Update Status / Remove)");
                Console.WriteLine("0. Exit");
                Console.WriteLine("--------------------------------------------------");

                int choice = ReadInt("Select an option: ");

                switch (choice)
                {
                    case 1:
                        ShowCompanyMenu();
                        break;
                    case 2:
                        ShowRecruiterMenu();
                        break;
                    case 3:
                        ShowJobMenu();
                        break;
                    case 4:
                        ShowCandidateMenu();
                        break;
                    case 5:
                        ShowApplicationMenu();
                        break;
                    case 0:
                        exit = true;
                        Console.WriteLine("Exiting Job Application Tracker. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose between 0 and 5.");
                        break;
                }
            }
        }

        // =====================================================================
        // E1 & Scenario 1: Fixed Seed Data (3 Companies, 3 Recruiters, 6 Jobs, 5 Candidates)
        // =====================================================================
        private static void SeedData()
        {
            // 1. إضافة 3 شركات (الشركتان 1 و 2 Approved لإضافة الوظائف الـ 6، والشركة 3 Pending لاختبار Scenario 3 & 7)
            _companyService.AddCompany(new Company { Id = 1, Name = "TechNova", Industry = "Software", City = "Cairo", Status = CompanyStatus.Approved }, out _);
            _companyService.AddCompany(new Company { Id = 2, Name = "CloudPulse", Industry = "Cloud & AI", City = "Alexandria", Status = CompanyStatus.Approved }, out _);
            _companyService.AddCompany(new Company { Id = 3, Name = "FutureBuild", Industry = "Engineering", City = "Giza", Status = CompanyStatus.Pending }, out _);

            // 2. إضافة 3 مسؤولي توظيف (Recruiters)
            _companyService.AddRecruiter(new Recruiter { Id = 1, Name = "Sara Ahmed", Email = "sara@technova.com", CompanyId = 1 }, out _);
            _companyService.AddRecruiter(new Recruiter { Id = 2, Name = "Omar Khaled", Email = "omar@cloudpulse.com", CompanyId = 2 }, out _);
            _companyService.AddRecruiter(new Recruiter { Id = 3, Name = "Mona Adel", Email = "mona@futurebuild.com", CompanyId = 3 }, out _);

            // 3. إضافة 6 وظائف (منها وظيفة مغلقة رقم 6 لاختبار Scenario 6)
            _jobService.AddJob(new Job { Id = 1, Title = "Backend .NET Developer", Description = "Build C# APIs", Salary = 25000m, Location = "Cairo", IsOpen = true, CompanyId = 1, RecruiterId = 1 }, out _);
            _jobService.AddJob(new Job { Id = 2, Title = "Frontend Engineer", Description = "UI & Web Apps", Salary = 20000m, Location = "Cairo", IsOpen = true, CompanyId = 1, RecruiterId = 1 }, out _);
            _jobService.AddJob(new Job { Id = 3, Title = "QA Automation Engineer", Description = "Testing systems", Salary = 18000m, Location = "Remote", IsOpen = true, CompanyId = 1, RecruiterId = 1 }, out _);
            _jobService.AddJob(new Job { Id = 4, Title = "Cloud Architect", Description = "Azure infrastructure", Salary = 35000m, Location = "Alexandria", IsOpen = true, CompanyId = 2, RecruiterId = 2 }, out _);
            _jobService.AddJob(new Job { Id = 5, Title = "Data Engineer", Description = "ETL pipelines", Salary = 28000m, Location = "Alexandria", IsOpen = true, CompanyId = 2, RecruiterId = 2 }, out _);
            _jobService.AddJob(new Job { Id = 6, Title = "Legacy Systems Admin", Description = "Maintenance role", Salary = 15000m, Location = "Alexandria", IsOpen = false, CompanyId = 2, RecruiterId = 2 }, out _);

            // 4. إضافة 5 مرشحين (Candidates)
            _candidateService.AddCandidate(new Candidate { Id = 1, FullName = "Ali Hassan", Email = "ali.hassan@mail.com", Phone = "01011112222", YearsOfExperience = 3 }, out _);
            _candidateService.AddCandidate(new Candidate { Id = 2, FullName = "Nourhan Tarek", Email = "nourhan@mail.com", Phone = "01122223333", YearsOfExperience = 5 }, out _);
            _candidateService.AddCandidate(new Candidate { Id = 3, FullName = "Youssef Ibrahim", Email = "youssef@mail.com", Phone = "01233334444", YearsOfExperience = 1 }, out _);
            _candidateService.AddCandidate(new Candidate { Id = 4, FullName = "Salma Mostafa", Email = "salma@mail.com", Phone = "01544445555", YearsOfExperience = 4 }, out _);
            _candidateService.AddCandidate(new Candidate { Id = 5, FullName = "Karim Mahmoud", Email = "karim@mail.com", Phone = "01099998888", YearsOfExperience = 0 }, out _);
        }

        // =====================================================================
        // 1. COMPANY SUB-MENU (E2, E3, H2)
        // =====================================================================
        private static void ShowCompanyMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- COMPANIES MENU ---");
                Console.WriteLine("1. List All Companies");
                Console.WriteLine("2. List Companies by Status (Pending / Approved / Rejected)");
                Console.WriteLine("3. Add Company");
                Console.WriteLine("4. Update Company");
                Console.WriteLine("5. Remove Company");
                Console.WriteLine("6. Change Company Status (Approve / Reject / Pending)");
                Console.WriteLine("7. Undo Last Company Status Change (H2 Stack)");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Select an option: ");
                switch (choice)
                {
                    case 1:
                        PrintCompaniesTable(_companyService.GetAllCompanies());
                        break;
                    case 2:
                        CompanyStatus filterStatus = ReadCompanyStatus("Select status to filter by (1=Pending, 2=Approved, 3=Rejected): ");
                        PrintCompaniesTable(_companyService.GetCompaniesByStatus(filterStatus));
                        break;
                    case 3:
                        string name = ReadRequiredString("Company Name: ");
                        string industry = ReadRequiredString("Industry: ");
                        string city = ReadRequiredString("City: ");
                        Company newComp = new Company { Name = name, Industry = industry, City = city, Status = CompanyStatus.Pending };
                        _companyService.AddCompany(newComp, out string addMsg);
                        Console.WriteLine(addMsg);
                        break;
                    case 4:
                        int updateId = ReadInt("Enter Company ID to update: ");
                        string newName = ReadRequiredString("New Company Name: ");
                        string newInd = ReadRequiredString("New Industry: ");
                        string newCity = ReadRequiredString("New City: ");
                        Company updatedComp = new Company { Id = updateId, Name = newName, Industry = newInd, City = newCity };
                        _companyService.UpdateCompany(updatedComp, out string updMsg);
                        Console.WriteLine(updMsg);
                        break;
                    case 5:
                        int removeId = ReadInt("Enter Company ID to remove: ");
                        _companyService.RemoveCompany(removeId, out string remMsg);
                        Console.WriteLine(remMsg);
                        break;
                    case 6:
                        int compId = ReadInt("Enter Company ID: ");
                        CompanyStatus newStatus = ReadCompanyStatus("Choose new status (1=Pending, 2=Approved, 3=Rejected): ");
                        _companyService.ChangeCompanyStatus(compId, newStatus, out string statusMsg);
                        Console.WriteLine(statusMsg);
                        break;
                    case 7:
                        _companyService.UndoLastStatusChange(out string undoMsg);
                        Console.WriteLine(undoMsg);
                        break;
                    case 0:
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private static void PrintCompaniesTable(List<Company> companies)
        {
            Console.WriteLine("\n+------+----------------------+----------------------+-----------------+------------+");
            Console.WriteLine($"| {"ID",-4} | {"Name",-20} | {"Industry",-20} | {"City",-15} | {"Status",-10} |");
            Console.WriteLine("+------+----------------------+----------------------+-----------------+------------+");

            if (companies.Count == 0)
            {
                Console.WriteLine($"| {"No companies found.",-79} |");
            }
            else
            {
                foreach (Company c in companies)
                {
                    Console.WriteLine($"| {c.Id,-4} | {Truncate(c.Name, 20),-20} | {Truncate(c.Industry, 20),-20} | {Truncate(c.City, 15),-15} | {c.Status,-10} |");
                }
            }
            Console.WriteLine("+------+----------------------+----------------------+-----------------+------------+");
        }

        // =====================================================================
        // 2. RECRUITER SUB-MENU
        // =====================================================================
        private static void ShowRecruiterMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- RECRUITERS MENU ---");
                Console.WriteLine("1. List All Recruiters");
                Console.WriteLine("2. Add Recruiter");
                Console.WriteLine("3. Update Recruiter");
                Console.WriteLine("4. Remove Recruiter");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Select an option: ");
                switch (choice)
                {
                    case 1:
                        PrintRecruitersTable(_companyService.GetAllRecruiters());
                        break;
                    case 2:
                        string name = ReadRequiredString("Recruiter Name: ");
                        string email = ReadRequiredString("Email: ");
                        int compId = ReadInt("Company ID: ");
                        Recruiter rec = new Recruiter { Name = name, Email = email, CompanyId = compId };
                        _companyService.AddRecruiter(rec, out string addMsg);
                        Console.WriteLine(addMsg);
                        break;
                    case 3:
                        int updId = ReadInt("Enter Recruiter ID to update: ");
                        string newName = ReadRequiredString("New Name: ");
                        string newEmail = ReadRequiredString("New Email: ");
                        int newCompId = ReadInt("New Company ID: ");
                        Recruiter updRec = new Recruiter { Id = updId, Name = newName, Email = newEmail, CompanyId = newCompId };
                        _companyService.UpdateRecruiter(updRec, out string updMsg);
                        Console.WriteLine(updMsg);
                        break;
                    case 4:
                        int remId = ReadInt("Enter Recruiter ID to remove: ");
                        _companyService.RemoveRecruiter(remId, out string remMsg);
                        Console.WriteLine(remMsg);
                        break;
                    case 0:
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private static void PrintRecruitersTable(List<Recruiter> recruiters)
        {
            Console.WriteLine("\n+------+----------------------+----------------------------+------------+");
            Console.WriteLine($"| {"ID",-4} | {"Name",-20} | {"Email",-26} | {"Company ID",-10} |");
            Console.WriteLine("+------+----------------------+----------------------------+------------+");

            if (recruiters.Count == 0)
            {
                Console.WriteLine($"| {"No recruiters found.",-68} |");
            }
            else
            {
                foreach (Recruiter r in recruiters)
                {
                    Console.WriteLine($"| {r.Id,-4} | {Truncate(r.Name, 20),-20} | {Truncate(r.Email, 26),-26} | {r.CompanyId,-10} |");
                }
            }
            Console.WriteLine("+------+----------------------+----------------------------+------------+");
        }

        // =====================================================================
        // 3. JOB SUB-MENU (E2, E5, Scenario 2, 3, 5)
        // =====================================================================
        private static void ShowJobMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- JOBS MENU ---");
                Console.WriteLine("1. List All Jobs");
                Console.WriteLine("2. Find Job by ID (via Dictionary<int, Job>)");
                Console.WriteLine("3. Add Job");
                Console.WriteLine("4. Update Job");
                Console.WriteLine("5. Remove Job");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Select an option: ");
                switch (choice)
                {
                    case 1:
                        PrintJobsTable(_jobService.GetAllJobs());
                        Console.WriteLine($"[Dictionary Sync Check] Jobs in Dictionary: {_jobService.GetDictionaryCount()}");
                        break;
                    case 2:
                        int findId = ReadInt("Enter Job ID to lookup in Dictionary: ");
                        Job? found = _jobService.GetJobByIdFromDictionary(findId);
                        if (found == null)
                        {
                            Console.WriteLine($"Job with ID {findId} was not found in Dictionary.");
                        }
                        else
                        {
                            PrintJobsTable(new List<Job> { found });
                        }
                        break;
                    case 3:
                        string title = ReadRequiredString("Job Title: ");
                        string desc = ReadRequiredString("Description: ");
                        decimal salary = ReadDecimal("Salary (above 0): ");
                        string loc = ReadRequiredString("Location: ");
                        int compId = ReadInt("Company ID: ");
                        int recId = ReadInt("Recruiter ID: ");
                        bool isOpen = ReadBool("Is the job open? (y/n): ");

                        Job newJob = new Job
                        {
                            Title = title,
                            Description = desc,
                            Salary = salary,
                            Location = loc,
                            CompanyId = compId,
                            RecruiterId = recId,
                            IsOpen = isOpen,
                            PostedDate = DateTime.Now
                        };
                        _jobService.AddJob(newJob, out string addMsg);
                        Console.WriteLine(addMsg);
                        break;
                    case 4:
                        int updId = ReadInt("Enter Job ID to update: ");
                        Job? existingJob = _jobService.GetJobByIdFromDictionary(updId);
                        if (existingJob == null)
                        {
                            Console.WriteLine($"Refused: Job with ID {updId} not found.");
                            break;
                        }
                        string newTitle = ReadRequiredString("New Title: ");
                        string newDesc = ReadRequiredString("New Description: ");
                        decimal newSalary = ReadDecimal("New Salary (above 0): ");
                        string newLoc = ReadRequiredString("New Location: ");
                        int newCompId = ReadInt("Company ID: ");
                        int newRecId = ReadInt("Recruiter ID: ");
                        bool newIsOpen = ReadBool("Is the job open? (y/n): ");

                        Job updatedJob = new Job
                        {
                            Id = updId,
                            Title = newTitle,
                            Description = newDesc,
                            Salary = newSalary,
                            Location = newLoc,
                            CompanyId = newCompId,
                            RecruiterId = newRecId,
                            IsOpen = newIsOpen,
                            PostedDate = existingJob.PostedDate
                        };
                        _jobService.UpdateJob(updatedJob, out string updMsg);
                        Console.WriteLine(updMsg);
                        break;
                    case 5:
                        int remId = ReadInt("Enter Job ID to remove: ");
                        _jobService.RemoveJob(remId, out string remMsg);
                        Console.WriteLine(remMsg);
                        break;
                    case 0:
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private static void PrintJobsTable(List<Job> jobs)
        {
            Console.WriteLine("\n+------+---------------------------+------------+--------------+--------+--------+------------+");
            Console.WriteLine($"| {"ID",-4} | {"Title",-25} | {"Salary",-10} | {"Location",-12} | {"Open",-6} | {"CompID",-6} | {"Posted",-10} |");
            Console.WriteLine("+------+---------------------------+------------+--------------+--------+--------+------------+");

            if (jobs.Count == 0)
            {
                Console.WriteLine($"| {"No jobs found.",-91} |");
            }
            else
            {
                foreach (Job j in jobs)
                {
                    string openStr = j.IsOpen ? "Yes" : "No";
                    Console.WriteLine($"| {j.Id,-4} | {Truncate(j.Title, 25),-25} | {j.Salary,10:F2} | {Truncate(j.Location, 12),-12} | {openStr,-6} | {j.CompanyId,-6} | {j.PostedDate:yyyy-MM-dd} |");
                }
            }
            Console.WriteLine("+------+---------------------------+------------+--------------+--------+--------+------------+");
        }

        // =====================================================================
        // 4. CANDIDATE SUB-MENU (E2, Scenario 4)
        // =====================================================================
        private static void ShowCandidateMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- CANDIDATES MENU ---");
                Console.WriteLine("1. List All Candidates");
                Console.WriteLine("2. Add Candidate");
                Console.WriteLine("3. Update Candidate");
                Console.WriteLine("4. Remove Candidate");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Select an option: ");
                switch (choice)
                {
                    case 1:
                        PrintCandidatesTable(_candidateService.GetAllCandidates());
                        break;
                    case 2:
                        string fullName = ReadRequiredString("Full Name: ");
                        string email = ReadRequiredString("Email (unique): ");
                        string phone = ReadRequiredString("Phone: ");
                        int exp = ReadInt("Years of Experience (0 or more): ");

                        Candidate cand = new Candidate { FullName = fullName, Email = email, Phone = phone, YearsOfExperience = exp };
                        _candidateService.AddCandidate(cand, out string addMsg);
                        Console.WriteLine(addMsg);
                        break;
                    case 3:
                        int updId = ReadInt("Enter Candidate ID to update: ");
                        string newName = ReadRequiredString("New Full Name: ");
                        string newEmail = ReadRequiredString("New Email: ");
                        string newPhone = ReadRequiredString("New Phone: ");
                        int newExp = ReadInt("New Years of Experience: ");

                        Candidate updCand = new Candidate { Id = updId, FullName = newName, Email = newEmail, Phone = newPhone, YearsOfExperience = newExp };
                        _candidateService.UpdateCandidate(updCand, out string updMsg);
                        Console.WriteLine(updMsg);
                        break;
                    case 4:
                        int remId = ReadInt("Enter Candidate ID to remove: ");
                        _candidateService.RemoveCandidate(remId, out string remMsg);
                        Console.WriteLine(remMsg);
                        break;
                    case 0:
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private static void PrintCandidatesTable(List<Candidate> candidates)
        {
            Console.WriteLine("\n+------+----------------------+----------------------------+----------------+--------+");
            Console.WriteLine($"| {"ID",-4} | {"Full Name",-20} | {"Email",-26} | {"Phone",-14} | {"Exp(Y)",-6} |");
            Console.WriteLine("+------+----------------------+----------------------------+----------------+--------+");

            if (candidates.Count == 0)
            {
                Console.WriteLine($"| {"No candidates found.",-80} |");
            }
            else
            {
                foreach (Candidate c in candidates)
                {
                    Console.WriteLine($"| {c.Id,-4} | {Truncate(c.FullName, 20),-20} | {Truncate(c.Email, 26),-26} | {Truncate(c.Phone, 14),-14} | {c.YearsOfExperience,-6} |");
                }
            }
            Console.WriteLine("+------+----------------------+----------------------------+----------------+--------+");
        }

        // =====================================================================
        // 5. APPLICATION SUB-MENU (E4, H1, Scenario 6)
        // =====================================================================
        private static void ShowApplicationMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- APPLICATIONS MENU ---");
                Console.WriteLine("1. List All Applications");
                Console.WriteLine("2. Apply to a Job (E4 & H1)");
                Console.WriteLine("3. Update Application Status");
                Console.WriteLine("4. Remove Application");
                Console.WriteLine("0. Back to Main Menu");

                int choice = ReadInt("Select an option: ");
                switch (choice)
                {
                    case 1:
                        PrintApplicationsTable(_applicationService.GetAllApplications());
                        break;
                    case 2:
                        int candId = ReadInt("Enter Candidate ID: ");
                        int jobId = ReadInt("Enter Job ID: ");
                        _applicationService.ApplyToJob(candId, jobId, out string applyMsg);
                        Console.WriteLine(applyMsg);
                        break;
                    case 3:
                        int appId = ReadInt("Enter Application ID: ");
                        ApplicationStatus status = ReadApplicationStatus("Select Status (1=Submitted, 2=UnderReview, 3=Interview, 4=Accepted, 5=Rejected): ");
                        _applicationService.UpdateApplicationStatus(appId, status, out string updMsg);
                        Console.WriteLine(updMsg);
                        break;
                    case 4:
                        int remId = ReadInt("Enter Application ID to remove: ");
                        _applicationService.RemoveApplication(remId, out string remMsg);
                        Console.WriteLine(remMsg);
                        break;
                    case 0:
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private static void PrintApplicationsTable(List<Application> apps)
        {
            Console.WriteLine("\n+------+--------+-------------+--------------+--------------+");
            Console.WriteLine($"| {"ID",-4} | {"Job ID",-6} | {"CandidateID",-11} | {"Applied Date",-12} | {"Status",-12} |");
            Console.WriteLine("+------+--------+-------------+--------------+--------------+");

            if (apps.Count == 0)
            {
                Console.WriteLine($"| {"No applications found.",-57} |");
            }
            else
            {
                foreach (Application a in apps)
                {
                    Console.WriteLine($"| {a.Id,-4} | {a.JobId,-6} | {a.CandidateId,-11} | {a.AppliedDate:yyyy-MM-dd,-12} | {a.Status,-12} |");
                }
            }
            Console.WriteLine("+------+--------+-------------+--------------+--------------+");
        }

        // =====================================================================
        // E6 & Scenario 5 / R1: Safe Input Helpers (Never Crashes on Invalid Input)
        // =====================================================================
        private static int ReadInt(string prompt)
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    string? input = Console.ReadLine();

                    if (int.TryParse(input, out int value))
                    {
                        return value;
                    }

                    // демонстрация int.Parse داخل try/catch كما ذُكر في E6
                    return int.Parse(input ?? string.Empty);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input! Please enter a valid whole number (e.g., 1, 2).");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Input error: {ex.Message}. Please try again.");
                }
            }
        }

        private static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    string? input = Console.ReadLine();
                    if (decimal.TryParse(input, out decimal value))
                    {
                        return value;
                    }
                    Console.WriteLine("Invalid decimal number! Please enter a valid amount (e.g., 15000).");
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid number format. Please try again.");
                }
            }
        }

        private static string ReadRequiredString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                Console.WriteLine("This field cannot be empty. Please enter a value.");
            }
        }

        private static bool ReadBool(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim().ToLower();
                if (input == "y" || input == "yes" || input == "true" || input == "1")
                    return true;
                if (input == "n" || input == "no" || input == "false" || input == "0")
                    return false;

                Console.WriteLine("Please enter 'y' for Yes or 'n' for No.");
            }
        }

        private static CompanyStatus ReadCompanyStatus(string prompt)
        {
            while (true)
            {
                int val = ReadInt(prompt);
                if (val == 1) return CompanyStatus.Pending;
                if (val == 2) return CompanyStatus.Approved;
                if (val == 3) return CompanyStatus.Rejected;
                Console.WriteLine("Please choose 1 (Pending), 2 (Approved), or 3 (Rejected).");
            }
        }

        private static ApplicationStatus ReadApplicationStatus(string prompt)
        {
            while (true)
            {
                int val = ReadInt(prompt);
                if (val == 1) return ApplicationStatus.Submitted;
                if (val == 2) return ApplicationStatus.UnderReview;
                if (val == 3) return ApplicationStatus.Interview;
                if (val == 4) return ApplicationStatus.Accepted;
                if (val == 5) return ApplicationStatus.Rejected;
                Console.WriteLine("Please choose a number between 1 and 5.");
            }
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 2) + "..";
        }
    }
}