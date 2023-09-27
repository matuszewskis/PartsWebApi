using Microsoft.EntityFrameworkCore;
using PartsWebApi.Dtos;
using PartsWebApi.Entities;
using PartsWebApi.Utils.Exceptions;

namespace PartsWebApi.Utils.Helpers
{
    public interface IPartHelper
    {
        Task CheckAddingPartRequirements(PartDto partDto);

        Task<DestinationType> GetProvidedDestination(PartDto partDto);
    }

    public class PartHelper : IPartHelper
    {
        public const int MAXIMUM_RECORDS_NUMBER = 20;
        public const int MAXIMUM_P1_DESTINATION_RECORDS_NUMBER = 10;

        public readonly ApiContext _context;

        public PartHelper(ApiContext context)
        {
            _context = context;
        }

        public async Task<DestinationType> GetProvidedDestination(PartDto dto)
        {
            var destination = await _context.DestinationTypes.FirstOrDefaultAsync(dt => dt.DestinationTypeId == dto.Destination);
            return destination ??
                   throw new ActionRequirementsNotFulfilledException("Provided Destination Code does not exist in database.");
        }

        public async Task CheckAddingPartRequirements(PartDto partDto)
        {
            var entities = await _context.Parts
                .Include(p => p.Destination)
                .ToListAsync();

            if (HasMaximumRecords(entities))
            {
                throw new ActionRequirementsNotFulfilledException("Maximum number of records in Parts table reached.");
            }
            else if (partDto.Destination == "P1" && HasMaximumRecordsWithP1Destination(entities))
            {
                throw new ActionRequirementsNotFulfilledException("Maximum number of records for Destination \"P1\" reached.");
            }
            else if (!IsNewDtoUnique(entities, partDto))
            {
                throw new ActionRequirementsNotFulfilledException("Record with provided data already exists in the database.");
            }
        }

        private static bool HasMaximumRecords(IEnumerable<Part> entities)
        {
            return entities.Count() >= MAXIMUM_RECORDS_NUMBER;
        }

        private static bool IsNewDtoUnique(IEnumerable<Part> entities, PartDto partDto)
        {
            return !entities.Any(e => e.Component == partDto.Component && e.Name == partDto.Name &&
                                      e.Destination?.DestinationTypeId == partDto.Destination);
        }

        private static bool HasMaximumRecordsWithP1Destination(IEnumerable<Part> entities)
        {
            return entities.Count(e => e.Destination?.DestinationTypeId == "P1") >= MAXIMUM_P1_DESTINATION_RECORDS_NUMBER;
        }
    }
}
