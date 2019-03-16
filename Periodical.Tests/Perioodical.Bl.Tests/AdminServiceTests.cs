using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.Repository;
using Periodicals.DAL.UnitOfWork;
using System;
using System.Collections.Generic;

namespace Periodical.Tests
{
    [TestClass]
    public class AdminServiceTests
    {
        private IAdminService _adminService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IGenericRepository<Host>> _mockHostRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHostRepository = new Mock<IGenericRepository<Host>>();
            _adminService = new AdminService(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public void BlockUser_PassedIdThereIsNoHostEdited_ReturnsOperationStatusFailure()
        {
            const int userId = 1;
            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(() => null);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);

            var result = _adminService.BlockUser(userId);

            Assert.IsFalse(result.Succedeed);
        }

        [TestMethod]
        public void BlockUser_PassedIdThereIsHostEdited_ReturnsOperationStatusSuccses()
        {
            const int userId = 1;
            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(() => new Host());

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);

            var result = _adminService.BlockUser(userId);

            Assert.IsTrue(result.Succedeed);
        }

        [TestMethod]
        public void UnlockUser_PassedIdThereIsNoHostEdited_ReturnsOperationStatusFailure()
        {
            const int userId = 1;
            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(() => null);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);

            var result = _adminService.UnlockUser(userId);

            Assert.IsFalse(result.Succedeed);
        }

        [TestMethod]
        public void UnlockUser_PassedIdThereIsHostEdited_ReturnsOperationStatusSuccses()
        {
            const int userId = 1;
            _mockHostRepository.Setup(repository => repository.GetById(userId))
                .Returns(() => new Host());

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);

            var result = _adminService.UnlockUser(userId);

            Assert.IsTrue(result.Succedeed);
        }

        [TestMethod]
        public void GetBlockedUsers_ThereAreThreeUsers_RetursnThreeUsers()
        {
            _mockHostRepository.Setup(repository => repository.Get(It.IsAny<Func<Host, bool>>()))
                .Returns(() => new[] { new Host(), new Host(), new Host() });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            const int expectedCount = 3;

            var result = _adminService.GetBlockedUsers();

            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void GetUnlockedUsers_ThereAreOneUser_RetursnOneUser()
        {
            _mockHostRepository.Setup(repository => repository.Get(It.IsAny<Func<Host, bool>>()))
                .Returns(() => new List<Host> { new Host()});
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.HostRepository)
                .Returns(_mockHostRepository.Object);
            const int expectedCount = 1;

            var result = _adminService.GetUnlockedUsers();

            Assert.AreEqual(expectedCount, result.Count);
        }

    }
}
