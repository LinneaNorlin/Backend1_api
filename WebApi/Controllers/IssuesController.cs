using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml.Linq;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly DataContext _context;

        public IssuesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(IssueRequest req)
        {
            try
            {
                var datetime = DateTime.Now;
                var issueEntity = new IssueEntity
                {
                    Subject = req.Subject,
                    Description = req.Description,
                    UserId = req.UserId,
                    Created = datetime,
                    Updated = datetime,
                    StatusId = 1
                };
                _context.Add(issueEntity);
                await _context.SaveChangesAsync();

                return new OkObjectResult(issueEntity);

            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var issues = new List<IssueResponse>();
                foreach (var issueEntity in await _context.Issues.Include(x => x.Status).Include(x => x.User).ToListAsync())
                    issues.Add(new IssueResponse
                    {
                        Id = issueEntity.Id,
                        Subject = issueEntity.Subject,
                        Description = issueEntity.Description,
                        Created = issueEntity.Created,
                        Updated = issueEntity.Updated,
                        Status = new StatusResponse
                        {
                            Id = issueEntity.Status.Id,
                            Status = issueEntity.Status.Status
                        },
                        User = new UserResponse
                        {
                            Id = issueEntity.User.Id,
                            FirstName = issueEntity.User.FirstName,
                            LastName = issueEntity.User.LastName,
                            Email = issueEntity.User.Email,
                            PhoneNumber = issueEntity.User.PhoneNumber
                        }
                    });

                return new OkObjectResult(issues);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var issueEntity = await _context.Issues.Include(x => x.Status).Include(x => x.User).Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
                if (issueEntity != null)
                {
                    var comments = new List<CommentResponse>();
                    foreach (var comment in issueEntity.Comments)
                        comments.Add(new CommentResponse
                        {
                            Id = comment.Id,
                            Comment = comment.Comment,
                            Created = comment.Created,
                            UserId = comment.UserId
                        });

                    return new OkObjectResult(new IssueResponse
                    {
                        Id = issueEntity.Id,
                        Subject = issueEntity.Subject,
                        Description = issueEntity.Description,
                        Created = issueEntity.Created,
                        Updated = issueEntity.Updated,
                        Status = new StatusResponse
                        {
                            Id = issueEntity.Status.Id,
                            Status = issueEntity.Status.Status
                        },
                        User = new UserResponse
                        {
                            Id = issueEntity.User.Id,
                            FirstName = issueEntity.User.FirstName,
                            LastName = issueEntity.User.LastName,
                            Email = issueEntity.User.Email,
                            PhoneNumber = issueEntity.User.PhoneNumber
                        },
                        Comments = comments
                    });
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IssueUpdateRequest req)
        {
            try
            {
                var _issueEntity = await _context.Issues.FindAsync(id);
                _issueEntity.StatusId = req.StatusId;


                _context.Entry(_issueEntity).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                var issueEntity = await _context.Issues.Include(x => x.Status).Include(x => x.User).Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
                if (issueEntity != null) { 
                
                    var comments = new List<CommentResponse>();

                    foreach (var comment in issueEntity.Comments)
                    comments.Add(new CommentResponse
                    {
                        Id = comment.Id,
                        Comment = comment.Comment,
                        Created = comment.Created,
                        UserId = comment.UserId
                    });

                return new OkObjectResult(new IssueResponse
                    {
                        Id = issueEntity.Id,
                        Subject = issueEntity.Subject,
                        Description = issueEntity.Description,
                        Created = issueEntity.Created,
                        Updated = issueEntity.Updated,
                        Status = new StatusResponse
                        {
                            Id = issueEntity.Status.Id,
                            Status = issueEntity.Status.Status
                        },
                        User = new UserResponse
                        {
                            Id = issueEntity.User.Id,
                            FirstName = issueEntity.User.FirstName,
                            LastName = issueEntity.User.LastName,
                            Email = issueEntity.User.Email,
                            PhoneNumber = issueEntity.User.PhoneNumber
                        },
                        Comments = comments
                    });
                }

            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

    }
}
