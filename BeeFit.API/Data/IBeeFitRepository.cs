using System.Collections.Generic;
using System.Threading.Tasks;
using BeeFit.API.Helpers;
using BeeFit.API.Models;

namespace BeeFit.API.Data
{
    public interface IBeeFitRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(int userId);
         Task<Like> GetLike(int userId, int recipientId);
         Task<Message> GetMessage(int id);
         Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
         Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
         Task<Workout> GetWorkout(int userId);
         Task<MuscleGroup> GetMuscleGroup(int id);
         Task<MuscleGroup> GetMuscleGroup(string name);
    }
}