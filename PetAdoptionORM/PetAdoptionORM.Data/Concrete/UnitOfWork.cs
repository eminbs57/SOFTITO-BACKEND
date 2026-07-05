using PetAdoptionORM.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetAdoptionORM.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Species = new Repository<PetAdoptionORM.Model.Species>(_db);
            Breed = new Repository<PetAdoptionORM.Model.Breed>(_db);
            Pet = new Repository<PetAdoptionORM.Model.Pet>(_db);
            AdoptionApplication = new Repository<PetAdoptionORM.Model.AdoptionApplication>(_db);
            Donation = new Repository<PetAdoptionORM.Model.Donation>(_db);
        }

        public IRepository<PetAdoptionORM.Model.Species> Species { get; private set; }
        public IRepository<PetAdoptionORM.Model.Breed> Breed { get; private set; }
        public IRepository<PetAdoptionORM.Model.Pet> Pet { get; private set; }
        public IRepository<PetAdoptionORM.Model.AdoptionApplication> AdoptionApplication { get; private set; }
        public IRepository<PetAdoptionORM.Model.Donation> Donation { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
