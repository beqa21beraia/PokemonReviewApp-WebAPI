using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _dataContext.Add(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _dataContext.Reviewers.Where(r => r.Id == reviewerId)
                .Include(r => r.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _dataContext.Reviewers.OrderBy(r => r.Id).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _dataContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _dataContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _dataContext.Update(reviewer);
            return Save();
        }
    }
}
