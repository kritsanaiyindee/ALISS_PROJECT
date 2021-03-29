using ALISS.Master.Library.DataAccess;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ALISS.Master.Library.Test
{
    public class MasterDataService
    {
        private readonly MasterContext _db;
        private readonly IMapper _mapper;

        public MasterDataService(MasterContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [Fact]
        public void GetAreaHealth()
        {
            // Arrange


            // Act
            //var actual = new MasterDataService(_db, _mapper).GetAreaHealth();

            // Assert
            //Assert.NotNull(actual);
        }
    }
}
