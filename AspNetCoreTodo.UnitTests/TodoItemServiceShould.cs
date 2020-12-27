using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Threading.Tasks;
using Xunit;


namespace AspNetCoreTodo.UnitTests
{
    public class TodoItemServiceShould
    {
        [Fact]
        public async Task AddNewItemAsImcompleteWithDueDate()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);

                var fakeUser = new IdentityUser
                {
                    Id = "fake-000",
                    UserName = "fake@eample.com"
                };

                await service.AddItemAsync(new TodoItem
                {
                    Title = "Testing?"
                }, fakeUser);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var itemsInDatabase = await context
                    .Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);

                var item = await context.Items.FirstAsync();
                Assert.Equal("Testing?", item.Title);
                Assert.False(item.IsDone);

                // Item shold be due 3 days from now (give or take a second)
                //var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
                //Assert.True(difference < TimeSpan.FromSeconds(1));
            }
        }

        /* This test should: 
         * The MarkDoneAsync() method returns false if it's passed an ID that doesn't exist
         * The MarkDoneAsync() method returns true when it makes a valid item as complete
         */
        [Fact]
        public async Task MarkDoneItem()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_MarkDoneItem").Options;

            var fakeUser = new IdentityUser
            {
                Id = "fake-000",
                UserName = "fake@eample.com"
            };

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);

                await context.Items.AddAsync(new TodoItem
                {
                    Title = "Testing?",
                    UserId = fakeUser.Id
                });

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);

                var existUser = await service.MarkDoneAsync(Guid.NewGuid(), new IdentityUser("notExistedUser"));
                Assert.False(existUser);

                var item = await context.Items.FirstAsync();
                var isDoneItem = await service.MarkDoneAsync(item.Id, fakeUser);
                Assert.True(isDoneItem);
            }
        }

        // TODO : The GetIncompleteItemsAsync() method returns only the items owned by a particular user
    }
}
