using System;
using System.Collections.Generic;
using System.Text;

namespace PetAdoptionORM.Data.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<PetAdoptionORM.Model.Species> Species { get; }
        IRepository<PetAdoptionORM.Model.Breed> Breed { get; }
        IRepository<PetAdoptionORM.Model.Pet> Pet { get; }
        IRepository<PetAdoptionORM.Model.AdoptionApplication> AdoptionApplication { get; }
        IRepository<PetAdoptionORM.Model.Donation> Donation { get; }
        void Save();
    }
}
