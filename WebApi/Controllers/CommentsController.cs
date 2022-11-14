using Microsoft.AspNetCore.Http;
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
    public class CommentsController : ControllerBase
    {
        private readonly DataContext _context;

        public CommentsController(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CommentRequest req)
        {
            try
            {
                var commentEntity = new CommentEntity
                {
                    Comment = req.Comment,
                    Created = DateTime.Now,
                    IssueId = req.IssueId,
                    UserId = req.UserId
                };
                _context.Add(commentEntity);
                await _context.SaveChangesAsync();
            
                return new OkObjectResult(commentEntity);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }
    }
}
