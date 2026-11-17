using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services
{
    public class ApplicationService
    {
        private readonly DataStore<Application> _applicationStore = new DataStore<Application>();

        private readonly HashSet<(int JopId, int CandidateId)> _appliedPairs = new HashSet<(int JopId, int CandidateId)>();

        private readonly JobService _jobService;
        private readonly CandidateService _candidateService;
        private readonly CompanyService _companyService;

        public ApplicationService(JobService jobService, CandidateService candidateService, CompanyService companyService)
        {
            _jobService = jobService;
            _candidateService = candidateService;
            _companyService = companyService;
        }

        public bool ApplyToJob(int candidateId,int jobId,out string message)
        {

            Candidate? candidate=_candidateService.GetCandidateById(candidateId);
            if (candidate == null)
            {
                message = $"Refused: Candidate with ID {candidateId} does not exist.";
                return false;
            }

            Job? job =_jobService.GetJobByIdFromDictionary(jobId);
            if (job == null)
            {
                message = $"Refused: Job with ID {jobId} does not exist.";
                return false;
            }

            Company? company = _companyService.GetCompanyById(job.CompanyId);
            if(company == null || company.Status != CompanyStatus.Approved)
            {
                string statusText = company != null ? company.Status.ToString() : "UnKnown";
                message = $"Refused: Cannot apply because the job's company is '{statusText}' (must be Approved).";
                return false;
            }

            if (!job.IsOpen)
            {
                message = $"Refused: Job '{job.Title}' (ID: {job.Id}) is closed and no longer accepting applications.";
                return false;
            }

            if (!_appliedPairs.Add((jobId, candidateId)))
            {
                message = $"Refused: Candidate '{candidate.FullName}' has already applied to job '{job.Title}'. Duplicate applications are blocked.";
                return false;

            }

            Application app = new Application
            {
                Id = _applicationStore.GetNextId(),
                JobId = jobId,
                CandidateId = candidateId,
                AppliedDate = DateTime.Now,
                Status = ApplicationStatus.Submitted
            };

            _applicationStore.Add(app);

            message = $"Application submitted successfully! (Application ID: {app.Id}, Candidate: '{candidate.FullName}', Job: '{job.Title}').";
            return true;
        }

        public bool UpdateApplicationStatus(int applicationId,ApplicationStatus newStatus,out string message)
        {
            Application? app = _applicationStore.GetById(applicationId);
            if (app == null)
            {
                message = $"Refused: Application with ID {applicationId} not found.";
                return false;

            }
            app.Status = newStatus;
            _applicationStore.Update(app);
            message = $"Application ID {applicationId} status updated to {newStatus}.";
            return true;
        }


        public bool RemoveApplication(int applicationId, out string message)
        {
            Application? existing = _applicationStore.GetById(applicationId);

            if (existing == null)
            {
                message = $"Refused: Application with ID {applicationId} not found.";
                return false;

            }

            _applicationStore.Remove(applicationId);
            _appliedPairs.Remove((existing.JobId, existing.CandidateId));

            message = $"Application ID {applicationId} removed and duplicate tracker synced.";
            return true;
        }

        public Application? GetApplicationById(int id)
        {
            return _applicationStore.GetById(id);
        }

        public List<Application> GetAllApplications()
        {
            return _applicationStore.GetAll();
        }


    }
}