using System;
using System.Collections.Generic;
using System.Text;
using ALISS.AUTH.Library;
using Xunit;

namespace ALISS.AUTH.Library.Tests
{
    public class MenuServiceTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public void AUTH_Menu_GetData_Should_HaveData(string mnu_id)
        {
            // Arrange


            // Act
            //var actual = new MenuService().GetData(mnu_id);

            // Assert
            //Assert.NotNull(actual);
        }

        [Fact]
        public void AUTH_Menu_GetList_Should_HaveList()
        {
            // Arrange


            // Act
            //var actual = new MenuService().GetList();

            // Assert
            //Assert.NotNull(actual);
        }

    }
}
