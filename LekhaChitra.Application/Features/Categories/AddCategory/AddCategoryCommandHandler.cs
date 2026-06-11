using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Categories.AddCategory
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;

        public AddCategoryCommandHandler( IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<ServiceResponse> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await _uow.AsyncRepositories<Category>()
                                           .GetQueryable()
                                           .Where(x => x.Name == request.Name)
                                           .FirstOrDefaultAsync(cancellationToken);
            if (categoryExists != null)
            {
                return ServiceResponse.Conflict($"Category with name '{request.Name}' already exists.");
            }
            else {
                var category = new Category()
                {
                    Name = request.Name
                };
                var result = await _uow.AsyncRepositories<Category>().AddAsync(category);
                if (result != null)
                {
                    await _uow.Save();
                    return ServiceResponse.Success($"Category '{request.Name}' added successfully.");
                }
                else
                {
                    return ServiceResponse.InternalServerError($"Failed to add category '{request.Name}'.");
                }
            }
        }
    }
}
