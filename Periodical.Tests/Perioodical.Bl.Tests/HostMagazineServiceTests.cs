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
    public class HostMagazineServiceTests
    {
        private IHostMagazineService _hostMagazineService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IGenericRepository<Host>> _mockHostRepository;
        private Mock<IGenericRepository<Magazine>> _mockMagazineRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMagazineRepository = new Mock<IGenericRepository<Magazine>>();
            _mockHostRepository = new Mock<IGenericRepository<Host>>();
            _hostMagazineService = new HostMagazineService(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public void GetUserMagazines_UserHasTwoMagazines_ReturnsTwoMagazines()
        {
            const int userId = 1;
            const int firstMagazineId = 1;
            const int secondMagazineId = 2;

            _mockMagazineRepository.Setup((repository => repository.GetById(firstMagazineId)))
                .Returns(new Magazine());
            _mockMagazineRepository.Setup((repository => repository.GetById(secondMagazineId)))
                .Returns(new Magazine());
            _mockMagazineRepository.Setup(repository => repository.Get(It.IsAny<Func<Magazine, bool>>()))
                .Returns(new[] { new Magazine() { MagazineId = firstMagazineId }, new Magazine() { MagazineId = secondMagazineId } });

            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(new Host() { Id = userId, Magazines = new List<Magazine>() { new Magazine() { MagazineId = firstMagazineId }, new Magazine() { MagazineId = secondMagazineId } } });

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedMagazinesCount = 2;

            var result = _hostMagazineService.GetUserMagazines(userId).ToList();

            Assert.AreEqual(expectedMagazinesCount, result.Count);
        }

        [TestMethod]
        public void GetUserMagazines_UserHasNoMagazines_ReturnsNull()
        {
            const int userId = 1;
            const int firstMagazineId = 1;
            const int secondMagazineId = 2;

            _mockMagazineRepository.Setup((repository => repository.GetById(firstMagazineId)))
                .Returns(new Magazine());
            _mockMagazineRepository.Setup((repository => repository.GetById(secondMagazineId)))
                .Returns(new Magazine());
            _mockMagazineRepository.Setup(repository => repository.Get(It.IsAny<Func<Magazine, bool>>()))
                .Returns(() => new List<Magazine>());

            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(new Host() { Id = userId });

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedMagazinesCount = 0;

            var result = _hostMagazineService.GetUserMagazines(userId).ToList();

            Assert.AreEqual(expectedMagazinesCount, result.Count);
        }

        [TestMethod]
        public void Delete_ThereIsNoSuchUser_ReturnsOperationStatusFailure()
        {
            const int userId = 1;
            const int magazineId = 1;

            _mockMagazineRepository.Setup((repository => repository.GetById(magazineId)))
                .Returns(new Magazine());

            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(() => null);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);

            var result = _hostMagazineService.Delete(new HostDTO() { Id = userId}, magazineId);

            Assert.IsFalse(result.Succedeed);
        }

        [TestMethod]
        public void Delete_ThereIsUser_ReturnsOperationStatusSuccses()
        {
            const int userId = 1;
            const int magazineId = 1;

            _mockMagazineRepository.Setup((repository => repository.GetById(magazineId)))
                .Returns(new Magazine());

            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(new Host());

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);

            var result = _hostMagazineService.Delete(new HostDTO() { Id = userId }, magazineId);

            Assert.IsTrue(result.Succedeed);
        }

        [TestMethod]
        public void AddMagazine_ThereAreUserAndMagazineInDataBase_ReturnsOperationStatusSuccses()
        {
            const int userId = 1;
            const int magazineId = 1;

            _mockMagazineRepository.Setup((repository => repository.GetById(magazineId)))
                .Returns(new Magazine());

            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(new Host());

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);

            var result = _hostMagazineService.AddMagazine(new HostDTO() { Id = userId }, magazineId);

            Assert.IsTrue(result.Succedeed);
        }

        [TestMethod]
        public void AddMagazine_ThereIsNoMagazineInDataBase_ReturnsOperationStatusFaiure()
        {
            const int userId = 1;
            const int magazineId = 1;

            _mockMagazineRepository.Setup((repository => repository.GetById(magazineId)))
                .Returns(() => null);

            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(new Host());

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);

            var result = _hostMagazineService.AddMagazine(new HostDTO() { Id = userId }, magazineId);

            Assert.IsFalse(result.Succedeed);
        }
    }
}
