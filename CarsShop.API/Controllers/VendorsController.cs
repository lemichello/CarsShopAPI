﻿using System.Linq;
using AutoMapper;
using CarsShop.API.Helpers;
using CarsShop.DAL.Entities;
using CarsShop.DAL.Repositories.Abstraction;
using CarsShop.DTO.VendorsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        public VendorsController(IRepository<Vendor> vendorsRepository, Profile profile)
        {
            _vendorsRepository = vendorsRepository;
            _dtoMapper         = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(profile)));
        }

        [HttpGet]
        public ActionResult GetVendors([FromQuery] int index, [FromQuery] int size)
        {
            var vendors = _vendorsRepository
                .GetAll()
                .AsNoTracking()
                .WithPagination(index, size)
                .Select(i => _dtoMapper.Map<VendorDto>(i));

            return Ok(vendors);
        }

        [HttpPost]
        public ActionResult AddVendor([FromBody] VendorDto vendor)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _vendorsRepository.Add(_dtoMapper.Map<Vendor>(vendor));

            return Ok();
        }

        private readonly IRepository<Vendor> _vendorsRepository;
        private readonly Mapper              _dtoMapper;
    }
}