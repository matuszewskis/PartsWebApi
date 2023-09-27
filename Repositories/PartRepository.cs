using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PartsWebApi.Dtos;
using PartsWebApi.Entities;
using PartsWebApi.Utils.Exceptions;
using PartsWebApi.Utils.Helpers;

namespace PartsWebApi.Repositories
{
    public interface IPartRepository
    {
        Task<IEnumerable<PartDto>> GetAll();

        Task<string> GetDestination(Guid partId);

        Task AddPart(PartDto partDto);

        Task UpdateDestination(Guid partId);

        Task RemovePart(Guid partId);
    }

    public class PartRepository : IPartRepository
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        private readonly IPartHelper _partHelper;


        public PartRepository(ApiContext context, IMapper mapper, IPartHelper partHelper)
        {
            _context = context;
            _mapper = mapper;
            _partHelper = partHelper;
        }

        public async Task<IEnumerable<PartDto>> GetAll()
        {
            var entities = await _context.Parts
                .Include(p => p.Destination)
                .ToListAsync();
            return entities.Select(e => _mapper.Map<PartDto>(e));
        }

        public async Task<string> GetDestination(Guid partId)
        {
            var entity = await _context.Parts
                .Include(p => p.Destination)
                .FirstOrDefaultAsync(p => p.Id == partId);

            return entity?.Destination?.DestinationTypeId ??
                   throw new EntityNotFoundException("Part with provided Id does not exist in database.");
        }

        public async Task AddPart(PartDto dto)
        {
            await _partHelper.CheckAddingPartRequirements(dto);

            var destination = await _partHelper.GetProvidedDestination(dto);

            if (destination != null)
            {
                // Insert the record into the database
                var entity = new Part() { Component = dto.Component, Name = dto.Name, Destination = destination};
                await _context.Parts.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Provided Destination Code is not assigned to any DestinationType", nameof(dto.Destination));
            }
        }

        public async Task UpdateDestination(Guid partId)
        {
            var entity = await _context.Parts
                .Include(p => p.Destination)
                .FirstOrDefaultAsync(p => p.Id == partId);

            if (entity != null)
            {
                var destinations = await _context.DestinationTypes
                    .OrderByDescending(d => d.Order)
                    .ToListAsync();

                if (destinations.FirstOrDefault()?.DestinationTypeId == entity.Destination?.DestinationTypeId)
                {
                    throw new ActionRequirementsNotFulfilledException("Operation failed. Given record has already its final destination.");
                }

                entity.Destination = destinations.Find(d => d.Order == entity.Destination.Order + 1);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundException("Record with provided Id does not exist in database.");
            }
        }

        public async Task RemovePart(Guid partId)
        {
            var entity = await _context.Parts.FindAsync(partId);

            if (entity != null)
            {
                _context.Parts.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundException("Record with provided Id does not exist in database.");
            }
        }
    }
}
