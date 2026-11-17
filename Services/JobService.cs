using System;
using System.Collections.Generic;
using System.Text.Json;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services
{
    public class JobService
    { 
        private readonly DataStore<Job> _jobStore=new DataStore<Job>();
        private readonly Dictionary<int,Job> _jobDictionary=new Dictionary<int,Job>();

        private readonly CompanyService _companyService;

        public JobService(CompanyService companyService)
        {
            _companyService = companyService;
        }


        public bool AddJob(Job job, out string message)
        {
            if (job == null || string.IsNullOrWhiteSpace(job.Title))
            {
                message = "Refused: Job title is required.";
                return false;
            }

            if (job.Salary <= 0)
            {
                message = "Refused: Salary must be greater than 0.";
                return false;
            }
            
            // company
            Company? company = _companyService.GetCompanyById(job.CompanyId);
            if (company == null)
            {
                message = $"Refused: Company with ID {job.CompanyId} does not exist.";
                return false;

            }

            //scenario 3
            if (company.Status != CompanyStatus.Approved) 
            {
                message = $"Refused: Cannot post a job for company '{company.Name}' because its status is '{company.Status}' (must be Approved).";
                return false;

            }

            //recruiter

            Recruiter? recruiter=_companyService.GetRecruiterById(job.RecruiterId);
            if(recruiter == null) 
            {
                message = $"Refused: Recruiter '{recruiter.Name}' does not belong to company '{company.Name}'.";
                return false;
            }

            if (job.Id <= 0)
            {
                job.Id= _jobStore.GetNextId();
            }

            if (!_jobStore.Add(job))
            {
                message = $"Refused: Job with ID {job.Id} already exists.";
                return false;
            }
            //e5

            _jobDictionary[job.Id] = job;


            message = $"Job '{job.Title}' added successfully (ID: {job.Id}) and indexed in Dictionary.";
            return true;
        }


        public bool UpdateJob(Job job,out string message)
        {

            if (job == null || string.IsNullOrWhiteSpace(job.Title))
            {
                message = "Refused: Job title is required.";
                return false;
            }

            if (job.Salary <= 0)
            {
                message = "Refused: Salary must be greater than 0.";
                return false;
            }

            //company
            Company? company = _companyService.GetCompanyById(job.CompanyId);
            if (company == null)
            {
                message = $"Refused: Company with ID {job.CompanyId} does not exist.";
                return false;

            }

            //scenario 3
            if (company.Status != CompanyStatus.Approved)
            {
                message = $"Refused: Cannot post a job for company '{company.Name}' because its status is '{company.Status}' (must be Approved).";
                return false;

            }

            if (!_jobStore.Update(job))
            {
                message = $"Refused: Job with ID {job.Id} not found.";
                return false;
            }

            _jobDictionary[job.Id] = job;

            message = $"Job ID {job.Id} updated in both DataStore and Dictionary.";
            return true;

        }

        public bool RemoveJob(int id,out string message)
        {
            bool removedFromStore=_jobStore.Remove(id);
            bool removeFromDict=_jobDictionary.Remove(id);

            if(!removedFromStore && !removeFromDict)
            {
                message = $"Refused: Job with ID {id} not found.";
                return false;
            }
            message = $"Job ID {id} removed from both DataStore and Dictionary.";
            return true;

        }

        public Job? GetJobByIdFromDictionary(int id)
        {
            if(_jobDictionary.TryGetValue(id,out Job? job))
            {
                return job;
            }
            return null;
        }
        public List<Job> GetAllJobs()
        {
            return _jobStore.GetAll();
        }

        public int GetDictionaryCount()
        {
            return _jobDictionary.Count;
        }

    }
}