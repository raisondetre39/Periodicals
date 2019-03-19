using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Repository;
using Periodicals.DAL.UnitOfWork;

namespace Periodical.Tests.Perioodical.Bl.Tests
{
    [TestClass]
    public class MagazineServiceTest
    {
        private IMagazineService _magazineService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IGenericRepository<Host>> _mockHostRepository;
        private Mock<IGenericRepository<Magazine>> _mockMagazineRepository;
        private Mock<IGenericRepository<Tag>> _mockTagRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMagazineRepository = new Mock<IGenericRepository<Magazine>>();
            _mockHostRepository = new Mock<IGenericRepository<Host>>();
            _magazineService = new MagazineService(_mockUnitOfWork.Object);
            _mockTagRepository = new Mock<IGenericRepository<Tag>>();
        }

        [TestMethod]
        public void GetBy_ThereIsOne_ReturnsOneMagazine()
        {
            const string magazineName = "Times";

            _mockMagazineRepository.Setup(repository => repository.Get(It.IsAny<Func<Magazine, bool>>()))
                .Returns(() => new[] { new Magazine() { MagazineName = magazineName } });

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedMagazineCount = 1;

            var result = _magazineService.GetBy(magazineName).ToList();

            Assert.AreEqual(expectedMagazineCount, result.Count);
        }

        [TestMethod]
        public void GetBy_ThereIsNoMagazine_ReturnsNull()
        {
            const string magazineName = "Times";

            _mockMagazineRepository.Setup(repository => repository.Get(It.IsAny<Func<Magazine, bool>>()))
                .Returns(() => new List<Magazine>());

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedMagazineCount = 0;

            var result = _magazineService.GetBy(magazineName).ToList();

            Assert.AreEqual(expectedMagazineCount, result.Count);
        }

        [TestMethod]
        public void GetAll_ThereIsThreeMagazines_ReturnsThreeMagazines()
        {
            _mockMagazineRepository.Setup(repository => repository.GetAll())
                .Returns(() => new[] { new Magazine(), new Magazine(), new Magazine() });

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedMagazineCount = 3;

            var result = _magazineService.GetAll().ToList();

            Assert.AreEqual(expectedMagazineCount, result.Count);
        }

        [TestMethod]
        public void GetById_ThereIsOneMagazine_RetursnOneMagazine()
        {
            const int magazineId = 1;

            _mockMagazineRepository.Setup((repository => repository.GetById(magazineId)))
                .Returns(new Magazine() { MagazineId = magazineId } );
            
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedUserId = 1;

            var result = _magazineService.GetById(magazineId);

            Assert.AreEqual(expectedUserId, result.Id);
        }


        [TestMethod]
        public void Edit_ThereIsNoMagazineEdited_ReturnsOperationStatusFailure()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);

            var result = _magazineService.Edit(null, 0, new int[] { 1 });

            Assert.IsFalse(result.Succedeed);
        }

        [TestMethod]
        public void Edit_ThereIsMagazineEdited_ReturnsOperationStatusSuccses()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);

            var result = _magazineService.Edit(new MagazineDTO(), 1, new int[] { 1 });

            Assert.IsTrue(result.Succedeed);
        }

        [TestMethod]
        public void Create_ThereHasAlredyBeenSuchMagazineInDataBase_ReturnsOperationStatusFailure()
        {
            _mockMagazineRepository.Setup(repository => repository.GetOne(It.IsAny<Func<Magazine, bool>>()))
                .Returns(() => new Magazine());

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);

            var result = _magazineService.Create(new MagazineDTO(), 1, new int[] { 1 });

            Assert.IsFalse(result.Succedeed);
        }

        [TestMethod]
        public void Create_ThereIsNoSuchMagazineInDataBase_ReturnsOperationStatusSuccses()
        {
            const int userId = 1;
            const int tagId = 1;
            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(new Host() { Id = userId });
            _mockMagazineRepository.Setup(repository => repository.GetOne(It.IsAny<Func<Magazine, bool>>()))
                .Returns(() => null);
            _mockTagRepository.Setup(repository => repository.GetById(tagId))
                .Returns(new Tag() { TagId = tagId });

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.TagRepository)
                .Returns(_mockTagRepository.Object);


            var result = _magazineService.Create(new MagazineDTO(), userId, new int[] { 1 });

            Assert.IsTrue(result.Succedeed);
        }
    }
}
