using System;
using System.Collections.Generic;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services
{
    public class CandidateService
    {
        private readonly DataStore<Candidate> _candidateStore = new DataStore<Candidate>();

        private readonly HashSet<string> _emailSet= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        //add
        public bool AddCandidate(Candidate candidate,out string message) 
        {
            if (candidate == null || string.IsNullOrWhiteSpace(candidate.FullName)){

                message = "Refused: Candidate full name is required.";
                return false;
            }

            if (candidate.YearsOfExperience < 0)
            {
                message = "Refused: Years of experience must be 0 or more.";
                return false;
            }

            string cleanEmail=(candidate.Email ?? string.Empty).Trim();
            if (!CompanyService.IsValidEmail(cleanEmail)) 
            {
                message = "Refused: Invalid email format.";
                return false;
            }

            if (!_emailSet.Add(cleanEmail)) 
            {
                message = $"Refused: A candidate with email '{cleanEmail}' already exists (case-insensitive check).";
                return false;
            }


            candidate.Email = cleanEmail;
            if (candidate.Id <= 0) 
            {
                candidate.Id=_candidateStore.GetNextId();
            }



            if (!_candidateStore.Add(candidate)) {

                _emailSet.Remove(cleanEmail);
                message = $"Refused: Candidate with ID {candidate.Id} already exists.";
                return false;
            }

            message = $"Candidate '{candidate.FullName}' added successfully (ID: {candidate.Id}).";
            return true;

        }
        //update
        public bool UpdateCandidate(Candidate updated, out string message)
        {
            if (updated == null || string.IsNullOrWhiteSpace(updated.FullName))
            {
                message = "Refused: Candidate full name is required.";
                return false;
            }

            if (updated.YearsOfExperience < 0)
            {
                message = "Refused: Years of experience must be 0 or more.";
                return false;
            }

            Candidate? existing = _candidateStore.GetById(updated.Id);
            if (existing == null)
            {
                message = $"Refused: Candidate with ID {updated.Id} not found.";
                return false;
            }

            string newEmail = (updated.Email ?? string.Empty).Trim();
            if (!CompanyService.IsValidEmail(newEmail))
            {
                message = "Refused: Invalid email format.";
                return false;
            }

           bool isSameEmil=string.Equals(existing.Email, newEmail, StringComparison.OrdinalIgnoreCase);

            if (!isSameEmil)
            {
                if (!_emailSet.Add(newEmail))
                {
                    message = $"Refused: Email '{newEmail}' is already used by another candidate.";
                    return false;
                }
                _emailSet.Remove(existing.Email);
            }

            updated.Email = newEmail;
            _candidateStore.Update(updated);
            message = $"Candidate ID {updated.Id} updated successfully.";
            return true;
        }

        public bool RemoveCandidate(int id,out string message) {

            Candidate? existing = _candidateStore.GetById(id);

            if (existing == null) 
            {
                message = $"Refused: Candidate with ID {id} not found.";
                return false;
            }

            _candidateStore.Remove(id);
            _emailSet.Remove(existing.Email);

            message = $"Candidate ID {id} removed successfully.";
            return true;
        }
        public Candidate? GetCandidateById(int id)
        {
            return _candidateStore.GetById(id);
        }

        public List<Candidate> GetAllCandidates()
        {
            return _candidateStore.GetAll();
        }

    }
}