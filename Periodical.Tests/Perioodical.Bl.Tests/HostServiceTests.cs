using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Repository;
using Periodicals.DAL.UnitOfWork;

namespace Periodical.Tests.Perioodical.Bl.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private IHostService _hostService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IGenericRepository<Host>> _mockHostRepository;
        private Mock<IGenericRepository<Magazine>> _mockMagazineRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMagazineRepository = new Mock<IGenericRepository<Magazine>>();
            _mockHostRepository = new Mock<IGenericRepository<Host>>();
            _hostService = new HostService(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public void Create_ThereHasAlredyBeenSuchUserInDataBase_ReturnsOperationStatusFailure()
        {
            _mockHostRepository.Setup(repository => repository.GetOne(It.IsAny<Func<Host, bool>>()))
                .Returns(() => new Host());

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);

            var result = _hostService.Create(new HostDTO(), "admin");

            Assert.IsFalse(result.Succedeed);
        }

        [TestMethod]
        public void Create_ThereIsNoSuchUserInDataBase_ReturnsOperationStatusSuccses()
        {
            _mockHostRepository.Setup(repository => repository.GetOne(It.IsAny<Func<Host, bool>>()))
                .Returns(() => null);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);

            var result = _hostService.Create(new HostDTO(), "admin");

            Assert.IsTrue(result.Succedeed);
        }

        [TestMethod]
        public void GetAllUsers_ThereAreFiveUsers_RetursnFiveUsers()
        {
            _mockHostRepository.Setup(repository => repository.GetAll())
                .Returns(new[] { new Host(), new Host(), new Host(), new Host(), new Host() });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            const int expectedCount = 5;

            var result = _hostService.GetAll();

            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void GetById_ThereIsOneUser_RetursnOneUser()
        {
            const int userId = 1;
            const int magazineId = 1;

            _mockMagazineRepository.Setup((repository => repository.GetById(magazineId)))
                .Returns(new Magazine());
            _mockMagazineRepository.Setup(repository => repository.Get(It.IsAny<Func<Magazine, bool>>()))
                .Returns(new[] { new Magazine() });

            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(new Host() { Id = userId });

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedUserId = 1;

            var result = _hostService.GetById(userId);

            Assert.AreEqual(expectedUserId, result.Id);
        }


    }
}
