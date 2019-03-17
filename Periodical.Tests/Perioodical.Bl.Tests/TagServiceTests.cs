using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.Repository;
using Periodicals.DAL.UnitOfWork;

namespace Periodical.Tests.Perioodical.Bl.Tests
{
    [TestClass]
    public class TagServiceTests
    {
        private ITagService _tagService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IGenericRepository<Tag>> _mockTagRepository;
        private Mock<IGenericRepository<Magazine>> _mockMagazineRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMagazineRepository = new Mock<IGenericRepository<Magazine>>();
            _mockTagRepository = new Mock<IGenericRepository<Tag>>();
            _tagService = new TagService(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public void GetAll_ThereAreThreeTags_RetursnThreeTags()
        {
            _mockTagRepository.Setup(repository => repository.GetAll())
                .Returns(() => new[] { new Tag(), new Tag(), new Tag() });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.TagRepository)
                .Returns(_mockTagRepository.Object);
            const int expectedCount = 3;

            var result = _tagService.GetAll();

            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void Get_ThereIsOneTag_ReturnsOneTag()
        {
            const string tagName = "fashion";
            _mockTagRepository.Setup(repository => repository.GetOne(It.IsAny<Func<Tag, bool>>()))
                .Returns(() => new Tag() { TagName = tagName });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.TagRepository)
                .Returns(_mockTagRepository.Object);
            const string expectedTagName = "fashion";

            var result = _tagService.Get("fashion");

            Assert.AreEqual(expectedTagName, result.TagName);
        }

        [TestMethod]
        public void Get_ThereIsNoTag_ReturnsNull()
        {
            _mockTagRepository.Setup(repository => repository.GetOne(It.IsAny<Func<Tag, bool>>()))
                .Returns(() => null);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.TagRepository)
                .Returns(_mockTagRepository.Object);

            var result = _tagService.Get("fashion");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetById_ThereIsOneTag_ReturnsOneTag()
        {
            const int tagId = 1;
            _mockTagRepository.Setup(repository => repository.GetById(tagId))
                .Returns(() => new Tag() { TagId = tagId });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.TagRepository)
                .Returns(_mockTagRepository.Object);
            const int expectedTagName = 1;

            var result = _tagService.GetById(1);

            Assert.AreEqual(expectedTagName, result.TagId);
        }

        [TestMethod]
        public void GetById_ThereIsNoTag_ReturnsNull()
        {
            const int tagId = 1;
            _mockTagRepository.Setup(repository => repository.GetById(tagId))
                .Returns(() => null);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.TagRepository)
                .Returns(_mockTagRepository.Object);

            var result = _tagService.GetById(1);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetByTagName_ThereIsOne_ReturnsOneTag()
        {
            const string tagName = "fashion";

            _mockMagazineRepository.Setup(repository => repository.Get(It.IsAny<Func<Magazine, bool>>()))
                .Returns(new[] { new Magazine() });

            _mockTagRepository.Setup(repository => repository.GetOne(It.IsAny<Func<Tag, bool>>()))
                .Returns(new Tag() { TagName = tagName });

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.TagRepository)
                .Returns(_mockTagRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedMagazineCount = 1;

            var result = _tagService.GetByTagName("fashion");

            Assert.AreEqual(expectedMagazineCount, result.Count);
        }

        [TestMethod]
        public void GetByTagName_ThereIsNoMagazines_ReturnsNull()
        {
            const string tagName = "fashion";

            _mockMagazineRepository.Setup(repository => repository.Get(It.IsAny<Func<Magazine, bool>>()))
                .Returns(() => new List<Magazine>());
            _mockTagRepository.Setup(repository => repository.GetOne(It.IsAny<Func<Tag, bool>>()))
                .Returns(new Tag() { TagName = tagName });

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.TagRepository)
                .Returns(_mockTagRepository.Object);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MagazineRepository)
                .Returns(_mockMagazineRepository.Object);
            const int expectedMagazineCount = 0;

            var result = _tagService.GetByTagName("fashion");

            Assert.AreEqual(expectedMagazineCount, result.Count);
        }
    }
}
