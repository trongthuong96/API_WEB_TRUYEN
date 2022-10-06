using API.Data;
using API.DataAccess.Repository.IRepository;
using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAccess.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _db;
        public CityRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CityExists(int id)
        {
            return _db.Cities.Any(c => c.Id == id);
        }

        public bool CityExists(string name)
        {
            bool value = _db.Cities.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool CreateOrUpdateCity(City city)
        {
            _db.Cities.Add(city);
            return Save();
        }

        public bool DeleteCity(City city)
        {
            _db.Cities.Remove(city);
            return Save();
        }

        public ICollection<City> GetCities()
        {
            return _db.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId)
        {
            return _db.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
