using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
         public AuctionsController(AuctionDbContext context,IMapper mapper)
         {
            _context = context;
            _mapper = mapper;
         }

         [HttpGet]
         public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
         {
            var auctions = await _context.Auctions
                    .Include( x => x.Item).OrderBy(x => x.Item.Make).ToListAsync();

           return _mapper.Map<List<AuctionDto>>(auctions);
         }

         [HttpGet("{id}")]
         public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
         {
            var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

            if(auction == null)
                return NotFound();

            return _mapper.Map<AuctionDto>(auction);
         }

         [HttpPost]
         public async Task<ActionResult<AuctionDto>> CreateAuction (CreateAuctionDto acutionDto)
         {
            var auction = _mapper.Map<Auction>(acutionDto);
            auction.Seller ="test";
            _context.Auctions.Add(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if(!result)
                return BadRequest("Error While Save Changes to DB");

            return CreatedAtAction(nameof(GetAuctionById),
                     new {auction.Id},_mapper.Map<AuctionDto>(auction));    
         }

         [HttpPut("{id}")]
         public async Task<ActionResult> UpdateAuction(Guid id,UpdateAuctionDto updateAuctionDto)
         {
            var acution = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

            if(acution == null)
                return NotFound();

            acution.Item.Make = updateAuctionDto.Make ?? acution.Item.Make;   
            acution.Item.Model = updateAuctionDto.Model ?? acution.Item.Model;   
            acution.Item.Color = updateAuctionDto.Color ?? acution.Item.Color;   
            acution.Item.Mileage = updateAuctionDto.Mileage ?? acution.Item.Mileage;   
            acution.Item.Year = updateAuctionDto.Year ?? acution.Item.Year; 

            var result = await _context.SaveChangesAsync() > 0;

            if(result)
                return Ok();

            return BadRequest("Problem Saving Changes"); 
         }

         [HttpDelete("{id}")]
         public async Task<ActionResult> DeleteAuction(Guid id)
         {
            var auction = await _context.Auctions.FindAsync(id);

            if(auction == null)
                return NotFound(); 

            _context.Auctions.Remove(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if(result)
                return Ok();

            return BadRequest("Problem Saving Changes"); 
         } 
    }
}