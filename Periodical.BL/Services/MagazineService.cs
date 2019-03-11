using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Periodical.BL.Services
{
    public class MagazineService : IMagazineService
    {
        UnitOfWork Database { get; set; }

        public MagazineService(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus Create(MagazineDTO magazineDTO, HostDTO author)
        {
            Magazine magazine = MagazineDTO.ToMagazine(magazineDTO, author.Id);
            if (magazine != null)
            {
                Database.MagazineRepository.Create(magazine);
                Host authorEdited = Database.HostRepository.GetById(author.Id);
                authorEdited.Magazines.Add(magazine);
                return new OperationStatus(true, "Create was succsesfull", "");
            }
            else
            {
                return new OperationStatus(false, "Something went wrong", "MagazineCreate");
            }
        }

        public OperationStatus Edit(MagazineDTO magazineDTO)
        {
            Magazine magazineEdited = MagazineDTO.ToMagazine(magazineDTO, magazineDTO.HostId);
            if (magazineEdited != null)
            {
                Database.MagazineRepository.Update(magazineEdited);

                return new OperationStatus(true, "Update was succsesfull", "");
            }
            else
            {
                return new OperationStatus(false, "Something went wrong", "MagazineUpdate");
            }
        }

        public MagazineDTO GetById(int? id)
        {
            Magazine magazine = Database.MagazineRepository.GetById(id);
            MagazineDTO magazineDTO = MagazineDTO.ToMagazineDTO(magazine);
            return magazineDTO;
        }

        public MagazineDTO Get(string name)
        {
            Magazine magazineCurrent = Database.MagazineRepository.GetOne(magazine => magazine.MagazineName == name);
            if (magazineCurrent != null)
            {
                MagazineDTO magazineDTO = MagazineDTO.ToMagazineDTO(magazineCurrent);
                return magazineDTO;
            }
            return null;
        }

        public IEnumerable<MagazineDTO> GetAll()
        {
            return Database.MagazineRepository.GetAll()
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine));
        }

        public IEnumerable<MagazineDTO> GetBy(string name)
        {
            return Database.MagazineRepository.Get(magazine => magazine.MagazineName == name)
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine));
        }

        public OperationStatus Delete(int? id)
        {
            Database.MagazineRepository.Delete(id);
            return new OperationStatus(true, "Delete was succsesfull", "");
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
