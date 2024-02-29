using AutoBogus;
using BasketServiceExample.Models;
using BasketServiceExample.Repositories;
using BasketServiceExample.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BasketServiceExample.Tests.Services
{
    public class BasketServiceTests
    {
        private readonly Mock<IBasketRepository> _basketRepository;
        private readonly BasketService _basketService;
        public BasketServiceTests() 
        {
            _basketRepository = new Mock<IBasketRepository>();
            _basketService = new BasketService(_basketRepository.Object);
        }

        [Fact]
        public async Task GetBasketDetail_ReturnEmptyBasket()
        {
            //Arrange
            Random random = new();
            int userId = random.Next(0, 10);

            List<Product> expectedProductList = new AutoFaker<List<Product>>().Generate();

            Basket expectedBasket = new AutoFaker<Basket>()
                .RuleFor(fake => fake.UserId, fake => userId)
                .RuleFor(fake => fake.Products, fake => null)
                .Generate();

            //Act
            _basketRepository
                .Setup(r => r.GetByUserIdAsync(
                    It.IsAny<int>()
                    ))
                .Returns(Task.FromResult(expectedBasket));

            //Assert
            var result = await _basketService.GetBasketDetail(userId);

            Assert.NotNull(result);
            Assert.Null(result.Products);
        }

        [Fact]
        public async Task GetBasketDetail_ReturnNonEmptyBasket()
        {
            //Arrange
            Random random = new();
            int userId = random.Next(0, 10);

            List<Product> expectedProductList = new AutoFaker<List<Product>>().Generate();
            decimal expectedTotalPrice = 123.45m;

            Basket expectedBasket = new AutoFaker<Basket>()
                .RuleFor(fake => fake.UserId, fake => userId)
                .RuleFor(fake => fake.Products, fake => expectedProductList)
                .RuleFor(fake => fake.TotalPrice, fake => expectedTotalPrice)
                .Generate();

            //Act
            _basketRepository
                .Setup(r => r.GetByUserIdAsync(
                    It.IsAny<int>()
                    ))
                .Returns(Task.FromResult(expectedBasket));

            //Assert
            var result = await _basketService.GetBasketDetail(userId);

            Assert.NotNull(result);
            Assert.NotNull(result.Products);
            Assert.True(result.Products.Count > 0); // use case 2
            Assert.Equal(result.TotalPrice, expectedTotalPrice); // use case 2
        }

        [Fact]
        public async Task AddProduct_FirstProduct_ReturnTrue()
        {
            //Arrange
            Random random = new();
            int userId = random.Next(0, 10);

            Product product = new AutoFaker<Product>()
                .Generate();

            _basketRepository
                .Setup(r => r.InsertAsync(
                    It.IsAny<int>(),
                    It.IsAny<Product>()
                    ))
                .Returns(Task.FromResult(true));

            //Act
            var result = await _basketService.AddProduct(userId, product);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(123, 1, 1)]
        [InlineData(123, 1, 2)] // testing use case 1
        public async Task AddProduct_MultipleProducts_ReturnTrue(int userId, int companyId1, int companyId2)
        {
            //Arrange
            Product product1 = new AutoFaker<Product>()
                .RuleFor(fake => fake.CompanyId, fake => companyId1)
                .Generate();

            Product product2 = new AutoFaker<Product>()
                .RuleFor(fake => fake.CompanyId, fake => companyId2)
                .Generate();

            _basketRepository
                .Setup(r => r.InsertAsync(
                    It.IsAny<int>(),
                    It.IsAny<Product>()
                    ))
                .Returns(Task.FromResult(true));

            //Act
            var firstInsertionResult = await _basketService.AddProduct(userId, product1);
            var secondInsertionResult = await _basketService.AddProduct(userId, product2);

            //Assert
            Assert.True(firstInsertionResult);
            Assert.True(secondInsertionResult); 
        }

        [Fact]
        public async Task RemoveProduct_ReturnTrue()
        {
            //Arrange
            Random random = new();
            int userId = random.Next(0, 10);

            Product product = new AutoFaker<Product>()
                .Generate();

            _basketRepository
                .Setup(r => r.DeleteAsync(
                    It.IsAny<int>(),
                    It.IsAny<Product>()
                    ))
                .Returns(Task.FromResult(true));

            //Act
            var result = await _basketService.RemoveProduct(userId, product);

            //Assert
            Assert.True(result);
        }
    }
}
