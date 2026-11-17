using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;

namespace JobApplicationTracker.Services
{
    public class CompanyService
    {
        private readonly DataStore<Company> _companyStore=new DataStore<Company>();
        private readonly DataStore<Recruiter> _recruiterStore = new DataStore<Recruiter>();

        private readonly Stack<(int CompanyId,CompanyStatus OldStatus)> _UndoStack=
            new Stack<(int CompanyId, CompanyStatus OldStatus)>();
        
        //--------------------------------------
        //--------company services--------------
        //--------------------------------------

        //Add
        public bool AddCompany(Company company,out string Message)
        {
            if (company==null || string.IsNullOrWhiteSpace(company.Name))
            {
                Message = "Company name is required.";
                return false;
            }
            if(company.Id<=0)
            {
                Message = "Company Id must be greater than zero.";
                return false;
            }
            if(!_companyStore.Add(company))
            {
                Message = $"Company with the same Id={company.Id} already exists.";
                return false;
            }



            Message = $"Company '{company.Name}' added successfully (ID: {company.Id}, Status: {company.Status}).";
            return true;
        }

        //Update
        public bool UpdateCompany(Company company, out string Message)
        {
            if (company == null || string.IsNullOrWhiteSpace(company.Name))
            {
                Message = "Company name is required.";
                return false;
            }
            if (company.Id <= 0)
            {
                company.Id = _companyStore.GetNextId();
            }
            var existingCompany = _companyStore.GetById(company.Id);
            if (existingCompany == null)
            {
                Message = $"Company with Id={company.Id} does not exist.";
                return false;
            }
            // Store the old status 
            company.Status=existingCompany.Status;
            _companyStore.Update(company);
            

            Message = $"Company '{company.Name}' updated successfully (ID: {company.Id}, Status: {company.Status}).";
            return true;
        }

        //remove 
        public bool RemoveCompany(int id,out string Message)
        {

            if (!_companyStore.Remove(id))
            {
                Message = $"Refused: Company with ID {id} not found.";
                return false;
            }

            Message = $"Company ID {id} removed successfully.";
            return true;
        }
        
        
        public Company? GetCompanyById(int id)
        {
            return _companyStore.GetById(id);
        }

        public List<Company> GetAllCompanies()
        {
            return _companyStore.GetAll();
        }

        // get by status

        public List<Company> GetCompaniesByStatus(CompanyStatus status)
        {

            List<Company> result= new List<Company>();
            List<Company> all= _companyStore.GetAll();

            foreach (Company company in all) {

                if (company.Status == status)
                {
                    result.Add(company);
                }
            
            }
            return result;
        }

        //change status
        public bool ChangeCompanyStatus(int companyId,CompanyStatus newStatus,out string Message)
        {
            Company? company=_companyStore.GetById(companyId);
            if (company == null)
            {
                Message = $"Refused: Company with ID {companyId} does not exist.";
                return false;
            }

            if (company.Status == newStatus)
            {
                Message = $"Company '{company.Name}' is already in '{newStatus}' status.";
                return false;
            }

            CompanyStatus oldStatus= company.Status;
            _UndoStack.Push((company.Id,oldStatus));

            company.Status = newStatus;
            _companyStore.Update(company);

            Message = $"Company '{company.Name}' (ID: {company.Id}) status changed from {oldStatus} to {newStatus}.";
            return true;

        }

        //undo last change
        public bool UndoLastStatusChange(out string Message)
        {
            if(_UndoStack.Count==0)
            {
                Message = "Nothing to undo: No previous company status changes found.";
                return false;
            }

            (int companyId,CompanyStatus oldStatus)=_UndoStack.Pop();
            Company? company = _companyStore.GetById(companyId);


            if (company == null) {
                Message = $"Cannot undo: Company ID {companyId} was removed.";
                return false;
            }
            CompanyStatus currentStatus= company.Status;
            company.Status = oldStatus;
            _companyStore.Update(company);

            Message = $"Undo successful: Company '{company.Name}' (ID: {company.Id}) restored from {currentStatus} back to {oldStatus}.";
            return true;
        }




        //--------------------------------------
        //--------Recruiter--------------
        //--------------------------------------

        //add
        public bool AddRecruiter(Recruiter recruiter,out string message)
        {

            if (recruiter == null || string.IsNullOrWhiteSpace(recruiter.Name))
            {
                message = "Refused: Recruiter name is required.";
                return false;
            }
            if (!IsValidEmail(recruiter.Email)) 
            {
                message = "Refused: Invalid email format for recruiter.";
                return false;

            }

            if (_companyStore.GetById(recruiter.CompanyId) == null)
            {
                message = $"Refused: Company with ID {recruiter.CompanyId} does not exist.";
                return false;

            }
            if (recruiter.Id <= 0)
            {
                recruiter.Id=_recruiterStore.GetNextId();
            }

            if (!_recruiterStore.Add(recruiter))
            {
                message = $"Refused: Recruiter with ID {recruiter.Id} already exists.";
                return false;

            }

            message = $"Recruiter '{recruiter.Name}' added successfully (ID: {recruiter.Id}).";
            return true;

        }

        //update
        public bool UpdateRecruiter(Recruiter recruiter,out string message)
        {
            if(recruiter==null || string.IsNullOrWhiteSpace(recruiter.Name))
            {
                message = "Refused: Recruiter name is required.";
                return false;

            }
            if (!IsValidEmail(recruiter.Email))
            {
                message = "Refused: Invalid email format for recruiter.";
                return false;

            }

            if (_companyStore.GetById(recruiter.CompanyId) == null)
            {
                message = $"Refused: Company with ID {recruiter.CompanyId} does not exist.";
                return false;

            }

            if (!_recruiterStore.Update(recruiter))
            {
                message = $"Refused: Recruiter ID {recruiter.Id} not found.";
                return false;

            }

            message = $"Recruiter ID {recruiter.Id} updated successfully.";
            return true;
        }

        //remove
        public bool RemoveRecruiter(int id,out string message)
        {
            if (!_recruiterStore.Remove(id)){

                message = $"Refused: Recruiter ID {id} not found.";
                return false;
            }

            message = $"Recruiter ID {id} removed successfully.";
            return true;
        }

        public Recruiter? GetRecruiterById(int id)
        {
            return _recruiterStore.GetById(id);

        }
        public List<Recruiter> GetAllRecruiters()
        {
            return _recruiterStore.GetAll();
        }


        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;

            }
            string trimmed= email.Trim();
            int atIndex=trimmed.IndexOf("@");
            int dotIndex=trimmed.IndexOf('.');

            return atIndex>0 && dotIndex>atIndex+1 && dotIndex<trimmed.Length;
        }

    }
}
