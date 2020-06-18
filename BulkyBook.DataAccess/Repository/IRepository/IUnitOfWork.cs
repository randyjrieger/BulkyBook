using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get;  }
        IProductRepository Product { get; }
        ISP_Call SP_Call { get; }
        ICompanyRepository Company { get;  }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
    }
}
