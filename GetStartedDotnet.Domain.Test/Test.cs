using GetStartedDotnet.Domain.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace GetStartedDotnet.Domain.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCreate()
        {
            var _repository = new Mock<IRepository>();

            dynamic model = new ExpandoObject();

            _repository.Setup(r => r.CreateAsync(new UserModel()))
                                .Returns<dynamic>(model);

            var _service = new GetStartedDotnet.Domain.Services.Services(_repository.Object);

            var added = _service.CreateAsync(It.IsAny<UserModel>()).Result;
            Assert.True(added.Success, "Failed adding");
        }

        /*
        [Test]
        public async Task TestUpdate()
        {
            var _repository = new Mock<IRepository>();

            _repository.Setup(r => r.Update(It.IsAny<UserModel>(), It.IsAny<Guid>()))
                                 .Returns(It.IsAny<dynamic>());

            var _service = new GetStartedDotnet.Domain.Services.Services(_repository.Object);

            var added = await _service.Update(It.IsAny<UserModel>(), It.IsAny<Guid>());
            Assert.True(added.Success, "Failed updating");
        }

        [Test]
        public async Task TestDelete()
        {

            var _repository = new Mock<IRepository>();

            _repository.Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<string>()))
                                 .Returns(It.IsAny<dynamic>());

            var _service = new GetStartedDotnet.Domain.Services.Services(_repository.Object);

            var added = await _service.Delete(It.IsAny<Guid>(), It.IsAny<string>());
            Assert.True(added.Success, "Failed updating");
        }
    

        [Test]
        public async Task TestGet()
        {
            var _repository = new Mock<IRepository>();

            _repository.Setup(r => r.Get(It.IsAny<Guid>()))
                                 .Returns(It.IsAny<dynamic>());

            var _service = new GetStartedDotnet.Domain.Services.Services(_repository.Object);

            var added = await _service.Get(It.IsAny<Guid>());
            Assert.True(added.Success, "Failed getting one");
        }

        [Test]
        public async Task TestGetAll()
        {
            var _repository = new Mock<IRepository>();

            _repository.Setup(r => r.GetAllAsync())
                                 .Returns(It.IsAny<dynamic>());

            var _service = new GetStartedDotnet.Domain.Services.Services(_repository.Object);

            var added = await _service.GetAllAsync();
            Assert.True(added.Success, "Failed getting all");
        }
     */
    }
}