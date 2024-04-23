using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Students.Model;

namespace Students.Repository
{
    public class StudentsRepository : IStudentsRepository
    {

        private readonly IMongoCollection<Studnt> _studnt;

        public StudentsRepository(IMongoClient mongoClient)
        {
            var db = mongoClient.GetDatabase("StudentDb");
            var collection = db.GetCollection<Studnt>(nameof(Studnt));
            _studnt = collection;
        }

        public async Task<ObjectId> CreateStudent(Studnt student)
        {
           await _studnt.InsertOneAsync(student);
           return student.Id;
        }

       
        public async Task<Studnt> GetStudentsByMobileNo(string MobileNo)
        {
            var filter = Builders<Studnt>.Filter.Eq(e => e.Mobile, MobileNo);
            Studnt studentInfo = await _studnt.Find(filter).FirstOrDefaultAsync();
            return studentInfo;
        }

        public async Task<bool> UpdateStudentByMobileNo(string MobileNo, Studnt student)
        {
            var filter = Builders<Studnt>.Filter.Eq(e => e.Mobile, MobileNo);
            var update = Builders<Studnt>.Update
                .Set(e => e.FirstName, student.FirstName)
                .Set(e => e.LastName, student.LastName)
                .Set(e => e.Gender, student.Gender)
                .Set(e => e.Mobile, student.Mobile)
                .Set(e => e.Age, student.Age)
                .Set(e => e.Caste, student.Caste)
                .Set(e => e.Religion, student.Religion)
                .Set(e => e.Address, student.Address)
                .Set(e => e.FatherName, student.FatherName)
                .Set(e=> e.MotherName, student.MotherName)
                .Set(e => e.FathersOccupation, student.FathersOccupation)
                .Set(e => e.MothersOccupation, student.MothersOccupation)
                .Set(e => e.standard, student.standard)
                .Set(e=> e.EmailId, student.EmailId);
                 
            var result = await _studnt.UpdateOneAsync(filter, update);
            return result.ModifiedCount == 1;
        }

        public async Task<bool> DeleteStudentsByMobileNo(string MobileNo)
        {
            var filter = Builders<Studnt>.Filter.Eq(e => e.Mobile, MobileNo);
            var result = await _studnt.DeleteOneAsync(filter);
            return result.DeletedCount == 1;
        }

        public async Task<List<Studnt>> GetAllStudents()
        {
            return await _studnt.Find(e => true).ToListAsync();
        }

        
    }
}
