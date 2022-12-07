using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyEshop.Data;
using MyEshop.Models;
using MyEshop.Data.Repositories;

namespace MyEshop.Components
{
    public class ProductGroupComponent : ViewComponent
    {
        private IGroupRepository _groupRepository;
        public ProductGroupComponent(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            

            return View("/Views/Components/ProductGroupComponent.cshtml", _groupRepository.GetGroupForShow());
        }
    }
}
