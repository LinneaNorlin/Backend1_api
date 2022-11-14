﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRequest req)
        {
            try
            {
                if (!await _context.Users.AnyAsync(x => x.Email == req.Email))
                {
                    var userEntity = new UserEntity { FirstName = req.FirstName, LastName = req.LastName, Email = req.Email, PhoneNumber = req.PhoneNumber };
                    _context.Add(userEntity);
                    await _context.SaveChangesAsync();

                    return new OkObjectResult(userEntity);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = new List<UserResponse>();
                foreach (var user in await _context.Users.ToListAsync())
                    users.Add(new UserResponse { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, PhoneNumber = user.PhoneNumber });

                return new OkObjectResult(users);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var userEntity = await _context.Users.FindAsync(id);
                if (userEntity != null)
                    return new OkObjectResult(new UserResponse
                    {
                        Id = userEntity.Id,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        Email = userEntity.Email,
                        PhoneNumber = userEntity.PhoneNumber
                    });
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }



    }
}
