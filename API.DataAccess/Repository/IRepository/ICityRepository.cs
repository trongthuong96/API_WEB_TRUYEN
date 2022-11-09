using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DataAccess.Repository.IRepository
{
    public interface ICityRepository
    {
        ICollection<City> GetCities();
        City GetCity(int id);
        bool CityExists(int id);
        bool CityExists(string name);
        bool CreateOrUpdateCity(City city);
        bool DeleteCity(City city);
        bool Save();
    }
}
